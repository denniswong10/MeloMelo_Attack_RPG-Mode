using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MeloMelo_Local;
using MeloMelo_Network;

[System.Serializable]
struct MarathonChallengeProgress
{
    public string title;
    public bool clearedChallenge;
    public int playedCount;
    public int totalScore;

    public MarathonChallengeProgress GetProgressData(string format)
    {
        Debug.Log(format);
        return JsonUtility.FromJson<MarathonChallengeProgress>(format);
    }
}

[System.Serializable]
public partial class ContentSelectionLocker_Script
{
    [SerializeField] private GameObject[] challengeOptions_array;
    private string[] enchant_challengeList;
    private string[] optionCodeIndex;
    private MarathonInfo isLoadedContent;

    public ContentSelectionLocker_Script()
    {
        isLoadedContent = null;
        Setup();
    }

    #region SETUP
    private void Setup()
    {
        enchant_challengeList = new string[] { "", "", "Challenge", "Skills" };
        optionCodeIndex = new string[] { "", "", "C", "S" };
    }
    #endregion

    #region MAIN
    public IEnumerator RefreshContent()
    {
        // Store temp data for searching
        MarathonInfo contentLoad = null;
        int currentCheck = 0;

        // Find all options available to unlock
        for (int option = 0; option < challengeOptions_array.Length; option++)
        {
            // Get only active option is locked
            if (challengeOptions_array[option].transform.GetChild(2).gameObject.activeInHierarchy)
            {
                do
                {
                    currentCheck++;
                    ResourceRequest contentData = Resources.LoadAsync<MarathonInfo>("Database_Marathon/" + enchant_challengeList[option] + "/" + optionCodeIndex[option] + currentCheck);
                    yield return new WaitUntil(() => contentData.isDone);

                    contentLoad = contentData.asset as MarathonInfo;
                    if (contentLoad != null) isLoadedContent = contentLoad;
                } while (contentLoad != null);

                // Logic: Get condition unlock content
                currentCheck = 0;
                challengeOptions_array[option].GetComponent<Button>().interactable = PlayerPrefs.GetString("MarathonProgress_Cleared_" + isLoadedContent.title, "F") == "T";
                challengeOptions_array[option].transform.GetChild(2).gameObject.SetActive(PlayerPrefs.GetString("MarathonProgress_Cleared_" + isLoadedContent.title, "F") != "T");
                Debug.Log(option + ": Condition Unlock: Cleared " + isLoadedContent.title);
            }
        }
    }
    #endregion
}

[System.Serializable]
public partial class MainSelection_Script
{
    [Header("Component")]
    [SerializeField] private GameObject[] options_array;
    [SerializeField] private RawImage[] buttonOptions_array;
    [SerializeField] private ContentSelectionLocker_Script contentVisiblility;
    public ContentSelectionLocker_Script get_lockedContent { get { return contentVisiblility; } }

    [SerializeField] private GameObject[] CurrencyPanel;

    public MainSelection_Script()
    {
        contentVisiblility = new ContentSelectionLocker_Script();
    }

    #region SETUP
    public IEnumerator Setup()
    {
        yield return new WaitForSeconds(1);
        GetSelectionView(1);
    }
    #endregion

    #region MAIN
    public void ToggleSelection(int index)
    {
        // Get selection view in any order of index
        GetSelectionView(index);

        UpdateCurrencyInterface();
    }
    #endregion

    #region COMPONENT
    private void GetSelectionView(int indexOfSelection)
    {
        for (int instance = 0; instance < options_array.Length; instance++)
        {
            buttonOptions_array[instance].color = (indexOfSelection - 1) == instance ? Color.green : Color.white;
            options_array[instance].SetActive(indexOfSelection - 1 == instance);
        }
    }

    private void UpdateCurrencyInterface()
    {
        PlayerPrefs.SetInt(LoginPage_Script.thisPage.GetUserPortOutput() + "_" + MeloMelo_Economy.currencyTagInArray[(int)MeloMelo_Economy.CurrencyType.HonorCoin],
            MeloMelo_ItemUsage_Settings.GetActiveItem("HONOR COIN").amount);

        for (int panel = 0; panel < CurrencyPanel.Length; panel++)
            CurrencyPanel[panel].GetComponent<CurrencyInTag_Scripts>().UpdateCurrencyValue();
    }
    #endregion
}

public class MarathonSelection_Script : MonoBehaviour
{
    public static MarathonSelection_Script thisMarathon;

    private GameObject[] BGM;
    private readonly string marathonPermitKey = "MarathonPermit";

    [Header("Setup")]
    public GameObject StartOutPanel;
    public GameObject CompletionPanel;
    public GameObject ChallengeTemplateSlot;
    public GameObject SaveIcon;

    [Header("Main")]
    public GameObject[] selectionPanel;
    public GameObject Marathon_BeginWarning;

    [Header("Main Function: Script")]
    [SerializeField] private MainSelection_Script mainSelection;

    [Header("Extra Function: Transcation Script")]
    [SerializeField] private GameObject ItemDropForExchange;
    [SerializeField] private GameObject ItemContentTemplate;
    [SerializeField] private GameObject TranscationTicketing;
    private GameObject UsingTranscationTicketing = null;

    // Start is called before the first frame update
    void Start()
    {
        thisMarathon = this;
        BGM_ClearCache();

        // Get alert message ready
        int lengthToSucess = 0;

        if (PlayerPrefs.HasKey("MarathonPermit"))
        {
            lengthToSucess = PlayerPrefs.GetString("Marathon_Assigned_Task", string.Empty) != "CustomList" ?
            Resources.Load<MarathonInfo>(PlayerPrefs.GetString("Marathon_Assigned_Task", string.Empty)).Difficultylevel.Length :
                MeloMelo_ExtensionContent_Settings.LoadMarathonDetail(PlayerPrefs.GetInt("MarathonInstanceNumber", 0)).track_difficulty.Length;
        }

        if (PlayerPrefs.HasKey("MarathonPermit") && PlayerPrefs.GetInt("MarathonChallenge_MCount") > lengthToSucess)
            CompletionPanel.SetActive(true);
        else
            StartOutPanel.SetActive(true);

        StartCoroutine(AwaitContentCache());
    }

    void Update()
    {
        if (selectionPanel[(int)MarathonTask_Script.TaskSelector.MainOption].activeInHierarchy && mainSelection == null)
        {
            mainSelection = new MainSelection_Script();
            StartCoroutine(mainSelection.Setup());
        }        
    }

    #region SETUP
    private IEnumerator AwaitContentCache()
    {
        yield return new WaitUntil(() => !CompletionPanel.activeInHierarchy && !StartOutPanel.activeInHierarchy);
        yield return new WaitForSeconds(1);
        ToggleMainSelection(1);
        StartCoroutine(ExchangeLoaderThroughMarathon());
    }

    private void BGM_ClearCache()
    {
        BGM = GameObject.FindGameObjectsWithTag("BGM");
        if (BGM.Length > 1) { for (int i = 1; i < BGM.Length; i++) { Destroy(BGM[i]); } }
    }

    private void ClearAllContent()
    {
        // Remove everythings from challenge listing
        GameObject contentLog = GameObject.FindGameObjectWithTag("Challenge_Storage");
        for (int slot = 0; slot < contentLog.transform.childCount; slot++)
            Destroy(contentLog.transform.GetChild(slot).gameObject);
    }

    private void CreateInternalChallengeList(string context)
    {
        GameObject contentLog = GameObject.FindGameObjectWithTag("Challenge_Storage");
        ClearAllContent();

        // Get context with list of challenge slot are available
        foreach (MarathonInfo info in Resources.LoadAll<MarathonInfo>("Database_Marathon/" + context))
        {
            // Challenge slot database must be ready for review
            if (info.trackList.Length == info.Difficultylevel.Length)
                CreateChallengeListArray(info.title, contentLog, info.Difficultylevel, info.GetConditionDetails());
        }
    }

    private void CreateExternalChallengeList(string context)
    {
        GameObject contentLog = GameObject.FindGameObjectWithTag("Challenge_Storage");

        // Get context with list of challenge slot are available
        for (int info = 0; info < MeloMelo_ExtensionContent_Settings.totalMarathonCount; info++)
        {
            // Challenge slot database must be ready for review
            BuildInChallengeInfo temp = MeloMelo_ExtensionContent_Settings.LoadMarathonDetail(info);

            if (temp.track_difficulty.Length == temp.track_title.Length && context == temp.directory_title)
                CreateChallengeListArray(temp.title, contentLog, temp.track_difficulty, temp.GetConditionDetails());
        }
    }

    private void CreateChallengeListArray(string title, GameObject target, string[] level_difficulty_array, string description)
    {
        // Create challenge slot with quick detail
        GameObject slot = Instantiate(ChallengeTemplateSlot, target.transform);
        slot.name = title;
        slot.transform.GetChild(0).GetComponent<Text>().text = slot.name;
        slot.transform.GetChild(1).GetComponent<Text>().text = description;

        // Display the stage slot with difficulty level
        for (int coverIndex = 0; coverIndex < level_difficulty_array.Length; coverIndex++)
            slot.GetComponent<ChallengeTask_Scripts>().CoverImage_List[coverIndex].transform.GetChild(0).GetComponent<Text>().text =
                "Lv " + level_difficulty_array[coverIndex];
    }

    private void ListDownChallengeDetail()
    {
        // Search detail from loaded progress data
        try { MarathonInfo info = Resources.Load<MarathonInfo>(PlayerPrefs.GetString("Marathon_Assigned_Task", string.Empty)); LoadChallengeDetailCache(info.title); }
        catch 
        { 
            BuildInChallengeInfo info = new BuildInChallengeInfo();
            info = MeloMelo_ExtensionContent_Settings.LoadMarathonDetail(PlayerPrefs.GetInt("MarathonInstanceNumber", 0));
            LoadChallengeDetailCache(info.title); 
        }
    }

    private void LoadChallengeDetailCache(string title)
    {
        // Played Count: Display
        selectionPanel[(int)MarathonTask_Script.TaskSelector.ConfirmationStart].transform.GetChild(4).GetChild(0).GetComponent<Text>().text =
            "Played Count: " + PlayerPrefs.GetInt("MarathonProgress_PlayedCount_" + title, 0);

        // Total Score: Display
        string finalScore = PlayerPrefs.GetInt("MarathonProgress_Score_" + title, 0) > 0 ?
            PlayerPrefs.GetInt("MarathonProgress_Score_" + title, 0).ToString("#,#") : "NOT PLAYED";

        selectionPanel[(int)MarathonTask_Script.TaskSelector.ConfirmationStart].transform.GetChild(5).GetChild(0).GetComponent<Text>().text =
            "Total Score: " + finalScore;

        // Status: Display
        selectionPanel[(int)MarathonTask_Script.TaskSelector.ConfirmationStart].transform.GetChild(6).GetChild(0).GetComponent<Text>().text =
            "Status: " + (PlayerPrefs.GetString("MarathonProgress_Cleared_" + title, "F") == "T" ? "CLEARED!" :
            PlayerPrefs.GetInt("MarathonProgress_PlayedCount_" + title, 0) > 0 ? "In Progress!" : "OPEN");
    }
    #endregion

    #region MAIN
    public void GoPreviousSelection()
    {
        // Checking for active selection
        bool isSelectionAvailable = false;

        // Start to find and close up selection which are active
        for (int current = 0; current < selectionPanel.Length; current++)
        {
            // Open up previous selection
            if (selectionPanel[current].activeInHierarchy && current - 1 > -1)
            {
                // Active selection has been found
                isSelectionAvailable = true;
                GetAnimatorSelection(current, false);
                GetAnimatorSelection(current - 1, true);
                break;
            }
        }

        // Selection not found and then execute the next line
        if (!isSelectionAvailable)
        {
            PlayerPrefs.DeleteKey("MarathonPermit");
            Invoke("GetExitMarathon", 1);
        }
    }

    public void DoSelectionPatcher()
    {
        // Perform selection toggle upon task given from the indivdual selection
        Invoke("PerformInitPatcher", 0.5f);
    }   

    public void StartAlertMarathonBegin(Button target)
    {
        target.interactable = false;
        Marathon_BeginWarning.SetActive(true);
    }

    public void CancelMarathonPlay(Button target)
    {
        target.interactable = true;
        Marathon_BeginWarning.SetActive(false);
    }

    public void StartMarathon()
    {
        // Start marathon play when confirmation details is proper
        GetAnimatorSelection((int)MarathonTask_Script.TaskSelector.ConfirmationStart, false);
        Invoke("BeginPlayMarathon", 1.5f);
    }
    #endregion

    #region COMPONENT (Update Content)
    private void PerformInitPatcher()
    {
        for (int selection = 0; selection < selectionPanel.Length; selection++)
        {
            // Find current active selection and perform following task
            if (selectionPanel[selection].activeInHierarchy && selectionPanel[selection].GetComponent<MarathonTask_Script>().taskHasChosen)
            {
                // First Patch: Setup and display the content accordingly
                switch (selectionPanel[selection].GetComponent<MarathonTask_Script>().choiceOfAction)
                {
                    // For alert message: Uses
                    case MarathonTask_Script.TaskSelector.Entry:
                        if (!PlayerPrefs.HasKey(marathonPermitKey)) Invoke("GetExitMarathon", 1);
                        else GetAnimatorSelection(0, true);
                        break;

                    // For selection panel: Uses
                    default:
                        // Close the current selection
                        GetAnimatorSelection(selection, false);

                        // Get existing selection ready to play
                        if (selectionPanel[selection + 1] != null)
                        {
                            // Open up ready selection
                            GetAnimatorSelection(selection + 1, true);

                            // Title: Label text from previous selection to the ready selection
                            selectionPanel[selection + 1].transform.GetChild(1).GetComponent<Text>().text =
                            selectionPanel[selection].GetComponent<MarathonTask_Script>().GetSelectionTitle(
                                selectionPanel[selection + 1].transform.GetChild(1).GetComponent<Text>().text, false);

                            // Perform final patch
                            PerformFinalPatcher(selectionPanel[selection].GetComponent<MarathonTask_Script>().choiceOfAction, selection);
                        }
                        break;
                }

                // Reset task chosen and put previous selection to non-active
                selectionPanel[selection].GetComponent<MarathonTask_Script>().taskHasChosen = false;
                selectionPanel[selection].SetActive(false);
                break;
            }
        }
    }

    private void PerformFinalPatcher(MarathonTask_Script.TaskSelector action_type, int currentSelection)
    {
        // Final Patch: Perform extra content on the following indivdual selection type
        switch (action_type)
        {
            case MarathonTask_Script.TaskSelector.MainOption:
                // Get previous task action to create all challenge listing
                CreateInternalChallengeList(selectionPanel[currentSelection].GetComponent<MarathonTask_Script>().GetSelectionTitle(string.Empty, true));

                // Extra: Custom Challenge
                CreateExternalChallengeList(selectionPanel[currentSelection].GetComponent<MarathonTask_Script>().GetSelectionTitle(string.Empty, true));
                break;

            case MarathonTask_Script.TaskSelector.ChallengeSelection:
                // Get previous task action to send out marathon detail
                bool isMarathonLocal = false;

                foreach (MarathonInfo findInfo in Resources.LoadAll<MarathonInfo>("Database_Marathon/" + 
                    selectionPanel[(int)action_type - 1].GetComponent<MarathonTask_Script>().GetSelectionTitle(string.Empty, true) + "/"))
                {
                    if (findInfo.title == selectionPanel[(int)action_type].GetComponent<MarathonTask_Script>().GetSelectionTitle(string.Empty, true)) 
                    {
                        LocalChallengePickUp(selectionPanel[(int)action_type - 1].GetComponent<MarathonTask_Script>().GetSelectionTitle(string.Empty, true),
                        selectionPanel[(int)action_type].GetComponent<MarathonTask_Script>().GetSelectionTitle(string.Empty, true));
                        isMarathonLocal = true;
                    }
                }

                if (!isMarathonLocal)
                {
                    Debug.Log("Custom Marathon: " + selectionPanel[(int)action_type].GetComponent<MarathonTask_Script>().GetSelectionTitle(string.Empty, true));
                    GlobalChallengePickUp(selectionPanel[(int)action_type].GetComponent<MarathonTask_Script>().GetSelectionTitle(string.Empty, true));
                }
                
                // Wait for current task is been process before sending out detail
                Invoke("ListDownChallengeDetail", 0.5f);
                break;
        }
    }
    #endregion

    #region COMPONENT (Update Script Component)
    private void GetAnimatorSelection(int selectionIndex, bool isOpening)
    {
        // Only if the task isn't entry condition
        if (selectionIndex < (int)MarathonTask_Script.TaskSelector.Entry)
        {
            // Get active and perform selection animation
            selectionPanel[selectionIndex].SetActive(isOpening);
            selectionPanel[selectionIndex].transform.GetComponent<Animator>().SetTrigger(isOpening ? "Opening" : "Closing");
        }
    }

    private void LocalChallengePickUp(string playOption, string challengeTitle)
    {
        // Get marathon info as for confirmation to play the challenge
        string actionReference = "Database_Marathon/" + playOption + "/";
        MarathonInfo currentSelection = null;

        // Find and assign info
        foreach (MarathonInfo findInfo in Resources.LoadAll<MarathonInfo>("Database_Marathon/" + playOption))
            if (findInfo.title == challengeTitle) { currentSelection = findInfo; break; }

        // Check through current selection is able for use
        if (currentSelection != null)
        {
            List<string> localAreaArray = new List<string>();
            List<string> localTitleArray = new List<string>();

            foreach (TrackDetails detail in currentSelection.trackList)
            {
                localAreaArray.Add(detail.areaName);
                localTitleArray.Add(detail.title);
            }

            // Peform task info
            UpdateChallengeInfo(currentSelection.title, localAreaArray.ToArray(), localTitleArray.ToArray(), currentSelection.GetConditionDetails());

            // Setup marathon cache
            CreateMarathonSetup(localTitleArray.ToArray(), localAreaArray.ToArray(), currentSelection.title, actionReference + currentSelection.name);

            // Setup life when selection rules is enable
            if (currentSelection.clearingType == MarathonInfo.ClearedMethod.Life)
            {
                // Clear cache for life during the previous selection
                ClearAllLifePointSetup();

                // Adjust any available life value to this selection
                foreach (JudgementAddons addons in currentSelection.conditionAddons)
                    CreateLifePointSetup((int)addons.judgeTitle - 1, addons.judgeCount);
            }

            // Clear previous selection cache receiving data
            ClearMarathonCache();
        }
    }

    private void GlobalChallengePickUp(string challengeTitle)
    {
        BuildInChallengeInfo customSelection = new BuildInChallengeInfo();
        
        // Find and assign info
        for (int instance = 0; instance < MeloMelo_ExtensionContent_Settings.totalMarathonCount; instance++)
        {
            if (MeloMelo_ExtensionContent_Settings.LoadMarathonDetail(instance).title == challengeTitle)
            {
                customSelection = MeloMelo_ExtensionContent_Settings.LoadMarathonDetail(instance);
                PlayerPrefs.SetInt("MarathonInstanceNumber", instance);
                break;
            }
        }

        // Peform task info
        UpdateChallengeInfo(customSelection.title, customSelection.track_area, customSelection.track_title, customSelection.GetConditionDetails());

        // Setup marathon cache
        CreateMarathonSetup(customSelection.track_title, customSelection.track_area, customSelection.title, "CustomList");

        // Setup life when selection rules is enable
        if (customSelection.conditionType == 2)
        {
            // Adjust all life value to this selection
            for (int index = 0; index < customSelection.condition_data.Split(",").Length; index++)
                CreateLifePointSetup(index, int.Parse(customSelection.condition_data.Split(",")[index]));
        }

        // Clear previous selection cache receiving data
        ClearMarathonCache();
    }

    private void UpdateChallengeInfo(string title, string[] areaList, string[] titleList, string description)
    {
        // Assigned details according to the number of difficulty been set
        for (int challenge = 0; challenge < titleList.Length; challenge++)
        {
            // Find music info and update cover details
            if (Resources.Load<MusicScore>("Database_Area/" + areaList[challenge] + "/" + titleList[challenge]) != null)
            {
                // Display Stage Cover
                GameObject.Find("Stage" + (challenge + 1)).transform.GetChild(0).GetChild(0).GetComponent<RawImage>().texture =
                    challenge + 1 < (PlayerPrefs.GetString("MarathonProgress_Cleared_" + title, "F") == "T" ? 1 : 0) +
                    (PlayerPrefs.GetInt("MarathonProgress_PlayedCount_" + title, 0) > 0 ? titleList.Length : 0) ?

                    Resources.Load<MusicScore>("Database_Area/" + areaList[challenge] + "/" + titleList[challenge]).Background_Cover :
                    Resources.Load<MusicScore>("Database_Area/Template_MusicScore").Background_Cover;
            }
        }

        // Display Logic Board action
        selectionPanel[(int)MarathonTask_Script.TaskSelector.ConfirmationStart].transform.GetChild(17).GetChild(1).gameObject.SetActive
            (PlayerPrefs.GetString("MarathonProgress_Cleared_" + title, "F") == "T" ? false : true);

        selectionPanel[(int)MarathonTask_Script.TaskSelector.ConfirmationStart].transform.GetChild(17).GetChild(0).gameObject.SetActive
            (PlayerPrefs.GetInt("MarathonProgress_PlayedCount_" + title, 0) > 0 ? false : true);

        // Display Cleared description
        selectionPanel[(int)MarathonTask_Script.TaskSelector.ConfirmationStart].transform.GetChild(3).GetComponent<Text>().text = description;

        // Agree to set an action
        //selectionPanel[(int)MarathonTask_Script.TaskSelector.ConfirmationStart].GetComponent<MarathonTask_Script>().
           //AllowActionTaken(actionReference + challengeTitle);
    }
    #endregion

    #region MAIN (Progress Handler)
    public void SaveProgress()
    {
        // Adding progress to written files from assets
        string progressTitle = PlayerPrefs.GetString("Marathon_Assigned_Task", string.Empty) != "CustomList" ?
                Resources.Load<MarathonInfo>(PlayerPrefs.GetString("Marathon_Assigned_Task", string.Empty)).title :
                    MeloMelo_ExtensionContent_Settings.LoadMarathonDetail(PlayerPrefs.GetInt("MarathonInstanceNumber", 0)).title;

        // Perform save progress
        StartCoroutine(LocalProgressHandler(true, progressTitle));
    }

    public void LoadProgress()
    {
        // Perform load progress
        StartCoroutine(LocalProgressHandler(false));
    }
    #endregion

    #region COMPONENT (Progress Handler)
    private IEnumerator LocalProgressHandler(bool isSavingProgress, string title_reference = "")
    {
        // User: Checking data from file if there is any
        SaveIcon.SetActive(true);
        SaveIcon.transform.GetChild(1).GetComponent<Text>().text = "Checking Data...";
        yield return new WaitForSeconds(0.5f);

        SaveIcon.transform.GetChild(1).GetComponent<Text>().text = (isSavingProgress ? "Saving" : "Load") + " Progress...";

        switch (LoginPage_Script.thisPage.portNumber)
        {
            case (int)MeloMelo_PlayerSettings.LoginType.GuestLogin:
                yield return new WaitUntil(() => OnLocalProgressInAction(title_reference, isSavingProgress));
                break;

            case (int)MeloMelo_PlayerSettings.LoginType.TempPass:
                if (isSavingProgress)
                {
                    CloudSave_DataManagement onSaveProgress = new CloudSave_DataManagement(LoginPage_Script.thisPage.GetUserPortOutput(), MeloMelo_PlayerSettings.GetWebServerUrl());
                    string serverAPI = "MeloMelo_Save_MarathonProgress_2025.php";
                    WWWForm progress = new WWWForm();

                    progress.AddField("User", LoginPage_Script.thisPage.GetUserPortOutput());
                    progress.AddField("Title", title_reference);
                    progress.AddField("Status", PlayerPrefs.GetInt("Marathon_Quest_Result", 0) == 1 ? "T" : "F");
                    progress.AddField("PlayedCount", PlayerPrefs.GetInt("MarathonProgress_PlayedCount_" + title_reference, 0) + 1);
                    progress.AddField("Score", PlayerPrefs.GetInt("Marathon_All_Score", 0));

                    onSaveProgress.GetServerToSave(serverAPI, progress);
                    yield return new WaitUntil(() => onSaveProgress.get_process.ToArray().Length == onSaveProgress.get_counter);
                }
                else
                {
                    CloudLoad_DataManagement onLoadProgress = new CloudLoad_DataManagement(LoginPage_Script.thisPage.GetUserPortOutput(), MeloMelo_PlayerSettings.GetWebServerUrl());
                    StartCoroutine(onLoadProgress.LoadMarathonProgress());
                    yield return new WaitUntil(() => onLoadProgress.cloudLogging.ToArray().Length == onLoadProgress.get_counter);
                }
                break;

            default:
                break;
        }

        // User: Give a sign up when everything is done
        SaveIcon.transform.GetChild(1).GetComponent<Text>().text = (isSavingProgress ? "Save" : "Load") + " Completed!";
        yield return new WaitForSeconds(0.5f);
        SaveIcon.GetComponent<Animator>().SetTrigger("Close");

        DoSelectionPatcher();
    }

    private bool OnLocalProgressInAction(string playTitle, bool isCurrentProgressSaved)
    {
        // Create assets for marathon progress list
        LocalData_MarathonDatabase progress = new LocalData_MarathonDatabase
                    (
                        LoginPage_Script.thisPage.GetUserPortOutput(),
                        "StreamingAssets/LocalData/MeloMelo_LocalSave_InGameProgress"
                    );

        // Get files attracted with player name
        progress.SelectFileForActionWithUserTag("savelog_MarathonPass_Data.txt");

        // Get progress action going for loading and saving
        switch (isCurrentProgressSaved)
        {
            case false:
                // Adding progress from written files to assets
                progress.LoadProgress();
                break;

            default:
                progress.SaveProgress
                        (
                            playTitle,
                            PlayerPrefs.GetInt("Marathon_Quest_Result", 0) == 1 ? true : false,
                            PlayerPrefs.GetInt("MarathonProgress_PlayedCount_" + playTitle, 0) + 1,
                            PlayerPrefs.GetInt("Marathon_All_Score", 0)
                        );
                break;
        }

        return true;
    }
    #endregion

    #region MISC (Redirect Scene)
    private void BeginPlayMarathon()
    {
        // Enter to Music Selection Stage
        SceneManager.LoadScene("Music Selection Stage");
    }

    private void GetExitMarathon()
    {
        // Leave this scene
        PlayerPrefs.SetInt("MarathonPass_Eternal", 1);
        SceneManager.LoadScene("Ref_PreSelection");
    }
    #endregion

    #region MISC (Cache Information Handler)
    private void ClearMarathonCache()
    {
        // Cache information through gameplay
        PlayerPrefs.SetInt("MarathonChallenge_MCount", 1); // Get current stage
        PlayerPrefs.DeleteKey("Marathon_Quest_Score"); // Score used in the current stage
        PlayerPrefs.DeleteKey("Marathon_Quest_ScoreAddons"); // Score used in the past stage
        PlayerPrefs.DeleteKey("Marathon_All_Score"); // Total up track score for all
    }

    private void CreateMarathonSetup(string[] titleS, string[] areaS, string main_title, string assigned_name)
    {
        // Cache information when leave scene
        PlayerPrefs.SetString("Marathon_Assigned_Task", assigned_name);
        PlayerPrefs.SetString("Marathon_Title_Text", main_title);

        for (int track = 0; track < titleS.Length; track++)
        {
            // Score clearing
            PlayerPrefs.DeleteKey("TrackListRecord_Score" + track);

            // Track Assign setup
            PlayerPrefs.SetString("Marathon_Assigned_Area_" + track, "Database_Area/" + areaS[track] + "/" + titleS[track]);
        }
    }

    private void CreateLifePointSetup(int typeOfJudge, int numberOfDeduction)
    {
        string[] judgeTitle = { "Critical_Perfect", "Perfect", "Bad", "Miss" };
        PlayerPrefs.DeleteKey(typeOfJudge + "_Deduct");
        if (typeOfJudge < judgeTitle.Length) PlayerPrefs.SetInt(judgeTitle[typeOfJudge] + "_Deduct", numberOfDeduction);
    }
    
    private void ClearAllLifePointSetup()
    {
        string[] judgeTitle = { "Critical_Perfect", "Perfect", "Bad", "Miss" };
        foreach (string judge in judgeTitle) PlayerPrefs.DeleteKey(judge + "_Deduct");
    }
    #endregion

    #region MISC 
    public void ToggleMainSelection(int index)
    {
        if (mainSelection != null)
            mainSelection.ToggleSelection(index);

        if (mainSelection.get_lockedContent != null)
            StartCoroutine(mainSelection.get_lockedContent.RefreshContent());
    }
    #endregion

    #region MISC (Exchange)
    private IEnumerator ExchangeLoaderThroughMarathon()
    {
        if (MeloMelo_Economy.exchangeContentOfMarathon != null && MeloMelo_Economy.exchangeContentOfMarathon.ToArray().Length > 0)
        {
            foreach (MarathonExchangeContent content in MeloMelo_Economy.exchangeContentOfMarathon)
            {
                ResourceRequest isItemAvailable = Resources.LoadAsync<ItemData>("Database_Item/#" + content.itemId);
                yield return isItemAvailable;

                ItemData itemUsedForShow = isItemAvailable.asset as ItemData;

                if (itemUsedForShow != null)
                {
                    GameObject exchangeData = Instantiate(ItemContentTemplate, ItemDropForExchange.transform);
                    exchangeData.GetComponent<Exchange_SelectionEditor>().GetSlotUpdateItem(itemUsedForShow);
                    exchangeData.GetComponent<Exchange_SelectionEditor>().GetSlotUpdatePricing(content.costAmount);
                }
            }

            if (ItemDropForExchange.transform.childCount > 1)
                Destroy(ItemDropForExchange.transform.GetChild(0).gameObject);
        }
    }

    public void MakeExchangeThroughTranscation(ItemData item, int costInItem)
    {
        if (UsingTranscationTicketing == null)
        {
            GameObject transcationBundeSet = Instantiate(TranscationTicketing, selectionPanel[(int)MarathonTask_Script.TaskSelector.MainOption].transform);
            UsingTranscationTicketing = transcationBundeSet.transform.GetChild(0).gameObject;
        }

        UsingTranscationTicketing.GetComponent<InGamePurchase_Transcation_PayRoll>().GetTranscation(item, costInItem, MeloMelo_Economy.CurrencyType.HonorCoin);
    }
    #endregion
}
