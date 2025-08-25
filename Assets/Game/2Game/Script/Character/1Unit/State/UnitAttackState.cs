using UnityEngine;

public class UnitAttackState : MonoBehaviour, IState<Unit>
{
    Unit unit;
    private float _attackTimer; // 공격 쿨타임을 관리할 타이머

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

        // 1. 타겟 방향 바라보기 (선택 사항: 2D 게임에서 필요하다면)
        // 2D 스프라이트의 좌우 반전을 통해 타겟 방향을 바라보게 합니다.
        Vector3 directionToTarget = (unit.Target.position - unit.transform.position).normalized;
        if (directionToTarget.x != 0)
        {
            SpriteRenderer spriteRenderer = unit.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = directionToTarget.x < 0; // 왼쪽으로 가면 flipX=true
            }
        }

        // 2. 공격 범위 체크 (필요시)
        // UnitStateManager에서 이미 AttackState로 전환하기 전에 범위를 체크하지만,
        // 혹시 이동 중에 타겟이 범위 밖으로 벗어난다면 여기서 다시 체크하여 상태를 전환할 수 있습니다.
        if (!unit.IsTargetInAttackRange())
        {
            // 타겟이 공격 범위 밖으로 나갔다면, 이동 상태로 전환 요청
            unit.UnitStateManager?.ChangeState(UnitState.MOVE);
            return;
        }

        // 3. 공격 쿨타임 관리 및 실제 공격 실행
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
            // 공격할 대상이 없으면 공격하지 않습니다. (이미 Action에서 체크했지만, 방어 코드)
            unit.UnitStateManager?.ChangeState(UnitState.IDLE);
            return;
        }

        // 공격 애니메이션 재생 (공격 순간에만 재생되는 경우)
        //unit.Animator?.SetTrigger("2_Attack");//

        // 타겟의 Unit 컴포넌트를 가져와 TakeDamage 메서드를 호출합니다.
        Unit targetUnit = unit.Target.GetComponent<Unit>();
        if (targetUnit != null)
        {
            float damage = unit.Stats.AttackPower; // 유닛의 공격력으로 피해량 설정
            targetUnit.TakeDamage(damage); // 대상에게 피해 적용

            // 공격 이펙트나 사운드 재생 (예시)
            // AudioManager.Instance.PlaySFX("AttackSound");
            // EffectManager.Instance.SpawnEffect("AttackImpact", _unitContext.Target.position);
        }
        else
        {
            Debug.LogWarning($"Target { unit.Target.name} does not have a Unit component to take damage.");
        }
    }
}