using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

public class SplashController : MonoBehaviour
{
    private void Start()
    {
        // isActive가 true로 변경될 때 호출되는 함수
        GoogleSheetManager.instance.IsSetData.Where(x => x).Subscribe(_ => SetAllGoogleData()).AddTo(this);

        // 예시로 2초 후에 isActive를 true로 변경
        //Observable.Timer(System.TimeSpan.FromSeconds(2)).Subscribe(_ => GoogleSheetManager.instance.IsSetData.Value = true).AddTo(this);

    }
    private void SetAllGoogleData()
    {
        Debug.Log("로비씬 이동");
        //LoadingSceneManager.LoadScene("SelectStoryScene");
    }
}
