﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score348 : MonoBehaviour
{
    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 0, 13, 0, 0, 0, 5, 0, 0, 0, 14, 0, 0, 0, 5, 0, 0, 0, 15, 0, 0, 0, 5, 0, 0, 2,

        0, 8, 0, 5, 0, 0, 1, 0, 5, 5, 0, 2, 0, 8, 0, 13, 0, 0, 5, 0, 14, 0, 0, 5, 0, 0,

        2, 0, 0, 8, 0, 0, 1, 2, 0, 1, 8, 0, 5, 0, 0, 0, 1, 0, 5, 5, 0, 2, 0, 8, 0, 4, // 3
  
        0, 5, 0, 13, 0, 0, 5, 0, 14, 0, 0, 5, 0, 0, 4, 0, 0, 0, 0, 16, 0, 0, 0, 0, 0, 0,

        1, 0, 0, 0, 17, 0, 0, 0, 0, 0, 0, 0, 2, 0, 4, 0, 8, 0, 5, 0, 5, 0, 0, 0, 0, 1,

        0, 0, 1, 0, 0, 5, 0, 0, 5, 0, 0, 13, 0, 0, 0, 0, 5, 0, 0, 14, 0, 0, 0, 0, 5, 0, // 6
   
        0, 0, 5, 0, 0, 0, 3, 0, 0, 2, 0, 2, 0, 0, 3, 0, 0, 8, 0, 8, 0, 0, 5, 0, 0, 15,

        0, 0, 0, 0, 5, 0, 0, 15, 0, 0, 0, 0, 5, 0, 0, 2, 0, 2, 0, 0, 8, 0, 8, 0, 0, 5,

        0, 0, 2, 0, 8, 0, 4, 0, 0, 1, 2, 0, 1, 8, 0, 4, 0, 5, 0, 0, 0, 1, 0, 54, 0, 0, // 9 

        0, 0, 5, 0, 0, 54, 0, 0, 0, 5, 0, 0, 23, 0, 0, 0, 0, 2, 0, 8, 0, 5, 0, 0, 5, 0,
   
        0, 0, 24, 0, 0, 0, 5, 0, 0, 1, 0, 1, 0, 0, 5, 0, 0, 6, 0, 0, 5, 0, 0, 2, 2, 0,

        4, 0, 8, 5, 0, 0, 1, 0, 9, 0, 0, 2, 0, 2, 0, 8, 0, 8, 0, 0, 5, 0, 0, 5, 0, 0, // 12

        6, 0, 0, 0, 0, 13, 0, 0, 14, 0, 0, 15, 0, 0, 0, 5, 0, 0, 23, 0, 0, 0, 24, 0, 0, 0,

        4, 0, 0, 2, 8, 0, 5, 0, 0, 0, 0, 1, 0, 0, 13, 0, 0, 15, 0, 0, 6, 0, 0, 1, 0, 0,
    
        14, 0, 0, 15, 0, 0, 6, 0, 1, 0, 5, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 15

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 0, 1, 2, 2, 5, 0, 0, 1, 8, 8, 5, 0, 0, 6, 0, 0, 1, 5, 1, 5, 0, 0, 1, 9, 1,

        9, 0, 0, 6, 0, 0, 13, 0, 0, 5, 0, 5, 0, 14, 0, 0, 5, 0, 5, 0, 15, 0, 0, 5, 0, 5,

        0, 2, 4, 2, 0, 5, 0, 8, 4, 8, 0, 0, 5, 0, 0, 0, 1, 0, 5, 5, 0, 2, 0, 8, 0, 4, // 3
  
        0, 5, 0, 13, 0, 0, 5, 0, 14, 0, 0, 5, 0, 0, 0, 0, 0, 0, 2, 2, 0, 8, 8, 4, 0, 0,

        5, 0, 5, 0, 8, 8, 0, 2, 2, 4, 0, 0, 5, 0, 13, 0, 0, 14, 0, 0, 5, 0, 0, 0, 0, 1,

        1, 5, 0, 1, 1, 6, 0, 1, 1, 5, 0, 24, 0, 0, 0, 23, 0, 0, 0, 2, 4, 2, 4, 0, 5, 0, // 6
   
        0, 0, 5, 0, 0, 0, 3, 0, 8, 8, 8, 13, 0, 0, 3, 0, 0, 8, 8, 14, 0, 0, 5, 0, 0, 2,

        2, 0, 8, 8, 5, 0, 0, 8, 8, 0, 2, 2, 5, 0, 0, 13, 0, 0, 5, 0, 13, 0, 0, 5, 0, 13,

        0, 0, 5, 0, 2, 2, 4, 0, 8, 8, 4, 0, 5, 0, 8, 4, 8, 4, 0, 0, 0, 16, 0, 0, 0, 0, // 9 

        0, 0, 5, 0, 0, 17, 0, 0, 0, 0, 0, 0, 5, 0, 0, 2, 4, 2, 0, 8, 4, 8, 0, 5, 0, 5,

        0, 1, 1, 5, 0, 1, 1, 6, 0, 0, 13, 0, 0, 5, 14, 0, 0, 5, 15, 0, 0, 5, 0, 2, 2, 0,

        8, 8, 4, 5, 0, 0, 1, 1, 9, 0, 1, 1, 5, 0, 8 , 4, 8, 4, 0, 5, 5, 0, 2, 4, 0, 8, // 12

        4, 0, 0, 0, 0, 17, 0, 7, 0, 7, 0, 7, 16, 0, 7, 0, 7, 0, 7, 5, 0, 23, 0, 0, 0, 24,

        0, 0, 0, 5, 0, 0, 5, 0, 0, 0, 0, 1, 1, 5, 24, 0, 0, 0, 1, 1, 5, 23, 0, 0, 0, 2,

        4, 0, 8, 4, 0, 1, 5, 0, 6, 0, 1, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 15

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };


    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Medium", score_database2, "Medium");
    }
}