using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAutoPoolController : MonoBehaviour
{
    public GameObject blueBallPrefab;
    public GameObject brownBallPrefab;
    public GameObject greenBallPrefab;
    public GameObject yellowBallPrefab;

    public Transform[] pools;
    public AP_Pool[] AP_pools;
    void Awake()
    {
        //사용할 프리팹 등록하기
        MF_AutoPool.InitializeSpawn(blueBallPrefab,0,2, AP_enum.EmptyBehavior.Grow, AP_enum.MaxEmptyBehavior.Fail);
        MF_AutoPool.InitializeSpawn(yellowBallPrefab,0,2, AP_enum.EmptyBehavior.Grow, AP_enum.MaxEmptyBehavior.ReuseOldest);
        MF_AutoPool.InitializeSpawn(greenBallPrefab, 0, 2, AP_enum.EmptyBehavior.ReuseOldest, AP_enum.MaxEmptyBehavior.ReuseOldest);
        MF_AutoPool.InitializeSpawn(brownBallPrefab, 0, 2, AP_enum.EmptyBehavior.ReuseOldest, AP_enum.MaxEmptyBehavior.ReuseOldest);


    }

    public void Start()
    {
        StartCoroutine(TestBallCreate());
    }


    IEnumerator TestBallCreate()
    {
        for (int i = 0; i < 10; i++)
        {
               GameObject a = MF_AutoPool.Spawn(blueBallPrefab, null, new Vector3(-4f, 25, 0), Quaternion.identity);
            GameObject b = MF_AutoPool.Spawn(yellowBallPrefab, null, new Vector3(-2f, 25, 0), Quaternion.identity);
            GameObject c = MF_AutoPool.Spawn(brownBallPrefab, null, new Vector3(2, 25, 0), Quaternion.identity);
            GameObject d = MF_AutoPool.Spawn(greenBallPrefab, null, new Vector3(4, 25, 0), Quaternion.identity);
            if(a!=null)
            a.transform.SetParent(pools[0].transform);
            if (b != null)
                b.transform.SetParent(pools[1].transform);
            if (c != null)
                c.transform.SetParent(pools[2].transform);
            if (d != null)
                d.transform.SetParent(pools[3].transform);

            yield return new WaitForSeconds(1.0f);

            MF_AutoPool.Despawn(c.GetComponent<AP_Reference>());
            MF_AutoPool.Despawn(d.GetComponent<AP_Reference>(),1);
        }

        MF_AutoPool.DespawnPool(blueBallPrefab);


        for (int i = 0; i < AP_pools.Length; i++)
        {
            print("풀 용량 : " + AP_pools[i].pool.Count);
            print("masterPool 용량 : " + AP_pools[i].masterPool.Count);
        }

        yield return new WaitForSeconds(10.0f);
        for (int i = 0; i < AP_pools.Length; i++)
        {
            print("풀 용량 : " + AP_pools[i].pool.Count);
            print("masterPool 용량 : " + AP_pools[i].masterPool.Count);
        }
    }
}
