using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MarathonDifficulty_Data
{
    public enum ClearedMethod { ScoreAchiever, Life, Judgement }
    [Header("Condition")]
    public ClearedMethod clearingType;
    public string clearingValue;
    public JudgementAddons[] conditionAddons;
}

[CreateAssetMenu(fileName = "MarathonAddtionalMode", menuName = "Marathon_Additional_Mode")]
public class MarathonAdditionalMode : ScriptableObject
{
    [Header("Setup")]
    public string title;
    public MarathonDifficulty_Data[] additional_difficulty_data;
    public int defaultModeSet;


    private string FindJudgeCount(int index, int mode_value)
    {
        foreach (JudgementAddons judgeData in additional_difficulty_data[mode_value].conditionAddons)
        {
            if ((int)judgeData.judgeTitle == index)
            {
                if (judgeData.judgeCount > 0) return "-" + judgeData.judgeCount;
                else return "+" + (-judgeData.judgeCount).ToString();
            }
        }

        return "-0";
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

    public string GetConditionDetails(int mode_value)
    {
        if (additional_difficulty_data[mode_value] != null)
        {
            MarathonDifficulty_Data data = additional_difficulty_data[mode_value];
            PlayerPrefs.SetInt("ExtraAddons_MarathonClearingType", (int)data.clearingType);

            switch (data.clearingType)
            {
                case MarathonDifficulty_Data.ClearedMethod.ScoreAchiever:
                    return ConfirmationOfClearingValue(
                        "Cleared this challenge with a rank of " +
                            MeloMelo_GameSettings.GetScoreRankStructure(data.clearingValue).rank + " or higher",

                        data.clearingValue
                        );

                case MarathonDifficulty_Data.ClearedMethod.Life:
                    return ConfirmationOfClearingValue(
                        "Survive this challenge with " + data.clearingValue + " life\n"
                            + "Critical (" + FindJudgeCount(1, mode_value) + "), Perfect (" + FindJudgeCount(2, mode_value) + "), " +
                                "   Bad (" + FindJudgeCount(3, mode_value) + "), Miss (" + FindJudgeCount(4, mode_value) + ")",

                        data.clearingValue
                        );

                default:
                    string[] valueSplit = data.clearingValue.Split('/');

                    return ConfirmationOfClearingValue(
                        "Cleared this challenge with " + valueSplit[1] + " or less " + GetJudgementFliter(int.Parse(valueSplit[0])),
                        data.clearingValue
                        );
            }
        }
        else
        {
            SetDefaultDifficulty();
            return GetConditionDetails(PlayerPrefs.GetInt("MarathonPlay_DifficultyMode", 1));
        }
    }

    #region MISC
    private string ConfirmationOfClearingValue(string description, string value)
    {
        PlayerPrefs.SetString("ClearingValue_Addons", value);
        return description;
    }

    private void SetDefaultDifficulty()
    {
        PlayerPrefs.SetInt("MarathonPlay_DifficultyMode", defaultModeSet);
    }
    #endregion
}
