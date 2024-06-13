using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PreSelection_Script : MonoBehaviour
{
    public static PreSelection_Script thisPre;

    private GameObject[] BGM;

    private List<AreaInfo> AreaDatabaseArray;
    private AreaInfo AreaDatabase;

    private int musicCounter = 0;
    private int maxArea;
    private int currentArea = 0;

    [Header("Area Processing: Component")]
    [SerializeField] private GameObject NextBtn;
    [SerializeField] private GameObject AvailableBox;
    [SerializeField] private GameObject[] DifficultyArea;

    [Header("Nagivator: Component")]
    [SerializeField] private GameObject LeftNav;
    [SerializeField] private GameObject RightNav;
    [SerializeField] private GameObject ScrollBar;

    private delegate void GetToogleOverNagivator();
    private GetToogleOverNagivator nagivator = null;

    // Other method of gets
    public AreaInfo get_AreaData { get { return AreaDatabase; } }
    public int get_currentArea { get { return currentArea; } }
    public int get_counter { get { return musicCounter; } }

    // Start is called before the first frame update
    void Start()
    {
        thisPre = this;
        GameObject.Find("Selection").GetComponent<Animator>().SetTrigger("Opening");

        AreaDatabaseArray = new List<AreaInfo>();
        StartCoroutine(GatherInfoAreaData());
        Invoke("LoadAudio_Assigned", 0.5f);
    }

    #region SETUP
    public void LoadAudio_Assigned()
    {
        BGM = GameObject.FindGameObjectsWithTag("BGM");
        if (BGM.Length > 1) { for (int i = 1; i < BGM.Length; i++) { Destroy(BGM[i]); } }
    }
    #endregion

    #region MAIN
    public void GetNagivatorClick(int click)
    {
        nagivator = PerformNagivatorUpdate(click);
        if (nagivator != null) nagivator();
    }

    public void GetAreaConfirm()
    {
        CloseWindowOfAreaBoard();
    }
    #endregion

    #region COMPONENT
    private void GetSeasonArea()
    {
        maxArea = AreaDatabaseArray.ToArray().Length;

        // Only maxArea is more than 0 set it to 1
        currentArea = maxArea > 0 ? 1 : 0;
        ScrollBar.GetComponent<Slider>().maxValue = maxArea;
        ScrollBar.GetComponent<Slider>().value = currentArea;

        // Load current area
        AreaDatabase = AreaDatabaseArray[currentArea - 1];
        DifficultyArea[PlayerPrefs.GetInt("BattleDifficulty_Mode", 1) - 1].GetComponent<RawImage>().enabled = true;

        RefreshScrollBarInterface();
        GetAreaInformationDash();
    }

    private IEnumerator GatherInfoAreaData()
    {
        int areaChecker = 0;
        AreaInfo area = null;

        do
        {
            areaChecker++;
            ResourceRequest areaRequest = Resources.LoadAsync<AreaInfo>("Database_Area/Season" + SeasonSelection.thisSelection.get_season + "/A" + areaChecker);
            yield return new WaitUntil(() => areaRequest.isDone);

            area = areaRequest.asset as AreaInfo;
            if (area != null) AreaDatabaseArray.Add(area);

        } while (area != null);

        Debug.Log("Data 1: " + AreaDatabaseArray.ToArray().Length);
        GetSeasonArea();
    }

    private void RefreshScrollBarInterface()
    {
        ScrollBar.GetComponent<Slider>().value = currentArea;
        ScrollBar.transform.GetChild(0).GetComponent<Text>().text = currentArea + "/" + maxArea;

        LeftNav.GetComponent<Button>().interactable = currentArea > 1;
        RightNav.GetComponent<Button>().interactable = currentArea < maxArea;
    }

    private void GetAreaInformationDash()
    {
        AreaDatabase = AreaDatabaseArray[currentArea - 1];

        // Display Title
        GameObject.Find("AreaTitle").transform.GetChild(0).GetComponent<Text>().text = AreaDatabase.AreaName + "\n" + "[ " + AreaDatabase.thisType + " Area ]";
        GameObject.Find("AreaSelection").GetComponent<RawImage>().texture = AreaDatabase.BG;

        // Display UI
        GameObject.Find("FileInfo").transform.GetChild(0).GetComponent<Text>().text = "TOTAL MUSIC: " + AreaDatabase.totalMusic;
        GameObject.Find("AchievementBoard").transform.GetChild(0).GetComponent<Text>().text = "Season " + AreaDatabase.season_num + "\n" + AreaDatabase.package_title;

        GetAreaChecklistOpening();
    }

    private void GetAreaChecklistOpening()
    {
        // Mini Function: Check For Avilable Area
        if (AreaDatabase.checkArea) OpenAreaSelectionMenu();
        else FailedAreaSelectionMenu(0, 1);
    }
    #endregion

    #region MISC (Nagivator Selection)
    private GetToogleOverNagivator PerformNagivatorUpdate(int index)
    {
        GetToogleOverNagivator temp = null;

        switch (index)
        {
            case 0:
                temp += GetReturnScenePage;
                break;

            default:
                temp += index == 1 ? ToggleToPreviousSelection : ToogleToNextSelection;
                temp += RefreshScrollBarInterface;
                temp += GetAreaInformationDash;
                break;
        }

        return temp;
    }

    private void GetReturnScenePage()
    {
        SceneManager.LoadScene("SeasonSelection");
    }

    private void ToggleToPreviousSelection()
    {
        currentArea--;
    }

    private void ToogleToNextSelection()
    {
        currentArea++;
    }
    #endregion

    #region MISC
    public void OpenAreaSelectionMenu() { NextBtn.SetActive(true); AvailableBox.SetActive(false); }

    public void FailedAreaSelectionMenu(int show, int hide) 
    { 
        NextBtn.SetActive(false); 
        AvailableBox.SetActive(true); 
        AvailableBox.transform.GetChild(hide).gameObject.SetActive(false); 
        AvailableBox.transform.GetChild(show).gameObject.SetActive(true); 
    }
    #endregion

    #region MISC (Transition)
    private void CloseWindowOfAreaBoard()
    {
        NextBtn.GetComponent<Button>().interactable = false;
        GameObject.Find("Selection").GetComponent<Animator>().SetTrigger("Closing");
        Invoke("BackgroundContentOfAreaMaterial", 2);
    }

    private void BackgroundContentOfAreaMaterial()
    {
        GameObject.Find("BG").GetComponent<RawImage>().texture = AreaDatabase.BG;
        Invoke("SetSelectionValueOfMusic", 2);
    }

    private void SetSelectionValueOfMusic()
    {
        PlayerPrefs.SetInt("LastSelection_Point", PlayerPrefs.GetInt(AreaDatabase.AreaName + "_LastMusic", 1));
        PlayerPrefs.SetString("LastSelection_Point2", PlayerPrefs.GetString(AreaDatabase.AreaName + "_LastDiffi", "NORMAL"));
        ReadyToTransitFromAreaSelection();
    }

    private void ReadyToTransitFromAreaSelection()
    {
        Destroy(GameObject.Find("BGM"));
        SceneManager.LoadScene("Music Selection Stage");
    }
    #endregion
}
