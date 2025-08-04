using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS_Counter : MonoBehaviour
{
    public int avgFrameRate;
    private float NextFrame = 0;
    public Texture[] fps_stats = new Texture[4];

    Texture Update_FPS(int status) { return fps_stats[status - 1]; }

    void CheckForNextFrame()
    {
        if (Time.time >= NextFrame)
        {
            if (avgFrameRate >= 40) { GetComponent<RawImage>().texture = Update_FPS(1); }
            else if (avgFrameRate >= 20) { GetComponent<RawImage>().texture = Update_FPS(2); }
            else if (avgFrameRate >= 1) { GetComponent<RawImage>().texture = Update_FPS(3); }
            else if (avgFrameRate <= 0) { GetComponent<RawImage>().texture = Update_FPS(4); }

            NextFrame = Time.time + 3;
        }
    }

    void Start()
    {
        bool isFpsOpen = QualitySettings.vSyncCount == 0;
        gameObject.SetActive(isFpsOpen);
    }

    void Update()
    {
        float current = 0;
        current = (int)(1f / Time.unscaledDeltaTime);
        avgFrameRate = (int)current;

        CheckForNextFrame();
    }
}
