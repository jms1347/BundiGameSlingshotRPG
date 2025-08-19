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
            // 타겟이 없으면 이동할 필요가 없으므로 Idle 상태로 돌아가도록 StateManager에 알립니다.
            // UnitStateManager에서 타겟 유무를 체크하고 상태 전환을 처리하는 것이 더 일반적입니다.
            // 하지만 상태 자체 내에서 비상 탈출 로직을 가질 수도 있습니다.
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
                // 방향에 따라 스프라이트를 뒤집습니다.
                spriteRenderer.flipX = direction.x < 0; // 왼쪽으로 가면 flipX=true (왼쪽을 바라봄)
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