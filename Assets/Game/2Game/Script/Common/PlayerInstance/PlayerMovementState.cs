// PlayerMovementState.cs
using UnityEngine;

// �� Ŭ������ MonoBehaviour�� �ƴϹǷ� ���� ������Ʈ�� �߰��� �ʿ� �����ϴ�.
// PlayerController ������ �ν��Ͻ�ȭ�˴ϴ�.
public class PlayerMovementState
{
    public bool IsUpPressed { get; set; }
    public bool IsDownPressed { get; set; }
    public bool IsLeftPressed { get; set; }
    public bool IsRightPressed { get; set; }

    /// <summary>
    /// ���� ���� �ִ� ����Ű���� ������� �������� �̵� ���� ���͸� ����մϴ�.
    /// </summary>
    public Vector3 GetCurrentMoveDirection()
    {
        Vector3 direction = Vector3.zero;
        if (IsUpPressed) direction += Vector3.forward;
        if (IsDownPressed) direction += Vector3.back;
        if (IsLeftPressed) direction += Vector3.left;
        if (IsRightPressed) direction += Vector3.right;

        // �밢�� �̵� �� �ӵ� ������ ���� ����ȭ
        return direction.normalized;
    }

    /// <summary>
    /// ���� � �̵� Ű�� �����ִ��� Ȯ���մϴ�.
    /// </summary>
    public bool IsAnyMoveKeyHolding()
    {
        return IsUpPressed || IsDownPressed || IsLeftPressed || IsRightPressed;
    }
}