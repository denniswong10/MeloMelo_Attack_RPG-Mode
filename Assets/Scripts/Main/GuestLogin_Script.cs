using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MeloMelo_Local;

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
            if (!PlayerPrefs.HasKey("AccountSync"))
            {
                PlayerPrefs.SetString("AccountSync_PlayerID", localPlayer.GetUserLocalByPlayerId());
                PlayerPrefs.SetString("AccountSync_UniqueID", localPlayer.GetUserLocalByUniqueId());
            }
        }
    }
    #endregion

    #region SETUP (NEW FEATURES!)
    [SerializeField] private GameObject[] PlayerMenu = new GameObject[2];
    [SerializeField] private Text Player_Name;

    private void LoadExistingPlayerProfile()
    {
        PlayerMenu[PlayerPrefs.HasKey("AccountSync") ? 1 : 0].SetActive(true);

        if (PlayerPrefs.HasKey("AccountSync"))
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
        PlayerPrefs.DeleteKey("AccountSync");
        UnityEngine.SceneManagement.SceneManager.LoadScene("LoginPage3");
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
