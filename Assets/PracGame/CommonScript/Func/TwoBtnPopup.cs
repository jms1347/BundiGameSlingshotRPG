using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TwoBtnPopup : Popup
{
    [Header("�ؽ�Ʈ")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI contentText;

    [Header("��ư")]
    [SerializeField] private Button confirmBtn;
    [SerializeField] private Button cancelBtn;
    [SerializeField] private Button dimmedBtn;

    [Header("���� �˾�")]
    [SerializeField] private GameObject pop;


    private void Awake()
    {

    }
}
