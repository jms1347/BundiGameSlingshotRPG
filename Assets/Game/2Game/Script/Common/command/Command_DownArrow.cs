using UnityEngine;

public class Command_DownArrow : ICommand
{
    private PlayerMovementState _movementState;

    public Command_DownArrow(PlayerMovementState state)
    {
        _movementState = state;
    }

    public void KeyDownExecute() { _movementState.IsDownPressed = true; Debug.Log("[Command_DownArrow] Down Arrow KeyDown."); }
    public void KeyExecute() { }
    public void KeyUpExecute() { _movementState.IsDownPressed = false; Debug.Log("[Command_DownArrow] Down Arrow KeyUp."); }
}