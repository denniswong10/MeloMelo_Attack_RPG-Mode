﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score60 : MonoBehaviour
{
    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 6, 0, 0, 0, 6, 0, 0, 0, 6,

        0, 0, 0, 5, 0, 0, 0, 5, 0, 0, 0, 0, 1, 0, 0, 6, 0, 0, 5, 0, 0, 1, 0, 0, 4, 0,

        0, 5, 0, 0, 1, 0, 0, 1, 0, 0, 5, 0, 0, 1, 0, 4, 0, 6, 0, 0, 0, 2, 0, 0, 0, 2,

        0, 0, 0, 2, 0, 0, 0, 1, 0, 0, 5, 0, 0, 2, 0, 0, 1, 0, 5, 0, 0, 6, 0, 1, 0, 2, 

        0, 0, 6, 0, 1, 0, 2, 0, 6, 0, 0, 2, 0, 0, 1, 0, 0, 5, 0, 0, 1, 0, 0, 5, 0, 0,

        2, 0, 0, 6, 0, 4, 0, 2, 0, 0, 5, 0, 1, 0, 2, 0, 0, 0, 1, 0, 0, 0, 5, 0, 0, 0,

        6, 0, 0, 2, 0, 0, 2, 0, 0, 4, 0, 0, 2, 0, 0, 5, 0, 6, 0, 2, 0, 7, 0, 0, 1, 1,

        0, 6, 0, 1, 0, 0, 2, 0, 0, 5, 0, 1, 0, 0, 2, 0, 0, 0, 1, 0, 0, 6, 0, 0, 5, 0,

        0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 0, 0, 0, 0, 1, 2, 6, 0, 1, 4, 5, 0, 1, 1, 6, 0, 1, 1, 4, 0, 6, 2, 1, 0, 6,

        0, 2, 2, 5, 0, 3, 0, 5, 0, 0, 0, 0, 1, 0, 5, 6, 1, 0, 5, 0, 2, 1, 5, 0, 4, 0,

        1, 5, 6, 0, 1, 0, 2, 1, 0, 4, 5, 6, 0, 2, 0, 5, 0, 6, 0, 1, 1, 2, 0, 4, 0, 2,

        2, 1, 0, 2, 0, 6, 1, 1, 0, 2, 5, 0, 3, 2, 0, 1, 1, 0, 5, 0, 0, 6, 1, 6, 0, 2,

        3, 0, 6, 5, 6, 0, 2, 0, 6, 3, 2, 2, 0, 1, 0, 1, 0, 5, 2, 2, 1, 0, 5, 0, 5, 0,

        2, 0, 0, 6, 1, 4, 0, 2, 3, 0, 5, 6, 1, 0, 2, 3, 0, 0, 1, 1, 2, 0, 5, 1, 2, 0,

        6, 0, 1, 2, 0, 1, 2, 0, 5, 6, 0, 2, 0, 0, 0, 5, 5, 6, 0, 2, 2, 5, 0, 0, 1, 6,

        0, 1, 0, 2, 2, 4, 0, 6, 0, 2, 2, 4, 0, 5, 0, 1, 1, 0, 1, 6, 0, 1, 1, 0, 3, 0,

        0, 3, 5, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Small", score_database2, "Medium");
    }
}
