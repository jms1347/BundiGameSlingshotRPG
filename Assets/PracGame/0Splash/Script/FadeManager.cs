using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;

public class FadeManager : Singleton<FadeManager>
{
    public CanvasGroup Fade_img;
    float fadeDuration = 2; //암전되는 시간
    public GameObject Loading;
    public TextMeshProUGUI Loading_text; //퍼센트 표시할 텍스트
    public string nextScene;

    public void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // 이벤트에 추가
    }
    IEnumerator LoadScene(string sceneName)
    {
        Loading.SetActive(true); //로딩 화면을 띄움

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false; //퍼센트 딜레이용

        float past_time = 0;
        float percentage = 0;

        while (!(async.isDone))
        {
            yield return null;

            past_time += Time.deltaTime;

            if (percentage >= 90)
            {
                percentage = Mathf.Lerp(percentage, 100, past_time);

                if (percentage == 100)
                {
                    async.allowSceneActivation = true; //씬 전환 준비 완료
                }
            }
            else
            {
                percentage = Mathf.Lerp(percentage, async.progress * 100f, past_time);
                if (percentage >= 90) past_time = 0;
            }
            Loading_text.text = percentage.ToString("0") + "%"; //로딩 퍼센트 표기
        }
    }


    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // 이벤트에서 제거*
    }


    public void ChangeScene(string sceneName)
    {
        //PhotonNetwork.IsMessageQueueRunning = false;
        Fade_img.DOFade(1, fadeDuration)
        .OnStart(() =>
        {
            Fade_img.blocksRaycasts = true; //아래 레이캐스트 막기
        })
        .OnComplete(() =>
        {
            //로딩화면 띄우며, 씬 로드 시작
            StartCoroutine(nameof(LoadScene), sceneName); /// 씬 로드 코루틴 실행 ///
        });
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Fade_img.DOFade(0, fadeDuration)
        .OnStart(() => {
            Loading.SetActive(false);
        })
        .OnComplete(() => {
            Fade_img.blocksRaycasts = false;
        });
    }



    public void OnLoading()
    {
        Loading.SetActive(true); //로딩 화면을 띄움

        Loading_text.gameObject.SetActive(false);
    }

    public void OffLoading()
    {
        Loading.SetActive(false); //로딩 화면을 띄움

        Loading_text.gameObject.SetActive(true);
    }


    private void OpenScene()
    {
        Fade_img.alpha = 1;

        Fade_img.DOFade(0, fadeDuration)
        .OnStart(() => {
            Loading.SetActive(false);
        })
        .OnComplete(() => {
            Fade_img.blocksRaycasts = false;
            Fade_img.DOKill();
        });
    }
}
