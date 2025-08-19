// Undo를 위한 ICommand 구현체
using UnityEngine;

public class UndoCommandKeyCode : ICommand
{
    public void KeyDownExecute() { 
        CommandInvoker.Instance.UndoCommand();
        Debug.Log("[UndoCommandKeyCode] Z키 눌림: Undo 요청."); 
    }
    public void KeyExecute() { }
    public void KeyUpExecute() { }
}