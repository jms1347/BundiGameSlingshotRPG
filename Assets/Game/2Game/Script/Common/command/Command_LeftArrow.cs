// Commands/Command_LeftArrow.cs
using UnityEngine;

public class Command_LeftArrow : ICommand
{
    private PlayerMovementState _movementState;

    public Command_LeftArrow(PlayerMovementState state)
    {
        _movementState = state;
    }

    public void KeyDownExecute() { _movementState.IsLeftPressed = true; Debug.Log("[Command_LeftArrow] Left Arrow KeyDown."); }
    public void KeyExecute() { }
    public void KeyUpExecute() { _movementState.IsLeftPressed = false; Debug.Log("[Command_LeftArrow] Left Arrow KeyUp."); }
}