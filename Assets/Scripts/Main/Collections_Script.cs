using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MeloMelo_ExtraComponent;

public class Collections_Script : MonoBehaviour
{
    public static Collections_Script thisCollect;

    private GameObject[] BGM;
    private string index;
    private bool transitionTime = false;

    public GameObject Selection;
    public GameObject Selection_Music;

    public GameObject[] AlertSign;
    public GameObject CharacterInfo;

    private MusicAlbum musicDatabase = new MusicAlbum();
    private UnitFormation_Manage unitParty = new UnitFormation_Manage();
    private CharacterAlbum list = new CharacterAlbum();
    public CharacterAlbum get_list { get { return list; } }

    private string ResMelo = string.Empty;
    public Dropdown AreaOption;
    private AreaInfo[] MusicList = null;

    public GameObject PopUpMusic;
    public GameObject MusicSelection_BackBtn;

    // Component System: Start-up
    private void BGM_Loader()
    {
        BGM = GameObject.FindGameObjectsWithTag("BGM");
        if (BGM.Length > 1) { for (int i = 1; i < BGM.Length; i++) { Destroy(BGM[i]); } }
    }

    private void LoadPackage_Content()
    {
        MusicList = Resources.LoadAll<AreaInfo>("Database_Area");

        list.Update_CharacterAlbum();
        //musicDatabase.Update_MusicAlbum(MusicList[0].AreaName);
    }

    // Program: Collection Scene
    void Start()
    {
        thisCollect = this;

        // Load Content: Intit
        LoadPackage_Content();

        // Check Program Start: Intit
        ResMelo = PlayerPrefs.GetString("Resoultion_Melo", string.Empty);
        Selection.GetComponent<Animator>().SetTrigger("Opening" + ResMelo);

        // System Component: Intit
        BGM_Loader();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !transitionTime && index != "MusicSelect")
        {
            switch (index)
            {
                case "CharacterSelect":
                    GameObject.Find("Selection_Character").GetComponent<Animator>().SetTrigger("Closing" + ResMelo);
                    break;

                default:
                    GameObject.Find("Selection").GetComponent<Animator>().SetTrigger("Closing" + ResMelo);
                    break;
            }

            Invoke("ReturnToSelection", 1.5f);
        }

        if (Selection_Music.activeInHierarchy && !transitionTime)
        {
           // if (Input.GetAxis("Mouse ScrollWheel") < 0)
           // { musicDatabase.Update_MusicLoader(1, false); musicDatabase.UpdateSelectMusic(AreaOption.options[AreaOption.value].text); }
//else if (Input.GetAxis("Mouse ScrollWheel") > 0)
           // { musicDatabase.Update_MusicLoader(-1, false); musicDatabase.UpdateSelectMusic(AreaOption.options[AreaOption.value].text); }
        }
    }

    void ReturnToSelection()
    { 
        if (Selection_Music.activeInHierarchy) { Selection_Music.SetActive(false); }

        if (Selection.activeInHierarchy) { SceneManager.LoadScene("Ref_PreSelection"); }
        else { Selection.SetActive(true); Selection.GetComponent<Animator>().SetTrigger("Opening"); }
    }

    // Button Transition: Selection Function
    public void Collection_SelectUI(string _index)
    {
        transitionTime = true;
        GameObject.Find("Selection").GetComponent<Animator>().SetTrigger("Closing" + ResMelo);
        index = _index;
        Invoke("Collection_SelectUI_E", 1.5f);
    }

    protected void Collection_SelectUI_E()
    {
        Selection.SetActive(false);

        switch (index)
        {
            case "MusicSelect":
                Selection_Music.SetActive(true);
                GameObject.Find("Selection_Music").GetComponent<Animator>().SetTrigger("Opening" + ResMelo);
                transitionTime = false;
              //  StartCoroutine(musicDatabase.FinishedTransition_SelectMusic(AreaOption.options[AreaOption.value].text));

                //CheckingMusicListArea();
                break;

            case "UnitSelect":
                GameObject.Find("Selection_Unit").GetComponent<Animator>().SetTrigger("Opening" + ResMelo);
                StartCoroutine(unitParty.FinishedTransition_SelectUI());
                break;

            case "CharacterSelect":
                GameObject.Find("Selection_Character").GetComponent<Animator>().SetTrigger("Opening" + ResMelo);
                transitionTime = false;
                list.UpdateCharacterList();
                break;

            case "StorageBag":
                SceneManager.LoadScene("StoragePage");
                break;
        }
    }

    public void StartMusicPlayer(bool stop) { }//musicDatabase.StartMusicPlayer(stop, AreaOption.options[AreaOption.value].text); }   
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

    // Addons: Back Button for music selection
    public void MusicSelection_BackFunction() 
    {
        MusicSelect_BackFunctionVisble(false);
        GameObject.Find("Selection_Music").GetComponent<Animator>().SetTrigger("Closing" + ResMelo);
        Invoke("ReturnToSelection", 1.5f);
    }

    public void MusicSelect_BackFunctionVisble(bool visible)
    {
        if (visible) { MusicSelection_BackBtn.SetActive(true); }
        else { MusicSelection_BackBtn.SetActive(false); }
    }

    public void NavChanger_List() { list.NavChanger(); }

    // Unit Formation: Button Interactive
    public void UIButton_UnitFormation(int _index) { unitParty.UIButton_UnitFormation(_index); }

    // Unit Formation: Transition
    public void GoUnitFormation(int index)
    {
        if (PlayerPrefs.GetString("Collection_EditMode", "F") == "T")
        {
            PlayerPrefs.SetInt("SlotSelect_setup", index);
            PlayerPrefs.SetString("SlotSelect_lastSelect", SceneManager.GetActiveScene().name);
            GameObject.Find("Selection_Character").GetComponent<Animator>().SetTrigger("Closing" + ResMelo);
            Invoke("GoUnitFormation_encode", 2);
        }
    }

    public void GoUnitFormation_encode() { SceneManager.LoadScene("Ref_CharacterSelection"); }

    // AreaOption: Initizating
    void CheckingMusicListArea()
    {
        foreach (AreaInfo i in MusicList) { AreaOption.AddOptions(new List<string> { i.AreaName }); }
    }

    // AreaOption Change: Function
    public void ChangeMusicList(Text text)
    {
        //musicDatabase.Update_MusicAlbum(text.text);
       // StartCoroutine(musicDatabase.FinishedTransition_SelectMusic(AreaOption.options[AreaOption.value].text));

        if (AlertSign[0].activeInHierarchy) { AlertSign[0].transform.GetChild(1).GetComponent<Text>().text = "This music is available in \n" + text.text + " Area."; }
    }
}
