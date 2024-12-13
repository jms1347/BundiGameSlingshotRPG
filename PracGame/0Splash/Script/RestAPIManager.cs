//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.Networking;
//using System;
//using Newtonsoft.Json;
//using UnityEditor;
//using Defective.JSON;
//using System.Threading.Tasks;
//using System.Text.RegularExpressions;
//using System.Net;
//using System.Net.Http;
//using System.Net.Sockets;
//using System.IO;

//public class RestAPIController : Singleton<RestAPIController>
//{
//    //private readonly string dbURLApi = "172.30.1.41/api";
//    private string dbURL = "https://dokdo.fairip.synology.me/";
//    private string dbURLApi = "https://dokdo.fairip.synology.me/api";
//    private readonly string urlLogin = "/login.php";
//    private readonly string urlPoint = "/point.php";
//    private readonly string urlRanking = "/ranking.php";
//    private readonly string urlContent = "/content";
//    private readonly string urlRoom = "/room.php";
//    private readonly string urlAllMission = "/mission_all.php";
//    private readonly string urlMissionState = "/mission_state.php";
//    private readonly string urlImageUpload = "/filesave.php";

//    string responseData = "";
//    string basicKey = "AmEDb3EKroi9Kd6MoeLQ";
//    private void Awake()
//    {
//        StartCoroutine(GetExternalIPAddressCoroutine());
//    }

//    private IEnumerator GetExternalIPAddressCoroutine()
//    {
//        var getIpTask = GetExternalIPAddress(); // 비동기 함수 호출
//        while (!getIpTask.IsCompleted)
//        {
//            yield return null; // 비동기 작업이 완료될 때까지 대기
//        }

//        Debug.Log($"체크 IP 주소: {getIpTask.Result}"); // 결과 출력
//        Debug.Log($"체크 IP 주소: " + getIpTask.ToString().Trim()); // 결과 출력

//        if (getIpTask.Result.Trim() == "118.42.96.245")
//        {
//            dbURL = "https://dokdo.fairip.synology.me";
//            dbURLApi = "https://dokdo.fairip.synology.me/api";
//            //dbURL = "http://172.30.1.41";
//            //dbURLApi = "http://172.30.1.41/api";
//        }
//        else
//        {
//            dbURL = "https://dokdo.fairip.synology.me";
//            dbURLApi = "https://dokdo.fairip.synology.me/api";
//        }
//    }
//    public async Task<string> GetExternalIPAddress()
//    {
//        using (HttpClient client = new HttpClient())
//        {
//            Task<string> getIpTask = client.GetStringAsync("http://ipinfo.io/ip");
//            string externalIP = await getIpTask;

//            if (String.IsNullOrWhiteSpace(externalIP.Trim()))
//            {
//                externalIP = GetInternalIPAddress();
//            }
//            return externalIP;
//        }
//    }

//    public string GetInternalIPAddress()
//    {
//        var host = Dns.GetHostEntry(Dns.GetHostName());
//        foreach (var ip in host.AddressList) { if (ip.AddressFamily == AddressFamily.InterNetwork) { return ip.ToString(); } }
//        throw new Exception("네트워크에서 IP주소를 가져올 수 없습니다.");
//    }


//    #region 네트워크 통신 함수
//    //일반 Post
//    protected IEnumerator Post(string url, WWWForm form, Action<string> callback)
//    {
//        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
//        {

//            yield return www.SendWebRequest();

//            if (www.result != UnityWebRequest.Result.Success)
//            {
//                Debug.LogError(www.error);

//                PopupManager.instance.OpenPopup("서버 네트워크 오류가 발생했습니다.", () => {
//                    StartCoroutine(Post(url, form, callback));
//                }, () => { });
//                //여기서 팝업 처리 
//                //callback?.Invoke(www.error);
//            }
//            else
//            {

//                string responseData = www.downloadHandler.text;

//                Debug.Log(url + " : " + responseData);

//                callback?.Invoke(responseData);
//            }
//        }
//    }

//    protected IEnumerator Post(string url, List<IMultipartFormSection> form, Action<string> callback)
//    {
//        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
//        {
//            yield return www.SendWebRequest();

//            if (www.result != UnityWebRequest.Result.Success)
//            {
//                Debug.LogError(www.error);

//                PopupManager.instance.OpenPopup("서버 네트워크 오류가 발생했습니다.", () => {
//                    StartCoroutine(Post(url, form, callback));
//                }, () => { });
//                //여기서 팝업 처리 
//                //callback?.Invoke(www.error);
//            }
//            else
//            {

//                string responseData = www.downloadHandler.text;

//                Debug.Log(url + " : " + responseData);

//                callback?.Invoke(responseData);
//            }
//        }
//    }

//    IEnumerator GetTexture(string imageUrl, Action<Texture2D> callback)
//    {

//        string tempUrl = dbURL + imageUrl;
//        Debug.Log("tempUrl : " + tempUrl);
//        if (!Uri.IsWellFormedUriString(tempUrl, UriKind.Absolute))
//        {
//            Debug.LogError("Invalid URL");
//            yield break;
//        }

//        UnityWebRequest request = UnityWebRequestTexture.GetTexture(tempUrl);
//        yield return request.SendWebRequest();

//        if (request.result != UnityWebRequest.Result.Success)
//        {
//            Debug.LogError(request.error);
//        }
//        else
//        {
//            Texture2D texture = DownloadHandlerTexture.GetContent(request);
//            callback(texture);
//        }
//    }
//    #endregion
//    #region 이미지 업로드 / 가져오기
//    public void UploadImage(byte[] fileData, string fileRoute, Action<string> callback)
//    {
//        string url = dbURLApi + urlImageUpload;

//        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();

//        // 파일 확장자에 따라 MIME 타입 결정
//        string mimeType = GetMimeType(fileRoute);
//        // 파일 데이터 추가
//        formData.Add(new MultipartFormFileSection("file", fileData, fileRoute, mimeType));

//        StartCoroutine(Post(url, formData, callback));
//    }

//    private string GetMimeType(string fileName)
//    {
//        string extension = Path.GetExtension(fileName).ToLowerInvariant();
//        switch (extension)
//        {
//            case ".jpg":
//            case ".jpeg":
//                return "image/jpeg";
//            case ".png":
//                return "image/png";
//            case ".pdf":
//                return "application/pdf";
//            default:
//                return "application/octet-stream"; // 기본값으로, 알려지지 않은 파일 형식 처리
//        }
//    }

//    public void GetImage(string pImageUrl, Action<Texture2D> callback)
//    {
//        StartCoroutine(GetTexture(pImageUrl, callback));
//    }
//    #endregion
//    #region 리퀘스트 함수
//    #region 룸
//    public void GetAllRoomInfo(Action<string> callback)
//    {
//        string url = dbURLApi + urlRoom;

//        WWWForm form = new WWWForm();
//        form.AddField("mode", "list");
//        form.AddField("key", basicKey);

//        StartCoroutine(Post(url, form, callback));
//    }

//    public void CreateRoom(int pRoomId, string pRoomTitle, string pPwd, string pIP, bool pPublic, string pUserId, Action<string> callback)
//    {
//        string url = dbURLApi + urlRoom;
//        RequestCreateRoom request = new RequestCreateRoom();
//        request.roomId = pRoomId.ToString("000");
//        request.roomTitle = pRoomTitle;
//        request.roomPwd = pPwd;
//        request.roomIp = pIP;
//        request.isPublic = pPublic;
//        request.userId = pUserId;
//        request.key = basicKey;

//        WWWForm form = new WWWForm();
//        form.AddField("mode", "in");
//        form.AddField("room_id", request.roomId);
//        form.AddField("room_title", request.roomTitle);
//        form.AddField("room_ip", request.roomIp);
//        form.AddField("is_public", request.isPublic ? "true" : "false");
//        form.AddField("userid", request.userId);
//        form.AddField("room_pass", request.roomPwd);
//        form.AddField("key", basicKey);

//        StartCoroutine(Post(url, form, callback));
//    }

//    public void GetRoomInfo(int pRoomID, Action<string> callback)
//    {
//        string url = dbURLApi + urlRoom;

//        WWWForm form = new WWWForm();
//        form.AddField("mode", "get");
//        form.AddField("room_id", pRoomID.ToString("000"));
//        form.AddField("key", basicKey);

//        StartCoroutine(Post(url, form, callback));
//    }

//    public void InRoom(string pUserId, int pRoomId, Action<string> callback)
//    {
//        string url = dbURLApi + urlRoom;

//        WWWForm form = new WWWForm();
//        form.AddField("mode", "userin");
//        form.AddField("userid", pUserId);
//        form.AddField("room_id", pRoomId.ToString("000"));
//        form.AddField("key", basicKey);

//        StartCoroutine(Post(url, form, callback));
//    }
//    public void OutRoom(string pUserId, int pRoomId, Action<string> callback)
//    {
//        string url = dbURLApi + urlRoom;

//        WWWForm form = new WWWForm();
//        form.AddField("mode", "userout");
//        form.AddField("userid", pUserId);
//        form.AddField("room_id", pRoomId.ToString("000"));
//        form.AddField("key", basicKey);

//        StartCoroutine(Post(url, form, callback));
//    }
//    #endregion
//    #region 미션
//    public void GetAllMission(string userId, Action<string> callback)
//    {
//        string url = dbURLApi + urlAllMission;

//        WWWForm form = new WWWForm();
//        form.AddField("userid", userId);
//        form.AddField("key", basicKey);

//        StartCoroutine(Post(url, form, callback));
//    }

//    public void GetMissionState(string userId, string missionId, Action<string> callback)
//    {
//        string url = dbURLApi + urlMissionState;

//        WWWForm form = new WWWForm();
//        form.AddField("userid", userId);
//        form.AddField("mission_id", missionId);
//        form.AddField("key", basicKey);

//        StartCoroutine(Post(url, form, callback));
//    }
//    #endregion
//    #region 로그인
//    public void Login(string userId, string password, Action<string> callback)
//    {
//        string url = dbURLApi + urlLogin;
//        RequestLogin request = new RequestLogin();
//        request.id = userId;
//        request.pwd = password;
//        request.key = basicKey;

//        WWWForm form = new WWWForm();
//        form.AddField("userid", request.id);
//        form.AddField("userpw", request.pwd);
//        form.AddField("key", request.key);

//        StartCoroutine(Post(url, form, callback));
//    }
//    #endregion
//    #region 포인트 적립
//    public void AddPoint(string userId, int pPoint, string pGameCode, int pMissionType, Action<string> callback)
//    {
//        string url = dbURLApi + urlPoint;
//        Debug.Log("userId : " + userId);
//        Debug.Log("pGameCode : " + pGameCode);
//        Debug.Log("pMissionType : " + pMissionType);
//        RequestMissionPoint request = new RequestMissionPoint();
//        request.id = userId;
//        request.point = pPoint;     //클라부분은 포인트 합산해서 업뎃해야됨
//        request.missionId = pGameCode;
//        request.missionType = pMissionType;
//        request.key = basicKey;

//        WWWForm form = new WWWForm();
//        form.AddField("userid", request.id);
//        form.AddField("point", request.point);
//        form.AddField("mission_id", request.missionId);
//        form.AddField("mission_type", request.missionType);
//        form.AddField("mode", "save");
//        form.AddField("key", request.key);

//        StartCoroutine(Post(url, form, callback));
//    }
//    #endregion
//    #region 전체 랭킹 조회
//    public void AllRank(Action<string> callback)
//    {
//        string url = dbURLApi + urlRanking;
//        RequestMissionPoint request = new RequestMissionPoint();
//        request.key = basicKey;

//        WWWForm form = new WWWForm();
//        form.AddField("key", request.key);

//        StartCoroutine(Post(url, form, callback));

//    }
//    #endregion
//    public void UsePointBtn()
//    {
//        string url = dbURLApi + urlPoint;
//        RequestMissionPoint request = new RequestMissionPoint();
//        request.id = "test1";
//        request.point = 5;     //클라부분은 포인트 합산해서 업뎃해야됨
//        request.missionId = "DD_DISPLAY";
//        request.missionType = 1;
//        request.key = basicKey;

//        WWWForm form = new WWWForm();
//        form.AddField("userid", request.id);
//        form.AddField("point", request.point);
//        form.AddField("mission_id", request.missionId);
//        form.AddField("misstion_type", request.missionType);
//        form.AddField("mode", "used");
//        form.AddField("key", request.key);

//        StartCoroutine(Post(url, form, SetJsonUsePoint));
//    }

//    public void GetSoloRankBtn()
//    {
//        string url = dbURLApi + urlRanking;
//        RequestRanking request = new RequestRanking();
//        request.id = "test1";
//        request.key = basicKey;

//        WWWForm form = new WWWForm();
//        form.AddField("userid", request.id);
//        form.AddField("key", request.key);

//        StartCoroutine(Post(url, form, SetJsonSoloRank));
//    }

//    public void GetAllRankBtn()
//    {
//        string url = dbURLApi + urlRanking;

//        WWWForm form = new WWWForm();
//        form.AddField("key", basicKey);

//        StartCoroutine(Post(url, form, SetJsonAllRank));
//    }
//    public void GetExhibitionDataBtn()
//    {
//        string url = dbURLApi + urlContent;
//        RequestMissionPoint request = new RequestMissionPoint();

//        WWWForm form = new WWWForm();
//        form.AddField("ca_id", "DD_DISPLAY");
//        form.AddField("key", basicKey);

//        StartCoroutine(Post(url, form, SetJsonExhibition));
//    }
//    #endregion
//    IEnumerator Post(string pUrl, WWWForm pParam, Action pSucCallback)
//    {
//        using (UnityWebRequest www = UnityWebRequest.Post(pUrl, pParam))
//        {
//            yield return www.SendWebRequest();
//            if (www.result != UnityWebRequest.Result.Success)
//            {
//                Debug.LogError(www.error);
//                PopupManager.instance.OpenPopup("네트워크 오류가 발생했습니다.\n" + www.error, () => { }, () => { });
//            }
//            else
//            {
//                responseData = www.downloadHandler.text;
//                Debug.Log(responseData);

//                pSucCallback();
//            }
//        }
//    }

//    #region 응답 콜백
//    #region 공통 response    

//    public ResponseResult SetJsonResponse(string pResponseData)
//    {
//        string decodeData = Utils.DecodeEncodedNonAsciiCharacters(pResponseData);
//        JSONObject jsonObj = new JSONObject(decodeData);

//        ResponseResult result = new ResponseResult();
//        result.result = jsonObj.GetField("result");
//        result.msg = jsonObj.GetField("msg");
//        result.info = jsonObj.GetField("info");
//        //result.userid = jsonObj.GetField("userid");
//        //result.rank = jsonObj.GetField("rank");
//        //result.point = jsonObj.GetField("point");
//        //if(jsonObj.GetField("ranking") != null)
//        //    result.rankList = jsonObj.GetField("ranking").list;

//        if (jsonObj.GetField("info") != null)
//            result.infoList = jsonObj.GetField("info").list;
//        if (jsonObj.GetField("mission_list") != null)
//            result.infoList = jsonObj.GetField("mission_list").list;

//        return result;
//    }
//    #endregion





//    #region 포인트 사용 콜백
//    public void SetJsonUsePoint()
//    {
//        //SetJsonResponse();

//        ////FR_Member fr_Member = new FR_Member();  //새로 선언이 아니라 기존물 담는 콜라이더 게임매니져 데이터를 가져와야함 (이건 임시 테스트용)
//        ////fr_Member.userId = result.info.GetField("userid").stringValue;              //이건 기존 아이디 확인용 or 검색
//        ////fr_Member.point = fr_Member.point - result.info.GetField("point").intValue; //기존 포인트에서 빼기
//    }
//    #endregion

//    #region 솔로랭크 콜백
//    public void SetJsonSoloRank()
//    {
//        // SetJsonResponse();

//        //FR_Member fr_Member = new FR_Member();  //새로 선언이 아니라 기존 게임매니져 데이터를 가져와야함 (이건 임시 테스트용)
//        //fr_Member.userId = result.userid.stringValue;
//        //fr_Member.rank = int.Parse(result.rank.stringValue);
//        //fr_Member.point = int.Parse(result.point.stringValue);
//    }
//    #endregion
//    #region 올 랭크 콜백
//    public void SetJsonAllRank()
//    {
//        //SetJsonResponse();

//        //List<FR_RankData> rankDataList = new List<FR_RankData>();
//        //for (int i = 0; i < result.rankList.Count; i++)
//        //{
//        //    FR_RankData rankData = new FR_RankData();
//        //    rankData.userId = result.rankList[i].GetField("userid").stringValue;
//        //    rankData.rank = int.Parse(result.rankList[i].GetField("rank").stringValue);
//        //    rankData.point = int.Parse(result.rankList[i].GetField("point").stringValue);

//        //    rankDataList.Add(rankData); 
//        //}
//    }
//    #endregion

//    #region 겟 전시관 데이터 콜백
//    public void SetJsonExhibition()
//    {
//        //SetJsonResponse();

//        //List<FR_CategoryData> exhibitionList = new List<FR_CategoryData>();
//        //for (int i = 0; i < result.exhibitionList.Count; i++)
//        //{
//        //    FR_CategoryData exhibitionListData = new FR_CategoryData();
//        //    exhibitionListData.contentId = result.exhibitionList[i].GetField("con_id").stringValue;
//        //    exhibitionListData.categoryId = result.exhibitionList[i].GetField("ca_id").stringValue;
//        //    exhibitionListData.ord = int.Parse(result.exhibitionList[i].GetField("ord").stringValue);
//        //    exhibitionListData.contentFile = result.exhibitionList[i].GetField("con_file").stringValue;

//        //    exhibitionList.Add(exhibitionListData);
//        //}
//    }

//    #endregion
//    #endregion
//}
