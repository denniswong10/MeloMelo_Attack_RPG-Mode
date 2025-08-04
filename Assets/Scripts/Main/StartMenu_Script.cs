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
        ConnectionAlert.SetActive(true);
    }

    private void LoadGameApplication()
    {
        string loadingText = "[Game Loading]\nInitialize...";
        MeloMelo_ExtensionContent_Settings.UpdateCharacterProfile();

        GetLoaderDisplay(
            loadingText, 3,
            Application.internetReachability != NetworkReachability.NotReachable ? "CheckParameterData" : "CheckParemterData_Connect"
        );
    }
    #endregion

    #region MAIN 
    public void SkipUpdateContent()
    {
        StartCoroutine(GetGateWayScene());
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
        yield return new WaitForSeconds(1);

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
        MeloMelo_ExtensionContent_Settings.LoadStartingStats();
    }

    private IEnumerator CheckingForExtensionContent()
    {
        GameObject preLoaded_loadingUI = Resources.Load<GameObject>("Prefabs/LoadingUI");
        GameObject loadingUI = Instantiate(preLoaded_loadingUI, transform);
        loadingUI.GetComponent<LoadingContent_Script>().NowLoading("Checking for content been loaded.\n Stay connected through the internet.This \n will take a while.");

        // Load: Marathon Content
        string jsonMarathonContent = PlayerPrefs.GetString("JSON_Custom_Marathon_Challenge", string.Empty);

        Task runMarathonContent = Task.Run(() =>
        {
            if (jsonMarathonContent != string.Empty)
            {
                MeloMelo_ExtensionContent_Settings.marathonListing = new CustomMarathonInfo().GetArrays(jsonMarathonContent);
                MeloMelo_ExtensionContent_Settings.totalMarathonCount = MeloMelo_ExtensionContent_Settings.marathonListing.data.Length;
            }
            else
                Debug.Log("This is empty (1st run)");
        });

        yield return new WaitUntil(() => runMarathonContent.IsCompleted);
        Debug.Log("Total Marathon Content: " + MeloMelo_ExtensionContent_Settings.totalMarathonCount + " Loaded!");

        // Load: Marathon Exchange
        string jsonMarathonExchange = PlayerPrefs.GetString("JSON_Custom_Marathon_Exchange", string.Empty);

        Task runMarathonExchange = Task.Run(() =>
        {
            if (jsonMarathonExchange != string.Empty)
            {
                MarathonExchangeArray exchangeArray = new MarathonExchangeArray().GetExchangeList(jsonMarathonExchange);
                MeloMelo_Economy.exchangeContentOfMarathon = new List<MarathonExchangeContent>();
                MeloMelo_Economy.exchangeContentOfMarathon.AddRange(exchangeArray.marathonContent);
            }
            else
                Debug.Log("This is empty (2nd run)");
        });

        yield return new WaitUntil(() => runMarathonExchange.IsCompleted);
        Debug.Log("Total Exchange Content (Marathon) : " + MeloMelo_Economy.exchangeContentOfMarathon.ToArray().Length + " Loaded!");

        yield return new WaitForSeconds(1);
        loadingUI.GetComponent<LoadingContent_Script>().DoneLoading();
    }
    #endregion
}
