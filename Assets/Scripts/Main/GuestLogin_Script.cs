using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MeloMelo_Local;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Threading;

public class GuestLogin_Script : MonoBehaviour
{
    public static GuestLogin_Script thisScript;

    private string guestLoginEntry = "GUEST";
    public string get_entry { get { return guestLoginEntry; } }

    private string[] entrytitle = new string[5];
    public string[] get_entrytitle { get { return entrytitle; } }

    public GameObject ProcessBtn;
    public GameObject[] entryButton;

    public GameObject entryFormDisplay;
    public GameObject profileList;
    [SerializeField] private GameObject LoadingUI;

    private Authenticate_LocalData localPlayer = null;
    public Authenticate_LocalData get_localPlayer { get { return localPlayer; } }

    void Start()
    {
        thisScript = this;
        LoadExistingPlayerProfile();
    }

    void Update()
    {
        entryButton[1].GetComponent<Button>().interactable = PlayerPrefs.GetString("SelectedGuestEntry", string.Empty) != string.Empty;
        ProcessBtn.GetComponent<Button>().interactable = PlayerPrefs.GetString("SelectedGuestEntry", string.Empty) != string.Empty;
    }

    #region SETUP
    private void RefreshEntryList()
    {
        ClearAllEntry();
        SpawnEntryEntity();
    }

    private void ClearAllEntry()
    {
        for (int entry = 0; entry < profileList.transform.childCount; entry++)
            Destroy(profileList.transform.GetChild(entry).gameObject);
    }

    private void SpawnEntryEntity()
    {
        foreach (string entry in entrytitle)
            if (entry != string.Empty)
                Instantiate(Resources.Load<RawImage>("ProfilePlate/NamePlate"), profileList.transform).transform.GetChild(0).GetComponent<Text>().text = entry;
    }
    
    private bool AddNewEntry(string name)
    {
        for (int i = 0; i < entrytitle.Length; i++)
        {
            if (entrytitle[i] == string.Empty)
            {
                entrytitle[i] = name;
                return true;
            }
        }

        return false;
    }
    #endregion

    #region MAIN
    public void ShowEntryDisplay()
    {
        entryFormDisplay.SetActive(true);
    }

    public void ConfirmNewEntry(InputField output)
    {
        entryFormDisplay.SetActive(false);

        if (AddNewEntry(output.text))
            RefreshEntryList();

        SaveProfileLocal();
    }

    public void RemoveEntry()
    {
        string currentEntry = PlayerPrefs.GetString("SelectedGuestEntry", string.Empty);

        for (int i = 0; i < entrytitle.Length; i++)
        {
            if (entrytitle[i] == currentEntry)
            {
                entrytitle[i] = string.Empty;
                PlayerPrefs.DeleteKey(entrytitle[i] + "PlayedCount_Data");
                PlayerPrefs.DeleteKey(entrytitle[i] + "totalRatePoint");
            }
        }

        RefreshEntryList();
        RemoveProfileLocal();

        PlayerPrefs.SetString("SelectedGuestEntry", string.Empty);
    }

    public void UpdateGuestEntryName(string name)
    {
        guestLoginEntry = name;
        LoginPage_Script.thisPage.portNumber = 0;

        if (localPlayer == null || (localPlayer != null && localPlayer.GetUserLocalByPlayerId() != name))
        {
            localPlayer = new Authenticate_LocalData(guestLoginEntry, "StreamingAssets/LocalData/MeloMelo_LocalPlayer_AuthenticateList");
            localPlayer.SelectFileForAction("localplayer_guest_list.txt");

            // Cache
            if (!MeloMelo_PlayerSettings.GetLocalUserAccount())
            {
                PlayerPrefs.SetString("AccountSync_PlayerID", localPlayer.GetUserLocalByPlayerId());
                PlayerPrefs.SetString("AccountSync_UniqueID", localPlayer.GetUserLocalByUniqueId());
            }
        }
    }
    #endregion

    #region MAIN (Login)
    public void GuestLogin(Button button)
    {
        button.interactable = false;

        string playerid = PlayerPrefs.HasKey("TempPass_PlayerId") ? PlayerPrefs.GetString("TempPass_PlayerId") : string.Empty;
        string user = PlayerPrefs.GetString("AccountSync_PlayerID");
        string pass = PlayerPrefs.GetString("AccountSync_UniqueID");

        // Save
        string reportUpdate = PlayerPrefs.GetString("MeloMelo_NewsReport_Daily", string.Empty);
        string latestUpdate = PlayerPrefs.GetString("GameLatest_Update", string.Empty);
        string customMarathonPlay = PlayerPrefs.GetString("JSON_Custom_Marathon_Challenge", string.Empty);

        PlayerPrefs.DeleteAll();

        if (playerid != string.Empty) PlayerPrefs.SetString("TempPass_PlayerId", playerid);
        MeloMelo_PlayerSettings.GetLocalUserAccount(1);
        PlayerPrefs.SetString("AccountSync_PlayerID", user);
        PlayerPrefs.SetString("AccountSync_UniqueID", pass);

        // Transfer
        PlayerPrefs.SetString("MeloMelo_NewsReport_Daily", reportUpdate);
        PlayerPrefs.SetString("GameLatest_Update", latestUpdate);
        PlayerPrefs.SetString("JSON_Custom_Marathon_Challenge", customMarathonPlay);

        StartCoroutine(CheckingForProgress(button));
    }
    #endregion

    #region COMPONENT (Login)
    private IEnumerator CheckingForProgress(Button button)
    {
        yield return StartCoroutine(LoadAllProgress());

        // Finialize setup config
        button.interactable = true;

        AsyncOperation loadScene = SceneManager.LoadSceneAsync("Menu");
        while (!loadScene.isDone)
        {
            LoginPage_Script.thisPage.Icon.transform.GetChild(1).GetComponent<Text>().text = "[Game Local]\nGame Loading...";
            yield return null;
        }
    }

    private IEnumerator LoadAllProgress()
    {
        LoadingUI.SetActive(true);

        string[] paths =
        {
            MeloMelo_GameSettings.GetLocalFileMainProgress,
            MeloMelo_GameSettings.GetLocalFilePointData,
            MeloMelo_GameSettings.GetLocalFileBattleProgress,
            MeloMelo_GameSettings.GetLocalFileAccountSettings,
            MeloMelo_GameSettings.GetLocalFileGameplaySettings,
            MeloMelo_GameSettings.GetLocalFileCharacterSettings,
            MeloMelo_GameSettings.GetLocalFileProfileData,
            MeloMelo_GameSettings.GetLocalFileCharacterStats,
            MeloMelo_GameSettings.GetLocalFileSkillDatabase,
            MeloMelo_GameSettings.GetLocalFileVirtualItemData,
            MeloMelo_GameSettings.GetLocalFileAdventureMode
        };

        LoginPage_Script.thisPage.Icon.SetActive(true);
        LoginPage_Script.thisPage.Icon.transform.GetChild(1).GetComponent<Text>().text = "[Game Local]\nChecking Data...";
        int currentProgressId = 0;

        while (currentProgressId < paths.Length)
        {
            yield return StartCoroutine(CheckingProgressLoaded(paths[currentProgressId], currentProgressId));

            LoginPage_Script.thisPage.Icon.transform.GetChild(1).GetComponent<Text>().text =
                "[Game Local]\nData Transfer " + currentProgressId + " / " + paths.Length + "...";

            currentProgressId++;
        }

        yield return new WaitForSeconds(0.5f);
        LoadingUI.SetActive(false);
    }

    private IEnumerator CheckingProgressLoaded(string path, int id)
    {
        LocalLoad_DataManagement data = new LocalLoad_DataManagement(LoginPage_Script.thisPage.GetUserPortOutput(), "StreamingAssets/LocalData/MeloMelo_LocalSave_InGameProgress");
        data.SelectFileForActionWithUserTag(path);

        switch (id)
        {
            case 0:
                Task<List<ScoreDatabase>> isProgressScore = data.PreLoading_ScoreData();
                yield return new WaitUntil(() => isProgressScore.IsCompleted);
                yield return StartCoroutine(data.PostLoading_ScoreData(isProgressScore.Result.ToArray()));
                break;

            case 1:
                Task<List<PointDatabase>> isProgressPoint = data.PreLoading_PointData();
                yield return new WaitUntil(() => isProgressPoint.IsCompleted);
                yield return StartCoroutine(data.PostLoading_PointData(isProgressPoint.Result.ToArray()));
                break;

            case 2:
                Task<List<BattleProgressDatabase>> isProgressBattleData = data.PreLoading_BattleProgressData();
                yield return new WaitUntil(() => isProgressBattleData.IsCompleted);
                yield return StartCoroutine(data.PostLoading_BattleProgressData(isProgressBattleData.Result.ToArray()));
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
                data.LoadProfileData();
                break;

            case 7:
                Task<List<BattleUnitDatabase>> isCharacterStatsReady = data.PreLoading_CharacterStatsData();
                yield return new WaitUntil(() => isCharacterStatsReady.IsCompleted);
                yield return StartCoroutine(data.PostLoading_CharacterStatsData(isCharacterStatsReady.Result.ToArray()));
                break;

            case 8:
                Task<List<SkillUnitDatabase>> isSkillsLoaded = data.PreLoading_SkillsData();
                yield return new WaitUntil(() => isSkillsLoaded.IsCompleted);
                yield return StartCoroutine(data.PostLoading_SkillsData(isSkillsLoaded.Result.ToArray()));
                break;

            case 9:
                Task<bool> isVirtualItemLoaded = data.PreLoading_VirtualItemData();
                yield return new WaitUntil(() => isVirtualItemLoaded.IsCompleted);
                Debug.Log("Total Item Stored: " + MeloMelo_ItemUsage_Settings.GetActiveItems().Length);
                break;

            case 10:
                Task<List<AdventureStoreData>> isAdventureDataLoaded = data.Preloading_AdventureModeData();
                yield return new WaitUntil(() => isAdventureDataLoaded.IsCompleted);
                yield return StartCoroutine(data.PostLoading_AdventureModeData(isAdventureDataLoaded.Result.ToArray()));
                break;

            default:
                break;
        }
    }
    #endregion

    #region SETUP (NEW FEATURES!)
    [SerializeField] private GameObject[] PlayerMenu = new GameObject[2];
    [SerializeField] private Text Player_Name;

    private void LoadExistingPlayerProfile()
    {
        PlayerMenu[MeloMelo_PlayerSettings.GetLocalUserAccount() ? 1 : 0].SetActive(true);

        if (MeloMelo_PlayerSettings.GetLocalUserAccount())
        {
            Player_Name.text = PlayerPrefs.GetString("AccountSync_PlayerID");
            UpdateGuestEntryName(PlayerPrefs.GetString("AccountSync_PlayerID"));
        }
        else
        {
            PlayerPrefs.DeleteKey("SelectedGuestEntry");
            for (int entry = 0; entry < entrytitle.Length; entry++) entrytitle[entry] = string.Empty;
            LoadProfileLocal();
        }
    }
    #endregion

    #region MAIN (NEW FEATURES!)
    public void RejectUsingExistingPlayer()
    {
        MeloMelo_PlayerSettings.GetLocalUserAccount(2);
        SceneManager.LoadScene("LoginPage3");
    }
    #endregion

    #region MISC
    private void SaveProfileLocal()
    {
        Authenticate_LocalData savePlayerId = new Authenticate_LocalData(entryFormDisplay.transform.GetChild(1).GetComponent<InputField>().text, 
            "StreamingAssets/LocalData/MeloMelo_LocalPlayer_AuthenticateList");

        savePlayerId.SelectFileForAction("localplayer_guest_list.txt");
        savePlayerId.MakeNewUserOnLocal(entryFormDisplay.transform.GetChild(3).GetComponent<InputField>().text);
    }

    private void LoadProfileLocal()
    {
        Authenticate_LocalData playerList = new Authenticate_LocalData("GUEST", 
            "StreamingAssets/LocalData/MeloMelo_LocalPlayer_AuthenticateList");

        playerList.SelectFileForAction("localplayer_guest_list.txt");

        if (playerList.GetUserAuthenticationList() != null)
            for (int player = 0; player < playerList.GetUserAuthenticationList().Length; player++)
                if (player < entrytitle.Length) entrytitle[player] = playerList.GetUserAuthenticationList()[player];

        RefreshEntryList();
    }

    private void RemoveProfileLocal()
    {
        Authenticate_LocalData removePlayer = new Authenticate_LocalData(PlayerPrefs.GetString("SelectedGuestEntry", string.Empty), 
            "StreamingAssets/LocalData/MeloMelo_LocalPlayer_AuthenticateList");

        removePlayer.SelectFileForAction("localplayer_guest_list.txt");
        removePlayer.DestroyNewUserOnLocal();
    }
    #endregion
}
