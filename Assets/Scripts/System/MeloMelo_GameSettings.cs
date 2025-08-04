using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_RPGEditor;

[System.Serializable]
public class MeloMelo_OptionSettings
{
    public float BGM_Volume;
    public float SE_Volume;
}

[System.Serializable]
public class MeloMelo_ScoreRankSetup
{
    public int score;
    public string rank;
    public Color colorBorder;
    public bool primary;
    public bool secondary;

    public MeloMelo_ScoreRankSetup GetSetup(string format)
    {
        Debug.Log(format);
        return JsonUtility.FromJson<MeloMelo_ScoreRankSetup>(format);
    }
}

[System.Serializable]
public class MeloMelo_StatusRemarkData
{
    public string remark;
    public int order;
    public Color colorBorder;

    public MeloMelo_StatusRemarkData GetSetup(string format)
    {
        Debug.Log(format);
        return JsonUtility.FromJson<MeloMelo_StatusRemarkData>(format);
    }
}

[System.Serializable]
public class MeloMelo_TrackSelectionData
{
    public string areaTitle;
    public int trackIndex;
    public int difficulty;
}

[System.Serializable]
public struct NoteSpeed_Settings
{
    public int bpm;
    public int baseSpeed;

    public NoteSpeed_Settings GetSetup(string format)
    {
        Debug.Log(format);
        return JsonUtility.FromJson<NoteSpeed_Settings>(format);
    }
}

[System.Serializable]
public struct AdventureStoreData
{
    public int adventure_id;
    public int route_id;
    public int status_id;

    public AdventureStoreData GetAdventureEditor(string format)
    {
        Debug.Log(format);
        return JsonUtility.FromJson<AdventureStoreData>(format);
    }
}

[System.Serializable]
public struct VirtualItemDatabase
{
    public string itemName;
    public int amount;

    public VirtualItemDatabase GetItemData(string format)
    {
        Debug.Log(format);
        return JsonUtility.FromJson<VirtualItemDatabase>(format);
    }

    public VirtualItemDatabase(string itemName, int amount)
    {
        this.itemName = itemName;
        this.amount = amount;
    }
}

public struct ElemetStartingStats
{
    public string element;
    public float strength;
    public float vitality;
    public float magic;
    public float multipler;

    public ElemetStartingStats(string name, float str, float vit, float mag, float multipler)
    {
        element = name;
        strength = str;
        vitality = vit;
        magic = mag;
        this.multipler = multipler;
    }
}

[System.Serializable]
public struct PlayEventRewardData
{
    public string itemName;
    public int maxObtain;
    public bool repeatableReward;
    public int playRequirement;
    public string version;

    public PlayEventRewardData InsertEventReward(string format)
    {
        Debug.Log(format);
        return JsonUtility.FromJson<PlayEventRewardData>(format);
    }
}

[System.Serializable]
public struct PlayEventArray
{
    public PlayEventRewardData[] data;

    public PlayEventArray GetBundle(string jsonData)
    {
        Debug.Log(jsonData);
        return JsonUtility.FromJson<PlayEventArray>(jsonData);
    }
}

[System.Serializable]
struct VersionControlArray
{
    public string[] versions;

    public VersionControlArray GetAllData(string jsonData)
    {
        Debug.Log(jsonData);
        return JsonUtility.FromJson<VersionControlArray>(jsonData);
    }
}

[System.Serializable]
public struct BuildInChallengeInfo
{
    public string title;
    public string[] track_title;
    public int[] track_difficulty_mode;
    public string[] track_area;
    public string[] track_difficulty;

    public string directory_title;
    public int conditionType;
    public string condition_data;

    private string GetValueStatus(string value)
    {
        if (int.Parse(value) >= 0) return "-" + int.Parse(value);
        else return "+" + -int.Parse(value);
    }

    public string GetConditionDetails()
    {
        switch (conditionType)
        {
            case 1:
                return "Cleared this challenge with a rank of " + MeloMelo_GameSettings.GetScoreRankStructure(condition_data).rank + " or higher";

            case 2:
                string[] life_rule = condition_data.Split(",");
                return "Survive this challenge with " + life_rule[4] + " life\n"
                    + "Critical (" + GetValueStatus(life_rule[0]) + "), Perfect (" + GetValueStatus(life_rule[1]) + "), Bad (" + GetValueStatus(life_rule[2]) + "), Miss (" + GetValueStatus(life_rule[3]) + ")";

            default:
                string[] valueSplit = condition_data.Split(',');
                return "Cleared this challenge with " + valueSplit[1] + " or less " + GetJudgementFliter(int.Parse(valueSplit[0]));
        }
    }

    private string GetJudgementFliter(int index)
    {
        switch (index)
        {
            case 1:
                return "Critical";

            case 2:
                return "Perfect";

            case 3:
                return "Bad";

            default:
                return "Miss";
        }
    }

    public BuildInChallengeInfo GetDetails(string format)
    {
        Debug.Log(format);
        return JsonUtility.FromJson<BuildInChallengeInfo>(format);
    }
}

[System.Serializable]
public struct CustomMarathonInfo
{
    public BuildInChallengeInfo[] data;

    public CustomMarathonInfo GetArrays(string format)
    {
        Debug.Log(format);
        return JsonUtility.FromJson<CustomMarathonInfo>(format);
    }
}

[System.Serializable]
public struct MarathonExchangeContent
{
    public int itemId;
    public int costAmount;

    public MarathonExchangeContent GetExchange(string format)
    {
        Debug.Log(format);
        return JsonUtility.FromJson<MarathonExchangeContent>(format);
    }
}

public struct MarathonExchangeArray
{
    public MarathonExchangeContent[] marathonContent;

    public MarathonExchangeArray GetExchangeList(string format)
    {
        Debug.Log(format);
        return JsonUtility.FromJson<MarathonExchangeArray>(format);
    }
}

[System.Serializable]
public struct InGamePurchaseContent
{
    public ItemData item;
    public int originalPrice;
    public float priceMultipier;
    public int minPurchasePolicy;

    public InGamePurchaseContent GetInStoreItem(string format)
    {
        Debug.Log(format);
        return JsonUtility.FromJson<InGamePurchaseContent>(format);
    }
}

public static class MeloMelo_GameSettings
{
    public const string CloudSaveSetting_MainProgress = "savelog_AchievementData";
    public const string CloudSaveSetting_BattleProgress = "savelog_BattleProgressData";
    public const string CloudSaveSetting_PointsData = "savelog_PointsData";
    public const string CloudSaveSetting_AcccountSettings = "savelog_PlayerSettings";
    public const string CloudSaveSetting_GameplaySettings = "savelog_GameplaySettings";
    public const string CloudSaveSetting_CharacterSettings = "savelog_CharacterSettings";
    public const string CloudSaveSetting_ProfileData = "savelog_Profile_Setup";
    public const string CloudSaveSetting_SelectionData = "savelog_SelectionBase";
    public const string CloudSaveSetting_CharacterStats = "savelog_CharacterProgressStats";
    public const string CloudSaveSetting_SkillDatabase = "savelog_AllSkillsDatabase";
    public const string CloudSaveSetting_ItemDatabase = "savelog_ItemDatabase";
    public const string CloudSaveSetting_ExchangeHistory = "savelog_ExchangeTranscation";
    public const string CloudSaveSetting_AdventureMode = "savelog_AdventureMode";

    public const string GetLocalFileMainProgress = "savelog_AchievementData.txt";
    public const string GetLocalFileBattleProgress = "savelog_BattleProgressData.txt";
    public const string GetLocalFileAccountSettings = "savelog_PlayerSettings.txt";
    public const string GetLocalFileGameplaySettings = "savelog_GameplaySettings.txt";
    public const string GetLocalFileCharacterSettings = "savelog_CharacterSettings.txt";
    public const string GetLocalFileCharacterStats = "savelog_CharacterProgressStats.txt";
    public const string GetLocalFileSkillDatabase = "savelog_AllSkillsDatabase.txt";
    public const string GetLocalFilePointData = "savelog_PointsData.txt";
    public const string GetLocalFileProfileData = "savelog_Profile_Setup.txt";
    public const string GetLocalFileSelectionData = "savelog_SelectionBase.txt";
    public const string GetLocalFileVirtualItemData = "savelog_ItemDatabase2.txt";
    public const string GetLocalFileExchangeHistory = "savelog_ExchangeTranscation2.txt";
    public const string GetLocalFileAdventureMode = "savelog_AdventureMode.txt";

    public const string GetLocalFileChartLegacy = "ChartData_1";
    public const string GetLocalFileChartOld = "ChartData_2";
    public const string GetLocalFileChartNew = "ChartData_3";

    public const string GetGroundEnemy = "Ground_Enemy";
    public const string GetItem = "Item";
    public const string GetItem2 = "Item2";
    public const string GetHeartPack = "Heart_Pack";
    public const string GetGroundAttack = "Ground_Attack";
    public const string GetAirEnemy = "Air_Enemy";
    public const string GetGroundTrap = "Ground_Trap";
    public const string GetAirTrap = "Air_Trap";
    public const string GetAirAttack = "Air_Attack";
    public const string GetFullAttack = "Full_Attack";
    public const string GetSpecialItem = "Special_Item";
    public const string GetAllTraps = "Full_Traps";

    public static int GetRecentStatusRemark = 0;

    public static string[] GetJudgeFastOrLate = { "Early", "Perfect", "Late" };
    public static string[] GetMainJudgeStats = { "Critical", "Perfect", "Bad", "Miss" };
    public static int[] GetJudgeFastOrLate_Value = new int[3];
    public static int JudgeTimingIndex = 0;
    public static bool targetJudgeTrigger = false;

    public static int[] FastNLate_Critcal = new int[3];
    public static int[] FastNLate_Perfect = new int[3];
    public static int[] FastNLate_Bad = new int[3];

    public static int[] GetInputOffsetValueRange = { -15, 15 };
    public static int[] GetAudioOffsetValueRange = { -15, 15 };

    private static List<MeloMelo_ScoreRankSetup> scoreRankListing = null;
    private static List<MeloMelo_StatusRemarkData> statusRemark = null;
    public static List<MeloMelo_TrackSelectionData> selectionLisitng = new List<MeloMelo_TrackSelectionData>();

    #region GAME: SCORING REFERENCE 
    public static void GetScoreStructureSetup()
    {
        if (scoreRankListing == null)
        {
            scoreRankListing = new List<MeloMelo_ScoreRankSetup>();
            string directory = Application.isEditor ? "Assets" : "MeloMelo_Data";

            System.IO.StreamReader readData = new System.IO.StreamReader(directory + "/StreamingAssets/PlaySettings/MeloMelo_ScoreToRank_Database.csv");
            string currentRead = readData.ReadLine();

            while (!readData.EndOfStream)
            {
                currentRead = readData.ReadLine();
                string[] dataValue = currentRead.Split(',');
                string[] colorElement = dataValue[2].Split('^');

                MeloMelo_ScoreRankSetup setup_template = new MeloMelo_ScoreRankSetup();
                setup_template.rank = dataValue[1];
                setup_template.score = int.Parse(dataValue[0]);
                setup_template.colorBorder = new Color(float.Parse(colorElement[0]), float.Parse(colorElement[1]), float.Parse(colorElement[2]));
                setup_template.primary = dataValue[3] == "TRUE" ? true : false;
                setup_template.secondary = dataValue[4] == "TRUE" ? true : false;

                scoreRankListing.Add(setup_template);
            }

            readData.Close();
        }
    }

    public static MeloMelo_ScoreRankSetup GetScoreRankStructure(string score)
    {
        foreach (MeloMelo_ScoreRankSetup structure in scoreRankListing.ToArray())
        {
            try
            {
                if (structure.secondary && int.Parse(score) >= structure.score) return structure;
                if (structure.primary && int.Parse(score) < structure.score) return structure;
            }
            catch
            {
                if (structure.secondary && score == structure.rank) return structure;
                if (structure.primary && score == structure.rank) return structure;
            }
        }

        return null;
    }

    public static int GetScoreRankStructureOrder(string rank)
    {
        for (int index = 0; index < scoreRankListing.ToArray().Length; index++)
        {
            if (rank == scoreRankListing[index].rank) return index + 1;
        }

        return scoreRankListing.ToArray().Length + 2;
    }

    public static void GetStatusRemarkStructureSetup()
    {
        statusRemark = new List<MeloMelo_StatusRemarkData>();
        string directory = Application.isEditor ? "Assets" : "MeloMelo_Data";

        System.IO.StreamReader readData = new System.IO.StreamReader(directory + "/StreamingAssets/PlaySettings/MeloMelo_TrackStatusRemark.csv");
        string currentRead = readData.ReadLine();

        while (!readData.EndOfStream)
        {
            currentRead = readData.ReadLine();
            string[] dataValue = currentRead.Split(',');
            string[] colorElement = dataValue[2].Split('^');

            MeloMelo_StatusRemarkData setup_template = new MeloMelo_StatusRemarkData();
            setup_template.remark = dataValue[0];
            setup_template.order = int.Parse(dataValue[1]);
            setup_template.colorBorder = new Color(float.Parse(colorElement[0]), float.Parse(colorElement[1]), float.Parse(colorElement[2]));

            statusRemark.Add(setup_template);
        }

        readData.Close();
    }

    public static MeloMelo_StatusRemarkData GetStatusByAchievement(int remark)
    {
        foreach (MeloMelo_StatusRemarkData structure in statusRemark.ToArray())
            if (remark == structure.order) return structure;

        return null;
    }
    #endregion

    #region GAME: SELECTION OPTIONS
    public static int GetLocalTrackSelectionLastVisited(string area, int options)
    {
        foreach (MeloMelo_TrackSelectionData selection in selectionLisitng)
            if (selection.areaTitle == area)
                return options == 1 ? selection.trackIndex : selection.difficulty;

        return 1;
    }
    #endregion

    #region GAME: MISC SETTINGS
    public static float GetLegacyIntitSpeed(float currentBPM)
    {
        const float speedLimitFromBPM = 400;
        if (currentBPM >= speedLimitFromBPM) return 5.5f;
        else return 3.5f;
    }

    public static string GetUnitDisplayHealth(int min, int max, string keyValue)
    {
        switch (PlayerPrefs.GetInt(keyValue, 0))
        {
            case 1:
                float value = min * (100f / max);
                return (int)Mathf.Floor(value) + " %";

            default:
                return min + "/" + max;
        }
    }
    #endregion
}

public static class MeloMelo_ExtensionContent_Settings
{
    public const string GetLocalFileExchangeHistory = "savelog_ExchangeTranscation2.txt";

    private static List<ElemetStartingStats> statsLisitng = null;
    private static PlayEventArray allStoredPlayEventRewards;

    public static int totalMarathonCount = 0;
    public static CustomMarathonInfo marathonListing;

    #region EXTENSION: MARATHON
    public static void LoadMarathonContent(string jsonData)
    {
        if (jsonData != string.Empty)
        {
            marathonListing = new CustomMarathonInfo().GetArrays(jsonData);
            totalMarathonCount = marathonListing.data.Length;
            Debug.Log("Total Marathon Content: " + totalMarathonCount + " Loaded!");
        }
    }

    public static BuildInChallengeInfo LoadMarathonDetail(int index)
    {
        return marathonListing.data[index];
    }
    #endregion

    #region EXTENSION: CHARACTER STATS
    public static void UpdateCharacterProfile()
    {
        foreach (ClassBase character in Resources.LoadAll<ClassBase>("Character_Data"))
        {
            if (character.name != "None")
            {
                StatsManage_Database data = new StatsManage_Database(character.name);
                character.UpdateCurrentStats(false);
                character.strength = data.GetCharacterStatus(character.level).GetStrength;
                character.vitality = data.GetCharacterStatus(character.level).GetVitality;
                character.magic = data.GetCharacterStatus(character.level).GetMagic;
                character.health = data.GetCharacterStatus(character.level).GetHealth;
                character.UpdateStatsCache(true);
            }
        }
    }

    public static void LoadStartingStats()
    {
        if (statsLisitng == null)
        {
            statsLisitng = new List<ElemetStartingStats>();
            string directory = Application.isEditor ? "Assets" : "MeloMelo_Data";

            System.IO.StreamReader readData = new System.IO.StreamReader(directory + "/StreamingAssets/PlaySettings/MeloMelo_RPGStartingStats.csv");
            string currentRead = readData.ReadLine();

            while (!readData.EndOfStream)
            {
                currentRead = readData.ReadLine();
                string[] dataValue = currentRead.Split(',');

                ElemetStartingStats setup_template = new ElemetStartingStats();
                setup_template.element = dataValue[0];
                setup_template.strength = float.Parse(dataValue[1]);
                setup_template.vitality = float.Parse(dataValue[2]);
                setup_template.magic = float.Parse(dataValue[3]);
                setup_template.multipler = float.Parse(dataValue[4]);

                statsLisitng.Add(setup_template);
            }

            readData.Close();
        }
    }

    public static ElemetStartingStats GetStatsWithElementBonus(string element)
    {
        foreach (ElemetStartingStats e in statsLisitng)
            if (e.element == element) return e;

        return statsLisitng[statsLisitng.ToArray().Length - 1];
    }
    #endregion

    #region EXTENSION: PLAY-EVENT REWARDS
    public static void LoadPlayEventRewards(string jsonData)
    {
        if (jsonData != string.Empty) allStoredPlayEventRewards = new PlayEventArray().GetBundle(jsonData);
        int count = 1;

        if (allStoredPlayEventRewards.data != null)
        {
            foreach (PlayEventRewardData playEvent in allStoredPlayEventRewards.data)
            {
                if (GetVersionNumber(StartMenu_Script.thisMenu.version) >= GetVersionNumber(playEvent.version))
                {
                    Debug.Log(count + ": Event Opend: " + playEvent.itemName + " x" + playEvent.maxObtain);
                    Debug.Log(count + " - Played Obatinable Count: " + playEvent.playRequirement);
                    Debug.Log(count + " - Event Obtainable Type: " + (playEvent.repeatableReward ? "Unlimited" : "Limited"));
                    count++;
                }
            }

            Debug.Log("Total Play Event: " + (count - 1));
        }
        else
            Debug.Log("No play event is available");
    }

    public static PlayEventRewardData[] GetEventRewardArray()
    {
        return allStoredPlayEventRewards.data;
    }

    public static int GetVersionNumber(string version)
    {
        int versionCount = 0;
        VersionControlArray versionControl = new VersionControlArray().GetAllData(
            PlayerPrefs.GetString("VersionControl_PlayEvent", string.Empty));

        foreach (string serachedVersion in versionControl.versions)
        {
            versionCount++;
            if (version == serachedVersion) return versionCount;
        }

        return 0;
    }
    #endregion

    #region EXTENSION: EXCHANGE POINTS
    public static bool GetCodeNotReedemable(string code_access)
    {
        MeloMelo_Local.LocalLoad_DataManagement loadForUse = new MeloMelo_Local.LocalLoad_DataManagement(LoginPage_Script.thisPage.GetUserPortOutput(),
            "StreamingAssets/LocalData/MeloMelo_LocalSave_InGameProgress");

        try
        {
            string[] dataForUse = loadForUse.GetLocalJsonFile(GetLocalFileExchangeHistory, true).Split("/");
            foreach (string data in dataForUse)
            {
                if (data != string.Empty)
                {
                    DataPackStructure dataPack = JsonUtility.FromJson<DataPackStructure>(data);
                    if (dataPack.unqiueCode == code_access) return true;
                }
            }
        }
        catch { }

        return false;
    }

    public static bool GetItemIsStackable(string itemName)
    {
        foreach (ItemData item in Resources.LoadAll<ItemData>("Database_Item"))
            if (itemName == item.itemName) return item.stackable;

        return false;
    }
    #endregion
}

public static class MeloMelo_PlayerSettings
{
    public enum LoginType { GuestLogin, TempPass }

    public static readonly string GetBGM_ValueKey = "BGM_DataKey";
    public static readonly string GetSE_ValueKey = "SE_DataKey";
    public static readonly string GetAudioMute_ValueKey = "AudioMute_DataKey";
    public static readonly string GetAudioVoice_ValueKey = "AudioVoice_DataKey";

    public static readonly string GetCharacterAnimation_ValueKey = "CharacterAnimation_DataKey";
    public static readonly string GetEnemyAnimation_ValueKey = "EnemyAnimation_DataKey";
    public static readonly string GetInterfaceAnimation_ValueKey = "InterfaceAnimation_DataKey";
    public static readonly string GetDamageIndicatorA_ValueKey = "DamageIndicator_A_DataKey";
    public static readonly string GetDamageIndicatorB_ValueKey = "DamageIndicator_B_DataKey";

    public static readonly string GetFrameRateLimit_ValueKey = "FrameLimit_DataKey";
    public static readonly string GetPeformanceOptimize_ValueKey = "PeformanceOptimize_DataKey";
    public static readonly string GetUnitHealthOnCharacter_ValueKey = "CharacterHealth_Type_DataKey";
    public static readonly string GetUnitHealthOnEnemy_ValueKey = "EnemyHealth_Type_DataKey";

    public static readonly string GetSpeedMeter_ValueKey = "SpeedMeter_DataKey";
    public static readonly string GetAirGuide_ValueKey = "AirGuide_DataKey";
    public static readonly string GetJudgeTimingOffset_ValueKey = "JudgeTiming_DataKey";
    public static readonly string GetFacnyMovement_ValueKey = "FancyMovement_DataKey";

    public static readonly string GetAutoSaveProgress_ValueKey = "SaveProgress_Auto_DataKey";
    public static readonly string GetAutoSavePlaySettings_ValueKey = "SavePlaySettings_Auto_DataKey";
    public static readonly string GetAutoSaveGameSettings_ValueKey = "SaveGameSettings_Auto_DataKey";

    #region LOG CAHCE
    public static bool GetLocalUserAccount()
    {
        return Application.internetReachability != NetworkReachability.NotReachable && PlayerPrefs.HasKey("AccountSync");
    }

    public static void GetLocalUserAccount(int options)
    {
        switch (options)
        {
            case 1:
                PlayerPrefs.SetInt("AccountSync", 1);
                break;

            case 2:
                PlayerPrefs.DeleteKey("AccountSync");
                break;

            default:
                break;
        }
    }
    #endregion

    #region MISC: Web Request
    public static string GetWebServerUrl() { return PlayerPrefs.GetString("GameWeb_URL", string.Empty); }
    public static void UpdateWebServerUrl(string url) { PlayerPrefs.SetString("GameWeb_URL", url); }
    public static void DisableWebServerCache() { PlayerPrefs.DeleteKey("GameWeb_URL"); }
    #endregion
}

public static class MeloMelo_CharacterInfo_Settings
{
    public static void UnlockCharacter(string className) { PlayerPrefs.SetInt(className + "_Unlock_Code", 1); }
    public static void LockedCharacter(string className) { PlayerPrefs.SetInt(className + "_Unlock_Code", 0); }
    public static bool GetCharacterStatus(string className) { return PlayerPrefs.GetInt(className + "_Unlock_Code", 0) == 1; }

    public static void SetUsageOfSecondarySkill(string className, int slot_id) { PlayerPrefs.SetInt(className + "_SecondarySkill_Id", slot_id); ; }
    public static int GetUsageOfSecondarySkill(string className) { return PlayerPrefs.GetInt(className + "_SecondarySkill_Id", 0); }

    public static void SetCharacterChosenSelection(bool condition) { PlayerPrefs.SetInt("Character_ChosenSelection", condition ? 1 : 0); }
    public static bool GetCharacterChosenSelection() { return PlayerPrefs.GetInt("Character_ChosenSelection", 0) == 1; }
}

public static class MeloMelo_ExtraStats_Settings
{
    public static void IncreaseStrengthStats(string className, int value) { PlayerPrefs.SetInt(className + "_Permant_STR", PlayerPrefs.GetInt(className + "_Permant_STR", 0) + value); }
    public static void IncreaseVitalityStats(string className, int value) { PlayerPrefs.SetInt(className + "_Permant_VIT", PlayerPrefs.GetInt(className + "_Permant_VIT", 0) + value); }
    public static void IncreaseMagicStats(string className, int value) { PlayerPrefs.SetInt(className + "_Permant_MAG", PlayerPrefs.GetInt(className + "_Permant_MAG", 0) + value); }
    public static void IncreaseBaseHealth(string className, int value) { PlayerPrefs.SetInt(className + "_Permant_HP", PlayerPrefs.GetInt(className + "_Permant_HP", 0) + value); }
    public static void IncreaseBaseSpeed(string className, int value) { PlayerPrefs.SetInt(className + "_Permant_SPD", PlayerPrefs.GetInt(className + "_Permant_SPD", 0) + value); }

    public static int  GetExtraStrengthStats(string className) { return PlayerPrefs.GetInt(className + "_Permant_STR", 0); }
    public static int GetExtraVitaltyStats(string className) { return PlayerPrefs.GetInt(className + "_Permant_VIT", 0); }
    public static int GetExtraMagicStats(string className) { return PlayerPrefs.GetInt(className + "_Permant_MAG", 0); }
    public static int GetExtraBaseHealth(string className) { return PlayerPrefs.GetInt(className + "_Permant_HP", 0); }
    public static int GetExtraBaseSpeed(string className) { return PlayerPrefs.GetInt(className + "_Permant_SPD", 0); }

    public static void SetMasteryPoint(string className, int value) { PlayerPrefs.SetInt(className + "_Mastery_Points", value); }
    public static void SetRebirthPoint(string className, int value) { PlayerPrefs.SetInt(className + "_Rebirth_Points", value); }
    public static int GetRebirthPoint(string className) { return PlayerPrefs.GetInt(className + "_Rebirth_Points", 0); }
    public static int GetMasteryPoint(string className) { return PlayerPrefs.GetInt(className + "_Mastery_Points", 0); }

    public static void GiveOutDamageResistance(int value) { PlayerPrefs.SetInt("MISC_Character_DamageResist", value); }
    public static int ResistAgainstDamageResistance(int orginalDamage)
    {
        int newDamage = orginalDamage + PlayerPrefs.GetInt("MISC_Character_DamageResist", 0);
        PlayerPrefs.SetInt("MISC_Character_DamageResist", 0);
        return newDamage;
    }
    public static void GiveOutBonusBaseDamage(int value) { PlayerPrefs.SetInt("MISC_Character_ExtraBaseDamage", value); }
    public static int GetBonusDamage() { return PlayerPrefs.GetInt("MISC_Character_ExtraBaseDamage", 0); }
}

public static class MeloMelo_SkillData_Settings
{
    public static void UnlockSkill(string skillName) { PlayerPrefs.SetInt(skillName + "_Unlock_Code", 1); }
    public static void LockedSkill(string skillName) { PlayerPrefs.SetInt(skillName + "_Unlock_Code", 0); }
    public static bool CheckSkillStatus(string skillName) { return PlayerPrefs.GetInt(skillName + "_Unlock_Code", 0) == 1; }

    public static void LearnSkill(string skillName) { PlayerPrefs.SetInt(skillName + "_Grade_Code", 1); }
    public static void UpgradeSkill(string skillName) { PlayerPrefs.SetInt(skillName + "_Grade_Code", PlayerPrefs.GetInt(skillName + "_Grade_Code", 0) + 1); }
    public static int CheckSkillGrade(string skillName) { return PlayerPrefs.GetInt(skillName + "_Grade_Code", 0); }
}

public static class MeloMelo_UnitData_Settings
{
    public static void SetSuccessHitOfAllEnemyTarget(int value, int typeOfTarget = -1)
    {
        switch (typeOfTarget)
        {
            case 1:
            case 6:
                PlayerPrefs.SetInt("MISC_Character_EnemySuccessfulHit", value);
                break;

            case 5:
                PlayerPrefs.SetInt("MISC_Character_EnemyAttackSuccessHit", value);
                break;

            default:
                PlayerPrefs.SetInt("MISC_Character_AllSuccessfulHit", value);
                break;
        }
    }
    public static void SetSuccessPickItem(int value) { PlayerPrefs.SetInt("MISC_Character_TotalOPickCount", value); }

    public static int GetSuccessHitOfAllEnemyTarget(int typeOfTarget = -1)
    {
        switch (typeOfTarget)
        {
            case 1:
            case 6:
                return PlayerPrefs.GetInt("MISC_Character_EnemySuccessfulHit", 0);

            case 5:
                return PlayerPrefs.GetInt("MISC_Character_EnemyAttackSuccessHit", 0);

            default:
                return PlayerPrefs.GetInt("MISC_Character_AllSuccessfulHit", 0);
        }
    }
    public static int GetSuccessPickItem() { return PlayerPrefs.GetInt("MISC_Character_TotalOPickCount", 0); }
}

public static class MeloMelo_ItemUsage_Settings
{
    private static List<VirtualItemDatabase> allStoredItem = null;

    #region ITEM: OVERVIEW
    public static void ClearItems()
    {
        if (allStoredItem != null) allStoredItem.Clear();
    }

    public static void ImportItems(VirtualItemDatabase item)
    {
        if (allStoredItem == null) allStoredItem = new List<VirtualItemDatabase>();
        if (item.amount > 0) allStoredItem.Add(item);
    }

    public static void ExportItems()
    {
        MeloMelo_Local.LocalSave_DataManagement exportData = new MeloMelo_Local.LocalSave_DataManagement
            (
                LoginPage_Script.thisPage.GetUserPortOutput(),
                "StreamingAssets/LocalData/MeloMelo_LocalSave_InGameProgress"
            );

        exportData.SelectFileForActionWithUserTag(MeloMelo_GameSettings.GetLocalFileVirtualItemData);

        foreach (VirtualItemDatabase item in allStoredItem)
            exportData.SaveVirtualItemFromPlayer(item.itemName, item.amount, true);
    }

    public static VirtualItemDatabase[] GetActiveItems()
    {
        return allStoredItem != null ? allStoredItem.ToArray() : null;
    }

    public static VirtualItemDatabase GetActiveItem(string itemName)
    {
        if (allStoredItem != null)
        {
            foreach (VirtualItemDatabase itemStorage in allStoredItem)
                if (itemStorage.itemName == itemName) return itemStorage;
        }

        return new VirtualItemDatabase();
    }

    public static void OverwriteActiveItem(string itemName, int amount)
    {
        bool itemFound = false;

        if (allStoredItem != null)
        {
            for (int id = 0; id < allStoredItem.ToArray().Length; id++)
            {
                if (allStoredItem[id].itemName == itemName)
                {
                    VirtualItemDatabase newUpdateItem = new VirtualItemDatabase();
                    newUpdateItem.itemName = itemName;
                    newUpdateItem.amount = allStoredItem[id].amount + amount;

                    allStoredItem.RemoveAt(id);
                    if (newUpdateItem.amount > 0) allStoredItem.Insert(id, newUpdateItem);
                    itemFound = true;
                    break;
                }
            }

            if (!itemFound) allStoredItem.Add(new VirtualItemDatabase(itemName, amount));
        }
    }
    #endregion

    #region ITEM: MANAGEMENT
    public static void SetItemUsed(string itemName) 
    { 
        int amount = PlayerPrefs.GetInt(itemName + "_VirtualItem_Unsaved_Used", 0);
        PlayerPrefs.SetInt(itemName + "_VirtualItem_Unsaved_Used", amount + 1);
    }

    public static int GetItemUsed(string itemName) { return PlayerPrefs.GetInt(itemName + "_VirtualItem_Unsaved_Used", 0); }
    public static string[] GetAllItemUsed() 
    {
        try
        {
            List<string> itemListing = new List<string>();
            MeloMelo_Local.LocalLoad_DataManagement loadForCheck = new MeloMelo_Local.LocalLoad_DataManagement(
                LoginPage_Script.thisPage.GetUserPortOutput(), "StreamingAssets/LocalData/MeloMelo_LocalSave_InGameProgress");

            foreach (string itemData in loadForCheck.GetLocalJsonFileToArray(LoginPage_Script.thisPage.GetUserPortOutput() + "_" +
                MeloMelo_GameSettings.GetLocalFileVirtualItemData))
            {
                if (itemData != string.Empty)
                {
                    VirtualItemDatabase item = new VirtualItemDatabase().GetItemData(itemData);
                    if (PlayerPrefs.GetInt(item.itemName + "_VirtualItem_Unsaved_Used", 0) > 0)
                        itemListing.Add(item.itemName);
                }
            }

            return itemListing.ToArray();
        }
        catch { }

        return null;
    }
    #endregion

    #region ITEM EFFECT: BOOST VALUE
    public static int GetExpBoost(string className) { return PlayerPrefs.GetInt(className + "_EXP_BOOST", 0); }
    public static int GetExpBoostByMultiply(string className) { return PlayerPrefs.GetInt(className + "_EXP_BOOST_2", 0); }
    public static int GetPowerBoost(string className) { return PlayerPrefs.GetInt(className + "_POWER_BOOST", 0); }
    public static int GetPowerBoostByMultiply(string className) { return PlayerPrefs.GetInt(className + "_POWER_BOOST_2", 0); }
    #endregion
}

public static class MeloMelo_Economy
{
    public enum CurrencyType { Credits, MagicStone, HonorCoin };
    public static string[] currencyTagInArray = { "Credits", "MagicStone", "Honor Coin" };

    public static List<MarathonExchangeContent> exchangeContentOfMarathon = null;

    public static void ImportMarathonExchangeContent(string json_format)
    {
        if (json_format != null)
        {
            MarathonExchangeArray exchangeArray = new MarathonExchangeArray().GetExchangeList(json_format);
            exchangeContentOfMarathon = new List<MarathonExchangeContent>();

            exchangeContentOfMarathon.AddRange(exchangeArray.marathonContent);
            Debug.Log("Total Exchange Content (Marathon) : " + exchangeContentOfMarathon.ToArray().Length + " Loaded!");
        }
    }
}

public static class MeloMelo_Adventure
{
    public static string[] routeStatus = { "Not Started", "Completed!", "In Progress" };

    public static void MarkRouteCleared(int storyId, int routeId) { PlayerPrefs.SetInt("RoutePlayableStatus_" + storyId + routeId, 1); }
    public static bool GetRouteCleared(int storyId, int routeId) { return PlayerPrefs.GetInt("RoutePlayableStatus_" + storyId + routeId, 0) == 1; }
    public static bool GetPreviousRouteCleared(int storyId, int routeId) { return PlayerPrefs.GetInt("RoutePlayableStatus_" + storyId +(routeId - 1), 0) == 1; }
}
