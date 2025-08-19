using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TwoBtnPopup : Popup
{
    [Header("텍스트")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI contentText;

    [Header("버튼")]
    [SerializeField] private Button confirmBtn;
    [SerializeField] private Button cancelBtn;
    [SerializeField] private Button dimmedBtn;

    [Header("실제 팝업")]
    [SerializeField] private GameObject pop;


    private void Awake()
    {

    }
}
