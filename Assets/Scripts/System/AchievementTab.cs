using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConditionState
{
    public enum Condition_Filler { MainScore, NumberOfTimes }
    public Condition_Filler fill_Type;
    public string value;
}

[CreateAssetMenu(fileName = "AchievementTab", menuName = "Achievement_Transcript")]
public class AchievementTab : ScriptableObject
{
    [Header("Setup")]
    public string title;
    public string description;
    public string titleReward_name;

    public enum Title_Tier { White, Bronze, Silver, Gold, Rhondonite }
    public enum Reward_Condition { RatePoint, PlayedCount, RankAchiever, StatusAchiever, SeasonCompletion, PointAchiever, BattleWonAchiever, Custom_Setup }
    [Header("Tier Sorter")]
    public Title_Tier Tier;
    public Reward_Condition useConditionReward;

    [Header("Condition Setup: Data Container")]
    public ConditionState[] fill_Array;

    [Header("Custom Setup: Use Command Data")]
    public string conditionOperate;

    #region MAIN
    public ConditionState GetConditionData(ConditionState.Condition_Filler _type)
    {
        for (int data_index = fill_Array.Length; data_index > 0; data_index--)
        {
            if (_type == fill_Array[data_index - 1].fill_Type)
                return fill_Array[data_index - 1];
        }

        return null;
    }
    #endregion
}
