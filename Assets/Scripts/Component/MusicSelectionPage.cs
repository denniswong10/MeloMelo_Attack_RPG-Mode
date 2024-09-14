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

    public void Setup_Page()
    {
        ScrollNagivatorSettings
            (
                // Total Selection
                PlayerPrefs.HasKey("MarathonPermit") ?
                    Resources.Load<MarathonInfo>(PlayerPrefs.GetString("Marathon_Assigned_Task", string.Empty)).Difficultylevel.Length 
                            :
                            PreSelection_Script.thisPre.get_AreaData.totalMusic
                ,

                // Current Selection
                PlayerPrefs.HasKey("MarathonPermit") ? PlayerPrefs.GetInt("MarathonChallenge_MCount") : PlayerPrefs.GetInt("LastSelection", 1)
            ); 

        RefreshMusicInformationPanel
            (
                // Area Assigned
                PlayerPrefs.HasKey("MarathonPermit") ?
                        PlayerPrefs.GetString("Marathon_Assigned_Area", string.Empty)
                        :
                        "Database_Area/" + PreSelection_Script.thisPre.get_AreaData.AreaName
                ,

                // Is play casual?
                !PlayerPrefs.HasKey("MarathonPermit")
            );

        GetNavAvaialbleToggle();

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
    private void RefreshMusicInformationPanel(string areaLocated, bool casualMode)
    {
        // Load music database
        MusicForm = Resources.Load<MusicScore>(areaLocated + "/M" + (casualMode ? ReservePickMode((int)ScrollNagivator_ProgressBar.value) :
            PlayerPrefs.GetInt("MarathonChallenge_MCount")));

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

        // Load music playing
        if (SelectionMenu_Script.thisSelect.get_BGM)
        {
            if (SelectionMenu_Script.thisSelect.get_BGM.GetComponent<AudioSource>().volume != 1)
            { SelectionMenu_Script.thisSelect.get_BGM.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("BGM_VolumeGET", 1); }
            SelectionMenu_Script.thisSelect.get_BGM.GetComponent<AudioSource>().clip = MusicForm.Music;
            SelectionMenu_Script.thisSelect.get_BGM.GetComponent<AudioSource>().time = MusicForm.PreviewTime;
            SelectionMenu_Script.thisSelect.get_BGM.GetComponent<AudioSource>().Play();
        }

        // Track Assigned Tag 
        if (MusicForm.ScoreObject != null)
        {
            NewReleaseTag.SetActive(PlayerPrefs.HasKey(MusicForm.Title + "_newReleaseTrack"));
            AreaBonusTag.SetActive(PlayerPrefs.HasKey(MusicForm.Title + "_areaBonusTrack"));
        }

        // Load new score sheet
        LoadNewScoreSheet();
    }
    #endregion

    #region MAIN
    public void NavOverList(bool reverse)
    {
        // Toogle over previous and next selection
        ScrollNagivator_ProgressBar.value += reverse ? -1 : 1;
        GetNavAvaialbleToggle();
    }

    public void ModifyOfMusicListChange()
    {
        ScrollNagivator_Text.text = ScrollNagivator_ProgressBar.value + "/" + ScrollNagivator_ProgressBar.maxValue;
        RefreshMusicInformationPanel("Database_Area/" + PreSelection_Script.thisPre.get_AreaData.AreaName, true);
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
                    (PlayerPrefs.HasKey("MarathonPermit") && i == PlayerPrefs.GetInt("DifficultyLevel_valve", 1) - 1))
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
            switch (PlayerPrefs.GetInt("DifficultyLevel_valve", 1))
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
            CheckForRestrictionTrack(PlayerPrefs.GetInt("DifficultyLevel_valve", 1));

            //CancelInvoke("LoadAndWriteBestRecord");
            //Invoke("LoadAndWriteBestRecord", 3);

            if (PlayerPrefs.GetInt("Marathon_Challenge", 0) == 0)
            {
                if (PlayerPrefs.GetInt(MusicForm.Title + "_BattleRemark_" + PlayerPrefs.GetInt("DifficultyLevel_valve", 0), 6) == 5)
                { RemarkIcon2.text = "FAILED!"; RemarkIcon2.color = Color.red; }

                else if (PlayerPrefs.GetString(MusicForm.Title + "_SuccessBattle_" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1) + PlayerPrefs.GetInt("BattleDifficulty_Mode", 1), "F") == "T")
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

    private void CheckForRestrictionTrack(int difficulty)
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

            //PlayerPrefs.GetInt(MusicForm.Title + "_maxPoint" + difficulty) : "--/--");
            AchieveRemark(difficulty);

            // Update Content (Battle Base)
            UpdateContentAchievementStatus_Board(1, 3).text = PlayerPrefs.GetInt(MusicForm.Title + "_techScore" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1) + PlayerPrefs.GetInt("BattleDifficulty_Mode", 1), 0).ToString("0");

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
               (PlayerPrefs.GetInt(MusicForm.Title + "_point" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 0) != 0 ?
               PlayerPrefs.GetInt(MusicForm.Title + "_point" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 0) + "/" +
               PlayerPrefs.GetInt("GetMaxPoint_" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 0) : "--/--");
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
}
