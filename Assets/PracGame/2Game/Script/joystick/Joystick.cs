using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;           //UI 네임스페이스 선언
using UnityEngine.EventSystems; //EventSystem 네임스페이스 선언

public class JoyStick : MonoBehaviour,
                               IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]private Image bgImg;
    [SerializeField] private Image joystickImg;
    private Vector3 inputVector;

    [SerializeField] private GameObject leftTopArrow, rightTopArrow, leftBottomArrow, rightBottomArrow;
    private void Start()
    {
       // bgImg = GetComponent<Image>();
       // joystickImg = transform.GetChild(0).GetComponent<Image>();
    }
    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform,
                ped.position,
                ped.pressEventCamera,
                out pos))
        {
            pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);

            inputVector = new Vector3(pos.x * 2, 0, pos.y * 2);
            inputVector = (inputVector.magnitude > 1.0f) ?
                          inputVector.normalized : inputVector;

            //// Move JoyStick Img
            joystickImg.rectTransform.anchoredPosition =
                new Vector3(inputVector.x * (bgImg.rectTransform.sizeDelta.x / 3)
                //조이스틱 안에 원 최대치
                , inputVector.z * (bgImg.rectTransform.sizeDelta.y / 3));
            SetArrow();
        }
    }

    public void SetArrow()
    {
        if (inputVector.x > 0 && inputVector.z > 0)
        {
            rightTopArrow.SetActive(false);
            leftTopArrow.SetActive(false);
            leftBottomArrow.SetActive(true);
            rightBottomArrow.SetActive(false);
        }
        else if (inputVector.x < 0 && inputVector.z > 0)
        {
            rightTopArrow.SetActive(false);
            leftTopArrow.SetActive(false);
            leftBottomArrow.SetActive(false);
            rightBottomArrow.SetActive(true);
        }
        else if (inputVector.x < 0 && inputVector.z < 0)
        {
            rightTopArrow.SetActive(true);
            leftTopArrow.SetActive(false);
            leftBottomArrow.SetActive(false);
            rightBottomArrow.SetActive(false);
        }
        else if (inputVector.x > 0 && inputVector.z < 0)
        {
            rightTopArrow.SetActive(false);
            leftTopArrow.SetActive(true);
            leftBottomArrow.SetActive(false);
            rightBottomArrow.SetActive(false);
        }
        else
        {
        }
    }
    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        inputVector = Vector3.zero;
        joystickImg.rectTransform.anchoredPosition = Vector3.zero;
    }
    public float Horizontal()
    {
        if (inputVector.x != 0)
        {
            return inputVector.x;
        }
        else
        {
            return Input.GetAxis("Horizontal");
        }
    }
    public float Vertical()
    {
        if (inputVector.z != 0)
        {
            return inputVector.z;
        }
        else
        {
            return Input.GetAxis("Vertical");
        }
    }
}
