using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProfileView_Menu : MonoBehaviour
{
    private int totalTrack = 0;
    private int totalTrackOnlyUlimate = 0;
    private int[] totalCountCleared;
    private int[] totalBattleCount;

    [SerializeField] private Text PlayerName;
    [SerializeField] private Text AccountID;

    [SerializeField] private Text Version;

    [SerializeField] private Text[] AchievementTracks;
    [SerializeField] private Text[] BattleAchieve;
    [SerializeField] private GameObject AccountSettings;

    private ResourceRequest loadAssetForUse;
    private List<int> areaLength;
    private bool calculateAreaLengthDone;
    [SerializeField] private GameObject LoadingIcon;

    // Start is called before the first frame update
    void Start()
    {
        totalCountCleared = new int[2];
        totalBattleCount = new int[3];

        areaLength = new List<int>();
        calculateAreaLengthDone = false;

        StartCoroutine(GetAllAreaOfLength());
        StartCoroutine(GetCommonScoreCalculate());
        StartCoroutine(GetExtraScoreCalculate());

        for (int total = 0; total < totalCountCleared.Length; total++) 
            StartCoroutine(GetTrackProgressOnAchievement(total));

        for (int total = 0; total < totalBattleCount.Length; total++)
            StartCoroutine(GetTrackProgressOnBattleSuccess(total));

        GetPlayerNameContent(GuestLogin_Script.thisScript.get_localPlayer.GetUserLocalByPlayerId());
        GetAccountIDContent();
        GetVersionDetail();
    }

    void Update()
    {
        if (loadAssetForUse != null)
        {
            LoadingIcon.SetActive(!loadAssetForUse.isDone);
        }
    }

    #region MAIN
    public void OpenAccountSettings(bool active)
    {
        AccountSettings.SetActive(active);
    }

    public void PreviousOption()
    {
        SceneManager.LoadScene("Options");
    }

    public void RevealUniqueKey()
    {
        AccountSettings.transform.GetChild(1).GetComponent<Text>().text = "Guest Name: " +
            GuestLogin_Script.thisScript.get_entry + "\nUnique ID: " + 
            GuestLogin_Script.thisScript.get_localPlayer.GetUserLocalByUniqueId();

        AccountSettings.transform.GetChild(2).GetComponent<Button>().interactable = false;
    }
    #endregion

    #region COMPONENT (ACCOUNT IDENTIFY)
    private void GetPlayerNameContent(string id)
    {
        PlayerName.text = "Player Name: " + id;
    }

    private void GetAccountIDContent()
    {
        AccountID.text = "Account ID: " + GetComponent<Auto_Authenticate_Config>().GetAccountID();
    }
    #endregion

    #region SETUP
    private void GetCommonBattleStatus(int difficulty)
    {
        // Value of Beginner, Intermidate, Advance Battle
        BattleAchieve[difficulty].text = totalBattleCount[difficulty] + "/" + (totalTrack * 3);
    }

    private void GetCommonTrackStatus(int difficulty)
    {
        // Value of Normal, Hard Track Status
        AchievementTracks[difficulty].text = totalCountCleared[difficulty] + "/" + totalTrack;
    }

    private void GetUltimateAchievementStatus(int value)
    {
        // Value of Ultimate Track Status
        AchievementTracks[2].text = "0/" + value;
        BattleAchieve[2].text = "0/" + (value * 3);
    }

    private void GetVersionDetail()
    {
        Version.text = "Installed Version: " + StartMenu_Script.thisMenu.version;
    }

    private IEnumerator GetAllAreaOfLength()
    {
        AreaInfo info = null;
        int count = 0;

        // Count total number of area available
        for (int season = 0; season < StartMenu_Script.thisMenu.get_seasonNum; season++)
        {
            // Start area count
            do
            {
                count++;
                loadAssetForUse = Resources.LoadAsync<AreaInfo>("Database_Area/Season" + season + "/A" + count);
                yield return new WaitUntil(() => loadAssetForUse.isDone);

                info = loadAssetForUse.asset as AreaInfo;
            } while (info != null);

            // Reset area count
            areaLength.Add(count);
            count = 0;
        }

        calculateAreaLengthDone = true;
    }
    #endregion

    #region COMPONENT (TRACK COUNTER)
    private IEnumerator GetCommonScoreCalculate()
    {
        yield return new WaitUntil(() => calculateAreaLengthDone);

        // Get Overall Track
        for (int season = 0; season < StartMenu_Script.thisMenu.get_seasonNum; season++)
        {
            for (int track = 0; track < areaLength[season]; track++)
            {
                loadAssetForUse = Resources.LoadAsync<AreaInfo>("Database_Area/Season" + season + "/A" + (track + 1));
                yield return new WaitUntil(() => loadAssetForUse.isDone);

                AreaInfo area = loadAssetForUse.asset as AreaInfo;
                if (area != null) totalTrack += area.totalMusic;

                // Refresh content
                for (int difficulty = 0; difficulty < totalCountCleared.Length; difficulty++)
                {
                    GetCommonBattleStatus(difficulty);
                    GetCommonTrackStatus(difficulty);
                }
            }
        }
    }

    private IEnumerator GetExtraScoreCalculate()
    {
        yield return new WaitUntil(() => calculateAreaLengthDone);

        List<AreaInfo> areaList = new List<AreaInfo>();
        AreaInfo area = null;
        int index = 0;

        // Adding area list
        for (int season = 0; season < StartMenu_Script.thisMenu.get_seasonNum; season++)
        {
            do
            {
                index++;
                loadAssetForUse = Resources.LoadAsync<AreaInfo>("Database_Area/Season" + season + "/A" + index);
                yield return new WaitUntil(() => loadAssetForUse.isDone);

                area = loadAssetForUse.asset as AreaInfo;
                if (area != null) areaList.Add(area);
            } while (area != null);

            index = 0;
        }

        // Get Overall Track
        for (int season = 0; season < StartMenu_Script.thisMenu.get_seasonNum; season++)
        {
            for (int areaL = 0; areaL < areaLength[season]; areaL++)
            {
                loadAssetForUse = Resources.LoadAsync<AreaInfo>("Database_Area/Season" + season + "/A" + (areaL + 1));
                yield return new WaitUntil(() => loadAssetForUse.isDone);
                area = loadAssetForUse.asset as AreaInfo;

                if (area != null)
                {
                    for (int selection = 0; selection < area.totalMusic; selection++)
                    {
                        loadAssetForUse = Resources.LoadAsync<MusicScore>("Database_Area/" + area.AreaName + "/M" + (selection + 1));
                        yield return new WaitUntil(() => loadAssetForUse.isDone);

                        MusicScore content = loadAssetForUse.asset as MusicScore;
                        if (content != null && content.UltimateAddons) GetUltimateAchievementStatus(totalTrackOnlyUlimate++);
                    }
                }
            }
        }
    }
    #endregion

    #region COMPONENT (PROGRESS CHECKER)
    private IEnumerator GetTrackProgressOnAchievement(int difficulty)
    {
        yield return new WaitUntil(() => calculateAreaLengthDone);

        AreaInfo area = null;

        for (int season = 0; season < StartMenu_Script.thisMenu.get_seasonNum; season++)
        {
            for (int areaL = 0; areaL < areaLength[season]; areaL++)
            {
                loadAssetForUse = Resources.LoadAsync<AreaInfo>("Database_Area/Season" + season + "/A" + (areaL + 1));
                yield return new WaitUntil(() => loadAssetForUse.isDone);
                area = loadAssetForUse.asset as AreaInfo;

                if (area != null)
                {
                    for (int selection = 0; selection < area.totalMusic; selection++)
                    {
                        loadAssetForUse = Resources.LoadAsync<MusicScore>("Database_Area/" + area.AreaName + "/M" + (selection + 1));
                        yield return new WaitUntil(() => loadAssetForUse.isDone);

                        MusicScore content = loadAssetForUse.asset as MusicScore;
                        if (content != null)
                        {
                            if (PlayerPrefs.GetInt(content.Title + "_BattleRemark_" + difficulty, 6) < 5)
                            {
                                totalCountCleared[difficulty - 1]++;
                                GetCommonTrackStatus(difficulty - 1);
                            }
                        }

                    }
                }
            }
        }
    }

    private IEnumerator GetTrackProgressOnBattleSuccess(int difficulty)
    {
        yield return new WaitUntil(() => calculateAreaLengthDone);

        AreaInfo area = null;

        for (int season = 0; season < StartMenu_Script.thisMenu.get_seasonNum; season++)
        {
            for (int areaL = 0; areaL < areaLength[season]; areaL++)
            {
                loadAssetForUse = Resources.LoadAsync<AreaInfo>("Database_Area/Season" + season + "/A" + (areaL + 1));
                yield return new WaitUntil(() => loadAssetForUse.isDone);
                area = loadAssetForUse.asset as AreaInfo;

                if (area != null)
                {
                    for (int selection = 0; selection < area.totalMusic; selection++)
                    {
                        loadAssetForUse = Resources.LoadAsync<MusicScore>("Database_Area/" + area.AreaName + "/M" + (selection + 1));
                        yield return new WaitUntil(() => loadAssetForUse.isDone);

                        MusicScore content = loadAssetForUse.asset as MusicScore;
                        if (content != null)
                        {
                            for (int playDiff = 0; playDiff < totalCountCleared.Length; playDiff++)
                            {
                                if (PlayerPrefs.GetString(content.Title + "_SuccessBattle_" + playDiff + difficulty, "F") == "T")
                                {
                                    totalBattleCount[difficulty - 1]++;
                                    GetCommonBattleStatus(difficulty - 1);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    #endregion
}
