﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    private string version = string.Empty;
    public string get_version { get { return version; } }

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

    // Program: Start Scene
    void Start()
    {
        thisMenu = this;
        rebootingProgram = null;

        serverURL = PlayerPrefs.GetString("GameWeb_URL", string.Empty);
        PlayerPrefs.DeleteKey("GameLatest_Update");
        Invoke("BootGameTitleScreen", 3);
    }

    // Transition --> From StartMenu_Transition
    void Update()
    {
        if (ReadyToLaunch())
        {
            if (Input.GetKeyDown(KeyCode.Escape) && rebootingProgram == null) { rebootingProgram = StartCoroutine(Reboot_Application()); }
            else if (Input.anyKeyDown) { LoadGameApplication(); }
        }
    }

    #region SETUP
    private bool ReadyToLaunch()
    {
        return startEnable.color.a == 1;
    }

    private void BootGameTitleScreen()
    {
        GameTitle_Background.SetTrigger("Open");
        version = Application.version + "." + seasonOutput + "." + versionIndex + buildLabel;
    }

    private void CheckParameterData()
    {
        string description =
            "New content required you to update your current version to " +
            PlayerPrefs.GetString("GameLatest_Update") +
            ". The current version are supposed to be replace to the updated version.";

        MeloMelo_GameSettings.GetScoreStructureSetup();
        MeloMelo_GameSettings.GetStatusRemarkStructureSetup();

        if (PlayerPrefs.GetString("GameLatest_Update", string.Empty) != version)
        {
            UpdateAlert.SetActive(true);
            UpdateAlert.transform.GetChild(3).GetComponent<Text>().text = description;
        }
        else
        {
            ReloadDisplay("[Game Loading]\n Completed!");
            Invoke("GetServerGateway_Scene", 3);
        }
    }

    private void CheckParemterData_Connect()
    {
        ConnectionAlert.SetActive(true);
    }

    private void LoadGameApplication()
    {
        string loadingText = "[Game Loading]\nInitialize...";

        GetLoaderDisplay(
            loadingText, 3,
            Application.internetReachability != NetworkReachability.NotReachable ? "CheckParameterData" : "CheckParemterData_Connect"
        );
    }
    #endregion

    #region MAIN 
    public void SkipUpdateContent()
    {
        GetServerGateway_Scene();
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
    private void GetServerGateway_Scene()
    {
        SceneManager.LoadScene("ServerGateway");
    }

    private IEnumerator Reboot_Application()
    {
        if (GameObject.Find("BGM").activeInHierarchy) Destroy(GameObject.Find("BGM"));
        AsyncOperation operate = SceneManager.LoadSceneAsync("LoadScene");
        yield return new WaitWhile(() => !operate.isDone);
    }
    #endregion
}