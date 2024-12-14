using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using TMPro;
using System.Linq;

public class AddressableManager : MonoBehaviour
{
    [Header("서버 UI")]
    public GameObject waitMessage;  //        guideText.text = "업데이트 체크중...";
    public GameObject downMessage;
    public GameObject barObj;
    public GameObject completePopup;
    public GameObject downBtn;

    public Image downSlider;
    public TextMeshProUGUI sizeInfoText;
    public TextMeshProUGUI downValText;



    [SerializeField] AssetReference assetRef;

    private long patchSize;
    private Dictionary<string, long> patchMap = new Dictionary<string, long>();

    [SerializeField] private List<string> labelList;

    private void Start()
    {
        //TestLoad();

        waitMessage.SetActive(true);
        downMessage.SetActive(false);

        StartCoroutine(InitAddressable());
        StartCoroutine(CheckUpdateFiles());
    }

    private string GetFileSize(long byteCnt)
    {
        string size = "0 Bytes";

        if (byteCnt >= 1073741824.0)
        {
            size = string.Format("{0:##.##}", byteCnt / 1073741824.0) + " GB";
        }
        else if (byteCnt >= 1048576.0)
        {
            size = string.Format("{0:##.##}", byteCnt / 1048576.0) + " MB";
        }
        else if (byteCnt >= 1024.0)
        {
            size = string.Format("{0:##.##}", byteCnt / 1024.0) + " KB";
        }
        else if (byteCnt > 0 && byteCnt < 1024.0)
        {
            size = byteCnt.ToString() + " Bytes";
        }

        return size;
    }
    IEnumerator InitAddressable()
    {
        var init = Addressables.InitializeAsync();
        yield return init;
    }

    IEnumerator CheckUpdateFiles()
    {
        var labels = labelList;

        patchSize = default;

        foreach (var label in labels)
        {
            var handle = Addressables.GetDownloadSizeAsync(label);

            yield return handle;

            patchSize += handle.Result;
        }

        if (patchSize > decimal.Zero)
        {
            //다운
            waitMessage.SetActive(false);
            downMessage.SetActive(true);

            sizeInfoText.text = "" + GetFileSize(patchSize);
        }
        else
        {
            downValText.text = " 100 % ";
            downSlider.fillAmount = 1f;
            yield return Utils.WaitForSecond(2f);

            waitMessage.transform.parent.parent.gameObject.SetActive(false);

            SetResource();

        }
    }

    public void SetResource()
    {
        // 사운드 데이터 넣기
        Utils.LoadAssetsByLabelAndCache<AudioClip>("Sound", assets =>
        {
            foreach (var asset in assets)
            {
                // 캐싱하거나 필요한 작업 수행
                Debug.Log("로드된 자산: " + asset.name);
                SoundManager.instance.AddAudioClip(asset.name, asset);
            }
        });
    }

    //업데이트 확인
    public void Btn_Down()
    {
        StartCoroutine(PatchFiles());
    }

    IEnumerator PatchFiles()
    {
        var labels = labelList;


        foreach (var label in labels)
        {
            var handle = Addressables.GetDownloadSizeAsync(label);

            yield return handle;

            if (handle.Result != decimal.Zero)
            {
                StartCoroutine(DownLoadLabel(label));
            }
        }

        yield return CheckDownLoad();
    }



    IEnumerator DownLoadLabel(string label)
    {
        barObj.SetActive(true);
        if (!patchMap.TryGetValue(label, out var value))
        {
            patchMap[label] = 0;
        }


        var handle = Addressables.DownloadDependenciesAsync(label, false);

        while (!handle.IsDone)
        {
            patchMap[label] = handle.GetDownloadStatus().DownloadedBytes;
            yield return new WaitForEndOfFrame();
        }

        patchMap[label] = handle.GetDownloadStatus().TotalBytes;
        Addressables.Release(handle);
    }

    IEnumerator CheckDownLoad()
    {
        var total = 0f;
        downValText.text = "0 %";

        while (true)
        {
            total += patchMap.Sum(tmp => tmp.Value);

            downSlider.fillAmount = total / patchSize;
            downValText.text = (int)(downSlider.fillAmount * 100) + " %";

            if (total == patchSize)
            {
                Debug.Log("다운로드 끝!");
                completePopup.SetActive(true);
                break;
            }

            total = 0f;
            yield return new WaitForEndOfFrame();
        }
    }

    //#region 사용 예시
    AsyncOperationHandle handle;
    public Image testImg;
    public Image testImg2;
    public void TestLoad()
    {
        //LoadPVR("Sprite_star");
        Utils.LoadAssetAndHandle<AudioClip>("Sound_click_1", sound =>
        {
            SoundManager.instance.AddAudioClip("Sound_click_1", Utils.OnLoadComplete(sound));
        });

    }
    public void LoadPVR(string pName)
    {
        //예시 불러오는 법
        Utils.LoadAssetAndHandle<Sprite>(pName, sprite => {
            testImg.sprite = sprite;
        });

        Utils.LoadAssetAndHandle<Sprite>(pName, sprite =>
        {
            testImg2.sprite = Utils.OnLoadComplete(sprite);
        });

    }


    public void UnLoadPVR()
    {
        if (handle.IsValid()) // handle이 유효한지 확인
        {
            Addressables.Release(handle); // 핸들을 해제
            handle = default; // 핸들을 초기 상태로 재설정
            testImg.sprite = null;
        }
    }
    //#endregion
}
