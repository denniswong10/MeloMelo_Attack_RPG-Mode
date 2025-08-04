using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteTick_Script : MonoBehaviour
{
    private float note_speed = 0;
    private bool isMarginActive = false;

    private Vector3 startPos, endPos;
    private float marginOffset;
    private float marginInitOffset;
    private float marginTravelLength;

    private double dspTime;
    private float dspTimeElpase;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<ParticleSystem>().Play();
        note_speed = BeatConductor.thisBeat.get_noteSpeed;

        // Begin tick movement
       ObstacleEnd_Check(0.1f);
    }

    private void ObstacleEnd_Check(float offset)
    {
        if (BeatConductor.thisBeat.Music_Database.NewChartSystem) NewChartMarginTracker(offset);
        else LegacyMarginTracker(offset);
    }

    #region MAIN
    public void UniservalMarginTracker()
    {
        if (BeatConductor.thisBeat.Music_Database.NewChartSystem) NewChartMarginMotionTracker();
        else LegacyMarginMotionTracker();
    }
    #endregion

    #region COMPONENT
    private void LegacyMarginTracker(float offset)
    {
        startPos = GameObject.Find("SpawnPoint").transform.position;
        endPos = GameObject.Find("Judgement Line").transform.position;

        marginTravelLength = BeatConductor.thisBeat.get_BPM_Calcuate *
            MeloMelo_GameSettings.GetLegacyIntitSpeed(BeatConductor.thisBeat.Music_Database.BPM);

        dspTime = AudioSettings.dspTime;
        marginOffset = offset;
        isMarginActive = true;
    }

    private void LegacyMarginMotionTracker()
    {
        if (isMarginActive)
        {
            marginInitOffset = marginOffset;
         
            double currentDSPTime = AudioSettings.dspTime;
            float t = (float)((currentDSPTime - dspTime) / marginTravelLength);
            t = Mathf.Clamp01(t); // Clamp to prevent overshooting

            float z = Mathf.Lerp(startPos.z, endPos.z + marginOffset, t);
            transform.position = new Vector3(transform.position.x, transform.position.y, z);

            if (t >= 1f)
            {
                // Snap to final Z position to correct any floating-point drift
                transform.position = new Vector3(transform.position.x, transform.position.y, endPos.z + marginInitOffset);
                Destroy(gameObject);
            }
        }
    }

    private void NewChartMarginTracker(float offset)
    {
        startPos = GameObject.Find("SpawnPoint").transform.position;
        endPos = GameObject.Find("Judgement Line").transform.position;
        marginInitOffset = endPos.z + offset;

        float travelDistance = Mathf.Abs(startPos.z - marginInitOffset);
        marginTravelLength = travelDistance / (note_speed * BeatConductor.thisBeat.get_BPM_Calcuate);
        dspTimeElpase = 0f;
        isMarginActive = true;
    }

    private void NewChartMarginMotionTracker()
    {
        if (isMarginActive)
        {
            if (dspTimeElpase < marginTravelLength)
            {
                // Only update elapsed time if audio is playing
                dspTimeElpase += Time.deltaTime;
                float t = Mathf.Clamp01(dspTimeElpase / marginTravelLength);

                float z = Mathf.Lerp(startPos.z, marginInitOffset, t);
                transform.position = new Vector3(transform.position.x, transform.position.y, z);
            }
            else
            {
                // Snap to target to avoid drift
                transform.position = endPos;
                Destroy(gameObject);
            }
        }
    }
    #endregion

    #region NOT IN USE
    //private IEnumerator LegacyJudgeTracker(float offset)
    //{
    //    Vector3 startPos = GameObject.Find("SpawnPoint").transform.position;
    //    Vector3 endPos = GameObject.Find("Judgement Line").transform.position;

    //    float duration = BeatConductor.thisBeat.get_BPM_Calcuate * 
    //        MeloMelo_GameSettings.GetLegacyIntitSpeed(BeatConductor.thisBeat.Music_Database.BPM);

    //    double startDSPTime = AudioSettings.dspTime;

    //    float zOffset = offset;

    //    while (true)
    //    {
    //        double currentDSPTime = AudioSettings.dspTime;
    //        float t = (float)((currentDSPTime - startDSPTime) / duration);
    //        t = Mathf.Clamp01(t); // Clamp to prevent overshooting

    //        float z = Mathf.Lerp(startPos.z, endPos.z + zOffset, t);
    //        transform.position = new Vector3(transform.position.x, transform.position.y, z);

    //        if (t >= 1f)
    //            break;

    //        yield return null;
    //    }

    //    // Snap to final Z position to correct any floating-point drift
    //    transform.position = new Vector3(transform.position.x, transform.position.y, endPos.z + offset);
    //    Destroy(gameObject);
    //}

    //private IEnumerator NewChartJudgeTracker(float offset)
    //{
    //    Transform judgeLine = GameObject.Find("Judgement Line").transform;
    //    float zTarget = judgeLine.position.z + offset;

    //    while (transform.position.z > zTarget)
    //    {
    //        float moveStep = note_speed * Time.deltaTime;
    //        float bpmFactor = BeatConductor.thisBeat.get_BPM_Calcuate;

    //        transform.Translate(Vector3.back * moveStep * bpmFactor);
    //        yield return null;
    //    }

    //    // Reached Judgement Line
    //    transform.position = new Vector3(transform.position.x, transform.position.y, zTarget);
    //    Destroy(gameObject);
    //}

    //private IEnumerator NewChartJudgeTracker(float offset)
    //{
    //    Vector3 startPos = GameObject.Find("SpawnPoint").transform.position;
    //    Vector3 endPos = GameObject.Find("Judgement Line").transform.position;
    //    float zTarget = endPos.z + offset;

    //    float travelDistance = Mathf.Abs(startPos.z - zTarget);
    //    float duration = travelDistance / (note_speed * BeatConductor.thisBeat.get_BPM_Calcuate);
    //    float elapsedAudioTime = 0f;

    //    while (elapsedAudioTime < duration)
    //    {
    //        // Only update elapsed time if audio is playing
    //        if (BeatConductor.thisBeat.GetComponent<AudioSource>().isPlaying)
    //        {
    //            elapsedAudioTime += Time.deltaTime;
    //            float t = Mathf.Clamp01(elapsedAudioTime / duration);

    //            float z = Mathf.Lerp(startPos.z, zTarget, t);
    //            transform.position = new Vector3(transform.position.x, transform.position.y, z);
    //        }
    //        else if (PlayerPrefs.GetInt("TrackCompleted", 0) == 1) 
    //            Destroy(gameObject);

    //        yield return null;
    //    }

    //    // Snap to target to avoid drift
    //    transform.position = endPos;
    //    Destroy(gameObject);
    //}

    //private void OldJudgementLine(float offset)
    //{
    //    if (transform.position.z <= GameObject.Find("Judgement Line").transform.position.z + offset)
    //    {
    //        transform.position = new Vector3(transform.position.x, transform.position.y, GameObject.Find("Judgement Line").transform.position.z + offset);
    //        Destroy(gameObject);
    //    }
    //    else
    //    {
    //        try { transform.Translate(Vector3.back * BeatConductor.thisBeat.get_BPM_Calcuate * note_speed * Time.deltaTime); }
    //        catch { transform.Translate(Vector3.back * note_speed * Time.deltaTime); }
    //    }
    //}
    #endregion
}
