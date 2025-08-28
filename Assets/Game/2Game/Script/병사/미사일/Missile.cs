using UnityEngine;

public class Missile : MonoBehaviour
{
    // 미사일의 이동 속도
    [SerializeField] private float moveSpeed = 10f;
    // 미사일의 최대 사정거리
    [SerializeField] private float maxDistance = 10f;

    private Transform target;
    private float damage;
    private Vector3 initialPosition;

    // 미사일을 발사한 유닛의 태그 (자신에게 피해를 주지 않기 위함)
    private string myUnitTag;
    // 미사일 발사 준비 함수
    public void Launch(Transform _target, float _damage, Vector3 startPosition, string _myUnitTag)
    {
        // 미사일의 초기 위치를 발사 위치로 설정
        transform.position = startPosition;
        // 미사일의 목표와 공격력 설정
        target = _target;
        damage = _damage;
        // 발사 시작 위치 저장 (최종 거리 계산용)
        initialPosition = startPosition;
        // 미사일을 발사한 유닛의 태그 저장
        myUnitTag = _myUnitTag;

        // 미사일을 활성화
        gameObject.SetActive(true);
    }

    private void Update()
    {
        // 타겟이 유효하지 않거나, 미사일이 최대 사정거리를 넘어가면 비활성화
        if (target == null || Vector3.Distance(initialPosition, transform.position) >= maxDistance)
        {
            gameObject.SetActive(false);
            return;
        }

        // 타겟을 향해 미사일 이동
        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌한 오브젝트가 자신(미사일)이 발사된 유닛이 아니고,
        // UnitBase 컴포넌트를 가지고 있으며, 다른 태그를 가지고 있는지 확인
        UnitBase otherUnit = other.GetComponent<UnitBase>();
        if (otherUnit != null && !other.CompareTag(myUnitTag))
        {
            // 타겟이 아니더라도 UnitBase를 가진 적이라면 데미지 적용
            otherUnit.TakeDamage(damage);

            // 데미지를 준 후 미사일 비활성화 (한 번만 피해를 주도록)
            gameObject.SetActive(false);
        }
    }
}
