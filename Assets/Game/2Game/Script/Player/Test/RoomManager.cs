using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class RoomManager : MonoBehaviour
{
    public float roomRadius = 10f;
    private SphereCollider roomCollider;
    private List<GameObject> objectsInRoom = new List<GameObject>(); // 룸 안의 모든 오브젝트 (여기에는 플레이어도 포함될 수 있음)
    private List<GameObject> enemiesInRoom = new List<GameObject>(); // 룸 안의 Enemy 태그 오브젝트 (이 리스트만 사용)

    public bool IsRoomActive { get; private set; }

    void Awake()
    {
        roomCollider = GetComponent<SphereCollider>();
        if (roomCollider == null)
        {
            roomCollider = gameObject.AddComponent<SphereCollider>();
        }
        roomCollider.radius = roomRadius;
        roomCollider.isTrigger = true;
    }

    void Update()
    {
        if (transform.localScale.x > 0.01f)
        {
            roomCollider.radius = transform.localScale.x / 2f;
            roomCollider.enabled = true;
        }
        else
        {
            roomCollider.enabled = false;
        }
    }

    public void ActivateRoom()
    {
        IsRoomActive = true;
        gameObject.SetActive(true);
        roomCollider.enabled = true;
        Debug.Log("Room Activated!");
    }

    public void DeactivateRoom()
    {
        IsRoomActive = false;
        gameObject.SetActive(false);
        roomCollider.enabled = false;
        objectsInRoom.Clear();
        enemiesInRoom.Clear();
        StopAllCoroutines();
        Debug.Log("Room Deactivated!");
    }

    void OnTriggerEnter(Collider other)
    {
        // 룸에 진입한 모든 오브젝트를 objectsInRoom에 추가 (플레이어 포함)
        if (!objectsInRoom.Contains(other.gameObject))
        {
            objectsInRoom.Add(other.gameObject);
            Debug.Log(other.name + " entered the room.");

            // **여기서 Enemy 태그만 enemiesInRoom에 추가합니다.**
            if (other.CompareTag("Enemy"))
            {
                enemiesInRoom.Add(other.gameObject);
                Debug.Log("Enemy " + other.name + " entered the room.");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (objectsInRoom.Contains(other.gameObject))
        {
            objectsInRoom.Remove(other.gameObject);
            Debug.Log(other.name + " exited the room.");

            if (enemiesInRoom.Contains(other.gameObject))
            {
                enemiesInRoom.Remove(other.gameObject);
                Debug.Log("Enemy " + other.name + " exited the room.");
            }
        }
    }

    // --- 룸 스킬 관련 함수 ---

    // 1. Q 스킬: 랜덤한 적 1명과 본체 위치 교환 (Shambles)
    public void SwapWithRandomEnemy(Transform playerTransform)
    {
        if (!IsRoomActive)
        {
            Debug.LogWarning("Room is not active. Cannot use Shambles.");
            return;
        }

        if (enemiesInRoom.Count == 0)
        {
            Debug.Log("No enemies in the room to swap with.");
            return;
        }

        int randomIndex = Random.Range(0, enemiesInRoom.Count);
        GameObject targetEnemy = enemiesInRoom[randomIndex];

        if (targetEnemy != null)
        {
            // 플레이어와 적의 Rigidbody를 고려한 위치 교환
            Rigidbody enemyRb = targetEnemy.GetComponent<Rigidbody>();
            Rigidbody playerRb = playerTransform.GetComponent<Rigidbody>();

            Vector3 playerOriginalPos = playerTransform.position;
            Vector3 enemyOriginalPos = targetEnemy.transform.position;

            if (playerRb != null) playerRb.position = enemyOriginalPos;
            else playerTransform.position = enemyOriginalPos;

            if (enemyRb != null) enemyRb.position = playerOriginalPos;
            else targetEnemy.transform.position = playerOriginalPos;

            Debug.Log($"Swapped position between Player and {targetEnemy.name}");
        }
    }

    // 2. W 스킬: 모든 적을 플레이어 앞으로 서서히 모으기 (Tact)
    public void StartGatheringEnemies(Transform playerTransform, float offsetZ = 5f, float gatherDuration = 0.5f)
    {
        if (!IsRoomActive)
        {
            Debug.LogWarning("Room is not active. Cannot gather enemies.");
            return;
        }

        if (enemiesInRoom.Count == 0)
        {
            Debug.Log("No enemies in the room to gather.");
            return;
        }

        StopAllCoroutines();

        // **enemiesInRoom 리스트를 순회하여 적들만 모읍니다.**
        foreach (GameObject enemy in enemiesInRoom)
        {
            if (enemy != null) // 룸 안에 들어왔다가 사라진 적이 있을 수 있으므로 null 체크
            {
                // 플레이어의 앞 방향 (Z축)으로 offsetZ 만큼 떨어진 위치 계산
                Vector3 targetGatherPoint = playerTransform.position + playerTransform.forward * offsetZ;
                Vector3 finalTargetPosition = targetGatherPoint + Random.insideUnitSphere * 0.5f;

                StartCoroutine(GatherEnemyCoroutine(enemy, finalTargetPosition, gatherDuration));
            }
        }
        Debug.Log("Gathering enemies started!");
    }

    private IEnumerator GatherEnemyCoroutine(GameObject enemy, Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = enemy.transform.position;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = timer / duration;

            enemy.transform.position = Vector3.Lerp(startPosition, targetPosition, progress);

            yield return null;
        }

        enemy.transform.position = targetPosition;
    }
}