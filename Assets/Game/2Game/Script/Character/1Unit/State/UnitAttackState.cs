using UnityEngine;

public class UnitAttackState : MonoBehaviour, IState<Unit>
{
    Unit unit;
    private float _attackTimer; // ���� ��Ÿ���� ������ Ÿ�̸�

    public void Handle(Unit context)
    {
        unit = context;
    }
    public void Action()
    {
        if (unit == null || unit.Target == null || unit.Target.gameObject == null)
        {
            unit.UnitStateManager?.ChangeState(UnitState.IDLE);
            return;
        }

        // 1. Ÿ�� ���� �ٶ󺸱� (���� ����: 2D ���ӿ��� �ʿ��ϴٸ�)
        // 2D ��������Ʈ�� �¿� ������ ���� Ÿ�� ������ �ٶ󺸰� �մϴ�.
        Vector3 directionToTarget = (unit.Target.position - unit.transform.position).normalized;
        if (directionToTarget.x != 0)
        {
            SpriteRenderer spriteRenderer = unit.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = directionToTarget.x < 0; // �������� ���� flipX=true
            }
        }

        // 2. ���� ���� üũ (�ʿ��)
        // UnitStateManager���� �̹� AttackState�� ��ȯ�ϱ� ���� ������ üũ������,
        // Ȥ�� �̵� �߿� Ÿ���� ���� ������ ����ٸ� ���⼭ �ٽ� üũ�Ͽ� ���¸� ��ȯ�� �� �ֽ��ϴ�.
        if (!unit.IsTargetInAttackRange())
        {
            // Ÿ���� ���� ���� ������ �����ٸ�, �̵� ���·� ��ȯ ��û
            unit.UnitStateManager?.ChangeState(UnitState.MOVE);
            return;
        }

        // 3. ���� ��Ÿ�� ���� �� ���� ���� ����
        _attackTimer -= Time.deltaTime;
        if (_attackTimer <= 0f)
        {
            PerformAttack();
            _attackTimer = unit.Stats.AttackSpeed;
        }
    }

    public void Enter()
    {
        if (unit == null) return;
        //unit.Animator?.SetTrigger("2_Attack");
        _attackTimer = unit.Stats.AttackSpeed;

    }

    public void Exit()
    {
        if (unit == null) return;

    }

    private void PerformAttack()
    {
        if (unit.Target == null || unit.Target.gameObject == null)
        {
            // ������ ����� ������ �������� �ʽ��ϴ�. (�̹� Action���� üũ������, ��� �ڵ�)
            unit.UnitStateManager?.ChangeState(UnitState.IDLE);
            return;
        }

        // ���� �ִϸ��̼� ��� (���� �������� ����Ǵ� ���)
        //unit.Animator?.SetTrigger("2_Attack");//

        // Ÿ���� Unit ������Ʈ�� ������ TakeDamage �޼��带 ȣ���մϴ�.
        Unit targetUnit = unit.Target.GetComponent<Unit>();
        if (targetUnit != null)
        {
            float damage = unit.Stats.AttackPower; // ������ ���ݷ����� ���ط� ����
            targetUnit.TakeDamage(damage); // ��󿡰� ���� ����

            // ���� ����Ʈ�� ���� ��� (����)
            // AudioManager.Instance.PlaySFX("AttackSound");
            // EffectManager.Instance.SpawnEffect("AttackImpact", _unitContext.Target.position);
        }
        else
        {
            Debug.LogWarning($"Target { unit.Target.name} does not have a Unit component to take damage.");
        }
    }
}