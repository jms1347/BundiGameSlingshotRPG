using System;
using System.Collections;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleSheetManager : Singleton<GoogleSheetManager>
{
    const string versionDbURL = "https://docs.google.com/spreadsheets/d/1W0Mype6OpsFGqNv6gAF2KMxCmQ3S6_W57e2VhlWF5fI/export?format=tsv&gid=0&range=A1:C";
    const string translationDbURL = "https://docs.google.com/spreadsheets/d/1W0Mype6OpsFGqNv6gAF2KMxCmQ3S6_W57e2VhlWF5fI/export?format=tsv&gid=1360483987&range=A1:C";
    public VersionSo versionSO;
    public TranslationSo translationSO;

    void Awake()
    {
       // DontDestroyOnLoad(this.gameObject);

        CheckGetAllGSData();
    }

    public int testIndex = 0;

    async void CheckGetAllGSData()
    {
        string result = await GetGSDataToURL(versionDbURL);
        await Task.Run(() => SetVersionData(result));
        result = await GetGSDataToURL(translationDbURL);
        await Task.Run(() => SetTranslationData(result));
        //LoadingSceneManager.LoadScene("SplashScene 1");
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

    #region 버전 데이터 넣기
    void SetVersionData(string data)
    {
        if (versionSO.versionData != null) versionSO.versionData = null;
        string[] row = data.Split('\t');
        versionSO.versionData = new VersionDB();
        versionSO.versionData.majorNum = int.Parse(row[0]);
        versionSO.versionData.minorNum = int.Parse(row[1]);
        versionSO.versionData.patchNum = int.Parse(row[2]);
    }
    #endregion
    #region 번역 데이터 넣기
    void SetTranslationData(string data)
    {
        if (translationSO.translationDataList != null || translationSO.translationDataList.Count > 0) translationSO.translationDataList.Clear();

        int lineSize;
        string[] line = data.Split('\n');
        lineSize = line.Length;
        for (int i = 0; i < lineSize; i++)
        {
            TranslationDB trans = new TranslationDB();
            string[] row = line[i].Split('\t');

            trans.key = row[0];
            trans.kor = row[1];
            trans.eng = row[2];

            translationSO.translationDataList.Add(trans);
        }        
    }
    #endregion
}
