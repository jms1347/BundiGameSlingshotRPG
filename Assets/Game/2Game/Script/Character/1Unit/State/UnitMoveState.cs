using UnityEngine;

public class UnitMoveState : MonoBehaviour, IState<Unit>
{
    Unit unit;
    public void Handle(Unit context)
    {
        unit = context;
    }
    public void Action()
    {
        if (unit == null || unit.Target == null)
        {
            // Ÿ���� ������ �̵��� �ʿ䰡 �����Ƿ� Idle ���·� ���ư����� StateManager�� �˸��ϴ�.
            // UnitStateManager���� Ÿ�� ������ üũ�ϰ� ���� ��ȯ�� ó���ϴ� ���� �� �Ϲ����Դϴ�.
            // ������ ���� ��ü ������ ��� Ż�� ������ ���� ���� �ֽ��ϴ�.
            unit.UnitStateManager?.ChangeState(UnitState.IDLE);
            return;
        }

        Vector3 direction = (unit.Target.position - unit.transform.position).normalized;
        Vector3 moveAmount = direction * unit.Stats.MoveSpeed * Time.deltaTime;

        unit.transform.position += moveAmount;

        if (direction.x != 0)
        {
            SpriteRenderer spriteRenderer = unit.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                // ���⿡ ���� ��������Ʈ�� �������ϴ�.
                spriteRenderer.flipX = direction.x < 0; // �������� ���� flipX=true (������ �ٶ�)
            }
        }
    }

    public void Enter()
    {
        if (unit == null) return;
        unit.Animator?.SetBool("1_Move", true);


    }

    public void Exit()
    {
        if (unit == null) return;

        unit.Animator?.SetBool("1_Move", false);

    }
}