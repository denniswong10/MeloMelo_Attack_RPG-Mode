﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score121 : MonoBehaviour
{
    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 1, 0, 0, 0, 1, 0,

        0, 0, 2, 0, 0, 0, 1, 0, 0, 1, 0, 0, 2, 0, 0, 1, 0, 0, 1, 0, 0, 4, 0, 0, 2, 0,

        0, 1, 0, 0, 4, 0, 0, 1, 0, 0, 1, 0, 0, 2, 0, 0, 0, 2, 0, 0, 5, 0, 0, 5, 0, 0,

        1, 0, 0, 0, 2, 0, 0, 0, 4, 0, 0, 0, 1, 0, 0, 1, 0, 0, 5, 0, 0, 2, 0, 0, 2, 0,

        0, 1, 0, 0, 0, 5, 0, 0, 0, 5, 0, 0, 0, 1, 0, 0, 0, 5, 0, 0, 5, 0, 0, 1, 0, 0,

        2, 0, 0, 2, 0, 0, 5, 0, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 1, 0, 0, 0, 5, 0, 0,

        0, 1, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 5,

        0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 4, 0, 0, 2, 0, 0, 1,

        1, 5, 0, 6, 0, 1, 0, 2, 2, 1, 0, 5, 0, 1, 6, 1, 0, 2, 0, 4, 0, 2, 0, 0, 6, 0,

        1, 2, 1, 0, 5, 0, 1, 1, 2, 0, 5, 0, 1, 1, 2, 0, 6, 0, 1, 1, 5, 0, 0, 3, 0, 0,

        1, 0, 1, 0, 2, 0, 1, 0, 1, 0, 4, 0, 2, 1, 1, 0, 1, 1, 5, 0, 0, 0, 3, 0, 0, 2,

        0, 0, 5, 0, 0, 1, 1, 2, 0, 6, 0, 1, 0, 1, 2, 2, 0, 1, 1, 5, 0, 6, 0, 1, 0, 6,

        5, 0, 1, 1, 2, 0, 1, 0, 5, 0, 0, 3, 0, 1, 1, 2, 0, 3, 0, 1, 1, 5, 0, 6, 0, 2,

        0, 2, 0, 1, 1, 2, 0, 3, 0, 1, 2, 1, 0, 5, 0, 3, 0, 1, 2, 1, 0, 2, 0, 2, 1, 2,

        0, 3, 0, 1, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Small", score_database2, "Medium");
    }
}