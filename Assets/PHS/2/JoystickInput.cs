using UnityEngine;
using UnityEngine.UI;

public class JoystickInput : MonoBehaviour
{
    public GameObject joystickUI;
    public RectTransform handle;
    public Slider chargeSlider;
    public float maxDragDistance = 300f;

    private Vector2 startTouchPosition;
    public Vector2 CurrentDirection { get; private set; }
    public float DragValue { get; private set; }

    public void ProcessInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 touchPosition = Input.mousePosition;

            // 조이스틱 및 슬라이더 활성화
            joystickUI.transform.position = touchPosition;
            joystickUI.SetActive(true);

            chargeSlider.gameObject.SetActive(true);
            chargeSlider.value = 0;

            startTouchPosition = touchPosition;
            handle.anchoredPosition = Vector2.zero;

            // 슬라이더를 캐릭터 아래로 이동
            Vector3 characterScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
            chargeSlider.transform.position = characterScreenPosition + new Vector3(0, -50, 0);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 dragPosition = Input.mousePosition;
            Vector2 currentDragPosition = (Vector2)dragPosition - startTouchPosition;

            float dragDistance = Mathf.Clamp(currentDragPosition.magnitude, 0, maxDragDistance);
            DragValue = dragDistance / maxDragDistance;
            CurrentDirection = currentDragPosition.normalized;

            handle.anchoredPosition = dragDistance <= maxDragDistance
                ? currentDragPosition
                : currentDragPosition.normalized * maxDragDistance;

            chargeSlider.value = DragValue;
        }

        if (Input.GetMouseButtonUp(0))
        {
            joystickUI.SetActive(false);
            chargeSlider.gameObject.SetActive(false);
            handle.anchoredPosition = Vector2.zero;
        }
    }
}