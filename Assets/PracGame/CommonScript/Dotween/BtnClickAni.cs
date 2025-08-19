using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using System.Collections;

public class BtnClickAni : MonoBehaviour, IPointerDownHandler
{
    private RectTransform buttonRectTransform;

    // 버튼 원래 크기를 저장해둘 변수
    private Vector3 originalScale;

    // 클릭했을 때 버튼이 얼마나 작아질지 (예: 0.9f 면 90% 크기)
    [SerializeField] private float scaleDownMultiplier = 0.9f;

    // 작아지는 데 걸리는 시간
    [SerializeField] private float scaleDownDuration = 0.1f;

    // 다시 커지는 데 걸리는 시간
    [SerializeField] private float scaleUpDuration = 0.1f;

    void Awake()
    {
        // 스크립트가 붙은 게임 오브젝트의 RectTransform 컴포넌트를 가져와요!
        buttonRectTransform = GetComponent<RectTransform>();
    }

    // 이 함수를 버튼의 OnClick 이벤트에 연결할 거예요!
    public void OnButtonClick()
    {
        buttonRectTransform.DOKill();
        buttonRectTransform.localScale = Vector3.one;
        buttonRectTransform.DOScale(Vector3.one * scaleDownMultiplier, scaleDownDuration).OnComplete(() =>
            buttonRectTransform.DOScale(Vector3.one, scaleUpDuration));


        // TODO: 여기다가 버튼이 눌렸을 때 실제로 하고 싶은 다른 작업들 (예: 씬 이동, 팝업 띄우기 등)을 추가하면 돼요!
        Debug.Log("버튼이 눌렸어요! DOtween 효과 재생!"); // 콘솔에 메시지 출력 예시
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnButtonClick();
    }
}