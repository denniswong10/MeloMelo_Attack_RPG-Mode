using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class RewardTerminal
{
    public enum RewardType { Item, Track }
    public RewardType typeOfReward;
    public string rewardName;
}

[System.Serializable]
public class SlotQuestLog
{
    public string logTitle;
    public int id;

    public enum SlotType { Story, Quest, Step, Goals };
    public SlotType mySlotTypte;
    public bool isOpen;
}

[System.Serializable]
public class FragmentInfo
{
    public int[] numberOfSteps;
    public MusicScore[] Quest_Stage;
    public SlotQuestLog[] myQuestLog;
}

[CreateAssetMenu(fileName = "StoryInfo", menuName = "StoryData")]
public class StoryInfo : ScriptableObject
{
    public string StoryTitle;
    public Texture StoryBG;

    public enum StoryType { Main, Side, Event };
    public RewardTerminal[] storyEndRewards;

    public FragmentInfo[] Stage;
}
