﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score150 : MonoBehaviour
{
    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 2, 0, 0, 0, 2, 0, 0, 0, 4, 0, 0, 0, 2, 0, 0, 0, 2, 0, 0, 0, 4, 0, 0, 0, 1,

        0, 0, 0, 5, 0, 0, 0, 1, 0, 2, 0, 5, 0, 1, 0, 2, 0, 5, 0, 1, 0, 0, 2, 0, 1, 1,

        0, 2, 2, 0, 1, 1, 0, 5, 0, 0, 0, 4, 0, 0, 0, 2, 0, 0, 0, 4, 0, 0, 0, 2, 0, 0, // 3

        0, 5, 0, 1, 0, 2, 0, 4, 0, 2, 0, 4, 0, 5, 0, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 5, 0, 0, 2, 2, 0, 4, 0, 2, 2, 0, 6, 0, 1, 0, 5, 0, 0, 0, 1, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 2, 0, 6, 0, 0, 1, 1, 0, 5, 0, 1, 1, 0, 4, 0, 1, 0, 2, 0, 1, // 6

        0, 4, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 5,

        0, 0, 0, 0, 0, 2, 0, 2, 0, 4, 0, 1, 0, 1, 0, 5, 0, 1, 0, 2, 0, 4, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 5, // 9

        0, 0, 2, 0, 0, 2, 0, 0, 4, 0, 0, 1, 0, 1, 0, 2, 0, 2, 0, 1, 0, 2, 0, 5, 0, 0,

        0, 6, 0, 0, 1, 0, 0, 6, 0, 0, 5, 0, 0, 1, 0, 0, 5, 0, 0, 6, 0, 0, 2, 2, 0, 4,

        0, 1, 0, 5, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, // 12

        0, 5, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 1, 0, 0, 0, 5, 0, 0, 4, 0, 0,

        1, 1, 0, 2, 2, 0, 4, 0, 2, 2, 0, 1, 0, 0, 0, 0, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 2, 0, 2, 0, 4, 0, 1, 0, 1, 0, 6, 0, 2, 0, 2, 0, 4, 0, 1, 0, 1, 0, 6, 0, 2,

        2, 0, 1, 5, 0, 0, 0, 7, 0, 2, 0, 7, 0, 5, 0, 7, 0, 2, 0, 7, 0, 5, 0, 7, 2, 4,

        0, 2, 1, 0, 4, 5, 0, 2, 0, 0, 0, 1, 1, 2, 0, 5, 0, 1, 1, 5, 0, 2, 0, 1, 1, 2, // 3

        0, 5, 0, 1, 0, 6, 0, 2, 0, 4, 0, 1, 2, 5, 0, 0, 0, 3, 0, 0, 0, 7, 0, 0, 0, 3,

        0, 1, 1, 5, 0, 0, 2, 4, 7, 0, 1, 1, 2, 0, 5, 0, 1, 2, 5, 0, 0, 0, 3, 2, 0, 0,

        0, 0, 0, 0, 1, 1, 2, 0, 7, 6, 5, 0, 1, 2, 5, 0, 1, 4, 0, 6, 0, 1, 1, 2, 0, 1, // 6

        1, 5, 0, 0, 0, 7, 0, 0, 1, 0, 2, 0, 5, 0, 0, 0, 0, 7, 0, 0, 1, 0, 2, 4, 0, 5,

        0, 5, 0, 0, 0, 1, 1, 2, 0, 4, 0, 1, 1, 5, 0, 2, 4, 2, 0, 1, 1, 6, 0, 0, 0, 0,

        0, 0, 0, 2, 0, 0, 0, 0, 0, 1, 0, 2, 0, 5, 0, 0, 0, 0, 0, 6, 7, 5, 0, 1, 2, 5, // 9

        0, 4, 0, 2, 0, 4, 0, 1, 1, 5, 0, 2, 0, 1, 1, 2, 0, 5, 0, 7, 2, 5, 0, 3, 1, 1,

        0, 4, 0, 7, 1, 6, 0, 3, 0, 7, 1, 5, 0, 3, 0, 1, 1, 0, 2, 2, 0, 1, 1, 2, 0, 5,

        0, 1, 1, 4, 0, 0, 0, 0, 0, 2, 0, 3, 0, 2, 0, 0, 0, 0, 0, 7, 0, 0, 0, 0, 0, 2, // 12

        0, 3, 0, 2, 0, 0, 0, 0, 1, 2, 7, 5, 0, 1, 0, 2, 3, 5, 1, 0, 1, 0, 5, 1, 4, 0,

        1, 2, 0, 2, 1, 0, 5, 1, 4, 1, 0, 5, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    // Start is called before the first frame update
    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Medium", score_database2, "Medium");
    }
}