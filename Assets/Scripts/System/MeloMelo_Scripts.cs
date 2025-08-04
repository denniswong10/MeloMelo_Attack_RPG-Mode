 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
                VirtualItemDatabase itemFromStorage = MeloMelo_ItemUsage_Settings.GetActiveItem(itemName);
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
            int itemAmount = MeloMelo_ItemUsage_Settings.GetActiveItem(itemName).amount -
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
            if (File.Exists(Application.streamingAssetsPath + "/Story/" + storyRef + ".txt"))
            {
                string readTxtFile = Application.streamingAssetsPath + "/Story/" + storyRef + ".txt";
                List<string> fileLine = File.ReadAllLines(readTxtFile).ToList();
                string txt_printer = string.Empty;

                foreach (string i in fileLine) { txt_printer += (i + "\n"); }
                PlayerPrefs.SetString("Display_Story", txt_printer);
                StoryMode_Scripts.thisStory.StoryLoaderPlayer(storyRef);
            }
        }

        public IEnumerator StoryTxtEffect()
        {
            if (PlayerPrefs.HasKey("Display_Story"))
            {
                float speed = 0.01f; // 0.07f;

                foreach (char i in PlayerPrefs.GetString("Display_Story"))
                {
                    StoryMode_Scripts.thisStory.StoryDisplayTxt(i);
                    yield return new WaitForSeconds(speed);
                }

                StoryMode_Scripts.thisStory.StoryDisplayContinue();
                PlayerPrefs.DeleteKey("Display_Story");
            }
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
    struct ComboArrayType
    {
        public string comboGroup { get; }
        public int value { get; }

        public ComboArrayType(string group, int value)
        {
            comboGroup = group;
            this.value = value;
        }
    }

    class NotationDataTemplate
    {
        public string notationType { get; protected set; }
        public bool isCheck { get; private set; }
        public string value { get; protected set; }
        public float points { get; protected set; }

        public void HasChecked(bool check) => isCheck = check;
    }

    class SingleNotationChecker : NotationDataTemplate
    {
        public SingleNotationChecker(string name, string index, float points = 0)
        {
            notationType = name;
            value = index;
            this.points = points;
            HasChecked(false);
        }
    }

    class MultipleNotationChecker : NotationDataTemplate
    {
        public MultipleNotationChecker(string name, string[] indexList, float points = 0)
        {
            notationType = name;
            foreach (string notation in indexList) value += notation + ",";
            this.points = points;
            HasChecked(false);
        }
    }

    class NotationPatternData
    {
        public enum AddingType { Once, Repeat };
        public AddingType patternCheckType;

        public int numberOfRequireCount { get; private set; }
        public bool isAdded { get; private set; }
        public float points { get; private set; }

        public NotationPatternData(int numberOfCount, float setPoint, AddingType typeOfCheck)
        {
            numberOfRequireCount = numberOfCount;
            patternCheckType = typeOfCheck;
            points = setPoint;
        }

        public void HasAdded()
        {
            if (patternCheckType == AddingType.Once) 
                isAdded = true;
        }

        public void ResetAdding() => isAdded = false;
    }

    class NotationCombinationChecker
    {
        public string notationType { get; }
        public int totalCount { get; private set; }
        public List<NotationPatternData> patternArray;

        public NotationCombinationChecker(string name, string combinationType, string pointInArray, bool repeatableType = false)
        {
            notationType = name;
            totalCount = 0;
            patternArray = new List<NotationPatternData>();

            string[] requiredCount = combinationType.Split(',');
            string[] pointFromArray = pointInArray.Split(',');

            if (requiredCount.Length == pointFromArray.Length)
            {
                for (int autoFill = 0; autoFill < requiredCount.Length; autoFill++)
                {
                    NotationPatternData data = new NotationPatternData(int.Parse(requiredCount[autoFill]), float.Parse(pointFromArray[autoFill]), 
                        repeatableType ? NotationPatternData.AddingType.Repeat : NotationPatternData.AddingType.Once);

                    data.ResetAdding();
                    patternArray.Add(data);
                }
            }
        }

        public void AddCount() => totalCount++;
        public void ResetCount() => totalCount = 0;
    }

    class NotationMixCombinationChecker
    {
        public List<NotationPatternData> patternRange;
        public int totalCount { get; private set; }
        public List<int> checkForDuplicate;
        public List<string> filterNonExistanceValue;

        public NotationMixCombinationChecker(int[] lengthOfCombination, float[] pointInArray, string[] filterOutValueNotExisted)
        {
            patternRange = new List<NotationPatternData>();
            checkForDuplicate = new List<int>();
            filterNonExistanceValue = new List<string>();
            totalCount = 0;

            if (lengthOfCombination != null && lengthOfCombination.Length > 0)
            {
                for (int notationId = 0; notationId < lengthOfCombination.Length; notationId++)
                {
                    NotationPatternData temp = new NotationPatternData(lengthOfCombination[notationId], pointInArray[notationId], NotationPatternData.AddingType.Once);
                    temp.ResetAdding();
                    patternRange.Add(temp);

                    filterNonExistanceValue.Add(filterOutValueNotExisted[notationId]);
                }
            }
        }

        public void CopyValue(int value)
        {
            //if (totalCount < patternRange[patternRange.ToArray().Length - 1].numberOfRequireCount)

            if (value != 0) checkForDuplicate.Add(value);
            totalCount++;
        }

        public bool CheckForNextDuplicated(int value)
        {
            foreach (int forDuplicateValue in checkForDuplicate)
                if (value == forDuplicateValue) return true;

            return false;
        }

        public bool ValueInclude(int[] data, int currentCount)
        {
            if (filterNonExistanceValue[currentCount] != string.Empty)
            {
                string[] valueS = filterNonExistanceValue[currentCount].Split(',');
                foreach (string value in valueS)
                    if (data.Contains(int.Parse(value)))
                        return true;
            }

            return false;
        }

        public void ResetCount()
        {
            totalCount = 0;
            //checkForDuplicate.Clear();
        }
    }

    // Unit: ScoringMeter Function
    public class ScoringMeter
    {
        // Memory Usage: ScoringMeter
        private List<ComboArrayType> get_combo_list;
        private List<SingleNotationChecker> get_notation_visible;
        private List<NotationCombinationChecker> get_notation_list;
        private NotationMixCombinationChecker get_notation_mixElement;

        private int noteCom_all = 0;

        private bool[] maxCom_all = new bool[3];
        private int[] noteType = new int[4];

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
            LevelSetup_Program(1, score_database);
            LevelSetup_Program(2, score_database2);
        }

        public ScoringMeter(int[] score_database, int[] score_database2, int[] score_database3)
        {
            LevelSetup_Program(1, score_database);
            LevelSetup_Program(2, score_database2);
            LevelSetup_Program(3, score_database3);
        }

        public ScoringMeter(int difficultyMap, int[] data)
        {
            LevelSetup_Program(difficultyMap, data);
        }

        // Score Function: Level Meter
        private void SetUp(int difficultyMap, int numberofEnemy, int numberofTraps, int maxCombo, int[] data)
        {
            difficultyLevel = 0;
            scoringCurrent = 0;

            for (int i = 0; i < maxCom_all.Length; i++) { maxCom_all[i] = false; }
            for (int i = 0; i < noteType.Length; i++) { noteType[i] = 0; }

            // Start of level editior
            LevelEditorPrompt(numberofTraps, numberofEnemy);

            // Gather point system by level editior
            PlayerPrefs.SetInt("GetMaxPoint_" + difficultyMap, maxCombo * 3);

            // Level combo complextity
            if (SceneManager.GetActiveScene().name == "Music Selection Stage")
            {
                string combo_grouping_name =
                    SelectionMenu_Script.thisSelect.get_selection.get_form.NewChartSystem ? "New_Chart_Scoring" :
                    SelectionMenu_Script.thisSelect.get_selection.get_form.UltimateAddons ? "Old_Chart_Scoring" : "Old_Chart_Scoring";

                scoringCurrent += GetCombo_LevelComplextity(combo_grouping_name, maxCombo);
            }
            else
            {
                string combo_grouping_name =
                    BeatConductor.thisBeat.Music_Database.NewChartSystem ? "New_Chart_Scoring" :
                    BeatConductor.thisBeat.Music_Database.UltimateAddons ? "Old_Chart_Scoring" : "Old_Chart_Scoring";

                scoringCurrent += GetCombo_LevelComplextity(combo_grouping_name, maxCombo);
            }

            // Level Visible Complextity
            if (get_notation_visible != null)
                foreach (SingleNotationChecker notation in get_notation_visible) 
                    scoringCurrent += GetVisible_LevelComplextity(data, notation.notationType);

            // Level Notation Pattern Complextity (One of a kind checker)
            if (get_notation_list != null)
            {
                foreach (NotationCombinationChecker notation in get_notation_list)
                {
                    string[] notationHolder = GetNotationStatus(notation.notationType).value.Split(',');

                    foreach (int i in data)
                    {
                        bool isPatternFound = MultiNotation_OR_Check(notationHolder, i);

                        if (isPatternFound)
                        {
                            notation.AddCount();

                            foreach (NotationPatternData combinationCount in notation.patternArray)
                            {
                                if (!combinationCount.isAdded && combinationCount.numberOfRequireCount == notation.totalCount)
                                {
                                    combinationCount.HasAdded();
                                    Debug.Log(notation.notationType + " ( " + combinationCount.numberOfRequireCount + ") : YES | " + combinationCount.points + "p");

                                    scoringCurrent += combinationCount.points;
                                    notation.ResetCount();
                                    break;
                                }
                            }
                        }
                        else 
                            notation.ResetCount();
                    }
                }
            }

            // Level Notation Pattern Complextity(Mix element in any sequence checker)
            //if (get_notation_mixElement != null)
            //{
            //    foreach (int i in data)
            //    {
            //        if (!get_notation_mixElement.CheckForNextDuplicated(i))
            //            get_notation_mixElement.CopyValue(i);

            //        else
            //            get_notation_mixElement.ResetCount();

            //        for (int mixCombinationCount = 0; mixCombinationCount < get_notation_mixElement.patternRange.ToArray().Length; mixCombinationCount++)
            //        {
            //            if (!get_notation_mixElement.ValueInclude(data, mixCombinationCount) && 
            //                !get_notation_mixElement.patternRange[mixCombinationCount].isAdded &&
            //                get_notation_mixElement.patternRange[mixCombinationCount].numberOfRequireCount == get_notation_mixElement.totalCount
            //                )
            //            {
            //                get_notation_mixElement.patternRange[mixCombinationCount].HasAdded();
            //                Debug.Log("Creation " + get_notation_mixElement.patternRange[mixCombinationCount].numberOfRequireCount + "/4: YES | " + get_notation_mixElement.patternRange[mixCombinationCount].points + "p");

            //                scoringCurrent += get_notation_mixElement.patternRange[mixCombinationCount].points;
            //                get_notation_mixElement.ResetCount();
            //                break;
            //            }
            //        }
            //    }
            //}

            // All Season: Scoring
            foreach (int i in data)
            {
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
                            if (!GetNotationStatus(MeloMelo_GameSettings.GetAirTrap).isCheck && !GetNotationStatus(MeloMelo_GameSettings.GetAirEnemy).isCheck && !maxCom_all[2]) { Debug.Log("Creation 5/4: YES"); maxCom_all[2] = true; scoringCurrent += 5; }
                            break;
                    }
                }
                else { noteCom_all = 0; }
            }

            difficultyLevel = 0.15f * scoringCurrent;
            Debug.Log("Overall Level (" + difficultyMap + "): " + scoringCurrent + " => " + difficultyLevel);

            if (SceneManager.GetActiveScene().name == "Music Selection Stage")
                SelectionMenu_Script.thisSelect.get_selection.UpdateData_Level(difficultyMap, difficultyLevel);
        }

        #region SETUP
        private void GetScoringDetails()
        {
            // Combo complex value
            if (get_combo_list == null) get_combo_list = new List<ComboArrayType>();
            else get_combo_list.Clear();

            if (get_combo_list != null)
            {
                get_combo_list.Add(new ComboArrayType("Old_Chart_Scoring", 1));
                get_combo_list.Add(new ComboArrayType("New_Chart_Scoring", 1));
                get_combo_list.Add(new ComboArrayType("Old_Chart_Scoring", 50));
                get_combo_list.Add(new ComboArrayType("Old_Chart_Scoring", 100));
                get_combo_list.Add(new ComboArrayType("Old_Chart_Scoring", 200));
                get_combo_list.Add(new ComboArrayType("New_Chart_Scoring", 100));
                get_combo_list.Add(new ComboArrayType("New_Chart_Scoring", 200));
                get_combo_list.Add(new ComboArrayType("New_Chart_Scoring", 300));
            }

            // Notation visible complex value
            if (get_notation_visible == null) get_notation_visible = new List<SingleNotationChecker>();
            else get_notation_visible.Clear();

            if (get_notation_visible != null)
            {
                get_notation_visible.Add(new SingleNotationChecker(MeloMelo_GameSettings.GetGroundEnemy, "1"));
                get_notation_visible.Add(new SingleNotationChecker(MeloMelo_GameSettings.GetItem, "2"));
                get_notation_visible.Add(new SingleNotationChecker(MeloMelo_GameSettings.GetGroundTrap, "3", 2));
                get_notation_visible.Add(new SingleNotationChecker(MeloMelo_GameSettings.GetHeartPack, "4"));
                get_notation_visible.Add(new SingleNotationChecker(MeloMelo_GameSettings.GetGroundAttack, "5"));
                get_notation_visible.Add(new SingleNotationChecker(MeloMelo_GameSettings.GetAirEnemy, "6", 2));
                get_notation_visible.Add(new SingleNotationChecker(MeloMelo_GameSettings.GetAirTrap, "7", 3));
                get_notation_visible.Add(new SingleNotationChecker(MeloMelo_GameSettings.GetItem2, "8"));
                get_notation_visible.Add(new SingleNotationChecker(MeloMelo_GameSettings.GetAirAttack, "9", 0.5f));
                get_notation_visible.Add(new SingleNotationChecker(MeloMelo_GameSettings.GetSpecialItem, "93", -4.5f)); // <= This will reduce all current difficulty level

                get_notation_visible.Add(new SingleNotationChecker(MeloMelo_GameSettings.GetFullAttack, 
                    GetMultipleNotationIndex(new string[] { MeloMelo_GameSettings.GetGroundAttack, MeloMelo_GameSettings.GetAirAttack }), 2));

                get_notation_visible.Add(new SingleNotationChecker(MeloMelo_GameSettings.GetAllTraps, 
                    GetMultipleNotationIndex(new string[] { MeloMelo_GameSettings.GetGroundTrap, MeloMelo_GameSettings.GetAirTrap }), 1));
            }

            // Notation any of a kind pattern complex value
            if (get_notation_list == null) get_notation_list = new List<NotationCombinationChecker>();
            else get_notation_list.Clear();

            if (get_notation_list != null)
            {
                get_notation_list.Add(new NotationCombinationChecker(MeloMelo_GameSettings.GetSpecialItem, "2,3,4", "0.5,1,1.5", true));
                get_notation_list.Add(new NotationCombinationChecker(MeloMelo_GameSettings.GetGroundEnemy, "3,4", "2,5"));
                get_notation_list.Add(new NotationCombinationChecker(MeloMelo_GameSettings.GetAllTraps, "3,4", "3,8"));
            }

            // Notation mix element pattern complex value
            string checkFifthMixNotIncluded = GetNotationStatus(MeloMelo_GameSettings.GetAirTrap).value + "," + GetNotationStatus(MeloMelo_GameSettings.GetAirEnemy).value;
            get_notation_mixElement = new NotationMixCombinationChecker(
                new int[] { 3, 4, 5 }, 
                new float[] { 10.2f, 14.5f, 0 }, 
                new string[] { string.Empty, string.Empty, checkFifthMixNotIncluded }
                );

            // Notation map length pattern complex value
            if (get_notation_visible != null)
            {
                string[] all_element_id = { 
                    MeloMelo_GameSettings.GetGroundEnemy, 
                    MeloMelo_GameSettings.GetItem, 
                    MeloMelo_GameSettings.GetGroundTrap, 
                    MeloMelo_GameSettings.GetHeartPack,
                    MeloMelo_GameSettings.GetGroundAttack,
                    MeloMelo_GameSettings.GetAirEnemy,
                    MeloMelo_GameSettings.GetAirTrap,
                    MeloMelo_GameSettings.GetItem2,
                    MeloMelo_GameSettings.GetAirAttack,
                    MeloMelo_GameSettings.GetSpecialItem
                };

                string extra_notation_value = GetMultipleCreationNotationIndex() != string.Empty ? ("," + GetMultipleCreationNotationIndex()) : string.Empty;
                string get_allValue_notation = GetMultipleNotationIndex(all_element_id) + extra_notation_value;

                get_notation_visible.Add(new SingleNotationChecker("All_Primary_Element", GetMultipleNotationIndex(all_element_id)));
                get_notation_visible.Add(new SingleNotationChecker("All_Mapped_Element", get_allValue_notation));

                if (get_notation_list != null) get_notation_list.Add(new NotationCombinationChecker("All_Mapped_Element", "2,3,4,5", "10,12.2,14,0"));
                Debug.Log("Extra Notation Index: " + (extra_notation_value != string.Empty ? extra_notation_value : "-None-"));
            }
        }

        // Score Intillization: Function
        private void LevelSetup_Program(int difficulty_id, int[] score_database)
        {
            PlayerPrefs.DeleteKey("EnemyTakeCounter_" + difficulty_id);
            PlayerPrefs.DeleteKey("TrapsTakeCounter_" + difficulty_id);
            PlayerPrefs.DeleteKey("Total_Notation_Count" + difficulty_id);

            ScanLevelDesign_ForDifficultyLevel(difficulty_id, score_database, true);
        }
        #endregion

        #region MAIN
        private void LevelEditorPrompt(int trapCount, int enemyCount)
        {
            try
            {
                if (GameManager.thisManager.DeveloperMode)
                {
                    Debug.Log("Traps: " + trapCount);
                    Debug.Log("Enemy: " + enemyCount);
                    Debug.Log("Combo: " + maxCombo);
                }
                else
                {
                    Debug.Log("Total Combo: " + maxCombo);
                }
            }
            catch { }
        }
        #endregion

        #region COMPONENT
        private float GetCombo_LevelComplextity(string groupTitle, int combo_reference)
        {
            float totalComplextity = 0;
            const float complexitityValue = 7.2f;

            if (get_combo_list != null)
            {
                foreach (ComboArrayType combo in get_combo_list)
                {
                    if (groupTitle == combo.comboGroup && combo_reference >= combo.value)
                        totalComplextity += complexitityValue;
                }
            }

            return totalComplextity;
        }

        private float GetVisible_LevelComplextity(int[] data, string visibleType)
        {
            float totalComplextity = 0;

            if (get_notation_visible != null)
            {
                foreach (SingleNotationChecker checkList in get_notation_visible)
                {
                    if (checkList.notationType == visibleType)
                    {
                        string[] numberOfType = checkList.value.Split(",");

                        if (numberOfType.Length > 1)
                        {
                            if (MultiNotation_AND_Check(numberOfType, data))
                            {
                                totalComplextity += checkList.points;
                                checkList.HasChecked(true);
                            }
                        }
                        else
                        {
                            int target = int.Parse(checkList.value);
                            if (data.Contains(target))
                            {
                                totalComplextity += checkList.points;
                                checkList.HasChecked(true);
                            }
                        }

                        break;
                    }
                }

                if (GetNotationStatus(visibleType).points > 0)
                    Debug.Log(visibleType + ": " + (GetNotationStatus(visibleType).isCheck ? "YES | " + totalComplextity + "p" : "NO"));
            }

            return totalComplextity;
        }

        private SingleNotationChecker GetNotationStatus(string notationType)
        {
            if (get_notation_visible != null)
            {
                foreach (SingleNotationChecker notation in get_notation_visible)
                {
                    if (notation.notationType == notationType) return notation;
                }
            }

            return new SingleNotationChecker(string.Empty, "-1");
        }

        private string GetMultipleNotationIndex(string[] notationList)
        {
            string indexWritten = string.Empty;

            for (int notation = 0; notation < notationList.Length; notation++)
                indexWritten += GetNotationStatus(notationList[notation]).value + 
                    (notation + 1 < notationList.Length ? "," : string.Empty);

            return indexWritten;
        }

        private string GetMultipleCreationNotationIndex()
        {
            List<string> listOfChartIndex = new List<string>();
            string indexWritten = string.Empty;

            if (SceneManager.GetActiveScene().name == "Music Selection Stage")
            {
                // Multiple Charting
                indexWritten = WrittenNewChartIndex(
                    SelectionMenu_Script.thisSelect.get_selection.get_form.UltimateAddons && SelectionMenu_Script.thisSelect.get_selection.get_form.addons1 != null 
                    && SelectionMenu_Script.thisSelect.get_selection.get_form.addons1.Length > 0
                    , SelectionMenu_Script.thisSelect.get_selection.get_form.addons1, patternList => patternList.SecondaryIndex
                    );

                if (listOfChartIndex != null && indexWritten != string.Empty) listOfChartIndex.Add(indexWritten);

                // Pattern Charting
                indexWritten = WrittenNewChartIndex(
                    SelectionMenu_Script.thisSelect.get_selection.get_form.NewChartSystem && SelectionMenu_Script.thisSelect.get_selection.get_form.addons3 != null
                    && SelectionMenu_Script.thisSelect.get_selection.get_form.addons3.Length > 0
                    , SelectionMenu_Script.thisSelect.get_selection.get_form.addons3, patternList => patternList.SecondaryIndex
                    );

                if (listOfChartIndex != null && indexWritten != string.Empty) listOfChartIndex.Add(indexWritten);

                // Modified Pattern Charting
                indexWritten = WrittenNewChartIndex(
                    SelectionMenu_Script.thisSelect.get_selection.get_form.NewChartSystem && SelectionMenu_Script.thisSelect.get_selection.get_form.addons3b != null
                    && SelectionMenu_Script.thisSelect.get_selection.get_form.addons3b.Length > 0
                    , SelectionMenu_Script.thisSelect.get_selection.get_form.addons3b, patternList => patternList.newIndex
                    );

                if (listOfChartIndex != null && indexWritten != string.Empty) listOfChartIndex.Add(indexWritten);
            }
            else
            {
                // Multiple Charting
                indexWritten = WrittenNewChartIndex(
                    BeatConductor.thisBeat.Music_Database.UltimateAddons && BeatConductor.thisBeat.Music_Database.addons1 != null
                    && BeatConductor.thisBeat.Music_Database.addons1.Length > 0
                    , BeatConductor.thisBeat.Music_Database.addons1, patternList => patternList.SecondaryIndex
                    );

                if (listOfChartIndex != null && indexWritten != string.Empty) listOfChartIndex.Add(indexWritten);

                // Pattern Charting
                indexWritten = WrittenNewChartIndex(
                    BeatConductor.thisBeat.Music_Database.NewChartSystem && BeatConductor.thisBeat.Music_Database.addons3 != null
                    && BeatConductor.thisBeat.Music_Database.addons3.Length > 0
                    , BeatConductor.thisBeat.Music_Database.addons3, patternList => patternList.SecondaryIndex
                    );

                if (listOfChartIndex != null && indexWritten != string.Empty) listOfChartIndex.Add(indexWritten);

                // Modified Pattern Charting
                indexWritten = WrittenNewChartIndex(
                    BeatConductor.thisBeat.Music_Database.NewChartSystem && BeatConductor.thisBeat.Music_Database.addons3b != null
                    && BeatConductor.thisBeat.Music_Database.addons3b.Length > 0
                    , BeatConductor.thisBeat.Music_Database.addons3b, patternList => patternList.newIndex
                    );

                if (listOfChartIndex != null && indexWritten != string.Empty) listOfChartIndex.Add(indexWritten);
            }

            // Final written all chart index
            if (listOfChartIndex != null)
            {
                indexWritten = string.Empty;

                for (int chartId = 0; chartId < listOfChartIndex.ToArray().Length; chartId++)
                    indexWritten += listOfChartIndex[chartId] + (chartId + 1 < listOfChartIndex.ToArray().Length ? "," : string.Empty);
            }

            return indexWritten;
        }

        private string WrittenNewChartIndex<AnyChartType>(bool condition, AnyChartType[] chartPatternList, Func<AnyChartType, int> value)
        {
            string tempWrittenIndex = string.Empty;

            if (condition)
            {
                AnyChartType[] tempPatternList = chartPatternList;

                for (int id = 0; id < tempPatternList.Length; id++)
                    tempWrittenIndex += value(tempPatternList[id]) + (id + 1 < tempPatternList.Length ? "," : string.Empty);
            }

            return tempWrittenIndex;
        }

        private void ScanLevelDesign_ForDifficultyLevel(int difficulty_id, int[] score_data, bool extraDifficultyEnable = false)
        {
            // Gather info scoring assets for uses
            GetScoringDetails();

            // Calculate object count from score_data
            for (int i = 0; i < score_data.Length; i++)
            {
                if (score_data[i] == int.Parse(GetNotationStatus(MeloMelo_GameSettings.GetGroundEnemy).value) ||
                    score_data[i] == int.Parse(GetNotationStatus(MeloMelo_GameSettings.GetAirEnemy).value))
                { EnemyCounter++; }

                if (score_data[i] == int.Parse(GetNotationStatus(MeloMelo_GameSettings.GetGroundTrap).value) ||
                    score_data[i] == int.Parse(GetNotationStatus(MeloMelo_GameSettings.GetAirTrap).value))
                { TrapCounter++; }

                if (MultiNotation_OR_Check(GetNotationStatus("All_Primary_Element").value.Split(','), score_data[i]))
                { maxCombo++; }

                // Pattern Charting
                if (SceneManager.GetActiveScene().name == "Music Selection Stage")
                {
                    bool isPatternChartingEnable = SelectionMenu_Script.thisSelect != null && SelectionMenu_Script.thisSelect.get_selection.get_form.NewChartSystem &&
                                                SelectionMenu_Script.thisSelect.get_selection.get_form.addons3 != null;

                    NewChartPatternSearch(score_data, i, isPatternChartingEnable, SelectionMenu_Script.thisSelect.get_selection.get_form.addons3.ToArray());

                    isPatternChartingEnable = SelectionMenu_Script.thisSelect != null && SelectionMenu_Script.thisSelect.get_selection.get_form.NewChartSystem &&
                                                SelectionMenu_Script.thisSelect.get_selection.get_form.addons3b != null;

                    NewChartPatternModded(score_data, i, isPatternChartingEnable, SelectionMenu_Script.thisSelect.get_selection.get_form.addons3.ToArray(), SelectionMenu_Script.thisSelect.get_selection.get_form.addons3b.ToArray());

                    isPatternChartingEnable = SelectionMenu_Script.thisSelect != null && SelectionMenu_Script.thisSelect.get_selection.get_form.UltimateAddons &&
                                                SelectionMenu_Script.thisSelect.get_selection.get_form.addons1 != null;

                    if (extraDifficultyEnable) NewChartMultipleSearch(score_data, i, isPatternChartingEnable, SelectionMenu_Script.thisSelect.get_selection.get_form.addons1);
                }
                else
                {
                    bool isPatternChartingEnable = BeatConductor.thisBeat != null && BeatConductor.thisBeat.Music_Database.NewChartSystem &&
                                                BeatConductor.thisBeat.Music_Database.addons3 != null;

                    NewChartPatternSearch(score_data, i, isPatternChartingEnable, BeatConductor.thisBeat.Music_Database.addons3.ToArray());

                    isPatternChartingEnable = BeatConductor.thisBeat != null && BeatConductor.thisBeat.Music_Database.NewChartSystem &&
                                                BeatConductor.thisBeat.Music_Database.addons3b != null;

                    NewChartPatternModded(score_data, i, isPatternChartingEnable, BeatConductor.thisBeat.Music_Database.addons3.ToArray(), BeatConductor.thisBeat.Music_Database.addons3b.ToArray());

                    isPatternChartingEnable = BeatConductor.thisBeat != null && BeatConductor.thisBeat.Music_Database.UltimateAddons &&
                                                BeatConductor.thisBeat.Music_Database.addons1 != null;

                    if (extraDifficultyEnable) NewChartMultipleSearch(score_data, i, isPatternChartingEnable, BeatConductor.thisBeat.Music_Database.addons1);
                }
            }

            CreateAllNotationCache();
            CreateEntriesCache(difficulty_id, EnemyCounter, TrapCounter, maxCombo);

            SetUp(difficulty_id, EnemyCounter, TrapCounter, maxCombo, score_data);
            ResetCounter();
        }
        #endregion

        #region MISC 
        // Extra: Reset Counter
        private bool MultiNotation_AND_Check(string[] value, int[] data)
        {
            foreach (string val in value)
            {
                int target = int.Parse(val);
                if (!data.Contains(target))
                    return false;
            }
            return true;
        }

        private bool MultiNotation_OR_Check(string[] value, int data)
        {
            foreach (string val in value)
            {
                int target = int.Parse(val);
                if (data == target)
                    return true;
            }
            return false;
        }

        private void ResetCounter()
        {
            EnemyCounter = 0;
            TrapCounter = 0;
            maxCombo = 0;
        }

        private void CreateAllNotationCache()
        {
            if (get_notation_visible != null)
            {
                foreach (SingleNotationChecker notation in get_notation_visible)
                {
                    string[] value = notation.value.Split(',');

                    if (value.Length == 1)
                    {
                        PlayerPrefs.SetInt(notation.notationType, int.Parse(notation.value));
                        Debug.Log("Notation Cache Added: " + notation.notationType + " => " + PlayerPrefs.GetInt(notation.notationType));
                    }
                }
            }
        }

        private void CreateEntriesCache(int difficulty_id, int enemyCount, int trapCount, int notationCount)
        {
            PlayerPrefs.SetInt("EnemyTakeCounter_" + difficulty_id, enemyCount);
            PlayerPrefs.SetInt("TrapsTakeCounter_" + difficulty_id, trapCount);
            PlayerPrefs.SetInt("Total_Notation_Count" + difficulty_id, notationCount);
        }
        #endregion

        #region MISC (NEW CHART ADDONS)
        private void NewChartPatternSearch(int[] score, int currentTick, bool condition, PatternCharting[] chartList)
        {
            if (condition)
            {
                foreach (PatternCharting chart in chartList)
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

                                    if (get_notation_visible != null)
                                    {
                                        if (col.PrimaryNote == int.Parse(GetNotationStatus(MeloMelo_GameSettings.GetGroundEnemy).value) ||
                                        col.PrimaryNote == int.Parse(GetNotationStatus(MeloMelo_GameSettings.GetAirEnemy).value))
                                        { EnemyCounter++; }

                                        if (col.PrimaryNote == int.Parse(GetNotationStatus(MeloMelo_GameSettings.GetGroundTrap).value) ||
                                            col.PrimaryNote == int.Parse(GetNotationStatus(MeloMelo_GameSettings.GetAirTrap).value))
                                        { TrapCounter++; }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void NewChartPatternModded(int[] score, int currentTick, bool condition, PatternCharting[] chartList_orginal, ChartModification[] chartList_modded)
        {
            if (condition)
            {
                foreach (ChartModification mod in chartList_modded)
                {
                    if (score[currentTick] == mod.newIndex)
                    {
                        foreach (PatternCharting chart in chartList_orginal)
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

                                            if (get_notation_visible != null)
                                            {
                                                if (col.PrimaryNote == int.Parse(GetNotationStatus(MeloMelo_GameSettings.GetGroundEnemy).value) ||
                                                col.PrimaryNote == int.Parse(GetNotationStatus(MeloMelo_GameSettings.GetAirEnemy).value))
                                                { EnemyCounter++; }

                                                if (col.PrimaryNote == int.Parse(GetNotationStatus(MeloMelo_GameSettings.GetGroundTrap).value) ||
                                                    col.PrimaryNote == int.Parse(GetNotationStatus(MeloMelo_GameSettings.GetAirTrap).value))
                                                { TrapCounter++; }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void NewChartMultipleSearch(int[] score, int currentTick, bool condition, MultipleCharting[] chartList)
        {
            if (condition)
            {
                foreach (MultipleCharting specialChart in chartList)
                {
                    if (score[currentTick] == specialChart.SecondaryIndex)
                    {
                        if (get_notation_visible != null)
                        {
                            // All Notes
                            if (MultiNotation_OR_Check(GetNotationStatus("All_Primary_Element").value.Split(','), specialChart.FirstLane)) maxCombo++;
                            if (MultiNotation_OR_Check(GetNotationStatus("All_Primary_Element").value.Split(','), specialChart.SecondLane)) maxCombo++;
                            if (MultiNotation_OR_Check(GetNotationStatus("All_Primary_Element").value.Split(','), specialChart.ThirdLane)) maxCombo++;
                            if (MultiNotation_OR_Check(GetNotationStatus("All_Primary_Element").value.Split(','), specialChart.FourthLane)) maxCombo++;
                            if (MultiNotation_OR_Check(GetNotationStatus("All_Primary_Element").value.Split(','), specialChart.FifthLane)) maxCombo++;

                            // Enemys
                            if (specialChart.FirstLane == int.Parse(GetNotationStatus(MeloMelo_GameSettings.GetGroundEnemy).value) ||
                                specialChart.FirstLane == int.Parse(GetNotationStatus(MeloMelo_GameSettings.GetAirEnemy).value))
                            { EnemyCounter++; }

                            if (specialChart.SecondLane == int.Parse(GetNotationStatus(MeloMelo_GameSettings.GetGroundEnemy).value) ||
                                specialChart.SecondLane == int.Parse(GetNotationStatus(MeloMelo_GameSettings.GetAirEnemy).value))
                            { EnemyCounter++; }

                            if (specialChart.ThirdLane == int.Parse(GetNotationStatus(MeloMelo_GameSettings.GetGroundEnemy).value) ||
                                specialChart.ThirdLane == int.Parse(GetNotationStatus(MeloMelo_GameSettings.GetAirEnemy).value))
                            { EnemyCounter++; }

                            if (specialChart.FourthLane == int.Parse(GetNotationStatus(MeloMelo_GameSettings.GetGroundEnemy).value) ||
                                specialChart.FourthLane == int.Parse(GetNotationStatus(MeloMelo_GameSettings.GetAirEnemy).value))
                            { EnemyCounter++; }

                            if (specialChart.FifthLane == int.Parse(GetNotationStatus(MeloMelo_GameSettings.GetGroundEnemy).value) ||
                                specialChart.FifthLane == int.Parse(GetNotationStatus(MeloMelo_GameSettings.GetAirEnemy).value))
                            { EnemyCounter++; }

                            // Traps
                            if (specialChart.FirstLane == int.Parse(GetNotationStatus(MeloMelo_GameSettings.GetGroundTrap).value) ||
                                specialChart.FirstLane == int.Parse(GetNotationStatus(MeloMelo_GameSettings.GetAirTrap).value))
                            { TrapCounter++; }

                            if (specialChart.SecondLane == int.Parse(GetNotationStatus(MeloMelo_GameSettings.GetGroundTrap).value) ||
                                specialChart.SecondLane == int.Parse(GetNotationStatus(MeloMelo_GameSettings.GetAirTrap).value))
                            { TrapCounter++; }

                            if (specialChart.ThirdLane == int.Parse(GetNotationStatus(MeloMelo_GameSettings.GetGroundTrap).value) ||
                                specialChart.ThirdLane == int.Parse(GetNotationStatus(MeloMelo_GameSettings.GetAirTrap).value))
                            { TrapCounter++; }

                            if (specialChart.FourthLane == int.Parse(GetNotationStatus(MeloMelo_GameSettings.GetGroundTrap).value) ||
                                specialChart.FourthLane == int.Parse(GetNotationStatus(MeloMelo_GameSettings.GetAirTrap).value))
                            { TrapCounter++; }

                            if (specialChart.FifthLane == int.Parse(GetNotationStatus(MeloMelo_GameSettings.GetGroundTrap).value) ||
                                specialChart.FifthLane == int.Parse(GetNotationStatus(MeloMelo_GameSettings.GetAirTrap).value))
                            { TrapCounter++; }
                        }
                    }
                }
            }
        }
        #endregion
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
    public struct ScoreDatabase
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
    public struct PointDatabase
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
    public struct PlayerSettingsDatabase
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

        public int maxFrameLimit;
        public int performanceOptimize;
        public int unitDisplayHealthOnCharacter;
        public int unitDisplayHealthOnEnemy;

        public bool sppedMeterOption;
        public bool fancyMovementOption;

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

        public int audio_offset;
        public int judge_offset;

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
    public struct BattleProgressDatabase
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
        public int magicStoneValue;
        public int submittedMapFragment;

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
    public struct BattleUnitDatabase
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
    public struct SkillUnitDatabase
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

            if (uniqueId == string.Empty) user.uniqueId = "1111";// Random.Range(99, 999) + "_T2024_t&" + Random.Range(5, 55);
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

        public async Task<List<ScoreDatabase>> PreLoading_ScoreData()
        {
            return await Task.Run(() =>
            {
                if (!File.Exists(directory + combinePath)) return new List<ScoreDatabase>();

                List<ScoreDatabase> dataArray = new List<ScoreDatabase>();

                foreach (string s in GetFormatToList())
                {
                    if (s != string.Empty)
                        dataArray.Add(new ScoreDatabase().GetTrackData(s));
                }

                return dataArray;
            });
        }

        public IEnumerator PostLoading_ScoreData(ScoreDatabase[] preLoaded_scoreData)
        {
            if (preLoaded_scoreData != null)
            {
                int maxLoadLimit = 50;
                int currentLoadIndex = 0;

                foreach (ScoreDatabase encode_data in preLoaded_scoreData)
                {
                    if (user == encode_data.user)
                    {
                        // Score is taken to the highest
                        if (encode_data.score >= PlayerPrefs.GetInt(encode_data.title + "_score" + encode_data.difficulty, 0))
                            PlayerPrefs.SetInt(encode_data.title + "_score" + encode_data.difficulty, (int)encode_data.score);

                        // Battle Status update to the latest
                        if (encode_data.remark <= PlayerPrefs.GetInt(encode_data.title + "_BattleRemark_" + encode_data.difficulty, 6))
                            PlayerPrefs.SetInt(encode_data.title + "_BattleRemark_" + encode_data.difficulty, encode_data.remark);

                        // Reduce score to 0 when defeated
                        if (PlayerPrefs.GetInt(encode_data.title + "_BattleRemark_" + encode_data.difficulty) == 5)
                            PlayerPrefs.SetInt(encode_data.title + "_score" + encode_data.difficulty, 0);
                    }

                    currentLoadIndex++;
                    if (currentLoadIndex % maxLoadLimit == 0) yield return null;
                }
            }
        }

        public async Task<List<PointDatabase>> PreLoading_PointData()
        {
            return await Task.Run(() =>
            {
                if (!File.Exists(directory + combinePath)) return new List<PointDatabase>();
                List<PointDatabase> dataArray = new List<PointDatabase>();

                foreach (string s in GetFormatToList())
                {
                    if (s != string.Empty)
                        dataArray.Add(new PointDatabase().GetTrackData(s));
                }

                return dataArray;
            });
        }

        public IEnumerator PostLoading_PointData(PointDatabase[] preLoaded_pointData)
        {
            if (preLoaded_pointData != null)
            {
                int maxLoadLimit = 50;
                int currentLoadIndex = 0;

                foreach (PointDatabase encode_data in preLoaded_pointData)
                {
                    if (encode_data.current >= PlayerPrefs.GetInt(encode_data.title + "_point" + encode_data.difficulty, 0))
                    {
                        // Fomrat: User, Difficulty, Points, MaxPoints
                        PlayerPrefs.SetInt(encode_data.title + "_point" + encode_data.difficulty, encode_data.current);
                        PlayerPrefs.SetInt(encode_data.title + "_maxPoint" + encode_data.difficulty, encode_data.max);
                    }

                    currentLoadIndex++;
                    if (currentLoadIndex % maxLoadLimit == 0) yield return null;
                }
            }
        }

        public void LoadAccountSettings()
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
                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetAudioMute_ValueKey, data.audio_mute_data ? 1 : 0);
                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetAudioVoice_ValueKey, data.audio_voice_data ? 1 : 0);

                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetInterfaceAnimation_ValueKey, data.allowIntefaceAnimation ? 1 : 0);
                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetCharacterAnimation_ValueKey, data.allowCharacterAnimation ? 1 : 0);
                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetEnemyAnimation_ValueKey, data.allowEnemyAnimation ? 1 : 0);
                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetDamageIndicatorA_ValueKey, data.allowDamageIndicatorOnAlly ? 1 : 0);
                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetDamageIndicatorB_ValueKey, data.allowDamageIndicatorOnEnemy ? 1 : 0);

                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetFrameRateLimit_ValueKey, data.maxFrameLimit);
                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetPeformanceOptimize_ValueKey, data.performanceOptimize);
                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetUnitHealthOnCharacter_ValueKey, data.unitDisplayHealthOnCharacter);
                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetUnitHealthOnEnemy_ValueKey, data.unitDisplayHealthOnEnemy);

                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetSpeedMeter_ValueKey, data.sppedMeterOption ? 0 : 1);
                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetFacnyMovement_ValueKey, data.fancyMovementOption ? 0 : 1);

                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetAutoSaveProgress_ValueKey, data.autoSaveGameProgress ? 1 : 0);
                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetAutoSaveGameSettings_ValueKey, data.autoSaveGameSettings ? 1 : 0);
                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetAutoSavePlaySettings_ValueKey, data.autoSavePlaySettings ? 1 : 0);
            }
            else
            {
                PlayerPrefs.SetString("HowToPlay_Notice", "T");
                PlayerPrefs.SetString("Control_notice", "T");
                PlayerPrefs.SetString("BattleSetup_Guide", "T");

                PlayerPrefs.SetFloat(MeloMelo_PlayerSettings.GetBGM_ValueKey, 0.5f);
                PlayerPrefs.SetFloat(MeloMelo_PlayerSettings.GetSE_ValueKey, 0.5f);
                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetAudioMute_ValueKey, 0);
                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetAudioVoice_ValueKey, 0);

                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetInterfaceAnimation_ValueKey, 1);
                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetCharacterAnimation_ValueKey, 1);
                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetEnemyAnimation_ValueKey, 1);
                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetDamageIndicatorA_ValueKey, 1);
                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetDamageIndicatorB_ValueKey, 1);

                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetFrameRateLimit_ValueKey, 1);
                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetPeformanceOptimize_ValueKey, 10);
                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetUnitHealthOnCharacter_ValueKey, 0);
                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetUnitHealthOnEnemy_ValueKey, 0);

                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetSpeedMeter_ValueKey, 1);
                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetFacnyMovement_ValueKey, 0);

                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetAutoSaveProgress_ValueKey, 1);
                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetAutoSaveGameSettings_ValueKey, 1);
                PlayerPrefs.SetInt(MeloMelo_PlayerSettings.GetAutoSavePlaySettings_ValueKey, 1);
            }
        }

        public void LoadGameplaySettings()
        {
            if (File.Exists(directory + combinePath))
            {
                PlayerGameplaySettings data = new PlayerGameplaySettings().GetSettingsData(GetProgressFile());

                PlayerPrefs.SetString("MVOption", data.mvOption ? "T" : "F");
                PlayerPrefs.SetInt("NoteSpeed", data.noteSpeed);
                PlayerPrefs.SetInt("Temp_NoteSpeed", data.noteSpeed);
                PlayerPrefs.SetString("Character_Active_Skill", data.autoSkill ? "T" : "F");

                PlayerPrefs.SetInt("AutoRetreat", data.AutoRetreat);
                PlayerPrefs.SetInt("ScoreDisplay", data.ScoreDisplay1);
                PlayerPrefs.SetInt("ScoreDisplay2", data.ScoreDisplay2);
                PlayerPrefs.SetInt("JudgeMeter_Setup", data.JudgeMeterSetup);
                PlayerPrefs.SetInt("Feedback_Display_Type_B", data.JudgeFeedback_TypeA);
                PlayerPrefs.SetInt("Feedback_Display_Type", data.JudgeFeedback_TypeB);

                PlayerPrefs.SetInt("AudioLatency_Id", data.audio_offset);
                PlayerPrefs.SetInt("InputLatency_Id", data.judge_offset);
            }
        }

        public void LoadCharacterSettings()
        {
            if (File.Exists(directory + combinePath))
            {
                PlayerCharacterSettings data = new PlayerCharacterSettings().GetSettingsData(GetProgressFile());

                for (int i = 0; i < 3; i++) { PlayerPrefs.SetString("Slot" + (i + 1) + "_charName", data.characterSlot[i]); }
                PlayerPrefs.SetString("CharacterFront", data.mainSlot);
            }
        }

        public void LoadProfileData()
        {
            if (File.Exists(directory + combinePath))
            {
                ProfileProgressDatabase data = new ProfileProgressDatabase().GetProfileData(GetProgressFile());

                PlayerPrefs.SetInt(user + "totalRatePoint", data.ratePoint);
                PlayerPrefs.SetInt(user + "UserRatePointToggle", data.ratePoint);
                PlayerPrefs.SetInt(user + "PlayedCount_Data", data.playedCount);
                PlayerPrefs.SetInt(user + "_Credits", data.creditValue);
                PlayerPrefs.SetInt(user + "_Magic Stone", data.magicStoneValue);
                PlayerPrefs.SetInt("Progress_Fragement", data.submittedMapFragment);
            }
            else
            {
                PlayerPrefs.SetInt(user + "totalRatePoint", 0);
                PlayerPrefs.SetInt(user + "UserRatePointToggle", 0);
                PlayerPrefs.SetInt(user + "PlayedCount_Data", 0);
                PlayerPrefs.SetInt(user + "_Credits", 0);
                PlayerPrefs.SetInt(user + "_Magic Stone", 0);
                PlayerPrefs.SetInt("Progress_Fragement", 0);
            }
        }

        public async Task<List<BattleProgressDatabase>> PreLoading_BattleProgressData()
        {
            return await Task.Run(() =>
            {
                if (!File.Exists(directory + combinePath)) new List<BattleProgressDatabase>();
                List<BattleProgressDatabase> dataArray = new List<BattleProgressDatabase>();

                foreach (string s in GetFormatToList())
                {
                    if (s != string.Empty)
                        dataArray.Add(new BattleProgressDatabase().GetProgressData(s));
                }

                return dataArray;
            });
        }

        public IEnumerator PostLoading_BattleProgressData(BattleProgressDatabase[] preLoaded_battlePrgoressData)
        {
            if (preLoaded_battlePrgoressData != null)
            {
                int maxLoadLimit = 50;
                int currentLoadIndex = 0;

                foreach (BattleProgressDatabase encode_data in preLoaded_battlePrgoressData)
                {
                    if (user == encode_data.user)
                    {
                        PlayerPrefs.SetString(encode_data.title + "_SuccessBattle_" + encode_data.difficulty + encode_data.area_difficulty, encode_data.success ? "T" : "F");
                        PlayerPrefs.SetInt(encode_data.title + "_techScore" + encode_data.difficulty + encode_data.area_difficulty, encode_data.score);
                    }

                    currentLoadIndex++;
                    if (currentLoadIndex % maxLoadLimit == 0) yield return null;
                }
            }
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

        public async Task<List<BattleUnitDatabase>> PreLoading_CharacterStatsData()
        {
            return await Task.Run(() =>
            {
                if (!File.Exists(directory + combinePath)) return new List<BattleUnitDatabase>();
                List<BattleUnitDatabase> dataArray = new List<BattleUnitDatabase>();

                foreach (string s in GetFormatToList())
                {
                    if (s != string.Empty)
                        dataArray.Add(new BattleUnitDatabase().GetCharacterStatus(s));
                }

                return dataArray;
            });
        }

        public IEnumerator PostLoading_CharacterStatsData(BattleUnitDatabase[] preLoaded_characterstatsData)
        {
            if (preLoaded_characterstatsData != null)
            {
                int maxLoadLimit = 50;
                int currentLoadIndex = 0;

                foreach (BattleUnitDatabase data in preLoaded_characterstatsData)
                {
                    PlayerPrefs.SetInt(data.id + "_LEVEL", data.level);
                    PlayerPrefs.SetInt(data.id + "_EXP", data.experience);
                    MeloMelo_CharacterInfo_Settings.UnlockCharacter(data.id);

                    int unUsedMasteryPoint = data.level * 2 - data.totalMasteryAdded;
                    int rebirthMasteryPoint = data.totalRebirthPoint * 99 * 2;
                    MeloMelo_ExtraStats_Settings.SetMasteryPoint(data.id, unUsedMasteryPoint + rebirthMasteryPoint);
                    MeloMelo_ExtraStats_Settings.SetRebirthPoint(data.id, data.totalRebirthPoint);

                    MeloMelo_ExtraStats_Settings.IncreaseStrengthStats(data.id, data.baseStrengthStats);
                    MeloMelo_ExtraStats_Settings.IncreaseVitalityStats(data.id, data.baseVitalityStats);
                    MeloMelo_ExtraStats_Settings.IncreaseMagicStats(data.id, data.baseMagicStats);

                    currentLoadIndex++;
                    if (currentLoadIndex % maxLoadLimit == 0) yield return null;
                }
            }
        }        

        public async Task<List<SkillUnitDatabase>> PreLoading_SkillsData()
        {
            return await Task.Run(() =>
            {
                if (!File.Exists(directory + combinePath)) return new List<SkillUnitDatabase>();
                List<SkillUnitDatabase> dataArray = new List<SkillUnitDatabase>();

                foreach (string s in GetFormatToList())
                {
                    if (s != string.Empty)
                        dataArray.Add(new SkillUnitDatabase().GetSkillDatabase(s));
                }

                return dataArray;
            });
        }

        public IEnumerator PostLoading_SkillsData(SkillUnitDatabase[] preloaded_skillData)
        {
            if (preloaded_skillData != null)
            {
                int maxLoadLimit = 50;
                int currentLoadIndex = 0;

                foreach (SkillUnitDatabase data in preloaded_skillData)
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

                    currentLoadIndex++;
                    if (currentLoadIndex % maxLoadLimit == 0) yield return null;
                }
            }
        }

        public async Task<List<AdventureStoreData>> Preloading_AdventureModeData()
        {
            return await Task.Run(() =>
            {
                if (!File.Exists(directory + combinePath)) return new List<AdventureStoreData>();
                List<AdventureStoreData> dataArray = new List<AdventureStoreData>();

                foreach (string s in GetFormatToList())
                {
                    if (s != string.Empty)
                        dataArray.Add(new AdventureStoreData().GetAdventureEditor(s));
                }

                return dataArray;
            });
        }

        public IEnumerator PostLoading_AdventureModeData(AdventureStoreData[] preLoaded_adventureModeData)
        {
            if (preLoaded_adventureModeData != null)
            {
                int maxLoadLimit = 50;
                int currentLoadIndex = 0;

                foreach (AdventureStoreData data in preLoaded_adventureModeData)
                {
                    PlayerPrefs.SetInt("RoutePlayableStatus_" + data.adventure_id + data.route_id, data.status_id);

                    currentLoadIndex++;
                    if (currentLoadIndex % maxLoadLimit == 0) yield return null;
                }
            }
        }

        public async Task<bool> PreLoading_VirtualItemData()
        {
            MeloMelo_ItemUsage_Settings.ClearItems();
            if (!File.Exists(directory + combinePath)) return false;

            List<VirtualItemDatabase> taskForLoadItem = await Task.Run(() =>
            {
                taskForLoadItem = new List<VirtualItemDatabase>();

                foreach (string s in GetFormatToList())
                {
                    if (s != string.Empty)
                        taskForLoadItem.Add(new VirtualItemDatabase().GetItemData(s));
                }

                return taskForLoadItem;
            });

            foreach (VirtualItemDatabase item in taskForLoadItem)
                MeloMelo_ItemUsage_Settings.ImportItems(item);

            return true;
        }

        public IEnumerator PostLoading_VirtualItemData()
        {
            Task<bool> isReloadedAllItem = PreLoading_VirtualItemData();
            yield return new WaitUntil(() => isReloadedAllItem.IsCompleted);
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

            data.bgm_audio_data = PlayerPrefs.GetFloat(MeloMelo_PlayerSettings.GetBGM_ValueKey);
            data.se_audio_data = PlayerPrefs.GetFloat(MeloMelo_PlayerSettings.GetSE_ValueKey);
            data.audio_mute_data = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetAudioMute_ValueKey) == 1 ? true : false;
            data.audio_voice_data = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetAudioVoice_ValueKey) == 1 ? true : false;

            data.allowIntefaceAnimation = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetInterfaceAnimation_ValueKey) == 1 ? true : false;
            data.allowCharacterAnimation = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetCharacterAnimation_ValueKey) == 1 ? true : false;
            data.allowEnemyAnimation = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetEnemyAnimation_ValueKey, 1) == 1 ? true : false;
            data.allowDamageIndicatorOnAlly = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetDamageIndicatorA_ValueKey, 1) == 1 ? true : false;
            data.allowDamageIndicatorOnEnemy = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetDamageIndicatorB_ValueKey, 1) == 1 ? true : false;

            data.maxFrameLimit = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetFrameRateLimit_ValueKey);
            data.performanceOptimize = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetPeformanceOptimize_ValueKey);
            data.unitDisplayHealthOnCharacter = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetUnitHealthOnCharacter_ValueKey, 0);
            data.unitDisplayHealthOnEnemy = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetUnitHealthOnEnemy_ValueKey, 0);
            
            data.sppedMeterOption = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetSpeedMeter_ValueKey) == 0 ? true : false;
            data.fancyMovementOption = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetFacnyMovement_ValueKey) == 0 ? true : false;

            data.autoSaveGameProgress = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetAutoSaveProgress_ValueKey, 1) == 1 ? true : false;
            data.autoSaveGameSettings = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetAutoSaveGameSettings_ValueKey, 1) == 1 ? true : false;
            data.autoSavePlaySettings = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetAutoSavePlaySettings_ValueKey, 1) == 1 ? true : false;

            JsonFormat += JsonUtility.ToJson(data);
            WriteToFile(JsonFormat);
        }

        public void SaveGameplaySettings(bool mvOption, int noteSpeed)
        {
            if (File.Exists(directory + combinePath)) 
            { File.Delete(directory + combinePath); }

            string JsonFormat = string.Empty;
            PlayerGameplaySettings data = new PlayerGameplaySettings();
            data.mvOption = mvOption;
            data.noteSpeed = noteSpeed;
            data.autoSkill = PlayerPrefs.GetString("Character_Active_Skill", "F") == "T" ? true : false;

            data.ScoreDisplay1 = PlayerPrefs.GetInt("ScoreDisplay", 0);
            data.ScoreDisplay2 = PlayerPrefs.GetInt("ScoreDisplay2", 0);
            data.JudgeMeterSetup = PlayerPrefs.GetInt("JudgeMeter_Setup", 0);

            data.JudgeFeedback_TypeA = PlayerPrefs.GetInt("Feedback_Display_Type_B", 0);
            data.JudgeFeedback_TypeB = PlayerPrefs.GetInt("Feedback_Display_Type", 1);

            data.audio_offset = PlayerPrefs.GetInt("AudioLatency_Id", 0);
            data.judge_offset = PlayerPrefs.GetInt("InputLatency_Id", 0);

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
            data.creditValue = PlayerPrefs.GetInt(user + "_Credits", 0);
            data.magicStoneValue = PlayerPrefs.GetInt(user + "_Magic Stone", 0);
            data.submittedMapFragment = PlayerPrefs.GetInt("Progress_Fragement", 0);

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

        public void SaveVirtualItemFromPlayer(string itemName, int amount, bool isStackable)
        {
            List<VirtualItemDatabase> listing = new List<VirtualItemDatabase>();
            VirtualItemDatabase itemContain = new VirtualItemDatabase(itemName, amount);

            if (File.Exists(directory + combinePath))
            {
                // Load all others existing item before overwriting the data
                foreach (string data_decode in GetFormatToList())
                    if (data_decode != string.Empty) listing.Add(new VirtualItemDatabase().GetItemData(data_decode));

                // Checking item amount isn't zero
                if (amount != 0)
                {
                    if (itemContain.amount < 0)
                    {
                        // Find existing item and modify the value
                        for (int itemOnUsed = 0; itemOnUsed < listing.ToArray().Length; itemOnUsed++)
                        {
                            // Get item and deduct them accordingly
                            if (listing[itemOnUsed].itemName == itemContain.itemName)
                            {
                                itemContain.amount += listing[itemOnUsed].amount;
                                listing.RemoveAt(itemOnUsed);
                                if (itemContain.amount > 0) listing.Insert(itemOnUsed, itemContain);
                                break;
                            }
                        }
                    }

                    else
                    {
                        bool isItemEmpty = false;

                        for (int itemToAdd = 0; itemToAdd < listing.ToArray().Length; itemToAdd++)
                        {
                            // Get item and add into the current amount
                            if (listing[itemToAdd].itemName == itemContain.itemName && isStackable)
                            {
                                itemContain.amount += listing[itemToAdd].amount;
                                listing.RemoveAt(itemToAdd);
                                listing.Insert(itemToAdd, itemContain);
                                isItemEmpty = true;
                                break;
                            }
                        }

                        if (!isItemEmpty)
                        {
                            // Create new slot of item stack them together. Otherwise make them individually
                            if (isStackable) listing.Add(itemContain);
                            else
                            {
                                for (int itemSpawn = 0; itemSpawn < itemContain.amount; itemSpawn++)
                                    listing.Add(new VirtualItemDatabase(itemContain.itemName, 1));
                            }
                        }
                    }
                }

                // Remove old data file from directory
                File.Delete(directory + combinePath);
            }

            // Create new slot for item
            else if (itemContain.amount > 0) listing.Add(itemContain);

            string jsonFormat = string.Empty;
            foreach (VirtualItemDatabase list in listing) { jsonFormat += JsonUtility.ToJson(list) + "/"; }
            WriteToFile(jsonFormat);
        }

        public void SaveExchangeTranscationHistory(string jsonData)
        {
            string currentTranscation = jsonData + "/";
            if (File.Exists(directory + combinePath))
            {
                currentTranscation += GetProgressFile();
                File.Delete(directory + combinePath);
            }

            WriteToFile(currentTranscation);
        }

        public void SaveAdventureRoutePlay()
        {
            if (File.Exists(directory + combinePath))
            { File.Delete(directory + combinePath); }

            string jsonFormat = string.Empty;
            int routeCount = 0;

            for (int story_type_id = 0; story_type_id < 2; story_type_id++)
            {
                routeCount = 1;

                while (PlayerPrefs.HasKey("RoutePlayableStatus_" + story_type_id + routeCount))
                {
                    AdventureStoreData newStoryAdd = new AdventureStoreData();
                    newStoryAdd.adventure_id = story_type_id;
                    newStoryAdd.route_id = routeCount;
                    newStoryAdd.status_id = PlayerPrefs.GetInt("RoutePlayableStatus_" + story_type_id + routeCount);

                    jsonFormat += JsonUtility.ToJson(newStoryAdd) + "/";
                    Debug.Log("Saved Adventure: " + story_type_id + "_id_" + routeCount);
                    routeCount++;
                }
            }

            WriteToFile(jsonFormat);
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
    [System.Serializable]
    public struct TrackEventStorage
    {
        public string title;
        public string cover;
        public int difficulty;
        public string level;
        public float score;
        public int point;

        public TrackEventStorage GetTrack(string format)
        {
            Debug.Log(format);
            return JsonUtility.FromJson<TrackEventStorage>(format);
        }
    }

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

        public void SaveProgressProfile(int totalRatePoint, int playedCount, int credit)
        {
            WWWForm profile = new WWWForm();
            profile.AddField("User", portId);
            profile.AddField("RatePoint", totalRatePoint);
            profile.AddField("Count", playedCount);
            profile.AddField("Credit", credit);

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

        public void SaveCharacterStatusData(string name, int level, int experience, int mastery, int rebirth, int str_stats, int vit_stats, int mag_stats)
        {
            WWWForm stats = new WWWForm();
            stats.AddField("User", portId);
            stats.AddField("CharacterName", name);
            stats.AddField("Level", level);
            stats.AddField("Exp", experience);

            stats.AddField("Mastery_Point", mastery);
            stats.AddField("Rebirth_Count", rebirth);
            stats.AddField("Base_STR", str_stats);
            stats.AddField("Base_VIT", vit_stats);
            stats.AddField("Base_MAG", mag_stats);

            const string serverAPI = "MeloMelo_Save_CharacterStatusData_2024.php";
            GetAlternativeServer(serverAPI, stats);
        }

        public void SaveChartDistributionData(int index, string jsonData)
        {
            List<TrackEventStorage> listing = new List<TrackEventStorage>();
            string[] dataS = jsonData.Split("/t");

            if (!string.IsNullOrEmpty(jsonData))
            {
                foreach (string data in dataS)
                {
                    try
                    {
                        TrackEventStorage temp = new TrackEventStorage().GetTrack(data);
                        if (data != string.Empty) listing.Add(temp);
                    } catch { }
                }
            }

            foreach (TrackEventStorage entry in listing)
            {
                WWWForm info = new WWWForm();
                info.AddField("User", portId);
                info.AddField("Title", entry.title);
                info.AddField("CoverImage", entry.cover);
                info.AddField("Difficulty", entry.difficulty);
                info.AddField("Level", entry.level);
                info.AddField("Score", (int)entry.score);
                info.AddField("Point", entry.point);
                info.AddField("Type", index);

                const string serverAPI = "MeloMelo_Save_TrackDistributionList_2024.php";
                GetAlternativeServer(serverAPI, info);
            }
        }

        public void ClearCacheDistributionData(int index)
        {
            WWWForm user = new WWWForm();
            user.AddField("User", portId);
            user.AddField("clearIndex", index);
            const string serverAPI = "MeloMelo_Clear_TrackDistributionList_2024.php";
            GetAlternativeServer(serverAPI, user);
        }

        public void SaveItemDataToServer(VirtualItemDatabase[] items)
        {
            WWWForm userClearId = new WWWForm();
            userClearId.AddField("User", portId);

            foreach (VirtualItemDatabase item in items)
            {
                WWWForm item_form = new WWWForm();
                item_form.AddField("User", portId);
                item_form.AddField("Item", item.itemName);
                item_form.AddField("Amount", item.amount);
                string serverAPI = "MeloMelo_Save_VirtualItemData_2025.php";
                GetAlternativeServer(serverAPI, item_form);
            }
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
            ResultMenu_Script.thisRes.gameObject.GetComponent<ServerCloud_Save_Script>().ConnectionEstablish(info, url_directory + url);
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
        private enum CloudLoad_ProfileField { UserID, RatePoint, PlayedCount, Credit, TotalField };
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
        private enum CloudLoad_CharacterData { Name, Level, Experience, TotalMastery, TotalRebirth, TotalStrStats, TotalVitStats, TotalMagStats, TotalField };
        private enum CloudLoad_MarathonProgress { Title, ClearedStatus, PlayedCount, Score, TotalField };

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

                    // Load playedCount data
                    PlayerPrefs.SetInt
                        (
                            profileData[id + (int)CloudLoad_ProfileField.UserID] + "_Credits",
                            int.Parse(profileData[id + (int)CloudLoad_ProfileField.Credit])
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
                    PlayerPrefs.SetFloat(MeloMelo_PlayerSettings.GetBGM_ValueKey, float.Parse(configurationData[id + (int)CloudLoad_PlayerSettingData.BGM]));

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

                for (int status = 0; status < characterStatus.Length / (int)CloudLoad_CharacterData.TotalField; status++)
                {
                    int id = status * (int)CloudLoad_LastSelectionVisited.TotalField;
                    string className = characterStatus[id + (int)CloudLoad_CharacterData.Name];

                    // Load progress to game application
                    MeloMelo_CharacterInfo_Settings.UnlockCharacter(className);
                    PlayerPrefs.SetInt(className + "_LEVEL", int.Parse(characterStatus[id + (int)CloudLoad_CharacterData.Level]));
                    PlayerPrefs.SetInt(className + "_EXP", int.Parse(characterStatus[id + (int)CloudLoad_CharacterData.Experience]));

                    // Extra: Character Stats
                    MeloMelo_ExtraStats_Settings.IncreaseStrengthStats(className, int.Parse(characterStatus[id + (int)CloudLoad_CharacterData.TotalStrStats]));
                    MeloMelo_ExtraStats_Settings.IncreaseVitalityStats(className, int.Parse(characterStatus[id + (int)CloudLoad_CharacterData.TotalVitStats]));
                    MeloMelo_ExtraStats_Settings.IncreaseMagicStats(className, int.Parse(characterStatus[id + (int)CloudLoad_CharacterData.TotalMagStats]));

                    // Extra: Attribute Point
                    MeloMelo_ExtraStats_Settings.SetMasteryPoint(className, int.Parse(characterStatus[id + (int)CloudLoad_CharacterData.TotalMastery]));
                    MeloMelo_ExtraStats_Settings.SetRebirthPoint(className, int.Parse(characterStatus[id + (int)CloudLoad_CharacterData.TotalRebirth]));
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
                    if (File.Exists(fileDirectory)) File.Delete(fileDirectory);
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
                    writeToLocal.Dispose();
                }
            }
            else
            {
                string channel_Build = Application.isEditor ? "Assets/" : "MeloMelo_Data/";
                for (int chart = 0; chart < 3; chart++)
                {
                    string fileDirectory = channel_Build + "StreamingAssets/LocalData/MeloMelo_LocalSave_ChartList/TempPass_ChartData_" + (chart + 1) + ".json";
                    if (File.Exists(fileDirectory)) File.Delete(fileDirectory);
                }
            }

            cloudLogging.Add(load.downloadHandler.text != "Empty");
            Debug.Log("Server: Load Chart Distribution Successful! [" + load.downloadHandler.text + "]");
            load.Dispose();
        }

        public IEnumerator LoadItemFromServer()
        {
            WWWForm info = new WWWForm();
            info.AddField("User", portId);
            counter++;

            const string serverAPI = "MeloMelo_Load_VirtualItemData_2025.php";
            UnityWebRequest load = UnityWebRequest.Post(urlWeb + serverAPI, info);
            yield return load.SendWebRequest();

            if (load.downloadHandler.text != "Empty")
            {
                string creatorText = string.Empty;
                string[] allItemRetrieve = load.downloadHandler.text.Split("\n");
                foreach (string itemData in allItemRetrieve) creatorText += itemData + "/";
                if (allItemRetrieve.Length > 0) MeloMelo_ItemUsage_Settings.ImportItems(new VirtualItemDatabase().GetItemData(creatorText));
            }

            cloudLogging.Add(load.downloadHandler.text != "Empty");
            Debug.Log("Server: Load Item Data Successful! [" + load.downloadHandler.text + "]");
            load.Dispose();
        }

        public IEnumerator LoadMarathonContentListing()
        {
            WWWForm info = new WWWForm();
            counter++;

            const string serverAPI = "MeloMelo_LoadExtensionMarathonContent_2025.php";
            UnityWebRequest load = UnityWebRequest.Post(urlWeb + serverAPI, info);
            yield return load.SendWebRequest();

            if (load.downloadHandler.text != "Empty")
            {
                MeloMelo_ExtensionContent_Settings.LoadMarathonContent(load.downloadHandler.text);
                Debug.Log("Server: Load Extension Marathon Content Successful! [ COMPLETED! ]");
            }
            else
                Debug.Log("Server: Load Extension Marathon Content Successful! [ FAILED! ]");

            cloudLogging.Add(load.downloadHandler.text != "Empty");
            load.Dispose();
        }

        public IEnumerator LoadMarathonProgress()
        {
            WWWForm info = new WWWForm();
            info.AddField("User", portId);
            counter++;

            const string serverAPI = "MeloMelo_Load_MarathonProgress_2025.php";
            UnityWebRequest load = UnityWebRequest.Post(urlWeb + serverAPI, info);
            yield return load.SendWebRequest();

            if (load.downloadHandler.text != "Empty")
            {
                string[] context = load.downloadHandler.text.Split("\t");

                for (int id = 0; id < context.Length / (int)CloudLoad_MarathonProgress.TotalField; id++)
                {
                    int current = id * (int)CloudLoad_MarathonProgress.TotalField;

                    PlayerPrefs.SetInt("MarathonProgress_Title_" + context[current + (int)CloudLoad_MarathonProgress.Title], 1);
                    PlayerPrefs.SetInt("MarathonProgress_PlayedCount_" + context[current + (int)CloudLoad_MarathonProgress.Title], int.Parse(context[current + (int)CloudLoad_MarathonProgress.PlayedCount]));
                    PlayerPrefs.SetString("MarathonProgress_Cleared_" + context[current + (int)CloudLoad_MarathonProgress.Title], context[current + (int)CloudLoad_MarathonProgress.ClearedStatus] == "1" ? "T" : "F");
                    PlayerPrefs.SetInt("MarathonProgress_Score_" + context[current + (int)CloudLoad_MarathonProgress.Title], int.Parse(context[current + (int)CloudLoad_MarathonProgress.Score]));
                }
            }

            cloudLogging.Add(load.downloadHandler.text != "Empty");
            Debug.Log("Server: Load Marathon Data Successful! [" + load.downloadHandler.text + "]");
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

        public Authenticate_DataManagement(string url)
        {
            LogOnSucess = false;
            portId = string.Empty;
            urlWeb = url + url_directory;
        }

        public IEnumerator AuthenticateUser(string user, string key)
        {
            WWWForm login = new WWWForm();
            login.AddField("User", user);
            login.AddField("Pass", key);

            UnityWebRequest authenticate = UnityWebRequest.Post(urlWeb + "MeloMelo_TempPassB.php", login);
            yield return authenticate.SendWebRequest();

            LogOnSucess = authenticate.downloadHandler.text.Split("\n")[0] == "Transfer Successful!";
            if (LogOnSucess)
            {
                portId = user;
                PlayerPrefs.SetString("Authentication_Login_PlayerName", authenticate.downloadHandler.text.Split("\n")[1]);
            }

            authenticate.Dispose();
        }

        public string GetUserPlayerName()
        {
            const string defaultName = "GUEST";
            return PlayerPrefs.GetString("Authentication_Login_PlayerName", defaultName);
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
        struct RatingCaterogyData
        {
            public string title;
            public int value;
            public Color border;

            public RatingCaterogyData(string title, int value, Color border)
            {
                this.title = title;
                this.value = value;
                this.border = border;
            }
        }

        private List<RatingCaterogyData> arrayData;
        private GameObject profile;

        public RatePointIndicator(string profilerName)
        {
            profile = GameObject.Find(profilerName) != null ? GameObject.Find(profilerName) : null;
            GetRateContentData();
        }

        #region SETUP
        private void GetRateContentData()
        {
            arrayData = new List<RatingCaterogyData>();

            arrayData.Add(new RatingCaterogyData("Rhodonite", 18500, Color.magenta));
            arrayData.Add(new RatingCaterogyData("Gold", 18000, Color.yellow));
            arrayData.Add(new RatingCaterogyData("Amethyst", 16000, new Color32(66, 0, 255, 255)));
            arrayData.Add(new RatingCaterogyData("Topaz", 13000, new Color32(255, 128, 0, 255)));
            arrayData.Add(new RatingCaterogyData("Red", 10000, Color.red));
            arrayData.Add(new RatingCaterogyData("Blue", 6000, Color.blue));
            arrayData.Add(new RatingCaterogyData("Green", 3000, Color.green));
            arrayData.Add(new RatingCaterogyData("White", 1, Color.white));
            arrayData.Add(new RatingCaterogyData("UnRated", 0, Color.black));
        }
        #endregion

        #region MAIN
        public void ProfileUpdate(string user, string label_1, string label_2)
        {
            if (profile != null)
            {
                profile.transform.GetChild(0).GetComponent<Text>().text = user;
                profile.transform.GetChild(2).GetComponent<Text>().text = label_1 + PlayerPrefs.GetInt(user + "PlayedCount_Data", 0);
                profile.transform.GetChild(4).GetComponent<Text>().text = label_2 + PlayerPrefs.GetInt(user + "totalRatePoint", 0);
            }

            UpdateRatePointMeter(PlayerPrefs.GetInt(user + "totalRatePoint", 0));
            UpdateColor_Border(user);
        }

        public void UpdateColor_Border(string user)
        {
            if (profile != null)
            {
                // Update Border Color
                for (int i = 0; i < arrayData.ToArray().Length; i++)
                {
                    if (PlayerPrefs.GetInt(user + "totalRatePoint", 0) >= arrayData[i].value)
                    {
                        profile.GetComponent<RawImage>().color = arrayData[i].border;
                        break;
                    }
                }
            }
        }
        #endregion

        #region COMPONENT
        private void UpdateRatePointMeter(int current)
        {
            if (profile != null)
            {
                for (int maxRate = 0; maxRate < arrayData.ToArray().Length; maxRate++)
                {
                    if (current >= arrayData[maxRate].value)
                    {
                        profile.transform.GetChild(3).GetComponent<Slider>().minValue = (maxRate != arrayData.ToArray().Length - 1) ? arrayData[maxRate].value : arrayData[arrayData.ToArray().Length - 1].value;
                        profile.transform.GetChild(3).GetComponent<Slider>().maxValue = (maxRate - 1 > -1) ? arrayData[maxRate - 1].value : arrayData[arrayData.ToArray().Length - 1].value;
                        profile.transform.GetChild(3).GetComponent<Slider>().value = current;
                        profile.transform.GetChild(6).GetComponent<Text>().text = arrayData[maxRate].title;
                        profile.transform.GetChild(7).GetComponent<Text>().text = FindMaxOutRatePoint(current);
                        break;
                    }
                }
            }
        }

        private string FindMaxOutRatePoint(int current)
        {
            if (current >= arrayData[0].value) 
                return "- MAX OUT -";
            else
                return profile.transform.GetChild(3).GetComponent<Slider>().maxValue - current + " more point" + ((current > 1) ? "s" : "");
        }
        #endregion
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
            if (level >= statsListing.ToArray().Length) return statsListing[statsListing.ToArray().Length - 1];
            else return statsListing[level < 0 ? 0 : (level - 1)];
        }
    }

    public class StatsDistribution
    {
        public readonly int baseHealth = 10;
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
                slot_Stats[i].UpdateStatsCache(false);
                ElemetStartingStats baseStats = MeloMelo_ExtensionContent_Settings.GetStatsWithElementBonus(
                    slot_Stats[i].elementType == ClassBase.ElementStats.Light ? "Light" :
                    slot_Stats[i].elementType == ClassBase.ElementStats.Dark ? "Dark" :
                    slot_Stats[i].elementType == ClassBase.ElementStats.Earth ? "Earth" : "None");

                UnitGroup currentUnit = SelectionMenu_Script.thisSelect.get_selection.get_form.Insert_Enemy
                    [PlayerPrefs.GetInt("BattleDifficulty_Mode", 1) - 1].myEnemySlot[i];
                ElemetStartingStats basicStats = MeloMelo_ExtensionContent_Settings.GetStatsWithElementBonus("None");

                switch (index)
                {
                    case "Enemy":
                        float damageAfterStats = currentUnit.str * basicStats.strength;

                        float damageResistedFromCharacter = basicStats.vitality * (slot_Stats[i].vitality +
                            MeloMelo_ExtraStats_Settings.GetExtraVitaltyStats(slot_Stats[i].name) + 
                            MeloMelo_ItemUsage_Settings.GetPowerBoost(slot_Stats[i].name));

                        float damageBonusResistanceFromCharacter = basicStats.vitality *
                            MeloMelo_ItemUsage_Settings.GetPowerBoostByMultiply(slot_Stats[i].name);

                        DMG += currentUnit.DMG + (int)damageAfterStats - ((int)(damageResistedFromCharacter + damageBonusResistanceFromCharacter * 0.01f * 80));
                        if (DMG <= 0) DMG = 0;
                        break;

                    case "Character":
                        float originalDamage = baseStats.strength + baseStats.strength * (slot_Stats[i].strength +
                            MeloMelo_ExtraStats_Settings.GetExtraStrengthStats(slot_Stats[i].name));

                        float damageResistedFromEnemy = currentUnit.vit * baseStats.vitality;

                        DMG += (int)originalDamage + MeloMelo_ItemUsage_Settings.GetPowerBoost(slot_Stats[i].name) + 
                            (int)originalDamage * MeloMelo_ItemUsage_Settings.GetPowerBoostByMultiply(slot_Stats[i].name) 
                            - (int)(damageResistedFromEnemy * 0.01f * 80);
                        if (DMG <= 0) DMG = 0;
                        break;
                }
            }
            return DMG;
        }

        public int get_UnitSpellResist(string index)
        {
            float resistance = 0;
            for (int i = 0; i < 3; i++)
            {
                switch (index)
                {
                    case "Character":
                        slot_Stats[i].UpdateStatsCache(false);
                        ElemetStartingStats baseStats = MeloMelo_ExtensionContent_Settings.GetStatsWithElementBonus(
                            slot_Stats[i].elementType == ClassBase.ElementStats.Light ? "Light" :
                            slot_Stats[i].elementType == ClassBase.ElementStats.Dark ? "Dark" :
                            slot_Stats[i].elementType == ClassBase.ElementStats.Earth ? "Earth" : "None");

                        resistance += slot_Stats[i].magic - slot_Stats[i].vitality * baseStats.multipler;
                        if (resistance <= 0) resistance = 0;
                        break;

                    case "Enemy":
                        UnitGroup currentUnit = SelectionMenu_Script.thisSelect.get_selection.get_form.Insert_Enemy
                            [PlayerPrefs.GetInt("BattleDifficulty_Mode", 1) - 1].myEnemySlot[i];
                        ElemetStartingStats basicStats = MeloMelo_ExtensionContent_Settings.GetStatsWithElementBonus("None");

                        resistance += currentUnit.mag - currentUnit.vit * basicStats.multipler;
                        if (resistance <= 0) resistance = 0;
                        break;
                }
            }
            return (int)resistance;
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
                        UnitGroup currentUnit = SelectionMenu_Script.thisSelect.get_selection.get_form.Insert_Enemy[PlayerPrefs.GetInt("BattleDifficulty_Mode", 1) - 1].myEnemySlot[i];
                        ElemetStartingStats basicStats = MeloMelo_ExtensionContent_Settings.GetStatsWithElementBonus("None");

                        HP += (PlayerPrefs.HasKey("Mission_Played") ? 0 : PreSelection_Script.thisPre.get_AreaData.EnemyBaseHealth[PlayerPrefs.GetInt("BattleDifficulty_Mode", 1) - 1])
                            + currentUnit.HP + (int)(baseHealth * basicStats.multipler * currentUnit.vit);
                        break;

                    case "Character":
                        if (slot_Stats[i].icon != null)
                        {
                            slot_Stats[i].UpdateStatsCache(false);
                            ElemetStartingStats baseStats = MeloMelo_ExtensionContent_Settings.GetStatsWithElementBonus(
                                slot_Stats[i].elementType == ClassBase.ElementStats.Light ? "Light" :
                                slot_Stats[i].elementType == ClassBase.ElementStats.Dark ? "Dark" :
                                slot_Stats[i].elementType == ClassBase.ElementStats.Earth ? "Earth" : "None");

                            float originalValue = slot_Stats[i].health + (baseHealth * baseStats.multipler * 
                                (slot_Stats[i].vitality + MeloMelo_ExtraStats_Settings.GetExtraVitaltyStats(slot_Stats[i].name)));

                            HP += (int)originalValue + MeloMelo_ItemUsage_Settings.GetPowerBoost(slot_Stats[i].name) +
                            (int)originalValue * MeloMelo_ItemUsage_Settings.GetPowerBoostByMultiply(slot_Stats[i].name);
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
            float power = 0;
            for (int id = 0; id < slot_Stats.Length; id++)
            {
                if (classType == slot_Stats[id].name)
                {
                    slot_Stats[id].UpdateStatsCache(false);
                    ElemetStartingStats baseStats = MeloMelo_ExtensionContent_Settings.GetStatsWithElementBonus(
                        slot_Stats[id].elementType == ClassBase.ElementStats.Light ? "Light" :
                        slot_Stats[id].elementType == ClassBase.ElementStats.Dark ? "Dark" :
                        slot_Stats[id].elementType == ClassBase.ElementStats.Earth ? "Earth" : "None");

                    power += baseStats.strength * (slot_Stats[id].strength + MeloMelo_ExtraStats_Settings.GetExtraStrengthStats(slot_Stats[id].name));
                    power += baseStats.magic * (slot_Stats[id].magic + MeloMelo_ExtraStats_Settings.GetExtraMagicStats(slot_Stats[id].name));
                    power += (int)(baseStats.vitality * (slot_Stats[id].vitality + MeloMelo_ExtraStats_Settings.GetExtraVitaltyStats(slot_Stats[id].name)));
                    power += baseStats.multipler * baseHealth * slot_Stats[id].health;
                    return (int)power;
                }
            }

            for (int unit = 0; unit < 3; unit++)
            {
                switch (classType)
                {
                    case "Enemy":
                        UnitGroup currentUnit = SelectionMenu_Script.thisSelect.get_selection.get_form.Insert_Enemy
                            [PlayerPrefs.GetInt("BattleDifficulty_Mode", 1) - 1].myEnemySlot[unit];
                        ElemetStartingStats basicStats = MeloMelo_ExtensionContent_Settings.GetStatsWithElementBonus("None");

                        power += currentUnit.str * basicStats.strength;
                        power += currentUnit.mag * basicStats.magic;
                        power += currentUnit.vit * basicStats.vitality;
                        power += currentUnit.HP * (basicStats.multipler * baseHealth);
                        break;

                    case "Character":
                        if (slot_Stats[unit].icon != null)
                        {
                            slot_Stats[unit].UpdateStatsCache(false);
                            ElemetStartingStats baseStats = MeloMelo_ExtensionContent_Settings.GetStatsWithElementBonus(
                                slot_Stats[unit].elementType == ClassBase.ElementStats.Light ? "Light" :
                                slot_Stats[unit].elementType == ClassBase.ElementStats.Dark ? "Dark" :
                                slot_Stats[unit].elementType == ClassBase.ElementStats.Earth ? "Earth" : "None");

                            power += baseStats.strength * (slot_Stats[unit].strength + MeloMelo_ExtraStats_Settings.GetExtraStrengthStats(slot_Stats[unit].name));
                            power += baseStats.magic * (slot_Stats[unit].magic + MeloMelo_ExtraStats_Settings.GetExtraMagicStats(slot_Stats[unit].name));
                            power += (int)(baseStats.vitality * (slot_Stats[unit].vitality + MeloMelo_ExtraStats_Settings.GetExtraVitaltyStats(slot_Stats[unit].name)));
                            power += baseStats.multipler * baseHealth * slot_Stats[unit].health;
                            PlayerPrefs.SetInt("Character_OverallPower", (int)power);
                        }
                        break;

                    default:
                        break;
                }
            }
                    
            return (int)power;
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