using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ArenaSelection_Script : MonoBehaviour
{
    public static ArenaSelection_Script thisArena;

    private bool InfoOpen = false;
    public GameObject difficulty_valve;

    public MusicScore[] MusicList = null;
    private int music_selector = 1;
    public int get_selector { get { return music_selector; } }

    [Header("Additional Content")]
    public GameObject MusicInformation_Txt;
    public GameObject Selection_Btn;
    public GameObject ScrollBar_obj;

    public GameObject[] BGM;
    private bool loadBGM = false;
    public bool get_loadBGM { get { return loadBGM; } }

    [Header("Selection Option")]
    public GameObject[] nav_select = new GameObject[2];

    [Header("Achievement Progress")]
    public GameObject AchivementBoard;
    public GameObject AchivementBoard_E;
    public GameObject NewIcon;
    public GameObject RemarkIcon;

    private float difficulty_normal = 0;
    public float get_normal { get { return difficulty_normal; } }

    private float difficulty_hard = 0;
    public float get_hard { get { return difficulty_hard; } }

    // Start is called before the first frame update
    void Start()
    {
        thisArena = this;
        BGM = GameObject.FindGameObjectsWithTag("BGM");

        StartCoroutine(LoadMusicContentServer());
    }

    // Update: Close this Menu
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !InfoOpen)
        {
            GameObject.Find("Selection").GetComponent<Animator>().SetTrigger("Closing");
            Invoke("ReturnMain", 2);
        }
    }

    #region MainFunction
    // Return Button: Go PreSelection
    public void ReturnMain()
    {
        if (GameObject.Find("BGM").activeInHierarchy && BGM[0] != null) { Destroy(BGM[0]); }
        SceneManager.LoadScene("Ref_PreSelection");
    }

    // Checking Data: Load Server Intitization
    void CheckingArenaSetup()
    {
        GameObject.Find("Selection").GetComponent<Animator>().SetTrigger("Opening");
        StartCoroutine(OpeningSelection());
    }

    IEnumerator OpeningSelection()
    {
        yield return new WaitForSeconds(1.5f);

        BGM = GameObject.FindGameObjectsWithTag("BGM");
        if (GameObject.Find("BGM").activeInHierarchy && BGM[0] != null) { DontDestroyOnLoad(BGM[0]); }
        if (BGM.Length > 1) { Destroy(BGM[1]); }

        loadBGM = true;
        LoadScrollBar();
        Invoke("DifficultyChanger_encode", 0.06f);
    }

    // Loader: Music Content
    void LoadContent()
    {
        if (GameObject.FindGameObjectWithTag("ScoreSheet"))
        {
            GameObject scoreSheet = GameObject.FindGameObjectWithTag("ScoreSheet");
            if (scoreSheet.activeInHierarchy) { Destroy(scoreSheet); }
        }

        for (int i = 0; i < MusicList.Length; i++)
        {
            if (i == (music_selector - 1))
            {
                Selection_Btn.transform.GetChild(0).GetComponent<RawImage>().texture = MusicList[i].Background_Cover;
                MusicInformation_Txt.transform.GetChild(0).GetComponent<Text>().text = "[ " + MusicList[i].ArtistName + " ]";
                MusicInformation_Txt.transform.GetChild(1).GetComponent<Text>().text = MusicList[i].Title;
                MusicInformation_Txt.transform.GetChild(2).GetComponent<Text>().text = "BPM: " + MusicList[i].BPM.ToString("0");
                MusicInformation_Txt.transform.GetChild(3).GetComponent<Text>().text = "Level Designer by " + MusicList[i].DesignerName;

                if (BGM[0] != null)
                {
                    if (BGM[0].GetComponent<AudioSource>().volume != 1) { BGM[0].GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat(MeloMelo_PlayerSettings.GetBGM_ValueKey, 1); }
                    BGM[0].GetComponent<AudioSource>().clip = MusicList[i].Music;
                    BGM[0].GetComponent<AudioSource>().time = MusicList[i].PreviewTime;
                    BGM[0].GetComponent<AudioSource>().Play();
                }

                if (MusicList[i].ScoreObject != null)
                {
                    Instantiate(MusicList[i].ScoreObject, transform.position, Quaternion.identity);
                    Invoke("DifficultyChanger_encode", 0.06f);
                }
                else
                {
                    GameObject.FindGameObjectWithTag("DifficultyDisplaySelection").transform.GetChild(0).GetComponent<Text>().text = "?";
                    if (NewIcon.activeInHierarchy) { NewIcon.SetActive(false); }
                    if (AchivementBoard.activeInHierarchy) { AchivementBoard.SetActive(false); }
                    if (AchivementBoard_E.activeInHierarchy) { AchivementBoard_E.SetActive(false); }
                    if (RemarkIcon.activeInHierarchy) { RemarkIcon.SetActive(false); }
                }
                break;
            }
        }

        GameObject.Find("PlayerName").GetComponent<Text>().text = "Player: " + PlayerPrefs.GetString(MusicList[music_selector - 1].Title + "_AssignedUser", "--");
        GameObject.Find("RankPlate").GetComponent<Text>().text = "Highest Rank: " + PlayerPrefs.GetString(MusicList[music_selector - 1].Title + "_AssignedRank", "--");
    }

    // Function Component: Music Index Navigator
    void LoadScrollBar()
    {
        ScrollBar_obj.GetComponent<Slider>().maxValue = MusicList.Length;
        ScrollBar_obj.transform.GetChild(0).GetComponent<Text>().text = music_selector + " / " + MusicList.Length;

        CheckForNav();
        LoadContent();
    }

    public void MusicChanger()
    {
        music_selector = (int)ScrollBar_obj.GetComponent<Slider>().value;
        ScrollBar_obj.transform.GetChild(0).GetComponent<Text>().text = music_selector + " / " + MusicList.Length;

        CheckForNav();
        LoadContent();
    }

    // Function Component: Checking Button Active
    void CheckForNav()
    {
        if (music_selector <= ScrollBar_obj.GetComponent<Slider>().minValue) { nav_select[0].GetComponent<Button>().interactable = false; }
        else if (!nav_select[0].GetComponent<Button>().interactable) { nav_select[0].GetComponent<Button>().interactable = true; }

        if (music_selector >= ScrollBar_obj.GetComponent<Slider>().maxValue) { nav_select[1].GetComponent<Button>().interactable = false; }
        else if (!nav_select[1].GetComponent<Button>().interactable) { nav_select[1].GetComponent<Button>().interactable = true; }
    }

    // Selection Button: Music Select
    public void NavChanger(bool left)
    {
        switch (left)
        {
            case true:
                ScrollBar_obj.GetComponent<Slider>().value--;
                break;

            case false:
                ScrollBar_obj.GetComponent<Slider>().value++;
                break;
        }
    }

    // Selection Button: Difficulty Select
    protected void DifficultyChanger_encode() { DifficultyChanger(PlayerPrefs.GetString(MusicList[music_selector - 1].Title + "_AssignedDiff", "NORMAL"), false); }
    public void DifficultyChanger(string list, bool onClick)
    {
        if (MusicList[music_selector - 1].ScoreObject != null)
        {
            Color thisColor = new Color(0, 0, 0);

            switch (list)
            {
                case "NORMAL":
                    thisColor = new Color(0, 0, 1);
                    PlayerPrefs.SetInt("DifficultyLevel_valve", 1);
                    if (difficulty_normal > 5.5f) { GameObject.FindGameObjectWithTag("DifficultyDisplaySelection").transform.GetChild(1).GetComponent<Text>().text = "NORMAL+"; }
                    else { GameObject.FindGameObjectWithTag("DifficultyDisplaySelection").transform.GetChild(1).GetComponent<Text>().text = list.ToString(); }

                    GameObject.FindGameObjectWithTag("DifficultyDisplaySelection").transform.GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetString("Difficulty_Normal_selectionTxt", "?");
                    break;

                case "HARD":
                    thisColor = new Color(1, 0, 0);
                    PlayerPrefs.SetInt("DifficultyLevel_valve", 2);
                    if (difficulty_hard > 10.5f) { GameObject.FindGameObjectWithTag("DifficultyDisplaySelection").transform.GetChild(1).GetComponent<Text>().text = "HARD+"; }
                    else { GameObject.FindGameObjectWithTag("DifficultyDisplaySelection").transform.GetChild(1).GetComponent<Text>().text = list.ToString(); }

                    GameObject.FindGameObjectWithTag("DifficultyDisplaySelection").transform.GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetString("Difficulty_Hard_selectionTxt", "?");
                    break;
            }

            difficulty_valve.GetComponent<RawImage>().color = thisColor;

            // Show Achievement Status
            switch (difficulty_valve.name)
            {
                case "NORMAL":
                    if (PlayerPrefs.GetInt(MusicList[music_selector - 1].Title + "_score1", 0) != 0)
                    {
                        NewIcon.SetActive(false);
                        AchivementBoard.SetActive(true);
                        AchivementBoard_E.SetActive(false);
                        AchivementBoard.transform.GetChild(1).GetComponent<Text>().text = "Score: " + PlayerPrefs.GetInt(MusicList[music_selector - 1].Title + "_score1", 0).ToString("0000000") + " | Rank: " + PlayerPrefs.GetString(MusicList[music_selector - 1].Title + "_rank1", "F");

                        RemarkIcon.SetActive(true);
                        AchieveRemark(1);
                        AchieveStatusRemark(1);
                    }
                    else
                    {
                        AchivementBoard.SetActive(false);
                        AchivementBoard_E.SetActive(true);
                        NewIcon.SetActive(true);
                        RemarkIcon.SetActive(false);
                    }
                    break;

                case "HARD":
                    if (PlayerPrefs.GetInt(MusicList[music_selector - 1].Title + "_score2", 0) != 0)
                    {
                        NewIcon.SetActive(false);
                        AchivementBoard.SetActive(true);
                        AchivementBoard_E.SetActive(false);
                        AchivementBoard.transform.GetChild(1).GetComponent<Text>().text = "Score: " + PlayerPrefs.GetInt(MusicList[music_selector - 1].Title + "_score2", 0).ToString("0000000") + " | Rank: " + PlayerPrefs.GetString(MusicList[music_selector - 1].Title + "_rank2", "F");

                        RemarkIcon.SetActive(true);
                        AchieveRemark(2);
                        AchieveStatusRemark(2);
                    }
                    else
                    {
                        AchivementBoard.SetActive(false);
                        AchivementBoard_E.SetActive(true);
                        NewIcon.SetActive(true);
                        RemarkIcon.SetActive(false);
                    }
                    break;
            }
        }
    }

    // Achievement Remark: Function
    protected void AchieveRemark(int index)
    {
        switch (PlayerPrefs.GetInt(MusicList[music_selector - 1].Title + "_BattleRemark_" + index, 6))
        {
            case 1:
                RemarkIcon.transform.GetChild(0).GetComponent<Text>().text = "PERFECT!";
                RemarkIcon.GetComponent<RawImage>().color = Color.yellow;
                break;

            case 2:
                RemarkIcon.transform.GetChild(0).GetComponent<Text>().text = "ALL ELIMINATE!";
                RemarkIcon.GetComponent<RawImage>().color = Color.blue;
                break;

            case 3:
                RemarkIcon.transform.GetChild(0).GetComponent<Text>().text = "MISSLESS!";
                RemarkIcon.GetComponent<RawImage>().color = Color.green;
                break;

            case 4:
                RemarkIcon.transform.GetChild(0).GetComponent<Text>().text = "CLEARED!";
                RemarkIcon.GetComponent<RawImage>().color = Color.green;
                break;

            default:
                RemarkIcon.transform.GetChild(0).GetComponent<Text>().text = "DEFEATED!";
                RemarkIcon.GetComponent<RawImage>().color = Color.red;
                break;
        }
    }

    // Achievement Status: Color Remark
    protected void AchieveStatusRemark(int index)
    {
        int score = int.Parse(PlayerPrefs.GetInt(MusicList[music_selector - 1].Title + "_score" + index, 0).ToString("0000000"));

        // Color Changer
        if (score == 1000000)
        {
            AchivementBoard.GetComponent<RawImage>().color = new Color(1, 0, 1);
        }
        else if (score >= 950000) { AchivementBoard.GetComponent<RawImage>().color = Color.yellow; }
        else if (score >= 900000) { AchivementBoard.GetComponent<RawImage>().color = Color.green; }
        else if (score >= 800000) { AchivementBoard.GetComponent<RawImage>().color = Color.blue; }
        else if (score >= 500000) { AchivementBoard.GetComponent<RawImage>().color = Color.gray; }
        else { AchivementBoard.GetComponent<RawImage>().color = Color.red; }
    }

    // Selection: Record Data Difficuly Level
    public void UpdateData_Level(int index, float level)
    {
        switch (index)
        {
            case 1:
                difficulty_normal = level;
                if (level - ((int)level + 0.5f) > 0f && level > 5) { PlayerPrefs.SetString("Difficulty_Normal_selectionTxt", (int)level + "+"); }
                else { PlayerPrefs.SetString("Difficulty_Normal_selectionTxt", (int)level + ""); }
                break;

            case 2:
                difficulty_hard = level;
                if (level - ((int)level + 0.5f) > 0f && level > 10) { PlayerPrefs.SetString("Difficulty_Hard_selectionTxt", (int)level + "+"); }
                else { PlayerPrefs.SetString("Difficulty_Hard_selectionTxt", (int)level + ""); }
                break;
        }
    }
    #endregion

    #region ServerCoding
    // Requesting Server: Load Music Content
    IEnumerator LoadMusicContentServer()
    {
        WWWForm load = new WWWForm();
        UnityWebRequest loadWeb = UnityWebRequest.Post("https://denniswong10-webpage.ml/database/transcripts/site5/UnityMeloArena.php", load);
        yield return loadWeb.SendWebRequest();

        // Retrieve Data
        if (loadWeb.downloadHandler.text != "error")
        {
            string[] length = loadWeb.downloadHandler.text.Split('\n');
            MusicList = new MusicScore[length.Length / 6];

            for (int i = 0; i < MusicList.Length; i++)
            {
                MusicList[i] = Resources.Load<MusicScore>("Database_Area/" + length[i * 6 + 2] + "/" + length[i * 6]);
                PlayerPrefs.SetString(length[i * 6] + "_AssignedDiff", length[i * 6 + 1]);
                PlayerPrefs.SetString(length[i * 6] + "_AssignedUser", length[i * 6 + 3]);
                PlayerPrefs.SetString(length[i * 6] + "_AssignedRank", length[i * 6 + 4]);
            }

            // Load Music Selection
            CheckingArenaSetup();
        }
        else { GameObject.Find("Selection_None").GetComponent<Animator>().SetTrigger("Opening"); }
    }
    #endregion
}
