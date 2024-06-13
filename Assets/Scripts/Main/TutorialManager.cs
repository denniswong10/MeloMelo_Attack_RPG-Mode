using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager thisManager;

    public enum PlayArea { small, medium, large };
    public PlayArea myArea = PlayArea.large;
    private float playBorder = 0;
    public float get_playBorder { get { return playBorder; } }
    private float limit_border = 0;
    public float get_limitBorder { get { return limit_border; } }

    private float battleProgress = 0;
    public float get_battleProgress { get { return battleProgress; } }

    public GameObject Alert_sign;

    private float NextRetreatTime = 0;
    private int RetreatCounter = 3;
    private bool RetreatButton = false;
    private bool RetreatSuccess = false;
    public GameObject Alert_Retreat;

    // Start is called before the first frame update
    void Start()
    {
        thisManager = this;
        PlayArea_Update("Small");
    }

    // Game Signal: Controller
    public void GameStarting()
    {
        try
        {
            Alert_sign.gameObject.SetActive(true);
            Alert_sign.GetComponent<Animator>().SetTrigger("Play");
        }
        catch { }
        Invoke("GameBegin", 5);
    }

    void GameBegin()
    {
        try { Alert_sign.transform.GetChild(0).GetComponent<Text>().text = "MUSIC START!"; } catch { }
        Conductor.thisCondut.Invoke("StartMusicButton", 1);
    }

    // PlayArea Management: Control Update 
    public void PlayArea_Update(string area)
    {
        switch (area)
        {
            case "Small":
                playBorder = 0.15f;
                limit_border = 0.52f;
                break;

            case "Medium":
                playBorder = 0.2f;
                limit_border = 0.8f;
                break;

            case "Large":
                playBorder = 0.28f;
                limit_border = 1.2f;
                break;
        }

        GameObject.Find("PlayArea").transform.localScale = new Vector3(playBorder, 1, 2);
    }

    // Update Function: Score Pugin
    void Update()
    {
        // Exit Sign: Status
        if (Input.GetKey(KeyCode.Escape) && !RetreatSuccess && BeatConductor.thisBeat.get_startNote)
        {
            if (!RetreatButton)
            {
                NextRetreatTime = Time.time + 1;
                RetreatButton = true;

                Alert_Retreat.SetActive(true);
                Alert_Retreat.transform.GetChild(0).GetComponent<Text>().text = "EXIT FROM BATTLE IN " + RetreatCounter + "...";
            }

            if (Time.time >= NextRetreatTime)
            {
                NextRetreatTime = Time.time + 1;
                RetreatCounter--;
                Alert_Retreat.transform.GetChild(0).GetComponent<Text>().text = "EXIT FROM BATTLE IN " + RetreatCounter + "...";

                if (RetreatCounter <= 0)
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Ref_PreSelection");
                }
            }
        }
        else if (RetreatButton && !RetreatSuccess)
        {
            Alert_Retreat.SetActive(false);
            RetreatButton = false;
            RetreatCounter = 3;
        }

        //if (!GameObject.Find("PlayArea").GetComponent<AudioSource>().isPlaying && BeatConductor.thisBeat.get_startNote && !GameManager.thisManager.DeveloperMode) { Invoke("BattleOver", 3); }
    }

    // Update Auto: Battle Progress Meter
    public void UpdateBattle_Progress(float amount)
    {
        battleProgress += amount;
        GameObject.Find("ProgressBar").GetComponent<Slider>().value += amount;
        GameObject.FindGameObjectWithTag("ProgressBar").GetComponent<Text>().text = battleProgress.ToString("0") + "%";

        if (GameObject.Find("ProgressBar").GetComponent<Slider>().value >= 80)
        {
            Color color = new Color32(36, 137, 5, 255);
            GameObject.Find("ProgressBar").transform.GetChild(1).GetChild(0).GetComponent<Image>().color = color;
        }
        else
        {
            Color color = new Color32(255, 0, 0, 255);
            GameObject.Find("ProgressBar").transform.GetChild(1).GetChild(0).GetComponent<Image>().color = color;
        }
    }

    // Update Auto: All Note (Perfect, Bad, Miss)
    public void UpdateNoteStatus(string index)
    {
        Vector3 position = new Vector3(GameObject.Find("Character").transform.position.x, GameObject.Find("Judgement Line").transform.position.y, GameObject.Find("Judgement Line").transform.position.z);
        Instantiate(Resources.Load<GameObject>("Prefabs/PopUp/" + index), position, Quaternion.identity);
    }   
}
