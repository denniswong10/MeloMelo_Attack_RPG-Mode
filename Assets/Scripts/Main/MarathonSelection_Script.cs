using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    // Start is called before the first frame update
    void Start()
    {
        BGM = GameObject.FindGameObjectsWithTag("BGM");
        if (BGM.Length > 1) { for (int i = 1; i < BGM.Length; i++) { Destroy(BGM[i]); } }

        // Get alert message ready
        if (PlayerPrefs.HasKey("MarathonPermit") && PlayerPrefs.GetInt("MarathonChallenge_MCount") 
            > Resources.Load<MarathonInfo>(PlayerPrefs.GetString("Marathon_Assigned_Task", string.Empty)).Difficultylevel.Length)
            CompletionPanel.SetActive(true);
        else
            StartOutPanel.SetActive(true);
    }

    #region SETUP
    private void ClearAllContent()
    {
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
        if (!isSelectionAvailable) Invoke("GetExitMarathon", 1);
    }

    public void DoSelectionPatcher()
    {
        // Perform selection toggle upon task given from the indivdual selection
        Invoke("PerformInitPatcher", 0.5f);
    }   

    public void StartMarathon()
    {
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
                ChallengePickedUp(selectionPanel[(int)MarathonTask_Script.TaskSelector.MainOption].GetComponent<MarathonTask_Script>().GetSelectionTitle(string.Empty, true),
                    selectionPanel[(int)MarathonTask_Script.TaskSelector.ChallengeSelection].GetComponent<MarathonTask_Script>().GetSelectionTitle(string.Empty, true));
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
                if (challenge + 1 < currentSelection.Difficultylevel.Length)
                    GameObject.Find("Stage" + (challenge + 1)).transform.GetChild(0).GetChild(0).GetComponent<RawImage>().texture =
                        Resources.Load<MusicScore>(actionReference + challengeTitle + "/M" + (challenge + 1)).Background_Cover;
            }
        }

        // Display Cleared description
        selectionPanel[(int)MarathonTask_Script.TaskSelector.ConfirmationStart].transform.GetChild(3).GetComponent<Text>().text = 
            currentSelection.GetConditionDetails();

        // Agree to set an action
        selectionPanel[(int)MarathonTask_Script.TaskSelector.ConfirmationStart].GetComponent<MarathonTask_Script>().
            AllowActionTaken(actionReference + challengeTitle);

        PlayerPrefs.SetString("Marathon_Assigned_Task", actionReference + currentSelection.name);
        PlayerPrefs.SetString("Marathon_Assigned_Area", actionReference + challengeTitle);
        PlayerPrefs.SetInt("MarathonChallenge_MCount", 1);
    }
    #endregion

    #region MAIN (Progress Handler)
    public void SaveProgress()
    {
        StartCoroutine(SaveLocalData());
    }

    private IEnumerator SaveLocalData()
    {
        SaveIcon.SetActive(true);
        SaveIcon.transform.GetChild(1).GetComponent<Text>().text = "Checking Data...";
        yield return new WaitForSeconds(0.5f);

        SaveIcon.transform.GetChild(1).GetComponent<Text>().text = "Saving Progress...";
        yield return new WaitForSeconds(1);

        SaveIcon.transform.GetChild(1).GetComponent<Text>().text = "Progress Error!";
        yield return new WaitForSeconds(0.5f);
        SaveIcon.GetComponent<Animator>().SetTrigger("Close");

        DoSelectionPatcher();
    }

    public void LoadProgress()
    {
        StartCoroutine(LoadLocalData());
    }

    private IEnumerator LoadLocalData()
    {
        SaveIcon.SetActive(true);
        SaveIcon.transform.GetChild(1).GetComponent<Text>().text = "Checking Data...";
        yield return new WaitForSeconds(0.5f);

        SaveIcon.transform.GetChild(1).GetComponent<Text>().text = "Load Progress...";
        yield return new WaitForSeconds(0.5f);

        SaveIcon.transform.GetChild(1).GetComponent<Text>().text = "Progress Error!";
        yield return new WaitForSeconds(0.5f);
        SaveIcon.GetComponent<Animator>().SetTrigger("Close");

        DoSelectionPatcher();
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
        SceneManager.LoadScene("Ref_PreSelection");
    }
    #endregion
}
