using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class JudgementAddons
{
    public enum JudgeHeadLine { Critical = 1, Perfect, Bad, Miss }
    public JudgeHeadLine judgeTitle;
    public int judgeCount;
}

[System.Serializable]
public class TrackDetails
{
    public string title;
    public string areaName;
    public enum DifficultyList { NORMAL = 1, HARD, ULTIMATE }
    public DifficultyList DifficultyType;
}

[CreateAssetMenu(fileName = "MarathonInfo", menuName = "ChallengeInfo")]
public class MarathonInfo : ScriptableObject
{
    [Header("Setup")]
    public string title;
    public TrackDetails[] trackList = new TrackDetails[4];
    public string[] Difficultylevel = new string[4];

    public enum ClearedMethod { ScoreAchiever, Life, Judgement }
    [Header("Condition")]
    public ClearedMethod clearingType;
    public string clearingValue;
    public JudgementAddons[] conditionAddons;

    #region COMPONENT
    private string FindJudgeCount(int index)
    {
        foreach (JudgementAddons judgeData in conditionAddons)
        {
            if ((int)judgeData.judgeTitle == index)
            {
                if (judgeData.judgeCount > 0) return "-" + judgeData.judgeCount;
                else return "+" + (-judgeData.judgeCount).ToString();
            }
        }

        return "-0";
    }

    public string GetConditionDetails()
    {
        switch (clearingType)
        {
            case ClearedMethod.ScoreAchiever:
                return "Cleared this challenge with a rank of " + MeloMelo_GameSettings.GetScoreRankStructure(clearingValue).rank + " or higher";

            case ClearedMethod.Life:
                return "Survive this challenge with " + clearingValue + " life\n"
                    + "Critical (" + FindJudgeCount(1) + "), Perfect (" + FindJudgeCount(2) + "), Bad (" + FindJudgeCount(3) + "), Miss (" + FindJudgeCount(4) + ")";

            default:
                string[] valueSplit = clearingValue.Split('/');
                return "Cleared this challenge with " + valueSplit[1] + " or less " + GetJudgementFliter(int.Parse(valueSplit[0]));
        }
    }

    public string GetJudgementFliter(int index)
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
    #endregion
}
