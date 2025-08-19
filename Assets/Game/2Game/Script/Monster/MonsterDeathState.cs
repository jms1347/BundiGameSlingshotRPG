using System.Collections;
using UnityEngine;



public class MonsterDeathState : MonoBehaviour, IState<Monster>
{
    public Monster monster;
    private IEnumerator actionCour;

    public void Action()
    {
        if (monster == null) return; // 이 상태는 몬스터가 죽었을 때만 호출되므로 !IsDead() 체크는 필요 없음

        if (actionCour != null)
            StopCoroutine(actionCour);
        actionCour = ActionCour();
        StartCoroutine(actionCour);
    }
    private IEnumerator ActionCour()
    {
        // 사망 애니메이션 재생 시간만큼 대기
        yield return Utils.WaitForSecond(3f);
        
        // 사망 애니메이션이 끝나면 오브젝트 비활성화 또는 풀에 반환
        // 예: monster.gameObject.SetActive(false);
        Destroy(monster.gameObject); // 테스트를 위해 바로 파괴

        Debug.Log("DeathRoutine terminated.");
    }

    public void Enter()
    {
        if (monster == null) return; // 이 상태는 몬스터가 죽었을 때만 호출되므로 !IsDead() 체크는 필요 없음

        // 사망 애니메이션 재생
        monster.MonsterAni?.SetTrigger("4_Death");
    }

    public void Exit()
    {
        if (actionCour != null)
            StopCoroutine(actionCour);

    }

    public void Handle(Monster context)
    {
        if (monster == null)
        {
            monster = context;
        }
    }
}
