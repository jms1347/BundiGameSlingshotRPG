using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsData : MonoBehaviour
{
    private int level;
    private int att;
    private int dep;
    private int maxHp;
    private int currentHp;
    private float moveSpeed;
    private float attSpeed;

    public int Level { get => level; set => level = value; }
    public int Att { get => att; set => att = value; }
    public int Dep { get => dep; set => dep = value; }
    public int MaxHp { get => maxHp; set => maxHp = value; }
    public int CurrentHp { get => currentHp; set => currentHp = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public float AttSpeed { get => attSpeed; set => attSpeed = value; }

    public void SetStatsData(int l, int a, int d, int maxH, int currentH, float msp, float asp)
    {
        level = l;
        att = a;
        dep = d;
        maxHp = maxH;
        currentHp = currentH;
        moveSpeed = msp;
        attSpeed = asp;
    }

    public StatsData GetStatsData()
    {
        return this.GetComponent<StatsData>();
    }
}
