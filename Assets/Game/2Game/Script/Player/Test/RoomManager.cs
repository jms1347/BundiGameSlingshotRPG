using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class RoomManager : MonoBehaviour
{
    public float roomRadius = 10f;
    private SphereCollider roomCollider;
    private List<GameObject> objectsInRoom = new List<GameObject>(); // �� ���� ��� ������Ʈ (���⿡�� �÷��̾ ���Ե� �� ����)
    private List<GameObject> enemiesInRoom = new List<GameObject>(); // �� ���� Enemy �±� ������Ʈ (�� ����Ʈ�� ���)

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
        // �뿡 ������ ��� ������Ʈ�� objectsInRoom�� �߰� (�÷��̾� ����)
        if (!objectsInRoom.Contains(other.gameObject))
        {
            objectsInRoom.Add(other.gameObject);
            Debug.Log(other.name + " entered the room.");

            // **���⼭ Enemy �±׸� enemiesInRoom�� �߰��մϴ�.**
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

    // --- �� ��ų ���� �Լ� ---

    // 1. Q ��ų: ������ �� 1��� ��ü ��ġ ��ȯ (Shambles)
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
            // �÷��̾�� ���� Rigidbody�� ����� ��ġ ��ȯ
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

    // 2. W ��ų: ��� ���� �÷��̾� ������ ������ ������ (Tact)
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

        // **enemiesInRoom ����Ʈ�� ��ȸ�Ͽ� ���鸸 �����ϴ�.**
        foreach (GameObject enemy in enemiesInRoom)
        {
            if (enemy != null) // �� �ȿ� ���Դٰ� ����� ���� ���� �� �����Ƿ� null üũ
            {
                // �÷��̾��� �� ���� (Z��)���� offsetZ ��ŭ ������ ��ġ ���
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