using UnityEngine;

public class PopupKeyUtils : MonoBehaviour
{
    public enum PopupNameKey
    {
        Basic =0 
    }

    #region 팝업 키값 가져오기
    public static string GetPopupKeyString(PopupNameKey soundKey)
    {
        switch (soundKey)
        {
            case PopupNameKey.Basic:
                return "BasicPopup";
            
            default:
                return string.Empty;
        }
    }
    #endregion
}
