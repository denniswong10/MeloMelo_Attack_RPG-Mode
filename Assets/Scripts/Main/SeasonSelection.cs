using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SeasonSelection : MonoBehaviour
{
    public static SeasonSelection thisSelection;

    // Season: Get Reference
    private int season = 0;
    public int get_season { get { return season; } }

    [Header("Content Fill: Componenet")]
    [SerializeField] private GameObject AlertBox;
    [SerializeField] private GameObject contentLoader;
    [SerializeField] private GameObject contentTemplate;

    private GameObject[] BGM;

    // Start is called before the first frame update
    void Start()
    {
        thisSelection = this;
        LoadAudio_Assigned();

        // Content: Browse through all resources
        LoadAllContent();
    }

    #region SETUP
    private void LoadAudio_Assigned()
    {
        BGM = GameObject.FindGameObjectsWithTag("BGM");
        if (BGM.Length > 1) { for (int i = 1; i < BGM.Length; i++) { Destroy(BGM[i]); } }
    }
    #endregion

    #region SETUP
    private void LoadAllContent()
    {
        foreach (ContentBundleData contentLog in Resources.LoadAll<ContentBundleData>("Database_Content_Management"))
        {
            GameObject content = Instantiate(contentTemplate);
            content.GetComponent<SeasonTemplateData>().SetEntryRestriction(contentLog.isRestricted);
            content.GetComponent<SeasonTemplateData>().AssignNewIndex(contentLog.content_id);

            content.transform.GetChild(0).GetComponent<RawImage>().texture = contentLog.Area_BG;
            content.transform.GetChild(1).GetComponent<Text>().text = contentLog.title;
            content.transform.GetChild(3).GetComponent<Text>().text = contentLog.sub_title;
            content.transform.GetChild(4).GetChild(0).GetComponent<Text>().text = contentLog.chartType == ContentBundleData.ChartType.Legacy ? "LEGACY" : "NEW CHART";
            content.transform.GetChild(5).gameObject.SetActive(contentLog.newContent);
            content.transform.SetParent(contentLoader.transform);
        }
    }
    #endregion

    #region MAIN
    public void GoBackScene()
    {
        SceneManager.LoadScene("Ref_PreSelection");
    }

    public void ContinueTemplateSelection(bool requireEntry, int season_index)
    {
        season = season_index;
        if (requireEntry) ProcessRequiredPoint(0);
        else ProcessToAreaSelection();
    }
    #endregion

    #region MAIN (HANDLER)
    public void Interaction_AlertBox(bool closePanel)
    {
        AlertBox.SetActive(false);
        if (!closePanel) ProcessToAreaSelection();
    }
    #endregion

    #region COMPONENT
    private void ProcessRequiredPoint(int acculatePoint)
    {
        if (LoginPage_Script.thisPage.get_user == "GUEST") acculatePoint = 0;

        AlertBox.SetActive(true);
        AlertBox.transform.GetChild(2).GetComponent<Text>().text =
            "Current Point: " + PlayerPrefs.GetInt(LoginPage_Script.thisPage.get_user + "totalRatePoint", 0) + 
            "\n" + "Required Point: " + acculatePoint;

        if (PlayerPrefs.GetInt(LoginPage_Script.thisPage.get_user + "totalRatePoint", 0) >= acculatePoint)
        {
            AlertBox.transform.GetChild(3).GetComponent<Button>().interactable = true;
        }
        else { AlertBox.transform.GetChild(3).GetComponent<Button>().interactable = false; }
    }

    private void ProcessToAreaSelection()
    {
        SceneManager.LoadScene("AreaSelection");
    }
    #endregion
}
