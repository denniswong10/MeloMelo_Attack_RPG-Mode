﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score277 : MonoBehaviour
{
    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2,

        0, 0, 0, 0, 0, 0, 0, 8, 0, 0, 0, 0, 0, 0, 0, 8, 0, 0, 0, 0, 0, 0, 0, 8, 0, 0,

        0, 2, 0, 0, 0, 2, 0, 0, 0, 2, 0, 0, 8, 0, 0, 5, 0, 0, 0, 1, 0, 0, 0, 4, 0, 0, // 3

        0, 1, 0, 0, 0, 4, 0, 8, 0, 0, 1, 0, 1, 0, 0, 2, 0, 0, 8, 0, 0, 2, 0, 0, 1, 0,

        1, 0, 0, 2, 0, 0, 8, 0, 0, 2, 0, 0, 5, 0, 0, 8, 0, 0, 0, 0, 0, 0, 0, 8, 0, 0,

        0, 0, 0, 0, 0, 8, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 8, 0, 0, 0, 0, // 6

        0, 0, 0, 2, 0, 0, 0, 4, 0, 0, 1, 0, 0, 1, 0, 0, 2, 0, 0, 8, 0, 0, 5, 0, 0, 0,

        1, 0, 0, 1, 0, 0, 2, 0, 0, 2, 0, 0, 1, 0, 0, 1, 0, 0, 8, 0, 0, 8, 0, 0, 4, 0,

        0, 1, 0, 1, 0, 0, 5, 0, 0, 1, 0, 1, 0, 0, 5, 0, 0, 0, 2, 0, 0, 8, 0, 0, 2, 0, // 9

        0, 4, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0,

        0, 0, 2, 0, 0, 8, 0, 0, 5, 0, 0, 0, 1, 0, 1, 0, 0, 2, 0, 0, 2, 0, 0, 1, 0, 1,

        0, 0, 5, 0, 0, 0, 1, 0, 0, 4, 0, 0, 1, 0, 0, 4, 0, 0, 1, 0, 1, 0, 0, 2, 0, 0, // 12

        8, 0, 0, 5, 0, 0, 0, 0, 1, 0, 0, 2, 0, 0, 2, 0, 0, 4, 0, 0, 8, 0, 0, 8, 0, 0,

        4, 0, 0, 1, 0, 1, 0, 0, 5, 0, 0, 1, 0, 1, 0, 0, 5, 0, 0, 4, 0, 2, 0, 2, 0, 0,

        1, 0, 0, 1, 0, 0, 8, 0, 0, 2, 0, 0, 4, 0, 0, 0, 5, 0, 5, 0, 0, 4, 0, 0, 5, 0, // 15

        5, 0, 0, 4, 0, 0, 2, 0, 0, 4, 0, 0, 2, 0, 8, 0, 2, 0, 0, 4, 0, 0, 1, 0, 0, 1,

        0, 0, 8, 0, 0, 4, 0, 0, 2, 0, 0, 4, 0, 0, 1, 0, 1, 0, 0, 5, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 5, 0, 0, 1, 0, 0, 5, 0, // 18

        0, 1, 0, 1, 0, 0, 5, 0, 0, 1, 0, 1, 0, 0, 5, 0, 0, 4, 0, 0, 2, 0, 8, 0, 2, 0,

        0, 4, 0, 0, 1, 0, 0, 1, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 2, 0, 8, 0, 2, 0, 0, 0,

        0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 21

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 0, 0, 2, 2, 2, 0, 0, 8, 0, 0, 2, 2, 2, 0, 0, 8,

        0, 0, 2, 0, 2, 0, 5, 5, 0, 8, 0, 8, 0, 2, 2, 2, 0, 0, 8, 0, 0, 2, 2, 2, 0, 0,

        8, 0, 8, 0, 2, 2, 0, 4, 0, 2, 2, 0, 4, 0, 0, 5, 0, 0, 0, 1, 0, 0, 5, 0, 0, 1, // 3

        0, 0, 5, 0, 0, 2, 8, 2, 8, 0, 5, 0, 5, 0, 0, 1, 1, 0, 5, 0, 0, 1, 1, 0, 9, 0,

        2, 0, 8, 0, 2, 0, 8, 8, 0, 4, 0, 0, 5, 0, 0, 8, 0, 0, 2, 0, 2, 0, 0, 8, 0, 0,

        2, 0, 2, 0, 0, 8, 0, 0, 1, 0, 1, 0, 0, 2, 0, 0, 1, 0, 1, 0, 0, 8, 0, 0, 2, 2, // 6

        8, 0, 0, 2, 0, 0, 8, 8, 2, 0, 5, 0, 0, 8, 8, 2, 0, 5, 0, 1, 1, 0, 5, 0, 0, 0,

        1, 0, 0, 3, 2, 0, 4, 0, 7, 2, 0, 4, 0, 1, 0, 1, 0, 5, 5, 0, 0, 1, 0, 1, 0, 2,

        2, 0, 1, 1, 0, 5, 0, 5, 0, 4, 0, 2, 8, 2, 5, 0, 0, 0, 3, 0, 0, 8, 0, 0, 2, 0, // 9

        0, 5, 0, 0, 3, 0, 0, 2, 2, 0, 8, 0, 8, 0, 0, 5, 0, 0, 1, 0, 2, 2, 8, 0, 0, 5,

        0, 1, 0, 8, 2, 2, 0, 0, 5, 0, 0, 1, 2, 2, 0, 4, 0, 1, 8, 8, 0, 1, 0, 1, 0, 5,

        0, 0, 9, 0, 0, 1, 1, 5, 0, 2, 2, 0, 1, 1, 0, 5, 5, 0, 8, 0, 8, 0, 0, 4, 0, 0, // 12

        2, 8, 0, 5, 0, 0, 0, 0, 3, 0, 2, 2, 0, 7, 0, 8, 8, 4, 0, 5, 0, 5, 0, 1, 1, 0,

        5, 0, 0, 1, 1, 5, 0, 0, 9, 0, 0, 1, 2, 0, 1, 8, 0, 5, 0, 8, 0, 2, 0, 8, 0, 0,

        5, 0, 0, 9, 0, 0, 5, 0, 0, 2, 8, 2, 8, 0, 4, 0, 5, 0, 9, 0, 0, 5, 0, 1, 1, 0, // 15

        2, 2, 0, 8, 8, 0, 5, 0, 2, 2, 2, 0, 4, 0, 8, 8, 8, 0, 4, 0, 2, 8, 2, 8, 0, 0,

        5, 0, 5, 0, 0, 1, 1, 5, 0, 4, 0, 1, 1, 9, 0, 2, 8, 5, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 2, 8, 2, 8, 0, 6, 0, 6, 0, // 18

        3, 0, 8, 8, 2, 2, 0, 6, 0, 6, 0, 0, 1, 0, 1, 0, 5, 0, 0, 2, 8, 2, 8, 0, 4, 0,

        2, 0, 2, 0, 5, 0, 0, 1, 5, 8, 9, 0, 0, 0, 7, 0, 0, 7, 1, 0, 5, 0, 0, 0, 0, 5,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 21

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Small", score_database2, "Medium");
    }
}