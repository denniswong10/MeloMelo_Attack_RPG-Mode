﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score314 : MonoBehaviour
{
    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 5, 0, 0, 5, 0, 0, 0, 5, 0, 2, 0, 2, 0, 8, 0, 8,

        0, 5, 0, 0, 0, 1, 0, 5, 0, 0, 5, 0, 0, 0, 5, 0, 8, 0, 8, 0, 2, 0, 2, 0, 5, 0, // 3
  
        0, 5, 0, 0, 1, 0, 1, 0, 2, 2, 0, 8, 8, 0, 0, 0, 4, 0, 0, 0, 0, 1, 0, 0, 0, 0,

        1, 0, 0, 0, 0, 5, 0, 5, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 4, 0, 0, 0,

        0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 4, 0, 0, 0, 0, 2, 0, 2, 0, 0, 8, 0, 8, 0, 0, // 6
   
        4, 0, 0, 5, 0, 0, 1, 0, 0, 1, 0, 0, 5, 0, 0, 1, 0, 0, 1, 0, 0, 6, 0, 0, 1, 0,

        0, 1, 0, 0, 5, 0, 5, 0, 0, 2, 2, 0, 0, 4, 0, 5, 0, 0, 1, 0, 1, 0, 8, 8, 0, 0,

        4, 0, 0, 1, 0, 0, 1, 0, 0, 2, 0, 8, 0, 5, 0, 0, 0, 0, 4, 0, 0, 0, 0, 4, 0, 0, // 9
   
        0, 2, 0, 2, 0, 0, 0, 0, 4, 0, 0, 0, 0, 4, 0, 0, 0, 0, 8, 0, 8, 0, 0, 0, 1, 0,

        1, 0, 5, 0, 2, 2, 0, 0, 1, 0, 1, 0, 5, 0, 8, 8, 0, 4, 0, 2, 0, 8, 0, 9, 0, 0, // Save: 284

        0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 5, 0, 0, 0, 0, 5, 0, 0, 0, 0, 9, 0, 0, // 12
  
        0, 0, 5, 0, 0, 0, 0, 5, 0, 0, 0, 0, 9, 0, 0, 0, 0, 5, 0, 0, 0, 0, 5, 0, 0, 0,

        1, 0, 0, 0, 1, 0, 0, 0, 5, 0, 0, 0, 0, 9, 0, 0, 0, 0, 5, 0, 0, 0, 0, 5, 0, 0,

        0, 0, 5, 0, 0, 0, 0, 9, 0, 0, 0, 0, 5, 0, 0, 0, 0, 5, 0, 0, 0, 0, 5, 0, 0, 0, // 15
  
        0, 9, 0, 0, 0, 0, 5, 0, 0, 0, 0, 5, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 5, 0, 0,

        0, 0, 1, 5, 0, 0, 1, 0, 5, 0, 9, 0, 0, 0, 1, 0, 5, 0, 9, 0, 0, 2, 0, 0, 8, 0,

        0, 2, 0, 0, 4, 0, 0, 5, 0, 0, 0, 5, 0, 0, 0, 9, 0, 0, 0, 5, 0, 0, 0, 5, 0, 0, // 18
  
        1, 0, 1, 0, 5, 0, 0, 0, 1, 0, 1, 0, 9, 0, 0, 2, 0, 0, 8, 0, 0, 5, 0, 0, 0, 9,

        0, 0, 1, 0, 0, 1, 0, 0, 5, 0, 8, 0, 2, 2, 0, 8, 0, 5, 0, 0, 0, 0, 9, 0, 0, 0,

        5, 0, 0, 0, 9, 0, 0, 0, 5, 0, 0, 0, 9, 0, 0, 5, 0, 2, 2, 0, 8, 8, 0, 5, 0, 5, // 21
   
        0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0
    };

    private int[] score_database2 =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 5, 0, 1, 1, 5, 0, 0, 2, 0, 4, 0, 8, 0, 5, 0, 0,

        1, 5, 0, 9, 1, 1, 0, 5, 5, 0, 9, 0, 0, 2, 2, 4, 8, 8, 5, 0, 2, 0, 2, 0, 5, 0, // 3
  
        9, 5, 0, 1, 1, 5, 5, 0, 1, 1, 5, 9, 0, 2, 8, 0, 4, 0, 0, 0, 0, 1, 0, 0, 0, 0,

        1, 0, 0, 0, 0, 5, 0, 5, 0, 2, 0, 8, 0, 5, 0, 0, 0, 1, 0, 0, 0, 0, 4, 0, 0, 0,

        1, 1, 0, 0, 0, 0, 5, 0, 5, 0, 0, 6, 0, 0, 0, 0, 2, 2, 4, 0, 1, 0, 1, 0, 8, 8, // 6
   
        4, 0, 0, 5, 0, 0, 1, 0, 0, 5, 0, 5, 0, 0, 2, 2, 4, 8, 8, 0, 1, 6, 0, 0, 2, 0,

        0, 8, 0, 0, 5, 0, 9, 0, 0, 1, 1, 5, 0, 4, 0, 1, 1, 9, 0, 2, 0, 8, 0, 4, 0, 2,

        8, 0, 4, 1, 0, 0, 5, 0, 2, 2, 4, 0, 8, 8, 4, 0, 5, 0, 5, 0, 0, 2, 2, 8, 0, 4, // 9
   
        8, 8, 2, 0, 4, 0, 5, 0, 5, 0, 0, 2, 2, 4, 8, 8, 0, 5, 0, 5, 0, 0, 1, 1, 1, 0,

        2, 0, 4, 0, 8, 8, 4, 0, 1, 1, 1, 0, 5, 0, 1, 1, 1, 0, 4, 2, 4, 8, 5, 9, 0, 0,

        0, 3, 0, 0, 3, 2, 8, 5, 0, 2, 0, 8, 0, 5, 0, 2, 8, 0, 5, 0, 1, 1, 0, 9, 0, 0, // 12
  
        5, 0, 5, 0, 9, 5, 0, 5, 0, 0, 1, 1, 5, 0, 1, 1, 9, 0, 2, 0, 8, 0, 5, 0, 0, 1,

        1, 2, 2, 1, 1, 8, 8, 0, 5, 0, 1, 1, 0, 9, 0, 0, 2, 2, 5, 0, 2, 2, 9, 0, 8, 0,

        8, 0, 5, 0, 0, 3, 1, 5, 0, 7, 6, 5, 9, 0, 3, 1, 5, 0, 7, 6, 5, 9, 0, 2, 2, 0, // 15
  
        8, 0, 8, 0, 5, 0, 9, 0, 1, 5, 0, 1, 9, 0, 2, 8, 2, 0, 1, 5, 0, 1, 9, 0, 8, 2,

        8, 0, 5, 0, 0, 2, 2, 2, 5, 8, 8, 8, 9, 2, 2, 2, 5, 8, 8, 8, 9, 0, 5, 2, 2, 2,

        9, 8, 8, 8, 5, 1, 9, 1, 2, 2, 2, 5, 8, 8, 8, 9, 2, 8, 2, 5, 0, 0, 1, 1, 5, 0, // 18
  
        1, 1, 0, 5, 0, 0, 0, 1, 1, 5, 0, 2, 0, 8, 0, 2, 0, 5, 8, 0, 1, 5, 0, 1, 9, 0,

        2, 2, 4, 8, 8, 0, 5, 0, 9, 0, 5, 0, 2, 2, 4, 8, 5, 0, 0, 0, 1, 0, 5, 0, 1, 0,

        5, 0, 2, 2, 8, 0, 4, 0, 2, 8, 2, 0, 4, 0, 1, 1, 5, 0, 2, 2, 0, 8, 8, 0, 9, 0, // 21
   
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Medium", score_database2, "Medium");
    }
}