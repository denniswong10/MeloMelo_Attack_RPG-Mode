using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "MarathonInfo", menuName = "ChallengeInfo")]
public class MarathonInfo : ScriptableObject
{
    [Header("Setup")]
    public string title;
    public string[] Difficultylevel = new string[4];
    public enum DifficultyList { NORMAL = 1, HARD, ULTIMATE }
    public DifficultyList[] DifficultyType = new DifficultyList[4];

    public enum ClearedMethod { ScoreAchiever, Life, Judgement }
    [Header("Condition")]
    public ClearedMethod clearingType;
    public string clearingValue;

    #region COMPONENT
    public string GetConditionDetails()
    {
        switch (clearingType)
        {
            case ClearedMethod.ScoreAchiever:
                return "Cleared this challenge with a rank of " + MeloMelo_GameSettings.GetScoreRankStructure(clearingValue).rank + " or higher";

            case ClearedMethod.Life:
                return "Survive this challenge with " + clearingValue + " life";

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
