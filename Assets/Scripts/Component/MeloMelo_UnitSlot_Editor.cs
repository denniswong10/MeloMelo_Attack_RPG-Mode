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

        if (PlayerPrefs.HasKey("MarathonPermit")) UpdateQuestCondition();
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
        EnemyLevel.text = "LV. " + BeatConductor.thisBeat.Music_Database.Insert_Enemy[PlayerPrefs.GetInt("BattleDifficulty_Mode", 1) - 1].level;
    }

    #region MAIN (Quest Updater)
    public void UpdateQuestCondition()
    {
        int clearingType = info != null ? (int)info.clearingType : info2.conditionType - 1;

        switch (clearingType)
        {
            case 0:
                WriteQuestScore(ReadTotalQuestScore(true) + (GameManager.thisManager.get_score1 != null ?
                    (int)GameManager.thisManager.get_score1.get_score : 0));
                break;

            case 1:
                int life = GameManager.thisManager.getJudgeWindow.get_perfect2 * PlayerPrefs.GetInt("Critical_Perfect_Deduct", 0) +
                    GameManager.thisManager.getJudgeWindow.get_perfect * PlayerPrefs.GetInt("Perfect_Deduct", 0) +
                    GameManager.thisManager.getJudgeWindow.get_bad * PlayerPrefs.GetInt("Bad_Deduct", 0) +
                    GameManager.thisManager.getJudgeWindow.get_miss * PlayerPrefs.GetInt("Miss_Deduct", 0);

                WriteQuestScore(ReadTotalQuestScore(true) + life);
                break;

            case 2:
                string[] splitValue = info != null ? info.clearingValue.Split('/') : info2.condition_data.Split(",");
                int previousScore = ReadTotalQuestScore(true);

                switch (int.Parse(splitValue[0]))
                {
                    case 1:
                        WriteQuestScore(previousScore + GameManager.thisManager.getJudgeWindow.get_perfect2);
                        break;

                    case 2:
                        WriteQuestScore(previousScore + GameManager.thisManager.getJudgeWindow.get_perfect);
                        break;

                    case 3:
                        WriteQuestScore(previousScore + GameManager.thisManager.getJudgeWindow.get_bad);
                        break;

                    default:
                        WriteQuestScore(previousScore + GameManager.thisManager.getJudgeWindow.get_miss);
                        break;
                }
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

    #region MISC (Slot Description)
    public string GetMarathonQuestSlot(int slotIndex)
    {
        switch (slotIndex)
        {
            case 1:
                int currentScore = PlayerPrefs.GetInt("Marathon_Quest_Score", 0);

                if (info != null)
                {
                    switch (info.clearingType)
                    {
                        case MarathonInfo.ClearedMethod.ScoreAchiever:
                            int totalStage = info.Difficultylevel.Length;
                            return ScoreAchieverCondition(totalStage);

                        case MarathonInfo.ClearedMethod.Life:
                            return LifeChallengeCondition(int.Parse(info.clearingValue) - currentScore,
                                int.Parse(info.clearingValue) - currentScore > 0);

                        case MarathonInfo.ClearedMethod.Judgement:
                            string[] splitvalue = info.clearingValue.Split('/');
                            int totalCount = int.Parse(splitvalue[1]);
                            return JudgementCondition(int.Parse(splitvalue[0]), totalCount);

                        default:
                            return "No Condition";
                    }
                }
                else
                {
                    switch (info2.conditionType)
                    {
                        case 1:
                            int totalStage = info2.track_difficulty.Length;
                            return ScoreAchieverCondition(totalStage);

                        case 2:
                            return LifeChallengeCondition(int.Parse(info2.condition_data.Split(",")[4]) - currentScore,
                                int.Parse(info2.condition_data.Split(",")[4]) - currentScore > 0);

                        case 3:
                            string[] splitvalue = info2.condition_data.Split(',');
                            int totalCount = int.Parse(splitvalue[1]);
                            return JudgementCondition(int.Parse(splitvalue[0]), totalCount);

                        default:
                            return "No Condition";
                    }
                }

            default:
                return "Marathon Mode\n\nStage " + PlayerPrefs.GetInt("MarathonChallenge_MCount", 1) + " / " +
                    (info != null ? info.Difficultylevel.Length : info2.track_difficulty.Length);
                        //Resources.Load<MarathonInfo>(PlayerPrefs.GetString("Marathon_Assigned_Task", string.Empty)).Difficultylevel.Length;
        }
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

    #region COMPONENT (Quest Condition)
    private string ScoreAchieverCondition(int totalStage)
    {
        int currentScore = ReadTotalQuestScore(false);
        int result = MeloMelo_GameSettings.GetScoreRankStructure(info != null ? info.clearingValue : info2.condition_data).score;
        FinalRecordForQuestCondition(currentScore >= (totalStage * result));

        if (currentScore >= (totalStage * result)) return "Score Reached:\n\nCLEARED!";
        else return "Score Reached:\n\n" + currentScore + " / " + (totalStage * result);
    }

    private string LifeChallengeCondition(int result, bool isStillProgress)
    {
        FinalRecordForQuestCondition(isStillProgress);
        if (isStillProgress) return "Life Challenge:\n\n" + result;
        else return "Life Challenge:\n\nFAILED!";
    }

    private string JudgementCondition(int judge_index, int numberOfJudge)
    {
        int currentScore = ReadTotalQuestScore(false);
        string resultValue;

        switch (judge_index)
        {
            case 1:
                resultValue = "Critical Cahllenge\n\n";
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

        FinalRecordForQuestCondition(currentScore < numberOfJudge);
        if (currentScore > numberOfJudge) return resultValue + "FAILED!";
        else return resultValue + currentScore + " / " + numberOfJudge;
    }
    #endregion
}
