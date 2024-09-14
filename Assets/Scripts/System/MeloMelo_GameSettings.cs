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

public static class MeloMelo_GameSettings
{
    public enum LoginType { GuestLogin, TempPass }

    public const string CloudSaveSetting_MainProgress = "savelog_AchievementData";
    public const string CloudSaveSetting_BattleProgress = "savelog_BattleProgressData";
    public const string CloudSaveSetting_PointsData = "savelog_PointsData";
    public const string CloudSaveSetting_AcccountSettings = "savelog_PlayerSettings";
    public const string CloudSaveSetting_GameplaySettings = "savelog_GameplaySettings";
    public const string CloudSaveSetting_CharacterSettings = "savelog_CharacterSettings";
    public const string CloudSaveSetting_ProfileData = "savelog_Profile_Setup";
    public const string CloudSaveSetting_SelectionData = "savelog_SelectionBase";
    public const string CloudSaveSetting_CharacterStats = "savelog_CharacterProgressStats";

    public const string GetLocalFileMainProgress = "savelog_AchievementData.txt";
    public const string GetLocalFileBattleProgress = "savelog_BattleProgressData.txt";
    public const string GetLocalFileAccountSettings = "savelog_PlayerSettings.txt";
    public const string GetLocalFileGameplaySettings = "savelog_GameplaySettings.txt";
    public const string GetLocalFileCharacterSettings = "savelog_CharacterSettings.txt";
    public const string GetLocalFileCharacterStats = "savelog_CharacterProgressStats.txt";
    public const string GetLocalFilePointData = "savelog_PointsData.txt";
    public const string GetLocalFileProfileData = "savelog_Profile_Setup.txt";
    public const string GetLocalFileCharacterData = "savelog_Character_Info.txt";
    public const string GetLocalFileSelectionData = "savelog_SelectionBase.txt";
    public const string GetLocalFileChartLegacy = "ChartData_1";
    public const string GetLocalFileChartOld = "ChartData_2";
    public const string GetLocalFileChartNew = "ChartData_3";

    public static int GetRecentStatusRemark = 0;

    public static string[] GetJudgeFastOrLate = { "Early", "Perfect", "Late" };
    public static string[] GetMainJudgeStats = { "Critical", "Perfect", "Bad", "Miss" };
    public static int[] GetJudgeFastOrLate_Value = new int[3];
    public static int JudgeTimingIndex = 0;
    public static bool targetJudgeTrigger = false;

    public static int[] FastNLate_Critcal = new int[3];
    public static int[] FastNLate_Perfect = new int[3];
    public static int[] FastNLate_Bad = new int[3];

    public static float[] GetInputDelayValue = { 0.008f, 0.005f, 0, 0.01f, 0.02f };
    public static int GetInputDelaySelection = 2;

    private static List<MeloMelo_ScoreRankSetup> scoreRankListing = null;
    private static List<MeloMelo_StatusRemarkData> statusRemark = null;
    public static List<MeloMelo_TrackSelectionData> selectionLisitng = new List<MeloMelo_TrackSelectionData>();

    #region GERENAL SETUP
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

    #region PLAYER SETUP
    public static void UpdateCharacterProfile()
    {
        foreach (ClassBase character in Resources.LoadAll<ClassBase>("Character_Data"))
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

    public static int GetLocalTrackSelectionLastVisited(string area, int options)
    {
        foreach (MeloMelo_TrackSelectionData selection in selectionLisitng)
            if (selection.areaTitle == area)
                return options == 1 ? selection.trackIndex : selection.difficulty;

        return 1;
    }
    #endregion
}
