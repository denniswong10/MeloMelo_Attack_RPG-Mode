using System.Collections;
using UnityEngine;
using MeloMelo_PlayerManagement;

public class Note_Script : MonoBehaviour
{
    private bool isHit = false;
    public bool isNotationHit { get { return isHit; } }

    public int note_index;
    public CharacterSettings.PICKUP_TYPE note_define_index;

    [SerializeField] private float NotePos;
    public float get_NotePos { get { return NotePos; } }

    public int hit_cycle = 0;

    private int preJudgedNote = -1;
    public int get_preJudgeNote { get { return preJudgedNote; } }

    private int preOffBeat = -1;
    public int get_preOffBeat { get { return preOffBeat; } }

    private bool notationTimeOut = false;
    private bool notationExpired = false;
    private float noteTimeToNextHit;

    void Start()
    {
        // Options:
        GetComponent<Notation_Visual_Script>().enabled = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetAirGuide_ValueKey) == 0;
        GetComponent<Notation_Motion_Script>().UpdateMotionBehaviour(PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetFacnyMovement_ValueKey) == 1);
    }

    #region SETUP (Advance)
    private void ClearOfCompletedCycle()
    {
        switch (note_define_index)
        {
            case CharacterSettings.PICKUP_TYPE.JUMP:
            case CharacterSettings.PICKUP_TYPE.ITEM:
            case CharacterSettings.PICKUP_TYPE.ITEM3:
            case CharacterSettings.PICKUP_TYPE.TRAP:
                Destroy(gameObject);
                break;

            default:
                if (note_index != 3) Destroy(gameObject);
                break;
        }
    }
    #endregion

    #region MAIN
    // Note Function: Reserve Mode
    public bool TriggerAsHits()
    {
        bool checkForHit = isHit;
        if (!checkForHit) isHit = true;
        return checkForHit;
    }

    public void Despawn() { ClearOfCompletedCycle(); }

    public void SetNoteCompleted(int judge, int offbeat)
    {
        preJudgedNote = judge;
        preOffBeat = offbeat;
    }
    #endregion

    #region COMPONENT (MAIN: Action List)
    public void Obstacle_NoteDodge(bool audio)
    {
        notationExpired = true;

        GameManager.thisManager.UpdateNoteStatus("Perfect_2");
        GameManager.thisManager.ModifyFastNLateJudge(1, 1);

        GameManager.thisManager.UpdatePoint(3);
        GameManager.thisManager.FinalScoreMultipler(BeatConductor.thisBeat.get_scorePerfect2);

        if (!GameManager.thisManager.DeveloperMode && audio)
        {
            float volume = PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetAudioMute_ValueKey) == 1 ?
                0 : PlayerPrefs.GetFloat(MeloMelo_PlayerSettings.GetSE_ValueKey);

            AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Audio/SE/EnemyAttack"), new Vector3(0, 0, -10f), volume);
        }

        ClearOfCompletedCycle();
    }
    #endregion

    #region COMPONENT (TIMING WINDOW)
    public void BeginNotationTimeOut()
    {
        float aboutPerfect = 2 * (BeatConductor.thisBeat.get_BPM_Calcuate * 0.25f);
        float aboutMiss = 2 * (BeatConductor.thisBeat.get_BPM_Calcuate * 0.25f);
        float closingBeforeMiss = BeatConductor.thisBeat.get_BPM_Calcuate;

        noteTimeToNextHit = Time.time + (aboutPerfect + aboutMiss + closingBeforeMiss);
        notationTimeOut = true;
    }

    public void GetTimeOutNotation()
    {
        if (notationTimeOut && !notationExpired)
        {
            if (Time.time >= noteTimeToNextHit) NotationHitExpired();

            else if (note_define_index == CharacterSettings.PICKUP_TYPE.TRAP)
            {
                // Traps (Ground) Traps (Air)
                Obstacle_NoteDodge(true);
            }
        }
    }

    private void NotationHitExpired()
    {
        notationExpired = true;

        if (!isHit)
        {
            GetComponent<BoxCollider>().enabled = false;
            if (preJudgedNote < 0) GameManager.thisManager.UpdateNoteStatus("Miss");
            MeloMelo_ScoreSystem.thisSystem.UpdateScoreDisplay();

            switch (note_define_index)
            {
                case CharacterSettings.PICKUP_TYPE.NONE:
                    if (GameObject.Find("Character").GetComponent<Character>().stats.get_name != "NA")
                        GameManager.thisManager.UpdateCharacter_Health(-(PlayerPrefs.GetInt("Enemy_OverallDamage", 0) * 2), false);

                    GameManager.thisManager.SpawnDamageIndicator(transform.position, 1, -PlayerPrefs.GetInt("Enemy_OverallDamage", 0) * 2);
                    break;

                default:
                    break;
            }
        }

        GetComponent<Notation_Motion_Script>().CheckLogicOnMiss();
    }
    #endregion

    #region NOT IN USE
    //public IEnumerator CheckingPerfect_Hit()
    //{
    //    float time_beforePerfect = 2 * (BeatConductor.thisBeat.get_BPM_Calcuate / 4);
    //    yield return new WaitForSeconds(time_beforePerfect);

    //    if (note_define_index == CharacterSettings.PICKUP_TYPE.TRAP)
    //    {
    //        // Traps (Ground) Traps (Air)
    //        Obstacle_NoteDodge(true);
    //    }

    //    StartCoroutine(CheckTiming_NoteHit());
    //}

    //// Note Check: Hit Timing
    //IEnumerator CheckTiming_NoteHit()
    //{
    //    float time_beforeMiss = 2 * (BeatConductor.thisBeat.get_BPM_Calcuate / 4);
    //    yield return new WaitForSeconds(time_beforeMiss);

    //    StartCoroutine(CheckTiming_AboutMiss_NoteHit());
    //}

    //IEnumerator CheckTiming_AboutMiss_NoteHit()
    //{
    //    float time_finalelsape = BeatConductor.thisBeat.get_BPM_Calcuate;
    //    yield return new WaitForSeconds(time_finalelsape);

    //    if (!isHit)
    //    {
    //        GetComponent<BoxCollider>().enabled = false;
    //        if (preJudgedNote < 0) GameManager.thisManager.UpdateNoteStatus("Miss");
    //        MeloMelo_ScoreSystem.thisSystem.UpdateScoreDisplay();

    //        switch (note_define_index)
    //        {
    //            case CharacterSettings.PICKUP_TYPE.NONE:
    //                if (GameObject.Find("Character").GetComponent<Character>().stats.get_name != "NA")
    //                    GameManager.thisManager.UpdateCharacter_Health(-(PlayerPrefs.GetInt("Enemy_OverallDamage", 0) * 2), false);

    //                GameManager.thisManager.SpawnDamageIndicator(transform.position, 1, -PlayerPrefs.GetInt("Enemy_OverallDamage", 0) * 2);
    //                break;

    //            default:
    //                break;
    //        }
    //    }

    //    GetComponent<Notation_Motion_Script>().CheckLogicOnMiss();
    //}

    //private IEnumerator NewChartJudgeTracker(float offset, int index)
    //{
    //    Vector3 startPos = transform.position;
    //    float zTarget = destinationDesired.z + offset;

    //    float travelDistance = Mathf.Abs(startPos.z - zTarget);
    //    float finalSpeed = Mathf.Max(note_speed * BeatConductor.thisBeat.get_BPM_Calcuate, 0.001f);
    //    float duration = travelDistance / finalSpeed;

    //    double startDSPTime = AudioSettings.dspTime;

    //    while (true)
    //    {
    //        double currentDSPTime = AudioSettings.dspTime;
    //        float t = (float)((currentDSPTime - startDSPTime) / duration);
    //        t = Mathf.Clamp01(t);

    //        // Optional: Add easing here
    //        float easedT = Mathf.SmoothStep(0f, 1f, t);

    //        float z = Mathf.Lerp(startPos.z, zTarget, isMotionSmoothen ? easedT : t);
    //        transform.position = new Vector3(transform.position.x, transform.position.y, z);

    //        if (t >= 1f)
    //            break;

    //        yield return null;
    //    }

    //    // Snap to target to avoid drift
    //    transform.position = new Vector3(transform.position.x, transform.position.y, zTarget);
    //    ProcessJudgeThroughInput(index);
    //}

    //private void RegularNote_HitCheck()
    //{
    //    if (direction_type != Direction.Backward)
    //    {
    //        if (hit_cycle > 0)
    //        {
    //            hit_cycle--;
    //            Debug.Log("E");
    //            GetComponent<BoxCollider>().enabled = hit_cycle < 2;
    //            StartCoroutine(CheckingPerfect_Hit());
    //        }
    //        else
    //        {
    //            if (!isHit)
    //            {
    //                GameManager.thisManager.UpdateNoteStatus("Miss");
    //                MeloMelo_ScoreSystem.thisSystem.UpdateScoreDisplay();

    //                if (GameObject.Find("Character").GetComponent<Character>().stats.get_name != "NA")
    //                    GameManager.thisManager.UpdateCharacter_Health(-(PlayerPrefs.GetInt("Enemy_OverallDamage", 0) * 2), false);

    //                GameManager.thisManager.SpawnDamageIndicator(transform.position, 1, -PlayerPrefs.GetInt("Enemy_OverallDamage", 0) * 2);
    //            }
    //        }
    //    }
    //}

    //private void OldJudgementLine(float offset, int index)
    //{
    //    if (transform.position.z <= GameObject.Find("Judgement Line").transform.position.z + offset)
    //    {
    //        switch (index)
    //        {
    //            case 1:
    //                GetComponent<BoxCollider>().enabled = previousNote == null || !previousNote.activeInHierarchy;// && hit_cycle < 1;

    //                direction_type = Direction.None;
    //                transform.position = new Vector3(transform.position.x, transform.position.y, GameObject.Find("Judgement Line").transform.position.z + offset);
    //                StartCoroutine(CheckingPerfect_Hit());
    //                break;

    //            case 2:
    //                direction_type = Direction.None;
    //                transform.position = new Vector3(transform.position.x, transform.position.y, GameObject.Find("Judgement Line").transform.position.z + offset);
    //                Obstacle_NoteDodge(false);
    //                break;
    //        }
    //    }
    //    else
    //    {
    //        try { transform.Translate(Vector3.back * BeatConductor.thisBeat.get_BPM_Calcuate * note_speed * Time.deltaTime); }
    //        catch { transform.Translate(Vector3.back * note_speed * Time.deltaTime); }
    //    }
    //}
    #endregion
}
