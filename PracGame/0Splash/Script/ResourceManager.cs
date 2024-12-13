using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ResourceData<T>
{
    public string resourceKey;
    public T resource;
}

public class ResourceManager : Singleton<GoogleSheetManager>
{
    public List<ResourceData<Sprite>> spriteList = new List<ResourceData<Sprite>>();
    public List<ResourceData<GameObject>> prefabList = new List<ResourceData<GameObject>>();
    public List<ResourceData<VideoClip>> videoClipList = new List<ResourceData<VideoClip>>();
    public List<ResourceData<AudioClip>> aduioClipList = new List<ResourceData<AudioClip>>();

}
