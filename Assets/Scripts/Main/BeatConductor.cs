using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using MeloMelo_LevelBuilder;
using MeloMelo_ScoreEditorMode;

public class BeatConductor : MonoBehaviour
{
    public static BeatConductor thisBeat;

    // Performace Score: GameProperties
    public readonly float fixedScore = 1000000;

    public enum BeatOption { MusicScore_FourBeats, MusicScore_EightBeats, Four_beat, Eight_beat };
    public BeatOption myOption = BeatOption.Four_beat;

    //public enum TimeSignature { T_34, T_44 };

    //private int minutes_H = 0;
    private int seconds_H = 0;
    private int milseconds_H = -1;
    private float BPM_Calcuate = 0;
    public float get_BPM_Calcuate { get { return BPM_Calcuate; } } 

    public MusicScore Music_Database;

    public Text BeatIndicator;
    private decimal NextTickTime = 0.000m;

    private GameObject note;
    public GameObject spawner;
    private bool startNote = false;
    public bool get_startNote { get { return startNote; } }

    public Text FeedingTime_sign;
    private int totalTickCounter = 0;
    public int get_spawnObject { get { return totalTickCounter; } }

    private int[] myData_Note = new int[2000];
    public int[] get_myData_Note { get { return myData_Note; } set { myData_Note = value; } }

    ScorePrinter myPrint = new ScorePrinter();
    private bool printScore = false;

    private float score_perfect2 = 0;
    public float get_scorePerfect2 { get { return score_perfect2; } }

    private float score_perfect = 0;
    public float get_scorePerfect { get { return score_perfect; } }

    private float score_bad = 0;
    public float get_scoreBad { get { return score_bad; } }

    private int[] myData_Note2 = new int[2000];
    public int[] get_myData_Note2 { get { return myData_Note2; } set { myData_Note2 = value; } }

    public GameObject SideUI_MusicInfo;
    private float SongDuration = 0;
    private float dspSongTime = 0;
    private float currentDuration = 0;

    private float musicPerformaceScore = 0;
    public float get_musicPerformanceScore { get { return musicPerformaceScore; } }

    public ParticleSystem SpeedMeter;
    private ChartModification[] noteModding;
    private string userInput;

    private List<GameObject> noteDataArray;
    [SerializeField] private GameObject whiteTick;
    [SerializeField] private GameObject specialChartTick;
    [SerializeField] private GameObject normalTick;
    [SerializeField] private GameObject blankTick;
    [SerializeField] private GameObject lastNoteTick;
    private int spawnCounter = 0;

    private int noteSpeed = 0;
    public int get_noteSpeed { get { return noteSpeed; } }

    [Header("Note Settings")]
    public int speedIndex;
    public bool autoPlayField;

    // Loading up all component through the game
    void Start()
    {
        thisBeat = this;

        try
        {
            Music_Database = ((PlayerPrefs.HasKey("Mission_Played")) ? BlackBoard_Scene.myBoard.get_musicData :
            SelectionMenu_Script.thisSelect.get_selection.get_form);
        }
        catch { }
         
        GetComponent<AudioSource>().clip = Music_Database.Music;
        BPM_Calcuate = 60 / Music_Database.BPM;
        NextTickTime = (decimal)Time.time + ((decimal)Music_Database.offset / 1000);

        // Gerenate score for playing
        Instantiate(Music_Database.ScoreObject, transform.position, Quaternion.identity);

        musicPerformaceScore = PlayerPrefs.GetInt(Music_Database.Title + "_score" + PlayerPrefs.GetInt("DifficultyLevel_valve", 1), 0);
        Invoke("UpdateMusicInfo", 0.05f);
        Invoke("StartEncode", 0.05f);

        // New Chart Content
        noteModding = Music_Database.addons3b;
    }

    void StartEncode()
    {
        int[] noteData = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 93 };
        StartCoroutine(IntiNoteArrayData(noteData));

        if (!GameManager.thisManager.DeveloperMode) { GameManager.thisManager.Invoke("GameStarting", 0.05f); }
        else { Invoke("StartMusicButton", 2); }
    }

    IEnumerator IntiNoteArrayData(int[] noteData)
    {
        noteDataArray = new List<GameObject>();

        for (int note = 0; note < noteData.Length; note++)
        {
            ResourceRequest intiNote = Resources.LoadAsync<GameObject>("Prefabs/Note/Note" +  noteData[note]);
            yield return new WaitUntil(() => intiNote.isDone);

            GameObject LoadedNote = intiNote.asset as GameObject;
            noteDataArray.Add(LoadedNote);
        }
    }

    // Music Information Panel: Load Panel
    void UpdateMusicInfo()
    {
        // Update Music Info
        if (!GameManager.thisManager.DeveloperMode)
        {
            SideUI_MusicInfo.transform.GetChild(0).GetComponent<RawImage>().texture = Music_Database.Background_Cover;
            SideUI_MusicInfo.transform.GetChild(1).GetComponent<Text>().text = Music_Database.ArtistName;
            SideUI_MusicInfo.transform.GetChild(2).GetComponent<Text>().text = Music_Database.Title;
            try { userInput = LoginPage_Script.thisPage.GetUserPortOutput(); }
            catch { userInput = GuestLogin_Script.thisScript == null ? "Level_Editor" : GuestLogin_Script.thisScript.get_entry; }

            SideUI_MusicInfo.transform.GetChild(4).GetComponent<Text>().text = "Player: " + userInput;

            dspSongTime = (float)AudioSettings.dspTime;
            SongDuration = GetComponent<AudioSource>().clip.length;
            GameObject.Find("Duration").GetComponent<Slider>().maxValue = SongDuration;
        }
    }

    // Update Function: Normal Tick
    void Update()
    {
        // Get track playback
        TrackDuration();

        // Get battle progress ux alerting
        if (!GameManager.thisManager.DeveloperMode) TriggerWarningMeter();
    }

    // Warning! High Memory Capacity: Update Function
    void FixedUpdate()
    {
        if (GetComponent<AudioSource>().isPlaying) { Music_Conductor(); }
        else if (!GetComponent<AudioSource>().isPlaying && startNote && !printScore && GameManager.thisManager.DeveloperMode)
        {
            printScore = true;
            myPrint.ScoringWriter();            
        }
    }

    #region MAIN
    // Music PlayThrough: Starting Point
    public void StartMusicButton()
    {
        if (!GameManager.thisManager.DeveloperMode) { GameObject.Find("MidAlert").gameObject.SetActive(false); }

        if (GameManager.thisManager.DeveloperMode) { myPrint.SongTitle(Music_Database.name); }
        NextTickTime = (decimal)Time.time + ((decimal)Music_Database.offset / 1000);

        // Setup Video Background
        VideoPlayerInstance();

        // Begin Meter Indicator
        SpawnMeterIndicatorInstance();

        // Setup scoring format
        ScoreInstanceFormat();

        // Auto Gerenate Level Difficulty
        LevelComplexityProgram(CreateScoringInstance());
    }

    // Score Reader: High Memory Capacity
    void Music_Conductor()
    {
        if (!startNote && (decimal)Time.time >= NextTickTime - (decimal)BPM_Calcuate)
        {
            if (myData_Note2[0] != 0) { SpawnTargetAsNote(note, spawner.transform.position, true); }
            if (whiteTick) GetTimingMarker(whiteTick);
            startNote = true;
        }

        if ((decimal)Time.time >= NextTickTime)
        {
            if (!GameManager.thisManager.DeveloperMode)
            {
                bool check = false;

                NextTickTime = (decimal)Time.time + (decimal)BPM_Calcuate;
                totalTickCounter++;

                if (Music_Database.UseBPMDisplay) Tick2_millseconds(8);
                else Tick2_millseconds(4);

                // Primary Note
                if (ChartReader())
                {
                    GetLastMarker();
                    check = true;
                }

                // Secondary Note
                if (Music_Database.NewChartSystem)
                {
                    // MultiCharting, Pattern Chart, Custom Properties Pattern
                    if (NewChartMultiplePattern() || NewChartPatternCreation() || NewChartPatternModded())
                    {
                        GetLastMarker();
                        check = true;
                    }
                }

                // Get blank line
                if (!check) GetTimingMarker(blankTick);
            }
            else
            {
                NextTickTime = (decimal)Time.time + (decimal)BPM_Calcuate;
                totalTickCounter++;

                if (myData_Note2[totalTickCounter] != 0) { SpawnTargetAsNote(note, new Vector3(Random.Range(-GameManager.thisManager.get_playField.get_limitBorder, GameManager.thisManager.get_playField.get_limitBorder), spawner.transform.position.y, spawner.transform.position.z), true); }
                GameObject.Find("Ticker").GetComponent<Text>().text = "Total Tick: " + totalTickCounter;

                Tick2_millseconds(4);
                GameObject.Find("Tick2").GetComponent<Text>().text = seconds_H + " : " + milseconds_H;
            }
        }
    }
    #endregion

    #region COMPONENT
    private void PlayAudioClip() { GetComponent<AudioSource>().Play(); }

    // Level Builder: Complexity Program
    private void LevelComplexityProgram(ScoringMeter indicator)
    {
        if (!GameManager.thisManager.DeveloperMode)
        {
            if (PlayerPrefs.HasKey("Mission_Played"))
            {
                SideUI_MusicInfo.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = "SPECIAL";

                Color32 myC = new Color32(193, 0, 255, 255);
                SideUI_MusicInfo.transform.GetChild(3).GetComponent<RawImage>().color = myC;
            }
            else
            {
                if (SelectionMenu_Script.thisSelect == null)
                {
                    print("Difficulty: ???");

                    switch (PlayerPrefs.GetInt("DifficultyLevel_valve", 1))
                    {
                        case 1:
                            PlayerPrefs.SetString("Difficulty_Normal_selectionTxt",
                                ((indicator.get_difficultyLevel - ((int)indicator.get_difficultyLevel + 0.5f) > 0f &&
                                indicator.get_difficultyLevel > 5) ? (int)indicator.get_difficultyLevel + "+" : (int)indicator.get_difficultyLevel + "")
                                );
                            break;

                        case 2:
                            PlayerPrefs.SetString("Difficulty_Hard_selectionTxt",
                                ((indicator.get_difficultyLevel - ((int)indicator.get_difficultyLevel + 0.5f) > 0f &&
                                indicator.get_difficultyLevel > 10) ? (int)indicator.get_difficultyLevel + "+" : (int)indicator.get_difficultyLevel + "")
                                );
                            break;
                    }
                }

                if (PlayerPrefs.GetInt("DifficultyLevel_valve", 1) == 1)
                {
                    SideUI_MusicInfo.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = "Normal [LV. " + PlayerPrefs.GetString("Difficulty_Normal_selectionTxt", "?") + "]";
                    SideUI_MusicInfo.transform.GetChild(3).GetComponent<RawImage>().color = Color.blue;

                }
                else if (PlayerPrefs.GetInt("DifficultyLevel_valve", 1) == 2)
                {
                    SideUI_MusicInfo.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = (indicator.get_difficultyLevel >= 16 ? "Expert [LV. " : "Hard [LV. ") + PlayerPrefs.GetString("Difficulty_Hard_selectionTxt", "?") + "]";
                    SideUI_MusicInfo.transform.GetChild(3).GetComponent<RawImage>().color = (indicator.get_difficultyLevel >= 16 ? new Color(1, 0.09f, 0.87f) : indicator.get_difficultyLevel >= 11 ? new Color(0.47f, 0, 1) : Color.red);
                }
                else if (PlayerPrefs.GetInt("DifficultyLevel_valve", 1) == 3)
                {
                    SideUI_MusicInfo.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = "Ulimate [LV. " + PlayerPrefs.GetString("Difficulty_Ultimate_selectionTxt", "?") + "]";
                    SideUI_MusicInfo.transform.GetChild(3).GetComponent<RawImage>().color = new Color(1, 0.4f, 0);
                }

                print("Difficulty Level: " + indicator.get_difficultyLevel);
                PlayerPrefs.SetFloat("difficultyLevel_play2", indicator.get_difficultyLevel);
            }
        }
    }

    // Duration Function
    private void TrackDuration()
    {
        currentDuration = (float)(AudioSettings.dspTime - dspSongTime);
        if (currentDuration <= SongDuration)
            GameObject.Find("Duration").GetComponent<Slider>().value = currentDuration;
    }

    // Warning Alert: Battle Progress Meter
    private void TriggerWarningMeter()
    {
        bool triggerWarning = GameManager.thisManager.get_progressMeter.ProgressDangerAlert(currentDuration > (SongDuration - 10) && startNote);
        GameObject.Find("SideUI-BattleProgress").GetComponent<Animator>().SetBool("Warning", triggerWarning);
    }

    // Video PlayThrough
    private void PlayVideoBackground()
    {
        if (!GameManager.thisManager.DeveloperMode && PlayerPrefs.GetString("MVOption", "T") == "T" && Music_Database.videoImport != null)
        {
            foreach (VideoPlayer player in GameObject.Find("MV").transform.GetComponentsInChildren<VideoPlayer>())
            {
                player.clip = Music_Database.videoImport;
                player.Play();
            }
        }
    }

    // Chart Modification: Component
    private int GetChartModificationPrimaryNote(ChartModification secondary, int primary)
    {
        if (secondary.PrimaryNote != 0) return secondary.PrimaryNote;
        return primary;
    }

    private float GetRandomXOffset(PatternLane _obj)
    {
        float x = GameObject.Find("Boss").transform.position.x;

        if (_obj.random_xOffset) return x + _obj.xOffset;
        else return _obj.xOffset;
    }

    private float GetBeatZOffset(PaterrnDefine _obj, int index)
    {
        if (_obj.matchBeat_zOffset) return 2 * index;
        else return _obj.zOffset;
    }
    #endregion

    #region ACTIVE
    private void Tick2_millseconds(int beatPerCount)
    {
        milseconds_H++;

        if (milseconds_H >= beatPerCount)
        {
            milseconds_H = 0;
            Tick2_seconds();
        }
    }

    private void Tick2_seconds()
    {
        seconds_H++;
        if (whiteTick) GetTimingMarker(whiteTick);
    }
    #endregion

    #region INSTANCE
    private void ScoreInstanceFormat()
    {
        if (GameManager.thisManager.getJudgeWindow.getOverallCombo != 0)
        {
            score_perfect2 = Mathf.Floor(fixedScore / GameManager.thisManager.getJudgeWindow.getOverallCombo);
            score_perfect = Mathf.Floor(0.5f * (fixedScore / GameManager.thisManager.getJudgeWindow.getOverallCombo));
            score_bad = Mathf.Floor(0.25f * (fixedScore / GameManager.thisManager.getJudgeWindow.getOverallCombo));
        }
    }

    private void VideoPlayerInstance()
    {
        if (Music_Database.videoOffset != 0)
        {
            if (Music_Database.videoOffset < 0)
            {
                PlayVideoBackground();
                GetComponent<AudioSource>().PlayDelayed(-Music_Database.videoOffset);
            }
            else
            {
                PlayAudioClip();
                Invoke("PlayVideoBackground", Music_Database.videoOffset);
            }
        }
        else
        {
            GetComponent<AudioSource>().Play();
            PlayVideoBackground();
        }
    }

    private void SpawnMeterIndicatorInstance()
    {
        //if (PlayerPrefs.GetInt("SpeedMeter_valve", 0) == 0 && !GameManager.thisManager.DeveloperMode) SpeedMeter.Play();

        // Note Speed Init
        //if (noteSpeed == 0) Custom_InitializeNoteSpeed(PlayerPrefs.GetInt("NoteSpeed", 0));
        //if (Application.isEditor) noteSpeed = MeloMelo_GameSettings.Custom_InitializeNoteSpeed((int)Music_Database.BPM, 6);

        // Print out audio detail
        Debug.Log("BPM: " + Music_Database.BPM + " - Offset: " + NextTickTime);
    }

    private ScoringMeter CreateScoringInstance()
    {
        ScoringMeter indicator = new ScoringMeter
            (0,
            GameManager.thisManager.getGameplayComponent.getTotalEnemy,
            GameManager.thisManager.getGameplayComponent.getTotalTraps,
            GameManager.thisManager.getJudgeWindow.getOverallCombo,
            myData_Note2
            );

        return indicator;
    }

    private void GetTimingMarker(GameObject obj)
    {
        GameObject line = Instantiate(obj);
        line.transform.position = new Vector3(spawner.transform.position.x, 0.05f, spawner.transform.position.z);
        line.transform.localScale = new Vector3(spawner.transform.localScale.x, line.transform.localScale.y, line.transform.localScale.z);
    }

    private void GetLastMarker()
    {
        if (spawnCounter >= GameManager.thisManager.getJudgeWindow.getOverallCombo) GetTimingMarker(lastNoteTick);
        else GetTimingMarker(normalTick);
    }
    #endregion

    #region CHART READER
    private bool ChartReader()
    {
        bool isSpawn = false;

        if (myData_Note2[totalTickCounter] != 0)
        {
            switch (myData_Note2[totalTickCounter])
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    note = noteDataArray[myData_Note2[totalTickCounter] - 1];
                    SpawnTargetAsNote(note, new Vector3(GameObject.Find("Boss").transform.position.x, spawner.transform.position.y, spawner.transform.position.z), true);
                    
                    spawnCounter++;
                    isSpawn = true;
                    break;

                case 93: // Item 3
                    note = noteDataArray[noteDataArray.ToArray().Length - 1];
                    SpawnTargetAsNote(note, new Vector3(GameObject.Find("Boss").transform.position.x, spawner.transform.position.y, spawner.transform.position.z), true);

                    spawnCounter++;
                    isSpawn = true;
                    break;

                case 1000: // Field Enlarge
                    GameManager.thisManager.get_playField.SwitchPlayFieldMat(false);
                    GameManager.thisManager.DrawOutPlayArea();
                    break;

                case 1001: // Field Shrink
                    GameManager.thisManager.get_playField.SwitchPlayFieldMat(true);
                    GameManager.thisManager.EraseOutPlayArea();
                    break;

                default:
                    break;
            }
        }

        return isSpawn;
    }

    private bool NewChartMultiplePattern()
    {
        bool isSpawn = false;

        if (Music_Database.addons1 != null)
        {
            foreach (MultipleCharting chart in Music_Database.addons1)
            {
                if (myData_Note2[totalTickCounter] == chart.SecondaryIndex)
                {
                    if (chart.FirstLane != 0)
                    {
                        note = Resources.Load<GameObject>("Prefabs/Note/Note" + chart.FirstLane);
                        SpawnTargetAsNote(note, new Vector3(-0.2f * chart.LaneSpacing, spawner.transform.position.y, spawner.transform.position.z), true);
                        spawnCounter++;
                    }

                    if (chart.SecondLane != 0)
                    {
                        note = Resources.Load<GameObject>("Prefabs/Note/Note" + chart.SecondLane);
                        SpawnTargetAsNote(note, new Vector3(-0.1f * chart.LaneSpacing, spawner.transform.position.y, spawner.transform.position.z), true);
                        spawnCounter++;
                    }

                    if (chart.ThirdLane != 0)
                    {
                        note = Resources.Load<GameObject>("Prefabs/Note/Note" + chart.ThirdLane);
                        SpawnTargetAsNote(note, new Vector3(0, spawner.transform.position.y, spawner.transform.position.z + chart.zOffset), true);
                        spawnCounter++;
                    }

                    if (chart.FourthLane != 0)
                    {
                        note = Resources.Load<GameObject>("Prefabs/Note/Note" + chart.FourthLane);
                        SpawnTargetAsNote(note, new Vector3(0.1f * chart.LaneSpacing, spawner.transform.position.y, spawner.transform.position.z), true);
                        spawnCounter++;
                    }

                    if (chart.FifthLane != 0)
                    {
                        note = Resources.Load<GameObject>("Prefabs/Note/Note" + chart.FifthLane);
                        SpawnTargetAsNote(note, new Vector3(0.2f * chart.LaneSpacing, spawner.transform.position.y, spawner.transform.position.z), true);
                        spawnCounter++;
                    }

                    isSpawn = true;
                }
            }
        }

        return isSpawn;
    }

    private bool NewChartPatternCreation()
    {
        bool isSpawn = false;

        if (Music_Database.addons3 != null)
        {
            foreach (PatternCharting chart in Music_Database.addons3)
            {
                // BluePrint
                if (myData_Note2[totalTickCounter] == chart.SecondaryIndex)
                {
                    for (int row = 0; row < chart.noteArray.Length; row++)
                    {
                        foreach (PatternLane col in chart.noteArray[row].noteOutput)
                        {
                            if (col.PrimaryNote != 0)
                            {
                                note = col.PrimaryNote == 93 ? noteDataArray[noteDataArray.ToArray().Length - 1] : noteDataArray[col.PrimaryNote - 1];
                                note.GetComponent<Note_Script>().hit_cycle = col.hitDelay;

                                Vector3 position = new Vector3(
                                    GetRandomXOffset(col),                      // S4
                                    spawner.transform.position.y,
                                    GetBeatZOffset(chart.noteArray[row], row)   // S4
                                    );

                                SpawnTargetAsNote(note, position, false);
                                spawnCounter++;
                            }
                        }
                    }

                    isSpawn = true;
                    //break;
                }
            }
        }

        return isSpawn;
    }

    private bool NewChartPatternModded()
    {
        bool isModded = false;

        if (Music_Database.addons3b != null)
        {
            foreach (ChartModification mod in Music_Database.addons3b)
            {
                // Modded
                if (myData_Note2[totalTickCounter] == mod.newIndex)
                {
                    // BluePrint
                    foreach (PatternCharting chart in Music_Database.addons3)
                    {
                        if (mod.SecondaryNote == chart.SecondaryIndex)
                        {
                            foreach (PaterrnDefine row in chart.noteArray)
                            {
                                foreach (PatternLane col in row.noteOutput)
                                {
                                    note = GetChartModificationPrimaryNote(mod, col.PrimaryNote) == 93 ? 
                                        noteDataArray[noteDataArray.ToArray().Length - 1] : noteDataArray[GetChartModificationPrimaryNote(mod, col.PrimaryNote) - 1];
                                    Vector3 position = new Vector3(
                                        (GetRandomXOffset(col) + mod.LanePositioning) * mod.scaling, // S4
                                        spawner.transform.position.y,
                                        GetBeatZOffset(row, 0)                                       // S4
                                        );

                                    SpawnTargetAsNote(note, position, false);
                                    spawnCounter++;
                                }
                            }

                            isModded = true;
                            //break;
                        }
                    }
                    //break;
                }
            }
        }

        return isModded;
    }

    private void SpawnTargetAsNote(GameObject target, Vector3 plotPoint, bool condition)
    {
        GameObject note = Instantiate(target, plotPoint, Quaternion.identity);
        if (condition) note.GetComponent<Note_Script>().JudgeLineToggle();
    }
    #endregion

    #region MISC 
    public void UpdateNoteSpeed(int speed)
    {
        noteSpeed = speed;
    }
    #endregion
}
