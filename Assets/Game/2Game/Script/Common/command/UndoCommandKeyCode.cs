// Undo�� ���� ICommand ����ü
using UnityEngine;

public class UndoCommandKeyCode : ICommand
{
    public void KeyDownExecute() { 
        CommandInvoker.Instance.UndoCommand();
        Debug.Log("[UndoCommandKeyCode] ZŰ ����: Undo ��û."); 
    }
    public void KeyExecute() { }
    public void KeyUpExecute() { }
}