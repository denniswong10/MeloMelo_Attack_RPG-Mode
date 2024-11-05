using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MeloMelo_RatingMeter;

public class Menu : MonoBehaviour
{
    public static Menu thisMenu;
    private GameObject[] BGM;

    private RatePointIndicator Profile = new RatePointIndicator();
    public Text[] menuContentTxt;

    [Header("NewsUpdate Component")]
    public RawImage Display_ReportError;
    public GameObject NewsBoard;
    public Text NewsBlockElement;

    private string userInput;

    void Start()
    {
        thisMenu = this;
        userInput = LoginPage_Script.thisPage.GetUserPortOutput();

        BGM_Setup();
        CallUpOptionTask();
    }

    #region SETUP 
    private void BGM_Setup()
    {
        BGM = GameObject.FindGameObjectsWithTag("BGM");
        if (BGM.Length > 1) { for (int i = 1; i < BGM.Length; i++) { Destroy(BGM[i]); } }
    }

    private void CallUpOptionTask()
    {
        // Top Section: Profile, Version
        Profile.ProfileUpdate(userInput, true, "Played Count: ", "Rate Point: ");
        VersionUpdate();

        // Right Section: News Report
        UpdateNewsReport();
    }
    #endregion

    #region MAIN
    public void Menu_InteractANDTransition(string scene)
    {
        // Started nagivator through menu
        if (scene != "Quit") SceneManager.LoadScene(scene);
        else Application.Quit();
    }
    #endregion

    #region COMPONENT
    private bool CheckingNetworkReachable()
    {
        if (PlayerPrefs.GetInt("serverEnable", 0) == 1) return true;
        else return false;
    }

    private void VersionUpdate()
    {
        string latestBuild = PlayerPrefs.GetString("GameLatest_Update", string.Empty);

        menuContentTxt[0].text = "CURRENT VERSION: " + StartMenu_Script.thisMenu.get_version;
        if (latestBuild != string.Empty) { menuContentTxt[2].text = "LATEST VERSION: " + latestBuild; }
        else { menuContentTxt[2].text = string.Empty; }
    }
    #endregion

    #region SETUP (NEWS UPDATE)
    [System.Serializable]
    struct NewsReportFormat
    {
        public string title;
        public string releasedDate;
        public string description;

        public NewsReportFormat GetReport(string format)
        {
            Debug.Log(format);
            return JsonUtility.FromJson<NewsReportFormat>(format);
        }
    }

    [System.Serializable]
    struct NewsReportList
    {
        public NewsReportFormat[] data_array;

        public NewsReportList GetReportArray(string format)
        {
            Debug.Log(format);
            return JsonUtility.FromJson<NewsReportList>(format);
        }
    }
    #endregion

    #region COMPONENT (NEWS UPDATE)
    private void UpdateNewsReport()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            if (PlayerPrefs.HasKey("AccountSync") && PlayerPrefs.HasKey("MeloMelo_NewsReport_Daily"))
            {
                string reportFormat = PlayerPrefs.GetString("MeloMelo_NewsReport_Daily");
                NewsReportList reportListing = new NewsReportList().GetReportArray(reportFormat);

                foreach (NewsReportFormat newsBlocking in reportListing.data_array)
                    CreateNewsLetter(newsBlocking.title, newsBlocking.description, newsBlocking.releasedDate);

                if (reportListing.data_array.Length == 0) UpdateErrorReport("THERE IS NO NEWS UPDATE");
            }
            else
                UpdateErrorReport("SIGN IN TO GUEST LOGIN");
        }
        else
            UpdateErrorReport("OFFLINE MODE");
    }

    private void UpdateErrorReport(string message)
    {
        Display_ReportError.gameObject.SetActive(true);
        Display_ReportError.transform.GetComponentInChildren<Text>().text = message;
    }

    private void CreateNewsLetter(string title, string description, string timeStamp)
    {
        Text newsBlock = Instantiate(NewsBlockElement);
        newsBlock.text = title + "\n" + timeStamp + "\n------------------------------\n" + description + "\n\n";
        newsBlock.transform.SetParent(NewsBoard.transform.GetChild(1).GetChild(0).transform);
    }
    #endregion
}
