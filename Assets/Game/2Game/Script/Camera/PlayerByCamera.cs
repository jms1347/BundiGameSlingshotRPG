using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerByCamera : MonoBehaviour
{
    public GameObject Target;               // 카메라가 따라다닐 타겟

    public float offsetX = 0.0f;            // 카메라의 x좌표
    public float offsetY = 10.0f;           // 카메라의 y좌표
    public float offsetZ = -10.0f;          // 카메라의 z좌표

    public float CameraSpeed = 10.0f;       // 카메라의 속도
    Vector3 TargetPos;                      // 타겟의 위치
    Vector3 oriOffset;                      // 타겟의 위치

    [SerializeField] bool isFollowTarget = false;

    [Header("범위 제한 변수")]
    [SerializeField] private BoxCollider2D boundary; // 박스 콜라이더를 드래그하여 할당합니다.
    private Vector3 minBounds;
    private Vector3 maxBounds;

    private float cameraHeight, cameraWidth;

    public bool IsFollowTarget { get => isFollowTarget; set => isFollowTarget = value; }

    
    public void SetBoundary(BoxCollider2D pBoxColl) {
        boundary = pBoxColl;

        minBounds = boundary.bounds.min;
        maxBounds = boundary.bounds.max;

        //Debug.Log("minBounds : " + minBounds);
        //Debug.Log("maxBounds : " + maxBounds);
        //Debug.Log("current x : " + this.transform.position.x);
        //Debug.Log("current y : " + this.transform.position.y);
        //Debug.Log("cameraHeight : " + cameraHeight);
        //Debug.Log("cameraWidth : " + cameraWidth);
    }

    private void Awake()
    {
        oriOffset = new Vector3(offsetX, offsetY, offsetZ);
        cameraHeight = Camera.main.orthographicSize * 2;
        cameraWidth = cameraHeight * ((float)Screen.width / (float)Screen.height);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Target != null)
        {
            if (IsFollowTarget)
            {
                if (IsCatOutsideBoundary())
                {
                    Vector3 newCameraPosition = new Vector3(
                            Mathf.Clamp(Target.transform.position.x + offsetX, minBounds.x + (cameraWidth * 0.5f), maxBounds.x - (cameraWidth * 0.5f)),
                            Mathf.Clamp(Target.transform.position.y + offsetY, minBounds.y + (cameraHeight * 0.5f), maxBounds.y - (cameraHeight * 0.5f)),
                             Target.transform.position.z + offsetZ
                        );

                    // 카메라의 위치를 고양이 위치로 부드럽게 이동
                    transform.position = Vector3.Lerp(transform.position, newCameraPosition, Time.deltaTime * CameraSpeed);

                }
                else
                {
                    // 타겟의 x, y, z 좌표에 카메라의 좌표를 더하여 카메라의 위치를 결정
                    TargetPos = new Vector3(
                        Target.transform.position.x + offsetX,
                        Target.transform.position.y + offsetY,
                        Target.transform.position.z + offsetZ
                        );

                    // 카메라의 움직임을 부드럽게 하는 함수(Lerp)
                    Vector3 newPosition = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * CameraSpeed);

                    // 카메라의 위치를 경계 내로 제한합니다.
                    newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x + (cameraWidth * 0.5f), maxBounds.x - (cameraWidth * 0.5f));
                    newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y + (cameraHeight * 0.5f), maxBounds.y - (cameraHeight * 0.5f));
                    newPosition.z = Target.transform.position.z + offsetZ;
                    transform.position = newPosition;
                }
            }
        }
    }
   

    public void Shake()
    {
        this.transform.DOShakePosition(0.5f, 1f, 10, 90, false, true, ShakeRandomnessMode.Harmonic)
            .OnComplete(() => this.transform.DOLocalMove(new Vector3(0, 0, this.transform.localPosition.z), 0.5f).SetEase(Ease.Flash));
    }
    public void SetOffset(Vector3 pVec)
    {
        offsetX = pVec.x;
        offsetY = pVec.y;
        offsetZ = pVec.z;
    }

    public void ResetOffset()
    {
        offsetX = oriOffset.x;
        offsetY = oriOffset.y;
        offsetZ = oriOffset.z; 
    }

    #region 고양이가 카메라 허용 경계 밖에 있는지 체크
    private bool IsCatOutsideBoundary()
    {
        // 카메라의 크기를 고려하여 경계 체크
        float halfCameraWidth = cameraWidth * 0.5f;
        float halfCameraHeight = cameraHeight * 0.5f;

        return Target.transform.position.x < minBounds.x + halfCameraWidth ||
               Target.transform.position.x > maxBounds.x - halfCameraWidth ||
               Target.transform.position.y < minBounds.y + halfCameraHeight ||
               Target.transform.position.y > maxBounds.y - halfCameraHeight;
    }
    //private bool IsCatOutsideBoundary( )
    //{
    //    // 고양이의 위치가 경계 밖에 있는지 확인
    //    return Target.transform.position.x < minBounds.x + (cameraWidth * 0.5f) ||
    //           Target.transform.position.x > maxBounds.x - (cameraWidth * 0.5f) ||
    //           Target.transform.position.y < minBounds.y + (cameraHeight * 0.5f) ||
    //           Target.transform.position.y > maxBounds.y - (cameraHeight * 0.5f);
    //}
    #endregion
}
