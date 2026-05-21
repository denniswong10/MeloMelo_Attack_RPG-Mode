using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeloMelo_UnitSlot_Editor : MonoBehaviour
{
    [Header("Setup: Level Display")]
    [SerializeField] private Text CharacterLevel;
    [SerializeField] private Text EnemyLevel;

    [Header("Setup: Slot Icon Display")]
    [SerializeField] private Texture EnemyIcon;

    [Header("Setup: Marathon Quest Condition")]
    private MarathonInfo info;
    private BuildInChallengeInfo info2;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetString("Marathon_Assigned_Task", string.Empty) != "CustomList") info = Resources.Load<MarathonInfo>(PlayerPrefs.GetString("Marathon_Assigned_Task", string.Empty));
        else
        {
            info = null;
            info2 = new BuildInChallengeInfo();
            info2 = MeloMelo_ExtensionContent_Settings.LoadMarathonDetail(PlayerPrefs.GetInt("MarathonInstanceNumber", 0));
        }

        if (PlayerPrefs.HasKey("MarathonPermit")) SetQuestCondition(true);
        Invoke("StandingBy", 0.5f);
    }

    private int GetAllyLevel()
    {
        int totalLevel = 0;
        MeloMelo_RPGEditor.StatsDistribution allyStats = new MeloMelo_RPGEditor.StatsDistribution();
        allyStats.load_Stats();

        foreach (Character_Base_Data character in allyStats.slot_Stats) if (character != null) totalLevel += character.level;
        return totalLevel;
    }

    private void StandingBy()
    {
        CharacterLevel.text = "LV. " + GetAllyLevel();
        EnemyLevel.text = "LV. " + BeatConductor.thisBeat.Music_Database.Insert_Enemy[MeloMelo_GameSettings.GetAreaDifficultyMode() - 1].level;
    }

    #region MAIN (Quest Updater)
    public int latestLocalScore { get; private set; }
    public int currentLocalScore { get; private set; }
    public List<string> judgementData { get; private set; }

    // Setup or Update: Quest Condition
    public void SetQuestCondition(bool updateLatestScore)
    {
        // Setup only: Option
        if (updateLatestScore)
        {
            latestLocalScore = ReadTotalQuestScore(true);
            currentLocalScore = 0;
            judgementData = null;
        }

        // Get quest updater between local or global
        if (info != null) LocalMarathonConditionUpdater(info.clearingType);
        else GlobalMarathonConditionUpdater(info2.conditionType - 1);
    }

    public void FinalizeQuestCondition()
    {
        // Set final score to the next stage
        WriteQuestScore(latestLocalScore + currentLocalScore);
    }

    // Condition Logic: Quest Condition
    private void LocalMarathonConditionUpdater(MarathonInfo.ClearedMethod clearingType)
    {
        // Find default condition -> Addons to find others difficulty settings
        bool difficultyActive = PlayerPrefs.GetInt("MarathonPlay_DifficultyMode", 1) != 1;
        int checkNewId = difficultyActive ? PlayerPrefs.GetInt("ExtraAddons_MarathonClearingType", 0) : (int)clearingType;

        switch (checkNewId)
        {
            case (int)MarathonInfo.ClearedMethod.ScoreAchiever:
                currentLocalScore = GameManager.thisManager.get_score1 != null ? (int)GameManager.thisManager.get_score1.get_score : 0;
                break;

            case (int)MarathonInfo.ClearedMethod.Life:
                currentLocalScore = GameManager.thisManager.getJudgeWindow.get_perfect2 * PlayerPrefs.GetInt("Critical_Perfect_Deduct", 0) +
                    GameManager.thisManager.getJudgeWindow.get_perfect * PlayerPrefs.GetInt("Perfect_Deduct", 0) +
                    GameManager.thisManager.getJudgeWindow.get_bad * PlayerPrefs.GetInt("Bad_Deduct", 0) +
                    GameManager.thisManager.getJudgeWindow.get_miss * PlayerPrefs.GetInt("Miss_Deduct", 0);
                break;

            case (int)MarathonInfo.ClearedMethod.Judgement:
                JudgeConditionFirstTimeSetup(
                    difficultyActive ? PlayerPrefs.GetString("ClearingValue_Addons", string.Empty).Split('/') : 
                        info != null ? info.clearingValue.Split('/') : info2.condition_data.Split(",")
                    );

                JudgeConditionUpdate();
                break;
        }

        Debug.Log("Update successful condition : LOCAL");
    }

    private void GlobalMarathonConditionUpdater(int clearingTypeById)
    {
        switch (clearingTypeById)
        {
            case 2:
                LocalMarathonConditionUpdater(MarathonInfo.ClearedMethod.Life);
                break;

            default:
                break;
        }

        Debug.Log("Update successful condition : GLOBAL");
    }

    private void JudgeConditionFirstTimeSetup(string[] clearingValue)
    {
        if (judgementData == null)
        {
            judgementData = new List<string>();

            foreach (string judgeValue in clearingValue)
                judgementData.Add(judgeValue);
        }
    }

    private void JudgeConditionUpdate()
    {
        switch (int.Parse(judgementData[0]))
        {
            case 1:
                currentLocalScore = GameManager.thisManager.getJudgeWindow.get_perfect2;
                break;

            case 2:
                currentLocalScore = GameManager.thisManager.getJudgeWindow.get_perfect;
                break;

            case 3:
                currentLocalScore = GameManager.thisManager.getJudgeWindow.get_bad;
                break;

            default:
                currentLocalScore = GameManager.thisManager.getJudgeWindow.get_miss;
                break;
        }
    }
    #endregion

    #region COMPONENT (Quest Resulting)
    private void WriteQuestScore(int score)
    {
        PlayerPrefs.SetInt("Marathon_Quest_Score", score);
    }

    private int ReadTotalQuestScore(bool previousScore)
    {
        return PlayerPrefs.GetInt(previousScore ? "Marathon_Quest_ScoreAddons" : "Marathon_Quest_Score", 0);
    }

    private void FinalRecordForQuestCondition(bool hasCleared)
    {
        PlayerPrefs.SetInt("Marathon_Quest_Result", hasCleared ? 1 : 0);
    }
    #endregion

    #region MISC (Quest Panel PinBoard)
    public string GetMarathonStageIndicator()
    {
        return "Marathon Mode\n\nStage " + 
            PlayerPrefs.GetInt("MarathonChallenge_MCount", 1) + " / " +
                (info != null ? info.Difficultylevel.Length : info2.track_difficulty.Length);
    }

    public string GetMarathonQuestCheckerIndicator()
    {
        // Find default condition -> Addons to find others difficulty settings
        bool isDifficultyModify = PlayerPrefs.GetInt("MarathonPlay_DifficultyMode", 1) != 1;

        int checkNewId = isDifficultyModify ?
            PlayerPrefs.GetInt("ExtraAddons_MarathonClearingType", 0) : (int)info.clearingType;

        string verifyClearingValue = isDifficultyModify ? 
            PlayerPrefs.GetString("ClearingValue_Addons", string.Empty) : info.clearingValue;

        if (info != null) return LocalQuestChecker(checkNewId, verifyClearingValue);
        else return GlobalQuestChecker(info2.conditionType);
    }

    public Texture GetMarathonProfileIcon(int slotIndex)
    {
        switch (slotIndex)
        {
            case 1:
                return EnemyIcon;

            default:
                return Resources.Load<Texture>("Character_Data/" + PlayerPrefs.GetString("CharacterFront", "NA"));
        }
    }
    #endregion

    #region COMPONENT (Quest Panel PinBoard)
    private string LocalQuestChecker(int clearing_type_id, string clearing_value)
    {
        switch ((MarathonInfo.ClearedMethod)clearing_type_id)
        {
            case MarathonInfo.ClearedMethod.ScoreAchiever:
                int totalStage = info.Difficultylevel.Length;
                return ScoreAchieverCondition(totalStage, clearing_value);

            case MarathonInfo.ClearedMethod.Life:
                int final_value = int.Parse(clearing_value);

                return LifeChallengeCondition(final_value - latestLocalScore - currentLocalScore,
                    final_value - latestLocalScore - currentLocalScore > 0);

            case MarathonInfo.ClearedMethod.Judgement:
                if (judgementData != null)
                {
                    int totalCount = int.Parse(judgementData[1]);
                    return JudgementCondition(int.Parse(judgementData[0]), totalCount);
                }
                else
                    return "???";

            default:
                return "No Condition";
        }
    }

    private string GlobalQuestChecker(int clearingTypeById)
    {
        switch (clearingTypeById)
        {
            case 1:
                int totalStage = info2.track_difficulty.Length;
                return ScoreAchieverCondition(totalStage, info.clearingValue);

            case 2:
                return LifeChallengeCondition(int.Parse(info2.condition_data.Split(",")[4]) - currentLocalScore,
                    int.Parse(info2.condition_data.Split(",")[4]) - currentLocalScore > 0);

            case 3:
                string[] splitvalue = info2.condition_data.Split(',');
                int totalCount = int.Parse(splitvalue[1]);
                return JudgementCondition(int.Parse(splitvalue[0]), totalCount);

            default:
                return "No Condition";
        }
    }
    #endregion

    #region COMPONENT (Quest Condition)
    private string ScoreAchieverCondition(int totalStage, string cleared_condition)
    {
        int result = MeloMelo_GameSettings.GetScoreRankStructure(cleared_condition).score;
        FinalRecordForQuestCondition(latestLocalScore + currentLocalScore >= (totalStage * result));

        if (latestLocalScore + currentLocalScore >= (totalStage * result)) return "Score Reached:\n\nCLEARED!";
        else return "Score Reached:\n\n" + (latestLocalScore + currentLocalScore) + " / " + (totalStage * result);
    }

    private string LifeChallengeCondition(int result, bool isStillProgress)
    {
        FinalRecordForQuestCondition(isStillProgress);
        if (isStillProgress) return "Life Challenge:\n\n" + result;
        else return "Life Challenge:\n\nFAILED!";
    }

    private string JudgementCondition(int judge_index, int numberOfJudge)
    {
        string[] resultValue = { "Critical Challenge\n\n",  "Perfect Challenge\n\n", "No Bad Challenge\n\n", "No Miss Challenge\n\n" };
        int checkValid_index = judge_index == resultValue.Length ? (resultValue.Length - 1) : (judge_index - 1);

        FinalRecordForQuestCondition(latestLocalScore + currentLocalScore < numberOfJudge);
        if (latestLocalScore + currentLocalScore > numberOfJudge) return resultValue + "FAILED!";
        else return resultValue[checkValid_index] + (latestLocalScore + currentLocalScore) + " / " + numberOfJudge;
    }
    #endregion
}
