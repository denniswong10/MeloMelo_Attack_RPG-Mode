using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SlotQuestLog
{
    public bool slotOpen;
    public bool clearSlot;

    public enum SlotType { Quest, Battle };
    public SlotType mySlotTypte;
}

[System.Serializable]
public class FragmentInfo
{
    public int numberOfSteps;
    public string HeadTitle;
    public string StoryDetail;
    public MusicScore Quest_Stage;

    public SlotQuestLog[] myQuestLog;

    public enum EpisodeManagement { Lock, Unlock, Clear, NoStatus };

    [Header("Option Variable")]
    public EpisodeManagement myEpi_Manage = EpisodeManagement.Lock;
}

[CreateAssetMenu(fileName = "StoryInfo", menuName = "StoryData")]
public class StoryInfo : ScriptableObject
{
    public string StoryTitle;
    public Texture StoryBG;

    public enum StoryType { Main, Side, Event };
    public StoryType S_Type = StoryType.Main;

    public FragmentInfo[] Stage;
}
