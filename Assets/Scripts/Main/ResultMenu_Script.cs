﻿using System.Collections;
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
        ResMelo = PlayerPrefs.GetString("Resoultion_Melo", string.Empty);
        try { userInput = GuestLogin_Script.thisScript.get_entry; } catch { userInput = LoginPage_Script.thisPage.get_user; }

        rateZone = GetComponent<RatePointToggleZone>();

        // Transfer score data for result 
        highscore = PlayerPrefs.GetInt(BeatConductor.thisBeat.Music_Database.Title + "_score" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), (int)PlayerPrefs.GetFloat("PerformanceScore", 0));
        techScore = PlayerPrefs.GetInt(BeatConductor.thisBeat.Music_Database.Title + "_techScore" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1) + PlayerPrefs.GetInt("BattleDifficulty_Mode", 1), 0);

        LoadAllResult();
        UpdateMusicInfo();

        // Cursor
        if (!Cursor.visible) Cursor.visible = true;
    }

    void Update()
    {
        rateZone.GetZoneFunctionProgram();
    }

    #region MAIN
    // Load Result Information
    void UpdateMusicInfo()
    {
        int levelMeter = (int)PlayerPrefs.GetFloat("difficultyLevel_play2", 0);
        string level = (PlayerPrefs.GetInt("DifficultyLevel_valve", 1) == 1 ? PlayerPrefs.GetString("Difficulty_Normal_selectionTxt", "?") : (PlayerPrefs.GetInt("DifficultyLevel_valve", 1) == 2 ? PlayerPrefs.GetString("Difficulty_Hard_selectionTxt", "?") : PlayerPrefs.GetString("Difficulty_Ultimate_selectionTxt", "?")));

        if (!GameManager.thisManager.DeveloperMode)
        {
            BG.texture = ((PlayerPrefs.HasKey("Mission_Played")) ? Resources.Load<Texture>("Background/BG1C") :
                PlayerPrefs.GetInt("Marathon_Challenge") == 0 ? PreSelection_Script.thisPre.get_AreaData.BG :
                Resources.Load<Texture>("Background/BG11"));

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
            RatePointIndicator border = new RatePointIndicator();
            border.UpdateColor_Border(userInput, "RateStatus");

            ScoreResult.transform.GetChild(1).GetComponent<Text>().text = Result_GetCurrentScore();
            ScoreResult.transform.GetChild(2).GetComponent<Text>().text = "[ " + Result_GetPreviousScore() + " ]";
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

            stats.slot_Stats[i].experience += ((PlayerPrefs.HasKey("Mission_Played")) ? 0 : PlayerPrefs.GetInt("Temp_Experience", 0));
            stats.slot_Stats[i].UpdateCurrentStats(true);

            if (stats.slot_Stats[i].name != "None")
            {
                int checkLevel;
                do
                {
                    checkLevel = stats.slot_Stats[i].level;
                    stats.slot_Stats[i].CheckLeveling(database.GetCharacterStatus(checkLevel).GetExperience);
                } while (checkLevel != stats.slot_Stats[i].level);

                GameObject.Find("Slot" + (i + 1) + "_CharInfo").transform.GetChild(2).GetComponent<Text>().text = "- " + stats.slot_Stats[i].characterName + " -";
                GameObject.Find("Slot" + (i + 1) + "_CharInfo").transform.GetChild(3).GetComponent<Text>().text = "EXP: " + stats.slot_Stats[i].experience + "/" + database.GetCharacterStatus(stats.slot_Stats[i].level).GetExperience + " (+" + PlayerPrefs.GetInt("Temp_Experience", 0) + ")";
                GameObject.Find("Slot" + (i + 1) + "_CharInfo").transform.GetChild(4).GetComponent<Text>().text = "LEVEL: " + stats.slot_Stats[i].level;

                GameObject.Find("Slot" + (i + 1) + "_CharInfo").transform.GetChild(1).GetComponent<Image>().enabled = true;
                GameObject.Find("Slot" + (i + 1) + "_CharInfo").transform.GetChild(1).GetComponent<Image>().sprite = stats.slot_Stats[i].icon;
            }
        }

        if (GameManager.thisManager.get_WinAlert) { GameObject.Find("Battle Status").GetComponent<Text>().text = "BATTLE SUCCESS!"; }
        else { GameObject.Find("Battle Status").GetComponent<Text>().text = "BATTLE FAILED!"; }

        GameObject.Find("Score2").GetComponent<Text>().text = "TECHNICAL SCORE: " + PlayerPrefs.GetInt("TechScore", 0) + " (" + TechScore_Ref() + ")";
        GameObject.Find("Score2_Slider").GetComponent<Slider>().maxValue = techScore;
        GameObject.Find("Score2_Slider").GetComponent<Slider>().value = PlayerPrefs.GetInt("TechScore", 0);

        GameObject.Find("BP").GetComponent<Text>().text = "BATTLE STREAK RATE: " + BattleRate_Ref().ToString("0.00") + "%";

        // Load Result 
        GameObject.Find("Perfect2").GetComponent<Text>().text = "Critical Perfect: " + PlayerPrefs.GetInt("Perfect2_count", 0);
        GameObject.Find("Perfect").GetComponent<Text>().text = "Perfect: " + PlayerPrefs.GetInt("Perfect_count", 0);
        GameObject.Find("Bad").GetComponent<Text>().text = "Bad: " + PlayerPrefs.GetInt("Bad_count", 0);
        GameObject.Find("Miss").GetComponent<Text>().text = "Miss: " + PlayerPrefs.GetInt("Miss_count", 0);
        GameObject.Find("MaxCombo_Value").GetComponent<Text>().text = PlayerPrefs.GetInt("MaxCombo_count", 0) + " / " + PlayerPrefs.GetInt("OverallCombo", 0);

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
        return PlayerPrefs.GetFloat("PerformanceScore", 0).ToString("0000000");
    }

    private string Result_GetPreviousScore()
    {
        return PlayerPrefs.GetInt(BeatConductor.thisBeat.Music_Database.Title + "_score" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 0).ToString("0000000");
    }

    private string Result_GetCurrentPoint(bool highscore)
    {
        if (highscore) return PlayerPrefs.GetInt(BeatConductor.thisBeat.Music_Database.Title + "_point" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 0).ToString();
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
        float i = PlayerPrefs.GetFloat("PerformanceScore", 0) - PlayerPrefs.GetInt(BeatConductor.thisBeat.Music_Database.Title + "_score" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 0);
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
        RateMeter rate = new RateMeter(LoginPage_Script.thisPage.GetUserPortOutput(), (int)PlayerPrefs.GetFloat("PerformanceScore", 0));
        TrackEventEntry entry = new TrackEventEntry();

        entry.title = BeatConductor.thisBeat.Music_Database.Title;
        entry.level = (PlayerPrefs.GetInt("DifficultyLevel_valve", 1) == 1 ? PlayerPrefs.GetString("Difficulty_Normal_selectionTxt", "?") : (PlayerPrefs.GetInt("DifficultyLevel_valve", 1) == 2 ? PlayerPrefs.GetString("Difficulty_Hard_selectionTxt", "?") : PlayerPrefs.GetString("Difficulty_Ultimate_selectionTxt", "?")));
        entry.point = rate.CheckFor_IncreaseRate(PlayerPrefs.GetFloat("difficultyLevel_play2", 0));
        entry.cover = BeatConductor.thisBeat.Music_Database.seasonNo + "/" + BeatConductor.thisBeat.Music_Database.Background_Cover.name;
        entry.difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        entry.score = PlayerPrefs.GetFloat("PerformanceScore", 0);

        TrackListingDistribution.thisList.GetRateContribution(FindTrackChartCateogry(BeatConductor.thisBeat.Music_Database.seasonNo), entry);
        TrackListingDistribution.thisList.ClearCacheRate(FindTrackChartCateogry(BeatConductor.thisBeat.Music_Database.seasonNo), entry);
    }

    private int FindTrackChartCateogry(int season)
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

    string TechScore_Ref()
    {
        int i = 0;
        i = techScore - PlayerPrefs.GetInt("TechScore", 0);

        if (i < 0) { i = -i; return "+" + i; }
        else { return "-" + i; }
    }

    float BattleRate_Ref()
    {
        float i = 0;
        i = ((float)100 / GameManager.thisManager.getJudgeWindow.getOverallCombo) * GameManager.thisManager.getJudgeWindow.getMaxCombo;
        return i;
    }

    // Rank Function
    void ScoreLevel_Rank()
    {
        float scoreR = PlayerPrefs.GetFloat("PerformanceScore", 0);
        GameObject.Find("Rank").GetComponent<Text>().text = "Stage Rank: " + MeloMelo_GameSettings.GetScoreRankStructure((int)scoreR).rank;
    }

    // Close Transition: Function
    public void GoSelection(int index)
    {
        switch (index)
        {
            case 1:
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
    }

    private void GoSelection2()
    {
        if (PlayerPrefs.GetInt(BeatConductor.thisBeat.Music_Database.Title + "_BattleRemark_" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 5) != 5
            && (int)PlayerPrefs.GetFloat("PerformanceScore", 0) > PlayerPrefs.GetInt(BeatConductor.thisBeat.Music_Database.Title + "_score" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1))
            )
        {
            Debug.Log("Local Score Achievement: OK!");
            PlayerPrefs.SetInt(BeatConductor.thisBeat.Music_Database.Title + "_score" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), (int)PlayerPrefs.GetFloat("PerformanceScore", 0));
        }

        if (LoginPage_Script.thisPage.portNumber == (int)MeloMelo_GameSettings.LoginType.TempPass && 
            LoginTemp_Script.thisTemp.get_login.get_success)
        { SaveIcon.SetActive(true); StartCoroutine(SavingData()); }
        else { SaveIcon.SetActive(true); StartCoroutine(LoadingGameApp()); }
    }

    // Saving score to server via local
    IEnumerator SavingData()
    {
        CheckTechScore();
        SaveIcon.transform.GetChild(1).GetComponent<Text>().text = "[Game Network]\nSaving Data...";
        yield return new WaitForSeconds(1);

        CloudSave_DataManagement data = new CloudSave_DataManagement(LoginPage_Script.thisPage.GetUserPortOutput(), PlayerPrefs.GetString("GameWeb_URL"));

        // Save 1
        data.SaveProgressTrack(
                BeatConductor.thisBeat.Music_Database.Title,
                PlayerPrefs.GetInt("DifficultyLevel_valve", 1),
                (int)PlayerPrefs.GetFloat("PerformanceScore", 0),
                GameManager.thisManager.getJudgeWindow.getMaxCombo
                );

        // Save 2
        data.SaveProgressTrackByRemark(
            BeatConductor.thisBeat.Music_Database.Title,
            PlayerPrefs.GetInt("DifficultyLevel_valve", 1),
            PlayerPrefs.GetInt(BeatConductor.thisBeat.Music_Database.Title + "_BattleRemark_" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 5)
            );

        // Save 3
        data.SaveProgressTrackByPoint(
                BeatConductor.thisBeat.Music_Database.Title,
                PlayerPrefs.GetInt("DifficultyLevel_valve", 1),
                PlayerPrefs.GetInt("Point_Scoring", 0)
                );

        yield return new WaitUntil(() => data.get_process.ToArray().Length == data.get_counter);

        if (GetProcessCloudSuccessful(data.get_process.ToArray())) 
            SaveIcon.transform.GetChild(1).GetComponent<Text>().text = "[Game Network]\nSave Successful";
        else 
            SaveIcon.transform.GetChild(1).GetComponent<Text>().text = "[Game Network]\nSave Failed!";

        yield return new WaitForSeconds(1);
        // Process to encode
        StartCoroutine(CheckingData());
    }

    bool GetProcessCloudSuccessful(bool[] condition)
    {
        foreach (bool check in condition)
            if (!check) return false;

        return true;
    }

    // Checking score from server via local
    IEnumerator CheckingData()
    {
        CheckTechScore();
        SaveIcon.transform.GetChild(1).GetComponent<Text>().text = "[Game Network]\nChecking Data...";
        yield return new WaitForSeconds(1);

        CloudSave_DataManagement data = new CloudSave_DataManagement(LoginPage_Script.thisPage.GetUserPortOutput(), PlayerPrefs.GetString("GameWeb_URL"));

        data.SaveSettingConfiguration(
                PlayerPrefs.GetString("MVOption", "T"),
                PlayerPrefs.GetInt("NoteSpeed", 20),
                PlayerPrefs.GetInt("AutoRetreat", 0),
                PlayerPrefs.GetInt("ScoreDisplay", 0),
                PlayerPrefs.GetInt("ScoreDisplay2", 0),
                PlayerPrefs.GetInt("JudgeMeter_Setup", 0)
                );

        data.SaveProgressProfile(
            PlayerPrefs.GetInt(LoginPage_Script.thisPage.get_user + "totalRatePoint", 0), 
            PlayerPrefs.GetInt(LoginPage_Script.thisPage.get_user + "PlayedCount_Data") + 1
            );

        data.SaveMusicSelectionLastVisited(
            PreSelection_Script.thisPre.get_AreaData.AreaName, 
            PlayerPrefs.GetInt("LastSelection", 1),
            PlayerPrefs.GetInt("DifficultyLevel_valve", 1)
            );

        data.SavePlayerData(
            PlayerPrefs.GetString("HowToPlay_Notice", "T"),
            PlayerPrefs.GetString("BattleSetup_Guide", "T"),
            PlayerPrefs.GetString("Control_notice", "T"),
            PlayerPrefs.GetFloat("BGM_VolumeGET", 1),
            PlayerPrefs.GetFloat("SE_VolumeGET", 1)
            );

        yield return new WaitUntil(() => data.get_process.ToArray().Length == data.get_counter);

        if (GetProcessCloudSuccessful(data.get_process.ToArray())) 
            SaveIcon.transform.GetChild(1).GetComponent<Text>().text = "[Game Network]\nCompleted!";
        else
            SaveIcon.transform.GetChild(1).GetComponent<Text>().text = "[Game Network]\nServer Error!";

        // Process to encode
        StartCoroutine(Encode_DataCheck());
    }

    IEnumerator Encode_DataCheck()
    {
        yield return new WaitForSeconds(1);
        SaveIcon.GetComponent<Animator>().SetTrigger("Close");

        //stats.SaveCloudCharacterStatus();
        Invoke("ProcessReSelection", 2);
    }

    // Loading game application
    IEnumerator LoadingGameApp()
    {
        if (!PlayerPrefs.HasKey("Mission_Played")) CheckTechScore();
        SaveIcon.transform.GetChild(1).GetComponent<Text>().text = "[Game Local]\nSaving Data...";
        bool isInvaild = GameManager.thisManager.getJudgeWindow.TotalJudgeCounted() == GameManager.thisManager.getJudgeWindow.getOverallCombo;

        if (isInvaild)
        {
            LocalSave_DataManagement data = new LocalSave_DataManagement(LoginPage_Script.thisPage.GetUserPortOutput(), "StreamingAssets/LocalData/MeloMelo_LocalSave_InGameProgress");

            data.SelectFileForActionWithUserTag(MeloMelo_GameSettings.GetLocalFileMainProgress);
            data.SaveProgress(BeatConductor.thisBeat.Music_Database.Title,
                PlayerPrefs.GetInt("DifficultyLevel_valve", 1),
                PlayerPrefs.GetFloat("PerformanceScore", 0),
                MeloMelo_GameSettings.GetScoreRankStructure((int)PlayerPrefs.GetFloat("PerformanceScore", 0)).rank);

            data.SelectFileForActionWithUserTag(MeloMelo_GameSettings.GetLocalFilePointData);
            data.SavePointProgress(BeatConductor.thisBeat.Music_Database.Title,
                PlayerPrefs.GetInt("DifficultyLevel_valve", 1),
                (int)GameManager.thisManager.get_point.get_score,
                PlayerPrefs.GetInt("OverallCombo", 0) * 3);

            data.SelectFileForActionWithUserTag(MeloMelo_GameSettings.GetLocalFileBattleProgress);
            data.SaveBattleProgress(
                BeatConductor.thisBeat.Music_Database.Title,
                PlayerPrefs.GetInt("DifficultyLevel_valve", 1),
                PlayerPrefs.GetInt("BattleDifficulty_Mode", 1),
                PlayerPrefs.GetString(BeatConductor.thisBeat.Music_Database.Title + "_SuccessBattle_" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1) + PlayerPrefs.GetInt("BattleDifficulty_Mode", 1), "F") == "T" ? true : false,
                PlayerPrefs.GetInt("TechScore", 0));

            data.SelectFileForActionWithUserTag(MeloMelo_GameSettings.GetLocalFileProfileData);
            data.SaveProfileState();

            data.SelectFileForActionWithUserTag(MeloMelo_GameSettings.GetLocalFileAccountSettings);
            data.SaveAccountSettings();

            data.SelectFileForActionWithUserTag(MeloMelo_GameSettings.GetLocalFileSelectionData);
            data.SaveLatestSelectionPoint(PreSelection_Script.thisPre.get_AreaData.AreaName, PlayerPrefs.GetInt("LastSelection", 1));

            StatsDistribution allStats = new StatsDistribution();
            allStats.load_Stats();

            foreach (ClassBase character in allStats.slot_Stats)
            {
                if (character.characterName != "None")
                {
                    data.SelectFileForActionWithUserTag(MeloMelo_GameSettings.GetLocalFileCharacterStats);
                    data.SaveCharacterStatsProgress(character.name, character.level, character.experience);
                }
            }
        }

        yield return new WaitForSeconds(2);
        SaveIcon.transform.GetChild(1).GetComponent<Text>().text = isInvaild ? "[Game Local]\nSave Successful" : "[Game Local]\nSave Failed!";

        yield return new WaitForSeconds(1);
        SaveIcon.GetComponent<Animator>().SetTrigger("Close");

        Invoke("ProcessReSelection", 2);
    }

    void CheckTechScore()
    {
        CloudSave_DataManagement data = new CloudSave_DataManagement(LoginPage_Script.thisPage.GetUserPortOutput(), PlayerPrefs.GetString("GameWeb_URL"));
        int currentScore = PlayerPrefs.GetInt("TechScore");

        if (currentScore > techScore)
        {
            PlayerPrefs.SetInt(BeatConductor.thisBeat.Music_Database.Title + "_techScore" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1) + PlayerPrefs.GetInt("BattleDifficulty_Mode", 1), currentScore);

            data.SaveBattleProgress(
                PlayerPrefs.GetInt("BattleDifficulty_Mode", 1),
                PlayerPrefs.GetInt("DifficultyLevel_valve", 1),
                BeatConductor.thisBeat.Music_Database.Title,
                PlayerPrefs.GetString(BeatConductor.thisBeat.Music_Database.Title + "_SuccessBattle_" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1) + PlayerPrefs.GetInt("BattleDifficulty_Mode", 1), "F"),
                PlayerPrefs.GetInt("temp_techScore", 0),
                currentScore
                );
        }
        else { PlayerPrefs.SetInt("temp_techScore", techScore); }
    }

    void ProcessReSelection()
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
        if (PlayerPrefs.HasKey("Mission_Played")) { PlayerPrefs.DeleteKey("Mission_Title"); }

        // Marathon: Set Next Stage
        if (PlayerPrefs.GetInt("Marathon_Challenge", 0) == 1) 
        { 
            int count = PlayerPrefs.GetInt("MarathonChallenge_MCount", 1);
            PlayerPrefs.SetInt("MarathonChallenge_MCount", (count + 1));
            bool[] check = new bool[3] { true, true, true };

            // Get Database: Condition 1
            if (Marathon_Selection.thisSelect.get_info_challenge.RankChallenge)
            {
                if (GetRankById(PlayerPrefs.GetString("StageRank", "F")) <= GetRankById(Marathon_Selection.thisSelect.get_info_challenge.setRank))
                { check[0] = true; }
                else check[0] = false;
            }

            // Get Database: Condition 2
            if (Marathon_Selection.thisSelect.get_info_challenge.RemarkChallenge)
            {
                if (Marathon_Selection.thisSelect.get_info_challenge.setRemark >= PlayerPrefs.GetInt(BeatConductor.thisBeat.Music_Database.Title + "_BattleRemark_" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 0))
                { check[1] = true; }
                else check[1] = false;
            }

            // Get Database: Condition 3
            if (Marathon_Selection.thisSelect.get_info_challenge.hitChallenge)
            {
                switch (Marathon_Selection.thisSelect.get_info_challenge.setJudge)
                {
                    case 1:
                        if ((PlayerPrefs.GetInt("Perfect2_count", 0) + PlayerPrefs.GetInt("Perfect_count", 0)) <=
                                Marathon_Selection.thisSelect.get_info_challenge.setAmount) { check[2] = true; }
                        else { check[2] = false; }
                        break;

                    case 2:
                        if (PlayerPrefs.GetInt("Bad_count", 0) <= Marathon_Selection.thisSelect.get_info_challenge.setAmount) 
                        { check[2] = true; }
                        else { check[2] = false; }
                        break;

                    case 3:
                        if (PlayerPrefs.GetInt("Miss_count", 0) <= Marathon_Selection.thisSelect.get_info_challenge.setAmount)
                        { check[2] = true; }
                        else { check[2] = false; }
                        break;

                    default:
                        if ((PlayerPrefs.GetInt("Perfect2_count", 0) + PlayerPrefs.GetInt("Perfect_count", 0) +
                            PlayerPrefs.GetInt("Bad_count", 0) + PlayerPrefs.GetInt("Miss_count", 0)) <=
                                Marathon_Selection.thisSelect.get_info_challenge.setAmount) { check[2] = true; }
                        else { check[2] = false; }
                        break;
                }
            }

            // Check Requirement: Meet Condition
            if (check[0] && check[1] && check[2]) 
            { PlayerPrefs.SetInt(Marathon_Selection.thisSelect.get_info_challenge.title + "_Content_" + (PlayerPrefs.GetInt("MarathonChallenge_MCount") - 1) + "_Cleared", 1); }
            else { PlayerPrefs.SetInt(Marathon_Selection.thisSelect.get_info_challenge.title + "_Content_" + (PlayerPrefs.GetInt("MarathonChallenge_MCount") - 1) + "_Cleared", 2); }
        }

        // Transition: Return to ogrin
        SceneManager.LoadScene
        (
            (PlayerPrefs.HasKey("Mission_Played") ? "BlackBoard" :
            PlayerPrefs.GetInt("Marathon_Challenge", 0) == 1 && PlayerPrefs.GetInt("MarathonChallenge_MCount") > 4 ? 
            "MarathonSelection" : "Music Selection Stage")
        );
    }

    private int GetRankById(string rankOutput)
    {
        //switch (index)
        //{
        //    case "X":
        //        return 1;

        //    case "SS":
        //        return 2;

        //    case "S":
        //        return 3;

        //    case "A":
        //        return 4;

        //    case "B":
        //        return 5;

        //    case "C":
        //        return 6;

        //    case "D":
        //        return 7;

        //    default:
        //        return 8;
        //}

        return MeloMelo_GameSettings.GetScoreRankStructureOrder(rankOutput);
    }
}