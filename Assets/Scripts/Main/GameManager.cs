using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MeloMelo_EditorBuild;
using MeloMelo_GameProperties;

public class GameManager : MonoBehaviour, IGameManager
{
    public static GameManager thisManager;

    // Component Bundle: Modify GameProperties
    private BattleProgressMeter progressMeter;
    private UnitStatusComponent characterStatus;
    private UnitStatusComponent enemyStatus;
    private JudgeWindowComponent judgeWindow;
    private GameplayObjectComponent gameplayWindow;
    private InGameObject_Manager inGameObjectWindow;
    public UnitStatusComponent get_characterStatus { get { return characterStatus; } }
    public UnitStatusComponent get_enemyStatus { get { return characterStatus; } }

    // Component Bundle: Get GameProperties
    public BattleProgressMeter get_progressMeter { get { return progressMeter; } }
    public JudgeWindowComponent getJudgeWindow { get { return judgeWindow; } }
    public GameplayObjectComponent getGameplayComponent { get { return gameplayWindow; } }
    public InGameObject_Manager getInGameObjectWindow { get { return inGameObjectWindow; } }

    // BattleGround PlayField Size: Modify
    private BattleGroundFieldComponent playField;

    // BattleGround PlayField Size: Get
    public BattleGroundFieldComponent get_playField { get { return playField; } }

    // Performance Score: GameProperties
    private GameSystem_Score score1;
    public GameSystem_Score get_score1 { get { return score1; } }

    // Point System: GameProperties
    private GameSystem_Score point;
    public GameSystem_Score get_point { get { return point; } }

    // Technical Score: GameProperties
    private GameSystem_Score score2;
    public GameSystem_Score get_score2 { get { return score2; } }

    // Display Object: GameProperties
    private Text Score;
    private Text Score2;

    private bool WinAlert = false;
    public bool get_WinAlert { get { return WinAlert; } }

    public bool DeveloperMode = false;

    public GameObject SkillAlert;
    public GameObject Alert_sign;
    public GameObject Bonus_sign;
    public GameObject GameOver;
    public GameObject AutoPlayText;
    public GameObject GamePromptAlert;
    private bool bonus_enable = true;

    private float NextRetreatTime = 0;
    private int RetreatCounter = 4;
    private bool RetreatSuccess = false;
    public GameObject Alert_Retreat;

    [Header("Health-Bar Additional")]
    public GameObject HealthBar_E;
    public GameObject OverKill_Bar;

    private string ResMelo = string.Empty;
    [SerializeField] private GameObject JudgeCounter;
    [SerializeField] private GameObject[] JudgeCounterParticle;

    [SerializeField] private GameObject[] characterSlotStatus;
    [SerializeField] private GameObject[] enemySlotStatus;
    [SerializeField] private Queue<InGameMessage> inGameMessageList;
    [SerializeField] private GameObject[] ExtraStatusDisplay;

    // Load Gameplay UI and function
    void Start()
    {
        thisManager = this;
        Screen.SetResolution(1360, 768, false);

        //PreSet_BattleSetup();
        //PlayerPrefs.SetInt("NoteSpeed", 3);
        //PlayerPrefs.SetInt("InputLatency_Id", 0);
        //PlayerPrefs.SetInt("AudioLatency_Id", 0);
        //PlayerPrefs.SetString("CharacterFront", "Warrior");

        //PlayerPrefs.DeleteKey("MarathonPermit");
        //PlayerPrefs.SetInt("DifficultyLevel_valve", 2);
        //PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetSpeedMeter_ValueKey, 0);
        //PlayerPrefs.SetInt("Enemy_OverallDamage", 0);

        if (!DeveloperMode)
        {
            if (JudgeCounter && PlayerPrefs.GetInt("JudgeMeter_Setup", 0) != 2) IntiJudgeCounterContent();
            else JudgeCounter.SetActive(false);

            PlayerPrefs.DeleteKey("CharacterKnockOutSuccessful");
            PlayerPrefs.DeleteKey("EnemyKnockOutSuccessful");           
            PlayerPrefs.DeleteKey("InGameMessage");
        }

        // Setup components
        progressMeter = new BattleProgressMeter();
        characterStatus = new UnitStatusComponent();
        enemyStatus = new UnitStatusComponent();
        judgeWindow = new JudgeWindowComponent();
        gameplayWindow = new GameplayObjectComponent();
        playField = new BattleGroundFieldComponent();
        inGameObjectWindow = GetComponent<InGameObject_Manager>();
        inGameMessageList = new Queue<InGameMessage>();

        // Extra Setup
        Invoke("UnitStatusSlot", 0.05f);

        ResMelo = PlayerPrefs.GetString("Resoultion_Melo", string.Empty);
        try { GameObject.Find("SideUI_MusicInfo").GetComponent<Animator>().SetBool("Open" + ResMelo, true); } catch { }

        try 
        { 
            GameObject.Find("RetreatBG").GetComponent<RawImage>().texture = MeloMelo_Environment_Settings.GetBackgroundCover();
        } 
        catch { }

        StartCoroutine(SetGameSettingsAsset());
        StartCoroutine(CheckForPlayAreaDesicion());

        // Remove Point
        PlayerPrefs.DeleteKey("Point_Scoring");
        PlayerPrefs.DeleteKey("UpperScoreTech");

        foreach (GameObject extraStatusIndicator in ExtraStatusDisplay)
        {
            extraStatusIndicator.SetActive(!PlayerPrefs.HasKey("MarathonPermit"));
            extraStatusIndicator.GetComponentInChildren<Text>().text = PlayerPrefs.GetFloat(extraStatusIndicator.name, 0) + "%";
        }

        // Scoring Structure
        if (Application.isEditor)
        {
            MeloMelo_GameSettings.GetScoreStructureSetup();
            MeloMelo_GameSettings.GetStatusRemarkStructureSetup();
            MeloMelo_ExtensionContent_Settings.LoadStartingStats();
            PlayerPrefs.SetString("Character_Active_Skill", "F");
        }
        else
            if (Cursor.visible) Cursor.visible = false; 
    }

    // Update Function: Score Pugin
    void Update()
    {
        if (!DeveloperMode)
        {
            CheckingBonusStatus();
            MeloMelo_ScoreSystem.thisSystem.CheckingForStatus();
        }

        CheckingRetreatStatus();
        EndOfPlay();
    }

    #region Setup Stats (Checking and setting up display)
    // Extra: Score Opening
    private void OpeningScore()
    {
        try { Score = GameObject.FindGameObjectWithTag("PerformanceScore").GetComponent<Text>(); } catch { Score = null; }
        try { Score2 = GameObject.FindGameObjectWithTag("TechScore").GetComponent<Text>(); } catch { Score2 = null; }

        // Set Score
        score1 = new GameSystem_Score();
        score1.SetMaxScore((int)BeatConductor.thisBeat.fixedScore + judgeWindow.getOverallCombo);

        // Set Point
        point = new GameSystem_Score();
        point.SetMaxScore(judgeWindow.getOverallCombo * 3);

        // Set technical
        score2 = new GameSystem_Score();

        // Update scoring
        //MeloMelo_ScoreSystem.thisSystem.UpdateScoreDisplay();

        // Extra: Counter
        float tempCount = score1.get_maxScore - judgeWindow.getOverallCombo * Mathf.Floor(BeatConductor.thisBeat.fixedScore / judgeWindow.getOverallCombo);
        score1.ModifyScore((int)tempCount);
    }

    private void SetupCharacterEntries()
    {
        MeloMelo_PlayEntries_Settings.AddEntriesToGamePlay(GameObject.FindGameObjectWithTag("MainCharacter"), MeloMelo_PlayEntries_Settings.PlayEntries.Character);
        MeloMelo_PlayEntries_Settings.AddEntriesToGamePlay(GameObject.Find("Judgement Line"), MeloMelo_PlayEntries_Settings.PlayEntries.Judgement_Line);
        MeloMelo_PlayEntries_Settings.AddEntriesToGamePlay(GameObject.FindGameObjectWithTag("ProgressBar"), MeloMelo_PlayEntries_Settings.PlayEntries.ProgressBar);
    }

    // Combo Searcher: Calcuate maxCombo
    private IEnumerator SetGameSettingsAsset()
    {
        yield return new WaitForSeconds(0.1f);

        if (judgeWindow != null) 
            judgeWindow.AddTotalCombo(PlayerPrefs.GetInt("Total_Notation_Count" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1)));

        if (gameplayWindow != null)
        {
            gameplayWindow.AddTotalCount(1, PlayerPrefs.GetInt("EnemyTakeCounter_" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1)));
            gameplayWindow.AddTotalCount(3, PlayerPrefs.GetInt("TrapsTakeCounter_" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1)));
        }

        // Set HP
        try
        {
            try
            {
                int totalBaseHealth = PreSelection_Script.thisPre.get_AreaData.EnemyBaseHealth[MeloMelo_GameSettings.GetAreaDifficultyMode() - 1];
                PlayerPrefs.SetInt("Enemy_AreaBaseHealth", totalBaseHealth);
                PlayerPrefs.SetInt("Enemy_AreaBaseHealth_MAX", totalBaseHealth);
            }
            catch 
            {
                PlayerPrefs.SetInt("Enemy_AreaBaseHealth", 0);
                PlayerPrefs.SetInt("Enemy_AreaBaseHealth_MAX", 0);
            }

            UpdateCharacter_Health(PlayerPrefs.GetInt("Character_OverallHealth", 1), true);
            UpdateEnemy_Health(PlayerPrefs.GetInt("Enemy_OverallHealth", 1), true);

            // Update Score System
            OpeningScore();

            // Update Character reference
            SetupCharacterEntries();
        }
        catch { }
    }

    // PlayArea Management: Control Update
    private IEnumerator CheckForPlayAreaDesicion()
    {
        yield return new WaitForSeconds(3);

        if (!DeveloperMode)
        {
            if (playField.IsDrawAreaPossible()) DrawOutPlayArea();
            else EraseOutPlayArea();
        }
    }

    public void DrawOutPlayArea()
    {
        if (!playField.CheckCompleteFieldDrawOut()) StartCoroutine(BeginDrawField());

        GameObject playArea = GameObject.Find("PlayArea");
        playArea.transform.localScale = new Vector3(playField.get_playBorder, 1, 2);
        playArea.transform.GetComponent<Renderer>().material.color = playField.get_playMatStatus;
    }

    public void EraseOutPlayArea()
    {
        if (!playField.CheckCompleteFieldEraseOut()) StartCoroutine(BeginEraseField());

        GameObject playArea = GameObject.Find("PlayArea");
        playArea.transform.localScale = new Vector3(playField.get_playBorder, 1, 2);
        playArea.transform.GetComponent<Renderer>().material.color = playField.get_playMatStatus;
    }

    private IEnumerator BeginDrawField()
    {
        yield return new WaitForSeconds(0.1f);
        playField.ModifyPlayBorder(0.01f);
        playField.ModifyLimitBorder(0.02f);

        // Return to update field
        DrawOutPlayArea();
    }

    private IEnumerator BeginEraseField()
    {
        yield return new WaitForSeconds(0.1f);
        playField.ModifyPlayBorder(-0.01f);
        playField.ModifyLimitBorder(-0.02f);

        // Return to update field
        EraseOutPlayArea();
    }
    #endregion

    #region Play_Condition (Checking game condition for ending)
    // Game Condition: Bonus Sign - Status
    void CheckingBonusStatus()
    {
        // Overall judge status of the overall combo
        if (judgeWindow.LevelPlayCleared() & BeatConductor.thisBeat.get_startNote && bonus_enable)
        {
            // Audio Voice
            float voiceVolume = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetAudioVoice_ValueKey) == 0 ? 
                PlayerPrefs.GetFloat(MeloMelo_PlayerSettings.GetSE_ValueKey) : 0;

            // Remove defeated play status and replace a new play status: Otherwise skip remove
            int status = (AutoPlayText.activeInHierarchy ? 6 : PlayerPrefs.GetInt(BeatConductor.thisBeat.Music_Database.Title + "_BattleRemark_" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 6));
            progressMeter.SetProgressRemark(status);
            if (progressMeter.ProgressRemarkChecker(progressMeter.GetProgressUnplayedStatus())) PlayerPrefs.DeleteKey(BeatConductor.thisBeat.Music_Database.Title + "_BattleRemark_" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1));

            // Special clearing status: PERFECT, ALL ELIMINATE, MISSLESS 
            bool maxOut = progressMeter.MaxOutBattleProgress();

            // Battle Progress: 100
            if (maxOut)
            {
                Bonus_sign.SetActive(maxOut);
                Bonus_sign.GetComponent<Animator>().SetTrigger("Bonus");
                bonus_enable = !maxOut;

                if (judgeWindow.AllPerfectPlay())
                {
                    if (progressMeter.ProgressRemarkChecker(progressMeter.GetProgressSpecialStatus(1)))
                    {
                        PlayerPrefs.SetInt(BeatConductor.thisBeat.Music_Database.Title + "_BattleRemark_" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 1);
                    }

                    MeloMelo_GameSettings.GetRecentStatusRemark = 1;
                    Bonus_sign.transform.GetChild(0).GetComponent<Text>().text = progressMeter.GetProgressSpecialStatus(1);
                    AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Audio/SE/Perfect"), new Vector3(0, 1.8f, -10.8f), voiceVolume);
                }

                else if (judgeWindow.FullComboPlay())
                {
                    if (progressMeter.ProgressRemarkChecker(progressMeter.GetProgressSpecialStatus(2)))
                    {
                        PlayerPrefs.SetInt(BeatConductor.thisBeat.Music_Database.Title + "_BattleRemark_" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 2);
                    }

                    MeloMelo_GameSettings.GetRecentStatusRemark = 2;
                    Bonus_sign.transform.GetChild(0).GetComponent<Text>().text = progressMeter.GetProgressSpecialStatus(2);
                    AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Audio/SE/Eliminate"), new Vector3(0, 1.8f, -10.8f), voiceVolume);
                }

                else
                {
                    if (progressMeter.ProgressRemarkChecker(progressMeter.GetProgressSpecialStatus(3)))
                    {
                        PlayerPrefs.SetInt(BeatConductor.thisBeat.Music_Database.Title + "_BattleRemark_" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 3);
                    }

                    MeloMelo_GameSettings.GetRecentStatusRemark = 3;
                    Bonus_sign.transform.GetChild(0).GetComponent<Text>().text = progressMeter.GetProgressSpecialStatus(3);
                    AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Audio/SE/Missless"), new Vector3(0, 1.8f, -10.8f), voiceVolume);
                }

                //if (LoadingTransition_Script.thisLoader != null)
                //    for (int i = 0; i < LoadingTransition_Script.thisLoader.get_statstoAll.slot_Stats.Length; i++) 
                //        LoadingTransition_Script.thisLoader.get_statstoAll.slot_Stats[i].StatLoader();
            }

            // Battle Progress: < 80
            else if (progressMeter.ClearedBattleProgress())
            {
                if (progressMeter.ProgressRemarkChecker(progressMeter.GetProgressPlayedStatus()))
                {
                    PlayerPrefs.SetInt(BeatConductor.thisBeat.Music_Database.Title + "_BattleRemark_" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 4);

                    //if (LoadingTransition_Script.thisLoader != null)
                    //    for (int i = 0; i < LoadingTransition_Script.thisLoader.get_statstoAll.slot_Stats.Length; i++)
                    //        LoadingTransition_Script.thisLoader.get_statstoAll.slot_Stats[i].StatLoader();
                }

                MeloMelo_GameSettings.GetRecentStatusRemark = 4;
            }

            // Battle Progress: < 50
            else
            {
                if (progressMeter.ProgressRemarkChecker(progressMeter.GetProgressUnplayedStatus()))
                { PlayerPrefs.SetInt(BeatConductor.thisBeat.Music_Database.Title + "_BattleRemark_" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 5); }

                MeloMelo_GameSettings.GetRecentStatusRemark = 5;
            }

            // Battle Lost: Default remark to defeated status
            if (!DeveloperMode)
            {
                GameObject KO = GameObject.FindGameObjectWithTag("EnemyStatus");
                if (KO.transform.GetChild(KO.transform.childCount - 1).gameObject.activeInHierarchy) { PlayerPrefs.SetString(BeatConductor.thisBeat.Music_Database.Title + "_SuccessBattle_" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1) + MeloMelo_GameSettings.GetAreaDifficultyMode(), "T"); WinAlert = true; }
                else { WinAlert = false; }
            }

            // Marathon Final Scoring
            if (PlayerPrefs.HasKey("MarathonPermit")) GetComponent<MeloMelo_UnitSlot_Editor>().FinalizeQuestCondition();
        }
    }

    // Clone: Force End: Function 
    IEnumerator ProgressResult()
    {
        yield return new WaitForSeconds(5);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Music Selection Stage");
    }
    #endregion

    #region Game Calling (Checking for signal for playing)
    // Game Signal: Controller
    public void GameStarting()
    {
        // Display skill features
        if (GetComponent<SkillManager>().IsSkillOnActive() && !PlayerPrefs.HasKey("MarathonPermit")) StartCoroutine(SkillFeatures());
        else OnStandyForPlay();
    }

    void TransitionToResult() { if (!RetreatSuccess) UnityEngine.SceneManagement.SceneManager.LoadScene("Result"); }
    #endregion

    #region SETUP (Score Updater)
    public float ScoreRefactoring()
    {
        return score1.get_maxScore - judgeWindow.getOverallCombo * BeatConductor.thisBeat.get_scorePerfect2;
    }
    #endregion

    // Display: All Score Remarks
    #region MAIN (Score Updater)
    public void UpdateScore(float _score)
    {
        if (score1 != null)
        {
            // Add performance score to the scoreboard and update the display
            score1.ModifyScore((int)_score);
            if (Score != null) Score.text = score1.get_score.ToString("000000");
        }
    }

    public void UpdateScore_Tech(int _score)
    {
        if (score2 != null)
        {
            // Add technical score to the status board and update the display
            int scoreFilter = !OverKill_Bar.activeInHierarchy ? _score : (_score * 2);
            score2.ModifyScore(scoreFilter);
            UpperLimitTechScore();

            if (Score2 != null) Score2.text = Mathf.Clamp(score2.get_score, 0, 9999999).ToString();
        }
    }

    public void UpdatePoint(int _point)
    {
        if (point != null)
        {
            point.ModifyScore(_point);
        }
    }
    #endregion

    // Update Auto: All Note (Perfect, Bad, Miss)
    #region SETUP (Play Condition)
    private void PreSet_BattleSetup()
    {
        if (!DeveloperMode)
        {
            PlayerPrefs.SetString("MVOption", "F");
            PlayerPrefs.SetInt("NoteSpeed", 4);
            PlayerPrefs.SetInt("DifficultyLevel_valve", 2);
            PlayerPrefs.SetInt("Feedback_Display_Type_B", 0);
            PlayerPrefs.SetInt("Feedback_Display_Type", 0);
            PlayerPrefs.SetInt("JudgeMeter_Setup", 1);
            PlayerPrefs.SetInt("ScoreDisplay2", 5);
            PlayerPrefs.DeleteKey("MarathonPermit");
        }
    }

    private IEnumerator SkillFeatures()
    {
        string[] allSkillOnActive = CheckForSkillAvailable();
        bool[] skillIsOnPrimary = { true, false };

        // Get skill ready for simulation run
        MeloMelo_RPGEditor.StatsDistribution characterStats = new MeloMelo_RPGEditor.StatsDistribution();
        characterStats.load_Stats(); 

        // Load all skills are available in the moment
        for (int skillLoader = 0; skillLoader < allSkillOnActive.Length; skillLoader++)
        {
            ResourceRequest loadedSkill = Resources.LoadAsync<SkillContainer>("Database_Skills/" + PlayerPrefs.GetString("CharacterFront", "None") +
                allSkillOnActive[skillLoader]);

            yield return loadedSkill;
            SkillContainer IsSkillReady = loadedSkill.asset as SkillContainer;

            if (IsSkillReady)
            {
                // Display information about skill effect
                UpdateSkillInformation(IsSkillReady);
                yield return new WaitForSeconds(2);

                foreach (ClassBase skillCaster in characterStats.character_base_reference)
                {
                    // Get character is leading the party member
                    if (skillCaster != null && PlayerPrefs.GetString("CharacterFront", "None") == skillCaster.name)
                    {
                        // Get character skill ready for use
                        GetComponent<SkillManager>().ExtractSkill(IsSkillReady, skillCaster);
                        GetComponent<SkillManager>().RegisterForSkillUsage(IsSkillReady, skillIsOnPrimary[skillLoader]);
                        break;
                    }
                }

                // Preview mode and wait for the skill is done reading
                yield return new WaitForSeconds(1);
            }
        }

        // Go to the next step
        ActivationOfEffect(1);
        yield return new WaitForSeconds(3);
        OnStandyForPlay();
    }

    private void OnStandyForPlay()
    {
        if (SkillAlert.activeInHierarchy) 
        {
            SkillAlert.GetComponent<Animator>().SetTrigger("Close");
            SkillAlert.SetActive(false); 
        }

        Alert_sign.gameObject.SetActive(true);
        Alert_sign.GetComponent<Animator>().SetTrigger("Play");

        Invoke("StartOfPlay", 5);
    }

    private void StartOfPlay()
    {
       if (Alert_sign) Alert_sign.transform.GetChild(0).GetComponent<Text>().text = "MUSIC START!";
       BeatConductor.thisBeat.Invoke("StartMusicButton", 1);
    }

    private void EndOfPlay()
    {
        if (!DeveloperMode)
        {
            bool isTrackCompleted = BeatConductor.thisBeat.get_startNote && !GameObject.Find("PlayArea").GetComponent<AudioSource>().isPlaying;
            bool isTrackEnded = (judgeWindow.get_perfect2 + judgeWindow.get_perfect + judgeWindow.get_bad + judgeWindow.get_miss) >= PlayerPrefs.GetInt("Total_Notation_Count" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1));
            if (isTrackCompleted && isTrackEnded) StartCoroutine(EndOfBattle());
        }
    }
    #endregion

    #region MAIN (Play Condition)
    private void Game_over()
    {
        GameOverDisplay(true);
        ForfeitTheGamePlay();

        MeloMelo_GameSettings.GetRecentStatusRemark = 5;
        Invoke("TransitionToResult", 3);
    }

    private void ForfeitTheGamePlay()
    {
        // Update battle field
        playField.FinishedStageField(5);

        // Status default goes to defeated
        if (PlayerPrefs.GetInt(BeatConductor.thisBeat.Music_Database.Title + "_BattleRemark_" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 0) > 4)
            PlayerPrefs.SetInt(BeatConductor.thisBeat.Music_Database.Title + "_BattleRemark_" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 5);

        // Update miss count
        int remaining = judgeWindow.getOverallCombo - judgeWindow.get_perfect2 - judgeWindow.get_perfect - judgeWindow.get_bad;
        PlayerPrefs.SetInt("Miss_count", remaining);
    }

    private IEnumerator EndOfBattle()
    {
        bool onSkillStillActive = GetComponent<SkillManager>().IsSkillOnActive() && !PlayerPrefs.HasKey("MarathonPermit");
        yield return new WaitForSeconds(4);
        UpdateEndResult();
        yield return new WaitForSeconds(onSkillStillActive ? 3 : 0.05f);

        // Update miss count
        if (onSkillStillActive) ActivationOfEffect(2);
        PlayerPrefs.SetInt("Miss_count", judgeWindow.get_miss);
        Invoke("TransitionToResult", 5);
    }
    #endregion

    #region MAIN (User Play Condition)
    public void RetreatTrigger()
    {
        RetreatSuccess = true;
        PlayerPrefs.SetInt("RetreatRoute", 1);

        foreach (GameObject extraStatusDisplay in ExtraStatusDisplay)
            extraStatusDisplay.SetActive(false);

        GameObject.Find("RetreatBG").GetComponent<Animator>().SetTrigger("Retreat");
        StartCoroutine(ProgressResult());
    }

    private void CheckingRetreatStatus()
    {
        if (MeloMelo_PlayEntries_Settings.GetEntriesToGamePlay(MeloMelo_PlayEntries_Settings.PlayEntries.Character) != null)
        {
            if (MeloMelo_PlayEntries_Settings.GetEntriesToGamePlay(MeloMelo_PlayEntries_Settings.PlayEntries.Character).GetComponent<Character>().get_character.getInput.GetForInputExitPlay()
                && !RetreatSuccess && BeatConductor.thisBeat.get_startNote)
            {
                if (Time.time >= NextRetreatTime)
                {
                    NextRetreatTime = Time.time + 1;
                    RetreatCounter--;
                    RetreatDisplay(true);

                    if (RetreatCounter <= 0) RetreatTrigger();
                }
            }
            else if (!RetreatSuccess)
            {
                RetreatDisplay(false);
                RetreatCounter = 4;
            }
        }
    }
    #endregion

    #region MAIN (Note Patcher)
    public void UpdateNoteStatus(string judge_string)
    {
        // Raise counter to judgeWindow
        Update_Counter(judge_string, true);

        // Raise judge with off-beat timing
        ModifyMainJudge();

        // Marathon: Update Status
        if (PlayerPrefs.HasKey("MarathonPermit"))
        {
            GetComponent<MeloMelo_UnitSlot_Editor>().SetQuestCondition(false);
            UpdateUnitStatusSlot();
        }

        // Score System: Update
        MeloMelo_ScoreSystem.thisSystem.UpdatePointDisplay();
        MeloMelo_ScoreSystem.thisSystem.UpdateScoreDisplay();

        // Update text judge counter or Create feedback on character
        if (DeveloperMode) GameObject.Find(judge_string).GetComponent<Text>().text = judge_string + ": " + Update_Counter(judge_string, false);
        else
        {
            if (MeloMelo_PlayEntries_Settings.GetEntriesToGamePlay(MeloMelo_PlayEntries_Settings.PlayEntries.Character) != null) // GameObject.Find("Character")
            {
                Vector3 position = new Vector3(
                    MeloMelo_PlayEntries_Settings.GetEntriesToGamePlay(MeloMelo_PlayEntries_Settings.PlayEntries.Character).transform.position.x, 
                    MeloMelo_PlayEntries_Settings.GetEntriesToGamePlay(MeloMelo_PlayEntries_Settings.PlayEntries.Judgement_Line).transform.position.y, 
                    MeloMelo_PlayEntries_Settings.GetEntriesToGamePlay(MeloMelo_PlayEntries_Settings.PlayEntries.Judgement_Line).transform.position.z
                    );
                // GameObject.Find("Judgement Line")

                // Extra: Judgement only shown determine on settings
                if ((PlayerPrefs.GetInt("Feedback_Display_Type_B") == 1 && judge_string != "Perfect_2") || PlayerPrefs.GetInt("Feedback_Display_Type_B") == 0)
                {
                    if (inGameObjectWindow != null)
                    {
                        GameObject popUp_target = inGameObjectWindow.judgeIndicator.GetPopUpText();

                        if (popUp_target != null)
                        {
                            popUp_target.transform.position = position;
                            popUp_target.GetComponent<IndicatorText_Script>().UpdateJudgementStatus(judge_string);
                            popUp_target.SetActive(true);
                        }

                        else if (inGameObjectWindow.judgeIndicator.IsSpawnAvailable())
                        {
                            GameObject newPopUp = Instantiate(Resources.Load<GameObject>("Prefabs/PopUp/" + judge_string), position, Quaternion.identity);
                            newPopUp.GetComponent<IndicatorText_Script>().UpdateJudgementStatus(judge_string);
                        }
                    }
                }
            }
        }
    }

    private int Update_Counter(string index, bool set)
    {
        switch (index)
        {
            case "Perfect_2":
                if (set) { judgeWindow.UpdateJudgeStatus(1); }
                return judgeWindow.get_perfect2;

            case "Perfect":
                if (set) { judgeWindow.UpdateJudgeStatus(2); }
                return judgeWindow.get_perfect;

            case "Bad":
                if (set) { judgeWindow.UpdateJudgeStatus(3); }
                return judgeWindow.get_bad;

            case "Miss":
                if (set) { judgeWindow.UpdateJudgeStatus(4); }
                return judgeWindow.get_miss;

            default:
                return 0;
        }
    }
    #endregion

    #region MAIN (RPG Starter)
    // Update Auto: Character Health
    public void UpdateCharacter_Health(int amount, bool setHP)
    {
        GameObject slotStatus = characterSlotStatus[PlayerPrefs.HasKey("MarathonPermit") ? 1 : 0];
        Text characterHealth = characterSlotStatus[0].transform.GetChild(1).GetChild(0).GetChild(3).GetComponent<Text>();

        if (slotStatus != null)
        {
            if (setHP)
            {
                characterStatus.SetMaxHealth(amount);
                characterStatus.ModifyHealth(characterStatus.get_maxHealth);

                if (slotStatus.transform.GetChild(1).GetChild(0).GetComponent<Slider>() != null)
                    slotStatus.transform.GetChild(1).GetChild(0).GetComponent<Slider>().maxValue = characterStatus.get_maxHealth;
            }
            else
            {
                if (slotStatus.GetComponent<Animator>() != null)
                {
                    if (PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetInterfaceAnimation_ValueKey) == 1)
                    {
                        if (amount < 0 && !slotStatus.transform.GetChild(slotStatus.transform.childCount - 1).gameObject.activeInHierarchy)
                            slotStatus.GetComponent<Animator>().SetTrigger("Damage" + ResMelo);

                        else
                            slotStatus.GetComponent<Animator>().SetTrigger("Heal" + ResMelo);
                    }
                }

                if (amount < 0)
                {
                    int damageAfterResist = MeloMelo_ExtraStats_Settings.ResistAgainstDamageResistance(amount);
                    characterStatus.ModifyHealth(damageAfterResist > 0 ? 0 : damageAfterResist);
                    GetComponent<SkillManager>().PromptResultOfDamageResistance(amount, damageAfterResist > 0 ? 0 : damageAfterResist, "Character");
                }
                else
                    characterStatus.ModifyHealth(amount);
            }

            if (characterHealth != null)
            {
                characterHealth.text = ((characterStatus.get_maxHealth == 1) ? "0/0" :
                   MeloMelo_GameSettings.GetUnitDisplayHealth(characterStatus.get_health, characterStatus.get_maxHealth,
                        MeloMelo_PlayerSettings.GetUnitHealthOnCharacter_ValueKey));

                if (characterStatus.get_health <= 0 && MeloMelo_PlayEntries_Settings.GetEntriesToGamePlay(MeloMelo_PlayEntries_Settings.PlayEntries.Character).GetComponent<Character>().stats.get_name != "NA" && !RetreatSuccess)
                {
                    // Item Usage: For health pot
                    int boostedLifePoint = MeloMelo_ItemUsage_Settings.GetExtraBoostLifePoint(PlayerPrefs.GetString("CharacterFront", "None"));

                    if (boostedLifePoint > 0)
                    {
                        GameHealingPot(boostedLifePoint);
                    }
                    else
                    {
                        slotStatus.transform.GetChild(slotStatus.transform.childCount - 1).gameObject.SetActive(true);

                        if (slotStatus.transform.GetChild(1).GetChild(0).GetComponent<Slider>() != null)
                            slotStatus.transform.GetChild(1).GetChild(0).GetComponent<Slider>().value = 0;

                        if (!PlayerPrefs.HasKey("CharacterKnockOutSuccessful"))
                        {
                            AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Audio/SE/Defeat"), new Vector3(0, 0, -10), PlayerPrefs.GetFloat(MeloMelo_PlayerSettings.GetSE_ValueKey));
                            PlayerPrefs.SetInt("CharacterKnockOutSuccessful", 1);
                            PromptInGameMessage("BATTLE CONDITION", "Character KnockOut !", "Your character is unable to continue this battle. This battle will end here instead.");

                            // Get game over screen
                            Invoke("Game_over", 2);
                        }
                    }
                }
                else
                    if (slotStatus.transform.GetChild(1).GetChild(0).GetComponent<Slider>() != null)
                        slotStatus.transform.GetChild(1).GetChild(0).GetComponent<Slider>().value = characterStatus.get_health;
            }
        }
    }

    private void GameHealingPot(int totalUsage)
    {
        // Life Pot: Usage up to 0 to 100
        int usageLifePointPlan = Mathf.Clamp(totalUsage, 0, 100);
        float calculateLifePoint = characterStatus.get_maxHealth / 100 * usageLifePointPlan;

        UpdateCharacter_Health((int)calculateLifePoint, false);
        SpawnDamageIndicator(transform.position, 1, (int)calculateLifePoint);
        MeloMelo_ItemUsage_Settings.UseExtraLifePoint(PlayerPrefs.GetString("CharacterFront", "None"), usageLifePointPlan);

        // Added message prompt
        GetComponent<SkillManager>().UpdatePotEffect("OnExtraLifeEffect", SkillManager.ItemUsageBuff.Healing);
        PromptInGameMessage("ITEM USAGE", "Recovering Health Point", "Your character used healing potion as character health has been reduced to 0.");
    }

    // Update Auto: Enemy Health
    public void UpdateEnemy_Health(int amount, bool setHP)
    {
        GameObject slotStatus = enemySlotStatus[PlayerPrefs.HasKey("MarathonPermit") ? 1 : 0];
        Text enemyHealth = enemySlotStatus[0].transform.GetChild(0).GetChild(0).GetChild(3).GetComponent<Text>();
        Text enemySeconadryHealth = enemySlotStatus[0].transform.GetChild(0).GetChild(1).GetChild(3).GetComponent<Text>();
        Slider secondaryHealthBar = enemySlotStatus[0].transform.GetChild(0).GetChild(1).GetComponent<Slider>();

        if (slotStatus != null)
        {
            if (setHP)
            {
                enemyStatus.SetMaxHealth(amount);
                enemyStatus.ModifyHealth(enemyStatus.get_maxHealth);
                HealthBar_E.GetComponent<Slider>().maxValue = enemyStatus.get_maxHealth;
                secondaryHealthBar.maxValue = PlayerPrefs.GetInt("Enemy_AreaBaseHealth_MAX", 0);
            }
            else
            {
                if (PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetInterfaceAnimation_ValueKey) == 1 &&
                    amount < 0 && !slotStatus.transform.GetChild(slotStatus.transform.childCount - 1).gameObject.activeInHierarchy) 
                        { slotStatus.GetComponent<Animator>().SetTrigger("Damage" + ResMelo); }
                enemyStatus.ModifyHealth(amount);
            }

            if (enemyHealth != null)
            {
                enemyHealth.text = MeloMelo_GameSettings.GetUnitDisplayHealth(
                    enemyStatus.get_health, enemyStatus.get_maxHealth, MeloMelo_PlayerSettings.GetUnitHealthOnEnemy_ValueKey);

                secondaryHealthBar.value = PlayerPrefs.GetInt("Enemy_AreaBaseHealth", 0);
                enemySeconadryHealth.text = secondaryHealthBar.value + "/" + secondaryHealthBar.maxValue;

                if (enemyStatus.get_health <= 0 && PlayerPrefs.GetInt("Enemy_AreaBaseHealth", 0) > 0)
                {
                    int calculateHealth = enemyStatus.get_maxHealth - enemyStatus.get_health;
                    Vector3 setPosition = MeloMelo_PlayEntries_Settings.GetEntriesToGamePlay(
                        MeloMelo_PlayEntries_Settings.PlayEntries.Target_Reference).transform.position;

                    if (PlayerPrefs.GetInt("Enemy_AreaBaseHealth", 0) - calculateHealth >= 0)
                    {
                        int newAreaHealth = PlayerPrefs.GetInt("Enemy_AreaBaseHealth", 0) - calculateHealth;
                        PlayerPrefs.SetInt("Enemy_AreaBaseHealth", newAreaHealth);

                        enemyStatus.ModifyHealth(calculateHealth);
                        SpawnDamageIndicator(setPosition, 2, calculateHealth);
                    }
                    else
                    {
                        int finalAreaHealth = PlayerPrefs.GetInt("Enemy_AreaBaseHealth", 0);
                        enemyStatus.ModifyHealth(finalAreaHealth);

                        PlayerPrefs.SetInt("Enemy_AreaBaseHealth", 0);
                        SpawnDamageIndicator(setPosition, 2, finalAreaHealth);
                    }

                    enemyHealth.text = MeloMelo_GameSettings.GetUnitDisplayHealth(
                    enemyStatus.get_health, enemyStatus.get_maxHealth, MeloMelo_PlayerSettings.GetUnitHealthOnEnemy_ValueKey);

                    secondaryHealthBar.value = PlayerPrefs.GetInt("Enemy_AreaBaseHealth", 0);
                    enemySeconadryHealth.text = secondaryHealthBar.value + "/" + secondaryHealthBar.maxValue;

                    if (HealthBar_E.activeInHierarchy)
                        HealthBar_E.GetComponent<Slider>().value = enemyStatus.get_health;
                }

                else if (enemyStatus.get_health <= 0)
                {
                    slotStatus.transform.GetChild(slotStatus.transform.childCount - 1).gameObject.SetActive(true);
                    OverkillBonus_Display(-enemyStatus.get_knockOutValue, enemyStatus.get_maxHealth);
                    if (HealthBar_E.activeInHierarchy) HealthBar_E.GetComponent<Slider>().value = 0;

                    if (!PlayerPrefs.HasKey("EnemyKnockOutSuccessful"))
                    {
                        AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Audio/SE/Victory"), new Vector3(0, 0, -10), PlayerPrefs.GetFloat(MeloMelo_PlayerSettings.GetSE_ValueKey));
                        PlayerPrefs.SetInt("EnemyKnockOutSuccessful", 1);
                        PromptInGameMessage("BATTLE CONDITION", "KnockOut Successful !", "Technical Score will be double on your next enemy or enemy-attack hits.");
                    }
                }
                else
                {
                    if (HealthBar_E.activeInHierarchy)
                        HealthBar_E.GetComponent<Slider>().value = enemyStatus.get_health;
                }
            }
        }
    }

    struct InGameMessage
    {
        public string topic;
        public string title;
        public string description;
    }

    public void PromptInGameMessage(string topic, string title, string description)
    {
        InGameMessage messageOnReceive = new InGameMessage();
        messageOnReceive.topic = topic;
        messageOnReceive.title = title;
        messageOnReceive.description = description;

        inGameMessageList.Enqueue(messageOnReceive);
        if (!PlayerPrefs.HasKey("InGameMessage")) StartCoroutine(GetInGameMessageCalled());
    }

    private IEnumerator GetInGameMessageCalled()
    {
        PlayerPrefs.SetInt("InGameMessage", 1);

        while (inGameMessageList.Count > 0)
        {
            InGameMessage currentMessage = inGameMessageList.Dequeue();

            GamePromptAlert.SetActive(true);
            GamePromptAlert.transform.GetChild(0).GetComponent<Text>().text = currentMessage.title;
            GamePromptAlert.transform.GetChild(1).GetComponent<Text>().text = currentMessage.description;
            GamePromptAlert.transform.GetChild(2).GetComponentInChildren<Text>().text = currentMessage.topic;
            yield return new WaitForSeconds(2.5f);
        }

        PlayerPrefs.DeleteKey("InGameMessage");
        GamePromptAlert.SetActive(false);       
    }

    // Spawn damage pop-up text in-game
    public void SpawnDamageIndicator(Vector3 target, int typeOfTarget, int damage, bool textBIG = false)
    {
        bool characterIndicator = typeOfTarget == 1 && PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetDamageIndicatorA_ValueKey, 1) == 1;
        bool enemyIndicator = typeOfTarget == 2 && PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetDamageIndicatorB_ValueKey, 1) == 1;

        if (characterIndicator || enemyIndicator)
        {
            bool characterVisibleValue = typeOfTarget == 1 && characterStatus.get_health > 0
                && characterStatus.get_health + damage <= characterStatus.get_maxHealth;

            bool enemyVisibleValue = typeOfTarget == 2 && enemyStatus.get_health > 0 
                && enemyStatus.get_health + damage <= enemyStatus.get_maxHealth;

            if ((characterVisibleValue || enemyVisibleValue) && !PlayerPrefs.HasKey("MarathonPermit"))
            {
                if (inGameObjectWindow != null)
                {
                    GameObject isDamageIndicator = inGameObjectWindow.damageIndicator.GetPopUpText();

                    if (isDamageIndicator != null)
                    {
                        isDamageIndicator.transform.localScale = textBIG ? new Vector3(3, 3, 1f) : new Vector3(1.2f, 1.2f, 1f);
                        isDamageIndicator.GetComponent<TextMesh>().color = damage == 0 ? Color.grey : damage > 0 ? new Color32(0, 158, 0, 255) : new Color32(209, 0, 0, 255);
                        isDamageIndicator.GetComponent<TextMesh>().text = damage == 0 ? "RESIST" : damage.ToString();
                        isDamageIndicator.transform.position = typeOfTarget == 1 ? new Vector3(target.x, 0, -3.5f) : new Vector3(target.x, 2.5f, 0);
                        isDamageIndicator.GetComponent<DamageIndicator_Script>().Setup(2);
                        isDamageIndicator.SetActive(true);
                    }

                    else if (inGameObjectWindow.damageIndicator.IsSpawnAvailable())
                    {
                        GameObject damageIndicator = Instantiate(Resources.Load<GameObject>("Prefabs/Floating Damage/DamageIndicator"));
                        damageIndicator.transform.localScale = textBIG ? new Vector3(3, 3, 1f) : new Vector3(1.2f, 1.2f, 1f);
                        damageIndicator.GetComponent<TextMesh>().color = damage == 0 ? Color.grey : damage > 0 ? new Color32(0, 158, 0, 255) : new Color32(209, 0, 0, 255);
                        damageIndicator.GetComponent<TextMesh>().text = damage == 0 ? "RESIST" : damage.ToString();

                        damageIndicator.transform.position = typeOfTarget == 1 ? new Vector3(target.x, 0, -3.5f) : new Vector3(target.x, 2.5f, 0);
                        damageIndicator.GetComponent<DamageIndicator_Script>().Setup(2);
                    }
                }
            }
        }
    }

    // Update Auto: Overkill Bonus
    private void OverkillBonus_Display(int overkill_value, int enemyHP_MAXvalue)
    {
        if (!OverKill_Bar.activeInHierarchy) { OverKill_Bar.SetActive(true); }
        else if (PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetInterfaceAnimation_ValueKey) == 1) 
        { OverKill_Bar.GetComponent<Animator>().SetTrigger("Hit"); }

        if (enemyHP_MAXvalue != 0)
        { 
            int overkill_multipler = PlayerPrefs.GetInt("PartySize_Index", 1);
            float final_score = 100f / (enemyHP_MAXvalue * overkill_multipler);
            OverKill_Bar.transform.GetChild(0).GetComponent<Text>().text = "Overkill Bonus: " + (final_score * overkill_value).ToString("0.00") + "%"; 
        }
        else { OverKill_Bar.transform.GetChild(0).GetComponent<Text>().text = "Overkill Bonus: --"; }
    }

    // Update Auto: Battle Progress Meter
    public void UpdateBattle_Progress(float amount)
    {
        progressMeter.Modify_BattleProgress(amount);
        BattleProgress_Display(progressMeter.get_battleProgress, progressMeter.GetBattleProgressBorder());
    }

    private void BattleProgress_Display(float progressValue, Color border)
    {
        if (MeloMelo_PlayEntries_Settings.GetEntriesToGamePlay(MeloMelo_PlayEntries_Settings.PlayEntries.ProgressBar) != null)
        {
            MeloMelo_PlayEntries_Settings.GetEntriesToGamePlay(MeloMelo_PlayEntries_Settings.PlayEntries.ProgressBar).GetComponent<Slider>().value = progressValue;
            MeloMelo_PlayEntries_Settings.GetEntriesToGamePlay(MeloMelo_PlayEntries_Settings.PlayEntries.ProgressBar).transform.GetChild(3).GetComponent<Text>().text = progressValue.ToString("0") + "%";
            MeloMelo_PlayEntries_Settings.GetEntriesToGamePlay(MeloMelo_PlayEntries_Settings.PlayEntries.ProgressBar).transform.GetChild(1).GetChild(0).GetComponent<Image>().color = border;
        }
        else
            Debug.Log("Progress Bar: No reference has been found yet...");
    }
    #endregion

    #region EXTRA (Offbeat Timing Management)
    private void IntiJudgeCounterContent()
    {
        int[] contentSize = { 3, 4 };

        for (int visible = 0; visible < contentSize[PlayerPrefs.GetInt("JudgeMeter_Setup", 0)]; visible++)
        {
            if (PlayerPrefs.GetInt("JudgeMeter_Setup") != 0) JudgeCounter.transform.GetChild(contentSize[PlayerPrefs.GetInt("JudgeMeter_Setup") - 1] + visible).gameObject.SetActive(true);
            else JudgeCounter.transform.GetChild(visible).gameObject.SetActive(true);
        }

        for (int i = 0; i < MeloMelo_GameSettings.GetJudgeFastOrLate_Value.Length; i++)
        {
            // Basic
            MeloMelo_GameSettings.GetJudgeFastOrLate_Value[i] = 0;
            GetFastNLate_Context(i);

            // Advance
            MeloMelo_GameSettings.FastNLate_Critcal[i] = 0;
            MeloMelo_GameSettings.FastNLate_Perfect[i] = 0;
            MeloMelo_GameSettings.FastNLate_Bad[i] = 0;
        }
    }

    public void ModifyFastNLateJudge(int state, int judge)
    {
        if (state < MeloMelo_GameSettings.GetJudgeFastOrLate.Length)
        {
            int judge_boundary = state > -1 && state < MeloMelo_GameSettings.GetJudgeFastOrLate_Value.Length ? state : 1;

            // Basic
            BasicFastNLateJudge(judge_boundary);
            if (Feeback_Settings_Filter(judge, judge_boundary)) DisplayCounterParticle_FastNLate(judge_boundary);

            // Advance
            AdvanceFastNLateJudge(judge_boundary, judge);
        }
    }

    private void GetFastNLate_Context(int content)
    {
        if (JudgeCounter != null && PlayerPrefs.GetInt("JudgeMeter_Setup") == 0)
        {
            JudgeCounter.transform.GetChild(content).GetComponent<Animator>().SetTrigger("Add");
            JudgeCounter.transform.GetChild(content).GetComponent<Text>().text = MeloMelo_GameSettings.GetJudgeFastOrLate[content] + ": " +
                MeloMelo_GameSettings.GetJudgeFastOrLate_Value[content];
        }
    }

    private void BasicFastNLateJudge(int offbeat_judge)
    {
        MeloMelo_GameSettings.GetJudgeFastOrLate_Value[offbeat_judge]++;
        GetFastNLate_Context(offbeat_judge);
    }

    private void DisplayCounterParticle_FastNLate(int offbeat_judge)
    {
        if (MeloMelo_PlayEntries_Settings.GetEntriesToGamePlay(MeloMelo_PlayEntries_Settings.PlayEntries.Character) != null && offbeat_judge < JudgeCounterParticle.Length)
        {
            if (inGameObjectWindow != null)
            {
                GameObject marginError_PopUp = inGameObjectWindow.judgeIndicator.GetPopUpText();

                if (marginError_PopUp != null)
                {
                    marginError_PopUp.transform.position = MeloMelo_PlayEntries_Settings.GetEntriesToGamePlay(MeloMelo_PlayEntries_Settings.PlayEntries.Character).transform.position;
                    marginError_PopUp.GetComponent<IndicatorText_Script>().UpdateMarginErrorStatus(offbeat_judge);
                    marginError_PopUp.SetActive(true);
                }

                else if (inGameObjectWindow.judgeIndicator.IsSpawnAvailable())
                {
                    GameObject newPopUp = Instantiate(JudgeCounterParticle[offbeat_judge], MeloMelo_PlayEntries_Settings.GetEntriesToGamePlay(MeloMelo_PlayEntries_Settings.PlayEntries.Character).transform.position, Quaternion.identity);
                    newPopUp.GetComponent<IndicatorText_Script>().UpdateMarginErrorStatus(offbeat_judge);
                }
            }
        }
    }

    private bool Feeback_Settings_Filter(int mainJudge, int subJudge)
    {
        if (PlayerPrefs.GetInt("Feedback_Display_Type", 0) == 0)
            return true;
        else if (PlayerPrefs.GetInt("Feedback_Display_Type", 0) == 1 && mainJudge != 1 && subJudge != 1)
            return true;
        else
            return false;
    }

    private void AdvanceFastNLateJudge(int state, int judge)
    {
        switch (judge)
        {
            case 2:
                MeloMelo_GameSettings.FastNLate_Perfect[state]++;
                break;

            case 3:
                MeloMelo_GameSettings.FastNLate_Bad[state]++;
                break;

            case 4:
                break;

            default:
                MeloMelo_GameSettings.FastNLate_Critcal[state]++;
                break;
        }
    }

    public void ModifyMainJudge()
    {
        if (PlayerPrefs.GetInt("JudgeMeter_Setup") == 1)
        {
            for (int judge = 0; judge < MeloMelo_GameSettings.GetMainJudgeStats.Length; judge++)
                JudgeCounter.transform.GetChild(judge + MeloMelo_GameSettings.GetJudgeFastOrLate.Length).GetComponent<Text>().text = MeloMelo_GameSettings.GetMainJudgeStats[judge] + ": " +
                    GetMainJudgeValue(judge);
        }
    }

    private int GetMainJudgeValue(int index)
    {
        switch (index)
        {
            case 0:
                return judgeWindow.get_perfect2;

            case 1:
                return judgeWindow.get_perfect;

            case 2:
                return judgeWindow.get_bad;

            default:
                return judgeWindow.get_miss;
        }
    }
    #endregion

    #region EXTRA (Score Multipler)
    public void FinalScoreMultipler(float raw_score)
    {
        float addons = raw_score * CurrentValueMultipler();

        // Update score to gameplay
        UpdateScore(addons);
    }

    public float CurrentValueMultipler()
    {
        float raw_value = 100f / judgeWindow.getOverallCombo * (judgeWindow.getOverallCombo - judgeWindow.get_miss) * 0.01f;
        return raw_value;
    }
    #endregion

    #region EXTRA (Gameplay Status)
    public void AutoPlayDisplay(bool enable)
    {
        AutoPlayText.SetActive(enable);
    }

    private void GameOverDisplay(bool stopAudio)
    {
        GameOver.SetActive(true);
        GameOver.GetComponent<Animator>().SetTrigger("Opening");
        GameObject.Find("PlayArea").GetComponent<AudioSource>().mute = stopAudio;
    }

    private void RetreatDisplay(bool active)
    {
        if ("ScoreEditor" != UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
        {
            Alert_Retreat.SetActive(active);
            Alert_Retreat.transform.GetChild(0).GetComponent<Text>().text = "RETREAT FROM BATTLE IN " + RetreatCounter + "...";
        }
    }

    private void UnitStatusSlot()
    {
        // Setup character
        for (int currentSlot = 0; currentSlot < characterSlotStatus.Length; currentSlot++)
            characterSlotStatus[currentSlot].SetActive((PlayerPrefs.HasKey("MarathonPermit") ? 1 : 0) == currentSlot);

        // Setup enemy
        for (int currentSlot = 0; currentSlot <  enemySlotStatus.Length; currentSlot++)
            enemySlotStatus[currentSlot].SetActive((PlayerPrefs.HasKey("MarathonPermit") ? 1 : 0) == currentSlot);

        Invoke("UpdateUnitStatusSlot", 0.05f);
    }

    private void UpdateUnitStatusSlot()
    {       
        // Action slot status
        if (PlayerPrefs.HasKey("MarathonPermit"))
        {
            // Character
            characterSlotStatus[1].transform.GetChild(characterSlotStatus[1].transform.childCount - 1).GetComponent<Text>().text = GetComponent<MeloMelo_UnitSlot_Editor>().GetMarathonStageIndicator();
            characterSlotStatus[1].transform.GetChild(1).GetChild(0).GetComponent<RawImage>().texture = GetComponent<MeloMelo_UnitSlot_Editor>().GetMarathonProfileIcon(0);

            // Enemy
            enemySlotStatus[1].transform.GetChild(enemySlotStatus[1].transform.childCount - 1).GetComponent<Text>().text = GetComponent<MeloMelo_UnitSlot_Editor>().GetMarathonQuestCheckerIndicator();
            enemySlotStatus[1].transform.GetChild(1).GetChild(0).GetComponent<RawImage>().texture = GetComponent<MeloMelo_UnitSlot_Editor>().GetMarathonProfileIcon(1);
        }
    }

    private void UpdateEndResult()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "ScoreEditor" && !Bonus_sign.activeInHierarchy)
        {
            Alert_sign.SetActive(true);
            Alert_sign.transform.GetComponent<Animator>().SetTrigger("Play");
            Alert_sign.transform.GetChild(0).GetComponent<Text>().text = progressMeter.GetProgressPassNFailStatus();
        }
    }
    #endregion

    #region EXTRA (Tech Status)
    private void UpperLimitTechScore()
    {
        const int maxLimit = 9999999;

        if (Score2 != null && score2.get_score > maxLimit)
        {
            if (RaiseStarStatus())
            {
                float exceedPoint = score2.get_score - maxLimit - 1;
                score2.ResetScore();
                score2.ModifyScore((int)exceedPoint);
            }
        }
    }

    private bool RaiseStarStatus()
    {
        bool isRaised = false;
        int count = 0;

        for (int star = 0; star < characterSlotStatus[0].transform.GetChild(6).transform.childCount; star++)
        {
            if (characterSlotStatus[0].transform.GetChild(6).GetChild(star).GetComponent<RawImage>().color.a != 1)
            {
                isRaised = true;
                count++;
                characterSlotStatus[0].transform.GetChild(6).GetChild(star).GetComponent<RawImage>().color = new Color(1, 1, 1, 1);
                break;
            }
            else
                count++;
        }

        PlayerPrefs.SetInt("UpperScoreTech", count);
        return isRaised;
    }
    #endregion

    #region EXTRA (Skill Information) 
    private void UpdateSkillInformation(SkillContainer info)
    {
        if (!SkillAlert.activeInHierarchy)
        {
            SkillAlert.SetActive(true);
            SkillAlert.GetComponent<Animator>().SetTrigger("Open");
        }

        SkillAlert.transform.GetChild(0).GetComponent<Text>().text = "Skill Effect - " + info.skillName + " (Lv. " + 
            MeloMelo_SkillData_Settings.CheckSkillGrade(info.skillName) + ")";

        SkillAlert.transform.GetChild(1).GetComponent<Text>().text = info.description;
    }

    private void ActivationOfEffect(int active_id)
    {
        if (Alert_sign.activeInHierarchy) Alert_sign.SetActive(false);
        if (Bonus_sign.activeInHierarchy) Bonus_sign.SetActive(false);

        if (!SkillAlert.activeInHierarchy)
        {
            SkillAlert.SetActive(true);
            SkillAlert.GetComponent<Animator>().SetTrigger("Open");
        }

        SkillAlert.transform.GetChild(0).GetComponent<Text>().text = active_id == 1 ? "Start of Track" : "End Of Track";
        SkillAlert.transform.GetChild(1).GetComponent<Text>().text = "Your character will perform skill effect " +
            (active_id == 1 ? "before the track begin" : "before the game ends");

        if (active_id == 1) GetComponent<SkillManager>().OnEffectUpdate(0, true);
        else GetComponent<SkillManager>().OnEffectUpdate(2, true);
    }

    private string[] CheckForSkillAvailable()
    {
        List<string> allSkillId = new List<string>();
        allSkillId.Add("_Primary_Skill");
        allSkillId.Add("_Secondary_Skill_" + MeloMelo_CharacterInfo_Settings.GetUsageOfSecondarySkill(PlayerPrefs.GetString("CharacterFront", "None")));

        for (int index = 0; index < allSkillId.ToArray().Length; index++)
        {
            // Lock everything for skill which not set
            GetComponent<SkillManager>().UnlockSkillSlot(index + 1, true);
        }

        return allSkillId.ToArray();
    }
    #endregion

    #region NOT IN USE
    void LoadResultCache()
    {
        PlayerPrefs.SetInt("Perfect2_count", judgeWindow.get_perfect2);
        PlayerPrefs.SetInt("Perfect_count", judgeWindow.get_perfect);
        PlayerPrefs.SetInt("Bad_count", judgeWindow.get_bad);
        PlayerPrefs.SetInt("Miss_count", judgeWindow.get_miss);
        PlayerPrefs.SetInt("OverallCombo", judgeWindow.getOverallCombo);
        PlayerPrefs.SetInt("MaxCombo_count", judgeWindow.getMaxCombo);
        PlayerPrefs.SetFloat("PerformanceScore", score1.get_score);
        PlayerPrefs.SetInt("TechScore", (int)score2.get_score);
    }
    #endregion
}
