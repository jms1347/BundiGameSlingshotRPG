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
        // 현재 Ground 오브젝트의 상태 확인
        if (currentState == GroundState.None)
        {
            // 빈 땅인 경우 처리
        }
        else if (currentState == GroundState.Flower)
        {
            // 꽃이 심어져 있는 경우 처리
        }else if(currentState == GroundState.VinylHouse)
        {
            // 비닐 하우스가 있는 경우 처리
        }
        else if (currentState == GroundState.Obstacle)
        {
            // 장애물이 있는 경우 처리
        }
    }

    public void SetGroundState(GroundState pState)
    {
        // Ground 오브젝트의 상태 설정
        currentState = pState;
    }


    public void PlantFlower()
    {
        // 현재 상태가 빈 땅인 경우에만 꽃을 심을 수 있습니다.
        if (currentState == GroundState.None)
        {
            // 꽃 프리팹을 생성하여 Ground 오브젝트에 배치
            //Instantiate(flowerPrefab, transform.position, Quaternion.identity, transform);

            // Ground 오브젝트의 상태를 Flower로 변경
            SetGroundState(GroundState.Flower);
        }
    }
}
