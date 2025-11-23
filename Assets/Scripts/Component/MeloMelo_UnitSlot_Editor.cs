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

        foreach (ClassBase character in allyStats.slot_Stats) if (character) totalLevel += character.level;
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

    private void LocalMarathonConditionUpdater(MarathonInfo.ClearedMethod clearingType)
    {
        switch (clearingType)
        {
            case MarathonInfo.ClearedMethod.ScoreAchiever:
                currentLocalScore = GameManager.thisManager.get_score1 != null ? (int)GameManager.thisManager.get_score1.get_score : 0;
                break;

            case MarathonInfo.ClearedMethod.Life:
                currentLocalScore = GameManager.thisManager.getJudgeWindow.get_perfect2 * PlayerPrefs.GetInt("Critical_Perfect_Deduct", 0) +
                    GameManager.thisManager.getJudgeWindow.get_perfect * PlayerPrefs.GetInt("Perfect_Deduct", 0) +
                    GameManager.thisManager.getJudgeWindow.get_bad * PlayerPrefs.GetInt("Bad_Deduct", 0) +
                    GameManager.thisManager.getJudgeWindow.get_miss * PlayerPrefs.GetInt("Miss_Deduct", 0);
                break;

            case MarathonInfo.ClearedMethod.Judgement:
                if (judgementData == null)
                {
                    judgementData = new List<string>();

                    if (info != null)
                        foreach (string judgeValue in info.clearingValue.Split('/'))
                            judgementData.Add(judgeValue);

                    else
                        foreach (string judgeValue in info2.condition_data.Split(","))
                            judgementData.Add(judgeValue);
                }

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
                break;
        }

        Debug.Log("Update successful condition : LOCAL");
    }

    private void GlobalMarathonConditionUpdater(int clearingTypeById)
    {
        Debug.Log("Update successful condition : GLOBAL");
    }

    public void SetQuestCondition(bool updateLatestScore)
    {
        // Get current score condition through marathon play
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
        if (info != null) return LocalQuestChecker(info.clearingType);
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
    private string LocalQuestChecker(MarathonInfo.ClearedMethod clearingType)
    {
        switch (clearingType)
        {
            case MarathonInfo.ClearedMethod.ScoreAchiever:
                int totalStage = info.Difficultylevel.Length;
                return ScoreAchieverCondition(totalStage);

            case MarathonInfo.ClearedMethod.Life:
                return LifeChallengeCondition(int.Parse(info.clearingValue) - latestLocalScore - currentLocalScore,
                    int.Parse(info.clearingValue) - latestLocalScore - currentLocalScore > 0);

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
                return ScoreAchieverCondition(totalStage);

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
    private string ScoreAchieverCondition(int totalStage)
    {
        int result = MeloMelo_GameSettings.GetScoreRankStructure(info != null ? info.clearingValue : info2.condition_data).score;
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
        string resultValue;

        switch (judge_index)
        {
            case 1:
                resultValue = "Critical Challenge\n\n";
                break;

            case 2:
                resultValue = "Perfect Challenge\n\n";
                break;

            case 3:
                resultValue = "No Bad Challenge\n\n";
                break;

            default:
                resultValue = "No Miss Challenge\n\n";
                break;
        }

        FinalRecordForQuestCondition(latestLocalScore + currentLocalScore < numberOfJudge);
        if (latestLocalScore + currentLocalScore > numberOfJudge) return resultValue + "FAILED!";
        else return resultValue + (latestLocalScore + currentLocalScore) + " / " + numberOfJudge;
    }
    #endregion
}
