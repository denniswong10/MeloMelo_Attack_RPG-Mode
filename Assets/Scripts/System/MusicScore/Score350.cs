﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score350 : MonoBehaviour
{
    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 93,
   
        0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 93, 0, 0, 0, 0, 0, 0, 5, 0, 5, 0, 93,
   
        0, 0, 0, 0, 5, 0, 5, 0, 93, 0, 0, 0, 1, 0, 2, 0, 4, 0, 0, 1, 0, 2, 0, 5, 0, 0, // 3
   
        0, 0, 1, 0, 2, 0, 0, 1, 0, 8, 0, 5, 0, 5, 0, 1, 0, 4, 0, 0, 1, 0, 2, 2, 0, 5,
   
        0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 5, 0, 0, 2, 0, 8, 0, 0, 4, 0, 0, 0,
   
        0, 0, 0, 2, 0, 2, 0, 4, 0, 8, 0, 8, 0, 4, 0, 1, 0, 5, 0, 0, 0, 2, 0, 4, 0, 8, // 6
   
        0, 4, 0, 5, 0, 0, 0, 0, 2, 0, 2, 0, 4, 0, 5, 0, 0, 8, 0, 8, 0, 4, 0, 5, 0, 0,
   
        1, 0, 2, 2, 0, 8, 8, 0, 5, 0, 0, 0, 0, 2, 0, 8, 0, 5, 0, 4, 0, 2, 0, 8, 0, 5,
   
        0, 0, 1, 0, 2, 0, 4, 0, 1, 0, 8, 8, 0, 5, 0, 0, 0, 0, 0, 9, 0, 0, 1, 1, 0, 2, // 9
   
        0, 8, 0, 5, 0, 2, 8, 0, 5, 0, 0, 2, 0, 4, 0, 8, 0, 4, 0, 5, 0, 0, 0, 0, 0, 0,
   
        9, 0, 0, 0, 93, 0, 0, 0, 5, 0, 0, 0, 93, 0, 0, 2, 8, 0, 5, 0, 1, 0, 8, 0, 5, 0,
   
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
         0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 0, 5, 0, 5, 0, 93,

        0, 93, 0, 93, 0, 0, 8, 8, 8, 0, 5, 0, 5, 0, 93, 0, 93, 0, 93, 0, 0, 1, 1, 5, 0, 93,

        0, 93, 0, 0, 1, 1, 9, 0, 93, 0, 93, 0, 2, 4, 0, 8, 4, 0, 5, 5, 0, 1, 9, 0, 0, 0, // 3
   
        0, 2, 2, 0, 5, 0, 8, 8, 0, 5, 0, 9, 0, 5, 0, 0, 2, 2, 2, 0, 8, 8, 8, 0, 5, 9,

        0, 0, 2, 2, 0, 0, 8, 8, 0, 0, 5, 0, 2, 2, 4, 0, 8, 8, 4, 0, 93, 0, 5, 0, 0, 0,

        0, 0, 0, 18, 0, 0, 0, 0, 0, 0, 2, 0, 18, 0, 0, 0, 0, 0, 0, 0, 2, 8, 5, 0, 1, 5, // 6
   
        0, 2, 8, 9, 0, 93, 0, 0, 18, 0, 0, 0, 0, 0, 0, 5, 5, 0, 18, 0, 0, 0, 0, 0, 2, 8,

        5, 0, 0, 5, 0, 1, 5, 1, 9, 0, 93, 0, 93, 0, 18, 0, 7, 0, 7, 0, 7, 0, 2, 8, 18, 0,

        0, 7, 0, 7, 0, 7, 0, 5, 0, 2, 2, 8, 5, 0, 0, 0, 0, 0, 0, 9, 2, 2, 0, 5, 8, 8, // 9
   
        0, 4, 2, 2, 0, 4, 8, 8, 5, 0, 0, 1, 2, 4, 0, 1, 8, 4, 0, 5, 0, 0, 1, 1, 0, 0,

        103, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 8, 0, 0, 1, 1, 0, 5, 5, 0, 2, 8, 2, 5, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Small", score_database2, "Medium");
    }
}