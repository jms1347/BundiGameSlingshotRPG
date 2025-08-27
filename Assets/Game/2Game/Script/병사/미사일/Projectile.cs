using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Stats")]
    public float projectileSpeed = 10f; // 투사체 이동 속도
    public float pierceCount = 1;       // 관통 계수
    public float lifetime = 5f;         // 투사체 생명 주기 (일정 시간 후 자동 소멸)

    private Vector2 direction;
    private int damage;
    private GameObject owner;
    private int currentPiercedCount = 0; // 현재 관통한 적의 수

    public void Initialize(Vector2 dir, int dmg, GameObject ownerObj)
    {
        direction = dir;
        damage = dmg;
        owner = ownerObj;

        // 특정 시간 후 자동 소멸
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // 투사체 이동
        transform.position += (Vector3)direction * projectileSpeed * Time.deltaTime;

        // 거리에 따른 데미지 및 스케일 감소 로직
        float distance = Vector2.Distance(owner.transform.position, transform.position);

        // 투사체 크기 조절 (가까이서 쏘면 커지고, 멀어지면 작아짐)
        // Lerp 함수를 사용하여 부드러운 스케일 변화 구현
        float scaleFactor = 1f;
        if (distance > owner.GetComponent<Archer>().minDamageScaleDistance)
        {
            float t = Mathf.InverseLerp(owner.GetComponent<Archer>().minDamageScaleDistance, owner.GetComponent<Archer>().maxDamageScaleDistance, distance);
            scaleFactor = Mathf.Lerp(1f, 0.5f, t); // 1.0f에서 0.5f까지 크기 감소
        }
        transform.localScale = Vector3.one * scaleFactor;

        // 데미지 계산 (투사체 스케일 비율만큼 데미지 감소)
        // damage = (int)(originalDamage * scaleFactor); // 이 방식은 Update에서 매번 데미지를 변경해야 해서 효율적이지 않음
        // 충돌 시에 스케일을 기반으로 데미지를 계산하는 것이 더 효율적입니다.
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌한 오브젝트가 적 태그를 가졌는지 확인
        if (other.CompareTag(owner.GetComponent<SoldierBase>().enemyTag))
        {
            // 관통 계수 확인
            if (currentPiercedCount < pierceCount)
            {
                // 데미지 계산
                int finalDamage = (int)(damage * transform.localScale.x);

                // 적에게 데미지 전달
                SoldierBase enemy = other.GetComponent<SoldierBase>();
                if (enemy != null)
                {
                    enemy.TakeDamage(finalDamage);
                    currentPiercedCount++;
                    Debug.Log($"Projectile hit {other.name}. Pierced count: {currentPiercedCount}/{pierceCount}.");
                }
            }

            // 관통 횟수를 모두 소진했거나, 관통 계수가 0이면 투사체 비활성화
            if (currentPiercedCount >= pierceCount)
            {
                gameObject.SetActive(false); // 비활성화
                Destroy(gameObject, 0.5f); // 비활성화 후 일정 시간 뒤 파괴 (메모리 관리)
            }
        }
    }
}