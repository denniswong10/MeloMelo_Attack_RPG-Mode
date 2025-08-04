using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MeloMelo_Network;
using MeloMelo_RatingMeter;
using MeloMelo_RPGEditor;
using MeloMelo_Local;

public class ResultMenu_Script : MonoBehaviour
{
    public static ResultMenu_Script thisRes;

    // Allocate: Score Transfer
    private int highscore = 0;
    private int techScore = 0;

    // Allocate: General Use
    public GameObject RecordBreaker;
    public GameObject SaveIcon;
    public RawImage BG;

    [Header("Output: Result")]
    [SerializeField] private GameObject ScoreResult;
    [SerializeField] private GameObject PointResult;
    [SerializeField] private GameObject TrackListResult;
    public GameObject PromptMessage;

    // Allocate: Stats Database
    private StatsDistribution stats = new StatsDistribution();
    private string ResMelo = string.Empty;

    [Header("Output: Music Database")]
    [SerializeField] private RawImage Result_CoverImage;
    [SerializeField] private Text Result_Artist;
    [SerializeField] private Text Result_Title;
    [SerializeField] private GameObject Result_DifficultyMeter;

    [Header("Feature: RatePointToggle")]
    [SerializeField] private GameObject[] ratePointMarker;
    [SerializeField] private Button[] unlock_ProcessingBtn;

    private string userInput;
    private RatePointToggleZone rateZone;

    void Start()
    {
        thisRes = this;
        Screen.SetResolution(1360, 768, false);

        ResMelo = PlayerPrefs.GetString("Resoultion_Melo", string.Empty);
        try { userInput = GuestLogin_Script.thisScript.get_entry; } catch { userInput = LoginPage_Script.thisPage.get_user; }

        rateZone = GetComponent<RatePointToggleZone>();

        // Transfer score data for result 
        highscore = PlayerPrefs.GetInt(BeatConductor.thisBeat.Music_Database.Title + "_score" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 0);
        techScore = PlayerPrefs.GetInt(BeatConductor.thisBeat.Music_Database.Title + "_techScore" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1) + PlayerPrefs.GetInt("BattleDifficulty_Mode", 1), 0);

        LoadAllResult();
        UpdateMusicInfo();
        RevertNoteSpeedCheck();

        // Cursor
        if (!Cursor.visible) Cursor.visible = true;
    }

    void Update()
    {
        rateZone.GetZoneFunctionProgram();
    }

    #region MAIN
    void RevertNoteSpeedCheck()
    {
        int previousSpeed = PlayerPrefs.GetInt("Temp_NoteSpeed", 0);
        PlayerPrefs.SetInt("NoteSpeed", previousSpeed);
    }

    // Load Result Information
    void UpdateMusicInfo()
    {
        int levelMeter = (int)PlayerPrefs.GetFloat("difficultyLevel_play2", 0);
        string level = (PlayerPrefs.GetInt("DifficultyLevel_valve", 1) == 1 ? PlayerPrefs.GetString("Difficulty_Normal_selectionTxt", "?") : (PlayerPrefs.GetInt("DifficultyLevel_valve", 1) == 2 ? PlayerPrefs.GetString("Difficulty_Hard_selectionTxt", "?") : PlayerPrefs.GetString("Difficulty_Ultimate_selectionTxt", "?")));

        if (!GameManager.thisManager.DeveloperMode)
        {
            BG.texture = PlayerPrefs.HasKey("Mission_Played") ? Resources.Load<Texture>("Background/BG1C") : 
                PlayerPrefs.HasKey("MarathonPermit") ? Resources.Load<Texture>("Background/BG11") :
                    PreSelection_Script.thisPre.get_AreaData.BG;

            // Gerenal Information
            Result_CoverImage.texture = BeatConductor.thisBeat.Music_Database.Background_Cover;
            Result_Artist.text = "[ " + BeatConductor.thisBeat.Music_Database.ArtistName + " ]";
            Result_Title.text = BeatConductor.thisBeat.Music_Database.Title;

            // Info: Difficulty Status
            switch (PlayerPrefs.GetInt("DifficultyLevel_valve"))
            {
                case 1:
                    string[] difficultyArray = { "[NORMAL PLUS] Lv. ", "[NORMAL] Lv.  " };
                    int[] maxLevel = { 5, 0 };
                    Color[] colorArray = { Color.blue, Color.blue };

                    AdvanceModifyDifficultyMeter(difficultyArray, maxLevel, colorArray, levelMeter, level);
                    break;

                case 3:
                    ModifyDifficultyMeter("[ULTIMATE] Lv. ", level, new Color(1, 0.4f, 0));
                    break;

                default:
                    string[] difficultyArray2 = { "[EXPERT] Lv. ", "[HARD PLUS] Lv. ", "[HARD] Lv. " };
                    int[] maxLevel2 = { 15, 10, 5 };
                    Color[] colorArray2 = { new Color(1, 0.09f, 0.87f), new Color(0.47f, 0, 1), Color.red };

                    AdvanceModifyDifficultyMeter(difficultyArray2, maxLevel2, colorArray2, levelMeter, level);
                    break;
            }

            // Rate and Score Information
            RatePointIndicator border = new RatePointIndicator("RateStatus");
            border.UpdateColor_Border(userInput);

            ScoreResult.transform.GetChild(1).GetComponent<Text>().text = Result_GetCurrentScore();
            ScoreResult.transform.GetChild(2).GetComponent<Text>().text = "[ " + highscore + " ]";
            ScoreResult.transform.GetChild(3).GetComponent<Text>().text = "( " + Result_GetScoreImprove() + " )";

            PointResult.transform.GetChild(1).GetComponent<Text>().text = Result_GetCurrentPoint(false) + "/" + Result_GetMaxPoint();
            PointResult.transform.GetChild(2).GetComponent<Text>().text = Result_GetCurrentPoint(true) + "/" + Result_GetMaxPoint();
            PointResult.transform.GetChild(3).GetComponent<Text>().text = "( " + Result_GetPointImprove() + " )";

            ScoreLevel_Rank();
            Invoke("GetViewStatus_User", 0.5f);

            // Rate Point Toggle
            Invoke("GetRatePointToggle", 1);
        }
    }

    void LoadAllResult()
    {
        stats.load_Stats();

        // Load Viewer
        for (int i = 0; i < stats.slot_Stats.Length; i++)
        {
            StatsManage_Database database = new StatsManage_Database(stats.slot_Stats[i].name);
            stats.slot_Stats[i].UpdateCurrentStats(false);

            int experienceAfterBoost = MeloMelo_ItemUsage_Settings.GetExpBoost(stats.slot_Stats[i].name) +
                (MeloMelo_ItemUsage_Settings.GetExpBoostByMultiply(stats.slot_Stats[i].name) > 0 ?
                    MeloMelo_ItemUsage_Settings.GetExpBoostByMultiply(stats.slot_Stats[i].name) * PlayerPrefs.GetInt("Temp_Experience", 0) :
                        PlayerPrefs.GetInt("Temp_Experience", 0));

            if (stats.slot_Stats[i].name != "None")
            {
                stats.slot_Stats[i].experience += PlayerPrefs.HasKey("MarathonPermit") ||
                database.GetCharacterStatus(stats.slot_Stats[i].level).GetExperience < 0
                ? 0 : experienceAfterBoost;
                stats.slot_Stats[i].UpdateCurrentStats(true);

                int checkLevel;
                if (database.GetCharacterStatus(stats.slot_Stats[i].level).GetExperience > 0)
                {
                    do
                    {
                        checkLevel = stats.slot_Stats[i].level;
                        stats.slot_Stats[i].CheckLeveling(database.GetCharacterStatus(checkLevel).GetExperience);
                    } while (checkLevel != stats.slot_Stats[i].level);

                    GameObject.Find("Slot" + (i + 1) + "_CharInfo").transform.GetChild(3).GetComponent<Text>().text = "EXP: "
                        + stats.slot_Stats[i].experience + "/" + database.GetCharacterStatus(stats.slot_Stats[i].level).GetExperience +
                        " (+" + (PlayerPrefs.HasKey("MarathonPermit") ? 0 : experienceAfterBoost) + ")";
                }
                else
                    GameObject.Find("Slot" + (i + 1) + "_CharInfo").transform.GetChild(3).GetComponent<Text>().text = string.Empty;

                GameObject.Find("Slot" + (i + 1) + "_CharInfo").transform.GetChild(2).GetComponent<Text>().text = "- " + stats.slot_Stats[i].characterName + " -";
                GameObject.Find("Slot" + (i + 1) + "_CharInfo").transform.GetChild(4).GetComponent<Text>().text = "LEVEL: " + stats.slot_Stats[i].level;

                GameObject.Find("Slot" + (i + 1) + "_CharInfo").transform.GetChild(1).GetComponent<Image>().enabled = true;
                GameObject.Find("Slot" + (i + 1) + "_CharInfo").transform.GetChild(1).GetComponent<Image>().sprite = stats.slot_Stats[i].icon;
            }
        }

        if (GameManager.thisManager.get_WinAlert) { GameObject.Find("Battle Status").GetComponent<Text>().text = "BATTLE SUCCESS!"; }
        else { GameObject.Find("Battle Status").GetComponent<Text>().text = "BATTLE FAILED!"; }

        GameObject.Find("Score2").GetComponent<Text>().text = "ADVENTURER RANK: " + (PlayerPrefs.GetInt("UpperScoreTech", 0) == 0 ? "NO STAR" :
            PlayerPrefs.GetInt("UpperScoreTech", 0) + " STAR");
        //+ " (" + TechScore_Ref() + ")";

        GameObject.Find("Score2_Slider").GetComponent<Slider>().maxValue = 12;// techScore;
        GameObject.Find("Score2_Slider").GetComponent<Slider>().value = PlayerPrefs.GetInt("UpperScoreTech", 0);
        // PlayerPrefs.GetInt("TechScore", 0);

        GameObject.Find("BP").GetComponent<Text>().text = "BATTLE STREAK RATE: " + BattleRate_Ref().ToString("0.00") + "%";

        // Load Result 
        GameObject.Find("Perfect2").GetComponent<Text>().text = "Critical Perfect: " + GameManager.thisManager.getJudgeWindow.get_perfect2;
        GameObject.Find("Perfect").GetComponent<Text>().text = "Perfect: " + GameManager.thisManager.getJudgeWindow.get_perfect;
        GameObject.Find("Bad").GetComponent<Text>().text = "Bad: " + GameManager.thisManager.getJudgeWindow.get_bad;
        GameObject.Find("Miss").GetComponent<Text>().text = "Miss: " + PlayerPrefs.GetInt("Miss_count", 0);

        GameObject.Find("MaxCombo_Value").GetComponent<Text>().text = GameManager.thisManager.getJudgeWindow.getMaxCombo + " / " +
            GameManager.thisManager.getJudgeWindow.getOverallCombo;

        // Second Hand Judge
        string[] FastNLate_Judge = { "Critical", "Perfect", "Bad" };
        for (int judge = 0; judge < FastNLate_Judge.Length; judge++)
            GameObject.Find("FastNLateStatus_" + FastNLate_Judge[judge]).GetComponent<Text>().text = Critical_FastLate_Judge(judge);

        GameObject.Find("TrackStatus").GetComponent<Text>().text = "Stage Status: " +
            (MeloMelo_GameSettings.GetStatusByAchievement(MeloMelo_GameSettings.GetRecentStatusRemark) != null
            ? MeloMelo_GameSettings.GetStatusByAchievement(
            MeloMelo_GameSettings.GetRecentStatusRemark).remark : "--");

        // Rate Point Contribution
        BuildUpRatePointContribution();
    }

    private string Critical_FastLate_Judge(int index)
    {
        switch (index)
        {
            case 0:
                return MeloMelo_GameSettings.FastNLate_Critcal[0] + " / " + MeloMelo_GameSettings.FastNLate_Critcal[1] + " / " + MeloMelo_GameSettings.FastNLate_Critcal[2];

            case 1:
                return MeloMelo_GameSettings.FastNLate_Perfect[0] + " / " + MeloMelo_GameSettings.FastNLate_Perfect[2];

            case 2:
                return MeloMelo_GameSettings.FastNLate_Bad[0] + " / " + MeloMelo_GameSettings.FastNLate_Bad[2];

            default:
                return string.Empty;
        }
    }

    public void ChangeViewAs(Text text)
    {
        PointResult.SetActive(text.name == "Score");
        ScoreResult.SetActive(text.name == "Point");

        GameObject.Find("Button_Extra").transform.GetChild(0).gameObject.name = text.name == "Score" ? "Point" : "Score";
        GameObject.Find("Button_Extra").transform.GetChild(0).GetComponent<Text>().text = "View as: " + text.name;

        if (PlayerPrefs.GetInt("Temp_" + text.name + "Result", 0) > 0) RecordBreaker.SetActive(true);
        else RecordBreaker.SetActive(false);
    }

    #region COMPONENT
    private string Result_GetCurrentScore()
    {
        return GameManager.thisManager.get_score1.get_score.ToString("0000000");
    }

    private string Result_GetPreviousScore()
    {
        return PlayerPrefs.GetInt(BeatConductor.thisBeat.Music_Database.Title + "_score" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 0).ToString("0000000");
    }

    private string Result_GetCurrentPoint(bool highscore)
    {
        string pointTitleName = BeatConductor.thisBeat.Music_Database.Title + "_point" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        int highscoreInPoint = PlayerPrefs.GetInt(pointTitleName, 0);

        if (highscore && highscoreInPoint < GameManager.thisManager.getJudgeWindow.getOverallCombo * 3)
            return PlayerPrefs.GetInt(pointTitleName, 0).ToString();

        else if (highscoreInPoint > GameManager.thisManager.getJudgeWindow.getOverallCombo * 3)
        {
            PlayerPrefs.SetInt(pointTitleName, (int)GameManager.thisManager.get_point.get_score);
            return PlayerPrefs.GetInt(pointTitleName, 0).ToString();
        }

        return ((int)GameManager.thisManager.get_point.get_score).ToString();
    }

    private string Result_GetMaxPoint()
    {
        return (GameManager.thisManager.getJudgeWindow.getOverallCombo * 3).ToString();
    }

    private void AdvanceModifyDifficultyMeter(string[] difficultyName, int[] limit, Color[] colorArray, int level, string levelTxt)
    {
        for (int meter = 0; meter < limit.Length; meter++)
        {
            if (level > limit[meter])
            {
                ModifyDifficultyMeter(difficultyName[meter], levelTxt, colorArray[meter]);
                break;
            }
        }
    }

    private void ModifyDifficultyMeter(string difficultyName, string level, Color border)
    {
        Result_DifficultyMeter.transform.GetChild(0).GetComponent<Text>().text = difficultyName + level;
        Result_DifficultyMeter.GetComponent<RawImage>().color = border;
    }
    #endregion

    #region MISC
    private string Result_GetScoreImprove()
    {
        float i = GameManager.thisManager.get_score1.get_score - highscore;
        PlayerPrefs.SetInt("Temp_ScoreResult", (int)i);

        if (i == 0) { return "+0"; }
        if (i > 0) { return "+" + (int)i; }
        else { return ((int)i).ToString(); }
    }

    private string Result_GetPointImprove()
    {
        int i = (int)GameManager.thisManager.get_point.get_score - PlayerPrefs.GetInt(BeatConductor.thisBeat.Music_Database.Title + "_point" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 0);
        PlayerPrefs.SetInt("Temp_PointResult", i);

        if (i == 0) { return "+0"; }
        if (i > 0) { return "+" + i; }
        else { return i.ToString(); }
    }

    private void GetViewStatus_User()
    {
        PointResult.SetActive(false);
        ScoreResult.SetActive(true);

        GameObject.Find("Button_Extra").transform.GetChild(0).gameObject.name = "Score";
        GameObject.Find("Button_Extra").transform.GetChild(0).GetComponent<Text>().text = "View as: Score";

        if (PlayerPrefs.GetInt("Temp_ScoreResult", 0) > 0) RecordBreaker.SetActive(true);
    }
    #endregion
    #endregion

    #region MAIN (New Rate Toggle Mode)
    private void GetRatePointToggle()
    {
        // Get data through toggle
        string data = "totalRatePoint";
        string output = "UserRatePointToggle";
        string user = LoginPage_Script.thisPage.GetUserPortOutput();

        // Final rate point output
        PlayerPrefs.SetInt(user + data, FinalizeRatePointValue(data));

        // Get rate point toggle over to new raise
        if (PlayerPrefs.GetInt(user + data, 0) != PlayerPrefs.GetInt(user + output, 0))
            rateZone.ProcessToRateZone(user, data);
        else
            CloseRatePointToogle(0);
    }

    private int FinalizeRatePointValue(string data)
    {
        // Retrieve data to calculate
        int currentT = PlayerPrefs.GetInt(LoginPage_Script.thisPage.GetUserPortOutput() + data, 0);
        int newRatePoint = 0;

        // Calculate the possible point that given
        newRatePoint += TrackListingDistribution.thisList.CalcuateTotalRatePoint() - currentT;

        // Update all status through the interface
        GameObject.Find("RateStatus").transform.GetChild(0).GetComponent<Text>().text =
            "Performance Rate: " + currentT + (newRatePoint >= 0 ? " (+" + newRatePoint + ")" : " (" + newRatePoint + ")");

        // Final output from calculating the rate point
        return newRatePoint + currentT;
    }

    public void CloseRatePointToogle(int markerId)
    {
        ratePointMarker[markerId].SetActive(true);

        foreach (Button button in unlock_ProcessingBtn)
            button.interactable = true;
    }

    #region ATTRACTED
    private void BuildUpRatePointContribution()
    {
        RateMeter rate = new RateMeter(LoginPage_Script.thisPage.GetUserPortOutput(), (int)GameManager.thisManager.get_score1.get_score);
        TrackEventEntry entry = new TrackEventEntry();

        entry.title = BeatConductor.thisBeat.Music_Database.Title;
        entry.level = (PlayerPrefs.GetInt("DifficultyLevel_valve", 1) == 1 ? PlayerPrefs.GetString("Difficulty_Normal_selectionTxt", "?") : (PlayerPrefs.GetInt("DifficultyLevel_valve", 1) == 2 ? PlayerPrefs.GetString("Difficulty_Hard_selectionTxt", "?") : PlayerPrefs.GetString("Difficulty_Ultimate_selectionTxt", "?")));
        entry.point = rate.CheckFor_IncreaseRate(PlayerPrefs.GetFloat("difficultyLevel_play2", 0));
        entry.cover = BeatConductor.thisBeat.Music_Database.seasonNo + "/" + BeatConductor.thisBeat.Music_Database.Background_Cover.name;
        entry.difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        entry.score = GameManager.thisManager.get_score1.get_score;

        // Process to addons track listing
        TrackListingDistribution.thisList.GetRateContribution(FindTrackChartCateogry(BeatConductor.thisBeat.Music_Database.seasonNo), entry);
        TrackListingDistribution.thisList.ClearCacheRate(FindTrackChartCateogry(BeatConductor.thisBeat.Music_Database.seasonNo), entry);
    }

    public int FindTrackChartCateogry(int season)
    {
        switch (season)
        {
            case 0:
            case 1:
            case 2:
                return 1;

            case 3:
                return 2;

            case 4:
                return 3;

            default:
                return 0;
        }
    }
    #endregion
    #endregion

    float BattleRate_Ref()
    {
        float i = 0;
        i = ((float)100 / GameManager.thisManager.getJudgeWindow.getOverallCombo) * GameManager.thisManager.getJudgeWindow.getMaxCombo;
        return i;
    }

    // Rank Function
    void ScoreLevel_Rank()
    {
        float scoreR = GameManager.thisManager.get_score1.get_score;
        GameObject.Find("Rank").GetComponent<Text>().text = "Stage Rank: " + MeloMelo_GameSettings.GetScoreRankStructure(scoreR.ToString()).rank;
    }

    // Close Transition: Function
    public void GoSelection(int index)
    {
        switch (index)
        {
            case 1:
                unlock_ProcessingBtn[0].interactable = false;
                GameObject.Find("Menu_Main").GetComponent<Animator>().SetTrigger("Closing");
                Invoke("GoSelection2", 1.5f);
                break;

            case 2:
                GameObject.Find("Menu_Main").GetComponent<Animator>().SetTrigger("Closing");
                Invoke("GoSelection_Viewer", 1.5f);
                break;

        }
    }

    private void GoSelection_Viewer()
    {
        GameObject.Find("Menu_Viewer").GetComponent<Animator>().SetTrigger("Opening");
    }

    public void GoSelection_ViewerButton(int index)
    {
        GameObject.Find("Menu_Viewer").GetComponent<Animator>().SetTrigger("Closing" + ResMelo);
        Invoke("GoReturn_Result", 1.5f);
    }

    private void GoReturn_Result()
    {
        GameObject.Find("Menu_Main").GetComponent<Animator>().SetTrigger("Opening");
        unlock_ProcessingBtn[0].interactable = true;
    }

    private void GoSelection2()
    {
        // Data: Overlay score with the latest one
        if (PlayerPrefs.GetInt(BeatConductor.thisBeat.Music_Database.Title + "_BattleRemark_" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 5) != 5
            && (int)GameManager.thisManager.get_score1.get_score > highscore)
        {
            Debug.Log("Local Score Achievement: OK!");
            PlayerPrefs.SetInt(BeatConductor.thisBeat.Music_Database.Title + "_score" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1),
                (int)GameManager.thisManager.get_score1.get_score);
        }

        // Data: Checking to preview track list
        if (PlayerPrefs.HasKey("MarathonPermit")) TrackListResult.SetActive(true);
        else ProcessToSaveMode(false);
    }

    #region DATA (Extra Component)
    public void CheckTechScore()
    {
        CloudSave_DataManagement data = new CloudSave_DataManagement(LoginPage_Script.thisPage.GetUserPortOutput(), MeloMelo_PlayerSettings.GetWebServerUrl());
        int currentScore = (int)GameManager.thisManager.get_score2.get_score;

        data.SaveBattleProgress(
                PlayerPrefs.GetInt("BattleDifficulty_Mode", 1),
                PlayerPrefs.GetInt("DifficultyLevel_valve", 1),
                BeatConductor.thisBeat.Music_Database.Title,
                PlayerPrefs.GetString(BeatConductor.thisBeat.Music_Database.Title + "_SuccessBattle_" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1) + PlayerPrefs.GetInt("BattleDifficulty_Mode", 1), "F"),
                techScore,
                currentScore
                );

        if (currentScore > techScore)
            PlayerPrefs.SetInt(BeatConductor.thisBeat.Music_Database.Title + "_techScore" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1) + PlayerPrefs.GetInt("BattleDifficulty_Mode", 1), currentScore);
    }

    public void CheckMarathonContent()
    {
        if (PlayerPrefs.HasKey("MarathonPermit"))
        {
            if (PlayerPrefs.GetInt(BeatConductor.thisBeat.Music_Database.Title + "_BattleRemark_" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 6) < 5)
            {
                PlayerPrefs.SetInt("MarathonChallenge_MCount", PlayerPrefs.GetInt("MarathonChallenge_MCount") + 1);
                PlayerPrefs.SetInt("Marathon_Quest_ScoreAddons", PlayerPrefs.GetInt("Marathon_Quest_Score", 0));

                int previousScore = PlayerPrefs.GetInt("Marathon_All_Score", 0);
                PlayerPrefs.SetInt("Marathon_All_Score", previousScore + (int)GameManager.thisManager.get_score1.get_score);
            }

            PlayerPrefs.DeleteKey("Marathon_Quest_Score");
        }
    }

    private void ProcessReSelection()
    {
        // Server Database: Store play points
        if (GameManager.thisManager.get_point.get_score > PlayerPrefs.GetInt(BeatConductor.thisBeat.Music_Database.Title + "_point" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 0))
        {
            // Point Loader
            PlayerPrefs.SetInt(BeatConductor.thisBeat.Music_Database.Title + "_point" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), (int)GameManager.thisManager.get_point.get_score);
            PlayerPrefs.SetInt(BeatConductor.thisBeat.Music_Database.Title + "_maxPoint" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), PlayerPrefs.GetInt("OverallCombo", 0) * 3);
        }

        // Server Database: Store Time-Stamp
        //StartCoroutine(data.SaveLastPlayedDate(LoginPage_Script.thisPage.get_user));

        // Local: Play Count increase
        PlayerPrefs.SetInt(userInput + "PlayedCount_Data", PlayerPrefs.GetInt(userInput + "PlayedCount_Data", 0) + 1);

        // Go Scene to Music Selection / BlackBoard
        if (PlayerPrefs.HasKey("Mission_Played")) 
        {
            int playedStatus = PlayerPrefs.GetInt(BeatConductor.thisBeat.Music_Database.Title + "_BattleRemark_" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 5) != 5 ? 1 : 2;
            PlayerPrefs.SetInt(PlayerPrefs.GetString("Mission_Title"), playedStatus); 
            PlayerPrefs.DeleteKey("Mission_Title"); 
        }

        // Transition: Return to ogrin
        try
        {
            SceneManager.LoadScene
            (
                PlayerPrefs.HasKey("Mission_Played") ? "StoryMode" :
                PlayerPrefs.HasKey("MarathonPermit") && PlayerPrefs.GetInt("MarathonChallenge_MCount") >
                    Resources.Load<MarathonInfo>(PlayerPrefs.GetString("Marathon_Assigned_Task", string.Empty)).Difficultylevel.Length
                        ? "MarathonSelection" : "Music Selection Stage"
            );
        }
        catch
        {
            SceneManager.LoadScene
            (
                PlayerPrefs.HasKey("MarathonPermit") && PlayerPrefs.GetInt("MarathonChallenge_MCount") >
                    MeloMelo_ExtensionContent_Settings.LoadMarathonDetail(PlayerPrefs.GetInt("MarathonInstanceNumber", 0)).track_difficulty.Length
                        ? "MarathonSelection" : "Music Selection Stage"
            );
        }
    }
    #endregion

    #region DATA (System Prompt)
    public void ProcessToSaveMode(bool skipTrackList)
    {
        // Special Condition: Using of track list
        if (!skipTrackList || skipTrackList && PlayerPrefs.HasKey("TrackList_End_List"))
        {
            if (skipTrackList) PlayerPrefs.SetInt("MarathonChallenge_MCount", 5);

            // Data: Saving progress
            if (TrackListResult.activeInHierarchy) TrackListResult.SetActive(false);
            SaveContentProgress(LoginPage_Script.thisPage.portNumber == (int)MeloMelo_PlayerSettings.LoginType.TempPass);
        }
    }

    private void SaveContentProgress(bool isNetworkAvailable)
    {
        // Perform action on interface
        if (isNetworkAvailable) GetComponent<ServerCloud_Save_Script>().StartSavingProcess();
        else GetComponent<Local_Save_Script>().StartSavingProcess();
    }

    public void ContentSavedCompleted(string title, bool isComplete)
    {
        if (isComplete)
            SaveIcon.transform.GetChild(1).GetComponent<Text>().text = title + "\nSave Successful";
        else
            SaveIcon.transform.GetChild(1).GetComponent<Text>().text = title + "\nSave Failed!";
    }
    #endregion

    #region MISC (Condition of Success) 
    public IEnumerator Encode_DataCheck()
    {
        yield return new WaitForSeconds(1);
        SaveIcon.GetComponent<Animator>().SetTrigger("Close");
        Invoke("ProcessReSelection", 2);
    }
    #endregion
}
