using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "MarathonInfo", menuName = "ChallengeInfo")]
public class MarathonInfo : ScriptableObject
{
    public string title;
    public enum ChallengeLevel { Rookie, Amateur, Master, GrandMaster, Emperor, God, GameMaster };
    public ChallengeLevel thisChallenge;

    public int[] pre_initDifficulty = new int[4];
    public MusicScore[] ChallengeRef = new MusicScore[4];

    public string[] Difficultylevel = new string[4];

    public bool challengeClear;
    public float clearPercentage;

    [Header("Condition_Check")]
    public bool RankChallenge;
    public string setRank;

    public bool RemarkChallenge;
    public int setRemark;

    public bool hitChallenge;
    public int setJudge;
    public int setAmount;

    string GetRemark_method(int index)
    {
        switch (index)
        {
            case 1:
                return "only perfect";

            case 2:
                return "all eliminate or above";

            case 3:
                return "missless or above";

            case 4:
                return "cleared or above";

            default:
                return "any state";
        }
    }

    string GetHit_method(int index)
    {
        switch (index)
        {
            case 1:
                return setAmount + " or less perfect";

            case 2:
                return setAmount + " or less bad";

            case 3:
                return setAmount + " or less miss";

            default:
                return setAmount + " or less perfect/bad/miss";
        }
    }

    public string Decode_ObjectiveDes()
    {
        string header = "- Clear all stage ";
        string condition = 
            setRank != "--" ? "with " + setRank + " or above" : "with any rank";
        string condition2 = "with " + GetRemark_method(setRemark);
        string condition3 = "with " + GetHit_method(setJudge);

        if (!RankChallenge && !RemarkChallenge && !hitChallenge || challengeClear)
        {
            GameObject.Find("NextBtn").GetComponent<Button>().interactable = false;
            return "- Objective Completed! -";
        }
        else
        {
            GameObject.Find("NextBtn").GetComponent<Button>().interactable = true;
            return (RankChallenge ? header + condition + "\n" : "") + (RemarkChallenge ? header + condition2 + "\n" : "")
           + (hitChallenge ? header + condition3 + "\n" : "");
        }
    }

    public int Decode_Level()
    {
        switch(thisChallenge)
        {
            case ChallengeLevel.GameMaster:
                return 7;

            case ChallengeLevel.Amateur:
                return 2;

            case ChallengeLevel.Master:
                return 3;

            case ChallengeLevel.GrandMaster:
                return 4;

            case ChallengeLevel.Emperor:
                return 5;

            case ChallengeLevel.God:
                return 6;

            default:
                return 1;
        }
    }

    public string Decode_LevelByName()
    {
        switch (thisChallenge)
        {
            case ChallengeLevel.GameMaster:
                return "GameMaster";

            case ChallengeLevel.Amateur:
                return "Amateur";

            case ChallengeLevel.Master:
                return "Master";

            case ChallengeLevel.GrandMaster:
                return "GrandMaster";

            case ChallengeLevel.Emperor:
                return "Emperor";

            case ChallengeLevel.God:
                return "God";

            default:
                return "Rookie";
        }
    }
}
