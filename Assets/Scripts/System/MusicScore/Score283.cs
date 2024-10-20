﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score283 : MonoBehaviour
{
    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 8, 0, 8, 0, 0, 5, 0, 0, 8, 8, 0, 0, 2, 2, 0,
   
        5, 0, 0, 1, 0, 8, 0, 8, 0, 0, 4, 0, 0, 2, 2, 0, 0, 4, 0, 2, 0, 8, 8, 0, 0, 5,
    
        5, 0, 0, 8, 2, 0, 0, 8, 2, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, // 3
  
        0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 6, 0, 0,
   
        0, 1, 0, 0, 2, 0, 8, 0, 0, 1, 0, 0, 1, 0, 0, 5, 0, 0, 0, 1, 0, 0, 1, 0, 0, 8,
  
        0, 2, 0, 4, 0, 0, 0, 1, 0, 0, 0, 5, 0, 0, 0, 1, 0, 0, 2, 0, 2, 0, 0, 4, 0, 0, // 6
   
        8, 0, 8, 0, 0, 4, 0, 0, 1, 0, 0, 1, 0, 0, 5, 0, 5, 0, 0, 0, 1, 0, 0, 8, 2, 0,
   
        0, 8, 2, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,
   
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 9
   
        0, 0, 8, 0, 0, 8, 0, 0, 8, 0, 0, 8, 0, 0, 5, 0, 2, 0, 0, 8, 8, 0, 5, 0, 0, 8,
   
        2, 0, 8, 0, 5, 0, 0, 1, 0, 0, 0, 6, 0, 0, 0, 1, 0, 0, 0, 5, 0, 0, 0, 1, 0, 0,
  
        0, 5, 0, 0, 0, 1, 0, 0, 0, 6, 0, 0, 0, 1, 0, 2, 0, 2, 0, 0, 1, 0, 8, 0, 8, 0, // 12
 
        0, 4, 0, 2, 2, 0, 4, 0, 8, 8, 0, 5, 5, 0, 0, 0, 1, 0, 0, 0, 6, 0, 0, 0, 1, 0,
  
        0, 0, 5, 0, 0, 0, 1, 0, 0, 0, 6, 0, 0, 1, 0, 2, 2, 0, 8, 8, 0, 5, 0, 0, 0, 0,
   
        5, 0, 0, 0, 0, 5, 0, 0, 0, 2, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 15
   
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 5, 0, 0, 5, 0, 0,
   
        0, 1, 0, 0, 5, 0, 0, 2, 2, 0, 8, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 5,
   
        0, 0, 0, 1, 0, 0, 0, 5, 0, 0, 2, 0, 2, 0, 4, 0, 0, 8, 0, 8, 0, 4, 0, 0, 0, 1, // 18
   
        0, 0, 0, 5, 0, 0, 0, 1, 0, 0, 0, 5, 0, 0, 2, 0, 2, 0, 8, 2, 0, 0, 8, 2, 0, 5,
   
        0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
   
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 0, 2, 2, 0, 0, 8, 0, 2, 2, 0, 0, 8, 0, 0, // 21
   
        5, 0, 0, 0, 1, 0, 1, 0, 5, 0, 0, 0, 8, 0, 0, 1, 0, 0, 6, 0, 0, 1, 0, 0, 5, 0,
   
        0, 1, 0, 0, 5, 0, 0, 1, 0, 0, 6, 0, 0, 2, 8, 0, 2, 8, 0, 5, 0, 0, 0, 0, 5, 0,
   
        0, 0, 0, 5, 0, 0, 0, 1, 0, 1, 0, 5, 0, 0, 1, 0, 1, 0, 8, 8, 0, 0, 2, 2, 0, 5, // 24
   
        0, 0, 0, 1, 0, 0, 5, 0, 0, 2, 0, 2, 0, 8, 0, 8, 0, 5, 0, 0, 0, 1, 0, 0, 6, 0,
   
        0, 2, 8, 0, 2, 0, 8, 0, 5, 0, 0, 0, 1, 0, 0, 5, 0, 0, 6, 0, 0, 2, 2, 0, 8, 8,
   
        0, 0, 5, 0, 0, 0, 1, 0, 1, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 27
   
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
   
        0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 5, 0, 1, 0, 8, 5, 0, 0, 1, 0, 5, 0, 2, 2, 0,

        5, 0, 0, 1, 0, 1, 0, 9, 0, 0, 8, 0, 0, 1, 0, 1, 0, 9, 0, 2, 2, 8, 8, 0, 0, 5,

        0, 1, 0, 1, 0, 6, 0, 2, 2, 2, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 8, 0, // 3
  
        0, 2, 0, 5, 0, 0, 8, 0, 0, 8, 0, 1, 0, 0, 8, 0, 0, 2, 0, 5, 0, 0, 1, 1, 5, 0,

        0, 1, 0, 1, 2, 0, 1, 8, 0, 5, 0, 0, 2, 2, 8, 8, 0, 1, 0, 1, 0, 5, 1, 0, 2, 8,

        0, 2, 8, 0, 5, 0, 0, 1, 1, 2, 0, 5, 0, 0, 1, 1, 2, 0, 5, 0, 5, 0, 0, 9, 0, 0, // 6
   
        1, 1, 2, 0, 4, 0, 0, 1, 1, 8, 0, 4, 0, 0, 5, 0, 9, 0, 0, 1, 1, 2, 0, 8, 8, 0,

        1, 1, 2, 2, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3,

        0, 0, 0, 0, 0, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 2, 0, 0, 2, 0, 8, 8, 5, // 9
   
        0, 0, 1, 0, 0, 1, 0, 0, 5, 0, 0, 1, 0, 0, 1, 0, 2, 2, 8, 8, 0, 1, 1, 0, 0, 5,

        0, 5, 0, 0, 1, 0, 5, 5, 0, 0, 1, 1, 0, 5, 0, 5, 0, 6, 0, 2, 2, 0, 1, 1, 5, 0,

        0, 0, 0, 1, 0, 5, 0, 2, 0, 1, 0, 9, 0, 8, 0, 2, 2, 2, 2, 0, 5, 0, 0, 2, 2, 0, // 12
 
        0, 4, 0, 8, 8, 0, 5, 0, 1, 0, 9, 0, 0, 0, 1, 0, 5, 0, 1, 1, 6, 0, 2, 0, 2, 0,

        5, 0, 0, 1, 0, 5, 5, 0, 0, 1, 5, 5, 0, 6, 0, 2, 8, 0, 8, 2, 8, 5, 0, 0, 0, 5,

        5, 0, 0, 0, 5, 2, 2, 0, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 15
   
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 5, 0, 1, 0, 1, 0, 5, 5, 0, 0,

        2, 2, 8, 0, 5, 0, 0, 2, 2, 8, 8, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 5, 0, 0, 1,

        0, 0, 1, 1, 0, 0, 5, 0, 0, 2, 2, 0, 8, 0, 4, 0, 2, 8, 0, 2, 8, 0, 0, 1, 0, 1, // 18
   
        0, 5, 0, 9, 0, 0, 1, 1, 0, 5, 0, 9, 0, 2, 2, 2, 2, 8, 2, 2, 2, 2, 8, 5, 0, 0,

        0, 0, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 2, 5, 2, 2, 9, 2, 2, 5, 2, 2, 2, 8, 8, 2, 2, 2, // 21
   
        5, 0, 0, 1, 5, 0, 1, 9, 0, 0, 8, 0, 8, 0, 1, 1, 0, 0, 6, 0, 0, 2, 2, 0, 4, 0,

        0, 8, 8, 0, 4, 0, 0, 1, 1, 2, 6, 0, 2, 2, 8, 0, 2, 8, 2, 5, 0, 0, 0, 0, 9, 0,

        0, 1, 1, 5, 0, 2, 0, 2, 0, 1, 1, 9, 0, 8, 0, 8, 0, 0, 1, 5, 5, 0, 2, 2, 0, 8, // 24
   
        0, 1, 5, 5, 0, 0, 9, 0, 0, 5, 0, 1, 0, 8, 0, 1, 0, 5, 0, 0, 2, 2, 2, 4, 2, 2,

        2, 6, 2, 2, 2, 4, 5, 0, 0, 0, 1, 1, 1, 1, 5, 0, 0, 1, 1, 1, 1, 6, 0, 2, 8, 8,

        8, 5, 0, 2, 2, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 27
   
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0
    };


    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Medium", score_database2, "Medium");
    }
}
