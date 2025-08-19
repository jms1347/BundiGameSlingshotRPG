// Commands/MoveUndoableCommand.cs
using UnityEngine;

public class MoveUndoableCommand : IUndoableCommand
{
    private Transform _targetTransform;
    private Vector3 _startPosition; // 명령이 시작될 때 (이동 시작)의 위치
    private Vector3 _endPosition;   // 명령이 완료될 때 (이동 종료)의 위치

    public MoveUndoableCommand(Transform target)
    {
        _targetTransform = target;
        _startPosition = target.position; // 이동 시작 지점 기록
        Debug.Log($"[MoveUndoableCommand] 생성: 시작 위치 {_startPosition}");
    }

    public void Execute()
    {
        // 실제 이동은 PlayerController의 FixedUpdate에서 이미 발생했습니다.
        // 여기서는 이동의 최종 결과(끝 위치)를 확정하고 기록하는 역할만 합니다.
        _endPosition = _targetTransform.position; // 명령 실행 시점의 최종 위치
        Debug.Log($"[MoveUndoableCommand] 실행: 최종 위치 {_endPosition} 기록 완료.");
    }

    public void Undo()
    {
        _targetTransform.position = _startPosition;
        Debug.Log($"[MoveUndoableCommand] Undo: 위치 {_endPosition} -> {_startPosition} (되돌림).");
    }

    public void Redo()
    {
        _targetTransform.position = _endPosition;
        Debug.Log($"[MoveUndoableCommand] Redo: 위치 {_startPosition} -> {_endPosition} (다시 실행).");
    }
}