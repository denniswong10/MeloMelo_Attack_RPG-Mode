using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class StartMenu_Script : MonoBehaviour
{
    public static StartMenu_Script thisMenu;

    // Scene Inspector
    [SerializeField] private Animator GameTitle_Background;
    [SerializeField] private Text startEnable;
    [SerializeField] private GameObject GameLoader_Icon;
    private GameObject LoadingScreen = null;

    [Header("Game Application")]
    [SerializeField] private int seasonOutput;
    [SerializeField] private int versionIndex;
    [SerializeField] private string buildLabel;
    public string version { get; private set; }

    [Header("Web Application: URL")]
    private string serverURL;
    private string latestBuild = string.Empty;

    // Other Component: Get
    public string get_serverURL { get { return serverURL; } }
    public string get_latestV { get { return latestBuild; } }
    public int get_seasonNum { get { return seasonOutput; } }
    public int get_versionNum { get { return versionIndex; } }

    public GameObject UpdateAlert;
    public GameObject ConnectionAlert;

    private Coroutine rebootingProgram;
    private float inputCooldown = 0.2f;
    private float inputTimer = 0f;
    private bool isStartEnable = false;

    // Program: Start Scene
    void Start()
    {
        thisMenu = this;
        rebootingProgram = null;

        Setup();
    }

    // Transition --> From StartMenu_Transition
    void Update()
    {
        if (isStartEnable)
            return;

        inputTimer += Time.deltaTime;

        if (ReadyToLaunch() && inputTimer >= inputCooldown)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && rebootingProgram == null)
            {
                isStartEnable = true;
                inputTimer = 0f;
                rebootingProgram = StartCoroutine(Reboot_Application());
            }
            else if (Input.anyKeyDown)
            {
                isStartEnable = true;
                inputTimer = 0f;
                LoadGameApplication();
            }
        }
    }

    #region SETUP
    private void Setup()
    {
        serverURL = MeloMelo_PlayerSettings.GetWebServerUrl();
        PlayerPrefs.DeleteKey("GameLatest_Update");
        StartCoroutine(DelayedBootScreen());
    }

    private bool ReadyToLaunch()
    {
        return startEnable.color.a == 1;
    }

    private IEnumerator DelayedBootScreen()
    {
        yield return new WaitForSeconds(3f);
        BootGameTitleScreen();
    }

    private void BootGameTitleScreen()
    {
        GameTitle_Background.SetTrigger("Open");
        version = Application.version + "." + seasonOutput + "." + versionIndex + buildLabel;
    }

    private void CheckParameterData()
    {
        LoadAllInGameAsset();

        if (PlayerPrefs.GetString("GameLatest_Update", string.Empty) != version && MeloMelo_PlayerSettings.GetLocalUserAccount())
        {
            UpdateAlert.SetActive(true);
            UpdateAlert.transform.GetChild(3).GetComponent<Text>().text = GetUpdateInfo();
            GetComponent<UpdateConfigPatcher>().enabled = true;
        }
        else
        {
            ReloadDisplay("[Game Loading]\n Completed!");
            StartCoroutine(GetGateWayScene());
        }
    }

    private void CheckParemterData_Connect()
    {
        LoadAllInGameAsset();
        ConnectionAlert.SetActive(true);
    }

    private void LoadGameApplication()
    {
        string loadingText = "[Game Loading]\nInitialize...";
        //MeloMelo_ExtensionContent_Settings.UpdateCharacterProfile();

        GetLoaderDisplay(
            loadingText, 3,
            (Application.internetReachability != NetworkReachability.NotReachable && PlayerPrefs.GetString("GameLatest_Update", string.Empty) != string.Empty) ? "CheckParameterData" : "CheckParemterData_Connect"
        );
    }
    #endregion

    #region MAIN 
    public void SkipUpdateContent()
    {
        StartCoroutine(GetGateWayScene());
    }

    public void SkipAndConnectOffline()
    {
        PlayerPrefs.DeleteKey("AccountSync");
    }

    public void GetUpdateContent()
    {
        string url = PlayerPrefs.GetString("GameUpdate_URL", string.Empty);
        if (url != string.Empty) Application.OpenURL(url);
    }

    public void RebootApplicationManual()
    {
        if (rebootingProgram == null) { rebootingProgram = StartCoroutine(Reboot_Application()); }
    }
    #endregion

    #region COMPONENT (Loader)
    private void GetLoaderDisplay(string message, int delay, string process)
    {
        GameLoader_Icon.GetComponent<Animator>().SetTrigger("Opening");
        GameLoader_Icon.transform.GetChild(1).GetComponent<Text>().text = message;
        Invoke(process, delay);
    }

    private void ReloadDisplay(string message)
    {
        GameLoader_Icon.transform.GetChild(1).GetComponent<Text>().text = message;
    }
    #endregion

    #region COMPONENT (Scene Transition)
    private IEnumerator GetGateWayScene()
    {
        yield return StartCoroutine(CheckingForExtensionContent());
        yield return StartCoroutine(CheckingForAreaLoaded());
        yield return StartCoroutine(CheckingForItemLoaded());
        yield return StartCoroutine(CheckingForStoryProgress());
        yield return StartCoroutine(CheckingForImportedCharacter());

        AsyncOperation loadScene = SceneManager.LoadSceneAsync("ServerGateway");
        while (!loadScene.isDone)
        {
            yield return null;
        }
    }

    private IEnumerator Reboot_Application()
    {
        if (GameObject.Find("BGM").activeInHierarchy) Destroy(GameObject.Find("BGM"));
        AsyncOperation operate = SceneManager.LoadSceneAsync("LoadScene");
        yield return new WaitWhile(() => !operate.isDone);
    }
    #endregion

    #region COMPONENT (Data Information)
    private string GetUpdateInfo()
    {
        return "New content required you to update your current version to " +
            PlayerPrefs.GetString("GameLatest_Update") +
            ". The current version are supposed to be replace to the updated version.";
    }

    private void LoadAllInGameAsset()
    {
        MeloMelo_GameSettings.GetScoreStructureSetup();
        MeloMelo_GameSettings.GetStatusRemarkStructureSetup();
        MeloMelo_GameSettings.GetZoneRewardSetup();
        MeloMelo_ExtensionContent_Settings.LoadStartingStats();

        // Load secondary currency
        MeloMelo_Economy.SetupSecondaryCurrency();
    }

    private IEnumerator CheckingForExtensionContent()
    {
        // Loading component: Text loader
        GetLoadingContent("Checking for content been loaded.\n Stay connected through the internet. \n This is an extension content in your marathon mode.");

        // Load: Marathon Content
        if (PlayerPrefs.GetString("storeCache_Connection", "off") == "OK!")
        {
            //MeloMelo_Network_RemoteConfig.ConfigurationBase setup_config = new MeloMelo_Network_RemoteConfig.ConfigurationSetup_MarathonContent();
            //StartCoroutine(setup_config.VerifyConfig());
            //yield return new WaitUntil(() => setup_config.GetConfigComplete());
        }

        string jsonMarathonContent = PlayerPrefs.GetString("JSON_Custom_Marathon_Challenge", string.Empty);

        Task runMarathonContent = Task.Run(() =>
        {
            if (jsonMarathonContent.Trim('{', '}') != string.Empty)
            {
                MeloMelo_ExtensionContent_Settings.marathonListing = new CustomMarathonInfo().GetArrays(jsonMarathonContent);
                MeloMelo_ExtensionContent_Settings.totalMarathonCount = MeloMelo_ExtensionContent_Settings.marathonListing.data.Length;
            }
            else
                Debug.Log("This is empty (1st run)");
        });

        yield return new WaitUntil(() => runMarathonContent.IsCompleted);
        Debug.Log("Total Marathon Content: " + MeloMelo_ExtensionContent_Settings.totalMarathonCount + " Loaded!");

        string jsonMarathonExchange = PlayerPrefs.GetString("JSON_Custom_Marathon_Exchange", string.Empty);
        Task runMarathonExchange = Task.Run(() =>
        {
            if (jsonMarathonExchange.Trim('{', '}') != string.Empty)
            {
                MarathonExchangeArray exchangeArray = new MarathonExchangeArray().GetExchangeList(jsonMarathonExchange);
                MeloMelo_Economy.exchangeContentOfMarathon = new List<MarathonExchangeContent>();
                MeloMelo_Economy.exchangeContentOfMarathon.AddRange(exchangeArray.marathonContent);
            }
            else
                Debug.Log("This is empty (2nd run)");
        });

        yield return new WaitUntil(() => runMarathonExchange.IsCompleted);
        Debug.Log("Total Exchange Content (Marathon) : " + (MeloMelo_Economy.exchangeContentOfMarathon != null ? MeloMelo_Economy.exchangeContentOfMarathon.ToArray().Length : 0) + " Loaded!");

        yield return new WaitForSeconds(1);
        GetLoadingCompleted();
    }

    private IEnumerator CheckingForItemLoaded()
    {
        // Loading component: Text loader
        GetLoadingContent("Item details will be needed to look up quickly without delay. \n Loading time might be longer depending on items is been added into the game.");

        int itemCount = 1;
        string[] filtered_item_checked = { "EXP_POTION", "POWER_POTION" };
        string[] filtered_possibleUsedItem = { "EXP_TICKET", "TRACK_TICKET" };
        MeloMelo_ItemStore_Management.preloaded_itemListing = new List<ItemData>();
        MeloMelo_ItemStore_Management.GetCharacterBoostItemSetup();
        MeloMelo_ItemStore_Management.GetPossibleItemSetup();

        while (MeloMelo_ItemStore_Management.preloaded_itemListing != null)
        {
            ResourceRequest itemRequest = Resources.LoadAsync<ItemData>("Database_Item/#" + itemCount);
            yield return new WaitUntil(() => itemRequest.isDone);

            ItemData itemRetrieved = itemRequest.asset as ItemData;
            if (itemRetrieved != null) { itemCount++; MeloMelo_ItemStore_Management.preloaded_itemListing.Add(itemRetrieved); }
            else break;
        }

        foreach (string possibleList in filtered_possibleUsedItem)
        {
            itemCount = 1;
            while (true)
            {
                ResourceRequest itemRequest = Resources.LoadAsync<UsageOfItemDetail>("Database_Item/Filtered_Items/" + possibleList + "/#" + itemCount);
                yield return new WaitUntil(() => itemRequest.isDone);

                UsageOfItemDetail itemRetrieved = itemRequest.asset as UsageOfItemDetail;
                if (itemRetrieved != null) { itemCount++; MeloMelo_ItemStore_Management.AddPossibleItemToList(itemRetrieved); }
                else break;
            }
        }

        foreach (string itemChecker in filtered_item_checked)
        {
            itemCount = 1;
            while (true)
            {
                ResourceRequest itemRequest = Resources.LoadAsync<UsageOfItemDetail>("Database_Item/Filtered_Items/" + itemChecker + "/#" + itemCount);
                yield return new WaitUntil(() => itemRequest.isDone);

                UsageOfItemDetail itemRetrieved = itemRequest.asset as UsageOfItemDetail;
                if (itemRetrieved != null) { itemCount++; MeloMelo_ItemStore_Management.AddCharacterBoostItemToList(itemRetrieved); }
                else break;
            }
        }

        GetLoadingCompleted();
        Debug.Log("Total Item Content : " + MeloMelo_ItemStore_Management.preloaded_itemListing.ToArray().Length + " Loaded!");
        Debug.Log("Total Possible Used Item : " + MeloMelo_ItemStore_Management.GetTotalPendingItemCount() + " Loaded!");
    }

    private IEnumerator CheckingForAreaLoaded()
    {
        // Loading component: Text loader
        GetLoadingContent("Loading track according to season and area. The world is still developing for expansion. \n This will take ages to load.");

        MeloMelo_AreaControl_Settings.OpenAreaControlToGame();
        int currentAreaCount = 0;
        AreaInfo loadedArea = null;

        for (int currentSeason = 0; currentSeason < seasonOutput + 1; currentSeason++)
        {
            do
            {
                currentAreaCount++;
                ResourceRequest areaToBeLoaded = Resources.LoadAsync<AreaInfo>("Database_Area/Season" + currentSeason + "/A" + currentAreaCount);
                yield return new WaitUntil(() => areaToBeLoaded.isDone);

                loadedArea = areaToBeLoaded.asset as AreaInfo;
                if (loadedArea != null) MeloMelo_AreaControl_Settings.LoadAreaToGame(loadedArea);
            }
            while (loadedArea != null);

            currentAreaCount = 0;
        }

        GetLoadingCompleted();
        Debug.Log("Total Area Setup : " + MeloMelo_AreaControl_Settings.GetAreaFromLoadGame().Length + " Loaded!");
    }

    private IEnumerator CheckingForStoryProgress()
    {
        // Loading component: Text loader
        GetLoadingContent("Getting story progress ready to restore.\n This will take a while.");

        MeloMelo_Adventure.allAdventureRouteData = new List<StoryProgressData>();
        string[] storyType = { "Event Story", "Main Story" };
        string[] storyDirectory = { "Event_Area_", "Main_Area_" };
        List<StoryInfo> mainStoryInfo = new List<StoryInfo>();
        List<StoryInfo> eventStoryInfo = new List<StoryInfo>();

        ResourceRequest requestForStoryInfo;
        int totalCountInfo = 1;

        for (int storyId = 0; storyId < storyType.Length; storyId++)
        {
            while (true)
            {
                requestForStoryInfo = Resources.LoadAsync<StoryInfo>("Database_Story/" + storyType[storyId] + "/" + storyDirectory[storyId] + totalCountInfo);
                yield return new WaitUntil(() => requestForStoryInfo.isDone);

                StoryInfo info = requestForStoryInfo.asset as StoryInfo;
                totalCountInfo++;

                if (info != null)
                {
                    switch (storyId)
                    {
                        case 1:
                            mainStoryInfo.Add(info);
                            break;

                        default:
                            eventStoryInfo.Add(info);
                            break;
                    }
                }
                else
                    break;
            }

            int totalCount = storyId == 0 ? eventStoryInfo.ToArray().Length : mainStoryInfo.ToArray().Length;
            Debug.Log("Load Completed: " + storyType[storyId] + " backup of " + totalCount + " have been found");
            totalCountInfo = 1;
        }
        
        // Load main story to game directory
        if (mainStoryInfo != null && mainStoryInfo.ToArray().Length > 0)
        {
            foreach (StoryInfo storyData in mainStoryInfo)
            {
                StoryProgressData progress = new StoryProgressData();
                progress.title = storyData.StoryTitle;
                progress.adventure_type = 1;
                progress.routeId_listing = new List<int>();

                for (int stage = 0; stage < storyData.Stage.Length; stage++)
                {
                    foreach (SlotQuestLog info in storyData.Stage[stage].myQuestLog)
                    {
                        progress.routeId_listing.Add(info.id);
                    }
                }

                MeloMelo_Adventure.allAdventureRouteData.Add(progress);
            }
        }

        // Load event story to game directory
        if (eventStoryInfo != null && eventStoryInfo.ToArray().Length > 0)
        {
            foreach (StoryInfo storyData in eventStoryInfo)
            {
                StoryProgressData progress = new StoryProgressData();
                progress.title = storyData.StoryTitle;
                progress.adventure_type = 0;
                progress.routeId_listing = new List<int>();

                for (int stage = 0; stage < storyData.Stage.Length; stage++)
                {
                    foreach (SlotQuestLog info in storyData.Stage[stage].myQuestLog)
                    {
                        progress.routeId_listing.Add(info.id);
                    }
                }

                MeloMelo_Adventure.allAdventureRouteData.Add(progress);
            }
        }

        yield return new WaitForSeconds(1);
        GetLoadingCompleted();
    }

    private IEnumerator CheckingForImportedCharacter()
    {
        // Loading component: Text loader
        GetLoadingContent("Loading of character details for quicker access.\n You can't miss out this part.");
        int totalCount = 0;

        string[] classRequireLoad =
        {
            "Warrior",
            "Warrior2",
            "Mage",
            "Mage2",
            "Marksman",
            "Marksman2"
        };

        foreach (string findClass in classRequireLoad)
        {
            ResourceRequest requestCharacterToLoad = Resources.LoadAsync<ClassBase>("Character_Data/" + findClass);
            yield return new WaitUntil(() => requestCharacterToLoad.isDone);

            ClassBase info = requestCharacterToLoad.asset as ClassBase;
            if (info != null)
            {
                totalCount++;
                MeloMelo_CharacterInfo_Settings.AddCharacterReferenceToGame(info);
            }
        }

        MeloMelo_CharacterInfo_Settings.CharacterProfileSetup();

        GetLoadingCompleted();
        Debug.Log("Total Character Loaded : " + totalCount + " Count Added!");
    }
    #endregion

    #region MISC 
    private void GetLoadingContent(string loading_text)
    {
        if (LoadingScreen == null)
        {
            GameObject preLoaded_loadingUI = Resources.Load<GameObject>("Prefabs/LoadingUI");
            GameObject loadingUI = Instantiate(preLoaded_loadingUI, transform);
            LoadingScreen = loadingUI;
        }

        LoadingScreen.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        LoadingScreen.GetComponent<LoadingContent_Script>().NowLoading(loading_text);
    }

    private void GetLoadingCompleted()
    {
        if (LoadingScreen != null) LoadingScreen.GetComponent<LoadingContent_Script>().DoneLoading();
    }
    #endregion
}
