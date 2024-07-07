using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TranslationDB
{
    public string key;
    public string kor;
    public string eng;
}

[CreateAssetMenu(fileName = "TranslationSO", menuName = "ScriptableObject/TranslationSO")]
public class TranslationSo : ScriptableObject
{
    public List<TranslationDB> translationDataList = new List<TranslationDB>();
}
