﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score360 : MonoBehaviour
{
    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 0, 0, 1, 0, 0, 0, 5, 0, 5, 0, 9, 0, 0, 0, 1, 0, 0, 0, 9, 0, 5, 0, 9, 0, 0,
  
        0, 1, 0, 0, 5, 0, 0, 1, 0, 0, 9, 0, 0, 0, 2, 2, 0, 4, 0, 8, 8, 0, 4, 0, 0, 5,
  
        0, 0, 5, 0, 0, 2, 2, 0, 4, 0, 8, 8, 0, 4, 0, 0, 5, 0, 0, 5, 0, 0, 2, 0, 8, 0, // 3
  
        5, 0, 0, 0, 3, 0, 0, 0, 3, 0, 0, 0, 3, 0, 0, 0, 93, 0, 0, 0, 93, 0, 0, 0, 3, 0,
  
        0, 0, 3, 0, 0, 0, 3, 0, 0, 0, 93, 0, 0, 0, 93, 0, 0, 0, 5, 5, 0, 0, 1, 0, 0, 1,
  
        0, 0, 5, 0, 0, 2, 0, 8, 0, 5, 0, 0, 0, 1, 0, 0, 14, 0, 0, 1, 0, 0, 12, 0, 0, 5, // 6
  
        0, 5, 0, 0, 1, 1, 0, 2, 0, 1, 1, 0, 8, 0, 4, 0, 0, 1, 0, 0, 5, 0, 0, 1, 0, 0,
  
        5, 9, 0, 0, 4, 0, 0, 0, 3, 0, 0, 0, 93, 0, 0, 0, 3, 0, 0, 1, 1, 0, 0, 14, 0, 0,
  
        14, 0, 0, 1, 1, 0, 5, 0, 0, 0, 1, 0, 5, 0, 1, 0, 9, 0, 2, 2, 0, 4, 0, 0, 1, 1, // 9
 
        0, 14, 0, 0, 5, 0, 14, 0, 0, 5, 0, 0, 9, 0, 1, 1, 0, 2, 0, 14, 0, 0, 1, 1, 0, 8,
 
        0, 14, 0, 0, 5, 0, 9, 5, 0, 0, 1, 2, 0, 4, 0, 5, 0, 0, 0, 30, 0, 0, 0, 0, 0, 31,
 
        0, 0, 0, 0, 0, 1, 0, 5, 5, 0, 9, 0, 0, 3, 0, 0, 3, 0, 0, 5, 0, 5, 0, 0, 1, 0, // 12
 
        0, 1, 0, 0, 2, 2, 0, 0, 8, 8, 0, 0, 1, 0, 1, 0, 5, 0, 5, 0, 9, 0, 0, 4, 0, 0,
  
        0, 30, 0, 4, 0, 0, 0, 31, 0, 4, 0, 0, 0, 1, 0, 5, 5, 0, 0, 3, 0, 0, 3, 0, 0, 5,
  
        0, 5, 0, 9, 0, 0, 1, 1, 0, 0, 2, 2, 0, 0, 1, 1, 0, 0, 8, 8, 0, 0, 5, 0, 0, 9, // 15
   
        0, 0, 5, 0, 93, 0, 93, 0, 2, 8, 0, 4, 0, 0, 0, 3, 0, 0, 0, 3, 0, 0, 0, 1, 0, 1,
 
        0, 0, 0, 3, 0, 0, 0, 3, 0, 0, 0, 1, 1, 0, 5, 0, 1, 1, 0, 9, 0, 0, 0, 3, 0, 0,
 
        0, 3, 0, 0, 0, 2, 2, 0, 1, 0, 8, 8, 0, 5, 0, 0, 6, 0, 0, 0, 0, 14, 0, 0, 0, 12, // 18
 
        0, 0, 0, 5, 0, 5, 0, 0, 30, 0, 0, 0, 0, 0, 31, 0, 0, 0, 0, 0, 5, 0, 0, 6, 0, 0,
 
        1, 0, 5, 14, 0, 0, 1, 0, 9, 14, 0, 0, 0, 5, 9, 0, 0, 2, 2, 0, 0, 8, 8, 0, 0, 1,
 
        1, 0, 0, 5, 0, 0, 2, 0, 4, 0, 8, 0, 0, 93, 0, 93, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, // 21
  
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 1, 1, 1, 5, 0, 51, 0, 0, 0, 0, 0, 0, 1, 5, 1, 5,

        0, 99, 0, 0, 2, 1, 8, 4, 2, 8, 0, 5, 99, 5, 0, 1, 14, 0, 0, 1, 12, 0, 0, 5, 0, 5,

        0, 2, 14, 0, 0, 12, 0, 0, 5, 0, 1, 1, 2, 0, 4, 2, 4, 0, 1, 1, 8, 0, 4, 8, 4, 0, // 3
  
        5, 0, 0, 3, 2, 0, 0, 3, 8, 0, 0, 1, 1, 1, 5, 0, 99, 0, 0, 53, 0, 0, 14, 0, 0, 53,

        0, 0, 12, 0, 0, 0, 1, 5, 1, 99, 0, 2, 2, 4, 8, 8, 0, 5, 5, 99, 0, 0, 1, 6, 0, 1,

        99, 0, 5, 0, 0, 2, 2, 4, 0, 8, 8, 4, 0, 5, 0, 99, 0, 1, 1, 5, 0, 1, 1, 99, 0, 2, // 6
  
        4, 2, 0, 8, 4, 8, 0, 5, 0, 99, 5, 0, 1, 1, 4, 0, 1, 1, 5, 0, 1, 1, 4, 0, 53, 0,

        53, 0, 1, 1, 5, 0, 3, 0, 1, 1, 99, 0, 3, 0, 4, 2, 2, 0, 4, 8, 8, 0, 14, 0, 0, 14,

        0, 0, 1, 1, 1, 0, 5, 0, 0, 0, 5, 5, 0, 5, 5, 0, 99, 0, 1, 2, 2, 4, 8, 8, 0, 1, // 9
 
        1, 53, 0, 53, 0, 5, 5, 5, 0, 99, 0, 0, 93, 0, 5, 5, 0, 99, 0, 1, 2, 2, 4, 8, 8, 0,

        53, 0, 53, 0, 1, 1, 1, 5, 0, 0, 4, 4, 0, 4, 99, 0, 7, 0, 0, 56, 0, 0, 57, 0, 0, 5,

        5, 0, 1, 1, 5, 0, 1, 1, 99, 0, 56, 0, 0, 56, 0, 0, 57, 0, 0, 57, 0, 0, 5, 0, 99, 0, // 12
 
        1, 1, 5, 0, 2, 2, 0, 1, 1, 5, 0, 8, 8, 0, 1, 5, 0, 1, 99, 0, 5, 0, 0, 93, 0, 0,

        0, 50, 0, 0, 0, 0, 51, 0, 0, 0, 0, 1, 111, 1, 1, 0, 50, 0, 0, 0, 0, 51, 0, 0, 0, 0,

        0, 1, 5, 1, 99, 0, 0, 4, 2, 2, 4, 8, 8, 0, 1, 1, 1, 0, 5, 5, 0, 99, 5, 0, 0, 6, // 15
   
        0, 0, 1, 0, 93, 0, 93, 0, 5, 5, 0, 99, 0, 56, 0, 0, 57, 0, 0, 56, 0, 0, 57, 0, 0, 5,

        5, 0, 0, 56, 0, 0, 57, 4, 0, 56, 0, 0, 57, 4, 0, 1, 1, 1, 0, 5, 0, 0, 6, 2, 2, 2,

        0, 6, 8, 8, 8, 0, 5, 0, 99, 0, 1, 1, 0, 5, 0, 4, 5, 0, 0, 0, 0, 1, 14, 4, 0, 0, // 18
 
        0, 5, 5, 0, 99, 0, 0, 1, 12, 0, 0, 0, 5, 5, 0, 99, 0, 0, 2, 4, 8, 4, 0, 1, 5, 1,

        99, 0, 1, 1, 14, 0, 0, 1, 1, 14, 0, 0, 5, 1, 5, 1, 0, 2, 2, 2, 4, 8, 8, 8, 0, 1,

        5, 0, 1, 99, 0, 2, 2, 4, 0, 8, 8, 4, 0, 3, 0, 3, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, // 21
  
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Medium", score_database2, "Medium");
    }
}
