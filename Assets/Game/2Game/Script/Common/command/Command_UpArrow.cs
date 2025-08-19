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
        // 키가 눌려있는 동안 계속 호출되지만, 여기서는 상태 업데이트 외에 추가 로직은 없습니다.
        // 실제 이동은 PlayerController에서 종합적으로 처리합니다.
    }

    public void KeyUpExecute()
    {
        _movementState.IsUpPressed = false;
        Debug.Log("[Command_UpArrow] Up Arrow KeyUp.");
    }
}
