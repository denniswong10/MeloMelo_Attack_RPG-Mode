using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MeloMelo_VirtualItem;

public class Marathon_Selection : MonoBehaviour
{
    public static Marathon_Selection thisSelect;
    private string userInput;

    private GameObject[] BGM;
    public GameObject Selection;
    public GameObject Selection_Entry;
    private string ResMelo = string.Empty;
    private Resolution resolutions;

    private string[] getTitle = { "Rookie", "Amateur", "Master", "GrandMaster", "Emperor", "God", "GameMaster" };
    public string[] getTitle_ref { get { return getTitle; } }

    private int maxChallengeList = 0;
    private int currentScrollList = 0;
    public int maxChallengeTitle = 1;
    private MarathonInfo database_info = null;
    public MarathonInfo get_info_challenge { get { return database_info; } }

    public Button[] NagivatorButton = new Button[2];
    public Slider scrollBar_ui;
    public GameObject[] CoverContent = new GameObject[4];
    public Text ObjectiveTitle;
    public Text AreaDes;
    public GameObject[] AlertMessage = new GameObject[2];

    public GameObject[] Selection_Content;
    public GameObject LoadingBar;
    public GameObject StartMarathonEntry;
    private bool checkData = false;

    private InventoryManagement InventoryData;
    public InventoryManagement Get_InventoryData { get { return InventoryData; } }

    void Start()
    {
        thisSelect = this;
        checkData = false;
        LoadAudio_Assigned();

        InventoryData = new InventoryManagement();

        if (PlayerPrefs.HasKey("MarathonChallenge_Admission"))
        {
            if (PlayerPrefs.HasKey("Marathon_ServerData")) Setup_MarathonEntry();
            else { StartCoroutine(GetServerDatabaseForMarathon()); }
        }
        else { SetupAsBeginner(); }
    }

    #region SETUP
    private void LoadAudio_Assigned()
    {
        BGM = GameObject.FindGameObjectsWithTag("BGM");
        if (BGM.Length > 1) { for (int i = 1; i < BGM.Length; i++) { Destroy(BGM[i]); } }
    }

    private void SetupAsBeginner()
    {
        AlertMessage[0].SetActive(true);
    }

    private void Setup_MarathonEntry()
    {
        OpeningTransitionToSelection(Selection_Entry, string.Empty);
    }
    #endregion

    #region COMPONENT (TRANSITION)
    private void OpeningTransitionToSelection(GameObject target, string LoadAfterTransit)
    {
        target.GetComponent<Animator>().SetTrigger("Opening");
        if (LoadAfterTransit != string.Empty) Invoke(LoadAfterTransit, 2);
    }

    private void ClosingTransitionToSelection(GameObject target, string LoadAfterTransit)
    {
        target.GetComponent<Animator>().SetTrigger("Closing");
        if (LoadAfterTransit != string.Empty) Invoke(LoadAfterTransit, 2);
    }
    #endregion

    #region MAIN
    public void BeginSetupAsBegginer()
    {
        AlertMessage[0].SetActive(false);
        PlayerPrefs.SetInt("MarathonChallenge_Admission", 1);
        SceneManager.LoadScene("MarathonSelection");
    }

    public void LeaveChallenge()
    {
        ClosingTransitionToSelection(Selection_Entry, string.Empty);

        PlayerPrefs.DeleteKey("LastPlay_MarathonChallenge");
        PlayerPrefs.DeleteKey("Marathon_Challenge");
        PlayerPrefs.DeleteKey("MarathonChallenge_MCount");
        PlayerPrefs.DeleteKey("Marathon_ServerData");

        if (InventoryData.Get_UnsaveItem) { StartCoroutine(SaveDatabase_Item()); }
        if (PlayerPrefs.HasKey("MarathonPremit")) { StartCoroutine(SaveDatabase_Challenge()); }
        StartCoroutine(WaitingForLeave());
    }
    #endregion

    #region COMPONENT (DATABASE)
    private IEnumerator WaitingForLeave()
    {
        yield return new WaitUntil(() => (!PlayerPrefs.HasKey("MarathonPremit") && !InventoryData.Get_UnsaveItem));
        SceneManager.LoadScene("Ref_PreSelection");
    }
    #endregion

    public void ContentSelect(int index)
    {
        for (int i = 0; i < Selection_Content.Length; i++)
        {
            if (i == index) { Selection_Content[i].SetActive(true); }
            else { Selection_Content[i].SetActive(false); }
        }

        CheckingForEntryContent(index);
    }

    private void CheckingForEntryContent(int index)
    {
        switch (index)
        {
            case 1: // Entry

                // Check All Pass
                //for (int i = 0; i < 4; i++)
                //{
                //    if (InventoryData.CheckForItem(i+1)) GameObject.Find("Entry" + (i+1)).GetComponent<Button>().interactable = true;
                //    else GameObject.Find("Entry" + (i+1)).GetComponent<Button>().interactable = false;
                //}

                // Check Entry Button
                if (PlayerPrefs.HasKey("MarathonEntry_Admitted")) 
                {
                    StartMarathonEntry.transform.GetChild(0).GetComponent<Text>().text = "CONTINUE";
                    StartMarathonEntry.GetComponent<Button>().interactable = true; 
                }
                else 
                {
                    StartMarathonEntry.transform.GetChild(0).GetComponent<Text>().text = "NEW ENTRY";
                    StartMarathonEntry.GetComponent<Button>().interactable = false; 
                }

                break;

            case 2: // Exchange
                break;

            default: // Shop

                // Check for purchase status
                for (int i = 0; i < 4; i++)
                {
                    if (!InventoryData.CheckForItem(i + 1)) GameObject.Find("PassEntry_" + (i + 1)).GetComponent<Button>().interactable = true;
                    else GameObject.Find("Entry" + (i + 1)).GetComponent<Button>().interactable = false;
                }

                break;
        }
    }

    public void SelectedEntryMode(int mode)
    {
        PlayerPrefs.SetInt("SelectMode_EntryMarthon", mode);
        StartMarathonEntry.GetComponent<Button>().interactable = true;
    }

    public void Encoder_SetupChallengeList()
    {
        ResMelo = PlayerPrefs.GetString("Resoultion_Melo", string.Empty);
        Selection_Entry.GetComponent<Animator>().SetTrigger("Closing" + ResMelo);

        // Able to continue
        PlayerPrefs.SetString("MarathonEntry_Admitted", "T");

        // Get Pass Entry
        if (PlayerPrefs.GetInt("SelectMode_EntryMarthon") == 1) { PlayerPrefs.SetInt("MarathonChallenge_TitleList", 1); }
        else if (PlayerPrefs.GetInt("SelectMode_EntryMarthon") == 2) { PlayerPrefs.SetInt("MarathonChallenge_TitleList", 3); }
        else if (PlayerPrefs.GetInt("SelectMode_EntryMarthon") == 3) { PlayerPrefs.SetInt("MarathonChallenge_TitleList", 5); }
        else if (PlayerPrefs.GetInt("SelectMode_EntryMarthon") == 4) { PlayerPrefs.SetInt("MarathonChallenge_TitleList", 7); }

        Invoke("Encoder_2_SetupChallengeList", 2);
    }

    private void Encoder_2_SetupChallengeList()
    {
        ResMelo = PlayerPrefs.GetString("Resoultion_Melo", string.Empty);
        Selection.GetComponent<Animator>().SetTrigger("Opening" + ResMelo);
        Invoke("Setup_ChallengeStage", 1.5f);
    }

    // Transition -> LoadingBar -> Setup_MarthonEntry
    private IEnumerator GetServerDatabaseForMarathon()
    {
        // Get LoginPage_Script on user input
        try { userInput = LoginPage_Script.thisPage.get_user; } catch { userInput = "GUEST"; }

        // Open LoadingBar for checking
        LoadingBar.SetActive(true);

        if (userInput == "GUEST") { LoadingBar.transform.GetChild(1).GetComponent<Text>().text = "Game Loading..."; }
        else { LoadingBar.transform.GetChild(1).GetComponent<Text>().text = "Checking Data..."; }

        // Server Coding...
        if (!checkData)
        {
            checkData = true;
            if (userInput == "GUEST") PlayerPrefs.SetInt("Marathon_ServerData", 0);
            else { StartCoroutine(InventoryData.LoadItemFromGlobal(userInput)); }
        }
        yield return new WaitForSeconds(3);

        // Check server data have transfer to local
        if (userInput != "GUEST")
        {
            if (PlayerPrefs.HasKey("Marathon_ServerData")) { LoadingBar.transform.GetChild(1).GetComponent<Text>().text = "Completed!"; }
            else { LoadingBar.transform.GetChild(1).GetComponent<Text>().text = "Failed!"; }
        }

        // Prepare LoadingBar to be close
        checkData = false;
        Invoke("GetMarathonLoadTransition", 1.5f);
    }

    private void GetMarathonLoadTransition()
    {
        LoadingBar.GetComponent<Animator>().SetTrigger("Close");
        Invoke("TransitionBackMarathon", 2);
    }

    private void TransitionBackMarathon() { SceneManager.LoadScene("MarathonSelection"); }

    // Setup: maxChallengeList
    private void Setup_ChallengeStage()
    {
        maxChallengeList = ChallengeListAssemble(PlayerPrefs.GetInt("MarathonChallenge_TitleList"));
        UpdateChallengeList(true);
    }

    private int ChallengeListAssemble(int index)
    {
        if (index == 7) { return 4; } // GameMaster
        if (index == 5 || index == 6) { return 10; } // Emperor or God
        else if (index == 3 || index == 4) { return 5; } // Master or GrandMaster
        else if (index == 2) { return 3; } // Amateur
        else { return 3; } // Rookie
    }

    // NagivationButton -> UpdateChallenegeList
    public void NagivationButton_Click(int index)
    {
        switch (index)
        {
            case 1:
                currentScrollList = currentScrollList - 1;
                break;

            case 2:
                currentScrollList = currentScrollList + 1;
                break;

            default:
                break;
        }

        UpdateChallengeList(false);
    }

    // LoadData: Challenge Interface
    private void UpdateChallengeList(bool SetAsDefault)
    {
        // Set user interface value
        if (SetAsDefault) { currentScrollList = PlayerPrefs.GetInt("LastPlay_MarathonChallenge", 1); }

        // Update Nagivator Button
        if (currentScrollList <= 1) NagivatorButton[0].interactable = false; 
        else NagivatorButton[0].interactable = true;

        if (currentScrollList >= maxChallengeList) NagivatorButton[1].interactable = false;
        else NagivatorButton[1].interactable = true;

        // Update the program interface
        scrollBar_ui.maxValue = maxChallengeList;
        scrollBar_ui.value = currentScrollList;
        scrollBar_ui.transform.GetChild(0).GetComponent<Text>().text = scrollBar_ui.value + "/" + scrollBar_ui.maxValue;

        // Update content data through interface
        database_info = Resources.Load<MarathonInfo>("Database_Marathon/" + 
            getTitle[PlayerPrefs.GetInt("MarathonChallenge_TitleList") - 1] + "/C" + scrollBar_ui.value);
        PreLoadCoverContent();
        LoadAllDescription();
    }

    // LoadData: Individual Challenge Information
    private void PreLoadCoverContent()
    {
        for (int i = 0; i < CoverContent.Length; i++)
        {
            CoverContent[i].transform.GetChild(0).GetComponent<Text>().text = "Lv" + database_info.pre_initDifficulty[i];

            // Clear LoadText
            CoverContent[i].transform.GetChild(1).gameObject.SetActive(false);

            // Cleared!
            CoverContent[i].transform.GetChild(2).gameObject.SetActive
                (PlayerPrefs.GetInt(database_info.title + "_Content_" + (i + 1) + "_Cleared", 0) == 1 ? true : false);

            // Not Clear!
            CoverContent[i].transform.GetChild(3).gameObject.SetActive
                (PlayerPrefs.GetInt(database_info.title + "_Content_" + (i + 1) + "_Cleared", 0) == 2 ? true : false);

            // Hightlight
            if (i == 3) { CoverContent[i].GetComponent<Animator>().SetBool("Caution", true); }
        }

        CheckForClearedList();
        CheckForCompleteTitleList();
    }

    // LoadData: Set Objective to challenge
    private void LoadAllDescription()
    {
        AreaDes.text = database_info.title + "\n" + "[" + getTitle[database_info.Decode_Level() - 1] + " LEVEL]";
        ObjectiveTitle.text = "Objective \n \n" + database_info.Decode_ObjectiveDes();
    }

    // Transition -> Music Seleciton Stage
    public void ProcessToSelection()
    {
        // Loading off the selection screen
        Selection.GetComponent<Animator>().SetTrigger("Closing" + ResMelo);
        Invoke("ProcessToSelection_2", 1.5f);
    }

    // Encode -> Transition(Music Seleciton Stage)
    public void ProcessToSelection_2()
    {
        // Challenge Start: From 1st Stage
        PlayerPrefs.SetInt("LastPlay_MarathonChallenge", currentScrollList);
        PlayerPrefs.SetInt("MarathonChallenge_MCount", 1);
        PlayerPrefs.SetInt("Marathon_Challenge", 1);
        SceneManager.LoadScene("Music Selection Stage");
    }

    public void CheckForCompleteTitleList()
    {
        MarathonInfo check = null;
        int count = 0;

        // Check for all challenge: Cleared!
        for (int i = 0; i < maxChallengeList; i++)
        {
            check = Resources.Load<MarathonInfo>("Database_Marathon/" + database_info.Decode_LevelByName() + "/C" + (i + 1));
            if (check.challengeClear) { count++; }
        }

        // All challenge clear: Ready to process to the next phase
        if (count == maxChallengeList) Invoke("PromoteNextTitle", 2);
    }

    public void CheckForClearedList()
    {
        int count = 0;

        // Check for all stage: Cleared!
        for (int i = 0; i < 4; i++)
        {
            if (PlayerPrefs.GetInt(database_info.title + "_Content_" + (i + 1) + "_Cleared", 0) == 1) { count++; }
        }

        // Mark as challenge cleared
        if (count == 4) { database_info.challengeClear = true; }
    }

    public void PromoteNextTitle()
    {
        // Loading off the next title screen
        Selection.GetComponent<Animator>().SetTrigger("Closing" + ResMelo);
        Invoke("PromoteNextTitle_2", 1.5f);
    }

    public void PromoteNextTitle_2()
    {
        // Promote Display: Start Respone
        AlertMessage[1].SetActive(true);
        switch(PlayerPrefs.GetInt("MarathonChallenge_TitleList"))
        {
            case 1: // Adavnce to A
            case 2: // Advance to MST
            case 3: // Advance to GMST
            case 4:
                AlertMessage[1].transform.GetChild(3).GetComponent<Button>().interactable = true;
                break;

            default:
                AlertMessage[1].transform.GetChild(3).GetComponent<Button>().interactable = false;
                break;
        }
    }

    public void AcceptNextChallenge()
    {
        // Update a next challenge
        int nextTitle = PlayerPrefs.GetInt("MarathonChallenge_TitleList");
        PlayerPrefs.SetInt("MarathonChallenge_TitleList", nextTitle + 1);

        AlertMessage[1].SetActive(false);
        Setup_ChallengeStage();
        Selection.GetComponent<Animator>().SetTrigger("Opening");

        // Restart the BoardList
        //SceneManager.LoadScene("MarathonSelection");
    }

    private IEnumerator SaveDatabase_Item()
    {
        LoadingBar.SetActive(true);
        if (userInput != "GUEST") 
        { 
            LoadingBar.transform.GetChild(1).GetComponent<Text>().text = "Cloud Database (Item) \n Saving Data...";
            InventoryData.SaveItemToGlobal(userInput);
        }
        else { LoadingBar.transform.GetChild(1).GetComponent<Text>().text = "Local Database (Item) \n Checking Data..."; }
        yield return new WaitForSeconds(3);

        if (userInput != "GUEST")
        {
            if (PlayerPrefs.HasKey("ItemSaved_Completed")) LoadingBar.transform.GetChild(1).GetComponent<Text>().text = "Cloud Database (Item) \n Save Succesful!";
            else LoadingBar.transform.GetChild(1).GetComponent<Text>().text = "Cloud Database (Item) \n Save Failed!";
        }
        else { LoadingBar.transform.GetChild(1).GetComponent<Text>().text = "Local Database (Item) \n Completed!"; }

        Invoke("SaveDatabase_Item_2", 2);
    }

    private void SaveDatabase_Item_2()
    {
        // Leave Menu
        InventoryData.ClearCache_ItemDatabase();
    }

    private IEnumerator SaveDatabase_Challenge()
    {
        LoadingBar.SetActive(true);
        if (userInput != "GUEST")
        {
            LoadingBar.transform.GetChild(1).GetComponent<Text>().text = "Saving Data...";
            // Server Coding
        }
        else { LoadingBar.transform.GetChild(1).GetComponent<Text>().text = "Checking Data..."; }
        yield return new WaitForSeconds(3);

        if (userInput != "GUEST")
        {
            if (PlayerPrefs.HasKey("ChallengeSaved_Completed")) LoadingBar.transform.GetChild(1).GetComponent<Text>().text = "Save Successful!";
            else LoadingBar.transform.GetChild(1).GetComponent<Text>().text = "Save Failed!";
        }
        else { LoadingBar.transform.GetChild(1).GetComponent<Text>().text = "Game Loading..."; }

        Invoke("SaveDatabase_Challenge_2", 2);
    }

    private void SaveDatabase_Challenge_2()
    {
        // Leave Menu
        PlayerPrefs.DeleteKey("MarathonPremit");
    }
}
