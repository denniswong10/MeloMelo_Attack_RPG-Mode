using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MeloMelo_RatingMeter;

public class Menu : MonoBehaviour
{
    private GameObject[] BGM;
    [SerializeField] private Text CurrentVersion;
    [SerializeField] private Text LatestVersion;

    [Header("NewsUpdate Component")]
    public RawImage Display_ReportError;
    public GameObject NewsBoard;
    public Text NewsBlockElement;

    void Start()
    {
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
        RatePointIndicator profile = new RatePointIndicator("Profile");
        profile.ProfileUpdate(LoginPage_Script.thisPage.GetUserPortOutput(), "Played Count: ", "Rate Point: ");
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
    private void VersionUpdate()
    {
        string latestBuild = PlayerPrefs.GetString("GameLatest_Update", string.Empty);

        if (CurrentVersion != null) CurrentVersion.text = "CURRENT VERSION: " + StartMenu_Script.thisMenu.version;
        if (LatestVersion != null) LatestVersion.text = latestBuild != string.Empty ? ("LATEST VERSION: " + latestBuild) : string.Empty;
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
            if (MeloMelo_PlayerSettings.GetLocalUserAccount() && PlayerPrefs.HasKey("MeloMelo_NewsReport_Daily"))
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
