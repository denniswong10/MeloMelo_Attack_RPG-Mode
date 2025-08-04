using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskTrackRecord_Script : MonoBehaviour
{
    [SerializeField] private GameObject[] entry;
    [SerializeField] private Text scoreboard;
    [SerializeField] private Texture emptyCover;
    private float giveUpTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.DeleteKey("TrackList_Provoke");
        PlayerPrefs.DeleteKey("TrackList_End_List");

        CreateNewEntry();
        StartCoroutine(DisplayAllEntry());
    }

    void Update()
    {
        if (PlayerPrefs.HasKey("TrackList_Provoke"))
        {
            if (Time.time >= giveUpTimer) { PlayerPrefs.SetInt("TrackList_End_List", 1); transform.GetChild(6).GetChild(0).GetComponent<Text>().text = "EXIT"; }
            else transform.GetChild(6).GetChild(0).GetComponent<Text>().text = "WAIT (" + (int)(giveUpTimer - Time.time) + "s)";
        }
    }

    #region MAIN
    public void GiveUpOptions()
    {
        if (!PlayerPrefs.HasKey("TrackList_Provoke") && !PlayerPrefs.HasKey("TrackList_End_List"))
        {
            PlayerPrefs.SetInt("TrackList_Provoke", 1);
            giveUpTimer = Time.time + 6;
        }
        else
        {
            PlayerPrefs.DeleteKey("TrackList_Provoke");
            transform.GetChild(6).GetChild(0).GetComponent<Text>().text = "GIVE UP";
        }
    }

    private void CreateNewEntry()
    {
        // Create first entry via empty entry
        for (int id = 0; id < entry.Length; id++)
        {
            // Find empty entry score
            if (PlayerPrefs.GetInt("TrackListRecord_Score" + id, 0) == 0)
            {
                // Record current play session to list
                PlayerPrefs.SetString("TrackListRecord_Title" + id, BeatConductor.thisBeat.Music_Database.Title);
                PlayerPrefs.SetInt("TrackListRecord_Score" + id, (int)GameManager.thisManager.get_score1.get_score);
                PlayerPrefs.SetString("TrackListRecord_PlayDifficulty" + id, TrackLevelInfo());

                PlayerPrefs.SetString("TrackListRecord_CoverImage" + id, "Database_CoverImage/CoverImage_" + BeatConductor.thisBeat.Music_Database.seasonNo + "/" +
                    BeatConductor.thisBeat.Music_Database.Background_Cover.name);
                break;
            }
        }
    }

    private IEnumerator DisplayAllEntry()
    {
        yield return new WaitForSeconds(0.5f);
        transform.GetChild(0).GetComponent<Text>().text = "Track List - " + PlayerPrefs.GetString("Marathon_Title_Text", "???");

        // Show scoreboard and update score for all available entry
        for (int id = 0; id < entry.Length; id++)
        {
            bool isScoreInTact = PlayerPrefs.GetInt("TrackListRecord_Score" + id) > 0;
            entry[id].transform.GetChild(1).gameObject.SetActive(isScoreInTact);
            entry[id].transform.GetChild(2).gameObject.SetActive(!isScoreInTact);
            EditTrackDetails(id);
            yield return new WaitForSeconds(1);
        }

        // Calculate all played track with score
        yield return new WaitForSeconds(0.5f);
        scoreboard.text = TotalScoreCalculate().ToString("#,#");

        // Check for progression
        CheckEntryCompleted();
    }

    private int TotalScoreCalculate()
    {
        int total = 0;

        for (int id = 0; id < entry.Length; id++)
            if (PlayerPrefs.HasKey("TrackListRecord_Score" + id))
                total += PlayerPrefs.GetInt("TrackListRecord_Score" + id, 0);

        return total;
    }
    #endregion

    #region COMPONENT
    private void EditTrackDetails(int id)
    {
        if (entry[id].transform.GetChild(1).gameObject.activeInHierarchy)
        {
            Texture cover = Resources.Load<Texture>(PlayerPrefs.GetString("TrackListRecord_CoverImage" + id, string.Empty));
            entry[id].transform.GetChild(0).GetComponent<RawImage>().texture = cover != null ? cover : emptyCover;

            entry[id].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetString("TrackListRecord_Title" + id, string.Empty);
            entry[id].transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "Score: " + PlayerPrefs.GetInt("TrackListRecord_Score" + id, 0).ToString();
            entry[id].transform.GetChild(1).GetChild(2).GetComponent<Text>().text = MeloMelo_GameSettings.GetScoreRankStructure(PlayerPrefs.GetInt("TrackListRecord_Score" + id, 0).ToString()).rank;
            entry[id].transform.GetChild(1).GetChild(3).GetComponent<Text>().text = PlayerPrefs.GetString("TrackListRecord_PlayDifficulty" + id, string.Empty);
        }
    }

    private string TrackLevelInfo()
    {
        switch (PlayerPrefs.GetInt("DifficultyLevel_valve", 1))
        {
            case 1:
                return "NORMAL [Lv. " + PlayerPrefs.GetString("Difficulty_Normal_selectionTxt", "?") + "]";

            case 2:
                return "HARD [Lv. " + PlayerPrefs.GetString("Difficulty_Hard_selectionTxt", "?") + "]";

            case 3:
                return "ULTIMATE [Lv. " + PlayerPrefs.GetString("Difficulty_Ultimate_selectionTxt", "?") + "]";

            default:
                return "?";
        }
    }

    private void CheckEntryCompleted()
    {
        transform.GetChild(7).GetComponent<Button>().interactable = true;
        transform.GetChild(6).gameObject.SetActive(PlayerPrefs.GetInt("MarathonChallenge_MCount", 1) != entry.Length);
        transform.GetChild(7).GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetInt("MarathonChallenge_MCount", 1) == entry.Length ? "FINISH" : "NEXT";
    }
    #endregion
}
