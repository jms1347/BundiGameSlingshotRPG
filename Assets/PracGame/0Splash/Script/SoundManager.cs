using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System;
using System.Linq;

[Serializable]
public class AudioClipDic
{
    public string audioClipName;
    public AudioClip audioClip;
}

public class SoundManager : Singleton<SoundManager>
{

    public enum SoundType
    {
        BGM = 0,
        Sfx = 1,
        UI = 2,
        LoopSfx = 3
    }

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Audio Source")]
    public AudioSource bgM;
    public AudioSource sfxM;
    public AudioSource uiM;
    public AudioSource LoopSfxM;
    public AudioSource defaultM;

    [Header("Audio Clip")]
    public List<AudioClipDic> audioClipList;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        //Debug.Log("arg0 : " + arg0);
        //Debug.Log("arg0 buildIndex : " + arg0.buildIndex);
        //Debug.Log("arg0 name : " + arg0.name);

        //if (arg0.buildIndex.ToString().Equals("1")  )
        //{
        //    //PlayBGMByKey("SelectStoryScene");
        //}
        //else
        //{

        //}
    }

    #region 오디오 Clip 가져오기(1개)
    public AudioClip GetAudioClip(string pClipNameKey)
    {
        if (pClipNameKey == "None") return null;

        AudioClipDic tempClipDic = audioClipList.Where(temp => temp.audioClipName == pClipNameKey).FirstOrDefault();

        if (tempClipDic == null)
        {
            return null;
        }
        else
        {
            return tempClipDic.audioClip;
        }
    }
    #endregion
    #region 오디오 Clip 가져오기(List에서 랜덤)
    public AudioClip GetAudioClipList(string pClipNameKey)
    {
        if (pClipNameKey == "None") return null;

        List<AudioClipDic> tempClipDic = audioClipList.Where(temp => temp.audioClipName == pClipNameKey).ToList();

        if (tempClipDic.Count == 0)
        {
            return null;
        }
        else
        {
            int tempIndex = UnityEngine.Random.Range(0, tempClipDic.Count);
            return tempClipDic[tempIndex].audioClip;
        }
    }
    #endregion

    //옵션을 변경할 때 소리의 불륨을 조절하는 함수
    public void SetVolume(SoundType type, float value)
    {
        audioMixer.SetFloat(type.ToString(), value);
    }

    #region 재생 / 멈춤
    public void PlaySoundByKey(SoundType soundType, string pClipKey, float volume = 1f)
    {
        if (!GetAudioSource(soundType).isPlaying)
        {
            AudioClip tempClip = GetAudioClip(pClipKey);
            if (tempClip != null)
            {
                GetAudioSource(soundType).clip = tempClip;
                GetAudioSource(soundType).volume = volume;
                GetAudioSource(soundType).Play();
            }
        }
    }
    public void StopSoundByType(SoundType soundType)
    {
        if (GetAudioSource(soundType).isPlaying)
        {
            GetAudioSource(soundType).Stop();
        }
    }
    #endregion

    #region 오디오 클립 타임 가져오기
    public float GetSoundClipTime(SoundType soundType)
    {

        if (GetAudioSource(soundType).clip != null)
        {
            return (float)GetAudioSource(soundType).clip.length;
        }

        return 0;
    }
    #endregion

    #region 오디오 소스 가져오기
    public AudioSource GetAudioSource(SoundType soundType)
    {
        switch (soundType)
        {
            case SoundType.BGM:
                return bgM;
            case SoundType.Sfx:
                return sfxM;
            case SoundType.LoopSfx:
                return LoopSfxM;
            case SoundType.UI:
                return uiM;
            default:
                return defaultM;
        }
    }
    #endregion
    #region 오디오 그룹 가져오기
    public AudioMixerGroup GetAudioMixerGroup(SoundType soundType)
    {
        switch (soundType)
        {
            case SoundType.BGM:
                return audioMixer.FindMatchingGroups("BGM")[0];
            case SoundType.Sfx:
                return audioMixer.FindMatchingGroups("Sfx")[0];
            case SoundType.LoopSfx:
                return audioMixer.FindMatchingGroups("LoopSfx")[0];
            case SoundType.UI:
                return audioMixer.FindMatchingGroups("UI")[0];
            default:
                return audioMixer.FindMatchingGroups("Default")[0];
        }
    }
    #endregion

    public GameObject ReturnPlayCreateSfxObj(SoundType soundType, string keyName, float volume = 1)
    {
        AudioClip clip = GetAudioClipList(keyName);
        if (clip == null)
        {
            Debug.LogError("호출한 사운드 파일이 사운드 매니져에 존재하지 않습니다.");
            return null;
        }
        GameObject go = new GameObject(keyName + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();

        audioSource.outputAudioMixerGroup = GetAudioMixerGroup(soundType);
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();

        return go;
        //Destroy(go, clip.length);
    }

    public void PlayCreateSfx(SoundType soundType, string keyName, float volume = 1)
    {
        AudioClip clip = GetAudioClipList(keyName);
        if (clip == null)
        {
            Debug.LogError("호출한 사운드 파일이 사운드 매니져에 존재하지 않습니다.");
            return;
        }
        GameObject go = new GameObject(keyName + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();

        audioSource.outputAudioMixerGroup = GetAudioMixerGroup(soundType);
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();

        Destroy(go, clip.length);
    }
    public void PlayCreateSfxToLifeTime(SoundType soundType, string keyName, float lifeTime)
    {
        AudioClip clip = GetAudioClipList(keyName);
        if (clip == null)
        {
            Debug.LogError("호출한 사운드 파일이 사운드 매니져에 존재하지 않습니다.");
            return;
        }
        GameObject go = new GameObject(keyName + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();

        audioSource.outputAudioMixerGroup = GetAudioMixerGroup(soundType);
        audioSource.clip = clip;
        audioSource.volume = 1;
        audioSource.Play();

        Destroy(go, lifeTime);
    }

    #region 사운드 추가
    public void AddAudioClip(string pClipNameKey, AudioClip pAudioClip)
    {
        AudioClipDic tempClip = new AudioClipDic();
        tempClip.audioClipName = pClipNameKey;
        tempClip.audioClip = pAudioClip;
        audioClipList.Add(tempClip);
    }
    #endregion

}