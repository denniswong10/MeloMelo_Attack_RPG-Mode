using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MeloMelo_RatingMeter;
using MeloMelo_RPGEditor;
using MeloMelo_ExtraComponent;
using MeloMelo_Local;
using MeloMelo_Network;

public class TrackLeaderBoardEntry
{
    public string user;
    public string title;
    public int score;
    public string rank;

    public TrackLeaderBoardEntry(string user, string title, int score, string rank)
    {
        this.user = user;
        this.title = title;
        this.score = score;
        this.rank = rank;
    }
}

public class SelectionMenu_Script : MonoBehaviour
{
    public static SelectionMenu_Script thisSelect;

    [SerializeField] private GameObject AreaBonusSign;
    [SerializeField] private GameObject NewReleaseSign;

    [Header("LeaderBoard: Component")]
    [SerializeField] private GameObject NetworkCofig;
    [SerializeField] private Text leaderBoard_entry_title;
    [SerializeField] private Text leaderBoard_entry_info;
    private List<TrackLeaderBoardEntry> trackLeaderBoardList = null;
    
    private MusicSelectionPage selection;
    public MusicSelectionPage get_selection { get { return selection; } }

    private GameObject BGM;
    public GameObject get_BGM { get { return BGM; } }

    private bool loadBGM = false;
    public bool get_loadBGM { get { return loadBGM; } }

    public GameObject[] DifficultyArea;

    [Header("Quick-Look Component")]
    public GameObject[] HowToPlay_Page;
    public GameObject[] ButtonUI_HTP;

    [Header("Information Component")]
    public bool InfoOpen;
    public Text AreaDifficulty;
    public Text NumberofEnemy;
    public Text NumberofTraps;
    public Text BaseHealth;
    public Text DamageRange;
    public Text UnitPower;

    private StatsDistribution getStats = new StatsDistribution();
    private RatePointIndicator Profile = new RatePointIndicator();
    private QuickLook notice = new QuickLook();

    [Header("Additional Content")]
    public GameObject CheckCounter;
    public GameObject FinalPhaseDisplay;
    public GameObject DifficultyDisplay;

    [Header("LeaderBoard")]
    public GameObject[] leaderBoard_sign = new GameObject[6];
    public Text LeaderBoard_rankingMode;
    public Button Refresh_LeaderBoard;

    public GameObject[] BestRecordBoard = new GameObject[3];

    private string ResMelo = string.Empty;

    [SerializeField] private GameObject currentActiveInfo;

    private LocalLoad_DataManagement local;

    // Program: Music Selection Scene
    void Start()
    {
        thisSelect = this;
        local = new LocalLoad_DataManagement(LoginPage_Script.thisPage.GetUserPortOutput(),
            "StreamingAssets/LocalData/MeloMelo_LocalSave_InGameProgress");
        local.SelectFileForActionWithUserTag(MeloMelo_GameSettings.GetLocalFileSelectionData);

        // Load Content: Intit
        ResMelo = PlayerPrefs.GetString("Resoultion_Melo", string.Empty);

        // Marathon/Casual: Selection
        if (PlayerPrefs.GetInt("Marathon_Challenge", 0) == 0) { CasualPlay_Setup(); }
        else { MarathonChallenge_Setup(); }
    }

    #region SETUP
    private void CasualPlay_Setup()
    {
        selection = GetComponent<MusicSelectionPage>();

        // Main: Load all content
        LoadPlayerEconomy();
        LoadSelectionContent();
        ShowBeginnerNotice();

        // Init temp content
        if (CheckCounter.activeInHierarchy) { CheckCounter.SetActive(false); }

        int loadDifficultyName = 1;

        if (!PlayerPrefs.HasKey("LastSelection"))
        {
            switch (LoginPage_Script.thisPage.portNumber)
            {
                case (int)MeloMelo_GameSettings.LoginType.TempPass:
                    PlayerPrefs.SetInt("LastSelection", MeloMelo_GameSettings.GetLocalTrackSelectionLastVisited(PreSelection_Script.thisPre.get_AreaData.AreaName, 1));
                    loadDifficultyName = MeloMelo_GameSettings.GetLocalTrackSelectionLastVisited(PreSelection_Script.thisPre.get_AreaData.AreaName, 2);
                    break;

                default:
                    PlayerPrefs.SetInt("LastSelection", local.LoadSelectionPickProgress(PreSelection_Script.thisPre.get_AreaData.AreaName, 2));
                    loadDifficultyName = local.LoadSelectionPickProgress(PreSelection_Script.thisPre.get_AreaData.AreaName, 1);
                    break;
            }
        }
        else
            loadDifficultyName = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
    
        // Selection Transition: Execute
        StartCoroutine(OpeningSelection(true, loadDifficultyName));
    }

    private void MarathonChallenge_Setup()
    {

    }
    #endregion

    #region MAIN
    public void ClosingSelection()
    {
        GameObject.Find("Selection").GetComponent<Animator>().SetTrigger("Closing" + ResMelo);
        Invoke("CloseSelection", 2);
    }

    private IEnumerator OpeningSelection(bool enableSelection, int getDifficulty)
    {
        yield return new WaitForSeconds(0.5f);

        // Get Enable Selection: Display
        selection.get_difficulty_valve.GetComponent<Button>().interactable = enableSelection;
        GameObject.Find("DisableFeature_Difficulty").GetComponent<Animator>().SetBool("Disable", !enableSelection);

        // Get Difficulty Select: Display
        PlayerPrefs.SetInt("DifficultyLevel_valve", getDifficulty);
        if (!enableSelection) { DifficultyChanger_encode(); }

        // BGM: Play through background
        LoadAudio_Assigned();

        // Final-Phase: Display
        if (PlayerPrefs.GetInt("Marathon_Challenge") == 1)
        {
            if (PlayerPrefs.GetInt("MarathonChallenge_MCount") > 3)
            {
                FinalPhaseDisplay.SetActive(true);
                FinalPhaseDisplay.GetComponent<Animator>().SetBool("FinalPhase", true);
            }

            // Display Stage Counter
            CheckCounterDisplay();
        }
        else
        {
            if (FinalPhaseDisplay.activeInHierarchy) FinalPhaseDisplay.GetComponent<Animator>().SetBool("FinalPhase", false);
            FinalPhaseDisplay.SetActive(false);
        }

        loadBGM = true;
        selection.Invoke("Setup_Page", 0.1f);

        // Cursor
        if (!Cursor.visible) Cursor.visible = true;
    }
    #endregion

    #region COMPONENT 
    private void LoadSelectionContent()
    {
        GameObject.Find("BG").GetComponent<RawImage>().texture = PreSelection_Script.thisPre.get_AreaData.BG;
        GameObject.Find("Selection").GetComponent<Animator>().SetTrigger("Opening" + ResMelo);
        DifficultyArea[PlayerPrefs.GetInt("BattleDifficulty_Mode", 1) - 1].GetComponent<RawImage>().enabled = true;
    }

    private void LoadPlayerEconomy()
    {
        getStats.load_Stats();
        Profile.ProfileUpdate(LoginPage_Script.thisPage.GetUserPortOutput(), false, string.Empty, string.Empty);
    }

    private void ShowBeginnerNotice()
    {
        if (PlayerPrefs.GetString("HowToPlay_Notice", "T") == "T")
        {
            notice.Opening_QuickLook("Selection_Control");
            notice.NoticePlay(HowToPlay_Page, ButtonUI_HTP);
        }
    }
    #endregion

    #region COMPONENT (MARATHON)
    private void CheckCounterDisplay()
    {
        // Init supporting content
        int checkPoint_init = 3;

        // Display checkpoint view
        if (PlayerPrefs.GetInt("MarathonChallenge_MCount", 0) <= checkPoint_init)
        {
            CheckCounter.SetActive(true);
            CheckCounter.transform.GetChild(0).GetComponent<Text>().text = "Stage \n" +
                PlayerPrefs.GetInt("MarathonChallenge_MCount") + "/" + checkPoint_init;
        }
        else
        {
            CheckCounter.SetActive(false);
        }
    }
    #endregion

    #region MISC (Instruction)
    public void NextFunction_NoticePlay(bool next) { if (next) notice.NextFunction_NoticePlay(HowToPlay_Page, ButtonUI_HTP); else notice.CloseFunction_NoticePlay("HowToPlay_Notice"); }

    // UI_Input: Quick Look Icon
    public void Clickable_Icon(string label) { notice.Opening_QuickLook(label); }

    public void CloseFunction_All() { notice.CloseFunction_NoticePlay(string.Empty); }
    #endregion

    // Loader: Music Content

    protected void DifficultyChanger_encode() { selection.DifficultyChanger(false); }

    public void BattleMode(GameObject obj)
    {
        switch (obj.transform.GetChild(0).GetComponent<Text>().text)
        {
            case "PROCESS":
                if (PlayerPrefs.GetInt("GetMaxPoint_" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 0) != 0)
                {
                    GameObject.Find("Selection").GetComponent<Animator>().SetTrigger("Closing" + ResMelo);
                    StartCoroutine(GoBatleMode());
                }
                break;

            case "INSPECT SETUP":
                if (PlayerPrefs.GetInt("GetMaxPoint_" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 0) != 0)
                    InformationMode("Selection_Info");
                break;

            default:
                break;
        }
    }

    IEnumerator GoBatleMode()
    {
        yield return new WaitForSeconds(2);
        PlayerPrefs.SetInt("LastSelection", (int)selection.get_ScrollNagivator_ProgressBar.value);
        SceneManager.LoadScene("BattleSetup");
    }

    // Transition: Information Mode
    public void InformationMode(string search)
    {
        InfoOpen = true;
        currentActiveInfo.GetComponent<Animator>().SetTrigger("Closing" + ResMelo);
        StartCoroutine(GoInfoMode(search));
    }

    IEnumerator GoInfoMode(string search)
    {
        yield return new WaitForSeconds(2);
        currentActiveInfo = GameObject.Find(search);
        currentActiveInfo.GetComponent<Animator>().SetTrigger("Opening" + ResMelo);
        Invoke("GoInfo_" + search, 1.5f);
    }

    protected void GoInfo_Selection_Info() //UpdateDetail()
    {
        AreaDifficulty.text = "Area Difficulty: " + getStats.get_AreaDifficulty() + " Level";
        NumberofEnemy.text = "Number of Enemy: " + PlayerPrefs.GetInt("EnemyTakeCounter_" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 0);
        NumberofTraps.text = "Number of Traps: " + PlayerPrefs.GetInt("TrapsTakeCounter_" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 0);
        UnitPower.text = "Enemy Unit Power: " + getStats.get_UnitPower("Enemy");
        DamageRange.text = "Enemy Unit Damage Range: " + getStats.get_UnitDamage("Enemy");
        BaseHealth.text = "Enemy Unit Base Health:  " + getStats.get_UnitHealth("Enemy") + " (+" + PreSelection_Script.thisPre.get_AreaData.EnemyBaseHealth[PlayerPrefs.GetInt("BattleDifficulty_Mode", 1) - 1] + ")";

        // Load LeaderBoard
        RefreshContentBoard();
    }

    #region COMPONENT (TRACK LEADERBOARD)
    private void RefreshContentBoard()
    {
        if (GetLeaderBoardNetwork())
        {
            Refresh_LeaderBoard.interactable = true;
        }
        else
        {
            Refresh_LeaderBoard.interactable = false;
            BuildUpLeaderBoardContent(GuestLogin_Script.thisScript.get_entrytitle);
            BuildUpLeaderBoardDisplayPanel();
        }
    }

    private bool GetLeaderBoardNetwork()
    {
        bool condition = NetworkCofig.GetComponent<MeloMelo_NetworkChecker>().get_server;
        LeaderBoard_rankingMode.text = condition ? "GLOBAL RANKING" : "LOCAL RANKING";
        return condition;
    }

    private void BuildUpLeaderBoardContent(string[] entrylist)
    {
        foreach (string entry in entrylist)
        {
            if (entry != string.Empty)
            {
                int score = PlayerPrefs.GetInt(entry + "_" + selection.get_form.Title + "_score" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 0);
                string rank = PlayerPrefs.GetString(entry + "_" + selection.get_form.Title + "_rank" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), "--");               
                if (rank != "--") trackLeaderBoardList.Add(new TrackLeaderBoardEntry(entry, selection.get_form.Title, score, rank));
            }
        }
    }

    private void BuildUpLeaderBoardDisplayPanel()
    {
        trackLeaderBoardList.Sort((a, b) => a.score.CompareTo(b.score));
        trackLeaderBoardList.Reverse();

        foreach (TrackLeaderBoardEntry entry in trackLeaderBoardList)
        {
            leaderBoard_entry_title.text = entry.user;
            leaderBoard_entry_info.text = "Score: " + entry.score + " | Rank: " + entry.rank;
        }
    }
    #endregion

    public void GoInfoMode_Return(string search)
    {
        GameObject.Find(search).GetComponent<Animator>().SetTrigger("Closing" + ResMelo);
        Invoke("GoInfoMode2", 1.5f);
    }

    void GoInfoMode2() { InfoOpen = false; GameObject.Find("Selection").GetComponent<Animator>().SetTrigger("Opening"); }

    // Close Selection: Function
    void CloseSelection()
    {
        if (PlayerPrefs.GetInt("Marathon_Challenge", 0) == 0)
        {
            GameObject.Find("BG").GetComponent<RawImage>().texture = Resources.Load<Texture>("Background/BG1");
        }

        Invoke("CloseSelection2", 2);
    }

    void CloseSelection2()
    {
        if (GameObject.Find("BGM").activeInHierarchy && BGM != null) { Destroy(BGM); }
        PlayerPrefs.DeleteKey("LastSelection");

        if (PlayerPrefs.GetInt("Marathon_Challenge", 0) == 0) SceneManager.LoadScene("AreaSelection" + ResMelo);
        else { SceneManager.LoadScene("MarathonSelection"); }
    }

    // Profile Hovering Function   
    public void OpenProfile()
    {
        GameObject.Find("Profile").GetComponent<Animator>().SetTrigger("Open");
    }

    public void CloseProfile()
    {
        GameObject.Find("Profile").GetComponent<Animator>().SetTrigger("Close");
    }

    #region MISC
    private void LoadAudio_Assigned()
    {
        BGM = null;
        BGM = GameObject.Find("BGM");
        DontDestroyOnLoad(BGM);
    }

    public void GetSignActivation(int index, bool active)
    {
        switch (index)
        {
            case 1:
                AreaBonusSign.SetActive(active);
                break;

            case 2:
                NewReleaseSign.SetActive(active);
                break;
        }
    }
    #endregion
}
