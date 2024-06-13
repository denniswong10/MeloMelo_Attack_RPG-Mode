using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MeloMelo_ExtraComponent;
using MeloMelo_PlayerManagement;

public class Note_Script : MonoBehaviour
{
    private float note_speed;
    private bool isHit = false;

    public int note_index;
    public CharacterSettings.PICKUP_TYPE note_define_index;

    [SerializeField] private float NotePos;
    public int hit_cycle = 0;

    private enum Direction { Forward, Backward, None };
    private Direction direction_type = Direction.Forward;

    private int preJudgedNote = -1;
    public int get_preJudgeNote { get { return preJudgedNote; } }

    private int preOffBeat = -1;
    public int get_preOffBeat { get { return preOffBeat; } }

    private ScoreFixedValue scoreF = new ScoreFixedValue();
    public ParticleSystem judgeline;

    void Start()
    {
        // Interact this object
        ReadyForInteract();

        // Set note position from start
        ReadyToRollOut();

        // Check For Option
        ChannelOutGuideAnimator();
    }

    void Update()
    {
        StartActionRolling();
    }

    #region SETUP (Basic)
    private void ReadyForInteract()
    {
        switch (note_define_index)
        {
            case CharacterSettings.PICKUP_TYPE.NONE:
            case CharacterSettings.PICKUP_TYPE.JUMP:
            case CharacterSettings.PICKUP_TYPE.ITEM:
                GetComponent<BoxCollider>().enabled = false;
                break;

            default:
                break;
        }
    }

    private void ReadyToRollOut()
    {
        transform.position = new Vector3(transform.position.x, NotePos, transform.position.z);
        note_speed = BeatConductor.thisBeat.get_noteSpeed;
    }
    #endregion

    #region SETUP (Advance)
    private void StartActionRolling()
    {
        if (direction_type == Direction.Forward) { Perform_Forward_Movement(); }
        else if (direction_type == Direction.Backward) { Perform_Reflect_Movement(); }
    }

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

    #region COMPONENT (MAIN: CHECKER)
    private void ObstacleEnd_Check(float offset)
    {
        if (transform.position.z <= GameObject.Find("Judgement Line").transform.position.z + offset)
        {
            GetComponent<BoxCollider>().enabled = true;

            direction_type = Direction.None;
            transform.position = new Vector3(transform.position.x, transform.position.y, GameObject.Find("Judgement Line").transform.position.z + offset);
            StartCoroutine(CheckingPerfect_Hit());
        }
        else
        {
            try { transform.Translate(Vector3.back * BeatConductor.thisBeat.get_BPM_Calcuate * note_speed * Time.deltaTime); }
            catch { transform.Translate(Vector3.back * note_speed * Time.deltaTime); }
        }
    }

    private void SpecialNote_Check(float offset)
    {
        if (transform.position.z <= GameObject.Find("Judgement Line").transform.position.z + offset)
        {
            GetComponent<BoxCollider>().enabled = false;

            direction_type = Direction.None;
            transform.position = new Vector3(transform.position.x, transform.position.y, GameObject.Find("Judgement Line").transform.position.z + offset);
            Obstacle_NoteDodge(false);
        }
        else
        {
            try { transform.Translate(Vector3.back * BeatConductor.thisBeat.get_BPM_Calcuate * note_speed * Time.deltaTime); }
            catch { transform.Translate(Vector3.back * note_speed * Time.deltaTime); }
        }
    }
    #endregion

    #region MAIN
    // Note Function: Reserve Mode
    public void Reflect_MoveEffect() { direction_type = Direction.Backward; }

    public void JudgeLineToggle() { if (judgeline) judgeline.Stop(); }

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
    private void Perform_Forward_Movement()
    {
        switch (note_define_index)
        {
            case CharacterSettings.PICKUP_TYPE.TRAP:
                ObstacleEnd_Check(-0.5f);
                break;

            case CharacterSettings.PICKUP_TYPE.ITEM3:
                SpecialNote_Check(-0.2f);
                break;

            default:
                ObstacleEnd_Check(0.2f);
                break;
        }
    }

    private void Perform_Reflect_Movement()
    {
        if (transform.position.z >= 1)
            Destroy(gameObject);

        else
            transform.Translate(Vector3.forward * BeatConductor.thisBeat.get_BPM_Calcuate * note_speed * Time.deltaTime);
    }

    private void Obstacle_NoteDodge(bool audio)
    {
        try
        {
            GameManager.thisManager.UpdateNoteStatus("Perfect_2");
            GameManager.thisManager.ModifyFastNLateJudge(1, 1);

            if (!GameManager.thisManager.DeveloperMode && audio) { AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Audio/SE/EnemyAttack"), new Vector3(0, 0, -10f)); }

            if (PlayerPrefs.GetInt("MissCP", 0) > 0) GameManager.thisManager.UpdateScore(BeatConductor.thisBeat.get_scorePerfect2 + scoreF.score_combo());
            else GameManager.thisManager.UpdateScore(BeatConductor.thisBeat.get_scorePerfect2);
        }
        catch
        {
            TutorialManager.thisManager.UpdateNoteStatus("Perfect_2");
            AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Audio/SE/EnemyAttack"), new Vector3(0, 0, -10f));
        }

        ClearOfCompletedCycle();
    }
    #endregion

    #region COMPONENT (TIMING WINDOW)
    IEnumerator CheckingPerfect_Hit()
    {
        float time_beforePerfect = 2 * (BeatConductor.thisBeat.get_BPM_Calcuate / 4);
        yield return new WaitForSeconds(time_beforePerfect);

        if (note_define_index == CharacterSettings.PICKUP_TYPE.TRAP)
        {
            // Traps (Ground) Traps (Air)
            Obstacle_NoteDodge(true);
        }

        StartCoroutine(CheckTiming_NoteHit());
    }

    // Note Check: Hit Timing
    IEnumerator CheckTiming_NoteHit()
    {
        float time_beforeMiss = 2 * (BeatConductor.thisBeat.get_BPM_Calcuate / 4);
        yield return new WaitForSeconds(time_beforeMiss);

        StartCoroutine(CheckTiming_AboutMiss_NoteHit());
    }

    IEnumerator CheckTiming_AboutMiss_NoteHit()
    {
        float time_finalelsape = BeatConductor.thisBeat.get_BPM_Calcuate;
        yield return new WaitForSeconds(time_finalelsape);

        if (!isHit)
        {
            GetComponent<BoxCollider>().enabled = false;

            switch (note_define_index)
            {
                case CharacterSettings.PICKUP_TYPE.NONE:
                    RegularNote_HitCheck();
                    break;

                default:
                    if (preJudgedNote < 0) GameManager.thisManager.UpdateNoteStatus("Miss");
                    MeloMelo_ScoreSystem.thisSystem.UpdateScoreDisplay();
                    break;
            }
        }

        if (direction_type == Direction.None) Destroy(gameObject);
    }

    private void RegularNote_HitCheck()
    {
        if (direction_type != Direction.Backward)
        {
            if (hit_cycle != 0)
            {
                hit_cycle--;
                GetComponent<BoxCollider>().enabled = true;
                StartCoroutine(CheckingPerfect_Hit());
            }
            else
            {
                if (!isHit)
                {
                    GameManager.thisManager.UpdateNoteStatus("Miss");
                    MeloMelo_ScoreSystem.thisSystem.UpdateScoreDisplay();

                    if (GameObject.Find("Character").GetComponent<Character>().stats.get_name != "NA")
                        GameManager.thisManager.UpdateCharacter_Health(-(PlayerPrefs.GetInt("Enemy_OverallDamage", 0) * 2), false);
                }
            }
        }
    }
    #endregion

    #region COMPONENT (GUIDELINE ANIMTAOR)
    private void ChannelOutGuideAnimator()
    {
        if (PlayerPrefs.GetInt("AirGuide_valve", 0) == 0 && ChannelAnimatorValve())
            if (judgeline != null) judgeline.Play();
    }

    private bool ChannelAnimatorValve()
    {
        if (note_define_index == CharacterSettings.PICKUP_TYPE.JUMP)
        {
            return true;
        }

        else if (note_define_index == CharacterSettings.PICKUP_TYPE.NONE)
        {
            switch (note_index)
            {
                case 2:
                    return true;

                default:
                    return false;
            }
        }

        return false;
    }
    #endregion
}
