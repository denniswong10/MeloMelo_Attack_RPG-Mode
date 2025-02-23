﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score391 : MonoBehaviour
{
    // 30, 31 - Sweep | 51 - Heart | 53 - Circle | 14, 15 - Curve | 103, 104 - BowCurve | 11, 20, 12 - RisingHeart
    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 0, 0, 0, 102, 0, 0, 51, 0, 0, 102, 0, 0, 51, 0, 0, 30, 0, 0, 0, 5, 0, 31, 0, 0, 0,

        5, 0, 1, 0, 5, 0, 1, 0, 9, 0, 102, 0, 0, 102, 0, 0, 51, 0, 0, 51, 0, 0, 1, 0, 5, 0,

        9, 0, 0, 1, 0, 5, 0, 1, 0, 9, 0, 5, 0, 0, 103, 0, 0, 0, 0, 104, 0, 0, 0, 0, 103, 0, // 3
  
        0, 0, 0, 104, 0, 0, 0, 0, 5, 0, 103, 0, 11, 0, 0, 104, 0, 12, 0, 0, 103, 0, 11, 0, 0, 104,

        0, 12, 0, 0, 1, 1, 0, 5, 0, 9, 0, 0, 0, 4, 0, 0, 0, 1, 0, 0, 6, 0, 0, 102, 0, 0,

        5, 0, 0, 102, 0, 0, 5, 5, 0, 51, 0, 0, 5, 0, 0, 0, 0, 0, 1, 0, 5, 0, 1, 0, 9, 0, // 6
   
        0, 0, 2, 2, 0, 4, 0, 5, 0, 0, 9, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 5, 0, 0, 0, 29,

        0, 0, 0, 0, 0, 102, 0, 0, 5, 0, 51, 0, 0, 5, 5, 0, 0, 0, 1, 0, 5, 5, 0, 0, 30, 0,

        0, 0, 0, 4, 0, 4, 0, 31, 0, 0, 0, 0, 4, 0, 4, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, // 9
   
        93, 0, 0, 0, 0, 93, 0, 0, 0, 0, 93, 0, 0, 0, 0, 3, 0, 0, 0, 0, 3, 0, 0, 0, 0, 3,

        0, 0, 0, 0, 0, 93, 0, 0, 0, 0, 3, 0, 0, 0, 93, 0, 0, 0, 0, 3, 0, 0, 0, 93, 0, 0,

        0, 0, 3, 0, 0, 0, 7, 0, 0, 0, 2, 2, 0, 5, 0, 0, 3, 0, 0, 0, 8, 0, 8, 0, 0, 3, // 12
 
        0, 0, 0, 1, 0, 5, 0, 3, 0, 0, 0, 7, 0, 0, 1, 0, 5, 0, 93, 0, 1, 0, 6, 0, 93, 0,

        2, 0, 4, 0, 7, 0, 8, 0, 4, 0, 5, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 5, 0, 0, 1, 0,

        0, 9, 0, 0, 1, 1, 0, 0, 5, 0, 0, 1, 1, 0, 0, 5, 6, 0, 0, 0, 0, 1, 0, 30, 0, 0, // 15
 
        0, 0, 1, 0, 31, 0, 0, 0, 0, 4, 0, 4, 0, 0, 5, 0, 0, 1, 0, 6, 0, 0, 0, 0, 0, 0,

        0, 0, 5, 0, 0, 1, 0, 102, 0, 0, 5, 0, 5, 0, 6, 0, 1, 1, 0, 30, 0, 0, 0, 31, 0, 0,

        0, 20, 0, 5, 0, 0, 0, 0, 1, 0, 2, 0, 2, 0, 5, 0, 5, 0, 8, 0, 8, 0, 5, 0, 6, 0, // 18
  
        0, 0, 7, 0, 0, 0, 7, 0, 0, 0, 0, 5, 0, 6, 0, 0, 1, 1, 0, 5, 0, 0, 1, 0, 6, 0,

        0, 1, 1, 0, 5, 0, 0, 4, 2, 0, 5, 0, 0, 0, 0, 1, 30, 0, 0, 0, 1, 31, 0, 0, 0, 5,

        0, 5, 9, 0, 5, 0, 5, 9, 0, 1, 0, 5, 0, 6, 0, 0, 0, 7, 0, 0, 0, 1, 1, 0, 5, 0, // 21
  
        0, 1, 1, 0, 9, 0, 0, 1, 1, 0, 1, 1, 0, 5, 0, 5, 0, 0, 6, 5, 0, 0, 9, 0, 0, 0,

        0, 0, 14, 0, 0, 0, 0, 15, 0, 0, 0, 0, 102, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 1, 1, 0,

        1, 1, 0, 5, 0, 6, 0, 0, 0, 0, 4, 0, 0, 0, 0, 4, 0, 0, 0, 0, 4, 0, 0, 0, 0, 2, // 24
 
        0, 0, 0, 0, 8, 0, 0, 0, 0, 5, 0, 0, 0, 0, 1, 2, 0, 5, 0, 0, 4, 0, 0, 0, 0, 4,

        0, 0, 0, 0, 5, 0, 0, 0, 0, 9, 0, 0, 0, 0, 1, 0, 5, 5, 0, 102, 0, 0, 102, 0, 0, 102,

        0, 0, 5, 5, 0, 1, 1, 0, 6, 0, 0, 1, 1, 0, 9, 0, 51, 0, 0, 51, 0, 0, 51, 0, 0, 5, // 27
  
        5, 0, 0, 1, 1, 0, 6, 0, 0, 0, 7, 0, 0, 0, 3, 0, 0, 2, 2, 0, 8, 8, 0, 5, 5, 0,

        1, 5, 0, 1, 9, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 93, 0, 0, 0, 93, 93,

        0, 8, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 30
  
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 0, 0, 0, 1, 1, 0, 5, 0, 5, 0, 4, 0, 51, 0, 0, 51, 0, 0, 1, 1, 5, 5, 0, 2, 2,
  
        4, 0, 8, 8, 4, 0, 1, 1, 5, 0, 1, 1, 0, 5, 0, 4, 0, 51, 0, 0, 51, 0, 0, 1, 1, 5,
  
        5, 0, 0, 1, 1, 6, 0, 1, 1, 6, 0, 5, 0, 0, 14, 0, 0, 0, 0, 1, 0, 1, 0, 5, 0, 0, // 3
  
        2, 2, 2, 2, 0, 4, 0, 5, 0, 0, 15, 0, 0, 0, 0, 1, 0, 1, 0, 5, 0, 0, 8, 8, 8, 8,
  
        0, 5, 0, 1, 1, 0, 4, 8, 8, 5, 0, 0, 0, 4, 0, 0, 0, 1, 1, 5, 9, 0, 0, 103, 0, 0,
  
        0, 0, 0, 1, 4, 1, 0, 5, 0, 0, 0, 0, 104, 0, 0, 0, 0, 0, 1, 5, 1, 9, 0, 0, 5, 0, // 6
   
        0, 0, 1, 2, 2, 5, 0, 1, 8, 8, 5, 0, 0, 6, 0, 0, 5, 0, 0, 0, 0, 93, 0, 0, 0, 29,
  
        0, 0, 0, 0, 0, 1, 1, 0, 5, 0, 53, 0, 0, 1, 1, 6, 0, 53, 0, 0, 1, 1, 5, 0, 30, 0,
  
        0, 0, 0, 1, 1, 6, 0, 31, 0, 0, 0, 0, 1, 5, 9, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, // 9
   
        7, 0, 0, 0, 0, 7, 0, 0, 0, 0, 7, 0, 0, 0, 0, 3, 0, 0, 0, 0, 3, 0, 0, 0, 0, 3,
   
        0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 3, 0, 0, 0, 2, 2, 0, 0, 0, 3, 0, 0, 0, 8, 8, 0,
  
        0, 0, 3, 0, 0, 0, 5, 0, 0, 0, 0, 1, 1, 0, 0, 0, 3, 0, 0, 0, 1, 5, 0, 0, 0, 3, // 12
 
        0, 0, 0, 1, 1, 5, 0, 3, 0, 0, 0, 5, 0, 0, 1, 1, 5, 0, 3, 0, 1, 1, 6, 0, 3, 0,
 
        2, 2, 4, 0, 3, 0, 8, 8, 4, 0, 5, 0, 0, 0, 0, 0, 0, 0, 53, 0, 0, 5, 0, 0, 53, 0,
 
        0, 9, 0, 0, 53, 20, 0, 0, 5, 0, 0, 53, 20, 0, 1, 1, 5, 0, 0, 0, 0, 1, 1, 30, 0, 0, // 15
 
        0, 0, 1, 1, 31, 0, 0, 0, 0, 1, 1, 5, 6, 0, 51, 0, 0, 1, 5, 9, 0, 0, 0, 0, 0, 0,
  
        0, 0, 102, 0, 0, 1, 1, 102, 0, 0, 1, 1, 5, 0, 1, 1, 6, 0, 102, 103, 0, 0, 0, 102, 0, 0,
  
        5, 6, 0, 51, 0, 0, 0, 0, 1, 1, 14, 0, 0, 0, 0, 1, 1, 15, 0, 0, 0, 0, 1, 5, 6, 0, // 18
  
        0, 0, 7, 0, 0, 0, 3, 0, 0, 0, 0, 1, 1, 6, 0, 103, 0, 20, 0, 20, 0, 102, 0, 104, 0, 20,
  
        0, 20, 0, 102, 0, 0, 1, 1, 5, 0, 4, 0, 0, 0, 0, 1, 1, 5, 0, 1, 1, 6, 5, 0, 30, 0,
  
        0, 53, 31, 0, 0, 53, 0, 20, 0, 1, 1, 5, 0, 6, 0, 0, 0, 7, 0, 0, 0, 102, 0, 0, 5, 0, // 21
  
        0, 102, 0, 0, 9, 0, 0, 102, 0, 102, 0, 51, 0, 0, 5, 0, 0, 1, 1, 6, 0, 0, 5, 0, 0, 0,
  
        0, 0, 14, 20, 0, 11, 0, 15, 20, 0, 12, 0, 51, 0, 20, 5, 9, 0, 0, 0, 0, 0, 0, 1, 1, 5,
 
        0, 1, 1, 9, 0, 5, 0, 0, 0, 0, 7, 0, 0, 0, 0, 7, 0, 0, 0, 0, 7, 0, 0, 0, 0, 3, // 24
 
        0, 0, 0, 0, 3, 0, 0, 0, 0, 3, 0, 0, 0, 0, 2, 2, 0, 5, 0, 0, 3, 0, 0, 0, 0, 7,
  
        0, 0, 0, 0, 3, 0, 0, 0, 0, 5, 0, 0, 0, 0, 1, 1, 0, 5, 0, 102, 0, 0, 5, 0, 0, 102,
  
        0, 0, 9, 0, 0, 1, 1, 5, 6, 0, 0, 1, 1, 0, 9, 0, 102, 0, 0, 5, 5, 0, 102, 0, 0, 5, // 27
  
        5, 0, 0, 1, 1, 8, 4, 0, 0, 0, 7, 0, 0, 0, 3, 0, 0, 2, 2, 4, 0, 8, 8, 4, 0, 0,
  
        1, 1, 5, 0, 1, 1, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 0, 0, 0, 2, 2,
  
        0, 4, 0, 0, 31, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 30
  
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
   
        0, 0, 0, 0, 0, 0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Medium", score_database2, "Medium");
    }
}
