using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MeloMelo_Story;

class AdventureRouteManagement
{
    public int totalStage { get; private set; }
    public int totalRoute { get; private set; }

    public int currentStage { get; private set; }

    public AdventureRouteManagement()
    {
        totalStage = 0;
        totalRoute = 0;
        currentStage = 0;
    }

    #region MAIN
    public void SetStage(int amount) => totalStage = amount;
    public void SetRoute(int amount) => totalRoute = amount;
    public void ChangeStage(int amount) => currentStage = amount;
    #endregion

    #region MISC
    public bool IsRouteReachedLimit(int currentRouteIndex) { return currentRouteIndex == totalRoute - 1; }
    public bool IsPastRouteAvailable(int currentRouteIndex) { return currentRouteIndex == 0 && currentStage > 0; }
    #endregion
}

public class StoryMode_Scripts : MonoBehaviour
{
    public static StoryMode_Scripts thisStory;
    private GameObject[] BGM;
    private readonly string[] storyType = { "Main Story", "Event Story" };

    [Header("Main UI - Channel")]
    public GameObject selection_main;
    public GameObject story_playscreen;

    public enum Menu_Selection_Route
    {
        LeftBtn = 1, RightBtn,
        RouteIcon = 4, StorySwither = 7, Location
    }

    [SerializeField] private GameObject SubMenu_Route_Detail;
    [SerializeField] private GameObject SubMenu_Route_Description;
    [SerializeField] private GameObject PromptMessagePop;
    [SerializeField] private GameObject StoryTransitionPop;

    private int currentAreaIndex;

    [Header("Content")]
    private StoryInfo[] storyAssets;
    private AdventureRouteManagement settings;
    public MusicScore missionTrack { get; private set; }

    [Header("Story Material Component")]
    [SerializeField] private Text storyTxt_writeOn;
    [SerializeField] private GameObject story_continueBtn;

    // Alert Box: Component
    private bool isPropmptOpen;
    private Queue<string> itemMessages;

    // Start is called before the first frame update
    void Start()
    {
        thisStory = this;
        isPropmptOpen = false;
        itemMessages = new Queue<string>();
        BGM_Setup();

        settings = new AdventureRouteManagement();
        PlayerPrefs.DeleteKey("Mission_Played");
        PlayerPrefs.DeleteKey("GatheringMode");

        selection_main.GetComponent<Animator>().SetTrigger("Opening");
        StartCoroutine(ReloadStoryAdventureAssets(storyType[PlayerPrefs.GetInt("StoryTypePlayBack", 0)]));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) ReturnToMain();
    }

    #region SETUP
    private void BGM_Setup()
    {
        BGM = GameObject.FindGameObjectsWithTag("BGM");
        if (BGM.Length > 1) { for (int i = 1; i < BGM.Length; i++) { Destroy(BGM[i]); } }
    }

    private IEnumerator ReloadStoryAdventureAssets(string storyType)
    {
        ClearRoute();
        yield return new WaitForSeconds(2);

        storyAssets = null;
        storyAssets = Resources.LoadAll<StoryInfo>("Database_Story/" + storyType);
        
        currentAreaIndex = GetCurrentAreaProgress();
        StoryTransitionPop.GetComponentInChildren<Text>().text = storyType;
        StoryTransitionPop.SetActive(true);

        yield return new WaitForSeconds(1);
        LoadAdventureInfo();
        StoryTransitionPop.SetActive(false);
    }

    private void LoadAdventureInfo()
    {
        selection_main.transform.GetChild((int)Menu_Selection_Route.RouteIcon).GetComponent<RawImage>().texture = storyAssets[currentAreaIndex].StoryBG;
        selection_main.transform.GetChild((int)Menu_Selection_Route.Location).GetComponent<Text>().text = storyAssets[currentAreaIndex].StoryTitle;
        SubMenu_Route_Description.GetComponent<RoutePanel_DescriptionScript>().UpdateCurrentStage(storyAssets[currentAreaIndex].Stage[settings.currentStage]);

        settings.SetStage(storyAssets[currentAreaIndex].Stage.Length);
        RouteLoader();       
    }

    private IEnumerator RefreshAdventureInfo(int routePointerPos)
    {
        yield return new WaitUntil(() => SubMenu_Route_Detail.transform.childCount > 0);

        // Refresh context to all available route log
        for (int id = 0; id < SubMenu_Route_Detail.transform.childCount; id++)
        {
            // Toggle current selection point
            const int checker_id = 4;
            SubMenu_Route_Detail.transform.GetChild(id).GetChild(checker_id).gameObject.SetActive(id == routePointerPos);
        }

        // Update context info in description
        SlotQuestLog currentLog = storyAssets[currentAreaIndex].Stage[settings.currentStage].myQuestLog[routePointerPos];

        SubMenu_Route_Description.GetComponent<RoutePanel_DescriptionScript>().UpdateTitleContext(currentLog.logTitle, 
            GetIndexForChapter(currentLog), currentLog.mySlotTypte);

        SubMenu_Route_Description.GetComponent<RoutePanel_DescriptionScript>().UpdateDescriptionContext(currentLog);

        RefreshNagivatorButtonIndex((int)Menu_Selection_Route.LeftBtn, routePointerPos > 0 || settings.IsPastRouteAvailable(routePointerPos));
        RefreshNagivatorButtonIndex((int)Menu_Selection_Route.RightBtn, routePointerPos < settings.totalRoute - 1 || settings.currentStage < settings.totalStage - 1);
        RefreshNagivatorButtonIndex((int)Menu_Selection_Route.StorySwither, true);
    }

    private void RouteLoader()
    {
        foreach (SlotQuestLog routeRole in storyAssets[currentAreaIndex].Stage[settings.currentStage].myQuestLog)
            SubMenu_Route_Detail.GetComponent<RoutePanel_VisualScript>().LoadRoute((int)routeRole.mySlotTypte);

        settings.SetRoute(storyAssets[currentAreaIndex].Stage[settings.currentStage].myQuestLog.Length);
        SubMenu_Route_Detail.GetComponent<RoutePanel_VisualScript>().UpdateRouteDetail(settings.currentStage + 1, settings.totalStage);

        PlayerPrefs.DeleteKey("StoryMode_Route_Selection_Id");
        StartCoroutine(RefreshAdventureInfo(PlayerPrefs.GetInt("StoryMode_Route_Selection_Id", 0)));
    }

    private void ClearRoute()
    {
        for (int instance = 0; instance < SubMenu_Route_Detail.transform.childCount; instance++)
            Destroy(SubMenu_Route_Detail.transform.GetChild(instance).gameObject);
    }

    private void RefreshNagivatorButtonIndex(int button_index, bool condition)
    {
        Button buttonInteract = selection_main.transform.GetChild(button_index).GetComponent<Button>();
        buttonInteract.interactable = condition;
    }
    #endregion

    #region MAIN
    public void NagivatorBtn(bool previous)
    {
        int currentSelectedRoute = PlayerPrefs.GetInt("StoryMode_Route_Selection_Id", 0);

        RefreshNagivatorButtonIndex((int)Menu_Selection_Route.LeftBtn, false);
        RefreshNagivatorButtonIndex((int)Menu_Selection_Route.RightBtn, false);

        if ((!previous && settings.IsRouteReachedLimit(currentSelectedRoute)) || (previous && settings.IsPastRouteAvailable(currentSelectedRoute)))
        {
            ClearRoute();
            settings.ChangeStage(settings.currentStage + (previous ? -1 : 1));
            LoadAdventureInfo();
        }
        else
        {
            int isToggling = currentSelectedRoute + (previous ? -1 : 1);
            StartCoroutine(RefreshAdventureInfo(isToggling));
            PlayerPrefs.SetInt("StoryMode_Route_Selection_Id", isToggling);
        }
    }

    public void SwitchStoryModeBtn()
    {
        int currentStoryType = PlayerPrefs.GetInt("StoryTypePlayBack") == 0 ? 1 : 0;
        settings.ChangeStage(0);

        RefreshNagivatorButtonIndex((int)Menu_Selection_Route.LeftBtn, false);
        RefreshNagivatorButtonIndex((int)Menu_Selection_Route.RightBtn, false);
        RefreshNagivatorButtonIndex((int)Menu_Selection_Route.StorySwither, false);

        StartCoroutine(ReloadStoryAdventureAssets(storyType[currentStoryType]));
        PlayerPrefs.SetInt("StoryTypePlayBack", currentStoryType);
    }

    public void RegisterMissionTrack(MusicScore track)
    {
        missionTrack = track;
    }

    public void ReturnToMain()
    {
        SceneManager.LoadScene("Ref_PreSelection");
    }
    #endregion

    #region MISC
    private int GetIndexForChapter(SlotQuestLog referenceFrom)
    {
        int chapterIndex = 1;

        foreach (FragmentInfo currentStage in storyAssets[currentAreaIndex].Stage)
        {
            foreach (SlotQuestLog currentLog in currentStage.myQuestLog)
            {
                if (referenceFrom == currentLog) return chapterIndex;
                else if (currentLog.mySlotTypte == referenceFrom.mySlotTypte) chapterIndex++;
            }
        }

        return -1;
    }

    private int GetCurrentAreaProgress()
    {
        int current = 0;

        foreach (StoryInfo story in storyAssets)
        {
            string searchFormat = "Title Deed: " + story.StoryTitle + " Story Play";
            VirtualItemDatabase item = MeloMelo_ItemUsage_Settings.GetActiveItem(searchFormat);

            if (item.itemName != searchFormat) return current;
            else current++;
        }

        return storyAssets.Length - 1;
    }

    public StoryInfo GetStoryArea()
    {
        return storyAssets[currentAreaIndex]; 
    }

    public bool IsTitleDeedPresented()
    {
        string searchFormat = "Title Deed: " + storyAssets[currentAreaIndex].StoryTitle + " Story Play";
        VirtualItemDatabase item = MeloMelo_ItemUsage_Settings.GetActiveItem(searchFormat);
        return storyAssets[currentAreaIndex].StoryTitle == item.itemName;
    }
    #endregion

    #region MISC (Old Story Builder)
    public void BeginStoryMode(string storyTitle)
    {
        StoryTool myTool = new StoryTool();
        myTool.StoryTxtExtraction(storyTitle);

        if (PlayerPrefs.HasKey("Display_Story"))
        {
            story_playscreen.GetComponent<Animator>().SetTrigger("Opening");
            StartCoroutine(myTool.StoryTxtEffect());
            StartCoroutine(StoryTellerPlayer());
        }
    }

    // Channel 3: Continue Button Interactive
    public void ContinueBtn_StoryMode()
    {
        story_continueBtn.SetActive(false);
        story_playscreen.GetComponent<Animator>().SetTrigger("Closing");

        storyTxt_writeOn.text = string.Empty;
        story_playscreen.GetComponent<AudioSource>().clip = null;
    }

    protected IEnumerator StoryTellerPlayer()
    {
        yield return new WaitForSeconds(1);
        if (GameObject.Find("BGM")) { GameObject.Find("BGM").GetComponent<AudioSource>().volume = 0.2f; }
        if (story_playscreen.GetComponent<AudioSource>().clip != null) { story_playscreen.GetComponent<AudioSource>().Play(); }

        yield return new WaitUntil(() => !story_playscreen.GetComponent<AudioSource>().isPlaying);
        if (GameObject.Find("BGM")) { GameObject.Find("BGM").GetComponent<AudioSource>().volume = 1; }
    }

    public void StoryLoaderPlayer(string storyRef)
    {
        try
        {
            AudioClip StoryTeller = Resources.Load<AudioClip>("Story/" + storyRef);
            story_playscreen.GetComponent<AudioSource>().clip = StoryTeller;
        }
        catch { }
    }

    public void StoryDisplayTxt(char print) { storyTxt_writeOn.text += print; }
    public void StoryDisplayContinue() { story_continueBtn.SetActive(true); }
    #endregion

    #region MISC (Rewarding System)
    public void ItemMessageAlert(string message)
    {
        itemMessages.Enqueue(message);

        if (!isPropmptOpen)
        {
            isPropmptOpen = true;
            StartCoroutine(GetItemDisplayPrompt());
        }
    }

    private IEnumerator GetItemDisplayPrompt()
    {
        while (itemMessages.Count > 0)
        {
            PromptMessagePop.SetActive(true);
            PromptMessagePop.GetComponentInChildren<Text>().text = itemMessages.Dequeue();
            yield return new WaitForSeconds(2);
        }

        PromptMessagePop.SetActive(false);
        isPropmptOpen = false;
    }
    #endregion 
}