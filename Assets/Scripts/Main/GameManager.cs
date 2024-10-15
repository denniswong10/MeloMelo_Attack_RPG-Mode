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

    // Component Bundle: Get GameProperties
    public BattleProgressMeter get_progressMeter { get { return progressMeter; } }
    public JudgeWindowComponent getJudgeWindow { get { return judgeWindow; } }
    public GameplayObjectComponent getGameplayComponent { get { return gameplayWindow; } }

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

    // Load Gameplay UI and function
    void Start()
    {
        thisManager = this;
        //PreSet_BattleSetup();
        //PlayerPrefs.SetInt("NoteSpeed", 6);

        if (JudgeCounter && PlayerPrefs.GetInt("JudgeMeter_Setup", 0) != 2) IntiJudgeCounterContent();
        else JudgeCounter.SetActive(false);

        // Setup components
        progressMeter = new BattleProgressMeter();
        characterStatus = new UnitStatusComponent();
        enemyStatus = new UnitStatusComponent();
        judgeWindow = new JudgeWindowComponent();
        gameplayWindow = new GameplayObjectComponent();
        playField = new BattleGroundFieldComponent();

        // Extra Setup
        Invoke("UnitStatusSlot", 0.05f);

        ResMelo = PlayerPrefs.GetString("Resoultion_Melo", string.Empty);
        try { GameObject.Find("SideUI_MusicInfo").GetComponent<Animator>().SetBool("Open" + ResMelo, true); } catch { }

        try { GameObject.Find("RetreatBG").GetComponent<RawImage>().texture = 
                PlayerPrefs.HasKey("MarathonPermit") ? Resources.Load<Texture>("Background/BG11") : PreSelection_Script.thisPre.get_AreaData.BG; 
            } 
        catch { }

        Invoke("Calcuate_Combo", 0.1f);
        Invoke("CheckForPlayAreaDesicion", 3);

        // Remove Point
        PlayerPrefs.DeleteKey("Point_Scoring");

        // Scoring Structure
        if (Application.isEditor)
        {
            MeloMelo_GameSettings.GetScoreStructureSetup();
            MeloMelo_GameSettings.GetStatusRemarkStructureSetup();
        }

        // Cursor
        if (Cursor.visible) Cursor.visible = false;
    }

    // Update Function: Score Pugin
    void Update()
    {
        if (!DeveloperMode) CheckingBonusStatus();
        CheckingRetreatStatus();
        EndOfPlay();
    }

    #region Setup Stats (Checking and setting up display)
    // Extra: Score Opening
    void OpeningScore()
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
        MeloMelo_ScoreSystem.thisSystem.UpdateScoreDisplay();

        // Extra: Counter
        float tempCount = score1.get_maxScore - judgeWindow.getOverallCombo * Mathf.Floor(BeatConductor.thisBeat.fixedScore / judgeWindow.getOverallCombo);
        score1.ModifyScore((int)tempCount);
    }

    // Combo Searcher: Calcuate maxCombo
    void Calcuate_Combo()
    {
        foreach (int i in BeatConductor.thisBeat.get_myData_Note2)
        {
            if (i != 0 && i < 10) { judgeWindow.AddTotalCombo(); }
            if (i == 93) { judgeWindow.AddTotalCombo(); }
            if (i == 1 || i == 6) { gameplayWindow.AddEnemyCount(); }
            if (i == 3 || i == 7) { gameplayWindow.AddTrapsCount(); }

            // New Chart System
            if (BeatConductor.thisBeat.Music_Database.UltimateAddons)
            {
                foreach (MultipleCharting specialChart in BeatConductor.thisBeat.Music_Database.addons1)
                {
                    if (i == specialChart.SecondaryIndex)
                    {
                        // All Notes
                        if (specialChart.FirstLane != 0) judgeWindow.AddTotalCombo();
                        if (specialChart.SecondLane != 0) judgeWindow.AddTotalCombo();
                        if (specialChart.ThirdLane != 0) judgeWindow.AddTotalCombo();
                        if (specialChart.FourthLane != 0) judgeWindow.AddTotalCombo();
                        if (specialChart.FifthLane != 0) judgeWindow.AddTotalCombo();

                        // Enemys
                        if (specialChart.FirstLane == 1 || specialChart.FirstLane == 6) gameplayWindow.AddEnemyCount();
                        if (specialChart.SecondLane == 1 || specialChart.SecondLane == 6) gameplayWindow.AddEnemyCount();
                        if (specialChart.ThirdLane == 1 || specialChart.ThirdLane == 6) gameplayWindow.AddEnemyCount();
                        if (specialChart.FourthLane == 1 || specialChart.FourthLane == 6) gameplayWindow.AddEnemyCount();
                        if (specialChart.FifthLane == 1 || specialChart.FifthLane == 6) gameplayWindow.AddEnemyCount();

                        // Traps
                        if (specialChart.FirstLane == 3 || specialChart.FirstLane == 7) gameplayWindow.AddTrapsCount();
                        if (specialChart.SecondLane == 3 || specialChart.SecondLane == 7) gameplayWindow.AddTrapsCount();
                        if (specialChart.ThirdLane == 3 || specialChart.ThirdLane == 7) gameplayWindow.AddTrapsCount();
                        if (specialChart.FourthLane == 3 || specialChart.FourthLane == 7) gameplayWindow.AddTrapsCount();
                        if (specialChart.FifthLane == 3 || specialChart.FifthLane == 7) gameplayWindow.AddTrapsCount();
                    }
                }
            }

            if (BeatConductor.thisBeat.Music_Database.NewChartSystem)
            {
                // Pattern Charting
                if (BeatConductor.thisBeat.Music_Database.addons3 != null)
                {
                    foreach (PatternCharting chart in BeatConductor.thisBeat.Music_Database.addons3)
                    {
                        if (i == chart.SecondaryIndex)
                        {
                            foreach (PaterrnDefine row in chart.noteArray)
                            {
                                foreach (PatternLane col in row.noteOutput)
                                {
                                    if (col.PrimaryNote != 0)
                                    {
                                        judgeWindow.AddTotalCombo();

                                        if (col.PrimaryNote == 1 || col.PrimaryNote == 6) { gameplayWindow.AddEnemyCount(); }
                                        if (col.PrimaryNote == 3 || col.PrimaryNote == 7) { gameplayWindow.AddTrapsCount(); }
                                    }
                                }
                            }
                        }
                    }
                }

                // Chart Modification
                if (BeatConductor.thisBeat.Music_Database.addons3b != null)
                {
                    foreach (ChartModification mod in BeatConductor.thisBeat.Music_Database.addons3b)
                    {
                        if (i == mod.newIndex)
                        {
                            foreach (PatternCharting chart in BeatConductor.thisBeat.Music_Database.addons3)
                            {
                                if (chart.SecondaryIndex == mod.SecondaryNote)
                                {
                                    foreach (PaterrnDefine row in chart.noteArray)
                                    {
                                        foreach (PatternLane col in row.noteOutput)
                                        {
                                            if (col.PrimaryNote != 0)
                                            {
                                                judgeWindow.AddTotalCombo();

                                                if (col.PrimaryNote == 1 || col.PrimaryNote == 6) { gameplayWindow.AddEnemyCount(); }
                                                if (col.PrimaryNote == 3 || col.PrimaryNote == 7) { gameplayWindow.AddTrapsCount(); }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // Set HP
        try
        {
            UpdateCharacter_Health(PlayerPrefs.GetInt("Character_OverallHealth", 1), true);
            try { UpdateEnemy_Health(PlayerPrefs.GetInt("Enemy_OverallHealth", 1) + PreSelection_Script.thisPre.get_AreaData.EnemyBaseHealth[PlayerPrefs.GetInt("BattleDifficulty_Mode", 1) - 1], true); }
            catch { UpdateEnemy_Health(PlayerPrefs.GetInt("Enemy_OverallHealth", 1), true); }

            // Update Score System
            OpeningScore();
        }
        catch { }
    }

    // PlayArea Management: Control Update
    private void CheckForPlayAreaDesicion()
    {
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
                    AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Audio/SE/Perfect"), new Vector3(0, 1.8f, -10.8f));
                }

                else if (judgeWindow.FullComboPlay())
                {
                    if (progressMeter.ProgressRemarkChecker(progressMeter.GetProgressSpecialStatus(2)))
                    {
                        PlayerPrefs.SetInt(BeatConductor.thisBeat.Music_Database.Title + "_BattleRemark_" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 2);
                    }

                    MeloMelo_GameSettings.GetRecentStatusRemark = 2;
                    Bonus_sign.transform.GetChild(0).GetComponent<Text>().text = progressMeter.GetProgressSpecialStatus(2);
                    AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Audio/SE/Eliminate"), new Vector3(0, 1.8f, -10.8f));
                }

                else
                {
                    if (progressMeter.ProgressRemarkChecker(progressMeter.GetProgressSpecialStatus(3)))
                    {
                        PlayerPrefs.SetInt(BeatConductor.thisBeat.Music_Database.Title + "_BattleRemark_" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 3);
                    }

                    MeloMelo_GameSettings.GetRecentStatusRemark = 3;
                    Bonus_sign.transform.GetChild(0).GetComponent<Text>().text = progressMeter.GetProgressSpecialStatus(3);
                    AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Audio/SE/Missless"), new Vector3(0, 1.8f, -10.8f));
                }

                if (LoadingTransition_Script.thisLoader != null)
                    for (int i = 0; i < LoadingTransition_Script.thisLoader.get_statstoAll.slot_Stats.Length; i++) 
                        LoadingTransition_Script.thisLoader.get_statstoAll.slot_Stats[i].StatLoader();
            }

            // Battle Progress: < 80
            else if (progressMeter.ClearedBattleProgress())
            {
                if (progressMeter.ProgressRemarkChecker(progressMeter.GetProgressPlayedStatus()))
                {
                    PlayerPrefs.SetInt(BeatConductor.thisBeat.Music_Database.Title + "_BattleRemark_" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 4);

                    if (LoadingTransition_Script.thisLoader != null)
                        for (int i = 0; i < LoadingTransition_Script.thisLoader.get_statstoAll.slot_Stats.Length; i++)
                            LoadingTransition_Script.thisLoader.get_statstoAll.slot_Stats[i].StatLoader();
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
                if (KO.transform.GetChild(KO.transform.childCount - 1).gameObject.activeInHierarchy) { PlayerPrefs.SetString(BeatConductor.thisBeat.Music_Database.Title + "_SuccessBattle_" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1) + PlayerPrefs.GetInt("BattleDifficulty_Mode", 1), "T"); WinAlert = true; }
                else { WinAlert = false; }
            }
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
        if (Score != null)
        {
            // Add performance score to the scoreboard and update the display
            score1.ModifyScore((int)_score);
            Score.text = score1.get_score.ToString("000000");

            // ScoreSystem: Display
            MeloMelo_ScoreSystem.thisSystem.UpdateScoreDisplay();
            MeloMelo_ScoreSystem.thisSystem.UpdatePointDisplay();
        }

        if (PlayerPrefs.HasKey("MarathonPermit"))
        {
            GetComponent<MeloMelo_UnitSlot_Editor>().UpdateQuestCondition();
            UnitStatusSlot();
        }
    }

    public void UpdateScore_Tech(int _score)
    {
        if (Score2 != null)
        {
            // Add technical score to the status board and update the display
            int scoreFilter = !OverKill_Bar.activeInHierarchy ? _score : (_score * 2);
            score2.ModifyScore(scoreFilter);
            Score2.text = score2.get_score.ToString();
        }
    }

    public void UpdatePoint(int _point)
    {
        if (point != null) point.ModifyScore(_point);
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
            SkillContainer loadedSkill = Resources.Load<SkillContainer>("Database_Skills/" + PlayerPrefs.GetString("CharacterFront", "None") +
                allSkillOnActive[skillLoader]);

            if (loadedSkill)
            {
                // Display information about skill effect
                UpdateSkillInformation(loadedSkill);
                yield return new WaitForSeconds(2);

                foreach (ClassBase currentPick in characterStats.slot_Stats)
                {
                    // Get character is leading the party member
                    if (PlayerPrefs.GetString("CharacterFront", "None") == currentPick.name)
                    {
                        // Get character skill ready for use
                        GetComponent<SkillManager>().ExtractSkill(loadedSkill, currentPick);
                        GetComponent<SkillManager>().RegisterForSkillUsage(loadedSkill, skillIsOnPrimary[skillLoader]);
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
        if (!GameObject.Find("PlayArea").GetComponent<AudioSource>().isPlaying && BeatConductor.thisBeat.get_startNote)
            StartCoroutine(EndOfBattle());
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
        GameObject.Find("RetreatBG").GetComponent<Animator>().SetTrigger("Retreat");
        StartCoroutine(ProgressResult());
    }

    private void CheckingRetreatStatus()
    {
        if (GameObject.Find("Character") && GameObject.Find("Character").GetComponent<Character>().get_character.getInput.GetForInputExitPlay() 
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
    #endregion

    #region MAIN (Note Patcher)
    public void UpdateNoteStatus(string judge_string)
    {
        // Raise counter to judgeWindow
        Update_Counter(judge_string, true);

        // Raise judge with off-beat timing
        ModifyMainJudge();

        // Update text judge counter or Create feedback on character
        if (DeveloperMode) GameObject.Find(judge_string).GetComponent<Text>().text = judge_string + ": " + Update_Counter(judge_string, false);
        else
        {
            Vector3 position = new Vector3(GameObject.Find("Character").transform.position.x, GameObject.Find("Judgement Line").transform.position.y, GameObject.Find("Judgement Line").transform.position.z);

            // Extra: Judgement only shown determine on settings
            if ((PlayerPrefs.GetInt("Feedback_Display_Type_B") == 1 && judge_string != "Perfect_2") || PlayerPrefs.GetInt("Feedback_Display_Type_B") == 0)
                Instantiate(Resources.Load<GameObject>("Prefabs/PopUp/" + judge_string), position, Quaternion.identity);
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
        GameObject slotStatus = GameObject.FindGameObjectWithTag("PartyStatus");
        GameObject characterHealth = GameObject.FindGameObjectWithTag("Character_Health");

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
                    if (PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetInterfaceAnimation_ValueKey, 1) == 1)
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
                characterHealth.GetComponent<Text>().text = ((characterStatus.get_maxHealth == 1) ? "0/0" : 
                    characterStatus.get_health + "/" + characterStatus.get_maxHealth);

                if (characterStatus.get_health <= 0 && GameObject.Find("Character").GetComponent<Character>().stats.get_name != "NA" && !RetreatSuccess)
                    { slotStatus.transform.GetChild(slotStatus.transform.childCount - 1).gameObject.SetActive(true); Game_over(); }
                else
                    if (slotStatus.transform.GetChild(1).GetChild(0).GetComponent<Slider>() != null)
                        slotStatus.transform.GetChild(1).GetChild(0).GetComponent<Slider>().value = characterStatus.get_health;
            }
        }
    }

    // Update Auto: Enemy Health
    public void UpdateEnemy_Health(int amount, bool setHP)
    {
        GameObject slotStatus = GameObject.FindGameObjectWithTag("EnemyStatus");
        GameObject enemyHealth = GameObject.FindGameObjectWithTag("Enemy_Health");

        if (slotStatus != null)
        {
            if (setHP)
            {
                enemyStatus.SetMaxHealth(amount);
                enemyStatus.ModifyHealth(enemyStatus.get_maxHealth);
                HealthBar_E.GetComponent<Slider>().maxValue = enemyStatus.get_maxHealth;
            }
            else
            {
                if (PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetInterfaceAnimation_ValueKey, 1) == 1 &&
                    amount < 0 && !slotStatus.transform.GetChild(slotStatus.transform.childCount - 1).gameObject.activeInHierarchy) 
                        { slotStatus.GetComponent<Animator>().SetTrigger("Damage" + ResMelo); }
                enemyStatus.ModifyHealth(amount);
            }

            if (enemyHealth != null)
            {
                enemyHealth.GetComponent<Text>().text = enemyStatus.get_health + "/" + enemyStatus.get_maxHealth;
                if (enemyStatus.get_health <= 0)
                {
                    slotStatus.transform.GetChild(slotStatus.transform.childCount - 1).gameObject.SetActive(true);
                    OverkillBonus_Display(-enemyStatus.get_knockOutValue, enemyStatus.get_maxHealth);
                }
                else
                {
                    if (HealthBar_E.activeInHierarchy)
                        HealthBar_E.GetComponent<Slider>().value = enemyStatus.get_health;
                }
            }
        }
    }

    public void SpawnDamageIndicator(Vector3 target, int typeOfTarget, int damage)
    {
        if ((typeOfTarget == 1 && PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetDamageIndicatorA_ValueKey, 1) == 1) ||
            (typeOfTarget == 2 && PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetDamageIndicatorB_ValueKey, 1) == 1))
        {
            if ((typeOfTarget == 1 && characterStatus.get_health > 0 && characterStatus.get_health < characterStatus.get_maxHealth)
            || (typeOfTarget == 2 && enemyStatus.get_health > 0 && enemyStatus.get_health < enemyStatus.get_maxHealth))
            {
                GameObject damageIndicator = Instantiate(Resources.Load<GameObject>("Prefabs/Floating Damage/DamageIndicator"));
                damageIndicator.GetComponent<TextMesh>().color = damage < 0 ? new Color32(125, 36, 5, 255) : new Color32(50, 137, 50, 255);
                damageIndicator.GetComponent<TextMesh>().text = (damage > 0 ? "+" : string.Empty) + damage.ToString();
                damageIndicator.transform.position = typeOfTarget == 1 ? new Vector3(target.x, 0, -3.5f) : new Vector3(target.x, 2.5f, 0);
                Destroy(damageIndicator, 0.5f);
            }
        }
    }

    // Update Auto: Overkill Bonus
    private void OverkillBonus_Display(int overkill_value, int enemyHP_MAXvalue)
    {
        if (!OverKill_Bar.activeInHierarchy) { OverKill_Bar.SetActive(true); }
        else if (PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetInterfaceAnimation_ValueKey, 1) == 1) 
        { OverKill_Bar.GetComponent<Animator>().SetTrigger("Hit"); }

        if (enemyHP_MAXvalue != 0)
        { OverKill_Bar.transform.GetChild(0).GetComponent<Text>().text = "Overkill Bonus: " + (100f / enemyHP_MAXvalue * overkill_value).ToString("0.00") + "%"; }
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
        GameObject.Find("ProgressBar").GetComponent<Slider>().value = progressValue;
        GameObject.FindGameObjectWithTag("ProgressBar").GetComponent<Text>().text = progressValue.ToString("0") + "%";
        GameObject.Find("ProgressBar").transform.GetChild(1).GetChild(0).GetComponent<Image>().color = border;
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
        if (GameObject.FindGameObjectWithTag("MainCharacter") != null && offbeat_judge < JudgeCounterParticle.Length)
            Instantiate(JudgeCounterParticle[offbeat_judge], GameObject.FindGameObjectWithTag("MainCharacter").transform.position, Quaternion.identity);
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
        string[] slot_key = { "PartyStatus", "EnemyStatus" };

        // Setup character
        for (int currentSlot = 0; currentSlot < characterSlotStatus.Length; currentSlot++)
            characterSlotStatus[currentSlot].SetActive((PlayerPrefs.HasKey("MarathonPermit") ? 1 : 0) == currentSlot);

        // Setup enemy
        for (int currentSlot = 0; currentSlot <  enemySlotStatus.Length; currentSlot++)
            enemySlotStatus[currentSlot].SetActive((PlayerPrefs.HasKey("MarathonPermit") ? 1 : 0) == currentSlot);

        // Action slot status
        if (PlayerPrefs.HasKey("MarathonPermit"))
        {
            for (int index = 0; index < slot_key.Length; index++)
            {
                GameObject slot = GameObject.FindGameObjectWithTag(slot_key[index]);
                slot.transform.GetChild(slot.transform.childCount - 1).GetComponent<Text>().text = 
                    GetComponent<MeloMelo_UnitSlot_Editor>().GetMarathonQuestSlot(index);
                slot.transform.GetChild(1).GetChild(0).GetComponent<RawImage>().texture = 
                    GetComponent<MeloMelo_UnitSlot_Editor>().GetMarathonProfileIcon(index);
            }
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
