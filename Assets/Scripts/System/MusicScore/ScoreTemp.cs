using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class ScoreTemp : MonoBehaviour
{
    public int difficulty;

    private int[] score_database = new int[1000];

    private int[] score_database2 = new int[1000];

    void Start()
    {
        difficulty = 1;// PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Small", score_database2, "Medium");
    }
}
