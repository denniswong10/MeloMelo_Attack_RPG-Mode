﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score134 : MonoBehaviour
{
    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 2, 0, 0, 4, 0, 0, 2, 0, 0, 2, 0, 0, 0, 1, 0,

        0, 0, 5, 0, 0, 1, 0, 0, 2, 0, 0, 4, 0, 0, 2, 0, 0, 1, 0, 0, 1, 0, 0, 5, 0, 0,

        2, 0, 0, 3, 0, 0, 0, 3, 0, 0, 0, 1, 0, 1, 0, 2, 0, 1, 0, 0, 5, 0, 0, 2, 0, 0,

        4, 0, 0, 2, 0, 0, 1, 0, 0, 1, 0, 2, 0, 0, 5, 0, 0, 0, 4, 0, 0, 0, 2, 0, 0, 0,

        4, 0, 0, 0, 1, 0, 2, 0, 5, 0, 1, 0, 2, 0, 5, 0, 2, 0, 0, 6, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,

        0, 0, 3, 0, 0, 2, 0, 0, 7, 0, 0, 2, 0, 0, 3, 0, 0, 5, 0, 0, 7, 0, 0, 6, 0, 0,

        5, 0, 0, 3, 0, 1, 0, 7, 0, 5, 0, 3, 0, 2, 0, 7, 0, 6, 0, 2, 0, 3, 0, 2, 0, 4,

        0, 0, 5, 0, 0, 5, 0, 0, 6, 0, 0, 0, 7, 0, 0, 3, 0, 5, 0, 3, 0, 1, 0, 6, 0, 5,

        0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 2, 0, 4, 0, 5, 0, 1, 1, 5, 0, 6, 0, 2, 0, 1, 1,

        2, 5, 0, 1, 0, 1, 0, 2, 0, 4, 0, 1, 0, 1, 0, 5, 0, 3, 1, 7, 2, 0, 7, 5, 3, 2,

        0, 2, 0, 3, 2, 0, 0, 3, 4, 0, 0, 1, 5, 1, 0, 2, 1, 2, 0, 7, 6, 0, 3, 2, 0, 1,

        1, 5, 0, 2, 0, 4, 0, 1, 1, 2, 0, 5, 0, 4, 1, 5, 0, 0, 2, 0, 0, 0, 3, 2, 7, 0,

        4, 0, 2, 0, 4, 3, 5, 0, 4, 0, 6, 0, 1, 0, 5, 0, 6, 0, 1, 2, 0, 0, 0, 2, 0, 0,

        0, 2, 0, 0, 0, 3, 0, 0, 0, 2, 0, 0, 0, 2, 0, 0, 0, 3, 0, 0, 0, 2, 0, 0, 0, 1,

        1, 5, 0, 3, 2, 5, 0, 7, 2, 1, 0, 1, 1, 5, 0, 3, 0, 5, 0, 1, 3, 5, 0, 7, 0, 6,

        1, 5, 0, 3, 1, 7, 2, 0, 4, 2, 4, 2, 0, 7, 6, 5, 3, 0, 2, 2, 0, 3, 2, 0, 1, 4,

        1, 0, 5, 3, 0, 5, 6, 0, 6, 7, 0, 2, 2, 2, 0, 1, 0, 5, 0, 2, 1, 3, 0, 1, 2, 5,

        0, 0, 0, 0, 0, 0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Small", score_database2, "Medium");
    }
}