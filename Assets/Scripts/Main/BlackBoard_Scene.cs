using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BlackBoard_Scene : MonoBehaviour
{
    public static BlackBoard_Scene myBoard;

    private MusicScore Music_Data = null;
    public MusicScore get_musicData { get { return Music_Data; } }

    public Text Mission_Txt;
    public Text None_Text;
    public Text Search_Text;
    public Text Error_Text;

    public Button ProcessBtn;

    private string[] Text_Element = new string[5];

    // Start is called before the first frame update
    void Start()
    {
        myBoard = this;
        PlayerPrefs.SetInt("Mission_Played", 1);

        if (!PlayerPrefs.HasKey("Mission_Title")) { Search_Text.gameObject.SetActive(true); StartCoroutine(Reload_NewMission()); }
        else { LoadMissionProfile(); }
    }

    IEnumerator Reload_NewMission()
    {
        WWWForm load = new WWWForm();
        load.AddField("ratePoint", PlayerPrefs.GetInt(LoginPage_Script.thisPage.get_user + "totalRatePoint", 0));

        UnityWebRequest mission = UnityWebRequest.Post("https://denniswong10-webpage.ml/database/transcripts/site5/MeloMelo_MissionList.php", load);
        yield return mission.SendWebRequest();

        if (mission.downloadHandler.text == "")
        {
            LoadMission_None();
        }
        else
        {
            string[] info = mission.downloadHandler.text.Split('\t');
            Debug.Log(mission.downloadHandler.text);
            int assign = Random.Range(0, info.Length / 5);

            PlayerPrefs.SetString("Mission_Title", info[assign * 5]);
            PlayerPrefs.SetString("Mission_Area", info[assign * 5 + 1]);
            PlayerPrefs.SetString("Mission_ClearCondition", info[assign * 5 + 2]);
            PlayerPrefs.SetString("Mission_Reward", info[assign * 5 + 3]);
            PlayerPrefs.SetString("Mission_Difficulty", info[assign * 5 + 4]);

            Debug.Log("R: " + PlayerPrefs.GetString("Mission_Title"));
            LoadMissionProfile();
        }

        // Load and clean
        mission.Dispose();
    }

    void LoadMissionProfile()
    {
        if (Search_Text.gameObject.activeInHierarchy) { Search_Text.gameObject.SetActive(false); }
        Music_Data = Resources.Load<MusicScore>("Database_Area/" + PlayerPrefs.GetString("Mission_Area") + "/" + PlayerPrefs.GetString("Mission_Title"));
        Mission_Txt.gameObject.SetActive(true);
        ProcessBtn.interactable = true;

        if (Music_Data != null)
        {
            Text_Element[0] = "Artist: " + Music_Data.ArtistName + "\n";
            Text_Element[1] = "Title: " + Music_Data.Title + "\n";
            Text_Element[2] = "\n" + "Clear Condition: " + "\n" + PlayerPrefs.GetString("Mission_ClearCondition", "--") + "\n";
            Text_Element[3] = "\n" + "Reward: " + PlayerPrefs.GetString("Mission_Reward", "--") + "\n";
            Text_Element[4] = "Difficulty: " + PlayerPrefs.GetString("Mission_Difficulty", "--");
        }
        else
            Error_Text.gameObject.SetActive(true);

        foreach (string i in Text_Element) { Mission_Txt.text += i; }
    }

    void LoadMission_None()
    {
        if (Search_Text.gameObject.activeInHierarchy) { Search_Text.gameObject.SetActive(false); }
        None_Text.gameObject.SetActive(true);
    }

    public void Challenge()
    {
        PlayerPrefs.SetInt("DifficultyLevel_valve", ((PlayerPrefs.GetString("Mission_Difficulty", "--") == "Normal") ? 1 : 2));
        SceneManager.LoadScene("BattleSetup");
    }

    public void ReturnToMain()
    {
        PlayerPrefs.DeleteKey("Mission_Played");
        SceneManager.LoadScene("Ref_PreSelection");
    }
}
