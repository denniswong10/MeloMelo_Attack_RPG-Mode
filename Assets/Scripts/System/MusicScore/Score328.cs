﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score328 : MonoBehaviour
{
    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 0, 0, 2, 0, 0, 8, 0, 8, 0, 5, 0, 0, 0, 2, 0, 0, 0, 8, 0, 8, 0, 0, 5, 0, 0,
  
        0, 0, 6, 0, 0, 2, 2, 0, 5, 0, 0, 8, 8, 0, 5, 0, 0, 2, 2, 4, 0, 5, 0, 0, 0, 0,
  
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 3
  
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
 
        0, 2, 8, 0, 2, 8, 0, 0, 4, 0, 0, 2, 8, 0, 5, 0, 0, 1, 5, 0, 1, 9, 0, 2, 0, 2,
  
        0, 4, 0, 8, 0, 8, 0, 5, 6, 0, 0, 1, 1, 0, 5, 0, 2, 4, 0, 5, 0, 1, 1, 0, 9, 0, // 6
  
        5, 0, 2, 2, 4, 8, 5, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 6, 0, 0, 0, 0, 5,
  
        0, 0, 0, 0, 6, 0, 0, 0, 0, 2, 0, 2, 0, 4, 0, 0, 0, 0, 8, 0, 8, 0, 4, 0, 0, 0,
  
        0, 1, 0, 0, 0, 6, 0, 0, 0, 1, 0, 0, 0, 6, 0, 0, 0, 1, 1, 0, 5, 0, 9, 0, 0, 0, // 9
  
        0, 0, 2, 2, 0, 4, 0, 0, 0, 8, 8, 0, 4, 0, 0, 0, 5, 0, 0, 0, 6, 0, 0, 0, 5, 0,
  
        0, 0, 1, 0, 1, 0, 4, 0, 1, 0, 1, 0, 6, 0, 2, 2, 4, 0, 8, 8, 4, 0, 5, 0, 0, 0,
  
        0, 0, 0, 0, 5, 0, 0, 2, 0, 0, 8, 0, 0, 4, 0, 0, 0, 2, 2, 0, 8, 8, 0, 4, 0, 0, // 12
 
        0, 2, 4, 0, 8, 4, 0, 5, 0, 0, 0, 2, 0, 2, 0, 4, 0, 8, 0, 8, 0, 4, 0, 5, 5, 0,
 
        0, 0, 0, 9, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 8, 0, 0,
  
        0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 5, 0, 5, 0, 0, 0, 1, // 15
  
        0, 0, 1, 0, 0, 5, 0, 5, 0, 0, 2, 8, 0, 4, 0, 5, 0, 0, 0, 2, 2, 2, 0, 0, 0, 5,
  
        0, 0, 0, 8, 8, 8, 0, 0, 0, 5, 0, 0, 0, 2, 4, 2, 0, 0, 0, 5, 0, 0, 0, 8, 4, 8,
  
        0, 0, 0, 5, 0, 0, 0, 1, 0, 0, 2, 0, 0, 4, 0, 0, 0, 1, 0, 0, 8, 0, 0, 4, 0, 0, // 18
  
        0, 2, 0, 2, 0, 4, 0, 0, 0, 8, 0, 8, 0, 4, 0, 0, 0, 2, 0, 4, 0, 2, 0, 0, 0, 8,
  
        0, 4, 0, 8, 0, 0, 0, 1, 1, 0, 5, 0, 5, 0, 0, 0, 1, 1, 0, 5, 0, 5, 0, 0, 2, 8,
  
        4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 21
  
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5,
  
        5, 0, 0, 0, 0, 0, 1, 0, 0, 5, 0, 0, 1, 0, 0, 9, 0, 0, 2, 2, 0, 8, 8, 0, 0, 0,
  
        1, 0, 0, 5, 0, 0, 1, 0, 0, 9, 0, 0, 2, 2, 0, 8, 8, 0, 5, 0, 0, 0, 1, 0, 0, 5, // 24
  
        0, 2, 2, 0, 1, 0, 0, 9, 0, 8, 8, 0, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0,
  
        0, 1, 0, 1, 0, 5, 0, 0, 0, 1, 0, 1, 0, 9, 0, 0, 0, 1, 0, 1, 0, 5, 0, 2, 2, 0,
  
        8, 8, 0, 0, 0, 1, 0, 1, 0, 9, 0, 2, 2, 0, 8, 8, 0, 0, 0, 1, 0, 0, 9, 0, 0, 1, // 27
  
        0, 0, 5, 0, 0, 4, 0, 0, 0, 2, 2, 4, 0, 8, 8, 4, 0, 5, 0, 5, 0, 0, 4, 0, 0, 2,
  
        2, 4, 0, 8, 8, 4, 0, 5, 0, 5, 0, 0, 9, 0, 0, 2, 8, 4, 0, 2, 8, 4, 0, 5, 0, 5,
   
        0, 0, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 30
   
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 0, 0, 2, 4, 2, 4, 0, 5, 0, 5, 0, 0, 8, 4, 8, 4, 0, 5, 0, 5, 0, 0, 1, 1, 2,

        0, 1, 1, 8, 0, 5, 0, 5, 0, 0, 2, 4, 8, 4, 0, 5, 0, 8, 4, 2, 4, 5, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 3
  
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0,

        0, 2, 4, 8, 8, 8, 0, 5, 0, 5, 0, 0, 8, 4, 2, 2, 2, 0, 5, 0, 5, 0, 0, 4, 2, 4,

        0, 4, 8, 4, 0, 1, 1, 5, 0, 0, 1, 1, 9, 0, 0, 2, 2, 4, 8, 8, 4, 5, 0, 5, 0, 1, // 6
  
        1, 0, 1, 1, 0, 5, 9, 0, 0, 0, 0, 0, 0, 2, 8, 5, 0, 0, 6, 0, 0, 8, 2, 5, 0, 0,

        9, 0, 0, 4, 2, 2, 2, 0, 4, 8, 8, 8, 0, 5, 0, 0, 5, 0, 0, 1, 2, 1, 8, 0, 0, 4,

        2, 4, 8, 0, 0, 5, 0, 0, 5, 0, 0, 1, 1, 2, 0, 1, 1, 8, 0, 5, 5, 0, 6, 0, 0, 0, // 9
  
        0, 0, 5, 2, 2, 4, 0, 5, 8, 8, 8, 4, 0, 1, 0, 1, 0, 5, 0, 0, 2, 4, 2, 0, 5, 0,

        0, 8, 4, 8, 0, 5, 0, 0, 2, 2, 4, 8, 8, 0, 5, 0, 2, 2, 4, 8, 8, 0, 5, 0, 0, 0,

        0, 0, 0, 0, 1, 1, 2, 2, 0, 8, 8, 0, 5, 9, 0, 0, 1, 1, 8, 8, 0, 2, 2, 0, 5, 9, // 12
 
        0, 4, 0, 0, 4, 0, 0, 2, 2, 4, 8, 8, 0, 2, 2, 4, 8, 8, 0, 5, 5, 0, 0, 5, 5, 0,

        0, 0, 1, 1, 5, 0, 0, 2, 0, 0, 8, 0, 0, 2, 0, 0, 1, 1, 9, 0, 0, 2, 0, 0, 8, 0,

        0, 2, 0, 0, 4, 2, 2, 0, 4, 8, 8, 0, 5, 0, 5, 0, 0, 2, 2, 4, 0, 5, 0, 8, 8, 4, // 15
  
        0, 5, 0, 0, 1, 5, 0, 2, 4, 2, 2, 0, 5, 5, 0, 1, 2, 2, 0, 1, 8, 8, 0, 0, 0, 5,

        0, 0, 0, 3, 0, 0, 0, 0, 3, 0, 0, 0, 0, 3, 0, 0, 0, 0, 3, 0, 0, 0, 0, 3, 0, 0,

        0, 0, 1, 2, 2, 0, 1, 8, 8, 0, 5, 0, 0, 4, 0, 0, 0, 1, 1, 0, 5, 0, 0, 1, 1, 0, // 18
  
        0, 9, 0, 0, 2, 4, 0, 0, 2, 8, 0, 0, 2, 4, 0, 0, 8, 4, 0, 0, 5, 5, 0, 0, 1, 0,

        1, 0, 5, 0, 1, 0, 1, 0, 9, 0, 0, 2, 2, 4, 8, 8, 0, 5, 0, 5, 0, 2, 2, 4, 8, 8,

        4, 0, 0, 0, 0, 7, 0, 0, 0, 0, 7, 0, 0, 0, 0, 7, 0, 0, 0, 0, 7, 0, 0, 0, 0, 4, // 21
  
        0, 0, 0, 0, 7, 0, 0, 0, 0, 4, 0, 0, 0, 0, 7, 0, 0, 0, 0, 4, 0, 0, 0, 0, 5, 5,

        0, 5, 5, 0, 0, 0, 1, 1, 2, 2, 0, 8, 8, 0, 5, 9, 0, 0, 1, 1, 8, 8, 0, 2, 2, 0,

        5, 9, 0, 0, 1, 0, 1, 0, 5, 9, 0, 1, 2, 2, 0, 1, 8, 8, 0, 5, 9, 0, 0, 1, 0, 5, // 24
  
        0, 1, 0, 9, 0, 2, 2, 4, 8, 8, 5, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 0,

        0, 1, 1, 5, 0, 5, 0, 0, 0, 1, 1, 9, 0, 9, 0, 0, 4, 2, 2, 0, 4, 8, 8, 0, 5, 0,

        5, 0, 0, 6, 0, 0, 1, 1, 1, 0, 5, 0, 5, 0, 0, 1, 1, 1, 0, 5, 0, 5, 0, 0, 0, 9, // 27
  
        2, 2, 5, 0, 0, 4, 2, 0, 4, 8, 0, 4, 2, 8, 0, 4, 2, 8, 0, 5, 0, 0, 9, 0, 0, 5,

        2, 2, 0, 5, 8, 8, 0, 0, 4, 0, 0, 1, 2, 1, 8, 0, 5, 0, 1, 2, 8, 5, 0, 0, 9, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 30
   
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Medium", score_database2, "Medium");
    }
}