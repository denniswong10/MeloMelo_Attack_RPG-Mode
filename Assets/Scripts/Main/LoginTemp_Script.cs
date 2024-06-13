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
        Invoke("LoadTempDetail", 0.5f);
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

    #region MAIN
    public void LoginPass()
    {
        LoginBtn.interactable = false;

        // Use cloud services through the available network
        cloudService = new CloudUsage_TempServices();

        // Generating playerId from the cloud database
        StartCoroutine(cloudService.GeneratingPlayerId(PlayerPrefs.GetString("GameWeb_URL")));
    }

    public void LoginPass_ProcessConfirmation()
    {
        // Update content display when login is processing
        playerid = playerid == string.Empty ? cloudService.get_playerId : playerid;
        LoadTempDetail();

        // Setup login authenticate through cloud
        login = new Authenticate_DataManagement(playerid, PlayerPrefs.GetString("GameWeb_URL"));

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
                StartCoroutine(login.GetAuthenticationFromServer(SecureKey.text));
                break;
        }
    }

    public void ProcessForCreating()
    {
        AlertBox[0].SetActive(false);

        StartCoroutine(cloudService.AccountBinding(PlayerPrefs.GetString("GameWeb_URL")));
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
        StartCoroutine(login.CreateNewEntryOnServer(SecureKey.text, entryField.transform.GetChild(1).GetComponent<InputField>().text));
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

    #region NETWORK
    public void SaveEntryPass(string message)
    {
        GetComponent<LoginPage_Script>().Icon.transform.GetChild(1).GetComponent<Text>().text = "[Game Network]\n" + message;
        GetComponent<LoginPage_Script>().UpdateUserProfileName(playerid);
        Invoke("GetMenuPlayThrough", 2);
    }

    public void LoadEntryPass(string message, string serverUser)
    {   
        GetComponent<LoginPage_Script>().Icon.transform.GetChild(1).GetComponent<Text>().text = "[Game Network]\n" + message;
        GetComponent<LoginPage_Script>().UpdateUserProfileName(serverUser);
        string url = PlayerPrefs.GetString("GameWeb_URL");

        if (login.get_success)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetString("TempPass_PlayerId", playerid);
            PlayerPrefs.SetString("GameWeb_URL", url);

            StartCoroutine(LoadCloudData());
            acquireEntryPass += GetMenuPlayThrough;
        }
        else
            Invoke("AwaitForReload", 2);
    }

    private IEnumerator LoadCloudData()
    {
        CloudLoad_DataManagement cloudData = new CloudLoad_DataManagement(
            LoginPage_Script.thisPage.GetUserPortOutput(), PlayerPrefs.GetString("GameWeb_URL"));

        for (int save = 0; save < 3; save++) 
            StartCoroutine(cloudData.LoadProgressTrack(save + 1));

        StartCoroutine(cloudData.LoadSettingCofiguration());
        StartCoroutine(cloudData.LoadProgressProfile());
        StartCoroutine(cloudData.LoadPlayerSettings());
        StartCoroutine(cloudData.LoadSelectionLastVisited());

        yield return new WaitUntil(() => cloudData.cloudLogging.ToArray().Length == cloudData.get_counter);
        acquireEntryPass();
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
}
