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

    [SerializeField] private CharacterAlbum_Base collection_characterAlbum;

    void Update()
    {
        if (Input.anyKeyDown && ContentTrackDashBoard.activeInHierarchy) ContentTrackDashBoard.SetActive(false);
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
        SubSelection[index].GetComponent<Animator>().SetTrigger("Opening" + ResMelo);

        switch (index)
        {
            case 1:
                collection_characterAlbum.LoadContent();
                break;

            case 2:
                if (album == null) album = new MusicAlbum();
                LoadMusicContent();
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
    #endregion

    [System.Serializable]
    public class CharacterAlbum_Base
    {
        [SerializeField] private GameObject selectionPanel;
        [SerializeField] private Texture NoneOfAbove;
        private List<SkillContainer> skill_listing_container;
        [SerializeField] private GameObject[] MasteryTabs;

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
            for (int i = 0; i < 3; i++)
            {
                selectionPanel.transform.GetChild(2).GetChild(5 + i).gameObject.SetActive(false);
                selectionPanel.transform.GetChild(8 + i).GetComponent<RawImage>().color = Color.white;
            }

            selectionPanel.transform.GetChild(2).GetChild(5 + index).gameObject.SetActive(true);
            selectionPanel.transform.GetChild(8 + index).GetComponent<RawImage>().color = Color.green;
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
        }
        #endregion

        #region MAIN (REBORN TAB)
        public void PickMasteryTab(int index)
        {
            PlayerPrefs.DeleteKey("MasteryShuffleTab");
            UpdateRebornTab();
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
        }
        #endregion

        #region COMPONENT (ALL TAB)
        private void UpdateStatsTab()
        {
            // Assign data information of level, experience through class base
            int currentLevel, currentExperience;
            currentLevel = Character_Database[characterToggleIndex - 1].level;
            currentExperience = Character_Database[characterToggleIndex - 1].experience;

            // Show that character icon is present in the class base
            selectionPanel.transform.GetChild(2).GetChild(1).GetComponent<Image>().enabled =
                Character_Database[characterToggleIndex - 1].icon != null;

            // Able to present character icon through current selection
            selectionPanel.transform.GetChild(2).GetChild(1).GetComponent<Image>().sprite =
                Character_Database[characterToggleIndex - 1].icon;

            // Fill in all character status information
            SetStatusInformation(0).text = "[ " + Character_Database[characterToggleIndex - 1].characterName + " ]";
            SetStatusInformation(1).text = "Class: " + characterStatus[characterToggleIndex - 1].GetCharacterStatus(currentLevel).GetClassName;

            SetStatusInformation(2).text = "STRENGTH (STR): " + characterStatus[characterToggleIndex - 1].GetCharacterStatus(currentLevel).GetStrength
                + (PlayerPrefs.GetInt("Permant_STR", 0) == 0 ? string.Empty : " (+" + PlayerPrefs.GetInt("Permant_STR", 0) + ")");

            SetStatusInformation(3).text = "VITALITY (VIT): " + characterStatus[characterToggleIndex - 1].GetCharacterStatus(currentLevel).GetVitality
                + (PlayerPrefs.GetInt("Permant_VIT", 0) == 0 ? string.Empty : " (+" + PlayerPrefs.GetInt("Permant_VIT", 0) + ")");

            SetStatusInformation(4).text = "MAGIC (MAG): " + characterStatus[characterToggleIndex - 1].GetCharacterStatus(currentLevel).GetMagic
                + (PlayerPrefs.GetInt("Permant_MAG", 0) == 0 ? string.Empty : " (+" + PlayerPrefs.GetInt("Permant_MAG", 0) + ")");

            SetStatusInformation(5).text = "BASE HEALTH: " + characterStatus[characterToggleIndex - 1].GetCharacterStatus(currentLevel).GetHealth
                + (PlayerPrefs.GetInt("Permant_HP", 0) == 0 ? " (+0)" : " (+" + PlayerPrefs.GetInt("Permant_HP", 0) + ")");

            selectionPanel.transform.GetChild(2).GetChild(2).GetComponent<Text>().text = "LEVEL: " +
                characterStatus[characterToggleIndex - 1].GetCharacterStatus(currentLevel).GetLevel;

            selectionPanel.transform.GetChild(2).GetChild(3).GetComponent<Text>().text = "EXP: " +
                currentExperience + "/" + characterStatus[characterToggleIndex - 1].GetCharacterStatus(currentLevel).GetExperience;

            selectionPanel.transform.GetChild(2).GetChild(4).GetComponent<Text>().text = "REBIRTH: " + PlayerPrefs.GetInt("Rebirth_Points", 0);
            UpdateSkillTab();
        }

        private void UpdateSkillTab()
        {
            string[] skill_key = { "_Primary_Skill", "_Secondary_Skill_1", "_Secondary_Skill_2", "_Secondary_Skill_3" };
            skill_listing_container.Clear();

            foreach (string skill_type in skill_key)
            {
                SkillContainer singleSkill = Resources.Load<SkillContainer>("Database_Skills/" + Character_Database[characterToggleIndex - 1].name +
                    skill_type);

                skill_listing_container.Add(singleSkill);
            }

            for (int id = 0; id < skill_listing_container.ToArray().Length; id++)
            {
                if (skill_listing_container[id]) SetSkillsInformation(id).texture = skill_listing_container[id].skillIcon;
                else SetSkillsInformation(id).texture = NoneOfAbove;
            }

            //UpdateRebornTab();
        }

        private void UpdateRebornTab()
        {
            if (!PlayerPrefs.HasKey("MasteryShuffleTab"))
            {
                for (int count = 0; count < MasteryTabs.Length; count++)
                {
                    int randomizeNumber = Random.Range(count + 0, count + 2);
                    PlayerPrefs.SetInt("Mastery" + (count + 1), randomizeNumber);
                    PlayerPrefs.SetInt("MasteryShuffleTab", 1);
                }
            }

            for (int index = 0; index < MasteryTabs.Length; index++)
            {
                string masteryRarity = Character_Database[characterToggleIndex - 1].level == 99 ? "Ultimate" : "Common";
                MasteryTabs[index].transform.GetChild(0).GetComponent<Text>().text = Resources.Load<MasteryContainer>("Database_Reborn_Cards/" +
                   Character_Database[characterToggleIndex - 1].name + "_" + masteryRarity + "_" + PlayerPrefs.GetInt("Mastery" + (index + 1), 1)).title;

                MasteryTabs[index].transform.GetChild(1).GetComponent<Text>().text = Resources.Load<MasteryContainer>("Database_Reborn_Cards/" +
                   Character_Database[characterToggleIndex - 1].name + "_" + masteryRarity + "_" + PlayerPrefs.GetInt("Mastery" + (index + 1), 1)).awardsTitle;
            }
        }
        #endregion

        #region MISC 
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
        #endregion
    }
}
