using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSelectionPage : MonoBehaviour
{
    [Header("Load: Music Detail")]
    [SerializeField] private MusicScore templateForm;
    private MusicScore MusicForm = null;
    public MusicScore get_form { get { return MusicForm; } }

    [Header("Init Setup: Detail Component")]
    [SerializeField] private GameObject MusicInformation_txt;
    [SerializeField] private RawImage CoverImageDisplay;
    [SerializeField] private RawImage MaxOutScoreTag;
    [SerializeField] private Slider ScrollNagivator_ProgressBar;
    [SerializeField] private Text ScrollNagivator_Text;
    [SerializeField] private Button[] Nav_Selector;

    [Header("Play Setup: Detail Component")]
    [SerializeField] private GameObject DifficultyDisplay;

    [SerializeField] private GameObject[] UserDetailFeedback;
    private enum UserDetailFeebackOrder { NewChartEntry, ExistingEntry, RestrictedContent, ContentLocked, LevelDetailContent };

    [Header("Init Setup: Result Details")]
    [SerializeField] private Text SkillLevelValue;
    [SerializeField] private Text RemarkIcon;
    [SerializeField] private Text RemarkIcon2;
    [SerializeField] private Text BattleBtn_text;

    [Header("Play Setup: User Toggle")]
    [SerializeField] private GameObject difficulty_valve;
    public GameObject get_difficulty_valve { get { return difficulty_valve; } }

    private enum BattleBtnPrompt { Process, ViewInfo, ContentLocked };

    private float[] difficulyValue = new float[3];
    private enum DifficultyValueIndex { Normal, Hard, Ultimate };

    public float get_normal { get { return difficulyValue[0]; } }
    public float get_hard { get { return difficulyValue[1]; } }
    public float get_ultimate { get { return difficulyValue[2]; } }

    public Slider get_ScrollNagivator_ProgressBar { get { return ScrollNagivator_ProgressBar; } }

    [Header("Extra Setup: Tag Arrangement")]
    [SerializeField] private GameObject NewReleaseTag;
    [SerializeField] private GameObject AreaBonusTag;
    [SerializeField] private GameObject ScoreTag;
    //private GameObject LoadingScreen = null;

    public IEnumerator Setup_Page()
    {
        yield return new WaitForSeconds(0.1f);

        ScrollNagivatorSettings
            (
                // Total Selection
                PlayerPrefs.HasKey("Mission_Played") ? 1 :
                PlayerPrefs.HasKey("MarathonPermit") && Resources.Load<MarathonInfo>(PlayerPrefs.GetString("Marathon_Assigned_Task", string.Empty)) != null ?
                        Resources.Load<MarathonInfo>(PlayerPrefs.GetString("Marathon_Assigned_Task", string.Empty)).Difficultylevel.Length :
                PlayerPrefs.HasKey("MarathonPermit") ? 
                        MeloMelo_ExtensionContent_Settings.LoadMarathonDetail(PlayerPrefs.GetInt("MarathonInstanceNumber", 0)).track_difficulty.Length :
                        PreSelection_Script.thisPre.get_AreaData.totalMusic
                ,

                // Current Selection
                PlayerPrefs.HasKey("Mission_Played") ? 1 : 
                    PlayerPrefs.HasKey("MarathonPermit") ? PlayerPrefs.GetInt("MarathonChallenge_MCount") :
                        PlayerPrefs.GetInt("LastSelection", 1)
            );

        yield return StartCoroutine(RefreshMusicInformationPanel
            (
                // Area Assigned
                PlayerPrefs.HasKey("Mission_Played") ? string.Empty :
                PlayerPrefs.HasKey("MarathonPermit") ?
                        PlayerPrefs.GetString("Marathon_Assigned_Area_" + (PlayerPrefs.GetInt("MarathonChallenge_MCount") - 1), string.Empty)
                        :
                        "Database_Area/" + PreSelection_Script.thisPre.get_AreaData.AreaName
                ,

                // Is play casual?
                PlayerPrefs.HasKey("Mission_Played") ? true : !PlayerPrefs.HasKey("MarathonPermit")
            ));       

        // Get checkpoint instead of nagivator
        foreach (Button navigator in Nav_Selector)
            navigator.gameObject.SetActive(!PlayerPrefs.HasKey("MarathonPermit"));
    }

    #region SETUP
    private void ScrollNagivatorSettings(int totalTrack, int currentPick)
    {
        if (ScrollNagivator_ProgressBar)
        {
            ScrollNagivator_ProgressBar.minValue = 1;
            ScrollNagivator_ProgressBar.maxValue = totalTrack;
            try { ScrollNagivator_ProgressBar.value = currentPick; } catch { Debug.LogError(ScrollNagivator_ProgressBar.value = currentPick); }

            if (ScrollNagivator_Text)
                ScrollNagivator_Text.text = ScrollNagivator_ProgressBar.value + "/" + ScrollNagivator_ProgressBar.maxValue;
            else
                Debug.Log("ScrollNagivatorSettings: Text not found!");
        }
        else
            Debug.Log("ScrollNagivatorSettings: Progress Bar not found!");
    }
    #endregion

    #region COMPONENT
    private IEnumerator RefreshMusicInformationPanel(string areaLocated, bool casualMode)
    {
        //if (LoadingScreen == null)
        //{
        //    LoadingScreen = Instantiate(Resources.Load<GameObject>("Prefabs/LoadingUI"), transform);
        //    LoadingScreen.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        //}

        // Load music database
        //LoadingScreen.GetComponent<LoadingContent_Script>().NowLoading("Loading track content...\n Just a moment");
        string formFiller = PlayerPrefs.HasKey("Mission_Played") ? string.Empty : (areaLocated + (casualMode ? "/M" + ReservePickMode((int)ScrollNagivator_ProgressBar.value) : string.Empty));

        if (PlayerPrefs.HasKey("Mission_Played")) MusicForm = StoryMode_Scripts.thisStory.missionTrack;
        else
        {
            foreach (Button navigator in Nav_Selector) navigator.interactable = false;
            ResourceRequest musicOnLoading = Resources.LoadAsync<MusicScore>(formFiller);

            yield return new WaitUntil(() => musicOnLoading.isDone);
            MusicForm = musicOnLoading.asset as MusicScore;
        }

        // Load bgm preSet options
        SelectionMenu_Script.thisSelect.get_BGM.GetComponent<BGM_MusicPlayer>().UpdateTrackDetails(MusicForm.Music, MusicForm.PreviewTime, (int)ScrollNagivator_ProgressBar.value);
        GetNavAvaialbleToggle();

        // Clear existing score sheet
        RemovePreviousScoreSheet();

        // Load music content text
        CoverImageDisplay.texture = GetTrackCover(MusicForm.Background_Cover);
        MusicInformation_txt.transform.GetChild(0).GetComponent<Text>().text = GetTrackArtistName(MusicForm.ArtistName);
        MusicInformation_txt.transform.GetChild(1).GetComponent<Text>().text = GetTrackTitleName(MusicForm.Title);
        MusicInformation_txt.transform.GetChild(2).GetComponent<Text>().text = GetTrackBPM(
            MusicForm.BPM,
            MusicForm.BPM_Skin,
            MusicForm.UseBPMDisplay
            );

        MusicInformation_txt.transform.GetChild(3).GetComponent<Text>().text = LevelDesignerAutoFilled(MusicForm.DesignerName);
        ToogleAchievementTab(true);

        // Track Assigned Tag
        NewReleaseTag.SetActive(MusicForm.ScoreObject != null && PlayerPrefs.HasKey(MusicForm.Title + "_newReleaseTrack"));
        AreaBonusTag.SetActive(MusicForm.ScoreObject != null && PlayerPrefs.HasKey(MusicForm.Title + "_areaBonusTrack"));

        // Load new score sheet
        LoadNewScoreSheet();
        //LoadingScreen.GetComponent<LoadingContent_Script>().DoneLoading();
    }

    private IEnumerator SwitchChartTagMode()
    {
        yield return new WaitForSeconds(0.5f);

        // Score Sheet Tag (Legacy, Modern, New)
        for (int tag_id = 0; tag_id < ScoreTag.transform.childCount; tag_id++)
            ScoreTag.transform.GetChild(tag_id).gameObject.SetActive(false);

        int difficulty = MeloMelo_GameSettings.GetTrackDifficultyMode();

        if (MusicForm.timingAddons != null && MusicForm.timingAddons.Length > 0)
        {
            foreach (NewTimingAddons addons in MusicForm.timingAddons)
            {
                if (difficulty == addons.difficulty_index)
                {
                    if (addons.active)
                    {
                        int setVisibleTag = MusicForm.NewChartSystem ? 2 : 0;
                        ScoreTag.transform.GetChild(setVisibleTag).gameObject.SetActive(true);
                        break;
                    }

                    else
                    {
                        int setVisibleTag = MusicForm.NewChartSystem ? 1 : 0;
                        ScoreTag.transform.GetChild(setVisibleTag).gameObject.SetActive(true);
                        break;
                    }
                }
            }
        }
        else
        {
            int setVisibleTag = MusicForm.NewChartSystem ? 1 : 0;
            ScoreTag.transform.GetChild(setVisibleTag).gameObject.SetActive(true);
        }
    }
    #endregion

    #region MAIN
    public void NavOverList(bool reverse)
    {
        // Toogle over previous and next selection
        ScrollNagivator_ProgressBar.value += reverse ? -1 : 1;
        //GetNavAvaialbleToggle();
    }

    public void ModifyOfMusicListChange()
    {
        ScrollNagivator_Text.text = ScrollNagivator_ProgressBar.value + "/" + ScrollNagivator_ProgressBar.maxValue;
        StartCoroutine(RefreshMusicInformationPanel("Database_Area/" + PreSelection_Script.thisPre.get_AreaData.AreaName, true));
    }

    private void GetNavAvaialbleToggle()
    {
        // Check for previous selection
        Nav_Selector[0].interactable = ScrollNagivator_ProgressBar.value > ScrollNagivator_ProgressBar.minValue;

        // Check for next selection
        Nav_Selector[1].interactable = ScrollNagivator_ProgressBar.value < ScrollNagivator_ProgressBar.maxValue;
    }
    #endregion

    #region MISC
    private void RemovePreviousScoreSheet()
    {
        // Load score object to finalize level info
        if (GameObject.FindGameObjectWithTag("ScoreSheet"))
        {
            GameObject scoreSheet = GameObject.FindGameObjectWithTag("ScoreSheet");
            if (scoreSheet.activeInHierarchy) { Destroy(scoreSheet); }
        }
    }

    private void LoadNewScoreSheet()
    {
        if (MusicForm.ScoreObject != null)
        {
            Instantiate(MusicForm.ScoreObject, transform.position, Quaternion.identity);
        }
        else
        {
            GameObject.FindGameObjectWithTag("DifficultyDisplaySelection").transform.GetChild(0).GetComponent<Text>().text = "?";
            CloseOutDisplayDifficulty();

            LoadUserFeedContent(UserDetailFeebackOrder.ContentLocked);
        }

        Invoke("PrintOutDisplayDifficulty", 0.08f);
        SelectionMenu_Script.thisSelect.Invoke("DifficultyChanger_encode", 0.06f);
    }

    private void PrintOutDisplayDifficulty()
    {
        // Find difficulty available for play
        if (MusicForm.ScoreObject != null)
        {
            string[] difficultyState = { "Difficulty_Normal_selectionTxt", "Difficulty_Hard_selectionTxt", "Difficulty_Ultimate_selectionTxt" };

            for (int i = 0; i < difficultyState.Length; i++)
            {
                // Use it for marathon and casual to find any quick difficulty level display
                if ((!PlayerPrefs.HasKey("MarathonPermit") && PlayerPrefs.GetString(difficultyState[i], "?") != "0") || 
                    (PlayerPrefs.HasKey("MarathonPermit") && i == MeloMelo_GameSettings.GetTrackDifficultyMode() - 1))
                {
                    DifficultyDisplay.transform.GetChild(i).gameObject.SetActive(true);
                    DifficultyDisplay.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetString(difficultyState[i], "?");
                }
                else
                    DifficultyDisplay.transform.GetChild(i).gameObject.SetActive(false);
            }

            // Ulimate Addons
            if (!MusicForm.UltimateAddons) DifficultyDisplay.transform.GetChild(2).gameObject.SetActive(false);
        }

        // None
        DifficultyDisplay.transform.GetChild(3).gameObject.SetActive(MusicForm.ScoreObject == null);
    }

    private void CloseOutDisplayDifficulty()
    {
        string closeOutText = "None";

        for (int i = 0; i < DifficultyDisplay.transform.childCount; i++)
            if (DifficultyDisplay.transform.GetChild(i).name != closeOutText)
                DifficultyDisplay.transform.GetChild(i).gameObject.SetActive(false);

        // Change attribute
        ModifyOfBattleButtonText(BattleBtnPrompt.ContentLocked);
    }

    private void ModifyOfBattleButtonText(BattleBtnPrompt prompt)
    {
        switch (prompt)
        {
            case BattleBtnPrompt.Process:
                BattleBtn_text.text = "PROCESS";
                break;

            case BattleBtnPrompt.ViewInfo:
                BattleBtn_text.text = "INSPECT SETUP";
                break;

            case BattleBtnPrompt.ContentLocked:
                BattleBtn_text.text = "HOW TO GET";
                break;
        }
    }

    private void LoadUserFeedContent(UserDetailFeebackOrder select)
    {
        // Find active content
        for (int info = 0; info < UserDetailFeedback.Length; info++) UserDetailFeedback[info].SetActive(false);

        // Load active content
        UserDetailFeedback[(int)select].SetActive(true);
    }
    #endregion

    #region MISC (Selection)
    private void OnClickEvent_DifficultyChanger(DifficultyValueIndex index)
    {
        // Gerenal Setup
        GameObject selection = GameObject.FindGameObjectWithTag("DifficultyDisplaySelection");
        Color thisColor = new Color(0, 0, 0);
        float maxDifficult = GetMaxOutDifficultyLevel(index);

        difficulty_valve.name = GetDifficultySettings(index);

        // Check logical of exceed difficulty level
        if (difficulyValue[(int)index] >= 16)
        {
            thisColor = GetDifficultyColorBorder(index, maxDifficult);
            selection.transform.GetChild(1).GetComponent<Text>().text = "BEYOND " + difficulty_valve.name;
        }
        else if (difficulyValue[(int)index] >= maxDifficult)
        {
            thisColor = GetDifficultyColorBorder(index, maxDifficult);
            selection.transform.GetChild(1).GetComponent<Text>().text = difficulty_valve.name + "+";
        }
        else
        {
            thisColor = GetDifficultyColorBorder(index, maxDifficult);
            selection.transform.GetChild(1).GetComponent<Text>().text = difficulty_valve.name.ToString();
        }

        // Toggle over difficulty
        PlayerPrefs.SetInt("DifficultyLevel_valve", (int)index + 1);

        // Display off difficulty level
        selection.transform.GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetString(GetDifficultyDataDetail(index), "?");

        // Update GUI of color border and skill level
        selection.GetComponent<RawImage>().color = thisColor;
        SkillLevelValue.text = MusicForm.ScaleLevel + "/" + templateForm.ScaleLevel;

        StartCoroutine(SwitchChartTagMode());

        // New Features: Added RPG Element
        GetAreaLeveling(MusicForm.Title);
        GetRPGElement_TrackBattleInfo(MusicForm.Title);
        GetMaxScoreTag(MusicForm.Title);
    }

    private void OnCheckEvent_DifficultyChanger(DifficultyValueIndex current)
    {
        difficulty_valve.name = GetDifficultySettings(current);

        // General Setup
        GameObject selection = GameObject.FindGameObjectWithTag("DifficultyDisplaySelection");
        Color thisColor = new Color(0, 0, 0);
        float maxOut = GetMaxOutDifficultyLevel(current);

        // Get current selection and color border
        thisColor = GetDifficultyColorBorder(current, maxOut);
        PlayerPrefs.SetInt("DifficultyLevel_valve", (int)current + 1);

        // Check logical of exceed difficulty level
        if (difficulyValue[(int)current] >= maxOut) { selection.transform.GetChild(1).GetComponent<Text>().text = difficulty_valve.name + "+"; }
        else { selection.transform.GetChild(1).GetComponent<Text>().text = difficulty_valve.name.ToString(); }

        // Display off difficulty level
        selection.transform.GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetString(GetDifficultyDataDetail(current), "?");

        // Update GUI of color border and skill level
        selection.GetComponent<RawImage>().color = thisColor;
        SkillLevelValue.text = MusicForm.ScaleLevel + "/" + templateForm.ScaleLevel;

        StartCoroutine(SwitchChartTagMode());

        // New Features: Added RPG Element
        GetAreaLeveling(MusicForm.Title);
        GetRPGElement_TrackBattleInfo(MusicForm.Title);
        GetMaxScoreTag(MusicForm.Title);
    }

    private string GetDifficultySettings(DifficultyValueIndex setting)
    {
        switch (setting)
        {
            case DifficultyValueIndex.Normal:
                return "NORMAL";

            case DifficultyValueIndex.Hard:
                return "HARD";

            case DifficultyValueIndex.Ultimate:
                return "ULTIMATE";
        }

        return "???";
    }
    private Color GetDifficultyColorBorder(DifficultyValueIndex difficulty, float max)
    {
        switch (difficulty)
        {
            case DifficultyValueIndex.Normal:
                return Color.blue;

            case DifficultyValueIndex.Hard:
                if (difficulyValue[(int)DifficultyValueIndex.Hard] >= 16) return new Color(1, 0.09f, 0.87f);
                else if (difficulyValue[(int)DifficultyValueIndex.Hard] >= max) return new Color(0.47f, 0, 1);
                else return Color.red;

            case DifficultyValueIndex.Ultimate:
                return new Color(1, 0.4f, 0);

            default:
                return Color.black;
        }
    }

    private string GetDifficultyDataDetail(DifficultyValueIndex relay)
    {
        switch (relay)
        {
            case DifficultyValueIndex.Normal:
                return "Difficulty_Normal_selectionTxt";

            case DifficultyValueIndex.Hard:
                return "Difficulty_Hard_selectionTxt";

            case DifficultyValueIndex.Ultimate:
                return "Difficulty_Ultimate_selectionTxt";

            default:
                return "???";
        }
    }

    private int GetMaxOutDifficultyLevel(DifficultyValueIndex mode)
    {
        switch (mode)
        {
            case DifficultyValueIndex.Normal:
                return 6;

            case DifficultyValueIndex.Hard:
                return 11;

            default:
                return 16;
        }
    }

    public void DifficultyChanger(bool onClick)
    {
        // Find content is mapped
        if (MusicForm.ScoreObject != null)
        {
            // Toggle between all available difficulty
            switch (MeloMelo_GameSettings.GetTrackDifficultyMode())
            {
                case 1:
                    if (onClick && !PlayerPrefs.HasKey("MarathonPermit")) OnClickEvent_DifficultyChanger(DifficultyValueIndex.Hard);
                    else OnCheckEvent_DifficultyChanger(DifficultyValueIndex.Normal);
                    break;

                case 2:
                    if (onClick && !PlayerPrefs.HasKey("MarathonPermit"))
                    {
                        if (MusicForm.UltimateAddons) OnClickEvent_DifficultyChanger(DifficultyValueIndex.Ultimate);
                        else OnClickEvent_DifficultyChanger(DifficultyValueIndex.Normal);
                    }
                    else
                    {
                        OnCheckEvent_DifficultyChanger(DifficultyValueIndex.Hard);
                    }
                    break;

                case 3:
                    if (onClick && !PlayerPrefs.HasKey("MarathonPermit")) OnClickEvent_DifficultyChanger(DifficultyValueIndex.Normal);
                    else
                    {
                        if (MusicForm.UltimateAddons)
                        {
                            difficulty_valve.name = "ULTIMATE";
                            OnCheckEvent_DifficultyChanger(DifficultyValueIndex.Ultimate);
                        }
                        else
                        {
                            difficulty_valve.name = "HARD";
                            OnCheckEvent_DifficultyChanger(DifficultyValueIndex.Hard);
                        }
                    }
                    break;
            }

            // Change attribute
            if (!UserDetailFeedback[(int)UserDetailFeebackOrder.ContentLocked].activeInHierarchy)
                ModifyOfBattleButtonText(BattleBtnPrompt.Process);

            // Show Achievement Status
            CheckForUnlockableTrack(MeloMelo_GameSettings.GetTrackDifficultyMode());

            //CancelInvoke("LoadAndWriteBestRecord");
            //Invoke("LoadAndWriteBestRecord", 3);

            if (PlayerPrefs.GetInt("Marathon_Challenge", 0) == 0)
            {
                if (PlayerPrefs.GetInt(MusicForm.Title + "_BattleRemark_" + MeloMelo_GameSettings.GetTrackDifficultyMode(), 6) == 5)
                { RemarkIcon2.text = "FAILED!"; RemarkIcon2.color = Color.red; }

                else if (PlayerPrefs.GetString(MusicForm.Title + "_SuccessBattle_" + MeloMelo_GameSettings.GetTrackDifficultyMode() + MeloMelo_GameSettings.GetAreaDifficultyMode(), "F") == "T")
                { RemarkIcon2.text = "SUCCESS!"; RemarkIcon2.color = Color.green; }
                else { RemarkIcon2.text = "DRAW!"; RemarkIcon2.color = Color.grey; }
            }
        }
        else
        {
            GameObject.FindGameObjectWithTag("DifficultyDisplaySelection").transform.GetChild(1).GetComponent<Text>().text = "???";
            GameObject.FindGameObjectWithTag("DifficultyDisplaySelection").transform.GetChild(0).GetComponent<Text>().text = "?";
            GameObject.FindGameObjectWithTag("DifficultyDisplaySelection").GetComponent<RawImage>().color = Color.black;
        }
    }

    private void CheckForUnlockableTrack(int difficulty)
    {
        if (MusicForm.SetRestriction)
        {
            if (RestrictionContentLifted(MusicForm))
                UpdateContentAchievementStatus(difficulty);

            else
            {
                // Get user interface to display
                LoadUserFeedContent(UserDetailFeebackOrder.RestrictedContent);
                ModifyOfBattleButtonText(BattleBtnPrompt.ContentLocked);
            }
        }

        else if (MusicForm.unlockAbleContent && !PlayerPrefs.HasKey("MarathonPermit"))
        {
            string itemRequired = "Title Deed: " + MusicForm.titleDeed_for_Area + " Story Play";
            VirtualItemDatabase itemForProve = MeloMelo_ItemUsage_Settings.GetActiveItem(itemRequired);

            UserDetailFeedback[(int)UserDetailFeebackOrder.RestrictedContent].transform.GetChild(0).GetChild(1).GetComponent<Text>().text
                = "Obtain " + itemRequired;

            if (itemForProve.itemName != itemRequired)
            {
                // Get user interface to display
                LoadUserFeedContent(UserDetailFeebackOrder.RestrictedContent);
                ModifyOfBattleButtonText(BattleBtnPrompt.ContentLocked);
            }
            else
                UpdateContentAchievementStatus(difficulty);
        }

        else
            UpdateContentAchievementStatus(difficulty);
    }

    private void UpdateContentAchievementStatus(int difficulty)
    {
        if (PlayerPrefs.GetInt(MusicForm.Title + "_BattleRemark_" + difficulty, 6) != 6)
        {
            // Get user interface to display
            LoadUserFeedContent(UserDetailFeebackOrder.ExistingEntry);

            // Update Content (Track Base)
            UpdateContentAchievementStatus_Board(0, 1).text = PlayerPrefs.GetInt(MusicForm.Title + "_score" + difficulty, 0).ToString("0000000");
            UpdateContentAchievementStatus_Board(0, 2).text = MeloMelo_GameSettings.GetScoreRankStructure(PlayerPrefs.GetInt(MusicForm.Title + "_score" + difficulty, 0).ToString()).rank;
            Invoke("UpdatePointContentStatus", 0.5f);

            UpdateContentAchievementStatus_Board(0, 5).text =
                PlayerPrefs.GetInt(MusicForm.Title + "_maxCombo" + difficulty, 0) + " / " + PlayerPrefs.GetInt(MusicForm.Title + "_overallCombo" + difficulty, 0);

            //PlayerPrefs.GetInt(MusicForm.Title + "_maxPoint" + difficulty) : "--/--");
            AchieveRemark(difficulty);

            //if (PlayerPrefs.GetString(MusicForm.Title + "_score" + difficulty + "_Rate", "F") == "T")
        }
        else
        {
            // Get user interface to display
            LoadUserFeedContent(UserDetailFeebackOrder.NewChartEntry);
        }
    }

    private void UpdatePointContentStatus()
    {
        UpdateContentAchievementStatus_Board(0, 3).text =
               (PlayerPrefs.GetInt(MusicForm.Title + "_point" + MeloMelo_GameSettings.GetTrackDifficultyMode(), 0) != 0 ?
               PlayerPrefs.GetInt(MusicForm.Title + "_point" + MeloMelo_GameSettings.GetTrackDifficultyMode(), 0) + "/" +
               PlayerPrefs.GetInt("GetMaxPoint_" + MeloMelo_GameSettings.GetTrackDifficultyMode(), 0) : "--/--");
    }

    private Text UpdateContentAchievementStatus_Board(int page, int line)
    {
        // Browse all achievement board texts
        return UserDetailFeedback[(int)UserDetailFeebackOrder.ExistingEntry].transform.GetChild(page).GetChild(1).GetChild(line).GetComponent<Text>();
    }

    protected void AchieveRemark(int index)
    {
        int i = PlayerPrefs.GetInt(MusicForm.Title + "_BattleRemark_" + index, 6);

        RemarkIcon.text = MeloMelo_GameSettings.GetStatusByAchievement(i) != null ? 
            MeloMelo_GameSettings.GetStatusByAchievement(i).remark : string.Empty;

        RemarkIcon.color = MeloMelo_GameSettings.GetStatusByAchievement(i) != null ? 
            MeloMelo_GameSettings.GetStatusByAchievement(i).colorBorder : Color.gray;
    }

    public void DisplayLevelDetails(int index)
    {
        LoadUserFeedContent(UserDetailFeebackOrder.LevelDetailContent);

        // Change attribute
        ModifyOfBattleButtonText(BattleBtnPrompt.ViewInfo);
    }

    public void UpdateData_Level(int index, float level)
    {
        switch (index)
        {
            case 1:
                difficulyValue[(int)DifficultyValueIndex.Normal] = level;
                PlayerPrefs.SetString(GetDifficultyDataDetail(DifficultyValueIndex.Normal), ((level - ((int)level + 0.5f) > 0f && level > 5) ? (int)level + "+" : (int)level + ""));
                break;

            case 2:
                difficulyValue[(int)DifficultyValueIndex.Hard] = level;
                PlayerPrefs.SetString(GetDifficultyDataDetail(DifficultyValueIndex.Hard), ((level - ((int)level + 0.5f) > 0f && level > 10) ? (int)level + "+" : (int)level + ""));
                break;

            case 3:
                difficulyValue[(int)DifficultyValueIndex.Ultimate] = level;
                PlayerPrefs.SetString(GetDifficultyDataDetail(DifficultyValueIndex.Ultimate), ((level - ((int)level + 0.5f) > 0f && level > 10) ? (int)level + "+" : (int)level + ""));
                break;
        }

        // Update level display
        if (MeloMelo_GameSettings.GetTrackDifficultyMode() == index) SelectionMenu_Script.thisSelect.UpdateCounterLevel(level);
    }
    #endregion

    #region MISC (Selection Info)
    private Texture GetTrackCover(Texture content)
    {
        if (content) return content;
        return templateForm.Background_Cover;
    }

    private string GetTrackArtistName(string output)
    {
        if (output != string.Empty) return "[ " + output + " ]";
        return "[" + templateForm.ArtistName + "]";
    }

    private string GetTrackTitleName(string output)
    {
        if (output != string.Empty) return output;
        return templateForm.Title;
    }

    private string GetTrackBPM(float output, float output2, bool condition)
    {
        if (condition) return "BPM: " + output2.ToString("0");
        return "BPM: " + output.ToString("0");
    }

    private string LevelDesignerAutoFilled(string designerName)
    {
        if (designerName == "---")
            return "";

        return "Level Designer by " + designerName;
    }
    #endregion

    #region MISC (Selection Order)
    private int ReservePickMode(int count)
    {
        if (PreSelection_Script.thisPre.get_AreaData.reverseOrder)
            return PreSelection_Script.thisPre.get_AreaData.totalMusic - count + 1;
        else
            return count;
    }
    #endregion

    #region COMPONENT (Extra Content Manage)
    private bool RestrictionContentLifted(MusicScore form)
    {
        int current = 0;
        GetUpdateOfRestrictContent(form);

        if (form.RestrictRequirement.Length != 0)
        {
            foreach (RestrictedZoneTemplate condition in form.RestrictRequirement)
            {
                int checkScore = 0;

                // Any difficulty
                if (condition.difficulty == 0)
                {
                    // Check all difficulty when found
                    for (int select = 1; select < 4; select++)
                    {
                        checkScore = PlayerPrefs.GetInt(condition.TrackTitle + "_score" + select, 0);
                        if (checkScore >= condition.score) { break; }
                    }
                }
                else
                    // Assigned Difficulty
                    checkScore = PlayerPrefs.GetInt(condition.TrackTitle + "_score" + condition.difficulty, 0);

                // Add current when found
                if (checkScore >= condition.score) current++;
            }

            return current == form.RestrictRequirement.Length;
        }
        else return false;
    }

    private void GetUpdateOfRestrictContent(MusicScore form)
    {
        if (form.RestrictRequirement.Length == 0)
            UserDetailFeedback[(int)UserDetailFeebackOrder.RestrictedContent].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "---";
        else
            UserDetailFeedback[(int)UserDetailFeebackOrder.RestrictedContent].transform.GetChild(0).GetChild(1).GetComponent<Text>().text = string.Empty;

        foreach (RestrictedZoneTemplate info in form.RestrictRequirement)
        {
            UserDetailFeedback[(int)UserDetailFeebackOrder.RestrictedContent].transform.GetChild(0).GetChild(1).GetComponent<Text>().text
                += info.TrackTitle + " [" + GetDifficultyByName(info.difficulty) + "] - " + GetScoreRequirement(info.score) + "\n";
        }
    }

    private string GetDifficultyByName(int index)
    {
        switch (index)
        {
            case 1:
                return "Normal";

            case 2:
                return "Hard";

            case 3:
                return "Ultimate";

            default:
                return "Any Difficulty";
        }
    }

    private string GetScoreRequirement(int score)
    {
        if (score < MeloMelo_GameSettings.GetScoreRankStructure("A").score) return "Cleared the track";
        else return "Rank " + MeloMelo_GameSettings.GetScoreRankStructure(score.ToString()).rank;
    }
    #endregion

    #region EXTRA_ELEMENT_SCRIPTING
    [SerializeField] private GameObject[] Label_Visible_Tag;

    public void SimpleVisibleLabel_BattleXP(bool visible)
    {
        Slider areaExperienceBar = UserDetailFeedback[(int)UserDetailFeebackOrder.ExistingEntry].transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Slider>();
        
        Label_Visible_Tag[2].SetActive(visible);
        Label_Visible_Tag[2].transform.GetComponentInChildren<Text>().text = areaExperienceBar.value + " / " + areaExperienceBar.maxValue;
    }

    public void SimpleVisibleLabel_ZoneLevel(bool visible)
    {
        Label_Visible_Tag[1].SetActive(visible);
    }

    public void SimpleVisibleLabel_MaxOutCount(bool visible)
    {
        Label_Visible_Tag[0].SetActive(visible);
    }

    public void ToogleAchievementTab(bool visible) 
    { 
        if (!PlayerPrefs.HasKey("MarathonPermit"))
        {
            UserDetailFeedback[(int)UserDetailFeebackOrder.ExistingEntry].transform.GetChild(0).gameObject.SetActive(visible);
            UserDetailFeedback[(int)UserDetailFeebackOrder.ExistingEntry].transform.GetChild(1).gameObject.SetActive(!visible);
        }
    }

    private void GetMaxScoreTag(string title)
    {
        if (PlayerPrefs.GetInt(title + "_maxScore" + MeloMelo_GameSettings.GetTrackDifficultyMode(), 0) > 0)
        {
            MaxOutScoreTag.gameObject.SetActive(true);
            MaxOutScoreTag.GetComponentInChildren<Text>().text = PlayerPrefs.GetInt(title + "_maxScore" + MeloMelo_GameSettings.GetTrackDifficultyMode(), 0).ToString();
        }
        else
            MaxOutScoreTag.gameObject.SetActive(false);
    }

    private void GetAreaLeveling(string title)
    {
        Slider areaExperienceBar = UserDetailFeedback[(int)UserDetailFeebackOrder.ExistingEntry].transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Slider>();
        int currentAreaLevel = PlayerPrefs.GetInt(title + "_areaLevel" + MeloMelo_GameSettings.GetAreaDifficultyMode() + MeloMelo_GameSettings.GetTrackDifficultyMode(), 1);

        UserDetailFeedback[(int)UserDetailFeebackOrder.ExistingEntry].transform.GetChild(1).GetChild(1).GetChild(0).GetChild(3).GetComponentInChildren<Text>().text
            = currentAreaLevel.ToString();

        areaExperienceBar.maxValue = currentAreaLevel < MeloMelo_GameSettings.maxLevelZoneArea ? MeloMelo_GameSettings.GetZoneData(currentAreaLevel).requiredExperience : 0;
        areaExperienceBar.value = PlayerPrefs.GetInt(title + "_areaExperience" + MeloMelo_GameSettings.GetAreaDifficultyMode() + MeloMelo_GameSettings.GetTrackDifficultyMode(), 0);
    }

    private void GetRPGElement_TrackBattleInfo(string title)
    {
        int trackDifficulty = MeloMelo_GameSettings.GetTrackDifficultyMode();
        int areaDifficulty = MeloMelo_GameSettings.GetAreaDifficultyMode();

        UpdateContentAchievementStatus_Board(1, 2).text =
            PlayerPrefs.GetInt(title + "_totalEnemyCountForTrack" + trackDifficulty + areaDifficulty, 0) + " / " + PlayerPrefs.GetInt(title + "_OverallEnemyCountForTrack" + trackDifficulty + areaDifficulty, 0);

        UpdateContentAchievementStatus_Board(1, 4).text =
            PlayerPrefs.GetInt(title + "_totalTrapsCountForTrack" + trackDifficulty + areaDifficulty, 0) + " / " + PlayerPrefs.GetInt(title + "_OverallTrapsCountForTrack" + trackDifficulty + areaDifficulty, 0);

        UpdateContentAchievementStatus_Board(1, 6).text =
            PlayerPrefs.GetInt(title + "_totalHealCountForTrack" + trackDifficulty + areaDifficulty, 0) + " / " + PlayerPrefs.GetInt(title + "_OverallHealCountForTrack" + trackDifficulty + areaDifficulty, 0);
    }
    #endregion
}
