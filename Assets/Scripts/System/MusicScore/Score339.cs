﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score339 : MonoBehaviour
{
    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 0, 0, 0, 50, 0, 0, 0, 5, 0, 0, 0, 50, 0, 0, 0, 5, 0, 0, 0, 50, 0, 0, 0, 0, 50,
   
        0, 0, 0, 5, 0, 0, 6, 0, 0, 0, 0, 2, 0, 8, 0, 1, 1, 0, 5, 0, 0, 0, 0, 0, 0, 0,
   
        0, 0, 0, 0, 0, 0, 14, 0, 0, 0, 0, 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, // 3
   
        1, 0, 0, 2, 0, 0, 1, 0, 0, 8, 0, 0, 4, 0, 0, 2, 0, 2, 0, 0, 4, 0, 8, 0, 8, 0,
   
        0, 5, 0, 0, 5, 0, 0, 1, 1, 0, 5, 0, 0, 4, 0, 2, 2, 0, 4, 0, 2, 2, 0, 5, 0, 0,
  
        1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, // 6
  
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0,
  
        0, 0, 0, 0, 0, 1, 0, 0, 5, 0, 0, 1, 1, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0,
   
        0, 0, 0, 1, 0, 0, 0, 5, 0, 0, 0, 1, 0, 0, 0, 5, 0, 5, 0, 0, 0, 1, 0, 0, 2, 2, // 9
   
        0, 8, 8, 0, 5, 0, 0, 0, 1, 1, 0, 5, 0, 0, 1, 1, 0, 6, 0, 0, 1, 1, 0, 5, 0, 6,
   
        0, 2, 2, 0, 8, 0, 8, 0, 1, 0, 2, 2, 0, 8, 0, 8, 0, 5, 0, 0, 5, 0, 0, 2, 2, 0,
    
        8, 8, 0, 2, 0, 8, 0, 5, 0, 0, 0, 0, 0, 0, 0, 101, 0, 0, 0, 0, 0, 0, 0, 0, 0, 101, // 12
    
        0, 0, 0, 0, 0, 0, 0, 0, 0, 101, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 4, 0,
   
        0, 0, 2, 0, 8, 0, 5, 0, 5, 0, 0, 8, 8, 0, 5, 0, 0, 6, 0, 0, 0, 5, 0, 0, 0, 5,
  
        0, 0, 0, 5, 0, 0, 0, 1, 1, 0, 2, 0, 2, 0, 0, 1, 1, 0, 8, 0, 8, 0, 5, 0, 0, 0, // 15
  
        4, 0, 0, 0, 4, 0, 0, 0, 2, 2, 0, 5, 0, 0, 8, 8, 0, 5, 0, 0, 1, 0, 1, 0, 5, 0,
  
        0, 1, 0, 1, 0, 6, 0, 0, 1, 0, 6, 0, 1, 0, 5, 0, 0, 2, 2, 0, 0, 5, 0, 0, 0, 0,
  
        50, 0, 0, 0, 50, 0, 0, 0, 5, 0, 0, 0, 50, 0, 0, 0, 0, 50, 0, 0, 0, 0, 5, 0, 0, 1, // 18
   
        1, 0, 2, 0, 8, 0, 0, 1, 1, 0, 8, 0, 2, 4, 0, 8, 4, 0, 5, 0, 0, 0, 0, 0, 0, 0,
   
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 0, 0, 0, 1, 2, 8, 0, 5, 0, 0, 0, 1, 2, 1, 5, 0, 6, 0, 0, 50, 0, 0, 1, 1, 5,

        0, 6, 0, 1, 1, 5, 0, 6, 0, 2, 0, 2, 0, 8, 0, 1, 2, 1, 5, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 1, 8, 0, 1, 4, 1, 6, 0, 0, // 3
   
        5, 0, 0, 2, 2, 5, 0, 8, 8, 5, 0, 0, 4, 0, 0, 1, 1, 2, 0, 1, 1, 8, 0, 5, 5, 0,

        5, 5, 0, 1, 2, 1, 0, 1, 8, 1, 0, 5, 0, 6, 0, 2, 2, 2, 4, 0, 8, 8, 8, 5, 0, 0,

        3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, // 6
  
        0, 8, 0, 8, 0, 0, 0, 0, 0, 0, 3, 0, 2, 2, 0, 8, 0, 8, 0, 0, 0, 0, 1, 1, 5, 0,

        0, 1, 1, 19, 0, 0, 0, 2, 2, 4, 0, 8, 8, 4, 0, 5, 0, 0, 0, 3, 0, 0, 0, 5, 0, 0,

        0, 0, 0, 1, 1, 5, 0, 5, 0, 0, 1, 1, 6, 0, 5, 5, 0, 5, 0, 0, 0, 1, 2, 0, 1, 8, // 9
   
        0, 1, 2, 1, 8, 0, 5, 0, 0, 0, 3, 0, 0, 0, 0, 3, 0, 0, 0, 0, 5, 0, 5, 0, 0, 0,

        1, 2, 8, 0, 5, 0, 0, 0, 1, 1, 2, 8, 0, 5, 0, 1, 1, 5, 0, 1, 1, 19, 0, 2, 2, 0,

        8, 8, 0, 4, 0, 1, 5, 6, 0, 0, 0, 0, 0, 0, 1, 1, 50, 0, 0, 0, 1, 1, 50, 0, 0, 0, // 12
    
        1, 1, 50, 0, 0, 0, 2, 4, 0, 8, 4, 0, 5, 0, 0, 5, 0, 0, 1, 1, 5, 0, 1, 1, 19, 0,

        0, 2, 2, 0, 8, 0, 5, 0, 5, 0, 0, 8, 8, 8, 5, 0, 0, 6, 0, 0, 0, 50, 0, 1, 1, 0,

        50, 0, 1, 1, 0, 50, 0, 1, 1, 0, 5, 0, 0, 2, 4, 2, 8, 0, 1, 2, 1, 8, 0, 5, 0, 0, // 15
  
        4, 2, 0, 4, 8, 0, 4, 2, 8, 0, 5, 0, 0, 1, 1, 0, 0, 5, 0, 0, 1, 1, 6, 0, 5, 0,

        0, 2, 2, 2, 0, 5, 0, 8, 8, 8, 0, 5, 0, 0, 4, 2, 4, 0, 8, 4, 8, 5, 0, 0, 0, 0,

        14, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 0, 8, 8, 8, 0, 5, 0, 5, 0, 0, 4, // 18
   
        2, 0, 4, 8, 0, 50, 0, 1, 1, 0, 50, 0, 1, 1, 0, 8, 1, 8, 5, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Small", score_database2, "Medium");
    }
}
