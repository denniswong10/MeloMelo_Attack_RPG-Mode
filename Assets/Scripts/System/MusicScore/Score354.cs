﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score354 : MonoBehaviour
{
    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 0, 2, 0, 0, 4, 0, 0, 8, 0, 0, 2, 2, 0, 8, 0, 0, 4, 0, 0, 5, 0, 2, 4, 0, 8,
   
        4, 0, 5, 5, 0, 0, 2, 2, 0, 8, 8, 0, 5, 0, 0, 5, 0, 0, 2, 2, 0, 4, 0, 8, 8, 0,
   
        4, 0, 0, 5, 0, 0, 0, 0, 1, 1, 0, 53, 0, 0, 1, 1, 0, 50, 0, 0, 1, 1, 0, 53, 0, 0, // 3

        1, 1, 0, 50, 0, 0, 2, 0, 2, 0, 4, 0, 8, 0, 8, 0, 0, 5, 0, 0, 1, 1, 0, 2, 4, 0,
   
        0, 1, 1, 0, 8, 4, 0, 0, 5, 0, 0, 2, 8, 0, 5, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 2,
   
        2, 0, 4, 0, 0, 1, 0, 0, 8, 8, 0, 4, 0, 0, 1, 0, 2, 2, 0, 8, 0, 4, 0, 0, 5, 0, // 6
   
        0, 1, 0, 2, 2, 0, 93, 0, 8, 8, 0, 93, 0, 2, 2, 0, 5, 0, 5, 0, 0, 2, 4, 0, 2, 4,
   
        0, 8, 4, 0, 8, 4, 0, 0, 5, 0, 0, 9, 0, 0, 1, 1, 0, 53, 0, 50, 0, 0, 1, 1, 0, 53,
   
        4, 0, 50, 0, 4, 5, 0, 0, 5, 0, 0, 6, 0, 0, 1, 2, 0, 1, 8, 0, 4, 0, 0, 1, 2, 0, // 9
  
        8, 0, 5, 0, 0, 0, 0, 3, 0, 0, 0, 7, 0, 0, 0, 3, 0, 0, 0, 7, 0, 0, 0, 5, 0, 0,
   
        1, 0, 1, 0, 2, 2, 0, 0, 1, 0, 1, 0, 8, 8, 0, 0, 5, 0, 0, 1, 0, 1, 0, 2, 4, 0,
  
        8, 4, 0, 1, 0, 1, 0, 5, 0, 0, 0, 3, 0, 0, 53, 4, 0, 0, 3, 0, 0, 50, 4, 0, 0, 3, // 12
   
        0, 0, 1, 1, 0, 5, 0, 0, 0, 0, 0, 9, 0, 0, 0, 0, 4, 2, 0, 4, 2, 0, 1, 1, 0, 0,
   
        5, 0, 0, 0, 0, 4, 2, 0, 4, 8, 0, 1, 5, 0, 1, 9, 0, 5, 0, 0, 3, 0, 0, 7, 0, 0,
   
        5, 0, 0, 3, 0, 0, 7, 0, 1, 1, 0, 4, 0, 0, 0, 0, 0, 9, 0, 0, 0, 53, 0, 50, 0, 0, // 15
   
        5, 0, 0, 1, 1, 0, 5, 0, 0, 0, 0, 0, 53, 0, 50, 0, 0, 1, 53, 0, 50, 0, 0, 5, 9, 0,
  
        0, 1, 0, 3, 0, 53, 0, 5, 0, 0, 1, 0, 3, 0, 53, 0, 0, 1, 0, 5, 0, 0, 0, 3, 0, 0,
  
        0, 7, 0, 0, 0, 3, 0, 0, 0, 5, 0, 0, 0, 4, 2, 0, 4, 8, 0, 5, 0, 0, 1, 5, 0, 1, // 18
  
        9, 0, 4, 0, 0, 1, 53, 0, 1, 50, 0, 0, 2, 8, 0, 4, 53, 0, 4, 50, 0, 0, 5, 0, 0, 3,
   
        0, 0, 7, 0, 0, 3, 0, 0, 5, 0, 0, 53, 4, 0, 50, 4, 0, 0, 5, 9, 0, 1, 0, 2, 2, 0,
    
        8, 8, 0, 5, 0, 5, 0, 2, 2, 0, 53, 0, 2, 2, 0, 50, 0, 1, 1, 0, 9, 0, 5, 0, 0, 0, // 21
    
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 0, 5, 0, 0, 2, 4, 0, 8, 4, 0, 1, 2, 8, 5, 0, 4, 2, 0, 4, 8, 0, 2, 2, 4, 8,

        8, 0, 1, 1, 5, 0, 2, 1, 2, 0, 8, 1, 8, 0, 0, 5, 0, 0, 1, 5, 0, 1, 9, 0, 53, 0,

        1, 1, 13, 0, 0, 0, 0, 0, 50, 0, 53, 0, 0, 1, 1, 5, 0, 53, 0, 53, 0, 0, 1, 1, 9, 0, // 3

        0, 1, 1, 4, 0, 1, 1, 6, 0, 1, 1, 4, 0, 1, 1, 6, 0, 5, 0, 0, 2, 4, 0, 8, 4, 0,

        2, 2, 1, 0, 5, 9, 0, 0, 53, 0, 0, 53, 0, 0, 12, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 12,

        0, 0, 5, 0, 0, 1, 0, 0, 13, 0, 0, 5, 0, 0, 1, 1, 2, 0, 1, 1, 8, 4, 0, 0, 5, 0, // 6
   
        0, 1, 2, 0, 1, 8, 0, 5, 0, 5, 0, 1, 2, 2, 2, 0, 1, 8, 8, 8, 0, 2, 4, 0, 8, 4,

        0, 1, 4, 1, 0, 1, 53, 0, 5, 0, 1, 50, 0, 0, 6, 5, 0, 9, 0, 1, 1, 0, 1, 1, 0, 5,

        5, 0, 50, 0, 50, 0, 5, 0, 5, 0, 0, 93, 0, 0, 2, 2, 4, 8, 8, 0, 5, 0, 2, 2, 4, 8, // 9
  
        8, 5, 0, 9, 0, 0, 0, 3, 0, 0, 0, 93, 0, 0, 0, 3, 0, 0, 0, 93, 0, 0, 0, 5, 0, 0,

        1, 1, 8, 0, 2, 4, 2, 0, 1, 1, 4, 0, 8, 4, 8, 0, 5, 0, 0, 1, 1, 5, 0, 1, 1, 9,

        0, 2, 0, 8, 0, 4, 5, 5, 0, 0, 0, 3, 2, 2, 2, 4, 8, 8, 3, 2, 2, 2, 4, 8, 8, 3, // 12
   
        0, 1, 1, 2, 0, 5, 0, 0, 0, 0, 0, 101, 0, 0, 0, 5, 5, 102, 0, 0, 0, 1, 1, 53, 0, 1,

        5, 0, 0, 0, 0, 4, 2, 0, 4, 8, 0, 1, 5, 0, 1, 9, 0, 5, 0, 0, 3, 0, 0, 7, 0, 0,

        1, 5, 0, 2, 2, 0, 8, 8, 0, 1, 1, 6, 0, 0, 0, 0, 0, 102, 0, 0, 0, 5, 5, 101, 0, 0, // 15
   
        0, 1, 1, 50, 0, 0, 9, 0, 0, 0, 0, 1, 4, 0, 2, 4, 0, 5, 5, 0, 9, 0, 50, 0, 0, 101,

        0, 4, 0, 102, 0, 4, 0, 5, 0, 0, 1, 1, 5, 0, 93, 0, 0, 1, 1, 5, 0, 5, 0, 93, 0, 0,

        0, 7, 0, 0, 0, 3, 0, 0, 1, 5, 0, 2, 4, 0, 1, 9, 0, 2, 2, 0, 8, 8, 0, 5, 0, 1, // 18
  
        1, 111, 1, 0, 0, 5, 101, 0, 4, 0, 102, 0, 4, 4, 0, 1, 1, 2, 0, 53, 0, 53, 0, 5, 0, 3,

        0, 2, 2, 0, 3, 0, 8, 8, 5, 0, 0, 1, 1, 0, 5, 9, 0, 1, 1, 5, 0, 1, 1, 9, 0, 2,

        2, 2, 8, 8, 8, 5, 0, 101, 4, 4, 0, 102, 4, 4, 0, 7, 0, 1, 5, 0, 1, 5, 0, 0, 0, 0, // 21
    
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Medium", score_database2, "Medium");
    }
}