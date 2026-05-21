using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeloMelo_SQL_Services_DataStructure
{
    #region SAVE SQL (API)
    public static readonly string MeloMelo_Save_Track_ProgressScoreData = "MeloMelo_SaveProgress_2024.php";
    public static readonly string MeloMelo_Save_Track_ProgressPointData = "MeloMelo_SaveProgress_2_2024.php";
    public static readonly string MeloMelo_Save_Track_ProgressBattleStatusData = "MeloMelo_SaveProgress_3_2024.php";
    public static readonly string MeloMelo_Save_TrackComboProgressData = "MeloMelo_Save_TrackComboProgress_2025.php";

    public static readonly string MeloMelo_Save_ExchangePoint_TranscationHistory = "MeloMelo_Save_ExchangePoint_Transcation_2025.php";
    #endregion

    #region LOAD SQL (API)
    public static readonly string MeloMelo_Load_Track_ProgressScoreData = "MeloMelo_LoadProgress_1_2024.php";
    public static readonly string MeloMelo_Load_Track_ProgressPointData = "MeloMelo_LoadProgress_2_2024.php";
    public static readonly string MeloMelo_Load_Track_ProgressBattleStatusData = "MeloMelo_LoadProgress_3_2024.php";

    public static readonly string MeloMelo_Load_SystemSettings = "MeloMelo_Load_SystemSettings_2025.php";
    public static readonly string MeloMelo_Load_GameSettings = "MeloMelo_SettingConfigOnLoad_2024.php";
    public static readonly string MeloMelo_Load_PlayerSettings = "MeloMelo_Load_PlayerSettings_2024.php";

    public static readonly string MeloMelo_Load_Profile = "MeloMelo_ProfileLoader_2024.php";
    public static readonly string MeloMelo_Load_LastSelectionTrack = "MeloMelo_Load_MusicSelectionLastVisited_2024.php";
    public static readonly string MeloMelo_Load_BattleFormation = "MeloMelo_Load_BattleFormation_2024.php";
    public static readonly string MeloMelo_Load_CharacterStatus = "MeloMelo_Load_CharacterStatusData_2024.php";

    public static readonly string MeloMelo_Load_VirtualItemData = "MeloMelo_Load_VirtualItemData_2025.php";
    public static readonly string MeloMelo_Load_MarathonSaveData = "MeloMelo_Load_MarathonProgress_2025.php";
    public static readonly string MeloMelo_Load_TrackComboProgressData = "MeloMelo_Load_TrackComboProgress_2025.php";

    public static readonly string MeloMelo_Load_ExchangePoint_TranscationHistory = "MeloMelo_Load_ExchangePoint_Transcation_2025.php";
    #endregion

    #region MISC
    public static int DifficultyByIndex(string difficulty)
    {
        return difficulty == "ULTIMATE" ? 3 : difficulty == "HARD" ? 2 : 1;
    }
    #endregion
}

#region DATA STRUCTURE (All Progress - Track)
[System.Serializable]
public class Cloud_DataStructure_TrackScoreData
{
    public int Secure_Key;
    public string UserId;
    public string Title;
    public string Difficulty;
    public int Score;
    public int Combo;
}

[System.Serializable]
public class CloudData_TrackScoreData
{
    public Cloud_DataStructure_TrackScoreData[] data;
}

[System.Serializable]
public class Cloud_DataStructure_TrackPointData
{
    public int Secure_Key;
    public string UserId;
    public string Title;
    public string Difficulty;
    public int Points;
}

[System.Serializable]
public class CloudData_TrackPointData
{
    public Cloud_DataStructure_TrackPointData[] data;
}

[System.Serializable]
public class Cloud_DataStructure_TrackBattleStatusData
{
    public int Secure_Key;
    public string UserId;
    public string Title;
    public string Difficulty;
    public int Remark;
}

[System.Serializable]
public class CloudData_TrackBattleStatusData
{
    public Cloud_DataStructure_TrackBattleStatusData[] data;
}

[System.Serializable]
public class Cloud_DataStructure_TrackComboProgress
{
    public int Secure_Key;
    public string UserId;
    public string Title;
    public string difficulty;
    public int maxCombo;
    public int overall_combo;
    public int maxOut_count;
}

[System.Serializable]
public class CloudData_TrackComboProgress
{
    public Cloud_DataStructure_TrackComboProgress[] data;
}
#endregion

#region DATA STRUCTURE (All Settings)
[System.Serializable]
public class Cloud_DataStructure_GameSettings
{
    public string UserId;
    public string MV;
    public int NoteSpeed;
    public int AutoRetreat;

    public int Display1;
    public int Display2;
    public int JudgeType;
    public int Judge_Feedback_Main;
    public int Judge_Feedback_Sub;

    public string character_active_skill;
}

[System.Serializable]
public class CloudData_GameSettings
{
    public Cloud_DataStructure_GameSettings config;
}

[System.Serializable]
public class Cloud_DataStructure_PlayerSettings
{
    public string UserId;
    public int Tutorial_Gameplay;
    public int Tutorial_BattleSetup;
    public int Tutorial_Control;

    public float BGM_Audio;
    public float SE_Audio;
}

[System.Serializable]
public class CloudData_PlayerSettings
{
    public Cloud_DataStructure_PlayerSettings player;
}

[System.Serializable]
public class Cloud_DataStructure_SystemSettings
{
    public string UserId;

    public int audio_mute;
    public int voice_mute;

    public int interface_animation;
    public int character_animation;
    public int enemy_animation;

    public int damageIndicator_A;
    public int damageIndicator_B;

    public int frameRateLimit;
    public int performanceOptimize;

    public int unitHealthType_OnCharacter;
    public int unitHealthType_OnEnemy;

    public int speedMeter;
    public int fancyMovement;

    public int autoSaveProgress;
    public int autoSaveGameSettings;
    public int autoSavePlayerSettings;

    public int playEventSettings_display;
    public int playEventSettings_reward;
    public int playEventSettings_skipOnGoing;
}

[System.Serializable]
public class CloudData_SystemSettings
{
    public Cloud_DataStructure_SystemSettings config;
}
#endregion

[System.Serializable]
public class Cloud_DataStructure_Profile
{
    public string UserId;
    public int RatePoint;
    public int PlayedCount;
    public int Credit;
}

[System.Serializable]
public class CloudData_Profile
{
    public Cloud_DataStructure_Profile profile;
}

[System.Serializable]
public class Cloud_DataStructure_TrackLastSelection
{
    public string UserId;
    public string AreaSelection;
    public int TrackSelection;
    public string Difficulty;
}

[System.Serializable]
public class CloudData_TrackLastSelection
{
    public Cloud_DataStructure_TrackLastSelection[] trackList;
}

[System.Serializable]
public class Cloud_DataStructure_BattleFormation
{
    public string UserId;
    public string Slot1;
    public string Slot2;
    public string Slot3;
    public string MainSlot;
}

[System.Serializable]
public class CloudData_BattleFormation
{
    public Cloud_DataStructure_BattleFormation config;
}

#region DATA STRUCTURE (All Progress - Character)
[System.Serializable]
public class Cloud_DataStructure_CharacterStatus
{
    public string UserId;
    public string NameId;
    public int Level_id;
    public int Exp_id;

    public int totalMasteryAdded;
    public int totalRebirthPoint;
    public int baseStrStats;
    public int baseVitStats;
    public int baseMagStats;
}

[System.Serializable]
public class CloudData_CharacterStatus
{
    public Cloud_DataStructure_CharacterStatus[] data;
}
#endregion

[System.Serializable]
public class Cloud_DataStructure_ItemData
{
    public string ItemName;
    public int Amount;
}

[System.Serializable]
public class CloudData_ItemData
{
    public Cloud_DataStructure_ItemData[] data;
}

[System.Serializable]
public class Cloud_DataStructure_MarathonProgress
{
    public string UserId;
    public string Title;

    public int cleared_status;
    public int playedCount;
    public int totalScore;
}

[System.Serializable]
public class CloudData_MarathonProgress
{
    public Cloud_DataStructure_MarathonProgress[] data;
}

[System.Serializable]
public class Cloud_DataStructure_TrackDistributionList
{
    public string UserId;
    public string Title;
    public string Image_ID;
    public int Difficulty;
    public string Level;

    public int Score;
    public int Point;
    public int Chart_ID;
}

[System.Serializable]
public class CloudData_TrackDistributionList
{
    public Cloud_DataStructure_TrackDistributionList[] trackList;
}