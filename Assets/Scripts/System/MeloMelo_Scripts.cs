 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Linq;

// MeloMelo: VirtualItem Component
namespace MeloMelo_VirtualItem
{
    public class InventoryManagement
    {

    }

    public class MiniStorageManagement
    {
        private string currentBoundItem;
        private bool itemOnUse;
        private List<GameObject> panels;

        #region SETUP
        public void SetupPanelBoundName(string name)
        {
            currentBoundItem = name;
        }

        public void AddReferencePanel(GameObject panel)
        {
            if (panels == null) panels = new List<GameObject>();
            panels.Add(panel);
        }
        #endregion

        #region MAIN
        public void UseItemFromStorage(string itemName)
        {
            itemOnUse = false;

            if (PlayerPrefs.GetString(currentBoundItem, string.Empty) == itemName)
            {
                // Check for exp potion and update amount uses
                VirtualItemDatabase itemFromStorage = MeloMelo_GameSettings.GetAllItemFromLocal(itemName);
                if (itemFromStorage.itemName == itemName && GetTotalAmountFromStorage(itemName) > 0)
                {
                    PlayerPrefs.DeleteKey(currentBoundItem);
                    MeloMelo_ItemUsage_Settings.SetItemUsed(itemFromStorage.itemName);
                    itemOnUse = true;
                }
            }
            else PlayerPrefs.SetString(currentBoundItem, itemName);
        }
        #endregion

        #region COMPONENT
        private int GetTotalAmountFromStorage(string itemName)
        {
            // Items counted in storage bag and current used items
            int itemAmount = MeloMelo_GameSettings.GetAllItemFromLocal(itemName).amount -
                MeloMelo_ItemUsage_Settings.GetItemUsed(itemName);

            return itemAmount;
        }
        #endregion

        #region MISC
        public string GetPanelItemInfo(string itemName) { return itemName + " ( x " + GetTotalAmountFromStorage(itemName) + " )"; }
        public bool CurrentlyInUse() { return itemOnUse; }
        #endregion
    }
}

// MeloMelo: Story Mode Component
namespace MeloMelo_Story
{
    public class StoryTool
    {
        public void StoryTxtExtraction(string storyRef)
        {
            string readTxtFile = Application.streamingAssetsPath + "/Story/" + storyRef + ".txt";
            List<string> fileLine = File.ReadAllLines(readTxtFile).ToList();
            string txt_printer = string.Empty;

            foreach (string i in fileLine) { txt_printer += (i + "\n"); }
            PlayerPrefs.SetString("Display_Story", txt_printer);
            StoryMode_Scripts.thisStory.StoryLoaderPlayer(storyRef);
        }

        public IEnumerator StoryTxtEffect()
        {
            float speed = 0.07f;

            foreach (char i in PlayerPrefs.GetString("Display_Story"))
            {
                StoryMode_Scripts.thisStory.StoryDisplayTxt(i);
                yield return new WaitForSeconds(speed);
            }
           
            StoryMode_Scripts.thisStory.StoryDisplayContinue();
        }
    }
}

// MeloMelo: Extra Componenet
namespace MeloMelo_ExtraComponent
{
    public class ScoreFixedValue
    {
        public float score_combo()
        {
            float scoreMax = BeatConductor.thisBeat.get_scorePerfect2;
            PlayerPrefs.SetInt("MissCP", (PlayerPrefs.GetInt("MissCP") - 1));

            float i = 0;
            //float j = GameManager.thisManager.get_perfect2_count + GameManager.thisManager.get_perfectCount + GameManager.thisManager.get_badCount + GameManager.thisManager.get_missCount;

            if (GameManager.thisManager.getJudgeWindow.getCombo + 1 >= GameManager.thisManager.getJudgeWindow.getMaxCombo)
                i = 0;
            else
                i = (GameManager.thisManager.getJudgeWindow.getCombo - GameManager.thisManager.getJudgeWindow.getMaxCombo) * 4;

            if (scoreMax + i < 0)
            {
               // MeloMelo_ScoreSystem.thisSystem.ReceivedComboPenatly(GameManager.thisManager.getJudgeWindow.getCombo - GameManager.thisManager.getJudgeWindow.getMaxCombo);
                return GameManager.thisManager.getJudgeWindow.getCombo - GameManager.thisManager.getJudgeWindow.getMaxCombo;
            }

            else
            {
                //MeloMelo_ScoreSystem.thisSystem.ReceivedComboPenatly(i);
                return i;
            }
        }
    }

    public class QuickLook
    {
        private int currentPage = 0;
        private string PageIndex = string.Empty;

        public void Opening_QuickLook (string index) { PageIndex = index; GameObject.Find(PageIndex).GetComponent<Animator>().SetTrigger("Opening" + PlayerPrefs.GetString("Resoultion_Melo", string.Empty)); }
        public void CloseFunction_NoticePlay(string updatePlayerPrefs)
        { if (updatePlayerPrefs != string.Empty) { PlayerPrefs.SetString(updatePlayerPrefs, "F"); } GameObject.Find(PageIndex).GetComponent<Animator>().SetTrigger("Closing" + PlayerPrefs.GetString("Resoultion_Melo", string.Empty)); }

        public void NoticePlay(GameObject[] obj, GameObject[] buttonUI)
        {
            if (currentPage < obj.Length - 1) { buttonUI[0].SetActive(true); buttonUI[1].SetActive(false); }
            else { buttonUI[0].SetActive(false); buttonUI[1].SetActive(true); }

            foreach (GameObject i in obj) { i.SetActive(false); }
            obj[currentPage].SetActive(true);
        }

        public void NextFunction_NoticePlay(GameObject[] obj, GameObject[] buttonUI)
        {
            currentPage++;
            NoticePlay(obj, buttonUI);
        }
    }

    public class CharacterAlbum
    {
        private List<ClassBase> Character_Database;
        public ClassBase[] get_CharacterData { get { return Character_Database.ToArray(); } }
        private int charLoader;

        public CharacterAlbum()
        {
            Character_Database = new List<ClassBase>();
            charLoader = 1;
        }

        #region SETUP
        public void Update_CharacterAlbum()
        {
            //// Add character into album
            //foreach (ClassBase character in Resources.LoadAll<ClassBase>("Character_Data"))
            //if (character.characterName != "None") Character_Database.Add(character);

            //// Get character length display to user
            //GameObject.Find("ScrollBar_List").GetComponent<Slider>().maxValue = Character_Database.ToArray().Length;
            //GameObject.Find("ScrollBar_List").transform.GetChild(0).GetComponent<Text>().text = 
            //    (int)GameObject.Find("ScrollBar_List").GetComponent<Slider>().value + "/" + 
            //    (int)GameObject.Find("ScrollBar_List").GetComponent<Slider>().maxValue;
            
            //// Update All Character Stats
            //foreach (ClassBase i in Character_Database) { i.UpdateCurrentStats(true); }
        }
        #endregion

        #region MAIN
        public void UpdateCharacterList()
        {
            //// Enable character icon
            //CollectionNew_Script.thisCollect.CharacterInfo.transform.GetChild(2).GetChild(1).GetComponent<Image>().enabled = 
            //    Character_Database[charLoader - 1].icon != null;

            //// Read and fill character icon
            //CollectionNew_Script.thisCollect.CharacterInfo.transform.GetChild(2).GetChild(1).GetComponent<Image>().sprite = 
            //    Character_Database[charLoader - 1].icon;

            //// Character status information
            //SetStatusInformation(2).text = "[ " + Character_Database[charLoader - 1].characterName + " ]";
            //SetStatusInformation(3).text = "Class: " + Character_Database[charLoader - 1].name;
            //SetStatusInformation(4).text = "STRENGTH: " + Character_Database[charLoader - 1].Strength;
            //SetStatusInformation(5).text = "VITALITY: " + Character_Database[charLoader - 1].vitality;
            //SetStatusInformation(6).text = "MAGIC: " + Character_Database[charLoader - 1].MAG;
            //SetStatusInformation(7).text = "BASE HEALTH: " + Character_Database[charLoader - 1].CHP + "(+0)";
            //SetStatusInformation(8).text = "LEVEL: " + Character_Database[charLoader - 1].level;
            //SetStatusInformation(9).text = "EXP: " + Character_Database[charLoader - 1].experience + "/" + Character_Database[charLoader - 1].expLimit;
        }

        public void NavChanger()
        {
            //// Update new index to loader
            //charLoader = (int)GetScrollBar().value;
            //GetScrollBar_DisplayScore().text = (int)GetScrollBar().value + "/" + (int)GetScrollBar().maxValue;

            //// Check interaction to nagivator [LEFT]
            //if (GetScrollBar().value == 1) { NagivatorSelector(1).interactable = false; }
            //else { NagivatorSelector(1).interactable = true; }

            //// Check interaction to nagivator [RIGHT]
            //if (GetScrollBar().value >= Character_Database.ToArray().Length) { NagivatorSelector(2).interactable = false; }
            //else { NagivatorSelector(2).interactable = true; }

            //// Get new information set from loader
            //UpdateCharacterList();
        }
        #endregion

        #region COMPONENT
        private Button NagivatorSelector(int index)
        {
            switch (index)
            {
                case 1:
                    return GameObject.Find("LeftNav").GetComponent<Button>();

                case 2:
                    return GameObject.Find("RightNav").GetComponent<Button>();

                default:
                    return null;
            }
        }

        private Slider GetScrollBar()
        {
            return GameObject.Find("ScrollBar_List").GetComponent<Slider>();
        }

        private Text GetScrollBar_DisplayScore()
        {
            return GameObject.Find("ScrollBar_List").transform.GetChild(0).GetComponent<Text>();
        }

        //private Text SetStatusInformation(int index)
        //{
        //    return CollectionNew_Script.thisCollect.CharacterInfo.transform.GetChild(2).GetChild(index).GetComponent<Text>();
        //}
        #endregion
    }

    public class UnitFormation_Manage
    {
        private GameObject[] UnitSlot = new GameObject[3];
        private MeloMelo_RPGEditor.StatsDistribution stats = new MeloMelo_RPGEditor.StatsDistribution();

        public IEnumerator FinishedTransition_SelectUI()
        {
            yield return new WaitForSeconds(1.5f);
            stats.load_Stats();
            UnitSlot = GameObject.FindGameObjectsWithTag("Slot");
            UnitSlot_Check();
        }

        public void UnitSlot_Check()
        {
            foreach (GameObject slot in UnitSlot)
            {
                if (PlayerPrefs.GetString(slot.name + "_charName", "None") != "None")
                {
                    slot.transform.GetChild(0).GetComponent<Text>().enabled = false;
                    if (!slot.transform.GetChild(1).GetComponent<Image>().enabled) { slot.transform.GetChild(1).GetComponent<Image>().enabled = true; }
                    slot.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("Character_Data/" + PlayerPrefs.GetString(slot.name + "_charName", "None"));

                    if (PlayerPrefs.GetString("CharacterFront", "NA") == PlayerPrefs.GetString(slot.name + "_charName", "None")) { GameObject.Find(slot.name).transform.GetChild(2).GetChild(0).GetComponent<Text>().text = "MAIN"; }
                    else { GameObject.Find(slot.name).transform.GetChild(2).GetChild(0).GetComponent<Text>().text = "PARTY"; }
                }
                else
                {
                    slot.transform.GetChild(0).GetComponent<Text>().enabled = true;
                    slot.transform.GetChild(1).GetComponent<Image>().enabled = false;
                    slot.transform.GetChild(2).gameObject.SetActive(false);
                }
            }

            GameObject.Find("UNIT STATUS").transform.GetChild(0).GetComponent<Text>().text = "UNIT POWER:\n" + stats.get_UnitPower("Character");
            try { GameObject.Find("UNIT STATUS 2").transform.GetChild(0).GetComponent<Text>().text = "UNIT RANK:\n" + get_Rank(); } catch { }
            try { GameObject.Find("UNIT STATUS 3").transform.GetChild(0).GetComponent<Text>().text = "HEALTH:\n" + stats.get_UnitHealth("Character"); } catch { }
            try { for (int i = 0; i < 3; i++) { if (stats.slot_Stats[i].name != "None") { GameObject.FindGameObjectWithTag("SetupCompleted").GetComponent<Button>().interactable = true; break; } else { GameObject.FindGameObjectWithTag("SetupCompleted").GetComponent<Button>().interactable = false; } } }
            catch { }
        }

        protected char get_Rank()
        {
            if (stats.get_UnitPower("Character") >= 10000) return 'S';
            else if (stats.get_UnitPower("Chracter") >= 8000) return 'A';
            else if (stats.get_UnitPower("Character") >= 5000) return 'B';
            else if (stats.get_UnitPower("Character") >= 2500) return 'C';
            else if (stats.get_UnitPower("Character") >= 1000) return 'D';
            else if (stats.get_UnitPower("Character") > 0) { return 'E'; }
            else { return 'F'; }
        }

        // Get index from Unit Manage
        public void UIButton_UnitFormation(int _index)
        {
            switch (_index)
            {
                case 1:
                    PlayerPrefs.SetString("Collection_EditMode", "T");
                    GameObject.Find("EditBtn").GetComponent<Button>().interactable = false;
                    break;

                case 2:
                    PlayerPrefs.DeleteKey("Collection_EditMode");
                    GameObject.Find("Selection_Unit").GetComponent<Animator>().SetTrigger("Closing");
                    Collections_Script.thisCollect.Invoke("ReturnToSelection", 1.5f);
                    break;

                default:
                    break;
            }
        }
    }

    public class MusicAlbum
    {
        private const string coverImageTemplateFile = "Database_Area/CoverImage_Template";
        private const string pathFolder = "Database_Area";

        private string currentAreaText;
        private int totalTrackInGame = 0;
        private int seasonRegistered = 0;

        private GameObject MusicPlayer = null;
        public GameObject get_musicplayer { get { return MusicPlayer; } }

        private List<AreaInfo> areaRegister;
        private List<int> areaLength;
        private bool isDoneRegistering;

        public MusicAlbum()
        {
            areaRegister = new List<AreaInfo>();
            areaLength = new List<int>();
            isDoneRegistering = false;

            MusicPlayer = GameObject.Find("BGM_MusicAlbum");
            try { seasonRegistered = StartMenu_Script.thisMenu.get_seasonNum; } catch { seasonRegistered = 3; }
        }

        #region SETUP
        public IEnumerator RegisterAllActiveArea()
        {
            AreaInfo area = null;
            int count = 0;

            for (int season = 0; season < seasonRegistered + 1; season++)
            {
                do
                {
                    count++;
                    ResourceRequest loadRequest = Resources.LoadAsync<AreaInfo>(pathFolder + "/Season" + season + "/A" + count);
                    yield return new WaitUntil(() => loadRequest.isDone);

                    area = loadRequest.asset as AreaInfo;
                    if (area != null) areaRegister.Add(area);

                } while (area != null);

                areaLength.Add(count);
                count = 0;
            }

            isDoneRegistering = true;
        }

        private void CalculateTotalMusicInGame()
        {
            List<string> options = new List<string>();

            for (int area = 0; area < areaRegister.ToArray().Length; area++)
            {
                totalTrackInGame += ImplementTotalMusic(areaRegister[area].AreaName);
                options.Add(areaRegister[area].AreaName);
            }

            GameObject.Find("AreaOptions").GetComponent<Dropdown>().AddOptions(options);
            UpdateDropDownContent();
        }

        private IEnumerator GetTrackByArea(string area)
        {
            for (int track = 0; track < ImplementTotalMusic(area); track++)
            {
                ResourceRequest loadrequest = Resources.LoadAsync<MusicScore>(pathFolder + "/" + area + "/M" + (track + 1));
                yield return new WaitUntil(() => loadrequest.isDone);

                MusicScore content = loadrequest.asset as MusicScore;
                if (content != null)
                {
                    Texture trackContent = content.Background_Cover;
                    bool restrictContent = content.SetRestriction;

                    loadrequest = Resources.LoadAsync<RawImage>(coverImageTemplateFile);
                    yield return new WaitUntil(() => loadrequest.isDone);

                    RawImage coverImage = loadrequest.asset as RawImage;
                    if (coverImage != null)
                    {
                        RawImage coverImage_template = GameObject.Instantiate(coverImage, GameObject.FindGameObjectWithTag("TrackList").transform);
                        coverImage_template.texture = trackContent;

                        coverImage_template.gameObject.GetComponent<TrackContentNagivator>().trackIndex = track + 1;
                        coverImage_template.gameObject.GetComponent<TrackContentNagivator>().contentLocked = restrictContent;
                    }
                }
            }
        }
        #endregion

        #region COMPONENT
        private MusicScore CurrentArea_Data(int index, string currentArea)
        {
            return Resources.Load<MusicScore>(pathFolder + "/" + currentArea + "/M" + index);
        }

        private int ImplementTotalMusic(string areaName)
        {
            int total = 0;

            foreach (AreaInfo area in areaRegister.ToArray())
            {
                if (area.AreaName == areaName)
                {
                    total = area.totalMusic;
                    return total;
                }
            }

            return total;
        }

        private void ClearTrackContent()
        {
            CollectionNew_Script.thisCollect.LoadingScreen_TrackContent.SetActive(true);

            for (int i = 0; i < GameObject.FindGameObjectWithTag("TrackList").transform.childCount; i++)
                GameObject.Destroy(GameObject.FindGameObjectWithTag("TrackList").transform.GetChild(i).gameObject);
        }

        private Text SetTrackInformation(int index)
        {
            return CollectionNew_Script.thisCollect.ContentTrackDashBoard.transform.GetChild(index).GetComponent<Text>();
        }
        #endregion

        #region MAIN
        public void UpdateSelectMusic(int index)
        {
            // Filling content information
            SetTrackInformation(0).text = "[ " + CurrentArea_Data(index, currentAreaText).ArtistName + " ]";
            SetTrackInformation(1).text = CurrentArea_Data(index, currentAreaText).Title;
            SetTrackInformation(2).text = "Imported by " + CurrentArea_Data(index, currentAreaText).DesignerName;
            CollectionNew_Script.thisCollect.ContentTrackDashBoard.transform.GetChild(3).GetComponent<RawImage>().texture = CurrentArea_Data(index, currentAreaText).Background_Cover;

            SetTrackInformation(5).text = CurrentArea_Data(index, currentAreaText).creditPoint != "--" ? SetTrackInformation(5).text = "© " + CurrentArea_Data(index, currentAreaText).creditPoint :
                SetTrackInformation(5).text = "";

            // Track Locked
            CollectionNew_Script.thisCollect.ContentTrackDashBoard.transform.GetChild(3).GetChild(0).gameObject.SetActive(CurrentArea_Data(index, currentAreaText).SetRestriction);
        }

        public IEnumerator FinishedTransition_SelectMusic()
        {
            yield return new WaitUntil(() => isDoneRegistering);
            CalculateTotalMusicInGame();
            CollectionNew_Script.thisCollect.LoadingScreen_TrackContent.SetActive(false);
        }

        public void UpdateDropDownContent()
        {
            currentAreaText = GameObject.Find("AreaOptions").transform.GetChild(0).GetComponent<Text>().text;
            ClearTrackContent();

            CollectionNew_Script.thisCollect.StartCoroutine(GetTrackByArea(currentAreaText));
            GameObject.Find("MusicCount").transform.GetComponent<Text>().text = ImplementTotalMusic(currentAreaText) + "/" + totalTrackInGame;
            CollectionNew_Script.thisCollect.LoadingScreen_TrackContent.SetActive(false);
        }
        #endregion

        // Get Player Music Playing
        public void StartMusicPlayer(bool stop)
        {
            if (!stop)
            {
                MusicPlayer.GetComponent<AudioSource>().clip = CurrentArea_Data(0, currentAreaText).Music;
                CollectionNew_Script.thisCollect.Popup_Notication(true, CurrentArea_Data(0, currentAreaText).Title);

                GameObject.Find("PlayBtn").GetComponent<Button>().interactable = false;
                GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>().mute = true;
                MusicPlayer.GetComponent<AudioSource>().Play();
                CollectionNew_Script.thisCollect.StartCoroutine(MusicPlaying());
            }
            else
            {
                CollectionNew_Script.thisCollect.Popup_Notication(false, "-----");

                GameObject.Find("PlayBtn").GetComponent<Button>().interactable = true;
                MusicPlayer.GetComponent<AudioSource>().Stop();
                GameObject.FindGameObjectWithTag("BGM").GetComponent<AudioSource>().mute = false;
            }
        }

        // Stop Player from Playing
        protected IEnumerator MusicPlaying()
        {
            yield return new WaitUntil(() => !MusicPlayer.GetComponent<AudioSource>().isPlaying);
            StartMusicPlayer(true);
        }
    }
}

// MeloMelo: Editor Build Structure
namespace MeloMelo_EditorBuild
{
    public interface IGameManager
    {
        //float get_playBorder { get; }
        //float get_limitBorder { get; }

        //int get_perfectCount { get; }
        //int get_badCount { get; }
        //int get_missCount { get; }
        //int get_maxComboCount { get; }

        //int get_overallCombo { get; }
        //int get_overallEnemy { get; }
        //int get_overallTraps { get; }
        //float get_battleProgress { get; }

        //void PlayArea_Update(string area);
        void GameStarting();

        void UpdateScore(float _score);
        void UpdateScore_Tech(int _score);
        void UpdateNoteStatus(string index);

        void UpdateCharacter_Health(int amount, bool setHP);
        void UpdateEnemy_Health(int amount, bool setHP);

        void UpdateBattle_Progress(float amount);
    }

    public interface ICharacter
    {
        IEnumerator Get_Jump();
    }
}

// MeloMelo: Score Printer
namespace MeloMelo_ScoreEditorMode
{
    // Unit: Score Generator
    public class ScorePrinter
    {
        public void SongTitle(string title)
        {
            StreamWriter myData = new StreamWriter("Assets/Resources/ScoreTxt.txt", true);
            myData.WriteLine("Song Title: " + title);
            myData.Close();
        }

        public void ScoringWriter()
        {
            StreamWriter myData = new StreamWriter("Assets/Resources/ScoreTxt.txt", true);
            int j = 0;

            for (int i = 0; i < BeatConductor.thisBeat.get_spawnObject; i++)
            {
                myData.Write(BeatConductor.thisBeat.get_myData_Note[i] + ", ");
                if (j > 24) { myData.Write("\n"); j = 0; }
                else { j++; }
            }

            myData.WriteLine("");
            myData.Close();
            Debug.Log("Score have been created! You can stop running now.");
        }
    }
}

// MeloMelo: Build Level Indicator
namespace MeloMelo_LevelBuilder
{
    // Unit: ScoringMeter Function
    public class ScoringMeter
    {
        // Memory Usage: ScoringMeter
        private int[] get_comboDiff = { 1, 50, 100, 200 };
        private int[] get_comboDiff2 = { 1, 100, 200, 300 };

        private int noteCom = 0;
        private int noteCom_enemy = 0;
        private int noteCom_trap = 0;
        private int noteCom_all = 0;
        private int specialItem_count = 0;

        private bool[] maxCom = new bool[4];
        private bool[] maxCom_enemy = new bool[2];
        private bool[] maxCom_trap = new bool[2];
        private bool[] maxCom_all = new bool[3];
        private int[] noteType = new int[4];
        private bool trapDetected = false;
        private bool trapDetected2 = false;
        private bool enemyAir = false;
        private bool attackAir = false;
        private bool fullAttack = false;
        private bool allTraps = false;
        private bool specialitem = false;

        private float difficultyLevel = 0;
        public float get_difficultyLevel { get { return difficultyLevel; } }
        private float scoringCurrent = 0;

        // Memory Usage: Level SetUp
        private int EnemyCounter = 0;
        private int TrapCounter = 0;
        private int maxCombo = 0;

        // One-time Setup: Function
        public ScoringMeter(int[] score_database, int[] score_database2)
        {
            LevelSetup_Program(score_database, score_database2);
        }

        public ScoringMeter(int[] score_database, int[] score_database2, int[] score_database3)
        {
            LevelSetup_Program(score_database, score_database2);
            LevelSetup_ExtraProgram(score_database3);
        }

        public ScoringMeter(int difficultyMap, int numberofEnemy, int numberofTraps, int maxCombo, int[] data)
        {
            SetUp(difficultyMap, numberofEnemy, numberofTraps, maxCombo, data);
        }

        // Score Function: Level Meter
        protected void SetUp(int difficultyMap, int numberofEnemy, int numberofTraps, int maxCombo, int[] data)
        {
            difficultyLevel = 0;
            scoringCurrent = 0;
            trapDetected = false;
            trapDetected2 = false;
            enemyAir = false;
            attackAir = false;
            fullAttack = false;
            allTraps = false;
            specialitem = false;

            for (int i = 0; i < maxCom.Length; i++) { maxCom[i] = false; }
            for (int i = 0; i < maxCom_enemy.Length; i++) { maxCom_enemy[i] = false; }
            for (int i = 0; i < maxCom_trap.Length; i++) { maxCom_trap[i] = false; }
            for (int i = 0; i < maxCom_all.Length; i++) { maxCom_all[i] = false; }
            for (int i = 0; i < noteType.Length; i++) { noteType[i] = 0; }

            PlayerPrefs.SetInt("GetMaxPoint_" + difficultyMap, maxCombo * 3);

            try
            {
                if (GameManager.thisManager.DeveloperMode)
                {
                    Debug.Log("Traps: " + numberofTraps);
                    Debug.Log("Enemy: " + numberofEnemy);
                    Debug.Log("Combo: " + maxCombo);
                }
                else
                {
                    Debug.Log("Total Combo: " + maxCombo);
                }
            }
            catch { }

            // Season 3: Scoring
            try
            {
                if (SelectionMenu_Script.thisSelect.get_selection.get_form.NewChartSystem)

                    if (SelectionMenu_Script.thisSelect.get_selection.get_form.UltimateAddons)
                        if (SelectionMenu_Script.thisSelect.get_selection.get_form.ScaleLevel < 4) 
                            foreach (int i in get_comboDiff) { if (maxCombo >= i) { scoringCurrent += 7.2f; } } // S1: 7.2
                        else
                            foreach (int i in get_comboDiff2) { if (maxCombo >= i) { scoringCurrent += 7.2f; } } // S1: 7.2

                    else if (SelectionMenu_Script.thisSelect.get_selection.get_form.addons3 != null)
                        foreach (int i in get_comboDiff2) { if (maxCombo >= i) { scoringCurrent += 7.2f; } } // S1: 7.2

                    else
                        foreach (int i in get_comboDiff) { if (maxCombo >= i) { scoringCurrent += 7.2f; } } // S1: 7.2
                else

                    foreach (int i in get_comboDiff) { if (maxCombo >= i) { scoringCurrent += 7.2f; } } // S1: 7.2
            }
            catch 
            {
                foreach (int i in get_comboDiff) { if (maxCombo >= i) { scoringCurrent += 7.2f; } } // S1: 7.2
            }

            // Season 0: Scoring
            foreach (int i in data) { if (i == 3 && !trapDetected) { Debug.Log("Ground Trap: YES"); trapDetected = true; scoringCurrent+=2; } } // S0: 3

            // Season 1: Scoring
            foreach (int i in data) { if (i == 7 && !trapDetected2) { Debug.Log("Air Trap: YES"); trapDetected2 = true; scoringCurrent += 3; } } // S1: 5
            foreach (int i in data) { if (i == 6 && !enemyAir) { Debug.Log("Air Enemy: YES"); enemyAir = true; scoringCurrent += 2; } } // S1: 2.5

            // Season 2: Scoring
            foreach(int i in data) { if (trapDetected && trapDetected2 & !allTraps) { Debug.Log("All Trap: YES"); allTraps = true; scoringCurrent += 1; } }
            foreach (int i in data) { if (i == 9 && !attackAir) { Debug.Log("Air Attack: YES"); attackAir = true; scoringCurrent += 0.5f; } }
            foreach (int i in data) { if (i == 5 && attackAir && !fullAttack) { Debug.Log("Full Attack: YES"); fullAttack = true; scoringCurrent += 2; } }

            // Season 3: Scoring
            foreach (int i in data) { if (i == 93 && !specialitem) { specialitem = true; Debug.Log("Special Item: YES"); scoringCurrent -= 4.5f; } }
            foreach (int i in data)
            {
                if (i == 93)
                {
                    specialItem_count++;

                    switch (specialItem_count)
                    {
                        case 4:
                            scoringCurrent += 1.5f;
                            specialItem_count = 0;
                            break;

                        case 3:
                            scoringCurrent += 1;
                            specialItem_count = 0;
                            break;

                        case 2:
                            scoringCurrent += 0.5f;
                            specialItem_count = 0;
                            break;
                    }
                }
                else { specialItem_count = 0; }
            }

            // All Season: Scoring
            foreach (int i in data)
            {
                // Enemy
                if (i == 1)
                {
                    noteCom_enemy++;

                    switch (noteCom_enemy)
                    {
                        case 3:
                            if (!maxCom_enemy[0]) { Debug.Log("Group Enemy: YES"); maxCom_enemy[0] = true; scoringCurrent += 2; }
                            break;

                        case 4:
                            if (!maxCom_enemy[1]) { Debug.Log("Group Enemy 2: YES"); maxCom_enemy[1] = true; scoringCurrent += 5; }
                            break;
                    }
                }
                else { noteCom_enemy = 0; }

                // Trap (Ground/Air)
                if (i == 3 || i == 7)
                {
                    noteCom_trap++;
                    switch (noteCom_trap)
                    {
                        case 3:
                            if (!maxCom_trap[0]) { Debug.Log("Small Trap: YES"); maxCom_trap[0] = true; scoringCurrent += 3; }
                            break;

                        case 4:
                            if (!maxCom_trap[1]) { Debug.Log("Massive Trap: YES"); maxCom_trap[1] = true; scoringCurrent += 8; }
                            break;
                    }
                }
                else { noteCom_trap = 0; }

                // All Element
                if (i != 0)
                {
                    bool check = false;
                    int index = 0;

                    for (int j = 0; j < noteType.Length; j++)
                    {
                        if (noteType[j] == i)
                        {
                            check = true;
                            for (int k = 0; k < noteType.Length; k++) { noteType[k] = 0; }
                        }
                        if (noteType[j] == 0) { index = j; break; }
                    }

                    if (!check) { noteType[index] = i; noteCom_all++; }
                    else { noteCom_all = 0; }

                    switch (noteCom_all)
                    {
                        case 3:
                            if (!maxCom_all[0]) { Debug.Log("Creation 3/4: YES"); maxCom_all[0] = true; scoringCurrent += 10.2f; }
                            break;

                        case 4:
                            if (!maxCom_all[1]) { Debug.Log("Creation 4/4: YES" + i); maxCom_all[1] = true; scoringCurrent += 14.5f; }
                            break;

                        case 5:
                            if (!trapDetected2 && !enemyAir && !maxCom_all[2]) { Debug.Log("Creation 5/4: YES"); maxCom_all[2] = true; scoringCurrent += 5; }
                            break;
                    }
                }
                else { noteCom_all = 0; }

                // Mapped note
                if (i != 0)
                {
                    noteCom++;

                    switch (noteCom)
                    {
                        case 2:
                            if (!maxCom[0]) { Debug.Log("Mapped 2/4: YES"); maxCom[0] = true; scoringCurrent+=10; }
                            break;

                        case 3:
                            if (!maxCom[1]) { Debug.Log("Mapped 3/4: YES"); maxCom[1] = true; scoringCurrent+=12.2f; }
                            break;

                        case 4:
                            if (!maxCom[2]) { Debug.Log("Mapped 4/4: YES"); maxCom[2] = true; scoringCurrent+=14; }
                            break;

                        case 5:
                            if (!trapDetected2 && !enemyAir && !maxCom[3]) { Debug.Log("Mapped 5/4: YES"); maxCom[3] = true; scoringCurrent += 0; } // 2.5f
                            break;
                    }
                }
                else { noteCom = 0; }
            }

            difficultyLevel = 0.15f * scoringCurrent;

            if (SceneManager.GetActiveScene().name == "ArenaSelection" || SceneManager.GetActiveScene().name == "Music Selection Stage" + PlayerPrefs.GetString("Resoultion_Melo", string.Empty))
            {
                try { if (difficultyMap == 1) { SelectionMenu_Script.thisSelect.get_selection.UpdateData_Level(1, difficultyLevel); } }
                catch { ArenaSelection_Script.thisArena.UpdateData_Level(1, difficultyLevel); }

                try { if (difficultyMap == 2) { SelectionMenu_Script.thisSelect.get_selection.UpdateData_Level(2, difficultyLevel); } }
                catch { ArenaSelection_Script.thisArena.UpdateData_Level(2, difficultyLevel); }

                try { if (difficultyMap == 3) { SelectionMenu_Script.thisSelect.get_selection.UpdateData_Level(3, difficultyLevel); } }
                catch { ArenaSelection_Script.thisArena.UpdateData_Level(3, difficultyLevel); }
            }
        }

        // Score Intillization: Function
        protected void LevelSetup_Program(int[] score_database, int[] score_database2)
        {
            for (int i = 0; i < 2; i++)
            {
                PlayerPrefs.DeleteKey("EnemyTakeCounter_" + (i + 1));
                PlayerPrefs.DeleteKey("TrapsTakeCounter_" + (i + 1));
            }

            // Normal
            for (int i = 0; i < score_database.Length; i++)
            {
                if (score_database[i] == 1 || score_database[i] == 6) { EnemyCounter++; }
                if (score_database[i] == 3 || score_database[i] == 7) { TrapCounter++; }
                if (score_database[i] != 0 && score_database[i] < 10) { maxCombo++; }
                if (score_database[i] == 93) { maxCombo++; }

                // Pattern Charting
                NewChartPatternSearch(score_database, i);
                NewChartPatternModded(score_database, i);
            }

            PlayerPrefs.SetInt("EnemyTakeCounter_1", EnemyCounter);
            PlayerPrefs.SetInt("TrapsTakeCounter_1", TrapCounter);

            SetUp(1, EnemyCounter, TrapCounter, maxCombo, score_database);
            ResetCounter();

            // Hard
            for (int i = 0; i < score_database2.Length; i++)
            {
                if (score_database2[i] == 1 || score_database2[i] == 6) { EnemyCounter++; }
                if (score_database2[i] == 3 || score_database2[i] == 7) { TrapCounter++; }
                if (score_database2[i] != 0 && score_database2[i] < 10) { maxCombo++; }
                if (score_database2[i] == 93) { maxCombo++; }

                // Pattern Charting
                NewChartPatternSearch(score_database2, i);
                NewChartPatternModded(score_database2, i);
            }

            PlayerPrefs.SetInt("EnemyTakeCounter_2", EnemyCounter);
            PlayerPrefs.SetInt("TrapsTakeCounter_2", TrapCounter);

            SetUp(2, EnemyCounter, TrapCounter, maxCombo, score_database2);
            ResetCounter();
        }

        protected void NewChartPatternSearch(int[] score, int currentTick)
        {
            if (SelectionMenu_Script.thisSelect != null &&
                SelectionMenu_Script.thisSelect.get_selection.get_form.NewChartSystem &&
                SelectionMenu_Script.thisSelect.get_selection.get_form.addons3 != null)
            {
                foreach (PatternCharting chart in SelectionMenu_Script.thisSelect.get_selection.get_form.addons3)
                {
                    if (score[currentTick] == chart.SecondaryIndex)
                    {
                        foreach (PaterrnDefine row in chart.noteArray)
                        {
                            foreach (PatternLane col in row.noteOutput)
                            {
                                if (col.PrimaryNote != 0)
                                {
                                    maxCombo++;

                                    if (col.PrimaryNote == 1 || col.PrimaryNote == 6) { EnemyCounter++; }
                                    if (col.PrimaryNote == 3 || col.PrimaryNote == 7) { TrapCounter++; }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void NewChartPatternModded(int[] score, int currentTick)
        {
            if (SelectionMenu_Script.thisSelect != null &&
                SelectionMenu_Script.thisSelect.get_selection.get_form.NewChartSystem &&
                SelectionMenu_Script.thisSelect.get_selection.get_form.addons3b != null)
            {
                foreach (ChartModification mod in SelectionMenu_Script.thisSelect.get_selection.get_form.addons3b)
                {
                    if (score[currentTick] == mod.newIndex)
                    {
                        foreach (PatternCharting chart in SelectionMenu_Script.thisSelect.get_selection.get_form.addons3)
                        {
                            if (mod.SecondaryNote == chart.SecondaryIndex)
                            {
                                foreach (PaterrnDefine row in chart.noteArray)
                                {
                                    foreach (PatternLane col in row.noteOutput)
                                    {
                                        if (col.PrimaryNote != 0)
                                        {
                                            maxCombo++;

                                            if (col.PrimaryNote == 1 || col.PrimaryNote == 6) { EnemyCounter++; }
                                            if (col.PrimaryNote == 3 || col.PrimaryNote == 7) { TrapCounter++; }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        protected void NewChartMultipleSearch(int[] score, int currentTick)
        {
            if (SelectionMenu_Script.thisSelect.get_selection.get_form.NewChartSystem && 
                SelectionMenu_Script.thisSelect.get_selection.get_form.addons1 != null)
            {
                foreach (MultipleCharting specialChart in SelectionMenu_Script.thisSelect.get_selection.get_form.addons1)
                {
                    if (score[currentTick] == specialChart.SecondaryIndex)
                    {
                        // All Notes
                        if (specialChart.FirstLane != 0) maxCombo++;
                        if (specialChart.SecondLane != 0) maxCombo++;
                        if (specialChart.ThirdLane != 0) maxCombo++;
                        if (specialChart.FourthLane != 0) maxCombo++;
                        if (specialChart.FifthLane != 0) maxCombo++;

                        // Enemys
                        if (specialChart.FirstLane == 1 || specialChart.FirstLane == 6) { EnemyCounter++; }
                        if (specialChart.SecondLane == 1 || specialChart.SecondLane == 6) { EnemyCounter++; }
                        if (specialChart.ThirdLane == 1 || specialChart.ThirdLane == 6) { EnemyCounter++; }
                        if (specialChart.FourthLane == 1 || specialChart.FourthLane == 6) { EnemyCounter++; }
                        if (specialChart.FifthLane == 1 || specialChart.FifthLane == 6) { EnemyCounter++; }

                        // Traps
                        if (specialChart.FirstLane == 3 || specialChart.FirstLane == 7) { TrapCounter++; }
                        if (specialChart.SecondLane == 3 || specialChart.SecondLane == 7) { TrapCounter++; }
                        if (specialChart.ThirdLane == 3 || specialChart.ThirdLane == 7) { TrapCounter++; }
                        if (specialChart.FourthLane == 3 || specialChart.FourthLane == 7) { TrapCounter++; }
                        if (specialChart.FifthLane == 3 || specialChart.FifthLane == 7) { TrapCounter++; }
                    }
                }
            }
        }

        protected void LevelSetup_ExtraProgram(int[] score_database)
        {
            PlayerPrefs.DeleteKey("EnemyTakeCounter_3");
            PlayerPrefs.DeleteKey("TrapsTakeCounter_3");

            // Ultimate
            for (int i = 0; i < score_database.Length; i++)
            {
                if (score_database[i] == 1 || score_database[i] == 6) { EnemyCounter++; }
                if (score_database[i] == 3 || score_database[i] == 7) { TrapCounter++; }
                if (score_database[i] != 0 && score_database[i] < 10) { maxCombo++; }
                if (score_database[i] == 93) { maxCombo++; }

                // Pattern Charting
                NewChartPatternSearch(score_database, i);
                NewChartPatternModded(score_database, i);

                // Count max combo of pattern note
                NewChartMultipleSearch(score_database, i);
            }

            Debug.Log("MaxCombo (Level Editor): " + maxCombo);
            PlayerPrefs.SetInt("EnemyTakeCounter_3", EnemyCounter);
            PlayerPrefs.SetInt("TrapsTakeCounter_3", TrapCounter);

            SetUp(3, EnemyCounter, TrapCounter, maxCombo, score_database);
            ResetCounter();
        }

        // Extra: Reset Counter
        protected void ResetCounter()
        {
            EnemyCounter = 0;
            TrapCounter = 0;
            maxCombo = 0;
        }
    }

    // Unit: Scoring_Control Function
    public class ScoreController
    {
        public ScoreController(int difficulty, int[] _score1, string normal_playArea, int[] _score2, string hard_playArea)
        {
            if (SceneManager.GetActiveScene().name == "Battleground Stage" + PlayerPrefs.GetString("Resoultion_Melo", string.Empty))
            {
                switch (difficulty)
                {
                    case 1:
                        GameManager.thisManager.get_playField.SetPlayField(normal_playArea);
                        BeatConductor.thisBeat.get_myData_Note2 = new int[_score1.Length];

                        for (int i = 0; i < _score1.Length; i++)
                        {
                            BeatConductor.thisBeat.get_myData_Note2[i] = _score1[i];
                        }
                        break;

                    case 2:
                        GameManager.thisManager.get_playField.SetPlayField(hard_playArea);
                        BeatConductor.thisBeat.get_myData_Note2 = new int[_score2.Length];

                        for (int i = 0; i < _score2.Length; i++)
                        {
                            BeatConductor.thisBeat.get_myData_Note2[i] = _score2[i];
                        }
                        break;
                }
            }

            else
            {
                ScoringMeter level = new ScoringMeter(_score1, _score2);
            }
        }

        public ScoreController(int difficulty, int[] _score1, string normal_playArea, int[] _score2, string hard_playArea, int[] _score3, string ultimate_playArea)
        {
            if (SceneManager.GetActiveScene().name == "Battleground Stage" + PlayerPrefs.GetString("Resoultion_Melo", string.Empty))
            {
                switch (difficulty)
                {
                    case 1:
                        GameManager.thisManager.get_playField.SetPlayField(normal_playArea);
                        BeatConductor.thisBeat.get_myData_Note2 = new int[_score1.Length];

                        for (int i = 0; i < _score1.Length; i++)
                        {
                            BeatConductor.thisBeat.get_myData_Note2[i] = _score1[i];
                        }
                        break;

                    case 2:
                        GameManager.thisManager.get_playField.SetPlayField(hard_playArea);
                        BeatConductor.thisBeat.get_myData_Note2 = new int[_score2.Length];

                        for (int i = 0; i < _score2.Length; i++)
                        {
                            BeatConductor.thisBeat.get_myData_Note2[i] = _score2[i];
                        }
                        break;

                    case 3:
                        GameManager.thisManager.get_playField.SetPlayField(ultimate_playArea);
                        BeatConductor.thisBeat.get_myData_Note2 = new int[_score3.Length];

                        for (int i = 0; i < _score3.Length; i++)
                        {
                            BeatConductor.thisBeat.get_myData_Note2[i] = _score3[i];
                        }
                        break;
                }
            }

            else
            {
                ScoringMeter level = new ScoringMeter(_score1, _score2, _score3);
            }
        }
    }
}

namespace MeloMelo_Local
{
    [System.Serializable]
    struct ScoreDatabase
    {
        public string user;
        public string title;
        public int difficulty;
        public float score;
        public string rank;
        public int remark;

        public ScoreDatabase GetTrackData(string format)
        {
            Debug.Log(format);
            return JsonUtility.FromJson<ScoreDatabase>(format);
        }
    }

    [System.Serializable]
    struct PointDatabase
    {
        public string user;
        public string title;
        public int difficulty;
        public int current;
        public int max;

        public PointDatabase GetTrackData(string format)
        {
            Debug.Log(format);
            return JsonUtility.FromJson<PointDatabase>(format);
        }
    }

    [System.Serializable]
    struct PlayerSettingsDatabase
    {
        public bool HTP;
        public bool Control_N;
        public bool battleGuide;

        public bool isOptionVisited;
        public bool isTransferDone;
        public bool isRetreatUsed;
        public bool isMarathonVisited;
        public bool isCollectionVisited;

        public string currentVersion;
        public string chartVersion;

        public float bgm_audio_data;
        public float se_audio_data;
        public bool audio_mute_data;
        public bool audio_voice_data;

        public bool allowIntefaceAnimation;
        public bool allowCharacterAnimation;
        public bool allowEnemyAnimation;
        public bool allowDamageIndicatorOnAlly;
        public bool allowDamageIndicatorOnEnemy;

        public bool autoSaveGameProgress;
        public bool autoSavePlaySettings;
        public bool autoSaveGameSettings;

        public PlayerSettingsDatabase GetSettingsData(string format)
        {
            Debug.Log(format);
            return JsonUtility.FromJson<PlayerSettingsDatabase>(format);
        }
    }

    [System.Serializable]
    struct PlayerGameplaySettings
    {
        public bool mvOption;
        public int noteSpeed;
        public bool autoSkill;
        public int AutoRetreat;

        public int ScoreDisplay1;
        public int ScoreDisplay2;
        public int JudgeMeterSetup;
        public int JudgeFeedback_TypeA;
        public int JudgeFeedback_TypeB;

        public PlayerGameplaySettings GetSettingsData(string format)
        {
            Debug.Log(format);
            return JsonUtility.FromJson<PlayerGameplaySettings>(format);
        }
    }

    [System.Serializable]
    struct PlayerCharacterSettings
    {
        public string[] characterSlot;
        public string mainSlot;

        public PlayerCharacterSettings(string main)
        {
            characterSlot = new string[3];
            mainSlot = main;
        }

        public PlayerCharacterSettings GetSettingsData(string format)
        {
            Debug.Log(format);
            return JsonUtility.FromJson<PlayerCharacterSettings>(format);
        }
    }

    [System.Serializable]
    struct BattleProgressDatabase
    {
        public string user;
        public string title;
        public int difficulty;
        public int area_difficulty;
        public int score;
        public bool success;

        public BattleProgressDatabase GetProgressData(string format)
        {
            Debug.Log(format);
            return JsonUtility.FromJson<BattleProgressDatabase>(format);
        }
    }

    [System.Serializable]
    struct ProfileProgressDatabase
    {
        public int ratePoint;
        public int playedCount;
        public int creditValue;

        public ProfileProgressDatabase GetProfileData(string format)
        {
            Debug.Log(format);
            return JsonUtility.FromJson<ProfileProgressDatabase>(format);
        }
    }

    [System.Serializable]
    struct LocalPlayerIdentification
    {
        public string playerId;
        public string uniqueId;

        public LocalPlayerIdentification GetPlayerInfo(string format)
        {
            Debug.Log(format);
            return JsonUtility.FromJson<LocalPlayerIdentification>(format);
        }
    }

    [System.Serializable]
    struct BattleUnitDatabase
    {
        public string id;
        public int level;
        public int experience;
        public int totalMasteryAdded;
        public int totalRebirthPoint;

        public int baseStrengthStats;
        public int baseVitalityStats;
        public int baseMagicStats;

        public BattleUnitDatabase GetCharacterStatus(string format)
        {
            Debug.Log(format);
            return JsonUtility.FromJson<BattleUnitDatabase>(format);
        }
    }

    [System.Serializable]
    struct LastSelectionEntrance
    {
        public int lastSelection;
        public int currentDifficulty;
        public string currentArea;

        public LastSelectionEntrance GetLatestSelection(string format)
        {
            Debug.Log(format);
            return JsonUtility.FromJson<LastSelectionEntrance>(format);
        }
    }

    [System.Serializable]
    struct SkillUnitDatabase
    {
        public string skillName;
        public int grade;
        public int status;

        public SkillUnitDatabase GetSkillDatabase(string format)
        {
            Debug.Log(format);
            return JsonUtility.FromJson<SkillUnitDatabase>(format);
        }
    }

    public class LocalData
    {
        protected string user = string.Empty;
        protected string localPath = string.Empty;
        protected string directory = string.Empty;
        protected string combinePath = string.Empty;

        #region MAIN

        public void SelectFileForAction(string name)
        {
            combinePath = localPath + "/" + name;
            Debug.Log("File: " + name);
        }

        public void SelectFileForActionWithUserTag(string name)
        {
            combinePath = localPath + "/" + user + "_" + name;
            Debug.Log("File: " + user + "_" + name);
        }
        #endregion

        #region COMPONENT
        protected string GetProgressFile()
        {
            string text = string.Empty;
            StreamReader reader = new StreamReader(directory + combinePath);
            text = reader.ReadToEnd();
            reader.Close();

            return text;
        }

        protected void WriteToFile(string text)
        {
            StreamWriter writer = new StreamWriter(directory + combinePath);
            writer.Write(text);
            writer.Close();
        }

        protected string[] GetFormatToList()
        {
            StreamReader reader = new StreamReader(directory + combinePath);
            string[] format = reader.ReadToEnd().Split('/');
            reader.Close();

            return format;
        }
        #endregion
    }

    public class Authenticate_LocalData : LocalData
    {
        public Authenticate_LocalData(string user, string path)
        {
            this.user = user;
            localPath = path;
            directory = (Application.isEditor ? "Assets/" : "MeloMelo_Data/");

            Debug.Log("LocalData Setup: " + this.user);
        }

        #region COMPONENT
        public void MakeNewUserOnLocal(string uniqueId)
        {
            // Create entry title
            LocalPlayerIdentification user = new LocalPlayerIdentification();
            user.playerId = this.user;

            if (uniqueId == string.Empty) user.uniqueId = Random.Range(99, 999) + "_T2024_t&" + Random.Range(5, 55);
            else user.uniqueId = uniqueId;

            // Process for checking new entry title
            if (!GetExistingUserOnLocal())
            {
                string jsonFormat = string.Empty;

                if (File.Exists(directory + combinePath)) jsonFormat += GetProgressFile();
                jsonFormat += "\n" + JsonUtility.ToJson(user) + "/";

                WriteToFile(jsonFormat);
            }
            else
            {
                List<LocalPlayerIdentification> users = new List<LocalPlayerIdentification>();

                foreach (string user_decode in GetFormatToList())
                    if (user_decode != string.Empty) users.Add(new LocalPlayerIdentification().GetPlayerInfo(user_decode));

                OverWriteEntryFromList(users, user);

                string jsonFormat = string.Empty;
                foreach (LocalPlayerIdentification info in users) jsonFormat += "\n" + JsonUtility.ToJson(info) + "/";
                WriteToFile(jsonFormat);
            }
        }

        public void DestroyNewUserOnLocal()
        {
            List<LocalPlayerIdentification> users = new List<LocalPlayerIdentification>();

            foreach (string user_decode in GetFormatToList())
                if (user_decode != string.Empty) users.Add(new LocalPlayerIdentification().GetPlayerInfo(user_decode));

            RemoveEntryFromList(users);
        }

        public bool GetExistingUserOnLocal()
        {
            foreach (string playerId in GetUserAuthenticationList())
                if (playerId == user) return true;

            return false;
        }

        public string[] GetUserAuthenticationList()
        {
            if (File.Exists(directory + combinePath))
            {
                List<LocalPlayerIdentification> dataArray = new List<LocalPlayerIdentification>();
                List<string> playerList = new List<string>();

                foreach (string decode_player in GetFormatToList())
                    if (decode_player != string.Empty) dataArray.Add(new LocalPlayerIdentification().GetPlayerInfo(decode_player));

                foreach (LocalPlayerIdentification player in dataArray.ToArray())
                    playerList.Add(player.playerId);

                return playerList.ToArray();
            }

            return null;
        }
        #endregion

        #region MODIFY LIST
        private void OverWriteEntryFromList(List<LocalPlayerIdentification> entry, LocalPlayerIdentification user)
        {
            foreach (LocalPlayerIdentification localUser in entry)
            {
                if (this.user == localUser.playerId)
                {
                    entry.Remove(localUser);
                    entry.Add(user);
                    break;
                }
            }
        }

        private void RemoveEntryFromList(List<LocalPlayerIdentification> entry)
        {
            string jsonFormat = string.Empty;

            foreach (LocalPlayerIdentification localUser in entry)
            {
                if (user == localUser.playerId)
                {
                    entry.Remove(localUser);
                    break;
                }
            }

            foreach (LocalPlayerIdentification localUser in entry)
                jsonFormat += JsonUtility.ToJson(localUser) + "/";

            WriteToFile(jsonFormat);
        }
        #endregion

        #region ACCOUNT INFO
        public string GetUserLocalByPlayerId()
        {
            return user;
        }

        public string GetUserLocalByUniqueId()
        {
            if (File.Exists(directory + combinePath))
            {
                List<LocalPlayerIdentification> dataArray = new List<LocalPlayerIdentification>();

                foreach (string decode_player in GetFormatToList())
                    if (decode_player != string.Empty) dataArray.Add(new LocalPlayerIdentification().GetPlayerInfo(decode_player));

                foreach (LocalPlayerIdentification player in dataArray.ToArray())
                    if (player.playerId == user) return player.uniqueId;
            }

            return string.Empty;
        }
        #endregion
    }

    public class LocalData_MarathonDatabase : LocalData
    {
        public LocalData_MarathonDatabase(string user, string path)
        {
            this.user = user;
            localPath = path;
            directory = (Application.isEditor ? "Assets/" : "MeloMelo_Data/");

            Debug.Log("Path: " + directory + localPath);
            Debug.Log("Registered_User (Load_Database): " + user);
        }

        #region MAIN
        public bool SaveProgress(string title, bool isDone, int count, int score)
        {
            MarathonChallengeProgress save = new MarathonChallengeProgress();
            save.title = title;
            save.clearedChallenge = isDone;
            save.playedCount = count;
            save.totalScore = score;

            if (File.Exists(directory + combinePath)) ReplaceExistingProgress(save);
            else AddNewProgress(save);

            return true;
        }

        public bool LoadProgress()
        {
            List<MarathonChallengeProgress> loader = new List<MarathonChallengeProgress>();

            if (File.Exists(directory + combinePath))
            {
                foreach (string load in GetFormatToList()) if (load != string.Empty) 
                    loader.Add(JsonUtility.FromJson<MarathonChallengeProgress>(load));

                foreach (MarathonChallengeProgress progress in loader)
                {
                    PlayerPrefs.SetInt("MarathonProgress_Title_" + progress.title, 1);
                    PlayerPrefs.SetInt("MarathonProgress_PlayedCount_" + progress.title, progress.playedCount);
                    PlayerPrefs.SetString("MarathonProgress_Cleared_" + progress.title, progress.clearedChallenge ? "T" : "F");
                    PlayerPrefs.SetInt("MarathonProgress_Score_" + progress.title, progress.totalScore);
                }
            }

            return true;
        }
        #endregion

        #region COMPONENT
        private void AddNewProgress(MarathonChallengeProgress newProgress)
        {
            string jsonString = JsonUtility.ToJson(newProgress) + "/";
            if (File.Exists(directory + combinePath)) jsonString += GetProgressFile();
            WriteToFile(jsonString);
        }

        private void ReplaceExistingProgress(MarathonChallengeProgress newProgress)
        {
            List<MarathonChallengeProgress> loader = new List<MarathonChallengeProgress>();
            foreach (string load in GetFormatToList()) if (load != string.Empty)
                    loader.Add(JsonUtility.FromJson<MarathonChallengeProgress>(load));

            for (int removeIndex = 0; removeIndex < loader.ToArray().Length; removeIndex++)
            {
                if (loader[removeIndex].title == newProgress.title) 
                    loader.Remove(loader[removeIndex]);
            }

            loader.Add(newProgress);
            string newSaveProgress = string.Empty;
            foreach (MarathonChallengeProgress save in loader) newSaveProgress += JsonUtility.ToJson(save) + "/";
            WriteToFile(newSaveProgress);
        }
        #endregion
    }

    public class LocalLoad_DataManagement : LocalData
    {
        public LocalLoad_DataManagement(string user, string path)
        {
            this.user = user;
            localPath = path;
            directory = (Application.isEditor ? "Assets/" : "MeloMelo_Data/");

            Debug.Log("Path: " + directory + localPath);
            Debug.Log("Registered_User (Load_Database): " + user);
        }

        public bool LoadProgress()
        {
            if (File.Exists(directory + combinePath))
            {
                List<ScoreDatabase> dataArray = new List<ScoreDatabase>();

                foreach (string s in GetFormatToList())
                {
                    if (s != string.Empty)
                        dataArray.Add(new ScoreDatabase().GetTrackData(s));
                }

                foreach (ScoreDatabase data in dataArray)
                {
                    if (user == data.user)
                    {
                        // Score is taken to the highest
                        if (data.score >= PlayerPrefs.GetInt(data.title + "_score" + data.difficulty, 0))
                            PlayerPrefs.SetInt(data.title + "_score" + data.difficulty, (int)data.score);

                        // Battle Status update to the latest
                        if (data.remark <= PlayerPrefs.GetInt(data.title + "_BattleRemark_" + data.difficulty, 6))
                            PlayerPrefs.SetInt(data.title + "_BattleRemark_" + data.difficulty, data.remark);

                        // ???
                        if (PlayerPrefs.GetInt(data.title + "_score" + data.difficulty, 0) >= 1000000)
                            PlayerPrefs.SetString(data.title + "_score" + data.difficulty + "_SS", "T");

                        // Reduce score to 0 when defeated
                        if (PlayerPrefs.GetInt(data.title + "_BattleRemark_" + data.difficulty) == 5)
                            PlayerPrefs.SetInt(data.title + "_score" + data.difficulty, 0);
                    }
                }
            }

            return true;
        }

        public bool LoadPointProgress()
        {
            if (File.Exists(directory + combinePath))
            {
                List<PointDatabase> dataArray = new List<PointDatabase>();

                foreach (string s in GetFormatToList())
                {
                    if (s != string.Empty)
                        dataArray.Add(new PointDatabase().GetTrackData(s));
                }

                foreach (PointDatabase data in dataArray)
                {
                    if (user == data.user && data.current >= PlayerPrefs.GetInt(data.title + "_point" + data.difficulty, 0))
                    {
                        // Fomrat: User, Difficulty, Points, MaxPoints
                        PlayerPrefs.SetInt(data.title + "_point" + data.difficulty, data.current);
                        PlayerPrefs.SetInt(data.title + "_maxPoint" + data.difficulty, data.max);
                    }
                }
            }

            return true;
        }

        public bool LoadAccountSettings()
        {
            if (File.Exists(directory + combinePath))
            {
                PlayerSettingsDatabase data = new PlayerSettingsDatabase().GetSettingsData(GetProgressFile());

                PlayerPrefs.SetString("HowToPlay_Notice", data.HTP ? "T" : "F");
                PlayerPrefs.SetString("Control_notice", data.Control_N ? "T" : "F");
                PlayerPrefs.SetString("BattleSetup_Guide", data.battleGuide ? "T" : "F");
                PlayerPrefs.SetString("MeloMelo_Current_PlayVersion", data.currentVersion);
                PlayerPrefs.SetString("MeloMelo_Current_ChartVersion", data.chartVersion);

                if (data.isOptionVisited) PlayerPrefs.SetInt("ReviewOption", 1);
                if (data.isTransferDone) PlayerPrefs.SetInt("ReviewTransfer", 1);
                if (data.isMarathonVisited) PlayerPrefs.SetInt("MarathonPass_Eternal", 1);
                if (data.isCollectionVisited) PlayerPrefs.SetInt("CollectionAlbum_Visited", 1);
                if (data.isRetreatUsed) PlayerPrefs.SetInt("RetreatRoute", 1);

                PlayerPrefs.SetFloat(MeloMelo_PlayerSettings.GetBGM_ValueKey, data.bgm_audio_data);
                PlayerPrefs.SetFloat(MeloMelo_PlayerSettings.GetSE_ValueKey, data.se_audio_data);
                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetInterfaceAnimation_ValueKey, data.allowIntefaceAnimation ? 1 : 0);
                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetCharacterAnimation_ValueKey, data.allowCharacterAnimation ? 1 : 0);
                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetEnemyAnimation_ValueKey, data.allowEnemyAnimation ? 1 : 0);
                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetDamageIndicatorA_ValueKey, data.allowDamageIndicatorOnAlly ? 1 : 0);
                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetDamageIndicatorB_ValueKey, data.allowDamageIndicatorOnEnemy ? 1 : 0);
                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetAutoSaveProgress_ValueKey, data.autoSaveGameProgress ? 1 : 0);
                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetAutoSaveGameSettings_ValueKey, data.autoSaveGameSettings ? 1 : 0);
                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetAutoSavePlaySettings_ValueKey, data.autoSavePlaySettings ? 1 : 0);
            }

            return true;
        }

        public bool LoadGameplaySettings()
        {
            if (File.Exists(directory + combinePath))
            {
                PlayerGameplaySettings data = new PlayerGameplaySettings().GetSettingsData(GetProgressFile());

                PlayerPrefs.SetString("MVOption", data.mvOption ? "T" : "F");
                PlayerPrefs.SetInt("NoteSpeed", data.noteSpeed);
                PlayerPrefs.SetString("AutoSkillOption", data.autoSkill ? "T" : "F");

                PlayerPrefs.SetInt("AutoRetreat", data.AutoRetreat);
                PlayerPrefs.SetInt("ScoreDisplay", data.ScoreDisplay1);
                PlayerPrefs.SetInt("ScoreDisplay2", data.ScoreDisplay2);
                PlayerPrefs.SetInt("JudgeMeter_Setup", data.JudgeMeterSetup);
                PlayerPrefs.SetInt("Feedback_Display_Type_B", data.JudgeFeedback_TypeA);
                PlayerPrefs.SetInt("Feedback_Display_Type", data.JudgeFeedback_TypeB);
            }

            return true;
        }

        public bool LoadCharacterSettings()
        {
            if (File.Exists(directory + combinePath))
            {
                PlayerCharacterSettings data = new PlayerCharacterSettings().GetSettingsData(GetProgressFile());

                for (int i = 0; i < 3; i++) { PlayerPrefs.SetString("Slot" + (i + 1) + "_charName", data.characterSlot[i]); }
                PlayerPrefs.SetString("CharacterFront", data.mainSlot);
            }

            return true;
        }

        public bool LoadProfileState()
        {
            if (File.Exists(directory + combinePath))
            {
                ProfileProgressDatabase data = new ProfileProgressDatabase().GetProfileData(GetProgressFile());

                PlayerPrefs.SetInt(user + "totalRatePoint", data.ratePoint);
                PlayerPrefs.SetInt(user + "UserRatePointToggle", data.ratePoint);
                PlayerPrefs.SetInt(user + "PlayedCount_Data", data.playedCount);
                PlayerPrefs.SetInt(user + "_Credit", data.creditValue);
            }

            return true;
        }

        public bool LoadBattleProgress()
        {
            if (File.Exists(directory + combinePath))
            {
                List<BattleProgressDatabase> dataArray = new List<BattleProgressDatabase>();

                foreach (string s in GetFormatToList())
                {
                    if (s != string.Empty)
                        dataArray.Add(new BattleProgressDatabase().GetProgressData(s));
                }

                foreach (BattleProgressDatabase data in dataArray)
                {
                    if (user == data.user)
                    {
                        PlayerPrefs.SetString(data.title + "_SuccessBattle_" + data.difficulty + data.area_difficulty, data.success ? "T" : "F");
                        PlayerPrefs.SetInt(data.title + "_techScore" + data.difficulty + data.area_difficulty, data.score);
                    }
                }
            }

            return true;
        }

        public int LoadSelectionPickProgress(string area, int option)
        {
            if (File.Exists(directory + combinePath))
            {
                List<LastSelectionEntrance> listing = new List<LastSelectionEntrance>();
                foreach (string data_decode in GetFormatToList()) if (data_decode != string.Empty)
                        listing.Add(new LastSelectionEntrance().GetLatestSelection(data_decode));

                switch (option)
                {
                    case 1:
                        foreach (LastSelectionEntrance searchSelection in listing)
                            if (searchSelection.currentArea == area) return searchSelection.currentDifficulty;
                        break;

                    default:
                        foreach (LastSelectionEntrance searchSelection in listing)
                            if (searchSelection.currentArea == area) return searchSelection.lastSelection;
                        break;
                }
            }

            return 1;
        }

        public bool LoadCharacterStatsProgress()
        {
            if (File.Exists(directory + combinePath))
            {
                List<BattleUnitDatabase> dataArray = new List<BattleUnitDatabase>();

                foreach (string s in GetFormatToList())
                {
                    if (s != string.Empty)
                        dataArray.Add(new BattleUnitDatabase().GetCharacterStatus(s));
                }

                foreach (BattleUnitDatabase data in dataArray)
                {
                    PlayerPrefs.SetInt(data.id + "_LEVEL", data.level);
                    PlayerPrefs.SetInt(data.id + "_EXP", data.experience);
                    MeloMelo_CharacterInfo_Settings.UnlockCharacter(data.id);

                    int unUsedMasteryPoint = data.level * 2 - data.totalMasteryAdded;
                    MeloMelo_ExtraStats_Settings.SetMasteryPoint(data.id, unUsedMasteryPoint);
                    MeloMelo_ExtraStats_Settings.SetRebirthPoint(data.id, data.totalRebirthPoint);

                    MeloMelo_ExtraStats_Settings.IncreaseStrengthStats(data.id, data.baseStrengthStats);
                    MeloMelo_ExtraStats_Settings.IncreaseVitalityStats(data.id, data.baseVitalityStats);
                    MeloMelo_ExtraStats_Settings.IncreaseMagicStats(data.id, data.baseMagicStats);
                }
            }

            return true;
        }

        public bool LoadAllSkillsType()
        {
            if (File.Exists(directory + combinePath))
            {
                List<SkillUnitDatabase> dataArray = new List<SkillUnitDatabase>();

                foreach (string s in GetFormatToList())
                {
                    if (s != string.Empty)
                        dataArray.Add(new SkillUnitDatabase().GetSkillDatabase(s));
                }

                foreach (SkillUnitDatabase data in dataArray)
                {
                    if (data.status == 1)
                    {
                        MeloMelo_SkillData_Settings.UnlockSkill(data.skillName);
                        for (int numberOfTimes = 0; numberOfTimes < data.grade; numberOfTimes++)
                        {
                            if (MeloMelo_SkillData_Settings.CheckSkillGrade(data.skillName) != 0)
                                MeloMelo_SkillData_Settings.UpgradeSkill(data.skillName);
                            else
                                MeloMelo_SkillData_Settings.LearnSkill(data.skillName);
                        }
                    }
                    else
                        MeloMelo_SkillData_Settings.LockedSkill(data.skillName);
                }
            }

            return true;
        }

        public bool LoadVirtualItemToPlayer()
        {
            if (File.Exists(directory + combinePath)) MeloMelo_GameSettings.StoreAllItemToLocal(GetProgressFile());
            return true;
        }

        #region RAW
        public string GetLocalJsonFile(string fileName, bool getUserId)
        {
            if (getUserId) SelectFileForActionWithUserTag(fileName);
            else SelectFileForAction(fileName);

            if (File.Exists(directory + combinePath)) return GetProgressFile();
            else return string.Empty;
        }

        public string[] GetLocalJsonFileToArray(string fileName)
        {
            SelectFileForAction(fileName);
            return GetFormatToList();
        }
        #endregion
    }

    public class LocalSave_DataManagement : LocalData
    {
        public LocalSave_DataManagement(string user, string path)
        {
            this.user = user;
            localPath = path;
            directory = (Application.isEditor ? "Assets/" : "MeloMelo_Data/");

            Debug.Log("Path: " + directory + localPath);
            Debug.Log("Registered_User (Save_Database): " + user);
        }

        public void SaveProgress(string title, int difficulty, float score, string rank)
        {
            string jsonFormat = string.Empty;
            ScoreDatabase data = new ScoreDatabase();
            data.user = user;
            data.title = title;
            data.difficulty = difficulty;
            data.score = score;
            data.rank = rank;
            data.remark = PlayerPrefs.GetInt(title + "_BattleRemark_" + difficulty, 0);

            if (File.Exists(directory + combinePath)) jsonFormat += GetProgressFile();
            jsonFormat += "\n" + JsonUtility.ToJson(data) + "/";

            WriteToFile(jsonFormat);
        }

        public void SavePointProgress(string title, int difficulty, int current, int max)
        {
            string jsonFormat = string.Empty;
            PointDatabase data = new PointDatabase();
            data.user = user;
            data.title = title;
            data.difficulty = difficulty;
            data.current = current;
            data.max = max;

            if (File.Exists(directory + combinePath)) jsonFormat += GetProgressFile();
            jsonFormat += "\n" + JsonUtility.ToJson(data) + "/";

            WriteToFile(jsonFormat);
        }

        public void SaveAccountSettings()
        {
            if (File.Exists(directory + combinePath))
            { File.Delete(directory + combinePath); }

            string JsonFormat = string.Empty;
            PlayerSettingsDatabase data = new PlayerSettingsDatabase();

            data.HTP = false;
            data.Control_N = false;
            data.battleGuide = false;
            data.currentVersion = PlayerPrefs.GetString("MeloMelo_Current_PlayVersion", string.Empty);
            data.chartVersion = PlayerPrefs.GetString("MeloMelo_Current_ChartVersion", string.Empty);

            data.isOptionVisited = PlayerPrefs.HasKey("ReviewOption");
            data.isTransferDone = PlayerPrefs.HasKey("ReviewTransfer");
            data.isMarathonVisited = PlayerPrefs.HasKey("MarathonPass_Eternal");
            data.isRetreatUsed = PlayerPrefs.HasKey("RetreatRoute");
            data.isCollectionVisited = PlayerPrefs.HasKey("CollectionAlbum_Visited");

            data.bgm_audio_data = PlayerPrefs.GetFloat(MeloMelo_PlayerSettings.GetBGM_ValueKey, 0.5f);
            data.se_audio_data = PlayerPrefs.GetFloat(MeloMelo_PlayerSettings.GetSE_ValueKey, 0.5f);
            data.audio_mute_data = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetAudioMute_ValueKey, 1) == 1 ? true : false;
            data.audio_voice_data = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetAudioVoice_ValueKey, 1) == 1 ? true : false;

            data.allowIntefaceAnimation = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetInterfaceAnimation_ValueKey, 1) == 1 ? true : false;
            data.allowCharacterAnimation = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetCharacterAnimation_ValueKey, 1) == 1 ? true : false;
            data.allowEnemyAnimation = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetEnemyAnimation_ValueKey, 1) == 1 ? true : false;
            data.allowDamageIndicatorOnAlly = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetDamageIndicatorA_ValueKey, 1) == 1 ? true : false;
            data.allowDamageIndicatorOnEnemy = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetDamageIndicatorB_ValueKey, 1) == 1 ? true : false;

            data.autoSaveGameProgress = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetAutoSaveProgress_ValueKey, 1) == 1 ? true : false;
            data.autoSaveGameSettings = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetAutoSaveGameSettings_ValueKey, 1) == 1 ? true : false;
            data.autoSavePlaySettings = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetAutoSavePlaySettings_ValueKey, 1) == 1 ? true : false;

            JsonFormat += JsonUtility.ToJson(data);
            WriteToFile(JsonFormat);
        }

        public void SaveGameplaySettings(bool mvOption, int noteSpeed, bool autoSkill, int ratePoint)
        {
            if (File.Exists(directory + combinePath)) 
            { File.Delete(directory + combinePath); }

            string JsonFormat = string.Empty;
            PlayerGameplaySettings data = new PlayerGameplaySettings();
            data.mvOption = mvOption;
            data.noteSpeed = noteSpeed;
            data.autoSkill = autoSkill;

            data.ScoreDisplay1 = PlayerPrefs.GetInt("ScoreDisplay", 0);
            data.ScoreDisplay2 = PlayerPrefs.GetInt("ScoreDisplay2", 0);
            data.JudgeMeterSetup = PlayerPrefs.GetInt("JudgeMeter_Setup", 0);

            data.JudgeFeedback_TypeA = PlayerPrefs.GetInt("Feedback_Display_Type_B", 0);
            data.JudgeFeedback_TypeB = PlayerPrefs.GetInt("Feedback_Display_Type", 1);

            JsonFormat += JsonUtility.ToJson(data);
            WriteToFile(JsonFormat);
        }

        public void SaveFormationSettings(string slot1, string slot2, string slot3, string mainSlot)
        {
            if (File.Exists(directory + combinePath))
            { File.Delete(directory + combinePath); }

            string JsonFormat = string.Empty;
            PlayerCharacterSettings data = new PlayerCharacterSettings(string.Empty);
            
            data.characterSlot[0] = slot1;
            data.characterSlot[1] = slot2;
            data.characterSlot[2] = slot3;
            data.mainSlot = mainSlot;

            JsonFormat += JsonUtility.ToJson(data);
            WriteToFile(JsonFormat);
        }

        public void SaveBattleProgress(string title, int difficulty, int area_difficulty, bool success, int score)
        {
            string JsonFormat = string.Empty;
            BattleProgressDatabase data = new BattleProgressDatabase();

            data.user = LoginPage_Script.thisPage.GetUserPortOutput();
            data.title = title;
            data.difficulty = difficulty;
            data.area_difficulty = area_difficulty;
            data.score = score;
            data.success = success;

            if (File.Exists(directory + combinePath)) JsonFormat += GetProgressFile();
            JsonFormat += "\n" + JsonUtility.ToJson(data) + "/";

            WriteToFile(JsonFormat);
        }

        public void SaveProfileState()
        {
            if (File.Exists(directory + combinePath))
            { File.Delete(directory + combinePath); }

            string JsonFormat = string.Empty;
            ProfileProgressDatabase data = new ProfileProgressDatabase();

            data.ratePoint = PlayerPrefs.GetInt(user + "totalRatePoint", 0);
            data.playedCount = PlayerPrefs.GetInt(user + "PlayedCount_Data", 0) + 1;
            data.creditValue = PlayerPrefs.GetInt(user + "_Credit", 0);

            JsonFormat += JsonUtility.ToJson(data);
            WriteToFile(JsonFormat);
        }

        public void SaveLatestSelectionPoint(string area, int lastSelection)
        {
            List<LastSelectionEntrance> listing = new List<LastSelectionEntrance>();
            LastSelectionEntrance savePoint = new LastSelectionEntrance();

            savePoint.currentArea = area;
            savePoint.currentDifficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
            savePoint.lastSelection = lastSelection;

            if (File.Exists(directory + combinePath))
            {
                foreach (string data_decode in GetFormatToList())
                    if (data_decode != string.Empty) listing.Add(new LastSelectionEntrance().GetLatestSelection(data_decode));

                File.Delete(directory + combinePath);

                foreach (LastSelectionEntrance list in listing)
                    if (list.currentArea == savePoint.currentArea) { listing.Remove(list); break; }
            }

            listing.Add(savePoint);

            string jsonFormat = string.Empty;
            foreach (LastSelectionEntrance list in listing) { jsonFormat += JsonUtility.ToJson(list) + "/"; }
            WriteToFile(jsonFormat);
        }

        public void SaveCharacterStatsProgress(string name, int level, int experience)
        {
            List<BattleUnitDatabase> listing = new List<BattleUnitDatabase>();
            BattleUnitDatabase character = new BattleUnitDatabase();

            character.id = name;
            character.level = level;
            character.experience = experience;
            character.totalMasteryAdded = level * 2 - MeloMelo_ExtraStats_Settings.GetMasteryPoint(name);
            character.totalRebirthPoint = MeloMelo_ExtraStats_Settings.GetRebirthPoint(name);

            character.baseStrengthStats = MeloMelo_ExtraStats_Settings.GetExtraStrengthStats(name);
            character.baseVitalityStats = MeloMelo_ExtraStats_Settings.GetExtraVitaltyStats(name);
            character.baseMagicStats = MeloMelo_ExtraStats_Settings.GetExtraMagicStats(name);

            if (File.Exists(directory + combinePath))
            {
                foreach (string data_decode in GetFormatToList())
                    if (data_decode != string.Empty) listing.Add(new BattleUnitDatabase().GetCharacterStatus(data_decode));

                File.Delete(directory + combinePath);

                foreach (BattleUnitDatabase list in listing)
                    if (list.id == name) { listing.Remove(list); break; }
            }

            listing.Add(character);

            string jsonFormat = string.Empty;
            foreach (BattleUnitDatabase list in listing) { jsonFormat += JsonUtility.ToJson(list) + "/"; }
            WriteToFile(jsonFormat);
        }

        public void SaveAllSkillsType()
        {
            List<SkillUnitDatabase> listing = new List<SkillUnitDatabase>();

            foreach (SkillContainer data in Resources.LoadAll<SkillContainer>("Database_Skills"))
            {
                if (MeloMelo_SkillData_Settings.CheckSkillStatus(data.skillName) || data.isUnlockReady)
                {
                    SkillUnitDatabase onSave = new SkillUnitDatabase();
                    onSave.skillName = data.skillName;
                    onSave.grade = MeloMelo_SkillData_Settings.CheckSkillGrade(data.skillName) == 0 ? 1 :
                        MeloMelo_SkillData_Settings.CheckSkillGrade(data.skillName);
                    onSave.status = MeloMelo_SkillData_Settings.CheckSkillStatus(data.skillName) ? 1 : 0;
                    listing.Add(onSave);
                }
            }

            string jsonFormat = string.Empty;
            foreach (SkillUnitDatabase list in listing) { jsonFormat += JsonUtility.ToJson(list) + "/"; }
            WriteToFile(jsonFormat);
        }

        public void SaveVirtualItemFromPlayer(string itemName, int amount)
        {
            List<VirtualItemDatabase> listing = new List<VirtualItemDatabase>();
            VirtualItemDatabase itemLoader = new VirtualItemDatabase();

            int updateAmount = amount;
            itemLoader.itemName = itemName;

            if (File.Exists(directory + combinePath))
            {
                foreach (string data_decode in GetFormatToList())
                    if (data_decode != string.Empty) listing.Add(new VirtualItemDatabase().GetItemData(data_decode));

                File.Delete(directory + combinePath);

                foreach (VirtualItemDatabase list in listing)
                    if (list.itemName == itemName) 
                    {
                        updateAmount += list.amount;
                        listing.Remove(list); 
                        break; 
                    }
            }

            itemLoader.amount = updateAmount;
            if (itemLoader.amount > 0) listing.Add(itemLoader);

            string jsonFormat = string.Empty;
            foreach (VirtualItemDatabase list in listing) { jsonFormat += JsonUtility.ToJson(list) + "/"; }
            WriteToFile(jsonFormat);
        }

        public void SaveExchangeTranscationHistory(string jsonData)
        {
            string currentTranscation = jsonData + "/";
            if (File.Exists(directory + combinePath))
            {
                File.Delete(directory + combinePath);
                currentTranscation += GetProgressFile();
            }

            WriteToFile(currentTranscation);
        }
    }

    public class CloudDatabase_Local_DataManagement : LocalData
    { 
        public CloudDatabase_Local_DataManagement(string playerId, string path)
        {
            user = playerId;
            localPath = path;
            directory = (Application.isEditor ? "Assets/" : "MeloMelo_Data/");

            Debug.Log("Path: " + directory + localPath);
            Debug.Log("Local->Cloud (Save_Database): " + user);
        }

        #region JSON
        public string GetJsonFormatFromLocalData(string fileName)
        {
            SelectFileForActionWithUserTag(fileName);

            if (File.Exists(directory + combinePath)) return GetProgressFile();      
            else return string.Empty;
        }

        public void SaveJsonFormatToLocalData(string fileName, string json)
        {
            SelectFileForActionWithUserTag(fileName);
            WriteToFile(json);
        }
        #endregion
    }
}

// MeloMelo: Networking
namespace MeloMelo_Network
{
    public class ServerIP_Settings
    {
        protected string portId = string.Empty;
        protected string urlWeb = string.Empty;
        protected readonly string url_directory = "/database/transcripts/site5/melomelo_backend/";
    }

    public class ServerData : ServerIP_Settings
    {
        public ServerData(string user, string url)
        {
            portId = user;
            urlWeb = url;
        }

        public IEnumerator WebReviewProgram(string comment)
        {
            WWWForm review = new WWWForm();
            review.AddField("userR", portId);
            review.AddField("commentR", comment);

            // Process to server submitting
            UnityWebRequest program = UnityWebRequest.Post(urlWeb + "/database/transcripts/site5/MeloMelo_ReviewSubmission.php", review);
            yield return program.SendWebRequest();

            // Server Prompt: Successful Review
            if (program.downloadHandler.text == "COMPLETED!") { PlayerPrefs.SetString("ReviewCompleted", "T"); }
            else { PlayerPrefs.SetString("ReviewCompleted", "F"); }

            // Load and Clean
            program.Dispose();
        }
    }

    public class CloudSave_DataManagement : ServerIP_Settings
    {
        private List<bool> processReport;
        public List<bool> get_process { get { return processReport; } }
        private int counter;
        public int get_counter { get { return counter; } }
        
        public CloudSave_DataManagement(string user, string url)
        {
            processReport = new List<bool>();
            counter = 0;

            portId = user;
            urlWeb = url;
        }

        #region MAIN (Data Handler)
        public void SaveProgressTrack(string title, int difficulty, int score, int combo)
        {
            WWWForm progress = new WWWForm();
            progress.AddField("User", portId);
            progress.AddField("Title", title);
            progress.AddField("Difficulty", difficulty);
            progress.AddField("Score", score);
            progress.AddField("Combo", combo);

            const string serverAPI = "MeloMelo_SaveProgress_2024.php";
            GetAlternativeServer(serverAPI, progress);
        }

        public void SaveProgressTrackByPoint(string title, int difficulty, int point)
        {
            WWWForm progress = new WWWForm();
            progress.AddField("User", portId);
            progress.AddField("Title", title);
            progress.AddField("Difficulty", difficulty);
            progress.AddField("Point", point);

            const string serverAPI = "MeloMelo_SaveProgress_2_2024.php";
            GetAlternativeServer(serverAPI, progress);
        }

        public void SaveProgressTrackByRemark(string title, int difficulty, int remark)
        {
            WWWForm progress = new WWWForm();
            progress.AddField("User", portId);
            progress.AddField("Title", title);
            progress.AddField("Difficulty", difficulty);
            progress.AddField("Remark", remark);

            const string serverAPI = "MeloMelo_SaveProgress_3_2024.php";
            GetAlternativeServer(serverAPI, progress);
        }

        public void SaveProgressProfile(int totalRatePoint, int playedCount)
        {
            WWWForm profile = new WWWForm();
            profile.AddField("User", portId);
            profile.AddField("RatePoint", totalRatePoint);
            profile.AddField("Count", playedCount);

            const string serverAPI = "MeloMelo_ProfileCapture_2024.php";
            GetAlternativeServer(serverAPI, profile);
        }

        public void SaveSettingConfiguration(string mv, int noteSpeed, int autoRetreat, int primaryDisplay, int secondaryDisplay, int judgeType)
        {
            WWWForm config = new WWWForm();
            config.AddField("User", portId);
            config.AddField("MV", mv);
            config.AddField("NoteSpeed", noteSpeed);
            config.AddField("AutoRetreat", autoRetreat);
            config.AddField("Display1", primaryDisplay);
            config.AddField("Display2", secondaryDisplay);
            config.AddField("JudgeType", judgeType);
            config.AddField("FeedbackA", PlayerPrefs.GetInt("Feedback_Display_Type_B", 0));
            config.AddField("FeedbackB", PlayerPrefs.GetInt("Feedback_Display_Type", 0));

            const string serverAPI = "MeloMelo_SettingConfigOnSave_2024.php";
            GetAlternativeServer(serverAPI, config);
        }

        public void SaveBattleProgress(int areaDifficulty, int trackDifficulty, string title, string battleSuccess, int previousScore, int score)
        {
            WWWForm progress = new WWWForm();
            progress.AddField("User", portId);
            progress.AddField("AreaDiff", areaDifficulty);
            progress.AddField("TrackDiff", trackDifficulty);
            progress.AddField("Title", title);
            progress.AddField("Success", battleSuccess);
            progress.AddField("PreviousScore", previousScore);
            progress.AddField("Score", score);

            const string serverAPI = "MeloMelo_BattleProgressOnSave_2024.php";
            GetAlternativeServer(serverAPI, progress);
        }

        public void SaveMusicSelectionLastVisited(string areaSelect, int trackSelect, int difficulty)
        {
            WWWForm info = new WWWForm();
            info.AddField("User", portId);
            info.AddField("AreaSelect", areaSelect);
            info.AddField("TrackSelect", trackSelect);
            info.AddField("TrackDiff", difficulty);

            const string serverAPI = "MeloMelo_Save_MusicSelectionLastVisited_2024.php";
            GetAlternativeServer(serverAPI, info);
        }

        public void SavePlayerData(string howToPlay, string setup, string control, float bgm, float se)
        {
            WWWForm info = new WWWForm();
            info.AddField("User", portId);
            info.AddField("HowToPlay", howToPlay);
            info.AddField("Setup", setup);
            info.AddField("Control", control);
            info.AddField("BGM", bgm.ToString());
            info.AddField("SE", se.ToString());

            const string serverAPI = "MeloMelo_Save_PlayerSettings_2024.php";
            GetAlternativeServer(serverAPI, info);
        }

        public void SaveBattleFormation(string slot1, string slot2, string slot3, string mainSlot)
        {
            WWWForm info = new WWWForm();
            info.AddField("User", portId);
            info.AddField("Slot_1", slot1);
            info.AddField("Slot_2", slot2);
            info.AddField("Slot_3", slot3);
            info.AddField("MainSlot", mainSlot);

            const string serverAPI = "MeloMelo_Save_BattleFormation_2024.php";
            GetAlternativeServer(serverAPI, info);
        }

        public void SaveCharacterStatusData(string name, int level, int experience)
        {
            WWWForm stats = new WWWForm();
            stats.AddField("User", portId);
            stats.AddField("CharacterName", name);
            stats.AddField("Level", level);
            stats.AddField("Exp", experience);

            const string serverAPI = "MeloMelo_Save_CharacterStatusData_2024.php";
            GetAlternativeServer(serverAPI, stats);
        }

        public void SaveChartDistributionData(string title, string coverImage, int difficulty, string level, int score, int point, int chartType)
        {
            WWWForm info = new WWWForm();
            info.AddField("User", portId);
            info.AddField("Title", title);
            info.AddField("CoverImage", coverImage);
            info.AddField("Difficulty", difficulty);
            info.AddField("Level", level);
            info.AddField("Score", score);
            info.AddField("Point", point);
            info.AddField("Type", chartType);

            const string serverAPI = "MeloMelo_Save_TrackDistributionList_2024.php";
            GetAlternativeServer(serverAPI, info);
        }
        #endregion

        #region MAIN (Data Sender / Data Requesting)
        public IEnumerator GetServerToSave(string key, WWWForm input)
        {
            counter++;
            UnityWebRequest save = UnityWebRequest.Post(urlWeb + key, input);
            yield return save.SendWebRequest();

            processReport.Add(save.downloadHandler.text == "OK!");
            Debug.Log(portId + ": CloudSave - " + key + " [" + save.downloadHandler.text + "]");
            save.Dispose();
        }

        private void GetAlternativeServer(string url, WWWForm info)
        {
            Debug.Log("Redirect to result menu...");
            ResultMenu_Script.thisRes.ConnectionEstablish(info, url_directory + url);
        }
        #endregion
    }

    public class CloudLoad_DataManagement : ServerIP_Settings
    {
        public List<bool> cloudLogging;
        private int counter;
        public int get_counter { get { return counter; } }

        private enum CloudLoad_TrackField { UserID, Title, Difficulty, TotalField };
        private enum CloudLoad_TrackField_TypeOfResult { Individual = 1, Main };
        private enum CloudLoad_ProfileField { UserID, RatePoint, PlayedCount, TotalField };
        private enum CloudLoad_SettingConfiguration 
        { 
            UserID, MV, NoteSpeed, AutoRetreat, 
            PrimaryDisplay, SecondaryDisplay, 
            JudgeType, Feedback_TypeA, Feedback_TypeB,
            TotalField 
        };

        private enum CloudLoad_LastSelectionVisited { AreaSelection, TrackSelection, TrackDifficuly, TotalField };
        private enum CloudLoad_PlayerSettingData { HowToPlay, Setup, Control, BGM, SE, TotalField };
        private enum CloudLoad_BattleFormationData { Slot1, Slot2, Slot3, MainSlot, TotalField };

        private enum CloudLoad_TrackList { Title, CoverImage, Difficulty_ID, Difficulty, Score, Point, ChartType, TotalField };

        public CloudLoad_DataManagement(string user, string url)
        {
            cloudLogging = new List<bool>();
            counter = 0;

            portId = user;
            urlWeb = url + url_directory;
        }

        #region MAIN (Data Handler)
        public IEnumerator LoadProgressTrack(int options)
        {
            WWWForm progress = new WWWForm();
            progress.AddField("User", portId);
            counter++;

            UnityWebRequest load = UnityWebRequest.Post(urlWeb + "MeloMelo_LoadProgress_" + options + "_2024.php", progress);
            yield return load.SendWebRequest();

            switch (options)
            {
                case 1:
                    string[] trackDataByScore = load.downloadHandler.text.Split("\n");

                    if (load.downloadHandler.text != "Empty")
                    {
                        int maxLength = (int)CloudLoad_TrackField.TotalField + (int)CloudLoad_TrackField_TypeOfResult.Main;

                        // Save Data (Main Score)
                        for (int piece = 0; piece < trackDataByScore.Length / maxLength; piece++)
                        {
                            // Data taken: UserId, Title, Difficulty, Score, Combo
                            int id = piece * maxLength;

                            int cloudScore = PlayerPrefs.GetInt(trackDataByScore[id + (int)CloudLoad_TrackField.Title] + "_score"
                                + int.Parse(trackDataByScore[id + (int)CloudLoad_TrackField.Difficulty]), 0);

                            // Get the latest score result from database
                            if (int.Parse(trackDataByScore[id + (int)CloudLoad_TrackField.TotalField]) > cloudScore)

                            {
                                // Load score data
                                PlayerPrefs.SetInt
                                (
                                    trackDataByScore[id + (int)CloudLoad_TrackField.Title] + "_score" +
                                    int.Parse(trackDataByScore[id + (int)CloudLoad_TrackField.Difficulty]),
                                    int.Parse(trackDataByScore[id + (int)CloudLoad_TrackField.TotalField])
                                );
                            }
                        }
                    }
                    break;

                case 2:
                    string[] trackDataByPoint = load.downloadHandler.text.Split("\n");

                    if (load.downloadHandler.text != "Empty")
                    {
                        int maxLength = (int)CloudLoad_TrackField.TotalField + (int)CloudLoad_TrackField_TypeOfResult.Individual;

                        // Save Data (Point System)
                        for (int piece = 0; piece < trackDataByPoint.Length / maxLength; piece++)
                        {
                            // Data taken: UserId, Title, Difficulty, Point
                            int id = piece * maxLength;

                            int cloudPoint = PlayerPrefs.GetInt(trackDataByPoint[id + (int)CloudLoad_TrackField.Title] + "_point" +
                                    int.Parse(trackDataByPoint[id + (int)CloudLoad_TrackField.Difficulty]), 0);

                            // Get the latest point result from database
                            if (int.Parse(trackDataByPoint[id + (int)CloudLoad_TrackField.TotalField]) > cloudPoint)

                            {
                                // Load point data
                                PlayerPrefs.SetInt
                                    (
                                        trackDataByPoint[id + (int)CloudLoad_TrackField.Title] + "_point" +
                                        int.Parse(trackDataByPoint[id + (int)CloudLoad_TrackField.Difficulty]),
                                        int.Parse(trackDataByPoint[id + (int)CloudLoad_TrackField.TotalField])
                                    );
                            }
                        }
                    }
                    break;

                case 3:
                    string[] trackDataByRemark = load.downloadHandler.text.Split("\n");

                    if (load.downloadHandler.text != "Empty")
                    {
                        int maxLength = (int)CloudLoad_TrackField.TotalField + (int)CloudLoad_TrackField_TypeOfResult.Individual;

                        // Save Data (Renark Status)
                        for (int piece = 0; piece < trackDataByRemark.Length / maxLength; piece++)
                        {
                            // Data taken: UserId, Title, Difficulty, Remark
                            int id = piece * maxLength;

                            int cloudRemark = PlayerPrefs.GetInt(trackDataByRemark[id + (int)CloudLoad_TrackField.Title] + "_BattleRemark_" +
                                    int.Parse(trackDataByRemark[id + (int)CloudLoad_TrackField.Difficulty]), 6);

                            // Get the latest status remark from database
                            if (int.Parse(trackDataByRemark[id + (int)CloudLoad_TrackField.TotalField]) < cloudRemark)
                            {
                                // Load remark data
                                PlayerPrefs.SetInt
                                    (
                                        trackDataByRemark[id + (int)CloudLoad_TrackField.Title] + "_BattleRemark_" +
                                        int.Parse(trackDataByRemark[id + (int)CloudLoad_TrackField.Difficulty]),
                                        int.Parse(trackDataByRemark[id + (int)CloudLoad_TrackField.TotalField])
                                    );
                            }
                        }
                    }
                    break;

                default:
                    break;
            }

            cloudLogging.Add(load.downloadHandler.text != "Empty");
            Debug.Log("Server: Load Track Successful! (Save " + options + ") [" + load.downloadHandler.text + "]");
            load.Dispose();
        }

        public IEnumerator LoadProgressProfile()
        {
            WWWForm profile = new WWWForm();
            profile.AddField("User", portId);
            counter++;

            UnityWebRequest load = UnityWebRequest.Post(urlWeb + "MeloMelo_ProfileLoader_2024.php", profile);
            yield return load.SendWebRequest();

            if (load.downloadHandler.text != string.Empty)
            {
                string[] profileData = load.downloadHandler.text.Split("\t");
                for (int piece = 0; piece < profileData.Length / (int)CloudLoad_ProfileField.TotalField; piece++)
                {
                    int id = piece * (int)CloudLoad_ProfileField.TotalField;

                    // Load ratePoint data
                    PlayerPrefs.SetInt
                        (
                            profileData[id + (int)CloudLoad_ProfileField.UserID] + "totalRatePoint",
                            int.Parse(profileData[id + (int)CloudLoad_ProfileField.RatePoint])
                        );

                    // Load playedCount data
                    PlayerPrefs.SetInt
                        (
                            profileData[id + (int)CloudLoad_ProfileField.UserID] + "PlayedCount_Data",
                            int.Parse(profileData[id + (int)CloudLoad_ProfileField.PlayedCount])
                        );
                }
            }

            cloudLogging.Add(load.downloadHandler.text != string.Empty);
            Debug.Log("Server: Load Profile Successful! [" + load.downloadHandler.text + "]");
            load.Dispose();
        }

        public IEnumerator LoadSettingCofiguration()
        {
            WWWForm config = new WWWForm();
            config.AddField("User", portId);
            counter++;

            UnityWebRequest load = UnityWebRequest.Post(urlWeb + "MeloMelo_SettingConfigOnLoad_2024.php", config);
            yield return load.SendWebRequest();

            if (load.downloadHandler.text != string.Empty)
            {
                string[] configurationData = load.downloadHandler.text.Split("\t");
                for (int piece = 0; piece < configurationData.Length / (int)CloudLoad_SettingConfiguration.TotalField; piece++)
                {
                    int id = piece * (int)CloudLoad_SettingConfiguration.TotalField;

                    // Load MV Setting data
                    PlayerPrefs.SetString("MVOption", configurationData[id + (int)CloudLoad_SettingConfiguration.MV]);

                    // Load NoteSpeed Setting data
                    PlayerPrefs.SetInt("NoteSpeed", int.Parse(configurationData[id + (int)CloudLoad_SettingConfiguration.NoteSpeed]));

                    // Load AutoRetreat Setting data
                    PlayerPrefs.SetInt("AutoRetreat", int.Parse(configurationData[id + (int)CloudLoad_SettingConfiguration.AutoRetreat]));

                    // Load Primary Display data
                    PlayerPrefs.SetInt("ScoreDisplay", int.Parse(configurationData[id + (int)CloudLoad_SettingConfiguration.PrimaryDisplay]));

                    // Load Secodary Display data
                    PlayerPrefs.SetInt("ScoreDisplay2", int.Parse(configurationData[id + (int)CloudLoad_SettingConfiguration.SecondaryDisplay]));

                    // Load Judge Type data
                    PlayerPrefs.SetInt("JudgeMeter_Setup", int.Parse(configurationData[id + (int)CloudLoad_SettingConfiguration.JudgeType]));

                    // Load Feedback 1
                    PlayerPrefs.SetInt("Feedback_Display_Type_B", int.Parse(configurationData[id + (int)CloudLoad_SettingConfiguration.Feedback_TypeA]));

                    // Load Feedback 2
                    PlayerPrefs.SetInt("Feedback_Display_Type", int.Parse(configurationData[id + (int)CloudLoad_SettingConfiguration.Feedback_TypeB]));
                }
            }

            cloudLogging.Add(load.downloadHandler.text != string.Empty);
            Debug.Log("Server: Load Gameplay Settings Successful! [" + load.downloadHandler.text + "]");
            load.Dispose();
        }

        public IEnumerator LoadPlayerSettings()
        {
            WWWForm config = new WWWForm();
            config.AddField("User", portId);
            counter++;

            UnityWebRequest load = UnityWebRequest.Post(urlWeb + "MeloMelo_Load_PlayerSettings_2024.php", config);
            yield return load.SendWebRequest();

            if (load.downloadHandler.text != string.Empty)
            {
                string[] configurationData = load.downloadHandler.text.Split("\t");
                for (int piece = 0; piece < configurationData.Length / (int)CloudLoad_PlayerSettingData.TotalField; piece++)
                {
                    int id = piece * (int)CloudLoad_PlayerSettingData.TotalField;

                    // Load game mechanics data
                    PlayerPrefs.SetString("HowToPlay_Notice", configurationData[id + (int)CloudLoad_PlayerSettingData.HowToPlay]);

                    // Load gameplay setup data
                    PlayerPrefs.SetString("BattleSetup_Guide", configurationData[id + (int)CloudLoad_PlayerSettingData.Setup]);

                    // Load control guide data
                    PlayerPrefs.SetString("Control_notice", configurationData[id + (int)CloudLoad_PlayerSettingData.Control]);

                    // Load background game music data
                    PlayerPrefs.SetFloat("BGM_VolumeGET", float.Parse(configurationData[id + (int)CloudLoad_PlayerSettingData.BGM]));

                    // Load sound effect data
                    PlayerPrefs.SetFloat("SE_VolumeGET", float.Parse(configurationData[id + (int)CloudLoad_PlayerSettingData.SE]));
                }
            }

            cloudLogging.Add(load.downloadHandler.text != string.Empty);
            Debug.Log("Server: Load Player Data Successful! [" + load.downloadHandler.text + "]");
            load.Dispose();
        }

        public IEnumerator LoadSelectionLastVisited()
        {
            WWWForm info = new WWWForm();
            info.AddField("User", portId);
            counter++;

            const string serverAPI = "MeloMelo_Load_MusicSelectionLastVisited_2024.php";
            UnityWebRequest load = UnityWebRequest.Post(urlWeb + serverAPI, info);
            yield return load.SendWebRequest();

            if (load.downloadHandler.text != "Empty")
            {
                string[] value = load.downloadHandler.text.Split('\n');

                for (int data = 0; data < value.Length / (int)CloudLoad_LastSelectionVisited.TotalField; data++)
                {
                    int id = data * (int)CloudLoad_LastSelectionVisited.TotalField;

                    // Load area selection
                    MeloMelo_TrackSelectionData selection = new MeloMelo_TrackSelectionData();
                    selection.areaTitle = value[id + (int)CloudLoad_LastSelectionVisited.AreaSelection];
                    selection.trackIndex = int.Parse(value[id + (int)CloudLoad_LastSelectionVisited.TrackSelection]);
                    selection.difficulty = int.Parse(value[id + (int)CloudLoad_LastSelectionVisited.TrackDifficuly]);
                    MeloMelo_GameSettings.selectionLisitng.Add(selection);
                }
            }

            cloudLogging.Add(load.downloadHandler.text != "Empty");
            Debug.Log("CloudLoad - " + serverAPI + " [" + load.downloadHandler.text + "]");
            load.Dispose();
        }

        public IEnumerator LoadBattleFormationData()
        {
            WWWForm info = new WWWForm();
            info.AddField("User", portId);
            counter++;

            const string serverAPI = "MeloMelo_Load_BattleFormation_2024.php";
            UnityWebRequest load = UnityWebRequest.Post(urlWeb + serverAPI, info);
            yield return load.SendWebRequest();
            
            if (load.downloadHandler.text != "Empty")
            {
                string[] characterData = load.downloadHandler.text.Split("\t");

                for (int i = 0; i < (int)CloudLoad_BattleFormationData.TotalField - 1; i++) 
                { PlayerPrefs.SetString("Slot" + (i + 1) + "_charName", characterData[i]); }

                PlayerPrefs.SetString("CharacterFront", characterData[(int)CloudLoad_BattleFormationData.MainSlot]);
            }

            cloudLogging.Add(load.downloadHandler.text != "Empty");
            Debug.Log("Server: Load Formation Data Successful! [" + load.downloadHandler.text + "]");
            load.Dispose();
        }

        public IEnumerator LoadCharacterStatusData()
        {
            WWWForm info = new WWWForm();
            info.AddField("User", portId);
            counter++;

            const string serverAPI = "MeloMelo_Load_CharacterStatusData_2024.php";
            UnityWebRequest load = UnityWebRequest.Post(urlWeb + serverAPI, info);
            yield return load.SendWebRequest();

            if (load.downloadHandler.text != "Empty")
            {
                string[] characterStatus = load.downloadHandler.text.Split("\t");

                for (int status = 0; status < characterStatus.Length / 3; status++)
                {
                    PlayerPrefs.SetInt(characterStatus[status * 3] + "_LEVEL", int.Parse(characterStatus[status * 3 + 1]));
                    PlayerPrefs.SetInt(characterStatus[status * 3] + "_EXP", int.Parse(characterStatus[status * 3 + 2]));
                }
            }

            cloudLogging.Add(load.downloadHandler.text != "Empty");
            Debug.Log("Server: Load Character Status Successful! [" + load.downloadHandler.text + "]");
            load.Dispose();
        }

        public IEnumerator LoadTrackDistributionChart()
        {
            WWWForm info = new WWWForm();
            info.AddField("User", portId);
            counter++;

            const string serverAPI = "MeloMelo_Load_TrackDistributionList_2024.php";
            UnityWebRequest load = UnityWebRequest.Post(urlWeb + serverAPI, info);
            yield return load.SendWebRequest();

            if (load.downloadHandler.text != "Empty")
            {
                string[] allChartData = load.downloadHandler.text.Split("\t");

                for (int chart = 0; chart < 3; chart++)
                {
                    string channel_Build = Application.isEditor ? "Assets/" : "MeloMelo_Data/";
                    string fileDirectory = channel_Build + "StreamingAssets/LocalData/MeloMelo_LocalSave_ChartList/TempPass_ChartData_" + (chart + 1) + ".json";

                    // Write server database into local written files
                    StreamWriter writeToLocal = new StreamWriter(fileDirectory);

                    for (int data = 0; data < allChartData.Length / (int)CloudLoad_TrackList.TotalField; data++)
                    {
                        if (chart + 1 == int.Parse(allChartData[data * (int)CloudLoad_TrackList.TotalField + (int)CloudLoad_TrackList.ChartType]))
                        {
                            string chartLocal = JsonUtility.ToJson(GetDistributionInfo
                                (
                                    allChartData[data * (int)CloudLoad_TrackList.TotalField + (int)CloudLoad_TrackList.Title],
                                    allChartData[data * (int)CloudLoad_TrackList.TotalField + (int)CloudLoad_TrackList.CoverImage],
                                    allChartData[data * (int)CloudLoad_TrackList.TotalField + (int)CloudLoad_TrackList.Difficulty_ID],
                                    allChartData[data * (int)CloudLoad_TrackList.TotalField + (int)CloudLoad_TrackList.Difficulty],
                                    int.Parse(allChartData[data * (int)CloudLoad_TrackList.TotalField + (int)CloudLoad_TrackList.Score]),
                                    int.Parse(allChartData[data * (int)CloudLoad_TrackList.TotalField + (int)CloudLoad_TrackList.Point])
                                )) + "/t";

                            writeToLocal.WriteLine(chartLocal);
                        }
                    }

                    writeToLocal.Close();
                }
            }

            cloudLogging.Add(load.downloadHandler.text != "Empty");
            Debug.Log("Server: Load Chart Distribution Successful! [" + load.downloadHandler.text + "]");
            load.Dispose();
        }
        #endregion

        #region COMPINENT (Chart Data Handler)
        private TrackEventEntry GetDistributionInfo(string title, string coverImage, string difficulty, string level, int score, int point)
        {
            TrackEventEntry template = new TrackEventEntry();
            template.title = title;
            template.cover = coverImage;
            template.difficulty = difficulty == "NORMAL" ? 1 : difficulty == "HARD" ? 2 : 3;
            template.level = level;
            template.score = score;
            template.point = point;

            return template;
        }
        #endregion
    }

    public class Extra_DataManagement : ServerIP_Settings
    {
        private enum CloudLoad_BeginnerGuide { UserID, HowToPlay, Control, BattleSetup, TotalField };

        public Extra_DataManagement(string user, string url)
        {
            portId = user;
            urlWeb = url;
        }

        public IEnumerator LoadBeginnerNoticeBoard()
        {
            WWWForm setup = new WWWForm();
            setup.AddField("User", portId);

            UnityWebRequest guide = UnityWebRequest.Post(urlWeb + "/database/transcripts/site5/MeloMelo_BeginnerGuideOnStart_2024.php", setup);
            yield return guide.SendWebRequest();

            string[] setupData = guide.downloadHandler.text.Split("\t");
            for (int piece = 0; piece < setupData.Length / (int)CloudLoad_BeginnerGuide.TotalField; piece++)
            {
                int id = piece * (int)CloudLoad_BeginnerGuide.TotalField;

                PlayerPrefs.SetString("HowToPlay_Notice", setupData[id + (int)CloudLoad_BeginnerGuide.HowToPlay]);
                PlayerPrefs.SetString("Control_notice", setupData[id + (int)CloudLoad_BeginnerGuide.Control]);
                PlayerPrefs.SetString("BattleSetup_Guide", setupData[id + (int)CloudLoad_BeginnerGuide.BattleSetup]);
            }

            Debug.Log("Server: Load Beginner Guide Successful!");
            guide.Dispose();
        }
    }

    public class Authenticate_DataManagement : ServerIP_Settings
    {
        private bool LogOnSucess;
        public bool get_success { get { return LogOnSucess; } }

        public Authenticate_DataManagement(string user, string url)
        {
            LogOnSucess = false;
            portId = user; 
            urlWeb = url + url_directory;
        }

        public IEnumerator GetAuthenticationFromServer(string key)
        {
            WWWForm login = new WWWForm();
            login.AddField("User", portId);
            login.AddField("Pass", key);

            UnityWebRequest authenticate = UnityWebRequest.Post(urlWeb + "MeloMelo_TempPassB.php", login);
            yield return authenticate.SendWebRequest();

            LogOnSucess = authenticate.downloadHandler.text.Split("\n")[0] == "Transfer Successful!";
            LoginTemp_Script.thisTemp.LoadEntryPass(authenticate.downloadHandler.text.Split("\n")[0], authenticate.downloadHandler.text.Split("\n")[1]);
            authenticate.Dispose();
        }

        public IEnumerator CreateNewEntryOnServer(string key, string playerName)
        {
            WWWForm login = new WWWForm();
            login.AddField("User", portId);
            login.AddField("Pass", key);
            login.AddField("Player", playerName);

            UnityWebRequest authenticate = UnityWebRequest.Post(urlWeb + "MeloMelo_TempPass.php", login);
            yield return authenticate.SendWebRequest();

            LogOnSucess = true;
            LoginTemp_Script.thisTemp.SaveEntryPass(authenticate.downloadHandler.text);
            authenticate.Dispose();
        }
    }

    public class CloudUsage_TempServices : ServerIP_Settings
    {
        public readonly string playerId_noEntry = "00XX00XX2024";

        private string playerId;
        public string get_playerId { get { return playerId; } }

        public CloudUsage_TempServices(string sourceHTTP)
        {
            playerId = "---";
            urlWeb = sourceHTTP + url_directory;
        }

        public IEnumerator GeneratingPlayerId()
        {
            WWWForm getData = new WWWForm();
            UnityWebRequest playerId_viaServer = UnityWebRequest.Post(urlWeb + "MeloMelo_TempPlayerIdGenerator.php", getData);
            yield return playerId_viaServer.SendWebRequest();

            playerId = (playerId_viaServer.downloadHandler.text != playerId_noEntry ? playerId_viaServer.downloadHandler.text : playerId_noEntry);
            playerId_viaServer.Dispose();

            LoginTemp_Script.thisTemp.LoginPass_ProcessConfirmation();
        }

        public IEnumerator AccountBinding(bool returnEntry)
        {
            WWWForm getData = new WWWForm();
            getData.AddField("PlayerId", playerId);
            getData.AddField("Status", returnEntry ? 2 : 1);

            UnityWebRequest accountBind = UnityWebRequest.Post(urlWeb + "MeloMelo_AccountBindingForTemp.php", getData);
            yield return accountBind.SendWebRequest();
            accountBind.Dispose();
        }
    }

    public class CloudServices_ControlPanel : ServerIP_Settings
    {
        public CloudServices_ControlPanel(string url)
        {
            urlWeb = url + url_directory;
        }

        #region MAIN
        public IEnumerator CheckNetwork_GlobalStatus(GameObject icon_reference, string networkName)
        {
            WWWForm net = new WWWForm();
            net.AddField("Title", networkName);

            UnityWebRequest getNet = UnityWebRequest.Post(urlWeb + "UnityLogin_InternetChecker.php", net);
            yield return getNet.SendWebRequest();

            if (getNet.downloadHandler.text != string.Empty) RedirectNetworkControl(icon_reference, getNet.downloadHandler.text);
            Debug.Log("Server Output: " + getNet.downloadHandler.text);
            getNet.Dispose();
        }

        public IEnumerator CheckNetwork_ServerStatus(string networkName, RawImage reference, int index)
        {
            WWWForm net = new WWWForm();
            net.AddField("Title", networkName);

            UnityWebRequest getNet = UnityWebRequest.Post(urlWeb + "UnityLogin_InternetChecker.php", net);
            yield return getNet.SendWebRequest();

            RedirectServerPage(reference, index, getNet.downloadHandler.text);
            Debug.Log("Server Panel [" + reference.transform.GetChild(0).GetComponent<Text>().text + " (" + index + ")]: " + getNet.downloadHandler.text);
            getNet.Dispose();
        }

        public IEnumerator CheckNetwork_IDInspection(string user)
        {
            WWWForm inspectID = new WWWForm();
            inspectID.AddField("ID", user);

            UnityWebRequest liveInspecting = UnityWebRequest.Post(urlWeb + "MeloMelo_LiveID_Inspector.php", inspectID);
            yield return liveInspecting.SendWebRequest();

            if (liveInspecting.downloadHandler.text != string.Empty) RedirectLoginPage(liveInspecting.downloadHandler.text);
            Debug.Log("ID Verification Check: " + liveInspecting.downloadHandler.text);
            liveInspecting.Dispose();
        }
        #endregion

        #region COMPONENT 
        private void RedirectNetworkControl(GameObject icon, string status)
        {
            icon.GetComponent<MeloMelo_NetworkChecker>().GetServerNetworkLive(status);
        }

        private void RedirectServerPage(RawImage reference, int index, string status)
        {
            ServerGateway_Script.thisServer.GetServerConntectedToCloud(reference, index, status);
        }

        private void RedirectLoginPage(string received_data)
        {
            LoginTemp_Script.thisTemp.GetServer_CheckedID(received_data);
        }
        #endregion
    }

    // Not In Use: Old Data Management
    public class Data_Management
    {
        private string urlWeb = string.Empty;

        public Data_Management(string url)
        {
            urlWeb = url;
        }

        // Requesting Server: Last Played Date
        public IEnumerator SaveLastPlayedDate(string user)
        {
            WWWForm fill = new WWWForm();
            fill.AddField("User", user);

            // Process to server: Mark date allocation
            UnityWebRequest save = UnityWebRequest.Post(urlWeb + "/database/transcripts/site5/MeloMelo_TimeStamp.php", fill);
            yield return save.SendWebRequest();

            // Load and clean
            save.Dispose();
        }

        // Requesting Server: Save Achievement points
        public IEnumerator SaveScoringPoint(string user, string title, int difficuly, int score, int maxPoint)
        {
            WWWForm fill = new WWWForm();
            fill.AddField("User", user);

            // Fill-up: Music Information
            fill.AddField("Title", title);
            fill.AddField("Difficulty", difficuly);

            // Fill-up: Data
            fill.AddField("Data", score);
            fill.AddField("Data2", maxPoint);
                
            // Process to server: Reference from Option
            UnityWebRequest save = UnityWebRequest.Post(urlWeb + "/database/transcripts/site5/MeloMelo_SaveScoringPoint.php", fill);
            yield return save.SendWebRequest();

            // Load and clean
            save.Dispose();
        }

        // Requesting Server: Load Achievement points
        public IEnumerator LoadScoringPoint(string user)
        {
            WWWForm fill = new WWWForm();
            fill.AddField("User", user);

            // Process to server: Reference from Option
            UnityWebRequest load = UnityWebRequest.Post(urlWeb + "/database/transcripts/site5/MeloMelo_LoadScoringPoint.php", fill);
            yield return load.SendWebRequest();

            // Server Database: Output
            if (load.downloadHandler.text != "")
            {
                string[] info = load.downloadHandler.text.Split('\n');

                for (int i = 0; i < (info.Length / 4); i++)
                {
                    // Fomrat: User, Difficulty, Points, MaxPoints
                    PlayerPrefs.SetInt(info[i * 4] + "_point" + info[i * 4 + 1], int.Parse(info[i * 4 + 2]));
                    PlayerPrefs.SetInt(info[i * 4] + "_maxPoint" + info[i * 4 + 1], int.Parse(info[i * 4 + 3]));
                }
            }

            // Load and clean
            load.Dispose();
        }

        // Requesting Server: Save Achievement Stats
        public IEnumerator SaveProgress(string user, string title, int difficulty, float score, string rank)
        {
            WWWForm temp2 = new WWWForm();
            temp2.AddField("User", user);

            // Fill-up: Music Information
            temp2.AddField("Title", title);
            if (difficulty == 1) { temp2.AddField("Difficulty", "NORMAL"); } 
            else if (difficulty == 2) { temp2.AddField("Difficulty", "HARD"); }
            else { temp2.AddField("Difficulty", "ULTIMATE"); }

            // Fill-up: Achievement Information
            temp2.AddField("Critical_Perfect", GameManager.thisManager.getJudgeWindow.get_perfect2);
            temp2.AddField("Perfect", GameManager.thisManager.getJudgeWindow.get_perfect);
            temp2.AddField("Bad", GameManager.thisManager.getJudgeWindow.get_bad);
            temp2.AddField("Miss", GameManager.thisManager.getJudgeWindow.get_miss);
            temp2.AddField("Score", (int)score);
            temp2.AddField("Rank", rank);
            temp2.AddField("Remark", PlayerPrefs.GetInt(title + "_BattleRemark_" + difficulty, 0));

            // Option: Save
            temp2.AddField("Option", 1);

            // Process to server: Reference from Option
            UnityWebRequest save2 = UnityWebRequest.Post(urlWeb + "/database/transcripts/site5/UnityLogin_MeloMelo2.php", temp2);
            yield return save2.SendWebRequest();

            // Load and Clean
            save2.Dispose();
        }

        // Requesting Server: Save Achievement Stats
        public IEnumerator CarryOverData(string user, string versionGET, int versionIndex, int counter)
        {
            WWWForm temp2 = new WWWForm();
            temp2.AddField("User", user);

            // Fill-up: Past version database
            temp2.AddField("VersionID_get", versionGET);
            temp2.AddField("VersionIndex", versionIndex);
            temp2.AddField("MusicCounter_load", counter);

            // Process to server: Waiting to be upgraded
            UnityWebRequest save_S = UnityWebRequest.Post(urlWeb + "/database/transcripts/site5/UnityLogin_MeloMelo3b.php", temp2);
            yield return save_S.SendWebRequest();

            // Server Respone: COMPLETED!
            if (save_S.downloadHandler.text == "COMPLETED") { PlayerPrefs.SetString("SaveProgressCompleted", "T"); }
            else { PlayerPrefs.SetString("SaveProgressCompleted", "F"); }

            // Load and Clean
            save_S.Dispose();
        }

        // Requesting Server: Save Progress Stats
        public IEnumerator SaveProgress_Server(string user, string title, int difficulty, float score, string rank, int currentN, int maxN)
        {
            WWWForm temp = new WWWForm();
            temp.AddField("User", user);

            // Fill-up: Music Information
            temp.AddField("Title", title);
            if (difficulty == 1) { temp.AddField("Difficulty", "NORMAL"); }
            else if (difficulty == 2) { temp.AddField("Difficulty", "HARD"); }
            else { temp.AddField("Difficulty", "ULTIMATE"); }

            // Fill-up: Achievement Information
            temp.AddField("Critical_Perfect", GameManager.thisManager.getJudgeWindow.get_perfect2);
            temp.AddField("Perfect", GameManager.thisManager.getJudgeWindow.get_perfect);
            temp.AddField("Bad", GameManager.thisManager.getJudgeWindow.get_bad);
            temp.AddField("Miss", GameManager.thisManager.getJudgeWindow.get_miss);
            temp.AddField("CurrentNote", currentN);
            temp.AddField("MaxNote", maxN);
            temp.AddField("Score", (int)score);
            temp.AddField("Rank", rank);
            temp.AddField("Remark", PlayerPrefs.GetInt(title + "_BattleRemark_" + difficulty, 0));

            // Option: Load
            temp.AddField("Option", 1);

            // Process to server: Reference from Option
            UnityWebRequest save = UnityWebRequest.Post(urlWeb + "/database/transcripts/site5/UnityLogin_MeloMelo.php", temp);
            yield return save.SendWebRequest();

            // Load and Clean
            save.Dispose();
        }

        // Requesting Server: Save Progress Stats
        public IEnumerator LoadProgress_Server(string user)
        {
            WWWForm temp3 = new WWWForm();
            temp3.AddField("User", user);
            temp3.AddField("Option", 2);

            // Process to server respone
            UnityWebRequest load = UnityWebRequest.Post(urlWeb + "/database/transcripts/site5/UnityLogin_MeloMelo.php", temp3);
            yield return load.SendWebRequest();

            // Load database through server: PlayedCount
            string[] length = load.downloadHandler.text.Split('\n');
            PlayerPrefs.DeleteKey("ServerIP");
            try { PlayerPrefs.SetInt("PlayedCount_Data", int.Parse(length[1])); }
            catch { PlayerPrefs.SetString("ServerIP", "F"); LoginPage_Script.thisPage.Icon.transform.GetChild(1).GetComponent<Text>().text = "SERVER ERROR!"; }

            // Load and clean
            load.Dispose();
        }

        // Requesting Server: Load Achievement Stats
        public IEnumerator LoadProgress(string user)
        {
            WWWForm temp2 = new WWWForm();
            temp2.AddField("Option", 2);
            temp2.AddField("User", user);

            // Process to server respone
            UnityWebRequest load = UnityWebRequest.Post(urlWeb + "/database/transcripts/site5/UnityLogin_MeloMelo2.php", temp2);
            yield return load.SendWebRequest();

            // Load database through server: Highscore
            string[] info = load.downloadHandler.text.Split('\t');
            string[] length = load.downloadHandler.text.Split('\n');
            PlayerPrefs.DeleteKey("ServerIP");

            try
            {
                for (int i = 0; i < int.Parse(length[1]); i++)
                {
                    switch (info[(i * 8) + 1])
                    {
                        case "NORMAL": // Normal Achievement
                            PlayerPrefs.SetInt(info[i * 8] + "_score1", int.Parse(info[(i * 8) + 2]));
                            PlayerPrefs.SetString(info[i * 8] + "_rank1", info[(i * 8) + 3]);

                            PlayerPrefs.SetInt(info[i * 8] + "_BattleRemark_1", int.Parse(info[(i * 8) + 4]));                           
                            if (PlayerPrefs.GetInt(info[i * 8] + "_score1", 0) >= 1000000) { PlayerPrefs.SetString(info[i * 8] + "_score1_SS", "T"); }

                            if (PlayerPrefs.GetInt(info[i * 8] + "_BattleRemark_1") == 5)
                            {
                                PlayerPrefs.SetInt(info[i * 8] + "_score1", 0);
                            }
                            break;

                        case "HARD": // Hard Achievement
                            PlayerPrefs.SetInt(info[i * 8] + "_score2", int.Parse(info[(i * 8) + 2]));
                            PlayerPrefs.SetString(info[i * 8] + "_rank2", info[(i * 8) + 3]);

                            PlayerPrefs.SetInt(info[i * 8] + "_BattleRemark_2", int.Parse(info[(i * 8) + 4]));
                            if (PlayerPrefs.GetInt(info[i * 8] + "_score2", 0) >= 1000000) { PlayerPrefs.SetString(info[i * 8] + "_score2_SS", "T"); }

                            if (PlayerPrefs.GetInt(info[i * 8] + "_BattleRemark_2") == 5)
                            {
                                PlayerPrefs.SetInt(info[i * 8] + "_score2", 0);
                            }
                            break;

                        case "ULTIMATE": // Ultimate Achievement
                            PlayerPrefs.SetInt(info[i * 8] + "_score3", int.Parse(info[(i * 8) + 2]));
                            PlayerPrefs.SetString(info[i * 8] + "_rank3", info[(i * 8) + 3]);

                            PlayerPrefs.SetInt(info[i * 8] + "_BattleRemark_3", int.Parse(info[(i * 8) + 4]));
                            if (PlayerPrefs.GetInt(info[i * 8] + "_score3", 0) >= 1000000) { PlayerPrefs.SetString(info[i * 8] + "_score3_SS", "T"); }

                            if (PlayerPrefs.GetInt(info[i * 8] + "_BattleRemark_3") == 5)
                            {
                                PlayerPrefs.SetInt(info[i * 8] + "_score3", 0);
                            }
                            break;
                    }
                }
            }
            catch { PlayerPrefs.SetString("ServerIP", "F"); LoginPage_Script.thisPage.Icon.transform.GetChild(1).GetComponent<Text>().text = "SERVER ERROR!"; }

            // Load and clean
            load.Dispose();
        }

        // Requesting Server: Load Battle-Setup QuickSetup
        public IEnumerator LoaderPackage(string user)
        {
            if (user != "GUEST")
            {
                WWWForm temp3 = new WWWForm();
                temp3.AddField("Option", 3);
                temp3.AddField("user", user);

                UnityWebRequest loader = UnityWebRequest.Post(urlWeb + "/database/transcripts/site5/UnityLogin_MeloMelo.php", temp3);
                yield return loader.SendWebRequest();

                string[] info = loader.downloadHandler.text.Split('\t');
                PlayerPrefs.DeleteKey("ServerIP");
                
                if (loader.downloadHandler.text != "error")
                {
                    try
                    {
                        PlayerPrefs.SetString("MVOption", info[1]);
                        PlayerPrefs.SetInt("NoteSpeed", int.Parse(info[0]));
                        PlayerPrefs.SetString("AutoSkillOption", info[9]);

                        for (int i = 0; i < 3; i++) { PlayerPrefs.SetString("Slot" + (i + 1) + "_charName", info[2 + i]); }
                        PlayerPrefs.SetString("CharacterFront", info[5]);
                        PlayerPrefs.SetString("HowToPlay_Notice", info[6]);
                        PlayerPrefs.SetString("Control_notice", info[6]);
                        PlayerPrefs.SetString("BattleSetup_Guide", info[6]);

                        PlayerPrefs.SetInt(user + "_unitGradeRate", int.Parse(info[7]));
                        PlayerPrefs.SetInt(user + "totalRatePoint", int.Parse(info[8]));
                        PlayerPrefs.SetString("Account_Version", info[10]);
                        PlayerPrefs.SetInt("VersionNum_index", int.Parse(info[11]));
                        PlayerPrefs.SetInt("RequestRatePoint", int.Parse(info[12]));

                        if (PlayerPrefs.GetInt("VersionNum_index", 0) == StartMenu_Script.thisMenu.get_versionNum && PlayerPrefs.GetInt("RequestRatePoint", 0) == 0) 
                        { PlayerPrefs.SetString("Account_Auth", "T"); }
                        else { PlayerPrefs.SetString("Account_Auth", "F"); }
                    }
                    catch { PlayerPrefs.SetString("ServerIP", "F"); LoginPage_Script.thisPage.Icon.transform.GetChild(1).GetComponent<Text>().text = "SERVER ERROR!"; }
                }

                // Load and clean
                loader.Dispose();
            }
        }

        // Requesting Server: Update Version
        public IEnumerator SaveLoadVersionAccount(string user, int version, string versionID)
        {
            WWWForm temp_versionLoader = new WWWForm();
            temp_versionLoader.AddField("user", user);
            temp_versionLoader.AddField("versionD", version);
            temp_versionLoader.AddField("version_ID", versionID);

            UnityWebRequest save_V = UnityWebRequest.Post(urlWeb + "/database/transcripts/site5/MeloMelo_AccountVersionUpdater.php", temp_versionLoader);
            yield return save_V.SendWebRequest();

            // Load and Clean
            save_V.Dispose();
        }

        // Requesting Server: Save Battle-Setup QuickSetup
        public IEnumerator SaveSettings(string user, int select, string select2, int speed, string mv, int gradePoint, int PP)
        {
            WWWForm temp_settings = new WWWForm();

            temp_settings.AddField("user", user);
            temp_settings.AddField("select", select);
            temp_settings.AddField("select2", select2);
            temp_settings.AddField("speed", speed);
            temp_settings.AddField("mv", mv);

            temp_settings.AddField("gradePoint", gradePoint);
            temp_settings.AddField("PerformanceP", PP);
           
            for (int i = 0; i < 3; i++) { temp_settings.AddField("Slot" + (i+1) + "_unit", PlayerPrefs.GetString("Slot" + (i+1) + "_charName", "None")); }
            temp_settings.AddField("CharacterFront", PlayerPrefs.GetString("CharacterFront", "NA"));
            temp_settings.AddField("HTP_out", PlayerPrefs.GetString("HowToPlay_Notice", "T"));
            temp_settings.AddField("AutoSkill", PlayerPrefs.GetString("AutoSkillOption", "T"));

            UnityWebRequest save_S = UnityWebRequest.Post(urlWeb + "/database/transcripts/site5/UnityLogin_MeloMelo3.php", temp_settings);
            yield return save_S.SendWebRequest();

            if (save_S.downloadHandler.text == "COMPLETED") { PlayerPrefs.SetString("SaveProgressCompleted", "T"); }
            else { PlayerPrefs.SetString("SaveProgressCompleted", "F"); }

            // Load and Clean
            save_S.Dispose();
        }

        // Requesting Server: Save Character Data
        public IEnumerator SaveCharacterData(string user, string charType, int level, int exp, int maxExp, string charName)
        {
            WWWForm temp_char = new WWWForm();
            temp_char.AddField("Option", 1);

            temp_char.AddField("user", user);
            temp_char.AddField("characterName", charType);
            temp_char.AddField("characterLevel", level);
            temp_char.AddField("characterExp", exp);
            temp_char.AddField("characterMaxXP", maxExp);
            temp_char.AddField("characterNameID", charName);

            UnityWebRequest save_C = UnityWebRequest.Post(urlWeb + "/database/transcripts/site5/MeloMelo_SaveLoadCharacterStatus.php", temp_char);
            yield return save_C.SendWebRequest();

            // Load and Clean
            save_C.Dispose();
        }

        // Requesting Server: Load Character Data
        public IEnumerator LoadCharacterData(string user)
        {
            if (user != "GUEST")
            {
                WWWForm temp_charL = new WWWForm();
                temp_charL.AddField("Option", 2);
                temp_charL.AddField("user", user);

                UnityWebRequest load_C = UnityWebRequest.Post(urlWeb + "/database/transcripts/site5/MeloMelo_SaveLoadCharacterStatus.php", temp_charL);
                yield return load_C.SendWebRequest();

                string[] info = load_C.downloadHandler.text.Split('\t');
                string[] length = load_C.downloadHandler.text.Split('\n');
                PlayerPrefs.DeleteKey("ServerIP");

                try
                {
                    for (int i = 0; i < int.Parse(length[1]); i++)
                    {
                        PlayerPrefs.SetInt(info[(i * 3)] + "_LEVEL", int.Parse(info[(i * 2) + 1]));
                        PlayerPrefs.SetInt(info[(i * 3)] + "_EXP", int.Parse(info[(i * 3) + 2]));
                    }
                }
                catch { PlayerPrefs.SetString("ServerIP", "F"); LoginPage_Script.thisPage.Icon.transform.GetChild(1).GetComponent<Text>().text = "SERVER ERROR!"; }

                // Load and clean
                load_C.Dispose();
            }
        }

        // Requesting Server: Save Battle-Progress Data
        public IEnumerator SaveBattleStatus(string user, string title, int difficulty, int areaDifficulty, string status, int techScore, int techC)
        {
            WWWForm temp_saveBattle = new WWWForm();
            temp_saveBattle.AddField("Option", 1);
            temp_saveBattle.AddField("user", user);

            temp_saveBattle.AddField("musicTitle", title);
            if (difficulty == 1) { temp_saveBattle.AddField("musicD", "NORMAL"); }
            else { temp_saveBattle.AddField("musicD", "HARD"); }

            switch (areaDifficulty)
            {
                case 1:
                    temp_saveBattle.AddField("areaD", "Beginner");
                    break;

                case 2:
                    temp_saveBattle.AddField("areaD", "Intermidate");
                    break;

                case 3:
                    temp_saveBattle.AddField("areaD", "Advance");
                    break;
            }

            temp_saveBattle.AddField("battleStatus", status);
            temp_saveBattle.AddField("tech", techScore);
            temp_saveBattle.AddField("tech_C", techC);

            UnityWebRequest saveBattle = UnityWebRequest.Post(urlWeb + "/database/transcripts/site5/MeloMelo_SaveLoadBattleStatus.php", temp_saveBattle);
            yield return saveBattle.SendWebRequest();

            // Load and Clean
            saveBattle.Dispose();
        }

        // Requesting Server: Load Battle-Progress Data
        public IEnumerator LoadBattleStatus(string user)
        {
            if (user != "GUEST")
            {
                WWWForm temp_loadBattle = new WWWForm();
                temp_loadBattle.AddField("Option", 2);
                temp_loadBattle.AddField("user", user);

                UnityWebRequest load_C = UnityWebRequest.Post(urlWeb + "/database/transcripts/site5/MeloMelo_SaveLoadBattleStatus.php", temp_loadBattle);
                yield return load_C.SendWebRequest();

                string[] info = load_C.downloadHandler.text.Split('\t');
                string[] length = load_C.downloadHandler.text.Split('\n');
                PlayerPrefs.DeleteKey("ServerIP");

                try
                {
                    for (int i = 0; i < int.Parse(length[1]); i++)
                    {
                        PlayerPrefs.SetString(info[(i * 5)] + "_SuccessBattle_" + GetMusicDiffi(info[(i * 5) + 1]) + GetAreaDiffi(info[(i * 5) + 2]), info[(i * 5) + 3]);
                        PlayerPrefs.SetInt(info[(i * 5)] + "_techScore" + GetMusicDiffi(info[i * 5] + 1) + GetAreaDiffi(info[(i * 5) + 2]), int.Parse(info[(i * 5) + 4]));
                    }
                }
                catch { PlayerPrefs.SetString("ServerIP", "F"); LoginPage_Script.thisPage.Icon.transform.GetChild(1).GetComponent<Text>().text = "SERVER ERROR!"; }

                // Load and clean
                load_C.Dispose();
            }
        }

        int GetMusicDiffi(string index)
        {
            if (index == "NORMAL") { return 1; }
            else { return 2; }
        }

        int GetAreaDiffi(string index)
        {
            switch(index)
            {
                case "Beginner":
                    return 1;

                case "Intermidate":
                    return 2;

                default:
                    return 3;
            }
        }

        // Requesting Server: Save LastPointer
        public IEnumerator SaveAreaLastPoint(string user, string area, int lastMusic, string lastDiff)
        {
            WWWForm temp_savePoint = new WWWForm();
            temp_savePoint.AddField("Option", 1);
            temp_savePoint.AddField("user", user);

            temp_savePoint.AddField("areaT", area);
            temp_savePoint.AddField("lastSelect_M", lastMusic);
            temp_savePoint.AddField("lastSelect_D", lastDiff);

            UnityWebRequest savePointAccess = UnityWebRequest.Post(urlWeb + "/database/transcripts/site5/MeloMelo_SaveLoadLastPointer.php", temp_savePoint);
            yield return savePointAccess.SendWebRequest();

            // Load and Clean
            savePointAccess.Dispose();
        }

        // Requesting Server: Load LastPointer
        public IEnumerator LoadAreaLastPoint(string user)
        {
            WWWForm temp_loadPoint = new WWWForm();
            temp_loadPoint.AddField("Option", 2);
            temp_loadPoint.AddField("user", user);

            UnityWebRequest loadPointAccess = UnityWebRequest.Post(urlWeb + "/database/transcripts/site5/MeloMelo_SaveLoadLastPointer.php", temp_loadPoint);
            yield return loadPointAccess.SendWebRequest();

            string[] info = loadPointAccess.downloadHandler.text.Split('\t');
            string[] length = loadPointAccess.downloadHandler.text.Split('\n');
            PlayerPrefs.DeleteKey("ServerIP");

            for (int i = 0; i < int.Parse(length[1]); i++)
            {
                try
                {
                    PlayerPrefs.SetInt(info[i * 3] + "_LastMusic", int.Parse(info[i * 3 + 1]));
                    PlayerPrefs.SetString(info[i * 3] + "_LastDiffi", info[i * 3 + 2]);
                }
                catch { PlayerPrefs.SetString("ServerIP", "F"); LoginPage_Script.thisPage.Icon.transform.GetChild(1).GetComponent<Text>().text = "SERVER ERROR!"; }

                // Load and clean
                loadPointAccess.Dispose();
            }
        }

        // Requesting Server: Retrieve and verify login
        public IEnumerator LoginInformation(string user, string pass)
        {
            WWWForm temp2 = new WWWForm();
            temp2.AddField("user", user);
            temp2.AddField("pass", pass);
            temp2.AddField("gameId_version", StartMenu_Script.thisMenu.get_versionNum);         
            
            UnityWebRequest verify = UnityWebRequest.Post(urlWeb + "/database/transcripts/site5/UnityLogin_MeloMelo_Login.php", temp2);
            yield return verify.SendWebRequest();

            if (verify.downloadHandler.text != "error") 
            { 
                PlayerPrefs.DeleteAll(); PlayerPrefs.SetString("UserID_Login", user);
                PlayerPrefs.SetString("Account_Autsh", "T");
                if (verify.downloadHandler.text == "error2") { PlayerPrefs.SetInt("Server_NotAvailable", 1); }
            }
            else
            {
                PlayerPrefs.SetString("Account_Autsh", "F");
                PlayerPrefs.DeleteKey("UserID_Login");
            }

            LoginPage_Script.thisPage.Invoke("UpdateAccount", 2);

            // Load and Clean
            verify.Dispose();
        }

        // Requesting Server: Retrieve Score to LeaderBoard
        public IEnumerator LeaderBoard_Informtaion(int index_ref, string title_ref, int difficulty_ref)
        {
            WWWForm leaderboard = new WWWForm();
            leaderboard.AddField("title", title_ref);
            leaderboard.AddField("difficulty", difficulty_ref);

            UnityWebRequest loadBoard = UnityWebRequest.Post(urlWeb + "/database/transcripts/site5/MeloMelo_LeaderBoard_Data.php", leaderboard);
            yield return loadBoard.SendWebRequest();

            // denniswong10, 294240, F, denniswong10, 1000000, X, denniswong10, 525011, D
            string[] info = loadBoard.downloadHandler.text.Split('\t');

            for (int i = 0; i < (info.Length / 3); i++)
            {
                if (i < index_ref)
                {
                    PlayerPrefs.SetInt(title_ref + "_score" + difficulty_ref + (i + 1) + "_Board_scoreOnly", int.Parse(info[i * 3 + 1]));
                    PlayerPrefs.SetString(title_ref + "_score" + difficulty_ref + (i + 1) + "_Board_user", info[i * 3]);
                    PlayerPrefs.SetString(title_ref + "_score" + difficulty_ref + (i + 1) + "_Board_score", "Score: " + info[i * 3 + 1] + " | Rank: " + info[i * 3 + 2]);
                }
                else
                {
                    PlayerPrefs.SetString(title_ref + "_score" + difficulty_ref + (i + 1) + "_Board_user", "New Entry");
                    PlayerPrefs.SetString(title_ref + "_score" + difficulty_ref + (i + 1) + "_Board_score", "- No Record -");
                }
            }

            // Update it when done
            //SelectionMenu_Script.thisSelect.get_selection.UpdateLeaderBoard_sign();

            // Load and clean
            loadBoard.Dispose();
        }

        // Requesting Server: Retrieve Competitor List
        public IEnumerator MemberRankingBoard()
        {
            WWWForm ranking_output = new WWWForm();
            UnityWebRequest ranking = UnityWebRequest.Post(urlWeb + "/database/transcripts/site5/MeloMelo_MemberRanking_Data.php", ranking_output);
            yield return ranking.SendWebRequest();

            string[] info = ranking.downloadHandler.text.Split('\t');
            //Menu.thisMenu.GlobalRankingBoard(info);

            // Load and clean
            ranking.Dispose();
        }
    }
}

// MeloMelo: Performance Meter
namespace MeloMelo_RatingMeter
{
    public class RateMeter
    {
        private int score = 0;
        private int performanceRate = 0;
        private int totalRate = 0;

        int[] rankX_Rate = { 0, 70, 100, 130, 160, 190, 214, 236, 255, 271, 290, 315, 331, 356, 375, 391, 415 };
        int[] rankSS_Rate = { 0, 60, 90, 120, 150, 180, 204, 226, 245, 261, 280, 305, 321, 346, 365, 381, 405 };
        int[] rankS_Rate = { 0, 50, 80, 110, 140, 170, 194, 216, 235, 251, 270, 295, 311, 336, 355, 371, 395 };
        int[] rankA_Rate = { 0, 40, 70, 100, 130, 160, 184, 206, 225, 241, 260, 285, 301, 326, 345, 361, 385 };

        // Base Setup: Insert Data
        public RateMeter(string user, int _score)
        {
            score = _score;    
            UpdateContent_Text(user);
        }

        // Check Content: Update Data
        public int CheckFor_IncreaseRate(float level)
        {
            float constant = level - (int)level;
            totalRate = (int)(constant * 10);

            if (score >= 1000000) return rankX_Rate[(int)level] + totalRate + GetTotalRate(totalRate);
            else if (score >= 980000) return rankSS_Rate[(int)level] + totalRate + GetTotalRate(totalRate);
            else if (score >= 950000) return rankS_Rate[(int)level] + totalRate + GetTotalRate(totalRate);
            else if (score >= 900000) return rankA_Rate[(int)level] + totalRate + GetTotalRate(totalRate);
            else return score / 1000000 * (rankX_Rate[(int)level] + totalRate) + GetTotalRate(totalRate);
        }

        // Check Content: Receiver
        private void UpdateContent_Text(string user)
        {
            performanceRate = PlayerPrefs.GetInt(user + "totalRatePoint", 0);
            GameObject.Find("RateStatus").transform.GetChild(0).GetComponent<Text>().text = "Performance Rate: " + performanceRate;
        }

        private int GetTotalRate(int constant)
        {
            int midRange = 1 + -(5 - constant);

            if (midRange > 0)
                return midRange * 3;
            else
                return 0;
        }
    }

    public class RatePointIndicator
    {
        private int[] ratingCaterogy = { 18500, 18000, 16000, 13000, 10000, 6000, 3000, 1, 0 };
        private string[] ratingTitle = { "Rhodonite", "Gold", "Amethyst", "Topaz", "Red", "Blue", "Green", "White", "UnRated" };
        private Color[] BorderColor = { Color.magenta, Color.yellow, new Color32(66, 0, 255, 255), new Color32(255, 128, 0, 255), Color.red, Color.blue, Color.green, Color.white, Color.black };
        private string profileFinder = "Profile";

        private void UpdateRatePointMeter(int current)
        {
            for (int maxRate = 0; maxRate < ratingCaterogy.Length; maxRate++)
            {
                if (current >= ratingCaterogy[maxRate])
                {
                    GameObject.Find(profileFinder).transform.GetChild(3).GetComponent<Slider>().minValue = (maxRate != ratingCaterogy.Length - 1) ? ratingCaterogy[maxRate] : ratingCaterogy[ratingCaterogy.Length - 1];
                    GameObject.Find(profileFinder).transform.GetChild(3).GetComponent<Slider>().maxValue = (maxRate - 1 > -1) ? ratingCaterogy[maxRate - 1] : ratingCaterogy[ratingCaterogy.Length - 1];
                    GameObject.Find(profileFinder).transform.GetChild(3).GetComponent<Slider>().value = current;
                    GameObject.Find(profileFinder).transform.GetChild(6).GetComponent<Text>().text = ratingTitle[maxRate];
                    GameObject.Find(profileFinder).transform.GetChild(7).GetComponent<Text>().text = FindMaxOutRatePoint(current);
                    break;
                }
            }
        }

        private string FindMaxOutRatePoint(int current)
        {
            if (current >= ratingCaterogy[0]) 
                return "- MAX OUT -";
            else
                return GameObject.Find(profileFinder).transform.GetChild(3).GetComponent<Slider>().maxValue - current + " more point" + ((current > 1) ? "s" : "");
        }

        public void ProfileUpdate(string user, bool exp, string label_1, string label_2)
        {
            GameObject.Find(profileFinder).transform.GetChild(0).GetComponent<Text>().text = user;
            GameObject.Find(profileFinder).transform.GetChild(2).GetComponent<Text>().text = label_1 + PlayerPrefs.GetInt(user + "PlayedCount_Data", 0);
            GameObject.Find(profileFinder).transform.GetChild(4).GetComponent<Text>().text = label_2 + PlayerPrefs.GetInt(user + "totalRatePoint", 0);

            UpdateRatePointMeter(PlayerPrefs.GetInt(user + "totalRatePoint", 0));

            UpdateColor_Border(user);
        }

        public void UpdateColor_Border(string user, string profileFinder)
        {
            this.profileFinder = profileFinder;
            UpdateColor_Border(user);
        }

        public void UpdateColor_Border(string user)
        {
            // Update Border Color
            for (int i = 0; i < ratingCaterogy.Length; i++)
            {
                if (PlayerPrefs.GetInt(user + "totalRatePoint", 0) >= ratingCaterogy[i])
                {
                    GameObject.Find(profileFinder).GetComponent<RawImage>().color = BorderColor[i];
                    break;
                }
            }
        }
    }
}

// MeloMelo: RPG Editor
namespace MeloMelo_RPGEditor
{
    public class CharacterStats
    {
        private string name;
        private int level;
        private int experience;
        private int health;
        private int strength;
        private int vitality;
        private int magic;

        public CharacterStats()
        {
            name = "???";
            level = 0;
            experience = 0;
            health = 1;
            strength = 0;
            vitality = 0;
            magic = 0;
        }

        public CharacterStats(string name, int level, int experience, int health, int strength, int vitality, int magic)
        {
            this.name = name;
            this.level = level;
            this.experience = experience;
            this.health = health;
            this.strength = strength;
            this.vitality = vitality;
            this.magic = magic;
        }

        public string GetClassName { get { return name; } }
        public int GetLevel { get { return level; } }
        public int GetExperience { get { return experience; } }
        public int GetHealth { get { return health; } }
        public int GetStrength { get { return strength; } }
        public int GetVitality { get { return vitality; } }
        public int GetMagic { get { return magic; } }
    }

    public class StatsManage_Database
    {
        private List<CharacterStats> statsListing;
        private const string fileDirectory = "MeloMelo_Data/StreamingAssets/PlaySettings/CharacterStats/";
        private const string fileDirectoryEditor = "Assets/StreamingAssets/PlaySettings/CharacterStats/";
        private const string fileName = "_ClassStats.csv";

        public StatsManage_Database(string className)
        {
            statsListing = new List<CharacterStats>();
            GetStatsDataThroughFile(className);
        }

        private void GetStatsDataThroughFile(string readData)
        {
            if (File.Exists((Application.isEditor ? fileDirectoryEditor : fileDirectory) + readData + fileName))
            {
                StreamReader dataReader = new StreamReader((Application.isEditor ? fileDirectoryEditor : fileDirectory) + readData + fileName);
                string currentStats = dataReader.ReadLine();

                while (!dataReader.EndOfStream)
                {
                    currentStats = dataReader.ReadLine();
                    string[] dataValue = currentStats.Split(',');

                    // Assign stats from current read point
                    CharacterStats statsData = new CharacterStats
                        (
                            readData, int.Parse(dataValue[0]),
                            int.Parse(dataValue[1]), int.Parse(dataValue[2]),
                            int.Parse(dataValue[3]), int.Parse(dataValue[4]),
                            int.Parse(dataValue[5])
                        );

                    // Adding assign stats to a list
                    statsListing.Add(statsData);
                }

                dataReader.Close();
            }
        }

        public CharacterStats GetCharacterStatus(int level)
        {
            return statsListing.ToArray().Length != 0 ? statsListing[level - 1] : new CharacterStats();
        }
    }

    public class StatsDistribution
    {
        public readonly int baseDamage = 5;
        public readonly float baseDefense = 2.5f;
        public readonly int baseHealth = 10;
        public readonly int baseMagic = 5;

        public ClassBase[] slot_Stats = new ClassBase[3];

        public void load_Stats()
        {
            for (int i = 0; i < slot_Stats.Length; i++)
            {
                slot_Stats[i] = Resources.Load<ClassBase>("Character_Data/" + PlayerPrefs.GetString("Slot" + (i + 1) + "_charName", "None"));
                slot_Stats[i].UpdateCurrentStats(false);
            }
        }

        // Unit Damage: Get All Types
        public int get_UnitDamage(string index)
        {
            int DMG = 0;
            for (int i = 0; i < 3; i++)
            {
                switch (index)
                {
                    case "Enemy":
                        DMG += SelectionMenu_Script.thisSelect.get_selection.get_form.Insert_Enemy[PlayerPrefs.GetInt("BattleDifficulty_Mode", 1) - 1].myEnemySlot[i].DMG;
                        break;

                    case "Character":
                        slot_Stats[i].UpdateStatsCache(false);
                        DMG += baseDamage * (slot_Stats[i].strength + MeloMelo_ExtraStats_Settings.GetExtraStrengthStats(slot_Stats[i].name)) +
                            baseDamage;
                        break;
                }
            }
            return DMG;
        }

        // Unit Health: Get All Types
        public int get_UnitHealth(string index)
        {
            int HP = 0;
            for (int i = 0; i < 3; i++)
            {
                switch (index)
                {
                    case "Enemy":
                        HP += SelectionMenu_Script.thisSelect.get_selection.get_form.Insert_Enemy[PlayerPrefs.GetInt("BattleDifficulty_Mode", 1) - 1].myEnemySlot[i].HP;
                        HP += PreSelection_Script.thisPre.get_AreaData.EnemyBaseHealth[PlayerPrefs.GetInt("BattleDifficulty_Mode", 1) - 1];
                        break;

                    case "Character":
                        if (slot_Stats[i].icon != null)
                        {
                            slot_Stats[i].UpdateStatsCache(false);
                            HP += slot_Stats[i].health + (baseHealth * (slot_Stats[i].vitality + MeloMelo_ExtraStats_Settings.GetExtraVitaltyStats(slot_Stats[i].name)));
                        }
                        break;

                    default:
                        break;
                }
            }
            return HP;
        }

        // Unit Power: Get All Types
        public int get_UnitPower(string classType = "Character")
        {
            int power = 0;
            for (int id = 0; id < slot_Stats.Length; id++)
            {
                if (classType == slot_Stats[id].name)
                {
                    slot_Stats[id].UpdateStatsCache(false);
                    power += baseDamage * (slot_Stats[id].strength + MeloMelo_ExtraStats_Settings.GetExtraStrengthStats(slot_Stats[id].name));
                    power += baseMagic * (slot_Stats[id].magic + MeloMelo_ExtraStats_Settings.GetExtraMagicStats(slot_Stats[id].name));
                    power += (int)(baseDefense * (slot_Stats[id].vitality + MeloMelo_ExtraStats_Settings.GetExtraVitaltyStats(slot_Stats[id].name)));
                    power += baseHealth * slot_Stats[id].health;
                    return power;
                }
            }

            for (int unit = 0; unit < 3; unit++)
            {
                switch (classType)
                {
                    case "Enemy":
                        power += SelectionMenu_Script.thisSelect.get_selection.get_form.Insert_Enemy[PlayerPrefs.GetInt("BattleDifficulty_Mode", 1) - 1].myEnemySlot[unit].str * 10;
                        power += SelectionMenu_Script.thisSelect.get_selection.get_form.Insert_Enemy[PlayerPrefs.GetInt("BattleDifficulty_Mode", 1) - 1].myEnemySlot[unit].mag * 15;
                        power += SelectionMenu_Script.thisSelect.get_selection.get_form.Insert_Enemy[PlayerPrefs.GetInt("BattleDifficulty_Mode", 1) - 1].myEnemySlot[unit].vit * 5;
                        power += SelectionMenu_Script.thisSelect.get_selection.get_form.Insert_Enemy[PlayerPrefs.GetInt("BattleDifficulty_Mode", 1) - 1].myEnemySlot[unit].DMG * 100;
                        break;

                    case "Character":
                        if (slot_Stats[unit].icon != null)
                        {
                            slot_Stats[unit].UpdateStatsCache(false);
                            power += baseDamage * (slot_Stats[unit].strength + MeloMelo_ExtraStats_Settings.GetExtraStrengthStats(slot_Stats[unit].name));
                            power += baseMagic * (slot_Stats[unit].magic + MeloMelo_ExtraStats_Settings.GetExtraMagicStats(slot_Stats[unit].name));
                            power += (int)(baseDefense * (slot_Stats[unit].vitality + MeloMelo_ExtraStats_Settings.GetExtraVitaltyStats(slot_Stats[unit].name)));
                            power += baseHealth * slot_Stats[unit].health;
                            PlayerPrefs.SetInt("Character_OverallPower", power);
                        }
                        break;

                    default:
                        break;
                }
            }
                    
            return power;
        }

        // Area Difficulty: Get All Types
        public string get_AreaDifficulty()
        {
            switch (PlayerPrefs.GetInt("BattleDifficulty_Mode", 1))
            {
                case 1:
                    return "Beginner";

                case 2:
                    return "Intermidate";

                default:
                    return "Advance";
            }
        }

        // Check for Unit Available
        public bool get_slotNotNull()
        {
            for(int i = 0; i < slot_Stats.Length; i++)
            {
                if (slot_Stats[i].name != "None") { return true; }
            }
            return false;
        }
    }

    public class UnitFormation_Management
    {
        public void ClearUnit(int unit_id = -1)
        {
            PlayerPrefs.DeleteKey("CharacterFront");
            PlayerPrefs.DeleteKey("CharacterOverallHealth");

            if (unit_id != -1) PlayerPrefs.DeleteKey("Slot" + (unit_id + 1) + "_charName");
            else for (int i = 0; i < 3; i++) PlayerPrefs.DeleteKey("Slot" + (i + 1) + "_charName");
        }

        public void SetMainForce(string charName)
        {
            PlayerPrefs.SetString("CharacterFront", charName);
            PlayerPrefs.SetInt("CharacterOverallHealth", 0);
        }
    }

    public class Character_StorageStats
    {
        private float WMHP = 0;
        public float get_WMHP { get { return WMHP; } }

        private string Name = string.Empty;
        public string get_name { get { return Name; } }

        private float WEXP = 0;
        private float WEXPLimit = 100;
        private float WCharacterlevel = 1;
        private float NumberofnotesHitW = 0;
        public float get_NumberofnotesHitW { get { return NumberofnotesHitW; } }
        
        private float WVit = 0;
        //private float WMag = 0;
        //private float Wspeed = 0;
        private float Wstatpoints = 0;

        public Character_StorageStats(string charName, float maxHP)
        {
            Name = charName;
            WMHP = maxHP;
        }

        public void Update_Character_StorageStats(GameObject obj, string charName, float maxHP)
        {
            Name = charName;
            try { WMHP = maxHP + Collections_Script.thisCollect.get_list.get_CharacterData[0].health; } catch { WMHP = maxHP + 100; }

            if (charName != "NA" && !GameManager.thisManager.DeveloperMode)
            {
                if (obj.transform.GetChild(2).gameObject.activeInHierarchy)
                { obj.transform.GetChild(2).gameObject.SetActive(false); }
            }
        }

        public void NumberofnotesHitW_input(GameObject obj, bool set)
        {
            if (set) { NumberofnotesHitW = 0; } else { NumberofnotesHitW++; }
            obj.GetComponent<Slider>().value = NumberofnotesHitW;
        }

        public void WEXP_input()
        {
            WEXP += PlayerPrefs.GetInt("BattleDifficulty_Mode", 1);
            PlayerPrefs.SetInt("Temp_Experience", (int)WEXP);
        }

        //Warrior Ability        
        void CharacterextraAbility()
        {
            switch (Name)
            {
                case "Warrior":
                    if (WVit % 5 == 0) { WVit += 3; }
                    break;

                default:
                    break;
            }
        }

        public void levelup()
        {
            if (WEXP == WEXPLimit)
            {
                WCharacterlevel++;
                if (WCharacterlevel <= 5)
                    Wstatpoints += 5;
                else
                    Wstatpoints++;
                WEXP = WEXP - WEXPLimit;
                WEXPLimit *= 2;
                PlayerPrefs.SetFloat(Name + "EXPL", WEXPLimit);
                PlayerPrefs.SetFloat(Name + "EXP", WEXP);
                PlayerPrefs.SetFloat(Name + "Lv", WCharacterlevel);
                PlayerPrefs.SetFloat(Name + "stats", Wstatpoints);
            }
        }
    }

    /*
    public class WarriorAbility
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "note")
            {
                //Destroy(other.gameObject);
                GameManager.thisManager.UpdateNoteStatus("Perfect");
                GameManager.thisManager.UpdateScore(BeatConductor.thisBeat.get_scorePerfect);
                if (other.GetComponent<Note_Script>().note_index == 1)
                    GameManager.thisManager.UpdateBattle_Progress((float)100 / GameManager.thisManager.get_overallEnemy);
            }
        }
    }
    
    public class StatsDistribution
    {
        public Button StatsincreaseVit;
        public Button StatsincreaseMag;
        public Button StatsincreaseSpeed;

        void StatsIncreaseVit()
        {
            GameObject.Find("Character").GetComponent<Character>().WVit++;
            GameObject.Find("Character").GetComponent<Character>().Wstatpoints--;
            PlayerPrefs.SetFloat("WarriorVit", GameObject.Find("Character").GetComponent<Character>().WVit);
            PlayerPrefs.SetFloat("WarriorSP", GameObject.Find("Character").GetComponent<Character>().Wstatpoints);
        }
        void StatsIncreaseSpeed()
        {
            GameObject.Find("Character").GetComponent<Character>().Wspeed++;
            GameObject.Find("Character").GetComponent<Character>().Wstatpoints--;
        }
        void StatsIncreaseMag()
        {
            GameObject.Find("Character").GetComponent<Character>().WMag++;
            GameObject.Find("Character").GetComponent<Character>().Wstatpoints--;
        }
    } */
}