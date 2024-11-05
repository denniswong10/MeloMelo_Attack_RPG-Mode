using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MeloMelo_ExtraComponent;
using MeloMelo_RPGEditor;

public class CollectionNew_Script : MonoBehaviour
{
    private enum Sub_Selection_Order
    {
        UnitFormation,
        CharacterList,
        MusicAlbum,

        AllSelection
    }

    public static CollectionNew_Script thisCollect;

    private GameObject[] BGM;
    private string ResMelo = string.Empty;

    public GameObject MainSelection;
    public GameObject[] SubSelection;

    private MusicAlbum album;
    public GameObject[] AlertSign;
    public GameObject PopUpMusic;
    public GameObject MusicSelection_BackBtn;

    public GameObject ContentTrackDashBoard;
    public GameObject LoadingScreen_TrackContent;
    [SerializeField] private GameObject BoostPanelTemplate;

    [SerializeField] private CharacterAlbum_Base collection_characterAlbum;
    [SerializeField] private CharacterFormation_Base collection_formation;

    void Update()
    {
        if (Input.anyKeyDown && ContentTrackDashBoard.activeInHierarchy) ContentTrackDashBoard.SetActive(false);
        if (GameObject.Find("EXP_Boost_Panel") != null) collection_characterAlbum.UpdateTicketPrompt("EXP_Boost_Panel");
    }

    // Component System: Start-up
    private void BGM_Loader()
    {
        BGM = GameObject.FindGameObjectsWithTag("BGM");
        if (BGM.Length > 1) { for (int i = 1; i < BGM.Length; i++) { Destroy(BGM[i]); } }
    }

    // Program: Collection Scene
    void Start()
    {
        thisCollect = this;

        // Check Program Start: Intit
        ResMelo = PlayerPrefs.GetString("Resoultion_Melo", string.Empty);
        MainSelection.GetComponent<Animator>().SetTrigger("Opening" + ResMelo);
        PlayerPrefs.SetInt("CollectionAlbum_Visited", 1);

        // System Component: Intit
        BGM_Loader();
    }

    #region MAIN
    public void ReturnToSelection(Button _ref)
    {
        MainSelection.GetComponent<Animator>().SetTrigger("Closing" + ResMelo);
        StartCoroutine(ReturnToSelection_decode(true));
        _ref.interactable = false;
    }

    public void ReturnToCollection(int index)
    {
        SubSelection[index].GetComponent<Animator>().SetTrigger("Closing" + ResMelo);
        StartCoroutine(ReturnToSelection_decode(false));
    }

    public void OpenSubSelection(int index)
    {
        MainSelection.GetComponent<Animator>().SetTrigger("Closing" + ResMelo);
        StartCoroutine(LoadUpSubSelection(index));
    }
    #endregion

    #region COMPONENT
    IEnumerator LoadUpSubSelection(int index)
    {
        yield return new WaitForSeconds(1.5f);
        if (index < SubSelection.Length) SubSelection[index].GetComponent<Animator>().SetTrigger("Opening" + ResMelo);

        switch (index)
        {
            case 1:
                collection_characterAlbum.LoadContent();
                break;

            case 2:
                if (album == null) album = new MusicAlbum();
                LoadMusicContent();
                break;

            case 3:
                SceneManager.LoadScene("StoragePage");
                break;

            default:
                collection_formation.UpdateFormationList();
                break;
        }
    }

    IEnumerator ReturnToSelection_decode(bool main)
    {
        yield return new WaitForSeconds(1.5f);

        if (main) SceneManager.LoadScene("Ref_PreSelection");
        else SceneManager.LoadScene("Collections");
    }
    #endregion

    #region FORMATION
    public void Formation_EditMode()
    {
        collection_formation.EditFormation();
    }

    public void Formation_SetButton()
    {
        collection_formation.SetMainFormation();
    }

    public void Formation_ClearAll()
    {
        collection_formation.ClearAllFormation();
    }

    public void Formation_SlotModify_Setup(int slot)
    {
        collection_formation.ChangeCharacterSettings(slot);
    }

    public void Formation_Toggle_UpdateContent(Dropdown content)
    {

    }
    #endregion

    #region MUSIC ALBUM
    private void LoadMusicContent()
    {
        StartCoroutine(album.RegisterAllActiveArea());
        StartCoroutine(album.FinishedTransition_SelectMusic());
    }

    public void LoadMusicList()
    {
        album.UpdateDropDownContent();
    }

    public void PlayMusicButton()
    {
        album.StartMusicPlayer(false);
    }

    public void StopMusicButton()
    {
        album.StartMusicPlayer(true);
    }

    public void Popup_Notication(bool on, string title)
    {
        if (on)
        {
            PopUpMusic.GetComponent<Animator>().SetTrigger("Open");
            MusicSelection_BackBtn.GetComponent<Animator>().SetTrigger("Close");
        }
        else
        {
            PopUpMusic.GetComponent<Animator>().SetTrigger("Close");
            MusicSelection_BackBtn.GetComponent<Animator>().SetTrigger("Open");
        }

        PopUpMusic.transform.GetChild(1).GetComponent<Text>().text = title;
    }

    public void OpenTrackInfo(int trackIndex)
    {
        ContentTrackDashBoard.SetActive(true);
        album.UpdateSelectMusic(trackIndex);
    }

    //private void LoadMusicContent()
    //{
    //    List<string> areaRegister = new List<string>();
    //    for (int season = 0; season < StartMenu_Script.thisMenu.get_seasonNum + 1; season++)
    //    {
    //        for (int index = 0; index < Resources.LoadAll<AreaInfo>("Database_Area/Season" + season).Length; index++)
    //        {
    //            areaRegister.Add(Resources.Load<AreaInfo>("Database_Area/Season" + season + "/A" + (index + 1)).AreaName);
    //        }
    //    }

    //    GameObject.Find("AreaOption").GetComponent<Dropdown>().AddOptions(areaRegister);
    //}

    //private void ToogleMusicContent(int value)
    //{
    //    GameObject MusicSelection_Ref = GameObject.Find("Selection_Music");
    //    string area = GameObject.Find("AreaOption").transform.GetChild(0).GetComponent<Text>().text;
    //    MusicScore content = Resources.Load<MusicScore>("Database_Area/" + area + "/M" + (value + 1));

    //    MusicSelection_Ref.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "[ " + content.ArtistName + " ]";
    //    MusicSelection_Ref.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = content.Title;
    //    MusicSelection_Ref.transform.GetChild(0).GetChild(3).GetComponent<Text>().text = "Imported by " + content.DesignerName;
    //    MusicSelection_Ref.transform.GetChild(2).GetComponent<Slider>().value = value;
    //    MusicSelection_Ref.transform.GetChild(1).GetChild(0).GetComponent<RawImage>().texture = content.Background_Cover;
    //    if (content.creditPoint != "--") { MusicSelection_Ref.transform.GetChild(0).GetChild(6).GetComponent<Text>().text = "@ " + content.creditPoint; }
    //    else { MusicSelection_Ref.transform.GetChild(0).GetChild(6).GetComponent<Text>().text = ""; }
    //}

    //public void LoadMusicList()
    //{
    //    int musicList = 0;
    //    newSelectedArea = GameObject.Find("AreaOption").transform.GetChild(0).GetComponent<Text>().text;

    //    if (currentAreaSelected != newSelectedArea)
    //    {
    //        currentAreaSelected = newSelectedArea;
    //        musicList = Resources.LoadAll<MusicScore>("Database_Area/" + currentAreaSelected).Length;
    //    }

    //    GameObject.Find("ScrollBar_Music").GetComponent<Slider>().maxValue = musicList;
    //    ToogleMusicContent(0);
    //}

    //public void ToggleOverMusicSwitcher(bool reverse)
    //{
    //    int index = (int)GameObject.Find("ScrollBar_Music").GetComponent<Slider>().maxValue;

    //    if (reverse) { currentListToogle--; }
    //    else { currentListToogle++; }

    //    if (currentListToogle < 0) currentListToogle = 0;
    //    else if (currentListToogle > (index - 1)) currentListToogle = index - 1;

    //    ToogleMusicContent(currentListToogle);
    //}

    //private void MusicScrollMethod()
    //{
    //    if (Input.GetKeyDown(KeyCode.UpArrow)) ToggleOverMusicSwitcher(true);
    //    else if (Input.GetKeyDown(KeyCode.DownArrow)) ToggleOverMusicSwitcher(false);
    //}
    #endregion

    #region CHARACTER ALBUM
    public void SelectCharacter_TabPanel(int index)
    {
        collection_characterAlbum.ToggleContentTab(index);
    }

    public void SelectNagivator_Top(bool previous)
    {
        collection_characterAlbum.NagivateSelection(previous ? -1 : 1);
    }

    public void SkillPanel_ToggleSkillDetail(int slot_index)
    {
        collection_characterAlbum.ToggleOverDetails(slot_index, true);
    }

    public void SkillPanel_OffToggleSkillDetail(int slot_index)
    {
        collection_characterAlbum.ToggleOverDetails(slot_index, false);
    }

    public void RebornPanel_SelectedPickUp(int slot_index)
    {
        collection_characterAlbum.PickMasteryTab(slot_index);
    }

    public void RebornPanel_ConfirmationTab_Confirm()
    {
        collection_characterAlbum.ConfirmMastery();
    }

    public void RebornPanel_ConfirmationTab_Cancel()
    {
        collection_characterAlbum.CancelMastery();
    }

    public void ExperienceBoost_StartUpButton(string miniPanel)
    {
        if (GameObject.Find(miniPanel) == null)
        {
            GameObject instance_panel = Instantiate(BoostPanelTemplate);
            instance_panel.name = miniPanel;

            instance_panel.GetComponent<VirtualStorageBag>().SetAlertPopReference(collection_characterAlbum.CharacterTab_MessageTab);
            instance_panel.GetComponent<VirtualStorageBag>().SetDefaultDescription("No ticket has been used");
            instance_panel.GetComponent<VirtualStorageBag>().SetItemForDisplay(GetItemArray());
            instance_panel.GetComponent<VirtualStorageBag>().SetLimitedUsageTime(false);
            instance_panel.transform.SetParent(collection_characterAlbum.selectionPanel.transform);
        }
    }

    private VirtualItemDatabase[] GetItemArray()
    {
        List<VirtualItemDatabase> listOfItem; 
        listOfItem = new List<VirtualItemDatabase>();
        foreach (UsageOfItemDetail item in Resources.LoadAll<UsageOfItemDetail>("Database_Item/Filtered_Items/EXP_TICKET"))
        {
            VirtualItemDatabase itemFound = MeloMelo_GameSettings.GetAllItemFromLocal(item.itemName);
            if (itemFound.amount > 0) listOfItem.Add(itemFound);
        }

        return listOfItem.ToArray();
    }
    #endregion

    [System.Serializable]
    public class CharacterAlbum_Base
    {
        public GameObject selectionPanel;
        [SerializeField] private Texture NoneOfAbove;
        [SerializeField] private Texture[] ElementListing;

        private List<SkillContainer> skill_listing_container;
        [SerializeField] private GameObject[] MasteryTabs;
        [SerializeField] private GameObject MasteryInfoTab;
        public GameObject CharacterTab_MessageTab;
        [SerializeField] private Text TicketPromptMessage;

        private List<ClassBase> Character_Database;
        private List<StatsManage_Database> characterStatus;
        private int characterToggleIndex = 1;

        #region SETUP
        public void LoadContent()
        {
            const string GetCharacterIsNull = "None";
            characterStatus = new List<StatsManage_Database>();
            Character_Database = new List<ClassBase>();
            skill_listing_container = new List<SkillContainer>();

            // Gather all available characters
            foreach (ClassBase character in Resources.LoadAll<ClassBase>("Character_Data"))
                if (character.characterName != GetCharacterIsNull) Character_Database.Add(character);

            // Get status format with all characters
            foreach (ClassBase character in Character_Database.ToArray())
                characterStatus.Add(new StatsManage_Database(character.name));

            // Update content with the data given
            UpdateAlbum();
            UpdateStatsTab();
            NavChanger();

            // Load default selected tab
            selectionPanel.transform.GetChild(8).GetComponent<RawImage>().color = Color.green;
        }
        #endregion

        #region MAIN
        public void NagivateSelection(int add)
        {
            // Modify scroll value according to which type of selection
            selectionPanel.transform.GetChild(3).GetComponent<Slider>().value += add;
            // Update nagivator content once modified
            NavChanger();
        }

        public void ToggleContentTab(int index)
        {
            if (index == 2 && !MeloMelo_CharacterInfo_Settings.GetCharacterStatus(Character_Database[characterToggleIndex - 1].name))
            {
                thisCollect.StartCoroutine(PromptLockedFeatures());
                thisCollect.StartCoroutine(GetPromptMessage("Unlock this character first"));
            }
            else
            {
                // Restart content information and continue for below
                for (int i = 0; i < 3; i++)
                {
                    selectionPanel.transform.GetChild(2).GetChild(5 + i).gameObject.SetActive(false);
                    selectionPanel.transform.GetChild(8 + i).GetComponent<RawImage>().color = Color.white;
                }

                // Get content information 
                selectionPanel.transform.GetChild(2).GetChild(5 + index).gameObject.SetActive(true);
                // Get button tab selected
                selectionPanel.transform.GetChild(8 + index).GetComponent<RawImage>().color = Color.green;
            }
        }
        #endregion

        #region MAIN (SKILL TAB)
        public void ToggleOverDetails(int skill_index, bool isShown)
        {
            selectionPanel.transform.GetChild(2).GetChild(6).GetChild(6).gameObject.SetActive(isShown);
            selectionPanel.transform.GetChild(2).GetChild(6).GetChild(6).GetChild(0).GetComponent<Text>().text =
                skill_listing_container[skill_index] ? skill_listing_container[skill_index].skillName : "???";

            selectionPanel.transform.GetChild(2).GetChild(6).GetChild(6).GetChild(1).GetComponent<Text>().text =
                skill_listing_container[skill_index] ? skill_listing_container[skill_index].description : string.Empty;

            CharacterTab_MessageTab.SetActive(isShown);
            if (MeloMelo_SkillData_Settings.CheckSkillStatus(skill_listing_container[skill_index].skillName) ||
                skill_listing_container[skill_index].isUnlockReady)
                CharacterTab_MessageTab.transform.GetChild(0).GetComponent<Text>().text = "Grade: " +
                    MeloMelo_SkillData_Settings.CheckSkillGrade(skill_listing_container[skill_index].skillName) + "  |  Skill can be upgraded";
            else
                CharacterTab_MessageTab.transform.GetChild(0).GetComponent<Text>().text = "You have not yet learn this skill";
        }
        #endregion

        #region MAIN (REBORN TAB)
        public void PickMasteryTab(int index)
        {
            // Display confirmation before processing to mastery
            GetMasteryContentConfirmationTab(index);
        }

        public void ConfirmMastery()
        {
            MasteryInfoTab.SetActive(false);
            string[] breakData = MasteryInfoTab.transform.GetChild(5).name.Split("_");
            ProcessToAddonsMastery(GetMasteryInfo(int.Parse(breakData[1])));

            int currentMasteryPoint = MeloMelo_ExtraStats_Settings.GetMasteryPoint(Character_Database[characterToggleIndex - 1].name);
            MeloMelo_ExtraStats_Settings.SetMasteryPoint(Character_Database[characterToggleIndex - 1].name, 
                currentMasteryPoint - GetMasteryPointCost());

            if (GetMasteryInfo(int.Parse(breakData[1])).name.Split("_")[1] == "Ultimate") Character_Database[characterToggleIndex - 1].ResetLevel();          
            PlayerPrefs.DeleteKey("MasteryShuffleTab");
            UpdateRebornTab();
        }

        public void CancelMastery()
        {
            MasteryInfoTab.SetActive(false);
        }
        #endregion

        #region COMPONENT 
        private void UpdateAlbum()
        {
            Slider scrollBar = selectionPanel.transform.GetChild(3).GetComponent<Slider>();

            // Get character length display to user
            scrollBar.GetComponent<Slider>().maxValue = Character_Database.ToArray().Length;
            UpdateScrollBarDisplay();

            // Get information ready before used
            foreach (ClassBase i in Character_Database) { i.UpdateCurrentStats(false); }
        }

        private void NavChanger()
        {
            Slider scrollBar = selectionPanel.transform.GetChild(3).GetComponent<Slider>();

            // Update new index to loader
            characterToggleIndex = (int)scrollBar.value;
            UpdateScrollBarDisplay();

            // Check interaction to nagivator [LEFT]
            NagivatorSelector(1).interactable = scrollBar.value > 1;

            // Check interaction to nagivator [RIGHT]
            NagivatorSelector(2).interactable = scrollBar.value < Character_Database.ToArray().Length;

            // Get new information set from loader
            UpdateStatsTab();
            if (!MeloMelo_CharacterInfo_Settings.GetCharacterStatus(Character_Database[characterToggleIndex - 1].name)) ToggleContentTab(0);
        }
        #endregion

        #region COMPONENT (ALL TAB)
        private void UpdateStatsTab()
        {
            // Assign data information of level, experience through class base
            int currentLevel, currentExperience;
            string className = Character_Database[characterToggleIndex - 1].name;
            currentLevel = Character_Database[characterToggleIndex - 1].level;
            currentExperience = Character_Database[characterToggleIndex - 1].experience;

            // Show that character icon is present in the class base
            selectionPanel.transform.GetChild(2).GetChild(1).GetComponent<Image>().enabled =
                Character_Database[characterToggleIndex - 1].icon != null;

            selectionPanel.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<RawImage>().texture =
                ElementListing[(int)Character_Database[characterToggleIndex - 1].elementType];

            // Able to present character icon through current selection
            selectionPanel.transform.GetChild(2).GetChild(1).GetComponent<Image>().sprite =
                Character_Database[characterToggleIndex - 1].icon;

            // Fill in all character status information
            SetStatusInformation(0).text = "[ " + Character_Database[characterToggleIndex - 1].characterName + " ]";
            SetStatusInformation(1).text = "Class: " + characterStatus[characterToggleIndex - 1].GetCharacterStatus(currentLevel).GetClassName;

            SetStatusInformation(2).text = "STRENGTH (STR): " + characterStatus[characterToggleIndex - 1].GetCharacterStatus(currentLevel).GetStrength
                + (MeloMelo_ExtraStats_Settings.GetExtraStrengthStats(className) == 0 ? string.Empty : " (" +
                GetValuePlusMinus(MeloMelo_ExtraStats_Settings.GetExtraStrengthStats(className)) + ")");

            SetStatusInformation(3).text = "VITALITY (VIT): " + characterStatus[characterToggleIndex - 1].GetCharacterStatus(currentLevel).GetVitality
                + (MeloMelo_ExtraStats_Settings.GetExtraVitaltyStats(className) == 0 ? string.Empty : " (" +
                GetValuePlusMinus(MeloMelo_ExtraStats_Settings.GetExtraVitaltyStats(className)) + ")");

            SetStatusInformation(4).text = "MAGIC (MAG): " + characterStatus[characterToggleIndex - 1].GetCharacterStatus(currentLevel).GetMagic
                + (MeloMelo_ExtraStats_Settings.GetExtraMagicStats(className) == 0 ? string.Empty : " (" +
                GetValuePlusMinus(MeloMelo_ExtraStats_Settings.GetExtraMagicStats(className)) + ")");

            SetStatusInformation(5).text = "BASE HEALTH: " + characterStatus[characterToggleIndex - 1].GetCharacterStatus(currentLevel).GetHealth
                + (MeloMelo_ExtraStats_Settings.GetExtraBaseHealth(className) == 0 ? " (+0)" : " (" +
                GetValuePlusMinus(MeloMelo_ExtraStats_Settings.GetExtraBaseHealth(className)) + ")");

            selectionPanel.transform.GetChild(2).GetChild(2).GetComponent<Text>().text = "LEVEL: " +
                characterStatus[characterToggleIndex - 1].GetCharacterStatus(currentLevel).GetLevel;

            selectionPanel.transform.GetChild(2).GetChild(3).GetComponent<Text>().text = "EXP: " +
                currentExperience + "/" + characterStatus[characterToggleIndex - 1].GetCharacterStatus(currentLevel).GetExperience;

            selectionPanel.transform.GetChild(2).GetChild(4).GetComponent<Text>().text = "REBIRTH: " + MeloMelo_ExtraStats_Settings.GetRebirthPoint(className);
            PlayerPrefs.SetString(VirtualStorageBag.VirtualStorage_UsableKey, Character_Database[characterToggleIndex - 1].name);
            UpdateSkillTab();
        }

        private void UpdateSkillTab()
        {
            // Load content skills data
            LoadIndividualSkillsData();

            // Load for display of skills data
            LoadDisplaySkillData();

            // Continue to load reborn tab
            UpdateRebornTab();
        }

        private void UpdateRebornTab()
        {
            // Find all available cards which is attached with the character class name
            int masteryLimit = 0;
            while (Resources.Load<MasteryContainer>("Database_Reborn_Cards/" + Character_Database[characterToggleIndex - 1].name + "_Common_" +
                (1 + masteryLimit)) != null)
                masteryLimit++;

            // Shuffle the card and assign them in 3 tabs
            RandomizeMasteryCard(masteryLimit);

            // Update content card with the assigned number
            UpdateRebornCardContent();
        }

        private IEnumerator PromptLockedFeatures()
        {
            selectionPanel.transform.GetChild(10).GetChild(1).gameObject.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            selectionPanel.transform.GetChild(10).GetChild(1).gameObject.SetActive(false);
        }
        #endregion

        #region MISC 
        public bool UseOfTicketAllow()
        {
            // Allow only character which are unlocked
            return MeloMelo_CharacterInfo_Settings.GetCharacterStatus(Character_Database[characterToggleIndex - 1].name);
        }

        public void UpdateTicketPrompt(string panel)
        {
            if (MeloMelo_ItemUsage_Settings.GetExpBoost(Character_Database[characterToggleIndex - 1].name) > 0)
                GameObject.Find(panel).transform.GetChild(2).GetComponent<Text>().text = 
                    "Boosted x" + MeloMelo_ItemUsage_Settings.GetExpBoost(Character_Database[characterToggleIndex - 1].name) +
                    " experience through ticket";
            else
                GameObject.Find(panel).transform.GetChild(2).GetComponent<Text>().text = "No ticket has been used";
        }

        private Button NagivatorSelector(int index)
        {
            switch (index)
            {
                case 1:
                    return selectionPanel.transform.GetChild(4).GetComponent<Button>();

                default:
                    return selectionPanel.transform.GetChild(5).GetComponent<Button>();
            }
        }

        private void UpdateScrollBarDisplay()
        {
            Slider scrollBar = selectionPanel.transform.GetChild(3).GetComponent<Slider>();
            Text displaybar = scrollBar.transform.gameObject.transform.GetChild(0).GetComponent<Text>();
            displaybar.text = (int)scrollBar.value + "/" + (int)scrollBar.maxValue;
        }

        private Text SetStatusInformation(int index)
        {
            return selectionPanel.transform.GetChild(2).GetChild(5).GetChild(index).GetComponent<Text>();
        }

        private RawImage SetSkillsInformation(int index)
        {
            return selectionPanel.transform.GetChild(2).GetChild(6).GetChild(2 + index).GetComponent<RawImage>();
        }

        private string GetValuePlusMinus(int value)
        {
            if (value != 0 && value > 0) return "+" + value;
            else return value.ToString();
        }
        #endregion

        #region COMPONENT (Skill Infomration Data)
        private void LoadIndividualSkillsData()
        {
            string[] skill_key = { "_Primary_Skill", "_Secondary_Skill_1", "_Secondary_Skill_2", "_Secondary_Skill_3" };
            skill_listing_container.Clear();

            foreach (string skill_type in skill_key)
            {
                SkillContainer singleSkill = Resources.Load<SkillContainer>("Database_Skills/" + Character_Database[characterToggleIndex - 1].name +
                    skill_type);

                skill_listing_container.Add(singleSkill);
            }
        }

        private void LoadDisplaySkillData()
        {
            for (int id = 0; id < skill_listing_container.ToArray().Length; id++)
            {
                if (skill_listing_container[id]) SetSkillsInformation(id).texture = skill_listing_container[id].skillIcon;
                else SetSkillsInformation(id).texture = NoneOfAbove;
            }
        }
        #endregion

        #region COMPONENT (Reborn Shuffle Data)
        private void RandomizeMasteryCard(int maxLimit)
        {
            if (!PlayerPrefs.HasKey("MasteryShuffleTab"))
            {
                if (maxLimit > 0)
                {
                    for (int count = 0; count < MasteryTabs.Length; count++)
                    {
                        int randomizeNumber = Character_Database[characterToggleIndex - 1].level >= 99 ? (count + 1) : Random.Range(1, maxLimit);
                        PlayerPrefs.SetInt("Mastery" + (count + 1), randomizeNumber);
                        PlayerPrefs.SetInt("MasteryShuffleTab", 1);
                    }
                }
            }
        }
        
        private void UpdateRebornCardContent()
        {
            for (int index = 0; index < MasteryTabs.Length; index++)
            {
                // Update content
                MasteryTabs[index].transform.GetChild(0).GetComponent<Text>().text = GetMasteryInfo(index) != null ? GetMasteryInfo(index).title : "???";
                MasteryTabs[index].transform.GetChild(1).GetComponent<Text>().text = GetMasteryInfo(index) != null ? GetMasteryInfo(index).awardsTitle : "???";
            }
        }

        private MasteryContainer GetMasteryInfo(int slot_index)
        {
            string masteryRarity = Character_Database[characterToggleIndex - 1].level >= 99 ? "Ultimate" : "Common";
            MasteryContainer masteryContent = Resources.Load<MasteryContainer>("Database_Reborn_Cards/" +
               Character_Database[characterToggleIndex - 1].name + "_" + masteryRarity + "_" + PlayerPrefs.GetInt("Mastery" + (slot_index + 1), 1));

            return masteryContent;
        }
        #endregion

        #region COMPONENT (Reborn Information Data)
        private void GetMasteryContentConfirmationTab(int index)
        {
            if (!MasteryInfoTab.activeInHierarchy)
            {
                MasteryInfoTab.SetActive(true);
                MasteryInfoTab.transform.GetChild(1).GetComponent<Text>().text = GetMasteryInfo(index - 1).title;
                MasteryInfoTab.transform.GetChild(2).GetComponent<Text>().text = GetMasteryInfo(index - 1).description;
                MasteryInfoTab.transform.GetChild(3).GetComponent<Text>().text = "Required Point: " +
                    MeloMelo_ExtraStats_Settings.GetMasteryPoint(Character_Database[characterToggleIndex - 1].name) + " >> " +
                    (MeloMelo_ExtraStats_Settings.GetMasteryPoint(Character_Database[characterToggleIndex - 1].name) - GetMasteryPointCost());

                // Check condition on confirm mastery
                MasteryInfoTab.transform.GetChild(5).name = "Confirm_" + (index - 1) + "_Btn";
                MasteryInfoTab.transform.GetChild(5).GetComponent<Button>().interactable = 
                    MeloMelo_ExtraStats_Settings.GetMasteryPoint(Character_Database[characterToggleIndex - 1].name) >= GetMasteryPointCost();
            }
        }

        private void ProcessToAddonsMastery(MasteryContainer addons)
        {
            // Get class from container
            string[] addons_data = addons.name.Split("_");
            string messagePrinter = string.Empty;
            foreach (AwardsSettings awards in addons.awards_settings)
            {
                switch (awards.awards_caterogy)
                {
                    case AwardsSettings.TypeOfAwards.STR:
                        MeloMelo_ExtraStats_Settings.IncreaseStrengthStats(addons_data[0], int.Parse(awards.awards_value));
                        messagePrinter += (int.Parse(awards.awards_value) > 0 ? "STRENGTH increased by " : "STRENGTH decreased by ") + int.Parse(awards.awards_value);
                        break;

                    case AwardsSettings.TypeOfAwards.VIT:
                        MeloMelo_ExtraStats_Settings.IncreaseVitalityStats(addons_data[0], int.Parse(awards.awards_value));
                        messagePrinter += (int.Parse(awards.awards_value) > 0 ? "VITALITY increased by " : "VITALITY decreased by ") + int.Parse(awards.awards_value);
                        break;

                    case AwardsSettings.TypeOfAwards.MAG:
                        MeloMelo_ExtraStats_Settings.IncreaseMagicStats(addons_data[0], int.Parse(awards.awards_value));
                        messagePrinter += (int.Parse(awards.awards_value) > 0 ? "MAGIC increased by " : "MAGIC increased by ") + int.Parse(awards.awards_value);
                        break;

                    case AwardsSettings.TypeOfAwards.SKILL:
                        if (!MeloMelo_SkillData_Settings.CheckSkillStatus(awards.awards_value)) MeloMelo_SkillData_Settings.UnlockSkill(awards.awards_value);
                        else MeloMelo_SkillData_Settings.UpgradeSkill(awards.awards_value);
                        messagePrinter += (!MeloMelo_SkillData_Settings.CheckSkillStatus(awards.awards_value) ? "Skill Learned: " : "Skill Upgraded: ") + awards.awards_value;
                            break;
                }

                messagePrinter += ",";
            }

            // Prompt user about the info
            thisCollect.StartCoroutine(GetPromptMessage(messagePrinter));
        }

        private int GetMasteryPointCost()
        {
            int level = Character_Database[characterToggleIndex - 1].level;
            return level > 99 ? 0 : 1;
        }

        private IEnumerator GetPromptMessage(string long_message)
        {
            string[] message_container = long_message.Split(",");
            foreach (string message in message_container)
            {
                if (message != string.Empty)
                {
                    yield return new WaitForSeconds(1);
                    if (!CharacterTab_MessageTab.activeInHierarchy) CharacterTab_MessageTab.SetActive(true);
                    CharacterTab_MessageTab.transform.GetChild(0).GetComponent<Text>().text = message;
                    yield return new WaitForSeconds(2);
                    CharacterTab_MessageTab.SetActive(false);
                }
            }
        }
        #endregion
    }

    [System.Serializable]
    public class CharacterFormation_Base
    {
        [SerializeField] private GameObject selectionPanel;
        private bool formationEditingMode = false;

        #region MAIN
        public void EditFormation()
        {
            formationEditingMode = true;
            UpdateFormationList();

            // Prompt: Message to player
            thisCollect.StartCoroutine(PromptMessage("Team Editing Mode!"));
        }

        public void SetMainFormation()
        {
            if (formationEditingMode)
            {
                formationEditingMode = false;
                thisCollect.StartCoroutine(PromptMessage("Team Editing Done!"));
            }
            else
                thisCollect.StartCoroutine(PromptMessage(IsFormationHasSet() ? "Team Formation Assigned!" : "Assigned Failed!"));

            // Update content and interactive
            UpdateFormationList();
        }

        public void ClearAllFormation()
        {
            // Clear all character from slot and then update content
            selectionPanel.transform.GetChild(11).GetComponent<Button>().interactable = false;
            for (int slot = 0; slot < 3; slot++) ClearCharacterSlot(slot);
            LoadFormationContent();

            // Prompt: Message to player
            thisCollect.StartCoroutine(PromptMessage("All character have been dismissed."));
        }

        public void ChangeCharacterSettings(int slot)
        {
            PlayerPrefs.SetInt("SlotSelect_setup", slot);
            PlayerPrefs.SetString("SlotSelect_lastSelect", SceneManager.GetActiveScene().name);
            SceneManager.LoadScene("Ref_CharacterSelection");
        }

        public void UpdateFormationList()
        {
            // Update slot: Content information
            LoadFormationContent();

            // Edit, Set, Clear: Interactive with button
            selectionPanel.transform.GetChild(9).GetComponent<Button>().interactable = !formationEditingMode;
            selectionPanel.transform.GetChild(10).GetComponent<Button>().interactable = !IsFormationHasSet() || formationEditingMode;
            selectionPanel.transform.GetChild(11).GetComponent<Button>().interactable = IsFormationHasSet();

            // All Slot: Ready for editing
            for (int slot = 0; slot < 3; slot++)
            {
                selectionPanel.transform.GetChild(3 + slot).GetChild(3).gameObject.SetActive(formationEditingMode);
                selectionPanel.transform.GetChild(3 + slot).GetComponent<Button>().interactable = formationEditingMode;
            }
        }
        #endregion

        #region COMPONENT
        private void LoadFormationContent()
        {
            // Toggle character slot
            int characterToggleIndex = 0;

            // Get formation all status information
            LoadFormationInformation();

            // Load all characters contain class data
            StatsDistribution formation = new StatsDistribution();
            formation.load_Stats();

            foreach (ClassBase character in formation.slot_Stats)
            {
                // Show character icon if assigned
                selectionPanel.transform.GetChild(3 + characterToggleIndex).GetChild(0).gameObject.SetActive(character.icon == null);
                selectionPanel.transform.GetChild(3 + characterToggleIndex).GetChild(1).GetComponent<Image>().enabled = character.icon != null;
                selectionPanel.transform.GetChild(3 + characterToggleIndex).GetChild(1).GetComponent<Image>().sprite = character.icon;
                selectionPanel.transform.GetChild(3 + characterToggleIndex).GetChild(2).gameObject.SetActive(character.icon != null);
                selectionPanel.transform.GetChild(3 + characterToggleIndex).GetChild(2).GetChild(0).GetComponent<Text>().text = GetCharacterMain(character.name);
                
                // Go to next character slot
                characterToggleIndex++;
            }
        }

        private void LoadFormationInformation()
        {
            StatsDistribution formation = new StatsDistribution();
            formation.load_Stats();

            selectionPanel.transform.GetChild(6).GetChild(0).GetComponent<Text>().text = "UNIT POWER:\n" + formation.get_UnitPower();
            selectionPanel.transform.GetChild(7).GetChild(0).GetComponent<Text>().text = "UNIT RANK:\n" + get_Rank(formation.get_UnitPower());
            selectionPanel.transform.GetChild(8).GetChild(0).GetComponent<Text>().text = "TOTAL HEALTH:\n" + formation.get_UnitHealth("Character");
        }

        private void ClearCharacterSlot(int slot_index)
        {
            UnitFormation_Management formationManage = new UnitFormation_Management();
            formationManage.ClearUnit(slot_index);
        }

        private void SetCharacterSlotStatus(string character)
        {
            UnitFormation_Management formationManage = new UnitFormation_Management();
            formationManage.SetMainForce(character);
            LoadFormationContent();
        }
        #endregion

        #region MISC
        public string GetCharacterMain(string character)
        {
            return character == PlayerPrefs.GetString("CharacterFront", string.Empty) ? "MAIN" : "PARTY";
        }

        public char get_Rank(int unitPower)
        {
            if (unitPower >= 10000) return 'S';
            else if (unitPower >= 8000) return 'A';
            else if (unitPower >= 5000) return 'B';
            else if (unitPower >= 2500) return 'C';
            else if (unitPower >= 1000) return 'D';
            else if (unitPower > 0) { return 'E'; }
            else { return 'F'; }
        }

        public bool IsFormationHasSet()
        {
            StatsDistribution formationManage = new StatsDistribution();
            formationManage.load_Stats();
            foreach (ClassBase character in formationManage.slot_Stats) if (character.icon != null) return true;
            return false;
        }

        public IEnumerator PromptMessage(string message)
        {
            selectionPanel.transform.GetChild(14).gameObject.SetActive(true);
            selectionPanel.transform.GetChild(14).GetChild(0).GetComponent<Text>().text = message;
            yield return new WaitForSeconds(2);
            selectionPanel.transform.GetChild(14).gameObject.SetActive(false);
        }
        #endregion
    }
}
