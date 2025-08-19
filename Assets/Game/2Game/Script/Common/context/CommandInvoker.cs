// CommandInvoker.cs (수정된 부분)
using System.Collections.Generic;
using UnityEngine;

public class CommandInvoker : Singleton<CommandInvoker>
{
    // 실제 스택은 그대로 유지
    private Stack<IUndoableCommand> _commandHistory = new Stack<IUndoableCommand>();
    private Stack<IUndoableCommand> _redoHistory = new Stack<IUndoableCommand>();

    // !!! 인스펙터 확인을 위한 임시 리스트 추가 !!!
    // Stack의 내용을 ToList()로 변환하여 여기에 담을 겁니다.
    [SerializeField] private List<string> _debugCommandHistory = new List<string>();
    [SerializeField] private List<string> _debugRedoHistory = new List<string>();


    // Stack의 내용을 Debug List로 업데이트하는 메서드
    private void UpdateDebugLists()
    {
        _debugCommandHistory.Clear();
        foreach (var cmd in _commandHistory)
        {
            _debugCommandHistory.Add(cmd.GetType().Name); // 명령의 타입 이름만 표시
        }
        // 스택은 LIFO이므로, 인스펙터에서 순서를 볼 때 역순으로 보일 수 있습니다.
        // 필요하다면 _debugCommandHistory.Reverse(); 를 추가하여 스택 푸시 순서대로 볼 수 있습니다.

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
        Debug.Log($"[CommandInvoker] 명령 실행: {command.GetType().Name}. 현재 명령 수: {_commandHistory.Count}");
        UpdateDebugLists(); // 명령 실행 후 디버그 리스트 업데이트
    }

    public void UndoCommand()
    {
        if (_commandHistory.Count > 0)
        {
            IUndoableCommand command = _commandHistory.Pop();
            command.Undo();
            _redoHistory.Push(command);
            Debug.Log($"[CommandInvoker] 명령 Undo: {command.GetType().Name}. 남은 명령 수: {_commandHistory.Count}");
            UpdateDebugLists(); // Undo 후 디버그 리스트 업데이트
        }
        else
        {
            Debug.Log("[CommandInvoker] 더 이상 되돌릴 명령이 없습니다.");
        }
    }

    public void RedoCommand()
    {
        if (_redoHistory.Count > 0)
        {
            IUndoableCommand command = _redoHistory.Pop();
            command.Redo();
            _commandHistory.Push(command);
            Debug.Log($"[CommandInvoker] 명령 Redo: {command.GetType().Name}. 현재 명령 수: {_commandHistory.Count}");
            UpdateDebugLists(); // Redo 후 디버그 리스트 업데이트
        }
        else
        {
            Debug.Log("[CommandInvoker] 더 이상 다시 실행할 명령이 없습니다.");
        }
    }
}