using UnityEngine;

public class UnitIdleState : MonoBehaviour, IState<Unit>
{
    Unit unit;
    public void Action()
    {
        if (unit == null) return; // ������ ����

        unit.FindNearestTarget(unit.SearchTag);
    }

    public void Enter()
    {

    }

    public void Exit()
    {
    }

    public void Handle(Unit context)
    {
        unit = context;
    }
}
