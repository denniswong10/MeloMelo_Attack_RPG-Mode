using System.Collections;
using UnityEngine;
using MeloMelo_PlayerManagement;

public class Notation_Motion_Script : MonoBehaviour
{
    private Note_Script mainScript;
    private bool isMotionSmoothen;

    private float note_speed;
    private Vector3 destinationDesired;
    private GameObject previousNote;

    private enum Direction { Forward, Backward, None };
    private Direction direction_type = Direction.Forward;

    private bool isReadyToRoll;
    private bool isRollCompleted;

    private Vector3 startPos;
    private float zOffset;
    private float zTarget;
    private float elapsedAudioTime;
    private double audioDspTime;
    private float duration;
    private int storeTargetId;

    // Start is called before the first frame update
    void Start()
    {
        mainScript = GetComponent<Note_Script>();
        isReadyToRoll = false;
        isRollCompleted = false;

        // Interact this object
        ReadyForInteract();

        // Set note position from start
        ReadyToRollOut();

        // Begin rolling of notation
        StartActionRolling();
    }

    void Update()
    {
        if (!mainScript.isNotationHit || direction_type != Direction.Backward) return;
        Perform_Reflect_Movement();
    }

    #region SETUP
    private void StartActionRolling()
    {
        if (direction_type == Direction.Backward) return;
        Perform_Forward_Movement();
    }

    private void ReadyForInteract()
    {
        switch (mainScript.note_define_index)
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
        destinationDesired = GameObject.Find("Judgement Line").transform.position;
        transform.position = new Vector3(transform.position.x, mainScript.get_NotePos, transform.position.z);
        note_speed = BeatConductor.thisBeat.get_noteSpeed;
    }
    #endregion

    #region MAIN
    public void UniversalMotionTracker()
    {
        if (BeatConductor.thisBeat.Music_Database.NewChartSystem) NewChartMotionTracker();
        else LegacyMotionTracker();
    }

    public void Reflect_MoveEffect() { direction_type = Direction.Backward; }

    public void UpdateMotionBehaviour(bool option)
    {
        isMotionSmoothen = option;
    }

    public void SetNoteHitOnPrevious(GameObject note)
    {
        previousNote = note;
    }

    public void CheckLogicOnMiss()
    {
        if (direction_type == Direction.None) Destroy(gameObject);
    }

    private void Perform_Forward_Movement()
    {
        switch (mainScript.note_define_index)
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

    private void ObstacleEnd_Check(float offset)
    {
        if (BeatConductor.thisBeat.Music_Database.NewChartSystem) NewChartJudgeTracker(offset, 1);
        else LegacyJudgeTracker(offset, 1);
    }

    private void SpecialNote_Check(float offset)
    {
        if (BeatConductor.thisBeat.Music_Database.NewChartSystem) NewChartJudgeTracker(offset, 2);
        else LegacyJudgeTracker(offset, 2);
    }
    #endregion

    #region COMPONENT
    private void LegacyJudgeTracker(float offset, int index)
    {
        startPos = transform.position;
        duration = BeatConductor.thisBeat.get_BPM_Calcuate *
            MeloMelo_GameSettings.GetLegacyIntitSpeed(BeatConductor.thisBeat.Music_Database.BPM);

        audioDspTime = AudioSettings.dspTime;

        zOffset = offset;
        storeTargetId = index;
        isReadyToRoll = true;
    }

    private void LegacyMotionTracker()
    {
        if (isReadyToRoll)
        {
            if (!isRollCompleted)
            {
                zTarget = zOffset;

                double currentDSPTime = AudioSettings.dspTime;
                float t = (float)((currentDSPTime - audioDspTime) / duration);
                t = Mathf.Clamp01(t); // Clamp to prevent overshooting

                // Smoother interpolation using easing (ease-in-out)
                float easedT = Mathf.SmoothStep(0f, 1f, t);

                float z = Mathf.Lerp(startPos.z, destinationDesired.z + zTarget, isMotionSmoothen ? easedT : t);
                transform.position = new Vector3(transform.position.x, transform.position.y, z);

                if (t >= 1f)
                {
                    // Snap to final Z position to correct any floating-point drift
                    transform.position = new Vector3(transform.position.x, transform.position.y, destinationDesired.z + zOffset);
                    ProcessJudgeThroughInput(storeTargetId);
                }
            }
        }
    }

    private void NewChartJudgeTracker(float offset, int index)
    {
        startPos = transform.position;
        zTarget = destinationDesired.z + offset;

        float travelDistance = Mathf.Abs(startPos.z - zTarget);
        float finalSpeed = Mathf.Max(note_speed * BeatConductor.thisBeat.get_BPM_Calcuate, 0.001f);
        duration = travelDistance / finalSpeed;

        elapsedAudioTime = 0f;
        storeTargetId = index;
        isReadyToRoll = true;
    }

    private void NewChartMotionTracker()
    {
        // Only update elapsed time if audio is playing
        if (isReadyToRoll) // BeatConductor.thisBeat.GetComponent<AudioSource>().isPlaying
        {
            if (elapsedAudioTime < duration && !isRollCompleted)
            {
                elapsedAudioTime += Time.deltaTime;

                float t = Mathf.Clamp01(elapsedAudioTime / duration);

                // Optional easing
                float easedT = Mathf.SmoothStep(0f, 1f, t);
                float z = Mathf.Lerp(startPos.z, zTarget, isMotionSmoothen ? easedT : t);
                transform.position = new Vector3(transform.position.x, transform.position.y, z);
            }

            else if (!isRollCompleted)
            {
                // Snap to exact position
                transform.position = new Vector3(transform.position.x, transform.position.y, zTarget);
                ProcessJudgeThroughInput(storeTargetId);
            }
        }
    }

    private void ProcessJudgeThroughInput(int index)
    {
        isRollCompleted = true;

        switch (index)
        {
            case 1:
                GetComponent<BoxCollider>().enabled = previousNote == null || !previousNote.activeInHierarchy;
                direction_type = Direction.None;
                mainScript.BeginNotationTimeOut();
                break;

            case 2:
                direction_type = Direction.None;
                mainScript.Obstacle_NoteDodge(false);
                break;
        }
    }
    #endregion

    #region NOT IN USE
    //private IEnumerator LegacyJudgeTracker(float offset, int index)
    //{
    //    Vector3 startPos = transform.position;
    //    float duration = BeatConductor.thisBeat.get_BPM_Calcuate *
    //        MeloMelo_GameSettings.GetLegacyIntitSpeed(BeatConductor.thisBeat.Music_Database.BPM);

    //    double startDSPTime = AudioSettings.dspTime;

    //    float zOffset = offset;

    //    while (true)
    //    {
    //        double currentDSPTime = AudioSettings.dspTime;
    //        float t = (float)((currentDSPTime - startDSPTime) / duration);
    //        t = Mathf.Clamp01(t); // Clamp to prevent overshooting

    //        // Smoother interpolation using easing (ease-in-out)
    //        float easedT = Mathf.SmoothStep(0f, 1f, t);

    //        float z = Mathf.Lerp(startPos.z, destinationDesired.z + zOffset, isMotionSmoothen ? easedT : t);
    //        transform.position = new Vector3(transform.position.x, transform.position.y, z);

    //        if (t >= 1f)
    //            break;

    //        yield return null;
    //    }

    //    // Snap to final Z position to correct any floating-point drift
    //    transform.position = new Vector3(transform.position.x, transform.position.y, destinationDesired.z + offset);
    //    ProcessJudgeThroughInput(index);
    //}

    //private IEnumerator NewChartJudgeTracker(float offset, int index)
    //{
    //    Vector3 startPos = transform.position;
    //    float zTarget = destinationDesired.z + offset;

    //    float travelDistance = Mathf.Abs(startPos.z - zTarget);
    //    float finalSpeed = Mathf.Max(note_speed * BeatConductor.thisBeat.get_BPM_Calcuate, 0.001f);
    //    float duration = travelDistance / finalSpeed;

    //    float elapsedAudioTime = 0f;

    //    while (elapsedAudioTime < duration)
    //    {
    //        // Only update elapsed time if audio is playing
    //        if (BeatConductor.thisBeat.GetComponent<AudioSource>().isPlaying)
    //        {
    //            elapsedAudioTime += Time.deltaTime;

    //            float t = Mathf.Clamp01(elapsedAudioTime / duration);

    //            // Optional easing
    //            float easedT = Mathf.SmoothStep(0f, 1f, t);
    //            float z = Mathf.Lerp(startPos.z, zTarget, isMotionSmoothen ? easedT : t);
    //            transform.position = new Vector3(transform.position.x, transform.position.y, z);
    //        }

    //        yield return null;
    //    }

    //    // Snap to exact position
    //    transform.position = new Vector3(transform.position.x, transform.position.y, zTarget);
    //    ProcessJudgeThroughInput(index);
    //}
    #endregion
}
