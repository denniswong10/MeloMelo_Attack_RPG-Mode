﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score267 : MonoBehaviour
{
    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 0, 2, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 0, 0, 0,
 
        0, 0, 2, 0, 0, 0, 0, 0, 8, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 8, 0, 0, 0, 0, 0,
 
        2, 0, 5, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, // 3
 
        0, 0, 0, 0, 5, 0, 0, 0, 1, 0, 0, 0, 4, 0, 0, 0, 5, 0, 0, 0, 1, 0, 0, 0, 4, 0,
  
        0, 5, 0, 0, 4, 0, 0, 5, 0, 0, 1, 0, 0, 1, 0, 0, 4, 0, 0, 1, 0, 0, 1, 0, 0, 4,
 
        0, 0, 1, 0, 0, 1, 0, 0, 4, 0, 0, 1, 0, 0, 1, 0, 0, 4, 0, 0, 1, 0, 0, 1, 0, 0, // 6
  
        5, 0, 0, 0, 2, 0, 0, 0, 8, 0, 0, 0, 2, 0, 0, 0, 8, 0, 0, 2, 0, 0, 2, 0, 0, 5,
  
        0, 0, 0, 1, 0, 0, 0, 4, 0, 0, 0, 1, 0, 0, 0, 5, 0, 0, 0, 1, 0, 0, 1, 0, 0, 5,
   
        0, 0, 2, 0, 0, 8, 0, 0, 2, 0, 0, 4, 0, 2, 0, 2, 0, 8, 0, 0, 5, 0, 0, 2, 0, 0, // 9
  
        8, 0, 0, 2, 0, 0, 4, 0, 8, 0, 8, 0, 2, 0, 0, 5, 0, 0, 1, 0, 0, 4, 0, 2, 8, 0,
   
        5, 0, 0, 0, 0, 0, 0, 0, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 0,
   
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 0, 0, 0, 0, 0, 0, 0, 0, 8, 0, 0, // 12
   
        0, 0, 0, 0, 0, 0, 2, 0, 0, 8, 0, 0, 2, 0, 0, 5, 0, 0, 2, 2, 0, 4, 0, 8, 0, 5,
   
        0, 0, 1, 0, 0, 1, 0, 0, 5, 0, 0, 2, 0, 2, 0, 4, 0, 0, 1, 0, 0, 1, 0, 0, 5, 0,
  
        0, 8, 0, 8, 0, 4, 0, 0, 0, 5, 0, 0, 0, 5, 0, 0, 1, 0, 0, 1, 0, 0, 5, 0, 2, 0, // 15
  
        8, 0, 0, 0, 5, 0, 0, 1, 0, 0, 1, 0, 0, 2, 0, 8, 0, 2, 0, 0, 4, 0, 2, 0, 8, 0,
  
        2, 0, 0, 1, 0, 0, 1, 0, 0, 4, 0, 0, 5, 0, 0, 1, 0, 0, 1, 0, 0, 5, 0, 0, 1, 0,
  
        0, 8, 8, 0, 4, 0, 2, 2, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 1, // 18
   
        0, 0, 8, 0, 8, 0, 5, 0, 0, 1, 0, 4, 0, 2, 2, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0,
   
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 0, 2, 0, 2, 0, 2, 0, 4, 0, 8, 0, 8, 0, 8, 0, 6, 0, 1, 0, 1, 0, 2, 0, 2, 0,

        8, 0, 8, 0, 0, 4, 0, 0, 2, 0, 0, 2, 0, 0, 4, 0, 0, 8, 0, 0, 8, 0, 0, 6, 0, 1,

        1, 2, 5, 0, 0, 0, 1, 0, 0, 0, 5, 0, 0, 0, 1, 0, 0, 0, 5, 0, 0, 2, 0, 2, 0, 8, // 3
 
        0, 2, 0, 1, 8, 8, 5, 0, 7, 0, 0, 1, 1, 5, 0, 0, 2, 0, 4, 0, 8, 0, 0, 1, 1, 5,

        0, 1, 1, 6, 0, 2, 0, 8, 0, 5, 0, 1, 1, 2, 0, 1, 1, 8, 0, 5, 0, 0, 7, 0, 0, 3,

        0, 0, 7, 0, 0, 3, 0, 0, 1, 2, 1, 8, 0, 0, 5, 0, 0, 3, 0, 0, 5, 0, 2, 2, 0, 4, // 6
  
        5, 0, 0, 0, 1, 0, 1, 0, 5, 0, 0, 2, 2, 8, 0, 4, 0, 2, 8, 2, 0, 0, 4, 0, 0, 5,

        0, 8, 0, 1, 0, 2, 0, 1, 0, 8, 0, 4, 2, 1, 0, 5, 0, 0, 0, 1, 2, 1, 5, 0, 0, 5,

        0, 2, 2, 0, 1, 0, 2, 2, 8, 0, 1, 0, 8, 2, 2, 0, 1, 2, 1, 0, 5, 0, 0, 6, 0, 0, // 9
  
        5, 0, 0, 3, 0, 2, 2, 4, 0, 0, 3, 0, 8, 8, 4, 0, 3, 2, 0, 8, 0, 4, 0, 1, 2, 1,

        5, 0, 0, 0, 7, 0, 2, 0, 8, 0, 2, 0, 2, 0, 2, 0, 8, 0, 2, 0, 2, 0, 2, 0, 3, 0,

        1, 1, 2, 0, 1, 1, 8, 0, 0, 5, 0, 2, 0, 1, 1, 5, 0, 8, 0, 1, 1, 5, 0, 2, 0, 0, // 12
   
        1, 0, 1, 0, 2, 0, 4, 0, 8, 8, 4, 0, 1, 2, 1, 5, 0, 2, 2, 8, 0, 5, 0, 8, 8, 2,

        0, 4, 1, 1, 0, 2, 8, 0, 4, 0, 2, 2, 0, 4, 0, 8, 8, 0, 1, 1, 0, 8, 4, 8, 5, 0,

        0, 3, 0, 1, 1, 2, 0, 0, 4, 0, 0, 1, 1, 8, 0, 0, 4, 0, 1, 1, 5, 0, 8, 0, 5, 0, // 15
  
        2, 0, 4, 0, 2, 2, 1, 5, 0, 0, 1, 2, 1, 5, 0, 7, 0, 1, 1, 2, 8, 0, 5, 0, 2, 0,

        5, 0, 2, 1, 0, 8, 1, 0, 2, 1, 8, 0, 5, 0, 0, 3, 2, 0, 7, 0, 0, 3, 8, 0, 7, 0,

        0, 1, 1, 0, 2, 0, 1, 1, 8, 0, 5, 0, 0, 0, 7, 0, 0, 0, 5, 0, 0, 0, 7, 0, 0, 1, // 18
   
        2, 1, 8, 0, 4, 2, 5, 0, 2, 1, 2, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Small", score_database2, "Medium");
    }
}