using UnityEngine;

public class Command_MouseLeftBtn : IMouseCommand
{

    private PlayerMouseInputState _mouseInputState;

    public Command_MouseLeftBtn(PlayerMouseInputState state)
    {
        _mouseInputState = state;
    }
    public void MouseDownExecute()
    {
        _mouseInputState.IsUpPressed = false;
        _mouseInputState.IsDownPressed = true;
    }

    public void MouseExecute()
    {
        _mouseInputState.ISPressed = true;

    }

    public void MouseUpExcute()
    {
        _mouseInputState.IsDownPressed = false;
        _mouseInputState.ISPressed = false;
        _mouseInputState.IsUpPressed = true;
    }
}
