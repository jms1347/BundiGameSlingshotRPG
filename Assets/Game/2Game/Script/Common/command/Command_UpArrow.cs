using UnityEngine;

public class Command_UpArrow : ICommand
{
    private PlayerMovementState _movementState;

    public Command_UpArrow(PlayerMovementState state)
    {
        _movementState = state;
    }

    public void KeyDownExecute()
    {
        _movementState.IsUpPressed = true;
        Debug.Log("[Command_UpArrow] Up Arrow KeyDown.");
    }

    public void KeyExecute()
    {
        // Ű�� �����ִ� ���� ��� ȣ�������, ���⼭�� ���� ������Ʈ �ܿ� �߰� ������ �����ϴ�.
        // ���� �̵��� PlayerController���� ���������� ó���մϴ�.
    }

    public void KeyUpExecute()
    {
        _movementState.IsUpPressed = false;
        Debug.Log("[Command_UpArrow] Up Arrow KeyUp.");
    }
}
