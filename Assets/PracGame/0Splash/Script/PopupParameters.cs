using System.Collections.Generic;
using UnityEngine;

public class PopupParameters : Dictionary<string, object>

{

    /// <summary>
    /// 등록된 key로 value를 찾습니다. key값이 없다면 spareValue를 반환합니다.
    /// </summary>

    public T GetValueOrSpare<T>(string key, T spareValue, bool logWarning = true)
    {
        if (TryGetValue(key, out var result)) return (T)result;

        result = spareValue;

        if (logWarning)
            Debug.LogWarning($"key [{key}] is not found. spareValue use.");

        return (T)result;
    }


    /// <summary>
    /// 등록된 key로 value를 찾습니다. key값이 없다면 해당 타입의 default Value를 반환합니다.
    /// </summary>
    public T GetValueOrDefault<T>(string key, bool logWarning = true)
    {
        if (TryGetValue(key, out var result)) return (T)result;

        if (logWarning)
            Debug.LogWarning($"key [{key}] is not found. defaultValue use.");

        return default;
    }

}