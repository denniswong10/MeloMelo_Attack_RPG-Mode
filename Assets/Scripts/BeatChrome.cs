using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatChrome : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float bpm = 120f; // beats per minute
    public AnimationCurve rhythmCurve; // Optional curve to shape movement

    private float beatDuration;
    private float timer;
    private bool movingToB = true;

    void Start()
    {
        beatDuration = 60f / bpm; // Convert BPM to seconds per beat
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        float t = timer / beatDuration;
        t = Mathf.Clamp01(t);

        // Optional: apply a rhythm curve to make it feel less linear
        if (rhythmCurve != null)
            t = rhythmCurve.Evaluate(t);

        if (movingToB)
            transform.position = Vector3.Lerp(pointA.position, pointB.position, t);
        else
            transform.position = Vector3.Lerp(pointB.position, pointA.position, t);

        // On beat, reset timer and switch direction
        if (timer >= beatDuration)
        {
            timer = 0f;
            movingToB = !movingToB;
        }
    }
}
