using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MeloMelo_Network;

public class LoginTemp_Script : MonoBehaviour
{
    public static LoginTemp_Script thisTemp;

    [SerializeField] private InputField SecureKey;
    [SerializeField] private Text PlayerID;
    [SerializeField] private Button LoginBtn;
    [SerializeField] private GameObject[] AlertBox;
    [SerializeField] private GameObject entryField;

    private string playerid = string.Empty;

    private Authenticate_DataManagement login = null;
    public Authenticate_DataManagement get_login { get { return login; } }

    private CloudUsage_TempServices cloudService = null;
    private enum AlertBoxPrompt { SignInAsNew, InvaildKey, NetworkFailire, MaxCapacity }

    private delegate void AcquireEntryTempPass();
    private AcquireEntryTempPass acquireEntryPass;

    // Start is called before the first frame update
    void Start()
    {
        thisTemp = this;

        playerid = PlayerPrefs.GetString("TempPass_PlayerId", string.Empty);
        UpdateContentLoginButton(playerid != string.Empty);

        // Get server data services (Player ID)
        GetServer_CheckID();
    }

    #region SETUP
    private void LoadTempDetail()
    {
        // Display playerId if existing account is been created
        PlayerID.text = "Player ID: " + (playerid == string.Empty ? "--" : playerid);
    }

    private void UpdateContentLoginButton(bool isSigned)
    {
        // Update button content on create if player account not found. Otherwise button to login
        LoginBtn.gameObject.transform.GetChild(0).GetComponent<Text>().text = isSigned ? "LOGIN" : "CREATE";
    }
    #endregion

    #region MAIN (Network Handler)
    public void LoginPass()
    {
        LoginBtn.interactable = false;
        LoginPage_Script.thisPage.portNumber = 1;

        // Use cloud services through the available network
        cloudService = new CloudUsage_TempServices(MeloMelo_PlayerSettings.GetWebServerUrl());

        // Generating playerId from the cloud database
        StartCoroutine(cloudService.GeneratingPlayerId());
    }

    public void LoginPass_ProcessConfirmation()
    {
        // Update content display when login is processing
        playerid = playerid == string.Empty ? cloudService.get_playerId : playerid;
        LoadTempDetail();

        // Setup login authenticate through cloud
        //login = new Authenticate_DataManagement(playerid, MeloMelo_PlayerSettings.GetWebServerUrl());

        // Create or transfer
        switch (LoginBtn.gameObject.transform.GetChild(0).GetComponent<Text>().text)
        {
            case "CREATE":
                PlayerPrefs.SetString("TempPass_PlayerId", playerid);
                LoadingTempPage("Creating Entry...");

                AlertBoxPrompt alertValue = playerid != cloudService.playerId_noEntry ? AlertBoxPrompt.SignInAsNew : AlertBoxPrompt.MaxCapacity;
                AlertBox[(int)alertValue].SetActive(true);
                if (playerid == cloudService.playerId_noEntry) PlayerPrefs.DeleteKey("TempPass_PlayerId");
                break;

            default:
                LoadingTempPage("Data Transfer...");
               // StartCoroutine(login.GetAuthenticationFromServer(SecureKey.text));
                break;
        }
    }
    #endregion

    #region MAIN (Menu Handler)
    public void ProcessForCreating()
    {
        AlertBox[0].SetActive(false);

        StartCoroutine(cloudService.AccountBinding(false));
        CreatePlayerNameEntry(playerid);
    }

    public void CancelCreateProcess()
    {
        AlertBox[0].SetActive(false);
        LoadingTempPage("Created Unsucessful!");
        PlayerPrefs.DeleteKey("TempPass_PlayerId");

        Invoke("AwaitForReload", 2);
    }

    public void CheckSecureKeyValid()
    {
        LoginBtn.interactable = SecureKey.text != string.Empty;
    }

    public void ConfirmPlayerNameEntry()
    {
        //StartCoroutine(login.CreateNewEntryOnServer(SecureKey.text, entryField.transform.GetChild(1).GetComponent<InputField>().text));
    }
    #endregion

    #region COMPONENT
    private void LoadingTempPage(string text)
    {
        GetComponent<LoginPage_Script>().Icon.SetActive(true);
        GetComponent<LoginPage_Script>().Icon.transform.GetChild(1).GetComponent<Text>().text = "[Game Network]\n" + text;
    }

    private void CreatePlayerNameEntry(string id)
    {
        entryField.SetActive(true);
        entryField.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "P_" + id;
    }
    #endregion

    #region COMPONENT (NETWORK HANDLER)
    private IEnumerator LoadCloudData()
    {
        CloudLoad_DataManagement cloudData = new CloudLoad_DataManagement(
            LoginPage_Script.thisPage.GetUserPortOutput(), MeloMelo_PlayerSettings.GetWebServerUrl());

        for (int save = 0; save < 3; save++)
            StartCoroutine(cloudData.LoadProgressTrack(save + 1));

        StartCoroutine(cloudData.LoadSettingCofiguration());
        StartCoroutine(cloudData.LoadProgressProfile());
        StartCoroutine(cloudData.LoadPlayerSettings());
        StartCoroutine(cloudData.LoadSelectionLastVisited());
        StartCoroutine(cloudData.LoadBattleFormationData());
        StartCoroutine(cloudData.LoadCharacterStatusData());
        StartCoroutine(cloudData.LoadTrackDistributionChart());

        yield return new WaitUntil(() => cloudData.cloudLogging.ToArray().Length == cloudData.get_counter);
        acquireEntryPass();
    }
    #endregion

    #region NETWORK (Redirect Component)
    public void SaveEntryPass(string message)
    {
        GetComponent<LoginPage_Script>().Icon.transform.GetChild(1).GetComponent<Text>().text = "[Game Network]\n" + message;

        if (message == "Completed!")
        {
            GetComponent<LoginPage_Script>().UpdateUserProfileName(entryField.transform.GetChild(1).GetComponent<InputField>().text);
            Invoke("GetMenuPlayThrough", 2);
        }
        else
        {
            StartCoroutine(cloudService.AccountBinding(true));
            PlayerPrefs.DeleteKey("TempPass_PlayerId");
            Invoke("AwaitForReload", 2);
        }
    }

    public void LoadEntryPass(string message, string serverUser)
    {   
        GetComponent<LoginPage_Script>().Icon.transform.GetChild(1).GetComponent<Text>().text = "[Game Network]\n" + message;
        GetComponent<LoginPage_Script>().UpdateUserProfileName(serverUser);
        string url = MeloMelo_PlayerSettings.GetWebServerUrl();

        if (login.get_success)
        {
            Debug.Log("[TempPass] Login as: " + LoginPage_Script.thisPage.get_user);
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetString("TempPass_PlayerId", playerid);
            MeloMelo_PlayerSettings.UpdateWebServerUrl(url);

            StartCoroutine(LoadCloudData());
            acquireEntryPass += GetMenuPlayThrough;
        }
        else
            Invoke("AwaitForReload", 2);
    }
    #endregion

    #region MISC
    private void GetMenuPlayThrough()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    private void AwaitForReload()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LoginPage2");
    }
    #endregion

    #region MISC (Auto Detection ID)
    private void GetServer_CheckID()
    {
        CloudServices_ControlPanel services = new CloudServices_ControlPanel(MeloMelo_PlayerSettings.GetWebServerUrl());
        StartCoroutine(services.CheckNetwork_IDInspection(PlayerPrefs.GetString("TempPass_PlayerId", string.Empty)));
    }

    public void GetServer_CheckedID(string id)
    {
        if (id != PlayerPrefs.GetString("TempPass_PlayerId", string.Empty))
        {
            PlayerPrefs.DeleteKey("TempPass_PlayerId");
            UpdateContentLoginButton(false);
        }

        Invoke("LoadTempDetail", 0.5f);
    }
    #endregion
}
