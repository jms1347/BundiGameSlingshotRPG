// InputManager.cs (수정된 부분)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    // 실제 딕셔너리는 그대로 유지
    private Dictionary<KeyCode, ICommand> _commands = new Dictionary<KeyCode, ICommand>();
    private Dictionary<int, IMouseCommand> _mouseCommands = new Dictionary<int, IMouseCommand>();
   
    // Dictionary의 키와 값을 각각의 리스트에 저장하여 표시합니다.
    [SerializeField] private List<KeyCode> _debugCommandKeys = new List<KeyCode>();
    [SerializeField] private List<string> _debugCommandNames = new List<string>();
    // 마우스 커맨드 딕셔너리 확인을 위한 리스트
    [SerializeField] private List<int> _debugCommandMouses = new List<int>();
    [SerializeField] private List<string> _debugCommandMouseNames = new List<string>();


    /// <summary>
    /// Dictionary의 내용을 Debug List로 업데이트하는 메서드
    /// </summary>
    private void UpdateDebugLists()
    {
        _debugCommandKeys.Clear();
        _debugCommandNames.Clear();

        foreach (var kvp in _commands)
        {
            _debugCommandKeys.Add(kvp.Key);
            _debugCommandNames.Add(kvp.Value.GetType().Name); // ICommand 객체의 타입 이름만 표시
        }
    }

    private void UpdateDebugMouseLists()
    {
        _debugCommandMouses.Clear();
        _debugCommandMouseNames.Clear();

        foreach (var kvp in _mouseCommands)
        {
            _debugCommandMouses.Add(kvp.Key);
            _debugCommandMouseNames.Add(kvp.Value.GetType().Name); // ICommand 객체의 타입 이름만 표시
        }
    }
    /// <summary>
    /// 외부에서 키와 해당 키에 할당할 명령(ICommand)을 등록합니다.
    /// </summary>
    public void RegisterCommand(KeyCode key, ICommand command)
    {
        if (_commands.ContainsKey(key))
        {
            Debug.LogWarning($"KeyCode '{key}'에 대한 명령이 이미 등록되어 있습니다. 교체합니다.");
            _commands[key] = command;
        }
        else
        {
            _commands.Add(key, command);
        }
        UpdateDebugLists(); // 명령 등록 후 디버그 리스트 업데이트
    }

    public void RegisterMouseCommand(int keyIndex, IMouseCommand command)
    {
        if (_mouseCommands.ContainsKey(keyIndex))
        {
            Debug.LogWarning($"Mouse '{keyIndex}'에 대한 명령이 이미 등록되어 있습니다. 교체합니다.");
            _mouseCommands[keyIndex] = command;

        }
        else
        {
            _mouseCommands.Add(keyIndex, command);

        }
        UpdateDebugMouseLists(); // 명령 등록 후 디버그 리스트 업데이트
    }

    void Update()
    {
        // 등록된 모든 키에 대해 현재 입력 상태를 확인하고 해당 ICommand 메서드를 호출합니다.
        foreach (var kvp in _commands)
        {
            KeyCode currentKey = kvp.Key;
            ICommand command = kvp.Value;

            if (command == null) continue;

            if (Input.GetKeyDown(currentKey))
            {
                command.KeyDownExecute();
            }

            if (Input.GetKey(currentKey))
            {
                command.KeyExecute();
            }

            if (Input.GetKeyUp(currentKey))
            {
                command.KeyUpExecute();
            }
        }

        foreach(var mvp in _mouseCommands)
        {
            int currentKey = mvp.Key;
            IMouseCommand command = mvp.Value;

            if (command == null) continue;

            if (Input.GetMouseButtonDown(currentKey))
            {
                command.MouseDownExecute();
            }
            if (Input.GetMouseButton(currentKey))
            {
                command.MouseExecute();
            }
            if (Input.GetMouseButtonUp(currentKey))
            {
                command.MouseUpExcute();
            }
        }
    }
}