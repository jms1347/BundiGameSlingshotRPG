using UnityEngine;
using System.Collections; // Coroutine�� ���� �ʿ�

public class UnitDeathState : MonoBehaviour, IState<Unit>
{
    private Unit unit; // Unit �ν��Ͻ� ����

    public void Handle(Unit context)
    {
        unit = context;
    }

    // Enter �޼���� �� ���·� ������ �� �� �� ȣ��˴ϴ�.
    public void Enter()
    {
        if (unit == null) return;
        Debug.Log($"{unit.gameObject.name} entered Death State.");

        unit.Animator?.SetTrigger("4_Death");      
    }


    public void Action()
    {
    }


    public void Exit()
    {
        if (unit == null) return;
        Debug.Log($"{unit.gameObject.name} exited Death State. (This might not be called if object is destroyed)");

    }

    
}