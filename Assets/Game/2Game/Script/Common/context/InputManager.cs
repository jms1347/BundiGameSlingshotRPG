// InputManager.cs (������ �κ�)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    // ���� ��ųʸ��� �״�� ����
    private Dictionary<KeyCode, ICommand> _commands = new Dictionary<KeyCode, ICommand>();
    private Dictionary<int, IMouseCommand> _mouseCommands = new Dictionary<int, IMouseCommand>();
   
    // Dictionary�� Ű�� ���� ������ ����Ʈ�� �����Ͽ� ǥ���մϴ�.
    [SerializeField] private List<KeyCode> _debugCommandKeys = new List<KeyCode>();
    [SerializeField] private List<string> _debugCommandNames = new List<string>();
    // ���콺 Ŀ�ǵ� ��ųʸ� Ȯ���� ���� ����Ʈ
    [SerializeField] private List<int> _debugCommandMouses = new List<int>();
    [SerializeField] private List<string> _debugCommandMouseNames = new List<string>();


    /// <summary>
    /// Dictionary�� ������ Debug List�� ������Ʈ�ϴ� �޼���
    /// </summary>
    private void UpdateDebugLists()
    {
        _debugCommandKeys.Clear();
        _debugCommandNames.Clear();

        foreach (var kvp in _commands)
        {
            _debugCommandKeys.Add(kvp.Key);
            _debugCommandNames.Add(kvp.Value.GetType().Name); // ICommand ��ü�� Ÿ�� �̸��� ǥ��
        }
    }

    private void UpdateDebugMouseLists()
    {
        _debugCommandMouses.Clear();
        _debugCommandMouseNames.Clear();

        foreach (var kvp in _mouseCommands)
        {
            _debugCommandMouses.Add(kvp.Key);
            _debugCommandMouseNames.Add(kvp.Value.GetType().Name); // ICommand ��ü�� Ÿ�� �̸��� ǥ��
        }
    }
    /// <summary>
    /// �ܺο��� Ű�� �ش� Ű�� �Ҵ��� ���(ICommand)�� ����մϴ�.
    /// </summary>
    public void RegisterCommand(KeyCode key, ICommand command)
    {
        if (_commands.ContainsKey(key))
        {
            Debug.LogWarning($"KeyCode '{key}'�� ���� ����� �̹� ��ϵǾ� �ֽ��ϴ�. ��ü�մϴ�.");
            _commands[key] = command;
        }
        else
        {
            _commands.Add(key, command);
        }
        UpdateDebugLists(); // ��� ��� �� ����� ����Ʈ ������Ʈ
    }

    public void RegisterMouseCommand(int keyIndex, IMouseCommand command)
    {
        if (_mouseCommands.ContainsKey(keyIndex))
        {
            Debug.LogWarning($"Mouse '{keyIndex}'�� ���� ����� �̹� ��ϵǾ� �ֽ��ϴ�. ��ü�մϴ�.");
            _mouseCommands[keyIndex] = command;

        }
        else
        {
            _mouseCommands.Add(keyIndex, command);

        }
        UpdateDebugMouseLists(); // ��� ��� �� ����� ����Ʈ ������Ʈ
    }

    void Update()
    {
        // ��ϵ� ��� Ű�� ���� ���� �Է� ���¸� Ȯ���ϰ� �ش� ICommand �޼��带 ȣ���մϴ�.
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