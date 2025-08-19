using System;
using System.Collections;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UniRx;

public class GoogleSheetManager : Singleton<GoogleSheetManager>
{
    const string skillDataUrl = "https://docs.google.com/spreadsheets/d/17gVAX6xYsTKI0w3HmAj4twFTVX6JiPZl5gP3TPkIZKU/export?format=tsv&gid=0&range=A2:I";
    //public VersionSo versionSO;
    public SkillDataSo skillDataSo;

    private BoolReactiveProperty isSetData = new BoolReactiveProperty(false);

    public BoolReactiveProperty IsSetData { get => isSetData; set => isSetData = value; }

    void Awake()
    {
       // DontDestroyOnLoad(this.gameObject);

        CheckGetAllGSData();
    }


    async void CheckGetAllGSData()
    {
        string result = await GetGSDataToURL(skillDataUrl);
        await Task.Run(() => SetSkillData(result));

        IsSetData.Value = true;
    }


    async UniTask<string> GetGSDataToURL(string url)
    {
        try
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            await www.SendWebRequest();
            string data = www.downloadHandler.text;
            return data;
        }
        catch
        {
            return "Error";
        }       
    }
    #region 번역 데이터 넣기
    void SetSkillData(string data)
    {
        if (skillDataSo.skillDataList != null || skillDataSo.skillDataList.Count > 0) skillDataSo.skillDataList.Clear();

        int lineSize;
        string[] line = data.Split('\n');
        lineSize = line.Length;
        for (int i = 0; i < lineSize; i++)
        {
            SkillData trans = new SkillData();
            string[] row = line[i].Split('\t');

            trans.skillCode = row[0];
            trans.skillName = row[1];
            trans.skillDesc = row[2];
            trans.iconIndex = row[3] != null ? int.Parse(row[3]) : 0;
            trans.cooldown = row[4] != null ? float.Parse(row[4]) : 0f;
            trans.damage = row[5] != null ? float.Parse(row[5]) : 0f;
            trans.mpCost = row[6] != null ? float.Parse(row[6]) : 0f;
            trans.castingTime = row[7] != null ? float.Parse(row[7]) : 0f;

            switch (row[8])
            {
                case "0":
                    trans.skillType = SkillType.Active;
                    break;
                case "1":
                    trans.skillType = SkillType.Buff;
                    break;
                case "2":
                    trans.skillType = SkillType.Passive;
                    break;
                case "3":
                    trans.skillType = SkillType.Ultimate;
                    break;
                default:
                    trans.skillType = SkillType.Active;
                    break;
            }
            skillDataSo.skillDataList.Add(trans);
        }
    }
    #endregion
    //#region 버전 데이터 넣기
    //void SetVersionData(string data)
    //{
    //    if (versionSO.versionData != null) versionSO.versionData = null;
    //    string[] row = data.Split('\t');
    //    versionSO.versionData = new VersionDB();
    //    versionSO.versionData.majorNum = int.Parse(row[0]);
    //    versionSO.versionData.minorNum = int.Parse(row[1]);
    //    versionSO.versionData.patchNum = int.Parse(row[2]);
    //}
    //#endregion
    //#region 번역 데이터 넣기
    //void SetTranslationData(string data)
    //{
    //    if (translationSO.translationDataList != null || translationSO.translationDataList.Count > 0) translationSO.translationDataList.Clear();

    //    int lineSize;
    //    string[] line = data.Split('\n');
    //    lineSize = line.Length;
    //    for (int i = 0; i < lineSize; i++)
    //    {
    //        TranslationDB trans = new TranslationDB();
    //        string[] row = line[i].Split('\t');

    //        trans.key = row[0];
    //        trans.kor = row[1];
    //        trans.eng = row[2];

    //        translationSO.translationDataList.Add(trans);
    //    }        
    //}
    //#endregion
}
