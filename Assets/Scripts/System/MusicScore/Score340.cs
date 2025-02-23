﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score340 : MonoBehaviour
{
    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 8, 0, 0, 0, 0, 0, 0, 0, 5, 0,
   
        0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 4, 0, 8, 0, 4, 0, 2, 0, 54, 0, 0, 0, 0, 0, 0,
  
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 3
   
        5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5,
 
        0, 0, 0, 1, 0, 5, 0, 0, 0, 1, 0, 2, 0, 5, 0, 0, 0, 1, 0, 6, 0, 0, 0, 1, 0, 8,
  
        0, 5, 0, 0, 0, 1, 0, 0, 4, 0, 0, 1, 0, 0, 2, 2, 0, 0, 1, 0, 0, 2, 0, 0, 8, 0, // 6
  
        0, 5, 0, 0, 1, 0, 1, 0, 2, 0, 4, 0, 0, 0, 2, 2, 0, 5, 0, 0, 2, 2, 0, 4, 0, 0,
  
        2, 8, 0, 4, 0, 0, 5, 0, 0, 8, 2, 0, 4, 0, 0, 5, 0, 0, 0, 1, 1, 0, 5, 0, 0, 1,
  
        1, 0, 5, 0, 0, 2, 0, 4, 0, 8, 0, 4, 0, 0, 2, 0, 4, 0, 8, 8, 0, 5, 0, 0, 0, 3, // 9
  
        0, 0, 2, 2, 0, 5, 0, 0, 0, 3, 0, 0, 1, 0, 2, 2, 0, 1, 0, 5, 0, 0, 0, 3, 0, 0,
  
        2, 0, 4, 0, 8, 0, 4, 0, 5, 0, 0, 0, 3, 0, 0, 1, 1, 0, 2, 0, 1, 1, 0, 8, 0, 5,
   
        0, 0, 0, 14, 0, 0, 0, 0, 0, 5, 0, 0, 5, 0, 0, 14, 0, 0, 0, 0, 0, 5, 0, 0, 5, 0, // 12
   
        0, 1, 0, 2, 0, 8, 0, 6, 0, 0, 0, 1, 0, 1, 0, 5, 0, 0, 1, 0, 1, 0, 6, 0, 0, 2,
        
        8, 0, 8, 0, 0, 5, 0, 1, 1, 0, 4, 0, 0, 0, 3, 0, 2, 2, 0, 4, 0, 5, 0, 0, 0, 3,
   
        0, 8, 8, 0, 4, 0, 2, 4, 0, 103, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 3, 0, 5, 0, 5, // 15
   
        0, 0, 3, 0, 0, 1, 0, 5, 0, 5, 0, 0, 8, 2, 0, 4, 0, 2, 2, 0, 8, 8, 0, 0, 0, 0,
   
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 2, 0, 4, 0, 0, 8, 8,
   
        0, 4, 0, 0, 0, 2, 0, 2, 0, 4, 0, 0, 0, 8, 8, 0, 4, 0, 0, 0, 1, 0, 5, 0, 0, 1, // 18
   
        6, 0, 0, 2, 8, 0, 5, 0, 6, 0, 0, 2, 2, 0, 4, 0, 8, 5, 0, 0, 0, 0, 0, 0, 0, 0,
   
        0, 0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 0, 0, 0, 0, 1, 1, 2, 8, 0, 0, 5, 0, 5, 0, 0, 1, 1, 2, 8, 0, 0, 0, 0, 5, 0,

        0, 1, 1, 5, 0, 16, 0, 1, 1, 5, 0, 2, 2, 4, 8, 8, 4, 2, 2, 5, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, // 3
   
        5, 16, 0, 0, 1, 0, 0, 1, 0, 0, 2, 2, 2, 2, 0, 5, 5, 16, 0, 0, 1, 1, 5, 0, 1, 1,

        16, 0, 0, 0, 0, 5, 0, 1, 0, 1, 0, 2, 2, 5, 0, 8, 8, 5, 0, 16, 0, 0, 1, 1, 2, 8,

        0, 5, 0, 0, 1, 1, 0, 5, 16, 0, 1, 1, 0, 5, 2, 2, 8, 8, 19, 0, 0, 2, 2, 0, 8, 8, // 6
  
        0, 5, 0, 0, 1, 1, 4, 0, 2, 2, 5, 0, 1, 1, 4, 0, 8, 8, 5, 0, 2, 2, 5, 19, 0, 0,

        14, 0, 0, 0, 5, 0, 5, 0, 0, 8, 4, 2, 4, 0, 5, 16, 0, 0, 14, 0, 0, 0, 5, 0, 5, 0,

        0, 2, 2, 4, 8, 8, 4, 5, 0, 19, 0, 0, 1, 1, 2, 0, 4, 0, 1, 1, 8, 5, 0, 0, 0, 2, // 9
  
        2, 2, 4, 8, 8, 5, 0, 5, 0, 13, 2, 2, 4, 2, 2, 4, 8, 8, 8, 5, 0, 5, 0, 13, 2, 2,

        4, 8, 8, 4, 8, 8, 8, 8, 5, 0, 5, 0, 13, 2, 2, 4, 8, 8, 4, 2, 2, 4, 8, 5, 0, 5,

        0, 0, 0, 54, 0, 0, 0, 5, 0, 54, 0, 0, 0, 5, 0, 0, 1, 1, 2, 0, 5, 5, 0, 1, 1, 8, // 12
   
        0, 5, 5, 19, 0, 0, 1, 4, 1, 0, 2, 4, 2, 0, 5, 0, 5, 0, 1, 16, 1, 0, 5, 0, 5, 0,

        2, 4, 8, 4, 0, 16, 0, 1, 5, 1, 19, 0, 0, 0, 54, 0, 17, 2, 8, 4, 0, 5, 0, 0, 0, 54,

        0, 17, 2, 2, 4, 8, 8, 5, 0, 103, 0, 0, 0, 0, 0, 0, 0, 1, 1, 5, 0, 13, 0, 1, 1, 16, // 15
   
        0, 17, 0, 1, 5, 1, 19, 0, 5, 5, 0, 2, 8, 4, 2, 8, 4, 2, 5, 0, 8, 4, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 5, 0, 0, 0, 0, 0, 0, 0, 1, 1, 5, 0, 2, 4, 8,

        4, 5, 0, 0, 1, 1, 0, 5, 0, 0, 1, 1, 5, 19, 0, 4, 2, 2, 0, 4, 8, 8, 5, 0, 0, 1, // 18
   
        16, 0, 1, 19, 0, 1, 1, 5, 5, 0, 1, 1, 5, 19, 0, 2, 8, 5, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Small", score_database2, "Medium");
    }
}
