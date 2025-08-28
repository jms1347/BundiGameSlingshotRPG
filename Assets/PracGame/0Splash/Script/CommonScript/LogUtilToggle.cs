using System.Linq;
using UnityEditor;
using UnityEngine;

public class LogUtilToggle : MonoBehaviour
{
    [MenuItem("Build/Debug Log/Enable DEBUG_LOG")]
    [System.Obsolete]
    public static void EnableDebugLog()
    {
        SetDebugLogDefine(true);
    }

    [MenuItem("Build/Debug Log/Disable DEBUG_LOG")]
    [System.Obsolete]
    public static void DisableDebugLog()
    {
        SetDebugLogDefine(false);
    }

    [System.Obsolete]
    public static void SetDebugLogDefine(bool enable)
    {
        var targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup).Split(";").ToList();

        if (enable && !defines.Contains("DEBUG_LOG"))
        {
            defines.Add("DEBUG_LOG");
        }
        else if (!enable)
        {
            defines.RemoveAll(d => d == "DEBUG_LOG");
        }

        PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, string.Join(";", defines));
    }
}
