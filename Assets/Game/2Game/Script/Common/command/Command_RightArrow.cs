// Commands/Command_RightArrow.cs
using UnityEngine;

public class Command_RightArrow : ICommand
{
    private PlayerMovementState _movementState;

    public Command_RightArrow(PlayerMovementState state)
    {
        _movementState = state;
    }

    public void KeyDownExecute() {
        _movementState.IsRightPressed = true; 
        Debug.Log("[Command_RightArrow] Right Arrow KeyDown.");
    }
    public void KeyExecute() { }
    public void KeyUpExecute() { 
        _movementState.IsRightPressed = false; 
        Debug.Log("[Command_RightArrow] Right Arrow KeyUp."); 
    }
}