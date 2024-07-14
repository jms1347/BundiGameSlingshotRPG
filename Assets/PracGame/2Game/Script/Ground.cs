using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public enum GroundState
    {
        None = 0,
        Flower = 1,
        VinylHouse = 2,
        Obstacle = 3
    }
    public GroundState currentState = GroundState.None;

    public void CheckGroundState()
    {
        // ���� Ground ������Ʈ�� ���� Ȯ��
        if (currentState == GroundState.None)
        {
            // �� ���� ��� ó��
        }
        else if (currentState == GroundState.Flower)
        {
            // ���� �ɾ��� �ִ� ��� ó��
        }else if(currentState == GroundState.VinylHouse)
        {
            // ��� �Ͽ콺�� �ִ� ��� ó��
        }
        else if (currentState == GroundState.Obstacle)
        {
            // ��ֹ��� �ִ� ��� ó��
        }
    }

    public void SetGroundState(GroundState pState)
    {
        // Ground ������Ʈ�� ���� ����
        currentState = pState;
    }


    public void PlantFlower()
    {
        // ���� ���°� �� ���� ��쿡�� ���� ���� �� �ֽ��ϴ�.
        if (currentState == GroundState.None)
        {
            // �� �������� �����Ͽ� Ground ������Ʈ�� ��ġ
            //Instantiate(flowerPrefab, transform.position, Quaternion.identity, transform);

            // Ground ������Ʈ�� ���¸� Flower�� ����
            SetGroundState(GroundState.Flower);
        }
    }
}
