// PlayerController.cs
using UnityEngine;

public class PlayerController : Character
{
    public float moveSpeed = 5f;

    private Transform _playerTransform;
    private Rigidbody _rigidbody;

    private PlayerMovementState _movementState; // 플레이어의 현재 이동 입력 상태
    private PlayerMouseInputState _mouseInputState; // 플레이어의 현재 이동 입력 상태
    private MoveUndoableCommand _currentMoveUndoableCommand; // 현재 진행 중인 이동 UndoableCommand
    private bool _isCurrentlyMoving = false; // 현재 실제로 이동 중인지 (물리적으로)

    [Header("UI컨트롤러")]
    [SerializeField] private CharacterUIHandler playerUIHandler; // 인스펙터에서 UI 핸들러 할당

    protected override void Awake()
    {
        base.Awake();

        _playerTransform = transform;
        _rigidbody = GetComponent<Rigidbody>();

        if (InputManager.Instance == null)
        {
            Debug.LogError("InputManager가 씬에 없습니다! GameObject에 InputManager 스크립트를 추가해주세요.");
            enabled = false;
            return;
        }
        if (CommandInvoker.Instance == null)
        {
            Debug.LogError("CommandInvoker가 씬에 없습니다! GameObject에 CommandInvoker 스크립트를 추가해주세요.");
            enabled = false;
            return;
        }
        if (_rigidbody == null)
        {
            Debug.LogWarning("Rigidbody 컴포넌트가 없습니다. 물리 기반 이동이 불안정할 수 있습니다. 추가를 권장합니다.");
        }
        if (_animator == null)
        {
            Debug.LogWarning("Animator 컴포넌트가 없습니다. 스킬 애니메이션이 작동하지 않을 수 있습니다.");
        }

        _movementState = new PlayerMovementState(); // 이동 상태 클래스 인스턴스화
        _mouseInputState = new PlayerMouseInputState(); // 이동 상태 클래스 인스턴스화

        // --- InputManager에 방향키 명령 등록 ---        
        // 이 명령들은 PlayerMovementState를 업데이트하는 역할을 합니다.
        InputManager.Instance.RegisterCommand(KeyCode.UpArrow, new Command_UpArrow(_movementState));
        InputManager.Instance.RegisterCommand(KeyCode.DownArrow, new Command_DownArrow(_movementState));
        InputManager.Instance.RegisterCommand(KeyCode.LeftArrow, new Command_LeftArrow(_movementState));
        InputManager.Instance.RegisterCommand(KeyCode.RightArrow, new Command_RightArrow(_movementState));

        // 마우스 관련 명령 등록
        InputManager.Instance.RegisterMouseCommand(0, new Command_MouseLeftBtn(_mouseInputState));


        // --- InputManager에 스킬 명령 등록 ---
        // PlayerController에서 GameObject와 Animator를 넘겨줍니다.
        //InputManager.Instance.RegisterCommand(KeyCode.Q, new SkillCommand(this, _animator));
        //InputManager.Instance.RegisterCommand(KeyCode.W, new SkillCommand(this, _animator));
        //InputManager.Instance.RegisterCommand(KeyCode.E, new SkillCommand(this, _animator));
        //InputManager.Instance.RegisterCommand(KeyCode.R, new SkillCommand(this, this.GetComponent<RoomSkill>()));
        // 초기에는 룸 스킬을 R 키에 할당합니다.




        // --- Undo/Redo 명령 등록 ---
        InputManager.Instance.RegisterCommand(KeyCode.Z, new UndoCommandKeyCode());
        InputManager.Instance.RegisterCommand(KeyCode.Y, new RedoCommandKeyCode());
    }
    void Start()
    {
        TakeDamage(50);
    }

    void FixedUpdate()
    {
        // PlayerMovementState를 통해 현재 눌려있는 모든 이동 키의 상태를 종합합니다.
        Vector3 currentMoveDirection = _movementState.GetCurrentMoveDirection();

        // --- 이동 시작/종료 감지 및 UndoableCommand 처리 ---
        if (_movementState.IsAnyMoveKeyHolding())
        {
            if (!_isCurrentlyMoving) // 이동이 이제 막 시작될 때
            {
                _isCurrentlyMoving = true;
                // 새로운 이동 UndoableCommand를 시작합니다.
                _currentMoveUndoableCommand = new MoveUndoableCommand(_playerTransform);
                Debug.Log("[PlayerController] 실제 이동 시작 감지.");
            }

            // 실제 물리적인 이동 처리
            if (_rigidbody != null)
            {
                _rigidbody.MovePosition(_rigidbody.position + currentMoveDirection * moveSpeed * Time.fixedDeltaTime);
            }
            else
            {
                _playerTransform.position += currentMoveDirection * moveSpeed * Time.fixedDeltaTime;
            }
        }
        else // 어떤 이동 키도 눌려있지 않을 때 (이동 종료)
        {
            if (_isCurrentlyMoving) // 이동이 방금 종료되었을 때
            {
                _isCurrentlyMoving = false;
                if (_currentMoveUndoableCommand != null)
                {
                    // 이동이 끝났으므로 최종 위치를 기록하고 CommandInvoker에 푸시합니다.
                    CommandInvoker.Instance.ExecuteCommand(_currentMoveUndoableCommand);
                    _currentMoveUndoableCommand = null;
                    Debug.Log("[PlayerController] 실제 이동 종료 감지 및 MoveUndoableCommand 실행.");
                }
            }
        }

        //마우스 이동
        Vector3 currentMouseMoveDirection = _mouseInputState.GetCurrentMousePosition();

        Vector3 directionToTarget = (currentMouseMoveDirection - _playerTransform.position);
        directionToTarget.y = 0; // Y축 차이 무시 (수평 이동)

        float distanceToTarget = directionToTarget.magnitude;

        if (distanceToTarget > 0.1f)
        {
            // 목표 방향으로 회전
            RotateToward(directionToTarget.normalized);
            // 목표 방향으로 이동
            PerformMovement(directionToTarget.normalized);
        }
    }

    private void PerformMovement(Vector3 direction)
    {
        if (_rigidbody != null)
        {
            _rigidbody.MovePosition(_rigidbody.position + direction * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            _playerTransform.position += direction * moveSpeed * Time.fixedDeltaTime;
        }
    }

    // 특정 방향으로 플레이어를 부드럽게 회전시키는 헬퍼 함수
    private void RotateToward(Vector3 targetDirection)
    {
        // Y축 고정을 위해 targetDirection의 Y값은 0으로 설정
        targetDirection.y = 0;
        if (targetDirection == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        _playerTransform.rotation = Quaternion.Slerp(_playerTransform.rotation, targetRotation, 10f * Time.fixedDeltaTime);
    }
}
