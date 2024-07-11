using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MeloMelo_Local;
using System.Threading.Tasks;
using System;

public class LoginPage_Script : MonoBehaviour
{
    public static LoginPage_Script thisPage;

    private string User = "GUEST";
    public string get_user { get { return User; } }

    public Text CurrentGameVersion;
    public GameObject Icon;
    private GameObject[] BGM;

    private Coroutine rebootApplication;
    public int portNumber = 0;

    // Program: Login Scene
    void Start()
    {
        thisPage = this;
        rebootApplication = null;

        // System Component: Intit
        BGM_Loader();

        // Interface: Intit
        CurrentGameVersion.text = "Installed Version: " + CheckingForVersionIndex();
    }

    void Update()
    {
        // Return To Start Menu: Function
        if (Input.GetKeyDown(KeyCode.Escape) && rebootApplication == null) { rebootApplication = StartCoroutine(GetRebootProcessing()); }
    }

    #region SETUP
    string CheckingForVersionIndex()
    {
        try { return StartMenu_Script.thisMenu.get_version; } catch { return "---"; }
    }

    IEnumerator GetRebootProcessing()
    {
        Destroy(BGM[0]);
        AsyncOperation process = SceneManager.LoadSceneAsync("LoadScene");
        yield return new WaitWhile(() => !process.isDone);
    }
    #endregion

    #region MAIN
    public void GuestLogin(Button button)
    {
        button.interactable = false;

        string playerid = PlayerPrefs.HasKey("TempPass_PlayerId") ? PlayerPrefs.GetString("TempPass_PlayerId") : string.Empty;
        string user = PlayerPrefs.GetString("AccountSync_PlayerID");
        string pass = PlayerPrefs.GetString("AccountSync_UniqueID");

        PlayerPrefs.DeleteAll();

        if (playerid != string.Empty) PlayerPrefs.SetString("TempPass_PlayerId", playerid);
        PlayerPrefs.SetInt("AccountSync", 1);
        PlayerPrefs.SetString("AccountSync_PlayerID", user);
        PlayerPrefs.SetString("AccountSync_UniqueID", pass);

        LoadAllProgress(button);
    }

    private void LoadAllProgress(Button button)
    {
        LocalLoad_DataManagement data = new LocalLoad_DataManagement(GetUserPortOutput(), "StreamingAssets/LocalData/MeloMelo_LocalSave_InGameProgress");
        string[] paths =
        {
            MeloMelo_GameSettings.GetLocalFileMainProgress,
            MeloMelo_GameSettings.GetLocalFilePointData,
            MeloMelo_GameSettings.GetLocalFileBattleProgress,
            MeloMelo_GameSettings.GetLocalFileAccountSettings,
            MeloMelo_GameSettings.GetLocalFileGameplaySettings,
            MeloMelo_GameSettings.GetLocalFileCharacterSettings,
            MeloMelo_GameSettings.GetLocalFileProfileData,
            MeloMelo_GameSettings.GetLocalFileCharacterStats
        };

        Icon.SetActive(true);
        Icon.transform.GetChild(1).GetComponent<Text>().text = "[Game Local]\nChecking Data...";

        for (int path = 0; path < paths.Length; path++)
        {
            data.SelectFileForActionWithUserTag(paths[path]);
            Icon.transform.GetChild(1).GetComponent<Text>().text = "[Game Local]\nData Transfer " + (path + 1) + "/"+ paths.Length + "...";

            switch (path)
            {
                case 1:
                    data.LoadPointProgress();
                    break;

                case 2:
                    data.LoadBattleProgress();
                    break;

                case 3:
                    data.LoadAccountSettings();
                    break;

                case 4:
                    data.LoadGameplaySettings();
                    break;

                case 5:
                    data.LoadCharacterSettings();
                    break;

                case 6:
                    data.LoadProfileState();
                    break;

                case 7:
                    data.LoadCharacterStatsProgress();
                    break;

                default:
                    data.LoadProgress();
                    break;
            }
        }

        // Finialize setup config
        button.interactable = true;
        MeloMelo_GameSettings.UpdateCharacterProfile();
        SceneManager.LoadScene("Menu");
    }

    public void LinkSite() { Application.OpenURL(PlayerPrefs.GetString("GameWeb_URL", string.Empty) + "/database/transcripts/site7"); }

    public void PreRegister_Direct()
    {
        Application.OpenURL(PlayerPrefs.GetString("GameWeb_URL", string.Empty) + "/form_content.php");
    }

    public void BackButton()
    {
        SceneManager.LoadScene("ServerGateway");
    }
    #endregion

    #region COMPONENT
    // Component System: Start-up
    private void BGM_Loader()
    {
        BGM = GameObject.FindGameObjectsWithTag("BGM");
        if (BGM.Length > 1) { for (int i = 1; i < BGM.Length; i++) { Destroy(BGM[i]); } }
    }
    #endregion

    #region MISC
    public void UpdateUserProfileName(string name) { User = name; }

    public string GetUserPortOutput()
    {
        switch (portNumber)
        {
            case 1:
                return GuestLogin_Script.thisScript.get_entry;

            default:
                return User;
        }
    }
    #endregion
}
