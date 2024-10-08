﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score144 : MonoBehaviour
{
    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 1, 0, 0, 0, 4, 0, 0, 0, 2, 0, 0, 0,

        4, 0, 0, 0, 2, 0, 0, 5, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 6, 0, 0, 1, 0, 0, 5, // 3

        0, 0, 2, 0, 0, 4, 0, 0, 2, 0, 0, 2, 0, 0, 1, 0, 5, 0, 0, 0, 0, 0, 0, 0, 1, 0,

        0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 2, 0, 5, 0, 0, 0, 1, 0, 0, 1, 0, 0, 6, 0,

        0, 5, 0, 0, 2, 0, 0, 2, 0, 0, 4, 0, 0, 5, 0, 0, 1, 0, 2, 2, 0, 5, 0, 0, 0, 2, // 6

        0, 0, 0, 2, 0, 0, 0, 4, 0, 0, 0, 2, 0, 0, 0, 2, 0, 0, 0, 4, 0, 0, 1, 0, 0, 1,

        0, 0, 5, 0, 0, 0, 1, 0, 0, 1, 0, 0, 5, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 4, 0,

        0, 2, 0, 0, 2, 0, 0, 2, 5, 0, 0, 0, 2, 2, 0, 0, 4, 0, 0, 0, 2, 2, 0, 0, 4, 0, // 9

        0, 0, 2, 2, 0, 0, 5, 0, 0, 0, 2, 0, 0, 0, 1, 0, 0, 0, 2, 2, 0, 0, 1, 0, 5, 0,

        1, 0, 5, 0, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 1, 0, 2, 0, 1, 0, 4, 0, 1, 1, 5, 0, 1, 1, 2, 0, 0, 5, 0, 0,

        4, 0, 2, 0, 4, 0, 2, 0, 1, 2, 4, 0, 1, 2, 5, 0, 0, 1, 0, 0, 2, 0, 1, 0, 4, 0, // 3

        5, 0, 1, 5, 1, 0, 5, 1, 5, 0, 6, 0, 1, 0, 6, 0, 5, 0, 0, 2, 0, 0, 2, 0, 0, 2,

        0, 0, 4, 0, 0, 2, 0, 0, 2, 0, 0, 2, 0, 0, 5, 0, 0, 0, 1, 1, 5, 0, 1, 1, 6, 0,

        2, 0, 1, 0, 4, 0, 1, 2, 5, 0, 4, 0, 1, 5, 2, 0, 1, 2, 0, 4, 1, 5, 0, 0, 0, 1, // 6

        0, 2, 0, 5, 0, 1, 1, 2, 0, 0, 0, 4, 0, 2, 0, 4, 0, 1, 1, 5, 0, 2, 0, 2, 0, 1,

        0, 5, 0, 0, 0, 1, 1, 5, 0, 1, 1, 6, 0, 2, 0, 0, 1, 1, 6, 0, 1, 1, 5, 0, 4, 0,

        2, 2, 0, 4, 0, 2, 0, 1, 2, 0, 0, 0, 2, 1, 5, 0, 7, 0, 2, 0, 1, 1, 5, 0, 3, 0, // 9

        2, 0, 1, 2, 5, 0, 7, 0, 2, 0, 1, 1, 5, 0, 4, 0, 2, 0, 1, 2, 0, 0, 2, 4, 2, 0,

        1, 1, 5, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    // Start is called before the first frame update
    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Small", score_database2, "Medium");
    }
}
