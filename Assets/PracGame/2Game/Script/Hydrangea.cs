using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hydrangea : MonoBehaviour
{
    public enum Grade
    {
        S = 0,
        A = 1,
        B = 2,
        C = 3,
        D = 4
    }


    public Grade grade;

    public void SetHydragea()
    {
        int ranGrade = Random.Range(0, 5);
        grade = (Grade)ranGrade;
    }
}
