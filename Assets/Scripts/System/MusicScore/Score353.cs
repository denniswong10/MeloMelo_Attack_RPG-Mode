﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score353 : MonoBehaviour
{
    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 0, 0, 0, 0, 5, 0, 0, 1, 0, 4, 0, 1, 0, 6, 0, 0, 14, 0, 11, 0, 5, 0, 0, 0, 93,

        0, 0, 0, 93, 0, 0, 0, 1, 5, 0, 1, 9, 0, 11, 0, 0, 1, 5, 0, 1, 9, 0, 14, 0, 0, 0,

        2, 2, 0, 8, 8, 0, 11, 0, 14, 0, 5, 0, 9, 0, 0, 1, 4, 0, 1, 4, 0, 2, 2, 0, 8, 8, // 3
  
        0, 4, 0, 5, 0, 9, 0, 0, 1, 11, 0, 1, 14, 0, 5, 0, 11, 0, 14, 0, 5, 0, 0, 1, 1, 0,

        5, 5, 0, 4, 0, 2, 0, 5, 0, 8, 0, 5, 0, 1, 1, 0, 2, 8, 0, 11, 0, 14, 0, 0, 0, 0,

        2, 4, 0, 8, 4, 0, 1, 23, 0, 24, 0, 5, 0, 0, 1, 1, 0, 5, 0, 1, 1, 0, 9, 23, 0, 24, // 6
 
        0, 0, 0, 93, 0, 0, 0, 93, 0, 0, 0, 5, 0, 0, 0, 93, 0, 0, 0, 93, 0, 0, 0, 3, 0, 0,

        0, 7, 0, 0, 0, 3, 0, 0, 0, 7, 0, 0, 0, 1, 1, 0, 5, 5, 0, 14, 0, 11, 0, 0, 0, 0,

        0, 0, 0, 0, 31, 0, 5, 0, 5, 0, 0, 31, 0, 5, 0, 5, 0, 0, 1, 5, 0, 1, 9, 0, 11, 0, // 9
  
        5, 0, 14, 0, 2, 0, 8, 5, 0, 0, 31, 4, 0, 5, 0, 0, 31, 4, 0, 5, 0, 0, 1, 1, 0, 23,

        0, 1, 1, 0, 24, 0, 0, 5, 5, 0, 1, 1, 0, 9, 0, 0, 2, 2, 0, 8, 0, 2, 2, 0, 8, 0,

        2, 2, 0, 5, 11, 0, 14, 0, 0, 2, 4, 0, 8, 4, 0, 5, 9, 0, 0, 1, 1, 0, 5, 5, 0, 1, // 12
  
        1, 0, 5, 5, 0, 2, 2, 0, 8, 8, 0, 11, 0, 4, 4, 0, 31, 0, 5, 5, 0, 4, 0, 0, 0, 31,

        0, 9, 5, 0, 4, 0, 2, 2, 0, 31, 0, 0, 8, 8, 0, 31, 0, 0, 4, 2, 0, 4, 8, 0, 1, 1,

        0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 1, 4, 0, 6, 0, 1, 5, 0, 6, 0, 37, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 36, 0, 5, 0, 35, 0, 5, 0, 1, 1, 5, 0, 1, 1, 9, 0, 14, 0, 0,

        2, 0, 0, 8, 0, 0, 23, 0, 24, 0, 5, 5, 5, 0, 11, 0, 0, 5, 0, 0, 93, 0, 14, 0, 0, 5, // 3
  
        2, 2, 0, 23, 0, 24, 0, 5, 5, 9, 0, 11, 0, 0, 4, 0, 0, 93, 0, 14, 0, 5, 5, 0, 1, 1,

        0, 5, 5, 0, 13, 0, 0, 5, 0, 11, 0, 5, 5, 0, 1, 2, 1, 8, 0, 14, 0, 0, 4, 0, 0, 0,

        1, 5, 14, 0, 0, 1, 1, 23, 0, 0, 1, 1, 24, 0, 0, 4, 5, 14, 0, 5, 5, 14, 0, 1, 6, 1, // 6
 
        9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 4, 0, 5, 6, 0, 2, 8, 5, 0, 0, 0,

        0, 0, 0, 0, 31, 0, 31, 0, 1, 1, 5, 0, 31, 0, 31, 0, 1, 1, 9, 0, 14, 0, 5, 5, 0, 23, // 9
  
        0, 5, 5, 0, 24, 0, 5, 9, 0, 0, 31, 4, 0, 31, 4, 0, 1, 1, 5, 0, 1, 1, 9, 0, 23, 0,

        0, 24, 0, 5, 0, 2, 2, 8, 0, 1, 1, 1, 0, 5, 0, 0, 2, 0, 0, 8, 0, 0, 4, 0, 0, 2,

        0, 0, 1, 1, 5, 0, 14, 0, 0, 1, 1, 9, 0, 11, 0, 0, 5, 9, 0, 31, 7, 3, 0, 31, 7, 3, // 12
  
        0, 1, 2, 1, 8, 0, 5, 0, 1, 2, 1, 8, 0, 14, 0, 0, 5, 1, 5, 1, 0, 11, 0, 0, 0, 31,

        0, 1, 2, 5, 0, 31, 4, 0, 4, 31, 0, 0, 5, 5, 0, 2, 2, 4, 0, 8, 8, 4, 0, 1, 1, 1,

        1, 0, 14, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Medium", score_database2, "Medium");
    }
}