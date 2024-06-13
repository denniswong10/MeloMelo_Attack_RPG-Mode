using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counter_Script : MonoBehaviour
{
    private float NextFrame = 0;
    private float timer;

    // Update is called once per frame
    void Update()
    {
        if (Time.unscaledTime > timer)
        {
            int fps = (int)(1f / Time.deltaTime);

            if (Time.time >= NextFrame)
            {
                NextFrame = Time.time + 1.5f;
                GetComponent<Text>().text = "FPS: " + (fps >= 120 ? 120 : fps);
            }
        }
    }
}
