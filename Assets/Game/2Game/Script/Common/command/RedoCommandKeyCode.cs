// Redo�� ���� ICommand ����ü
using UnityEngine;

public class RedoCommandKeyCode : ICommand
{
    public void KeyDownExecute() { 
        CommandInvoker.Instance.RedoCommand(); 
        Debug.Log("[RedoCommandKeyCode] YŰ ����: Redo ��û."); 
    }
    public void KeyExecute() { }
    public void KeyUpExecute() { }
}