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

    public GameObject CharacterInfo;
    private List<ClassBase> Character_Database;
    private List<StatsManage_Database> characterStatus;
    private int characterToggleIndex = 1;

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
                LoadCharacterContent();
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

    #region CHARACTER LIST (SETUP)
    private void LoadCharacterContent()
    {
        const string GetCharacterIsNull = "None";
        characterStatus = new List<StatsManage_Database>();
        Character_Database = new List<ClassBase>();

        // Gather all available characters
        foreach (ClassBase character in Resources.LoadAll<ClassBase>("Character_Data"))
            if (character.characterName != GetCharacterIsNull) Character_Database.Add(character);

        // Get status format with all characters
        foreach (ClassBase character in Character_Database.ToArray()) 
            characterStatus.Add(new StatsManage_Database(character.name));

        // Update content with the data given
        Update_CharacterAlbum();
        UpdateCharacterList();
        NavChanger();
    }

    #region CHARACTER LIST (MAIN)
    public void NagivateCharacaterList(int add)
    {
        // Modify scroll value according to which type of selection
        SubSelection[(int)Sub_Selection_Order.CharacterList].transform.GetChild(3).GetComponent<Slider>().value += add;
        // Update nagivator content once modified
        NavChanger();
    }
    #endregion

    #region CHARACTER LIST (COMPONENT)
    private void Update_CharacterAlbum()
    {
        Slider scrollBar = SubSelection[(int)Sub_Selection_Order.CharacterList].transform.GetChild(3).GetComponent<Slider>();

        // Get character length display to user
        scrollBar.GetComponent<Slider>().maxValue = Character_Database.ToArray().Length;
        UpdateScrollBarDisplay();

        // Get information ready before used
        foreach (ClassBase i in Character_Database) { i.UpdateCurrentStats(false); }
    }
    #endregion

    private void UpdateCharacterList()
    {
        // Assign data information of level, experience through class base
        int currentLevel, currentExperience;
        currentLevel = Character_Database[characterToggleIndex - 1].level;
        currentExperience = Character_Database[characterToggleIndex - 1].experience;

        // Show that character icon is present in the class base
        CharacterInfo.transform.GetChild(2).GetChild(1).GetComponent<Image>().enabled =
            Character_Database[characterToggleIndex - 1].icon != null;

        // Able to present character icon through current selection
        CharacterInfo.transform.GetChild(2).GetChild(1).GetComponent<Image>().sprite =
            Character_Database[characterToggleIndex - 1].icon;

        // Fill in all character status information
        SetStatusInformation(2).text = "[ " + Character_Database[characterToggleIndex - 1].characterName + " ]";
        SetStatusInformation(3).text = "Class: " + characterStatus[characterToggleIndex - 1].GetCharacterStatus(currentLevel).GetClassName;
        SetStatusInformation(4).text = "STRENGTH: " + characterStatus[characterToggleIndex - 1].GetCharacterStatus(currentLevel).GetStrength;
        SetStatusInformation(5).text = "VITALITY: " + characterStatus[characterToggleIndex - 1].GetCharacterStatus(currentLevel).GetVitality;
        SetStatusInformation(6).text = "MAGIC: " + characterStatus[characterToggleIndex - 1].GetCharacterStatus(currentLevel).GetMagic;
        SetStatusInformation(7).text = "BASE HEALTH: " + characterStatus[characterToggleIndex - 1].GetCharacterStatus(currentLevel).GetHealth + " (+0)";
        SetStatusInformation(8).text = "LEVEL: " + characterStatus[characterToggleIndex - 1].GetCharacterStatus(currentLevel).GetLevel;
        SetStatusInformation(9).text = "EXP: " + currentExperience + "/" + characterStatus[characterToggleIndex - 1].GetCharacterStatus(currentLevel).GetExperience;
    }

    private void NavChanger()
    {
        Slider scrollBar = SubSelection[(int)Sub_Selection_Order.CharacterList].transform.GetChild(3).GetComponent<Slider>();

        // Update new index to loader
        characterToggleIndex = (int)scrollBar.value;
        UpdateScrollBarDisplay();

        // Check interaction to nagivator [LEFT]
        NagivatorSelector(1).interactable = scrollBar.value > 1;

        // Check interaction to nagivator [RIGHT]
        NagivatorSelector(2).interactable = scrollBar.value < Character_Database.ToArray().Length;

        // Get new information set from loader
        UpdateCharacterList();
    }

    private void UpdateScrollBarDisplay()
    {
        Slider scrollBar = SubSelection[(int)Sub_Selection_Order.CharacterList].transform.GetChild(3).GetComponent<Slider>();
        Text displaybar = scrollBar.transform.gameObject.transform.GetChild(0).GetComponent<Text>();
        displaybar.text = (int)scrollBar.value + "/" + (int)scrollBar.maxValue;
    }

    private Button NagivatorSelector(int index)
    {
        switch (index)
        {
            case 1:
                return SubSelection[(int)Sub_Selection_Order.CharacterList].transform.GetChild(4).GetComponent<Button>();

            default:
                return SubSelection[(int)Sub_Selection_Order.CharacterList].transform.GetChild(5).GetComponent<Button>();
        }
    }

    private Text SetStatusInformation(int index)
    {
        return CharacterInfo.transform.GetChild(2).GetChild(index).GetComponent<Text>();
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
}
