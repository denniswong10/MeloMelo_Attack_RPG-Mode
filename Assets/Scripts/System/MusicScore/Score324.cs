﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score324 : MonoBehaviour
{
    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0,
  
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 1, 0, 0, 5, 0, 0, 1, 0, 0,
  
        9, 0, 0, 2, 2, 0, 8, 5, 0, 2, 0, 8, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 2, 0, // Save: 64
  
        2, 0, 4, 0, 5, 0, 0, 0, 1, 0, 0, 0, 2, 0, 4, 0, 8, 0, 4, 0, 0, 0, 1, 0, 0, 0,
  
        5, 0, 2, 2, 0, 5, 0, 8, 8, 0, 0, 5, 0, 0, 1, 0, 0, 1, 0, 0, 2, 8, 0, 5, 0, 0, // Save: 128
  
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, // 6
  
        5, 0, 0, 0, 6, 0, 0, 0, 1, 0, 0, 2, 0, 0, 8, 0, 0, 0, 1, 0, 0, 2, 0, 0, 8, 0,
   
        0, 5, 0, 5, 0, 0, 0, 1, 0, 0, 2, 0, 8, 8, 0, 5, 0, 0, 1, 0, 0, 2, 2, 0, 5, 0,
   
        0, 8, 8, 0, 5, 0, 0, 0, 1, 0, 0, 2, 2, 0, 5, 0, 0, 8, 8, 0, 5, 0, 0, 6, 0, 0, // 9
  
        5, 0, 0, 0, 1, 0, 0, 5, 0, 0, 5, 0, 0, 0, 1, 0, 0, 2, 2, 0, 4, 0, 8, 8, 0, 4,
  
        0, 5, 0, 6, 0, 0, 0, 2, 2, 0, 4, 0, 8, 8, 0, 4, 0, 5, 0, 5, 0, 0, 6, 0, 0, 5, // Save: 264
   
        0, 5, 0, 0, 1, 0, 0, 1, 0, 0, 2, 2, 0, 4, 0, 8, 8, 0, 4, 0, 2, 0, 2, 0, 8, 0, // 12
   
        8, 0, 0, 5, 0, 0, 1, 0, 1, 0, 4, 0, 1, 0, 1, 0, 4, 0, 5, 5, 0, 0, 0, 0, 0, 0, // Save: 332
   
        0, 1, 0, 0, 5, 0, 0, 0, 5, 0, 0, 2, 2, 0, 0, 8, 8, 0, 0, 5, 0, 5, 0, 0, 1, 0,
   
        0, 5, 0, 0, 5, 0, 0, 2, 2, 0, 0, 8, 8, 0, 5, 0, 0, 5, 0, 0, 1, 0, 2, 0, 1, 0, // 15
   
        8, 0, 4, 0, 1, 5, 0, 2, 0, 2, 8, 0, 4, 0, 1, 0, 1, 0, 2, 0, 8, 4, 0, 2, 4, 0,
   
        1, 0, 1, 0, 5, 0, 0, 1, 0, 1, 0, 6, 0, 8, 0, 1, 1, 0, 0, 5, 0, 0, 1, 1, 0, 5,
   
        5, 0, 2, 2, 0, 8, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 5, 0, 0, // 18
   
        8, 0, 8, 0, 5, 0, 0, 1, 0, 0, 1, 0, 0, 5, 5, 0, 2, 0, 2, 0, 4, 0, 8, 0, 8, 0,
   
        0, 5, 0, 0, 5, 0, 0, 1, 1, 0, 0, 5, 0, 0, 5, 0, 0, 2, 2, 0, 0, 1, 0, 5, 0, 1,
   
        0, 5, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 21
   
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
   
        0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 1, 0, 0, 1, 0, 0, 5, 0, 0, 0, 0, 0, 2, 0,

        0, 0, 1, 0, 0, 1, 0, 0, 9, 0, 0, 0, 0, 0, 5, 0, 0, 2, 2, 2, 4, 0, 8, 8, 8, 4,

        0, 9, 0, 1, 1, 1, 0, 5, 0, 5, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 2, 2, 4, // Save: 64
  
        0, 5, 5, 0, 1, 0, 1, 0, 8, 8, 4, 0, 5, 0, 5, 0, 1, 1, 1, 0, 5, 0, 6, 0, 0, 0,

        1, 0, 1, 0, 5, 5, 0, 1, 0, 1, 0, 2, 2, 8, 4, 0, 2, 8, 2, 4, 0, 1, 1, 6, 0, 0, // Save: 128
  
        0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 5, 0, 6, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 6, 0, // 6
  
        0, 0, 6, 0, 0, 0, 1, 1, 1, 0, 2, 2, 2, 0, 8, 8, 8, 0, 5, 0, 5, 6, 0, 0, 2, 0,

        4, 5, 0, 9, 0, 0, 1, 1, 1, 0, 5, 5, 6, 0, 1, 0, 1, 0, 2, 2, 4, 8, 8, 0, 5, 0,

        0, 1, 0, 1, 5, 0, 0, 1, 1, 2, 0, 4, 2, 0, 4, 8, 0, 5, 5, 0, 6, 0, 0, 1, 1, 1, // 9
  
        5, 0, 0, 0, 9, 0, 0, 5, 0, 1, 1, 2, 0, 1, 1, 8, 0, 2, 8, 2, 4, 8, 2, 8, 0, 1,

        1, 5, 5, 9, 0, 0, 3, 2, 2, 0, 3, 8, 8, 8, 0, 1, 5, 1, 0, 9, 1, 9, 0, 2, 8, 5, // Save: 264
   
        0, 6, 0, 1, 1, 1, 0, 5, 0, 1, 1, 1, 0, 2, 2, 8, 8, 5, 0, 2, 2, 8, 8, 0, 1, 1, // 12
   
        1, 0, 6, 0, 1, 1, 1, 0, 5, 0, 2, 2, 8, 0, 4, 0, 5, 9, 5, 6, 0, 0, 0, 0, 0, 0, // Save: 332
   
        0, 7, 1, 1, 9, 0, 3, 0, 5, 0, 5, 0, 7, 6, 1, 5, 0, 3, 0, 5, 0, 5, 0, 0, 1, 1,

        2, 0, 1, 1, 8, 0, 5, 0, 5, 0, 0, 1, 1, 5, 5, 0, 6, 0, 1, 1, 1, 0, 2, 2, 2, 4, // 15
   
        8, 8, 8, 0, 1, 5, 0, 2, 4, 2, 4, 0, 8, 4, 8, 4, 0, 1, 1, 0, 5, 5, 0, 6, 0, 0,

        1, 1, 3, 0, 5, 0, 1, 3, 1, 0, 5, 0, 3, 1, 1, 0, 5, 9, 5, 6, 0, 0, 1, 1, 1, 5,

        9, 0, 2, 2, 8, 8, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 5, 3, 0, 7, 6, 7, // 18
   
        0, 2, 8, 2, 5, 0, 0, 3, 6, 7, 0, 1, 1, 1, 5, 0, 2, 2, 4, 0, 8, 8, 4, 0, 5, 0,

        0, 9, 0, 0, 1, 1, 5, 1, 1, 5, 0, 6, 0, 3, 2, 2, 0, 3, 8, 8, 0, 1, 1, 1, 0, 5,

        0, 6, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 21
   
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Medium", score_database2, "Medium");
    }
}