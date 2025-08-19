using System.Collections.Generic;
using UnityEngine;

public class PopupParameters : Dictionary<string, object>

{

    /// <summary>
    /// ��ϵ� key�� value�� ã���ϴ�. key���� ���ٸ� spareValue�� ��ȯ�մϴ�.
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
    /// ��ϵ� key�� value�� ã���ϴ�. key���� ���ٸ� �ش� Ÿ���� default Value�� ��ȯ�մϴ�.
    /// </summary>
    public T GetValueOrDefault<T>(string key, bool logWarning = true)
    {
        if (TryGetValue(key, out var result)) return (T)result;

        if (logWarning)
            Debug.LogWarning($"key [{key}] is not found. defaultValue use.");

        return default;
    }

}