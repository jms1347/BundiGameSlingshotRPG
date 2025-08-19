// CommandInvoker.cs (������ �κ�)
using System.Collections.Generic;
using UnityEngine;

public class CommandInvoker : Singleton<CommandInvoker>
{
    // ���� ������ �״�� ����
    private Stack<IUndoableCommand> _commandHistory = new Stack<IUndoableCommand>();
    private Stack<IUndoableCommand> _redoHistory = new Stack<IUndoableCommand>();

    // !!! �ν����� Ȯ���� ���� �ӽ� ����Ʈ �߰� !!!
    // Stack�� ������ ToList()�� ��ȯ�Ͽ� ���⿡ ���� �̴ϴ�.
    [SerializeField] private List<string> _debugCommandHistory = new List<string>();
    [SerializeField] private List<string> _debugRedoHistory = new List<string>();


    // Stack�� ������ Debug List�� ������Ʈ�ϴ� �޼���
    private void UpdateDebugLists()
    {
        _debugCommandHistory.Clear();
        foreach (var cmd in _commandHistory)
        {
            _debugCommandHistory.Add(cmd.GetType().Name); // ����� Ÿ�� �̸��� ǥ��
        }
        // ������ LIFO�̹Ƿ�, �ν����Ϳ��� ������ �� �� �������� ���� �� �ֽ��ϴ�.
        // �ʿ��ϴٸ� _debugCommandHistory.Reverse(); �� �߰��Ͽ� ���� Ǫ�� ������� �� �� �ֽ��ϴ�.

        _debugRedoHistory.Clear();
        foreach (var cmd in _redoHistory)
        {
            _debugRedoHistory.Add(cmd.GetType().Name);
        }
        // _debugRedoHistory.Reverse();
    }

    public void ExecuteCommand(IUndoableCommand command)
    {
        command.Execute();
        _commandHistory.Push(command);
        _redoHistory.Clear();
        Debug.Log($"[CommandInvoker] ��� ����: {command.GetType().Name}. ���� ��� ��: {_commandHistory.Count}");
        UpdateDebugLists(); // ��� ���� �� ����� ����Ʈ ������Ʈ
    }

    public void UndoCommand()
    {
        if (_commandHistory.Count > 0)
        {
            IUndoableCommand command = _commandHistory.Pop();
            command.Undo();
            _redoHistory.Push(command);
            Debug.Log($"[CommandInvoker] ��� Undo: {command.GetType().Name}. ���� ��� ��: {_commandHistory.Count}");
            UpdateDebugLists(); // Undo �� ����� ����Ʈ ������Ʈ
        }
        else
        {
            Debug.Log("[CommandInvoker] �� �̻� �ǵ��� ����� �����ϴ�.");
        }
    }

    public void RedoCommand()
    {
        if (_redoHistory.Count > 0)
        {
            IUndoableCommand command = _redoHistory.Pop();
            command.Redo();
            _commandHistory.Push(command);
            Debug.Log($"[CommandInvoker] ��� Redo: {command.GetType().Name}. ���� ��� ��: {_commandHistory.Count}");
            UpdateDebugLists(); // Redo �� ����� ����Ʈ ������Ʈ
        }
        else
        {
            Debug.Log("[CommandInvoker] �� �̻� �ٽ� ������ ����� �����ϴ�.");
        }
    }
}