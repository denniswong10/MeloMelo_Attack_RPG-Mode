using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MeloMelo_RatingMeter;

public class PlayerScore
{
    public string entryName;
    public int ratePoint;
    public int playCount;

    public PlayerScore(string e, int r, int p)
    {
        entryName = e;
        ratePoint = r;
        playCount = p;
    }
}

public class Menu : MonoBehaviour
{
    public static Menu thisMenu;
    private GameObject[] BGM;

    private RatePointIndicator Profile = new RatePointIndicator();
    public Text[] menuContentTxt;

    [Header("Ranking Board Component")]
    public GameObject RankBoard;
    public RawImage Display_OfflineMode;
    public GameObject Display_LoadingMode;

    private int connectingAttempt = 0;
    public Text RankingTitle;

    private string userInput;

    void Start()
    {
        thisMenu = this;
        userInput = LoginPage_Script.thisPage.GetUserPortOutput();

        CallUpOptionTask();
    }

    #region MAIN 
    private void CallUpOptionTask()
    {
        Profile.ProfileUpdate(userInput, true, "Played Count: ", "Rate Point: ");
        VersionUpdate();
        CheckingForServerRank();

        BGM = GameObject.FindGameObjectsWithTag("BGM");
        if (BGM.Length > 1) { for (int i = 1; i < BGM.Length; i++) { Destroy(BGM[i]); } }

        GameObject.Find("CreidtCounter").transform.GetChild(0).GetComponent<Text>().text = 
            "Credit: " + PlayerPrefs.GetInt(LoginPage_Script.thisPage.GetUserPortOutput() + "_Credit", 0);
    }

    public void Menu_InteractANDTransition(string scene)
    {
        // Main - Start (Ref_PreSelection<1><2>), Options (Options)

        if (scene != "Quit") SceneManager.LoadScene(scene);
        else Application.Quit();
    }
    #endregion

    #region COMPONENT
    bool CheckingNetworkReachable()
    {
        if (PlayerPrefs.GetInt("serverEnable", 0) == 1) return true;
        else return false;
    }

    void CheckingForServerRank()
    {
        // Show Display
        Display_LoadingMode.SetActive(true);
        RankingTitle.text = "GLOBAL RANKING";

        // Load Display of ranking
        if (CheckingNetworkReachable())
        {
            MeloMelo_Network.Data_Management input = new MeloMelo_Network.Data_Management(PlayerPrefs.GetString("GameWeb_URL"));
            StartCoroutine(input.MemberRankingBoard());
            Display_LoadingMode.SetActive(false);
        }
        else if (connectingAttempt <= 3)
        {
            Display_LoadingMode.transform.GetChild(0).GetComponent<Text>().text += ".";
            Invoke("CheckingForServerRank", 2);
            connectingAttempt++;
        }
        else if (connectingAttempt > 3)
        {
            // Show Display
            Display_LoadingMode.SetActive(false); 
            RankingTitle.text = "LOCAL RANKING";
            LocalRankingBoard();
        }
    }

    void VersionUpdate()
    {
        string latestBuild = PlayerPrefs.GetString("GameLatest_Update", string.Empty);

        menuContentTxt[0].text = "CURRENT VERSION: " + StartMenu_Script.thisMenu.get_version;
        if (latestBuild != string.Empty) { menuContentTxt[2].text = "LATEST VERSION: " + latestBuild; }
        else { menuContentTxt[2].text = string.Empty; }
    }
    #endregion

    #region MISC (GLOBAL RANKING)
    public void GlobalRankingBoard(string[] data)
    {
        List<PlayerScore> playerScore = new List<PlayerScore>();

        for (int i = 0; i < (data.Length / 3); i++)
        {
            if (i < RankBoard.transform.childCount)
                playerScore.Add(new PlayerScore(data[i * 3], int.Parse(data[i * 3 + 1]), int.Parse(data[i * 3 + 2])));
            else 
                break;
        }

        // Display the board
        GetBoardDisplay(playerScore);
    }
    #endregion

    #region MISC (LOCAL RANKING)
    private void LocalRankingBoard()
    {
        List<PlayerScore> playerScore = new List<PlayerScore>();

        foreach (string entry in GuestLogin_Script.thisScript.get_entrytitle)
            playerScore.Add(new PlayerScore(GetNoEntryName(entry), PlayerPrefs.GetInt(entry + "totalRatePoint", 0), PlayerPrefs.GetInt(entry + "PlayedCount_Data", 0)));

        // Display the board
        GetBoardDisplay(playerScore);
    }
    #endregion

    #region MISC (RANKING BOARD)
    private void GetBoardDisplay(List<PlayerScore> list)
    {
        // Sort the list
        list.Sort((a, b) => a.ratePoint.CompareTo(b.ratePoint));
        list.Reverse();

        // Display board
        for (int i = 0; i < list.ToArray().Length; i++)
        {
            if (i < RankBoard.transform.childCount)
            {
                RankBoard.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = GetNoEntryName(list[i].entryName);
                RankBoard.transform.GetChild(i).GetChild(2).GetComponent<Text>().text = FindEntryAuthorised(list[i].entryName, "totalRatePoint") != string.Empty ? "Rate Point: " + list[i].ratePoint : string.Empty;
                RankBoard.transform.GetChild(i).GetChild(3).GetComponent<Text>().text = FindEntryAuthorised(list[i].entryName, "PlayedCount_Data") != string.Empty ? "Last Played: " + list[i].playCount : "- No Record -";
            }
            else break;
        }
    }

    private string GetNoEntryName(string entry)
    {
        if (entry != string.Empty) return entry;
        return "New Entry";
    }

    private string FindEntryAuthorised(string entry, string input)
    {
        if (entry != string.Empty) return PlayerPrefs.GetInt(entry + input, 0).ToString();
        return string.Empty;
    }
    #endregion
}
