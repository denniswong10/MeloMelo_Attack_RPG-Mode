using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MeloMelo_Story;

public class StoryMode_Scripts : MonoBehaviour
{
    public static StoryMode_Scripts thisStory;

    [Header("Database_StoryMode")]
    private StoryInfo[] myStoryList;

    [Header("Main UI - Channel 1")]
    public GameObject Menu_Selection;
    public RawImage StoryBG;
    public Text StoryTitle;
    public GameObject ProcessBtn;

    [Header("Main UI - Channel 2")]
    public GameObject Menu_Selection2;
    public RawImage StoryBG2;
    public Text StoryTitle2;
    public Text HeadTitle;
    public Text FragmentAmt;
    public GameObject BeginBtn;
    public GameObject[] Slot;
    public GameObject[] EpsiodeStatus = new GameObject[4];

    [Header("Main UI - Channel 3")]
    public GameObject Menu_Selection3;
    public Text StoryDes2;
    public GameObject ContinueBtn;

    [Header("Nagivation_Select")]
    public GameObject Left;
    public GameObject Right;
    public Slider NagivatorBar;
    private int selector = 1;

    [Header("Nagivation_Select2")]
    public GameObject Left2;
    public GameObject Right2;
    private int selector2 = 1;

    [Header("Channel Management")]
    private bool mainChannel = true;

    // Start is called before the first frame update
    void Start()
    {
        thisStory = this;

        Menu_Selection.GetComponent<Animator>().SetTrigger("Opening");
        StartCoroutine(Transition_StoryMode(1));
    }

    IEnumerator Transition_StoryMode(int _index)
    {
        yield return new WaitForSeconds(0.05f);
        switch (_index)
        {
            case 2:
                SceneManager.LoadScene("Ref_PreSelection");
                break;

            case 3:
                yield return new WaitForSeconds(0.5f);
                mainChannel = false;
                Menu_Selection2.GetComponent<Animator>().SetTrigger("OpenStory");
                CheckNagivator2();
                break;

            case 4:
                yield return new WaitForSeconds(0.5f);
                mainChannel = true;
                Menu_Selection.GetComponent<Animator>().SetTrigger("Opening");
                break;

            case 5:
                StoryTool myTool = new StoryTool();
                myTool.StoryTxtExtraction(PlayerPrefs.GetString("StoryLine_Index", string.Empty));
                StartCoroutine(myTool.StoryTxtEffect());
                StartCoroutine(StoryTellerPlayer());
                break;

            default:
                myStoryList = Resources.LoadAll<StoryInfo>("Database_Story");
                CheckNavigator();
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (mainChannel)
            {
                case true:
                    Menu_Selection.GetComponent<Animator>().SetTrigger("Closing");
                    StartCoroutine(Transition_StoryMode(2));
                    break;

                case false:
                    Menu_Selection2.GetComponent<Animator>().SetTrigger("CloseStory");
                    StartCoroutine(Transition_StoryMode(4));
                    break;
            }
        }
    }

    // Main Channel: Nagivator
    void CheckNavigator()
    {
        NagivatorBar.maxValue = myStoryList.Length;
        NagivatorBar.value = selector;

        if (selector <= 1) { Left.GetComponent<Button>().interactable = false; }
        else { Left.GetComponent<Button>().interactable = true; }

        if (selector >= myStoryList.Length) { Right.GetComponent<Button>().interactable = false; }
        else { Right.GetComponent<Button>().interactable = true; }

        // Change Story Scene
        StoryBG.texture = myStoryList[selector - 1].StoryBG;
        StoryTitle.text = myStoryList[selector - 1].StoryTitle + "\n [ " + myStoryList[selector - 1].S_Type.ToString() + " Story ]";
        NagivatorBar.transform.GetChild(0).GetComponent<Text>().text = selector + "/" + myStoryList.Length;

        // Check Selection Button
        if (myStoryList[selector - 1].Stage.Length == 0) { ProcessBtn.GetComponent<Button>().interactable = false; }
        else { ProcessBtn.GetComponent<Button>().interactable = true; }
    }

    // Channel 2: Nagivator 
    void CheckNagivator2()
    {
        StoryBG2.texture = myStoryList[selector - 1].StoryBG;
        StoryTitle2.text = myStoryList[selector - 1].StoryTitle + "\n [ " + myStoryList[selector - 1].S_Type.ToString() + " Story ]";

        if (selector2 <= 1) { Left2.GetComponent<Button>().interactable = false; }
        else { Left2.GetComponent<Button>().interactable = true; }

        if (selector2 >= myStoryList[selector - 1].Stage.Length) { Right2.GetComponent<Button>().interactable = false; }
        else { Right2.GetComponent<Button>().interactable = true; }

        // Change Scenerio
        HeadTitle.text = "Episode " + selector2 + " : " + myStoryList[selector - 1].Stage[selector2 - 1].HeadTitle;
        FragmentAmt.text = "Fragment Gathered: " + "0 / " + myStoryList[selector - 1].Stage[selector2 - 1].numberOfSteps;

        // Check for Story Detail
        if (myStoryList[selector - 1].Stage[selector2 - 1].StoryDetail == "--") { BeginBtn.GetComponent<Button>().interactable = false; }
        else { BeginBtn.GetComponent<Button>().interactable = true; }

        CheckSlotOpening();
        CheckEpisodeStatus();
    }

    // Channel 2: Slot
    void CheckSlotOpening()
    {
        // Reset Visible
        for (int i = 0; i < 3; i++)
        {
            Slot[i].transform.GetChild(0).gameObject.SetActive(false);
            Slot[i].transform.GetChild(1).gameObject.SetActive(false);
            Slot[i].transform.GetChild(2).gameObject.SetActive(false);
        }

        for (int i = 0; i < 3; i++)
        {
            // Checking Open Slot: Display
            if (myStoryList[selector - 1].Stage[selector2 - 1].myQuestLog[i].slotOpen)
            {
                Slot[i].transform.GetChild(3).gameObject.SetActive(false);

                // Checking Slot Type: Display
                if (myStoryList[selector - 1].Stage[selector2 - 1].myQuestLog[i].clearSlot)
                {
                    Slot[i].transform.GetChild(2).gameObject.SetActive(true);
                }
                else
                {
                    switch (myStoryList[selector - 1].Stage[selector2 - 1].myQuestLog[i].mySlotTypte)
                    {
                        case SlotQuestLog.SlotType.Quest:
                            Slot[i].transform.GetChild(0).gameObject.SetActive(true);
                            break;

                        case SlotQuestLog.SlotType.Battle:
                            Slot[i].transform.GetChild(1).gameObject.SetActive(true);
                            break;
                    }
                }
            }
            else { Slot[i].transform.GetChild(3).gameObject.SetActive(true); }
        }
    }

    // Channel 2: Checking Status
    void CheckEpisodeStatus()
    {
        foreach (GameObject i in EpsiodeStatus) { i.SetActive(false); }
        BeginBtn.GetComponent<Button>().interactable = true;

        switch (myStoryList[selector - 1].Stage[selector2 - 1].myEpi_Manage)
        {
            case FragmentInfo.EpisodeManagement.Lock:
                EpsiodeStatus[0].SetActive(true);
                BeginBtn.GetComponent<Button>().interactable = false;
                break;

            case FragmentInfo.EpisodeManagement.Unlock:
                EpsiodeStatus[1].SetActive(true);
                break;

            case FragmentInfo.EpisodeManagement.Clear:
                EpsiodeStatus[2].SetActive(true);
                break;

            default:
                break;
        }
    }

    public void ClickedNaviator(int index)
    {
        switch (index)
        {
            case 1:
                selector++;
                break;
            case 2:
                selector--;
                break;
        }

        CheckNavigator();
    }

    public void ClickedNaviator2(int index)
    {
        switch (index)
        {
            case 1:
                selector2++;
                break;
            case 2:
                selector2--;
                break;
        }

        CheckNagivator2();
    }

    public void Selected_StoryTransition()
    {
        Menu_Selection.GetComponent<Animator>().SetTrigger("Closing");
        StartCoroutine(Transition_StoryMode(3));
    }

    // Channel 2: Begin Button Interactive
    public void BeginStoryMode()
    {
        BeginBtn.GetComponent<Button>().interactable = false;
        Menu_Selection3.GetComponent<Animator>().SetTrigger("Opening");

        PlayerPrefs.SetString("StoryLine_Index", myStoryList[selector - 1].Stage[selector2 - 1].StoryDetail);
        StartCoroutine(Transition_StoryMode(5));
    }

    // Channel 3: Continue Button Interactive
    public void ContinueBtn_StoryMode()
    {
        ContinueBtn.SetActive(false);
        Menu_Selection3.GetComponent<Animator>().SetTrigger("Closing");

        StoryDes2.GetComponent<Text>().text = string.Empty;
        Menu_Selection3.GetComponent<AudioSource>().clip = null;
        CheckNagivator2();

        // Unlock Episode
        myStoryList[selector - 1].Stage[selector2 - 1].myEpi_Manage = FragmentInfo.EpisodeManagement.NoStatus;
        CheckEpisodeStatus();
    }

    // Channel 3: PlugIn-Effect
    public void StoryDisplayTxt(char print) { StoryDes2.GetComponent<Text>().text += print; }
    public void StoryDisplayContinue() { ContinueBtn.SetActive(true); }

    // Channel 3: Story Teller
    public void StoryLoaderPlayer(string storyRef)
    {
        try
        {
            AudioClip StoryTeller = Resources.Load<AudioClip>("Story/" + storyRef);
            Menu_Selection3.GetComponent<AudioSource>().clip = StoryTeller;
        }
        catch { }
    }

    protected IEnumerator StoryTellerPlayer() 
    {
        yield return new WaitForSeconds(1);
        if (GameObject.Find("BGM")) { GameObject.Find("BGM").GetComponent<AudioSource>().volume = 0.2f; }
        if (Menu_Selection3.GetComponent<AudioSource>().clip != null) { Menu_Selection3.GetComponent<AudioSource>().Play(); }
       
        yield return new WaitUntil(() => !Menu_Selection3.GetComponent<AudioSource>().isPlaying);
        if (GameObject.Find("BGM")) { GameObject.Find("BGM").GetComponent<AudioSource>().volume = 1; }
    }

    // Channel 2: StoryMode Slot Interact
    public void StoryMode_SlotInteractive(int _ref)
    {
        if (!EpsiodeStatus[3].activeInHierarchy)
        {
            if (myStoryList[selector - 1].Stage[selector2 - 1].myQuestLog[_ref - 1].slotOpen)
            {
                switch (myStoryList[selector - 1].Stage[selector2 - 1].myQuestLog[_ref - 1].mySlotTypte)
                {
                    case SlotQuestLog.SlotType.Quest:
                        break;

                    case SlotQuestLog.SlotType.Battle:
                        break;
                }
            }
            else
            {
                EpsiodeStatus[3].SetActive(true);
            }
        }      
    }

    public void ContinueButton_SlotInteract() { EpsiodeStatus[3].SetActive(false); }
}