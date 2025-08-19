using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerByCamera : MonoBehaviour
{
    public GameObject Target;               // ī�޶� ����ٴ� Ÿ��

    public float offsetX = 0.0f;            // ī�޶��� x��ǥ
    public float offsetY = 10.0f;           // ī�޶��� y��ǥ
    public float offsetZ = -10.0f;          // ī�޶��� z��ǥ

    public float CameraSpeed = 10.0f;       // ī�޶��� �ӵ�
    Vector3 TargetPos;                      // Ÿ���� ��ġ
    Vector3 oriOffset;                      // Ÿ���� ��ġ

    [SerializeField] bool isFollowTarget = false;

    [Header("���� ���� ����")]
    [SerializeField] private BoxCollider2D boundary; // �ڽ� �ݶ��̴��� �巡���Ͽ� �Ҵ��մϴ�.
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

                    // ī�޶��� ��ġ�� ����� ��ġ�� �ε巴�� �̵�
                    transform.position = Vector3.Lerp(transform.position, newCameraPosition, Time.deltaTime * CameraSpeed);

                }
                else
                {
                    // Ÿ���� x, y, z ��ǥ�� ī�޶��� ��ǥ�� ���Ͽ� ī�޶��� ��ġ�� ����
                    TargetPos = new Vector3(
                        Target.transform.position.x + offsetX,
                        Target.transform.position.y + offsetY,
                        Target.transform.position.z + offsetZ
                        );

                    // ī�޶��� �������� �ε巴�� �ϴ� �Լ�(Lerp)
                    Vector3 newPosition = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * CameraSpeed);

                    // ī�޶��� ��ġ�� ��� ���� �����մϴ�.
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

    #region ����̰� ī�޶� ��� ��� �ۿ� �ִ��� üũ
    private bool IsCatOutsideBoundary()
    {
        // ī�޶��� ũ�⸦ ����Ͽ� ��� üũ
        float halfCameraWidth = cameraWidth * 0.5f;
        float halfCameraHeight = cameraHeight * 0.5f;

        return Target.transform.position.x < minBounds.x + halfCameraWidth ||
               Target.transform.position.x > maxBounds.x - halfCameraWidth ||
               Target.transform.position.y < minBounds.y + halfCameraHeight ||
               Target.transform.position.y > maxBounds.y - halfCameraHeight;
    }
    //private bool IsCatOutsideBoundary( )
    //{
    //    // ������� ��ġ�� ��� �ۿ� �ִ��� Ȯ��
    //    return Target.transform.position.x < minBounds.x + (cameraWidth * 0.5f) ||
    //           Target.transform.position.x > maxBounds.x - (cameraWidth * 0.5f) ||
    //           Target.transform.position.y < minBounds.y + (cameraHeight * 0.5f) ||
    //           Target.transform.position.y > maxBounds.y - (cameraHeight * 0.5f);
    //}
    #endregion
}
