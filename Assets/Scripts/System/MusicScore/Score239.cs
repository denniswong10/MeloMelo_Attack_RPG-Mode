﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score239 : MonoBehaviour
{
    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 1, 0, 0, 0, 1, // 3

        0, 0, 0, 8, 0, 0, 0, 1, 0, 0, 0, 4, 0, 0, 0, 2, 0, 0, 0, 1, 0, 0, 0, 4, 0, 0,

        0, 8, 0, 2, 0, 0, 5, 0, 0, 0, 0, 0, 0, 6, 0, 0, 2, 0, 0, 2, 0, 0, 0, 6, 0, 0,

        8, 0, 0, 8, 0, 0, 0, 1, 0, 0, 6, 0, 0, 2, 0, 0, 4, 0, 0, 2, 0, 0, 2, 0, 0, 5, // 6

        0, 0, 6, 0, 0, 8, 0, 8, 0, 0, 4, 0, 0, 8, 0, 0, 2, 0, 0, 9, 0, 0, 0, 0, 0, 0,

        1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 1, 0, 0, 2, 0, 8, 0, 0, 4, 0, 0, 1, 0, 0, // 9

        2, 0, 8, 0, 2, 0, 0, 6, 0, 1, 0, 1, 0, 5, 0, 0, 0, 9, 0, 0, 0, 5, 0, 0, 0, 0,

        0, 0, 6, 0, 0, 1, 0, 1, 0, 5, 0, 0, 0, 4, 0, 0, 2, 0, 8, 0, 5, 0, 0, 0, 1, 0,

        0, 4, 0, 0, 2, 0, 0, 6, 0, 0, 2, 0, 0, 8, 0, 8, 0, 0, 2, 0, 2, 0, 0, 1, 0, 0, // 12

        0, 5, 0, 0, 0, 0, 0, 0, 9, 0, 0, 1, 0, 6, 0, 5, 0, 0, 0, 4, 0, 0, 8, 0, 2, 0,

        5, 0, 0, 0, 6, 0, 0, 1, 0, 1, 0, 5, 0, 0, 6, 0, 0, 1, 0, 1, 0, 9, 0, 0, 8, 0,

        8, 0, 4, 0, 0, 8, 0, 2, 0, 4, 0, 0, 0, 0, 0, 1, 0, 2, 0, 0, 8, 0, 0, 8, 0, 0, // 15

        2, 0, 0, 2, 0, 0, 1, 0, 1, 0, 9, 0, 0, 1, 0, 1, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 1, 0, 0, 0, 0, 0, 0, 0, 6, 0, 0, 0, 0, 0, 1, 0, 0, 5, 0, 0, 9, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 1, 0, 5, 0, 0, 6, 0, 0, 1, 0, 1, 0, // 3

        9, 0, 0, 8, 2, 8, 0, 4, 0, 0, 1, 0, 1, 0, 0, 6, 0, 1, 0, 2, 0, 8, 0, 4, 0, 0,

        1, 1, 2, 8, 0, 0, 5, 0, 0, 0, 0, 0, 0, 1, 0, 0, 6, 1, 1, 0, 2, 0, 8, 6, 0, 1,

        0, 1, 1, 5, 0, 8, 0, 2, 0, 2, 6, 0, 2, 2, 4, 0, 8, 8, 0, 4, 0, 0, 2, 1, 1, 5, // 6

        0, 0, 6, 1, 0, 2, 2, 4, 0, 2, 8, 4, 0, 1, 0, 1, 1, 0, 8, 5, 0, 0, 0, 8, 0, 0,

        3, 0, 0, 2, 0, 0, 8, 0, 0, 2, 0, 0, 8, 0, 0, 2, 0, 3, 0, 0, 2, 0, 0, 8, 0, 0,

        2, 0, 0, 8, 0, 0, 3, 0, 0, 2, 0, 0, 1, 1, 0, 2, 2, 8, 0, 1, 1, 4, 0, 2, 2, 0, // 9

        8, 0, 8, 0, 1, 2, 1, 6, 0, 8, 2, 1, 0, 5, 0, 3, 0, 5, 0, 7, 0, 9, 0, 0, 0, 0,

        0, 0, 7, 0, 1, 1, 9, 0, 3, 5, 0, 1, 1, 9, 0, 0, 7, 0, 1, 1, 5, 0, 3, 1, 1, 0,

        0, 8, 0, 8, 2, 0, 1, 1, 4, 0, 2, 0, 3, 8, 0, 3, 2, 0, 1, 1, 2, 0, 1, 1, 5, 0, // 12

        0, 9, 0, 0, 0, 0, 0, 0, 1, 1, 0, 3, 0, 4, 1, 5, 0, 7, 6, 1, 0, 8, 8, 0, 2, 4,

        5, 0, 0, 0, 6, 1, 1, 0, 9, 1, 0, 5, 0, 0, 3, 0, 0, 7, 0, 9, 0, 5, 0, 0, 8, 2,

        2, 0, 4, 2, 2, 8, 0, 1, 1, 5, 0, 0, 0, 0, 0, 1, 0, 6, 0, 0, 1, 0, 0, 6, 0, 0, // 15

        3, 2, 0, 7, 8, 0, 1, 1, 1, 0, 9, 0, 1, 1, 1, 9, 0, 6, 0, 0, 0, 2, 0, 0, 0, 8,

        0, 1, 0, 2, 0, 8, 0, 2, 0, 6, 0, 2, 0, 8, 0, 1, 0, 0, 9, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Small", score_database2, "Medium");
    }
}