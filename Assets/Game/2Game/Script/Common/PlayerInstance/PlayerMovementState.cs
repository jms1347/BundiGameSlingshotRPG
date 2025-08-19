// PlayerMovementState.cs
using UnityEngine;

// 이 클래스는 MonoBehaviour가 아니므로 씬에 컴포넌트로 추가할 필요 없습니다.
// PlayerController 내에서 인스턴스화됩니다.
public class PlayerMovementState
{
    public bool IsUpPressed { get; set; }
    public bool IsDownPressed { get; set; }
    public bool IsLeftPressed { get; set; }
    public bool IsRightPressed { get; set; }

    /// <summary>
    /// 현재 눌려 있는 방향키들을 기반으로 종합적인 이동 방향 벡터를 계산합니다.
    /// </summary>
    public Vector3 GetCurrentMoveDirection()
    {
        Vector3 direction = Vector3.zero;
        if (IsUpPressed) direction += Vector3.forward;
        if (IsDownPressed) direction += Vector3.back;
        if (IsLeftPressed) direction += Vector3.left;
        if (IsRightPressed) direction += Vector3.right;

        // 대각선 이동 시 속도 유지를 위해 정규화
        return direction.normalized;
    }

    /// <summary>
    /// 현재 어떤 이동 키라도 눌려있는지 확인합니다.
    /// </summary>
    public bool IsAnyMoveKeyHolding()
    {
        return IsUpPressed || IsDownPressed || IsLeftPressed || IsRightPressed;
    }
}