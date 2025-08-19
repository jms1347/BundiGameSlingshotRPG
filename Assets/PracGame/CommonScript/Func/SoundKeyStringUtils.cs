using UnityEngine;

public class SoundKeyStringUtils : MonoBehaviour
{
    public enum SoundNameKey
    {
        Run,
        Jump,
        Land,
        Drop,
        WallSlide_Stop,
        Sliding_Slow,
        Sliding_Fast,
        Land_Small,
        GM_FG_Fall,
        GM_FG_Land,
        GM_FG_Shake,
        GM_M_Jump,
        GM_PS_Bound,
        GM_PS_Fly,
        GM_PS_Touch,
        GM_SI_Land,
        TimeSkill_Charge,
        TimeSkill_Error,
        CameraMove_Default,
        GM_SI_Make,
        Death,
        TimeSkill_Move
    }

    #region 오디오 키값 가져오기
    public static string GetSoundKeyString(SoundNameKey soundKey)
    {
        switch (soundKey)
        {
            case SoundNameKey.Run:
                return "Run";
            case SoundNameKey.Jump:
                return "Jump";
            case SoundNameKey.Land:
                return "Land";
            case SoundNameKey.Drop:
                return "Drop";
            case SoundNameKey.Sliding_Slow:
                return "Sliding_Slow";
            case SoundNameKey.Sliding_Fast:
                return "Sliding_Fast";
            case SoundNameKey.Land_Small:
                return "Land_Small";
            case SoundNameKey.GM_FG_Fall:
                return "GM_FG_Fall";
            case SoundNameKey.GM_FG_Land:
                return "GM_FG_Land";
            case SoundNameKey.GM_FG_Shake:
                return "GM_FG_Shake";
            case SoundNameKey.GM_M_Jump:
                return "GM_M_Jump";
            case SoundNameKey.GM_PS_Bound:
                return "GM_PS_Bound";
            case SoundNameKey.GM_PS_Fly:
                return "GM_PS_Fly";
            case SoundNameKey.GM_PS_Touch:
                return "GM_PS_Touch";
            case SoundNameKey.GM_SI_Land:
                return "GM_SI_Land";
            case SoundNameKey.TimeSkill_Charge:
                return "TimeSkill_Charge";
            case SoundNameKey.TimeSkill_Error:
                return "TimeSkill_Error";
            case SoundNameKey.CameraMove_Default:
                return "CameraMove_Default";
            case SoundNameKey.WallSlide_Stop:
                return "WallSlide_Stop";
            case SoundNameKey.GM_SI_Make:
                return "GM_SI_Make";
            case SoundNameKey.Death:
                return "Death";
            case SoundNameKey.TimeSkill_Move:
                return "TimeSkill_Move";
            default:
                return string.Empty;
        }
    }
    #endregion
}
