// Commands/MoveUndoableCommand.cs
using UnityEngine;

public class MoveUndoableCommand : IUndoableCommand
{
    private Transform _targetTransform;
    private Vector3 _startPosition; // ����� ���۵� �� (�̵� ����)�� ��ġ
    private Vector3 _endPosition;   // ����� �Ϸ�� �� (�̵� ����)�� ��ġ

    public MoveUndoableCommand(Transform target)
    {
        _targetTransform = target;
        _startPosition = target.position; // �̵� ���� ���� ���
        Debug.Log($"[MoveUndoableCommand] ����: ���� ��ġ {_startPosition}");
    }

    public void Execute()
    {
        // ���� �̵��� PlayerController�� FixedUpdate���� �̹� �߻��߽��ϴ�.
        // ���⼭�� �̵��� ���� ���(�� ��ġ)�� Ȯ���ϰ� ����ϴ� ���Ҹ� �մϴ�.
        _endPosition = _targetTransform.position; // ��� ���� ������ ���� ��ġ
        Debug.Log($"[MoveUndoableCommand] ����: ���� ��ġ {_endPosition} ��� �Ϸ�.");
    }

    public void Undo()
    {
        _targetTransform.position = _startPosition;
        Debug.Log($"[MoveUndoableCommand] Undo: ��ġ {_endPosition} -> {_startPosition} (�ǵ���).");
    }

    public void Redo()
    {
        _targetTransform.position = _endPosition;
        Debug.Log($"[MoveUndoableCommand] Redo: ��ġ {_startPosition} -> {_endPosition} (�ٽ� ����).");
    }
}