using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using System.Collections;

public class BtnClickAni : MonoBehaviour, IPointerDownHandler
{
    private RectTransform buttonRectTransform;

    // ��ư ���� ũ�⸦ �����ص� ����
    private Vector3 originalScale;

    // Ŭ������ �� ��ư�� �󸶳� �۾����� (��: 0.9f �� 90% ũ��)
    [SerializeField] private float scaleDownMultiplier = 0.9f;

    // �۾����� �� �ɸ��� �ð�
    [SerializeField] private float scaleDownDuration = 0.1f;

    // �ٽ� Ŀ���� �� �ɸ��� �ð�
    [SerializeField] private float scaleUpDuration = 0.1f;

    void Awake()
    {
        // ��ũ��Ʈ�� ���� ���� ������Ʈ�� RectTransform ������Ʈ�� �����Ϳ�!
        buttonRectTransform = GetComponent<RectTransform>();
    }

    // �� �Լ��� ��ư�� OnClick �̺�Ʈ�� ������ �ſ���!
    public void OnButtonClick()
    {
        buttonRectTransform.DOKill();
        buttonRectTransform.localScale = Vector3.one;
        buttonRectTransform.DOScale(Vector3.one * scaleDownMultiplier, scaleDownDuration).OnComplete(() =>
            buttonRectTransform.DOScale(Vector3.one, scaleUpDuration));


        // TODO: ����ٰ� ��ư�� ������ �� ������ �ϰ� ���� �ٸ� �۾��� (��: �� �̵�, �˾� ���� ��)�� �߰��ϸ� �ſ�!
        Debug.Log("��ư�� ���Ⱦ��! DOtween ȿ�� ���!"); // �ֿܼ� �޽��� ��� ����
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnButtonClick();
    }
}