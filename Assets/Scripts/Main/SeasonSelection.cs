using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SeasonSelection : MonoBehaviour
{
    public static SeasonSelection thisSelection;

    private int season = 0;
    public int get_season { get { return season; } }

    private GameObject[] BGM;
    private string ResMelo = string.Empty;

    public GameObject AlertBox;

    // Start is called before the first frame update
    void Start()
    {
        thisSelection = this;
        LoadAudio_Assigned();
    }

    #region SETUP
    public void LoadAudio_Assigned()
    {
        BGM = GameObject.FindGameObjectsWithTag("BGM");
        if (BGM.Length > 1) { for (int i = 1; i < BGM.Length; i++) { Destroy(BGM[i]); } }
    }
    #endregion

    #region MAIN
    public void GoBackScene()
    {
        SceneManager.LoadScene("Ref_PreSelection" + ResMelo);
    }

    public void SelectSeason(int index)
    {
        season = index;
        SceneManager.LoadScene("AreaSelection");
    }
    #endregion

    public void ProcessRequiredPoint(int acculatePoint)
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

    public void CloseProcessRequiredPoint()
    {
        AlertBox.SetActive(false);
    }
}
