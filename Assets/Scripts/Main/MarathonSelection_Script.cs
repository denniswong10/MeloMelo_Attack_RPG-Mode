using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MeloMelo_Local;

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
        if (GameObject.Find("Credit_Currency") != null)
            GameObject.Find("Credit_Currency").GetComponentInChildren<Text>().text = 
                PlayerPrefs.GetInt(LoginPage_Script.thisPage.GetUserPortOutput() + "_Credit", 0).ToString();

        if (GameObject.Find("QuestCleared_Currency") != null)
            GameObject.Find("QuestCleared_Currency").GetComponentInChildren<Text>().text = "0";
    }
    #endregion
}

public class MarathonSelection_Script : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        BGM_ClearCache();

        // Get alert message ready
        if (PlayerPrefs.HasKey("MarathonPermit") && PlayerPrefs.GetInt("MarathonChallenge_MCount") 
            > Resources.Load<MarathonInfo>(PlayerPrefs.GetString("Marathon_Assigned_Task", string.Empty)).Difficultylevel.Length)
            CompletionPanel.SetActive(true);
        else
            StartOutPanel.SetActive(true);
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

    private void CreateChallengeList(string context)
    {
        GameObject contentLog = GameObject.FindGameObjectWithTag("Challenge_Storage");
        ClearAllContent();

        // Get context with list of challenge slot are available
        foreach (MarathonInfo info in Resources.LoadAll<MarathonInfo>("Database_Marathon/" + context))
        {
            // Challenge slot database must be ready for review
            if (Resources.LoadAll<MusicScore>("Database_Marathon/" + context + "/" + info.title).Length == info.Difficultylevel.Length)
            {
                // Create challenge slot with quick detail
                GameObject slot = Instantiate(ChallengeTemplateSlot, contentLog.transform);
                slot.name = info.title;
                slot.transform.GetChild(0).GetComponent<Text>().text = slot.name;
                slot.transform.GetChild(1).GetComponent<Text>().text = info.GetConditionDetails();

                // Display the stage slot with difficulty level
                for (int coverIndex = 0; coverIndex < slot.GetComponent<ChallengeTask_Scripts>().CoverImage_List.Length; coverIndex++)
                    slot.GetComponent<ChallengeTask_Scripts>().CoverImage_List[coverIndex].transform.GetChild(0).GetComponent<Text>().text =
                        "Lv " + info.Difficultylevel[coverIndex];
            }
        }
    }

    private void ListDownChallengeDetail()
    {
        // Search detail from loaded progress data
        MarathonInfo info = Resources.Load<MarathonInfo>(PlayerPrefs.GetString("Marathon_Assigned_Task", string.Empty));

        // Played Count: Display
        selectionPanel[(int)MarathonTask_Script.TaskSelector.ConfirmationStart].transform.GetChild(4).GetChild(0).GetComponent<Text>().text =
            "Played Count: " + PlayerPrefs.GetInt("MarathonProgress_PlayedCount_" + info.title, 0);

        // Total Score: Display
        selectionPanel[(int)MarathonTask_Script.TaskSelector.ConfirmationStart].transform.GetChild(5).GetChild(0).GetComponent<Text>().text =
            "Total Score: " + PlayerPrefs.GetInt("MarathonProgress_Score_" + info.title, 0);

        // Status: Display
        selectionPanel[(int)MarathonTask_Script.TaskSelector.ConfirmationStart].transform.GetChild(6).GetChild(0).GetComponent<Text>().text =
            "Status: " + (PlayerPrefs.GetString("MarathonProgress_Cleared_" + info.title, "F") == "T" ? "CLEARED!" :
            PlayerPrefs.GetInt("MarathonProgress_PlayedCount_" + info.title, 0) > 0 ? "In Progress!" : "OPEN");
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
                CreateChallengeList(selectionPanel[currentSelection].GetComponent<MarathonTask_Script>().GetSelectionTitle(string.Empty, true));
                break;

            case MarathonTask_Script.TaskSelector.ChallengeSelection:
                // Get previous task action to send out marathon detail
                ChallengePickedUp(selectionPanel[(int)action_type - 1].GetComponent<MarathonTask_Script>().GetSelectionTitle(string.Empty, true),
                    selectionPanel[(int)action_type].GetComponent<MarathonTask_Script>().GetSelectionTitle(string.Empty, true));
                
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

    private void ChallengePickedUp(string playOption, string challengeTitle)
    {
        // Get marathon info as for confirmation to play the challenge
        string actionReference = "Database_Marathon/" + playOption + "/";
        MarathonInfo currentSelection = null;

        // Find and assign info
        foreach (MarathonInfo findInfo in Resources.LoadAll<MarathonInfo>("Database_Marathon/" + playOption))
            if (findInfo.title == challengeTitle) { currentSelection = findInfo; break; }

        // Assigned details according to the number of difficulty been set
        for (int challenge = 0; challenge < currentSelection.Difficultylevel.Length; challenge++)
        {
            // Find music info and update cover details
            if (Resources.Load<MusicScore>(actionReference + challengeTitle + "/M" + (challenge + 1)) != null)
            {
                // Display Stage Cover
                GameObject.Find("Stage" + (challenge + 1)).transform.GetChild(0).GetChild(0).GetComponent<RawImage>().texture =
                    challenge + 1 < (PlayerPrefs.GetString("MarathonProgress_Cleared_" + currentSelection.title, "F") == "T" ? 1 : 0) + 
                    (PlayerPrefs.GetInt("MarathonProgress_PlayedCount_" + currentSelection.title, 0) > 0 ? currentSelection.Difficultylevel.Length : 0) ? 

                    Resources.Load<MusicScore>(actionReference + challengeTitle + "/M" + (challenge + 1)).Background_Cover :
                    Resources.Load<MusicScore>("Database_Area/Template_MusicScore").Background_Cover;
            }
        }

        // Display Logic Board action
        selectionPanel[(int)MarathonTask_Script.TaskSelector.ConfirmationStart].transform.GetChild(17).GetChild(1).gameObject.SetActive
            (PlayerPrefs.GetString("MarathonProgress_Cleared_" + currentSelection.title, "F") == "T" ? false : true);

        selectionPanel[(int)MarathonTask_Script.TaskSelector.ConfirmationStart].transform.GetChild(17).GetChild(0).gameObject.SetActive
            (PlayerPrefs.GetInt("MarathonProgress_PlayedCount_" + currentSelection.title, 0) > 0 ? false : true);

        // Display Cleared description
        selectionPanel[(int)MarathonTask_Script.TaskSelector.ConfirmationStart].transform.GetChild(3).GetComponent<Text>().text = 
            currentSelection.GetConditionDetails();

        // Agree to set an action
        selectionPanel[(int)MarathonTask_Script.TaskSelector.ConfirmationStart].GetComponent<MarathonTask_Script>().
            AllowActionTaken(actionReference + challengeTitle);

        // Clear all cache
        CreateMarathonSetup(currentSelection, actionReference, challengeTitle);
        ClearMarathonCache();
    }
    #endregion

    #region MAIN (Progress Handler)
    public void SaveProgress()
    {
        // Perform save progress
        StartCoroutine(LocalProgressHandler(true));
    }

    public void LoadProgress()
    {
        // Perform load progress
        StartCoroutine(LocalProgressHandler(false));
    }
    #endregion

    #region COMPONENT (Progress Handler)
    private IEnumerator LocalProgressHandler(bool isSavingProgress)
    {
        // User: Checking data from file if there is any
        SaveIcon.SetActive(true);
        SaveIcon.transform.GetChild(1).GetComponent<Text>().text = "Checking Data...";
        yield return new WaitForSeconds(0.5f);

        // User: Get type of progress handler while it is performing
        SaveIcon.transform.GetChild(1).GetComponent<Text>().text = (isSavingProgress ? "Saving" : "Load") + " Progress...";
        yield return new WaitUntil(() => IsProgressInAction(isSavingProgress));

        // User: Give a sign up when everything is done
        SaveIcon.transform.GetChild(1).GetComponent<Text>().text = (isSavingProgress ? "Save" : "Load") + " Completed!";
        yield return new WaitForSeconds(0.5f);
        SaveIcon.GetComponent<Animator>().SetTrigger("Close");

        DoSelectionPatcher();
    }

    private bool IsProgressInAction(bool isCurrentProgressSaved)
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
                // Adding progress to written files from assets
                MarathonInfo currentProgress = Resources.Load<MarathonInfo>(PlayerPrefs.GetString("Marathon_Assigned_Task", string.Empty));
                progress.SaveProgress
                    (
                        currentProgress.title,
                        PlayerPrefs.GetInt("Marathon_Quest_Result", 0) == 1 ? true : false, 
                        PlayerPrefs.GetInt("MarathonProgress_PlayedCount_" + currentProgress.title, 0) + 1,
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

    private void CreateMarathonSetup(MarathonInfo info, string reference, string title)
    {
        // Cache information when leave scene
        PlayerPrefs.SetString("Marathon_Assigned_Task", reference + info.name);
        PlayerPrefs.SetString("Marathon_Assigned_Area", reference + title);
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
}
