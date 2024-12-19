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
        // 터치가 있을 때
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // 터치 시작 시
            if (touch.phase == TouchPhase.Began)
            {
                // 터치한 스크린 포지션 저장
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
