using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MeloMelo_RPGEditor;
using MeloMelo_ExtraComponent;
using UnityEngine.Video;

public class BattleSetup_Script : MonoBehaviour
{
    private GameObject[] BGM;

    private int[] speed = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
    private string[] speed_label = { "--", "x1.5", "x2", "x2.5", "x3", "x3.5", "x4", "x4.5", "x5", "x5.5", "x6", "SONIC" };

    private string[] scoreDisplay_label = { "Score", "Points", "Score & Points", "Score & CP", "Score & HiScore", "Score With Rank" };
    private string[] scoreDisplay2_label = { "OFF", "Points", "CP", "HiPoints", "HiScore", "Max Score", "Min Score", "Points V2" };
    private string[] autoRetreat_label = { "OFF", "CP +0", "BORDER/S", "BORDER/SS", "BORDER/X", "MY BEST SCORE" };
    private string[] bottomDisplay_label = { "Default", "JudgeTiming", "Nothing" };
    private string[] feedbackDisplay_label = { "ALL", "Perfect & below", "OFF" };
    private string[] feedbackDisplay2_label = { "Critical Included", "Standard", "Don't Include" };

    public GameObject[] DifficultyArea;

    private UnitFormation_Manage unitSlot = new UnitFormation_Manage();
    private QuickLook notice = new QuickLook();

    [Header("Quick-Look Component")]
    public GameObject[] GuideNotice = new GameObject[2];
    public GameObject[] ButtonUI_GN = new GameObject[2];
    private string ResMelo = string.Empty;

    [Header("Setup GUI: Component")]
    [SerializeField] private GameObject CharacterSetup_GUI;
    [SerializeField] private GameObject GameplaySetup_GUI;
    [SerializeField] private GameObject[] MainSetupOfGameplayGUI;

    [Header("MV: Component Setup")]
    [SerializeField] private Text MV_imported;
    [SerializeField] private Button MV_currentSwitch;
    [SerializeField] private Text MV_Status;

    [Header("NoteSpeed: Component Setup")]
    [SerializeField] private Text NoteSpeed_current;
    [SerializeField] private Slider NoteSpeed_currentSwitch;
    [SerializeField] private Text NoteSpeed_Status;

    [Header("ScoreDisplay: Component Setup")]
    [SerializeField] private Text ScoreDisplay_Front;
    [SerializeField] private Text ScoreDisplay_Side;
    [SerializeField] private GameObject[] ScoreDisplayPanel;

    [Header("AutoRetreat: Component Setup")]
    [SerializeField] private Text AutoRetreat_type;
    [SerializeField] private Text AutoRetreat_choice;
    [SerializeField] private GameObject[] AutoRetreatPanel;

    [Header("BottomDisplay: Component Setup")]
    [SerializeField] private Text BottomDisplay_type;
    [SerializeField] private Button BottomDisplay_PreviousBtn;
    [SerializeField] private Button BottomDisplay_NextBtn;

    [Header("FeedbackDisplay: Component Setup")]
    [SerializeField] private Text FeedbackDisplay_type;
    [SerializeField] private Text FeedbackDisplay2_type;
    [SerializeField] private GameObject[] FeedbackDisplay_Panel;

    [SerializeField] private GameObject SkillSlot_Panel;
    [SerializeField] private GameObject[] BoostPanel;
    [SerializeField] private GameObject BoostPanelTemplate;
    [SerializeField] private GameObject MessagePrompt;

    // Load All Database
    void Start()
    {
        ResMelo = PlayerPrefs.GetString("Resoultion_Melo", string.Empty);
        //StartCoroutine(GetVideoClipLoaded(SelectionMenu_Script.thisSelect.get_form.seasonNo, SelectionMenu_Script.thisSelect.get_form.Title));

        BGM = GameObject.FindGameObjectsWithTag("BGM");
        try { if (GameObject.Find("BGM").activeInHierarchy && BGM[0] != null) { DontDestroyOnLoad(BGM[0]); } }
        catch { Debug.Log("BGM Not Detected"); }
        if (BGM.Length > 1) { Destroy(BGM[1]); }

        DifficultyArea[PlayerPrefs.GetInt("BattleDifficulty_Mode", 1) - 1].GetComponent<RawImage>().enabled = true;

        if (PlayerPrefs.GetString("BattleSetup_Guide", "T") == "T")
        {
            notice.Opening_QuickLook("QuickLook");
            notice.NoticePlay(GuideNotice, ButtonUI_GN);
        }

        GameObject.Find("BG").GetComponent<RawImage>().texture =
            !PlayerPrefs.HasKey("MarathonPermit") ? PreSelection_Script.thisPre.get_AreaData.BG :
                Resources.Load<Texture>("Background/BG11");

        LoadGameplaySetup();
    }

    #region SETUP
    private void ReturnScene()
    {
        StartCoroutine(ClosingBattleSetup(((PlayerPrefs.HasKey("Mission_Played")) ? "BlackBoard" : "Music Selection Stage" + ResMelo)));
    }

    private void StartScene()
    {
        StartCoroutine(ClosingBattleSetup("Stage Transition" + ResMelo));
    }

    private void LoadCharacterSetup()
    {
        OpeningTransitionSetup(CharacterSetup_GUI, string.Empty);
        StartCoroutine(unitSlot.FinishedTransition_SelectUI());
    }

    private void LoadGameplaySetup()
    {
        OpeningTransitionSetup(GameplaySetup_GUI, string.Empty);

        MV_Option(false);
        IntiNoteSpeed();
        IntiAutoRetreat();
        IntiScoreDisplay();
        IntiBottomDisplay();
        IntiFeedbackDisplay();

        AssignSkillSlot();
        AddonsToExpBoost();
        AddonsToPowerBoost();
    }
    #endregion

    #region MAIN
    public void ReturnToSelectionTrack()
    {
        ClosingTransitionSetup(GameplaySetup_GUI, "ReturnScene");
    }

    public void ReturnToGamePlaySetup()
    {
        ClosingTransitionSetup(CharacterSetup_GUI, "LoadGameplaySetup");
    }

    public void CompleteGamePlaySetup()
    {
        ClosingTransitionSetup(GameplaySetup_GUI, "LoadCharacterSetup");
    }

    public void CompleteCharacterSetup()
    {
        ClosingTransitionSetup(CharacterSetup_GUI, "StartScene");
    }
    #endregion

    #region COMPONENT 
    private void OpeningTransitionSetup(GameObject target, string processOption)
    {
        target.GetComponent<Animator>().SetTrigger("Opening");
        if (processOption != string.Empty) Invoke(processOption, 1);
    }

    private void ClosingTransitionSetup(GameObject target, string processOption)
    {
        target.GetComponent<Animator>().SetTrigger("Closing");
        if (processOption != string.Empty) Invoke(processOption, 1);
    }

    private IEnumerator ClosingBattleSetup(string index)
    {
        yield return new WaitForSeconds(2);
        if (BGM.Length > 0) { Destroy(BGM[0]); }
        SceneManager.LoadScene(index);
    }
    #endregion

    #region COMPONENT (SETUP INTERACTION)
    // MV Option
    public void MV_Option(bool click)
    {
        bool condition = SelectionMenu_Script.thisSelect.get_selection.get_form.videoImport != null;
        MV_imported.text = condition ? "YES" : "NO";
        MV_Status.text = condition ? "Ready" : "N/A";
        MV_currentSwitch.interactable = condition;

        if (condition)
        {
            string mvOption = PlayerPrefs.GetString("MVOption", "T");

            if (click)
            {
                mvOption = mvOption == "T" ? "F" : "T";
                PlayerPrefs.SetString("MVOption", mvOption);
            }

            // Update UI Text
            switch (mvOption)
            {
                case "T":
                    MV_currentSwitch.transform.GetChild(0).GetComponent<Text>().text = "ON";
                    break;

                case "F":
                    MV_currentSwitch.transform.GetChild(0).GetComponent<Text>().text = "OFF";
                    break;
            }
        }
    }

    // Note Speed
    public void NoteSpeed_Option()
    {
        int speed = (int)NoteSpeed_currentSwitch.value;
        PlayerPrefs.SetInt("NoteSpeed", GetSpeedValueSet(speed));
        NoteSpeed_current.text = speed_label[GetSpeedValueSet(speed)];
    }

    private void IntiNoteSpeed()
    {
        NoteSpeed_currentSwitch.maxValue = speed_label.Length - 1;
        NoteSpeed_currentSwitch.value = PlayerPrefs.GetInt("NoteSpeed", 0);
        NoteSpeed_Status.text = GetNoteSpeedStatus();
        NoteSpeed_Option();
    }

    private string GetNoteSpeedStatus()
    {
        switch (SelectionMenu_Script.thisSelect.get_selection.get_form.seasonNo)
        {
            case 0:
            case 1:
                return "Fixed (Basic)";

            case 2:
                return "Fixed v2 (Normal)";

            default:
                NoteSpeed_currentSwitch.interactable = true;
                return "Custom (Details)";
        }
    }

    private int GetSpeedValueSet(int speed)
    {
        switch (SelectionMenu_Script.thisSelect.get_selection.get_form.seasonNo)
        {
            case 0:
            case 1:
                return PlayerPrefs.GetInt("NoteSpeed_Legacy", 0);

            case 2:
                return PlayerPrefs.GetInt("NoteSpeed_Legacy_v2", 1);

            default:
                return speed;
        }
    }

    // Score Display
    public void ScoreDisplay_Option()
    {
        foreach (GameObject main in MainSetupOfGameplayGUI) main.SetActive(false);
        foreach (GameObject panel in ScoreDisplayPanel) panel.SetActive(true);

        ScoreDisplayPanel[1].transform.GetChild(2).GetComponent<Text>().text = scoreDisplay_label[PlayerPrefs.GetInt("ScoreDisplay", 0)];
        ScoreDisplayPanel[0].transform.GetChild(2).GetComponent<Text>().text = scoreDisplay2_label[PlayerPrefs.GetInt("ScoreDisplay2", 0)];
    }

    private void IntiScoreDisplay()
    {
        ScoreDisplay_Front.text = scoreDisplay2_label[PlayerPrefs.GetInt("ScoreDisplay2", 0)];
        ScoreDisplay_Side.text = scoreDisplay_label[PlayerPrefs.GetInt("ScoreDisplay", 0)];
    }

    // Auto Retreat
    public void AutoRetreat_Option()
    {
        foreach (GameObject main in MainSetupOfGameplayGUI) main.SetActive(false);
        foreach (GameObject panel in AutoRetreatPanel) panel.SetActive(true);
    }

    private void IntiAutoRetreat()
    {
        AutoRetreat_type.text = autoRetreat_label[PlayerPrefs.GetInt("AutoRetreat", 0)];
        AutoRetreat_choice.text = "--";
    }

    public void CloseSubSettings(int index)
    {
        switch (index)
        {
            case 1:
                foreach (GameObject panel in ScoreDisplayPanel) panel.SetActive(false);
                IntiScoreDisplay();
                break;

            case 2:
                foreach (GameObject panel in AutoRetreatPanel) panel.SetActive(false);
                IntiAutoRetreat();
                break;

            case 3:
                foreach (GameObject panel in FeedbackDisplay_Panel) panel.SetActive(false);
                IntiFeedbackDisplay();
                break;
        }

        foreach (GameObject main in MainSetupOfGameplayGUI) main.SetActive(true);
        PlayerPrefs.SetInt("SettingConfiguration_Pending", 1);
    }

    // Bottom Display
    public void BottomDisplay_Customize(bool previous)
    {
        if (previous) PlayerPrefs.SetInt("JudgeMeter_Setup", PlayerPrefs.GetInt("JudgeMeter_Setup") - 1);
        else PlayerPrefs.SetInt("JudgeMeter_Setup", PlayerPrefs.GetInt("JudgeMeter_Setup") + 1);
        CheckForNagivatorOnBottomDisplay();

        BottomDisplay_type.text = bottomDisplay_label[PlayerPrefs.GetInt("JudgeMeter_Setup")];
    }

    private void IntiBottomDisplay()
    {
        BottomDisplay_type.text = bottomDisplay_label[PlayerPrefs.GetInt("JudgeMeter_Setup", 0)];
        CheckForNagivatorOnBottomDisplay();
    }

    private void CheckForNagivatorOnBottomDisplay()
    {
        BottomDisplay_PreviousBtn.interactable = PlayerPrefs.GetInt("JudgeMeter_Setup") > 0;
        BottomDisplay_NextBtn.interactable = PlayerPrefs.GetInt("JudgeMeter_Setup") < bottomDisplay_label.Length - 1;
    }

    // Feedback Display
    private void IntiFeedbackDisplay()
    {
        FeedbackDisplay_type.text = feedbackDisplay_label[PlayerPrefs.GetInt("Feedback_Display_Type_B", 0)];
        FeedbackDisplay2_type.text = feedbackDisplay2_label[PlayerPrefs.GetInt("Feedback_Display_Type", 1)];
    }

    public void FeedbackDisplay_Option()
    {
        foreach (GameObject main in MainSetupOfGameplayGUI) main.SetActive(false);
        foreach (GameObject panel in FeedbackDisplay_Panel) panel.SetActive(true);

        FeedbackDisplay_Panel[1].transform.GetChild(2).GetComponent<Text>().text = feedbackDisplay_label[PlayerPrefs.GetInt("Feedback_Display_Type_B", 0)];
        FeedbackDisplay_Panel[0].transform.GetChild(2).GetComponent<Text>().text = feedbackDisplay2_label[PlayerPrefs.GetInt("Feedback_Display_Type", 1)];
    }
    #endregion

    #region COMPONENT (CHARACTER CHECKLIST)
    // Character Selection
    public void AssignCharacterSlot(int id)
    {
        PlayerPrefs.SetInt("SlotSelect_setup", id);
        PlayerPrefs.SetString("SlotSelect_lastSelect", SceneManager.GetActiveScene().name);
        ClosingTransitionSetup(CharacterSetup_GUI, "TransitionInCharacterSelection");
    }

    private void TransitionInCharacterSelection() { SceneManager.LoadScene("Ref_CharacterSelection"); }

    // Skill Selection
    private void AssignSkillSlot()
    {
        SkillContainer autoSelectedSkill = Resources.Load<SkillContainer>("Database_Skills/" + PlayerPrefs.GetString("CharacterFront", "None") + "_Primary_Skill");
        bool isSkillActive = PlayerPrefs.GetString("Character_Active_Skill", "T") == "T" ? true : false;
        SkillSlot_Panel.transform.GetChild(2).GetComponent<Button>().interactable = autoSelectedSkill ? true : false;

        if (autoSelectedSkill)
        {
            SkillSlot_Panel.GetComponent<RawImage>().texture = autoSelectedSkill.skillIcon;
            SkillSlot_Panel.transform.GetChild(0).GetComponent<Text>().text = autoSelectedSkill.skillName;
            SkillSlot_Panel.transform.GetChild(1).GetComponent<Text>().text = autoSelectedSkill.description;
            SkillSlot_Panel.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = isSkillActive ? "ACTIVE" : "OFF";
        }
        else
            SkillSlot_Panel.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = "OFF";
    }

    public void ToggleSkillSlot()
    {
        string toggleResult = PlayerPrefs.GetString("Character_Active_Skill", "T");
        PlayerPrefs.SetString("Character_Active_Skill", toggleResult == "T" ? "F" : "T");
        AssignSkillSlot();
    }

    public void SaveComponentCharacterSettings()
    {
        MeloMelo_Local.LocalSave_DataManagement saveSetting = new MeloMelo_Local.LocalSave_DataManagement(
            LoginPage_Script.thisPage.GetUserPortOutput(),
            "StreamingAssets/LocalData/MeloMelo_LocalSave_InGameProgress"
            );

        saveSetting.SelectFileForActionWithUserTag(MeloMelo_GameSettings.GetLocalFileCharacterSettings);

        saveSetting.SaveFormationSettings(
            PlayerPrefs.GetString("Slot1_charName", "None"),
            PlayerPrefs.GetString("Slot2_charName", "None"),
            PlayerPrefs.GetString("Slot3_charName", "None"),
            PlayerPrefs.GetString("CharacterFront", "None")
            );
    }

    // Boost Panel
    #region BOOST PANEL:
    #region SETUP
    private void AddonsToExpBoost()
    {
        GameObject panel = GameObject.Find("ExpBoostSlot");
        if (panel)
        {
            panel.transform.GetChild(1).GetComponent<Text>().text = MeloMelo_ItemUsage_Settings.GetAllyExpBoost() > 0 ? "x " +
                MeloMelo_ItemUsage_Settings.GetAllyExpBoost() : "- None -";

            panel.transform.GetChild(2).GetComponent<Button>().interactable = MeloMelo_ItemUsage_Settings.GetAllyExpBoost() == 0;
        }
    }

    private void AddonsToPowerBoost()
    {
        GameObject panel = GameObject.Find("PowerBoostSlot");
        if (panel)
        {
            panel.transform.GetChild(1).GetComponent<Text>().text = MeloMelo_ItemUsage_Settings.GetAllyPowerBoost() > 0 ? "+ " +
                MeloMelo_ItemUsage_Settings.GetAllyPowerBoost() : "- None -";

            panel.transform.GetChild(2).GetComponent<Button>().interactable = MeloMelo_ItemUsage_Settings.GetAllyPowerBoost() == 0;
        }
    }

    private int GetTotalPotionCount(string title)
    {
        // Potion counted in storage bag and current used items
        int potionAmount = MeloMelo_GameSettings.GetAllItemFromLocal(title).amount -
            MeloMelo_ItemUsage_Settings.GetItemUsed(title);

        return potionAmount;
    }
    #endregion

    #region MAIN
    public void GetBoostPanel(string parameterData)
    {
        // Parameter: Panel, Display_id
        string[] option = parameterData.Split(",");
        BoostPanel[int.Parse(option[0])].SetActive(!PlayerPrefs.HasKey("MarathonPermit") && (option[1] == "1" ? true : false));
    }

    public void DisplayPoitionInfo(string parameterData)
    {
        // Parameter: Title, Display_id
        string[] option = parameterData.Split(",");
        MessagePrompt.SetActive(option[1] == "1" ? true : false);
        MessagePrompt.transform.GetChild(0).GetComponent<Text>().text = option[0] + " ( x " + Mathf.Clamp(GetTotalPotionCount(option[0]), 0, 9999) + " )";
    }

    public void ActivationOfExpBoost(string parameterData)
    {
        // Parameter: Panel, title
        string[] option = parameterData.Split(",");

        if (PlayerPrefs.GetString("AllyExpBoost_Bound", string.Empty) == option[1])
        {
            // Check for exp potion and update amount uses
            VirtualItemDatabase expPotion = MeloMelo_GameSettings.GetAllItemFromLocal(option[1]);
            if (expPotion.itemName == option[1] && GetTotalPotionCount(option[1]) > 0)
            {
                PlayerPrefs.DeleteKey("TicketUsage_Bound");
                MeloMelo_ItemUsage_Settings.SetItemUsed(expPotion.itemName);
                MeloMelo_ItemUsage_Settings.SetAllyExpBoost(int.Parse(option[1].Split("X")[0]));

                AddonsToExpBoost();
                BoostPanel[int.Parse(option[0])].SetActive(false);
                DisplayPoitionInfo(option[1] + ",0");
            }
        }
        else PlayerPrefs.SetString("AllyExpBoost_Bound", option[1]);
    }

    public void ActivationOfPowerBoost(string parameterData)
    {
        // Parameter: Panel, title, opertaorType, amount
        string[] option = parameterData.Split(",");

        if (PlayerPrefs.GetString("AllyPowerBoost_Bound", string.Empty) == option[1])
        {
            // Check for power potion and update amount uses
            VirtualItemDatabase powerPotion = MeloMelo_GameSettings.GetAllItemFromLocal(option[1]);
            if (powerPotion.itemName == option[1] && GetTotalPotionCount(option[1]) > 0)
            {
                PlayerPrefs.DeleteKey("AllyPowerBoost_Bound");
                MeloMelo_ItemUsage_Settings.SetItemUsed(powerPotion.itemName);
                MeloMelo_ItemUsage_Settings.SetAllyPowerBoost(option[2] == "A" ? int.Parse(option[3]) :
                    GetPowerUnitWithMultipleBoost(int.Parse(option[3])));

                AddonsToPowerBoost();
                BoostPanel[int.Parse(option[0])].SetActive(false);
                DisplayPoitionInfo(option[1] + ",0");
            }
        }
        else PlayerPrefs.SetString("AllyPowerBoost_Bound", option[1]);
    }

    public void DragInterfaceComponent(int index)
    {
        BoostPanel[index].transform.position = new Vector3(Mathf.Clamp(Input.mousePosition.x, BoostPanel[index].transform.localScale.x * 0.5f, 
            Screen.width - (BoostPanel[index].transform.localScale.x * 0.5f)),
                Mathf.Clamp(Input.mousePosition.y, BoostPanel[index].transform.localScale.y * 0.5f, 
                    Screen.height - (BoostPanel[index].transform.localScale.y * 0.5f)), 0);
    }

    private int GetPowerUnitWithMultipleBoost(int multiple)
    {
        int totalUnitValue = 0;
        StatsDistribution units = new StatsDistribution();
        units.load_Stats();

        foreach (ClassBase unit in units.slot_Stats)
        {
            if (PlayerPrefs.GetString("CharacterFront", "NA") == unit.name)
            {
                totalUnitValue += unit.strength;
                totalUnitValue += unit.vitality;
                totalUnitValue += unit.magic;
                break;
            }
        }

        return totalUnitValue * multiple;
    }
    #endregion
    #endregion

    #region BOOST PANEL 2:
    public void GetInstancePanel(string parameterData)
    {
        string[] option = parameterData.Split(",");

        if (GameObject.Find(option[0]) == null)
        {
            GameObject instance_panel = Instantiate(BoostPanelTemplate);
            instance_panel.name = option[0];

            instance_panel.GetComponent<VirtualStorageBag>().SetAlertPopReference(MessagePrompt);
            instance_panel.GetComponent<VirtualStorageBag>().SetDefaultDescription(option[1]);
            instance_panel.GetComponent<VirtualStorageBag>().SetItemForDisplay(GetItemArray(option[0]));
            instance_panel.transform.SetParent(CharacterSetup_GUI.transform);
        }
    }

    private VirtualItemDatabase[] GetItemArray(string panelType)
    {
        List<VirtualItemDatabase> listOfItem;

        switch (panelType)
        {
            case "EXP_Boost_Panel":
                listOfItem = new List<VirtualItemDatabase>();
                string[] filteredItem_EXP = { "2X EXP POTION", "5X EXP POTION", "10X EXP POTION" };
                foreach (string item in filteredItem_EXP)
                {
                    VirtualItemDatabase itemFound = MeloMelo_GameSettings.GetAllItemFromLocal(item);
                    if (itemFound.amount > 0) listOfItem.Add(itemFound);
                }
                return listOfItem.ToArray();

            case "Power_Boost_Panel":
                listOfItem = new List<VirtualItemDatabase>();
                string[] filteredItem_POWER = { "+5 POWER POTION", "+10 POWER POTION", "2X POWER POTION" };
                foreach (string item in filteredItem_POWER)
                {
                    VirtualItemDatabase itemFound = MeloMelo_GameSettings.GetAllItemFromLocal(item);
                    if (itemFound.amount > 0) listOfItem.Add(itemFound);
                }
                return listOfItem.ToArray();
        }

        return null;
    }
    #endregion
    #endregion

    #region MISC (MV)
    private IEnumerator GetVideoClipLoaded(int season, string clipId)
    {
        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync("Assets/StreamingAssets/mv_s" + season);
        yield return request;

        AssetBundle videoBundle = request.assetBundle;

        if (videoBundle)
        {
            AssetBundleRequest grant = videoBundle.LoadAssetAsync(clipId);
            yield return grant;
            SelectionMenu_Script.thisSelect.get_selection.get_form.videoImport = grant.asset as VideoClip;
            Debug.Log("Found:" + clipId);
        }

        Debug.Log("Finished:" + clipId);
        yield return null;
    }
    #endregion

    #region MISC (Instruction Guide)
    // UI_Input: Notice Play
    public void NextFunction_NoticePlay(bool next)
    {
        if (next)
            notice.NextFunction_NoticePlay(GuideNotice, ButtonUI_GN);
        else notice.CloseFunction_NoticePlay("BattleSetup_Guide");
    }
    #endregion

    #region COMPONENT (SCORE DISPLAY PANEL)
    public void ScoreDisplay_SearchNagivator(bool reserve)
    {
        // Find value of this nagivator
        ScoreDisplayUniversal_Nagivator("ScoreDisplay", 1, reserve, ScoreDisplayPanel, scoreDisplay_label, scoreDisplay2_label);
    }

    public void ScoreDisplay2_SearchNagivator(bool reserve)
    {
        // Find value of this nagivator
        ScoreDisplayUniversal_Nagivator("ScoreDisplay2", 0, reserve, ScoreDisplayPanel, scoreDisplay_label, scoreDisplay2_label);
    }

    public void FeedbackDisplay_Main_SearchNagivator(bool reserve)
    {
        // Find value of this nagivator
        ScoreDisplayUniversal_Nagivator("Feedback_Display_Type_B", 1, reserve, FeedbackDisplay_Panel, feedbackDisplay_label, feedbackDisplay2_label);
    }

    public void FeedbackDisplay_Side_SearchNagivator(bool reserve)
    {
        // Find value of this nagivator
        ScoreDisplayUniversal_Nagivator("Feedback_Display_Type", 0, reserve, FeedbackDisplay_Panel, feedbackDisplay_label, feedbackDisplay2_label);
    }

    private void ScoreDisplayUniversal_Nagivator(string content, int index, bool reserve, GameObject[] panel, string[] option1, string[] option2)
    {
        // Get current available data and selection
        int selection = PlayerPrefs.GetInt(content, 0);

        switch (reserve)
        {
            case true:
                if (selection > 0) selection--;
                else selection = 0;
                break;

            default:
                int length = (index == 1 ? option1.Length : option2.Length) - 1;
                if (selection < length) selection++;
                else selection = length;
                break;
        }

        // Check for previous selection bound
        panel[index].transform.GetChild(3).GetComponent<Button>().interactable = selection > 0;

        // Check for next selection bound
        panel[index].transform.GetChild(4).GetComponent<Button>().interactable = selection <
                (index == 1 ? option1.Length : option2.Length) - 1;

        // Update display text to user
        panel[index].transform.GetChild(2).GetComponent<Text>().text = 
            index == 1 ? option1[selection] : option2[selection];

        // Store local current selection
        PlayerPrefs.SetInt(content, selection);
    }

    public void SaveComponentForScoreDisplay()
    {
        MeloMelo_Local.LocalSave_DataManagement saveSetting = new MeloMelo_Local.LocalSave_DataManagement(
            LoginPage_Script.thisPage.GetUserPortOutput(),
            "StreamingAssets/LocalData/MeloMelo_LocalSave_InGameProgress"
            );

        saveSetting.SelectFileForActionWithUserTag(MeloMelo_GameSettings.GetLocalFileGameplaySettings);

        saveSetting.SaveGameplaySettings(
            PlayerPrefs.GetString("MVOption", "T") == "T" ? true : false,
            PlayerPrefs.GetInt("NoteSpeed", 20), false,
            PlayerPrefs.GetInt(LoginPage_Script.thisPage.GetUserPortOutput() + "totalRatePoint", 0)
            );
    }

    public void SaveComponentForFeedbackDisplay()
    {
        SaveComponentForScoreDisplay();
    }
    #endregion

    #region COMPONENT (AUTO RETREAT PANEL)
    public void SaveComponentForAutoRetreat()
    {
       
    }
    #endregion
}