using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickPad : MonoBehaviour, IPointerDownHandler
{
    public JoyStick joystick;

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(eventData.position);
        joystick.transform.position = mousePos;
        joystick.gameObject.SetActive(true);
    }

    void Update()
    {
        // ��ġ�� ���� ��
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // ��ġ ���� ��
            if (touch.phase == TouchPhase.Began)
            {
                // ��ġ�� ��ũ�� ������ ����
                joystick.transform.position = touch.position;
                joystick.gameObject.SetActive(true);

                Debug.Log("Saved Position: " + joystick.transform.position);
            }

            if(touch.phase == TouchPhase.Ended)
            {
                joystick.gameObject.SetActive(false);

            }
        }
    }
}
