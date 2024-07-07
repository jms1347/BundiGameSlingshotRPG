using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TranslationManager : Singleton<TranslationManager>
{
    public TranslationSo translationSo;

    public string Text(string pKey)
    {

        if (translationSo.translationDataList.Find((temp) => temp.key.Contains(pKey)) == null)
        {
            Debug.Log("키값 없음");
        }

        int index = translationSo.translationDataList.FindIndex((temp)=>temp.key == pKey);
        if (index == -1) return "";

        switch (Application.systemLanguage)
        {
            case SystemLanguage.Korean:
                return translationSo.translationDataList[index].kor;
            case SystemLanguage.English:
                return translationSo.translationDataList[index].eng;
            default:
                return translationSo.translationDataList[index].kor;
        }
    }

}
