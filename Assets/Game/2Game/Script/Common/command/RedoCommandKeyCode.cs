// Redo를 위한 ICommand 구현체
using UnityEngine;

public class RedoCommandKeyCode : ICommand
{
    public void KeyDownExecute() { 
        CommandInvoker.Instance.RedoCommand(); 
        Debug.Log("[RedoCommandKeyCode] Y키 눌림: Redo 요청."); 
    }
    public void KeyExecute() { }
    public void KeyUpExecute() { }
}