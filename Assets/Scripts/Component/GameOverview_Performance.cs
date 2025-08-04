using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverview_Performance : MonoBehaviour
{
    [SerializeField] private bool isOptimizeAllow;
    [SerializeField] private int maxFrameLimit;
    [SerializeField] private bool GamePlayPerfromance;

    private int[] maxrateData = { 60, 120, 240 };

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = isOptimizeAllow ? maxFrameLimit :
            maxrateData[PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetFrameRateLimit_ValueKey)];

        QualitySettings.vSyncCount = isOptimizeAllow ? 1 : 0;

        if (GamePlayPerfromance) Debug.Log("Game Current Frame: " + Application.targetFrameRate);
    }
}
