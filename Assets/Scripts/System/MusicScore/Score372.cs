﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score372 : MonoBehaviour
{
    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 52, 0, 0, 0, 0, 0, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0,
   
        0, 0, 0, 0, 1, 0, 5, 0, 9, 0, 0, 0, 2, 0, 0, 0, 8, 0, 0, 0, 1, 0, 4, 0, 0, 1,
  
        0, 4, 0, 0, 5, 0, 0, 0, 2, 0, 0, 4, 0, 0, 8, 0, 0, 4, 0, 0, 0, 1, 0, 1, 0, 5, // 3
   
        0, 0, 1, 0, 1, 0, 6, 0, 0, 0, 5, 0, 9, 0, 0, 1, 0, 0, 0, 5, 0, 5, 0, 0, 0, 2,
   
        0, 4, 0, 0, 8, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 2, 0, 0, 0, 5, 0, 0, 1,
  
        0, 0, 8, 0, 4, 0, 5, 0, 0, 1, 0, 0, 4, 0, 4, 0, 0, 8, 0, 4, 0, 0, 1, 0, 0, 5, // 6
   
        0, 9, 0, 0, 1, 0, 0, 0, 5, 0, 9, 0, 0, 5, 0, 0, 0, 53, 0, 0, 0, 0, 0, 0, 0, 53,
   
        0, 0, 0, 5, 0, 0, 0, 1, 0, 0, 0, 5, 0, 9, 0, 0, 0, 4, 0, 0, 4, 0, 0, 4, 0, 0,
   
        4, 0, 0, 0, 1, 0, 5, 0, 0, 1, 0, 6, 0, 0, 1, 0, 0, 5, 0, 0, 0, 93, 0, 0, 0, 93, // 9
   
        0, 0, 0, 53, 0, 0, 4, 0, 0, 0, 1, 0, 0, 0, 5, 0, 0, 0, 53, 0, 4, 0, 0, 4, 0, 0,
   
        0, 1, 0, 0, 53, 0, 0, 4, 0, 0, 1, 0, 1, 0, 4, 0, 0, 4, 0, 0, 5, 0, 4, 0, 5, 0,
   
        9, 0, 0, 0, 2, 0, 2, 0, 4, 0, 8, 0, 8, 0, 5, 0, 0, 4, 0, 4, 0, 5, 0, 5, 0, 0, // 12
  
        7, 0, 0, 2, 0, 8, 0, 2, 0, 8, 0, 0, 4, 0, 1, 0, 1, 0, 5, 0, 0, 1, 0, 1, 0, 6,
  
        0, 5, 0, 0, 5, 0, 0, 7, 0, 0, 7, 0, 2, 0, 2, 0, 4, 0, 8, 0, 8, 0, 0, 0, 4, 0,
  
        4, 0, 9, 0, 9, 0, 1, 0, 4, 0, 1, 0, 4, 0, 0, 5, 0, 0, 0, 53, 0, 4, 0, 53, 0, 4, // 15
  
        0, 53, 0, 4, 0, 1, 0, 1, 0, 5, 0, 9, 0, 0, 7, 0, 0, 7, 0, 0, 1, 0, 1, 0, 5, 0,
  
        2, 0, 4, 0, 8, 0, 4, 0, 0, 5, 0, 103, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 93, 0, 5,
  
        0, 1, 0, 93, 0, 5, 0, 4, 0, 4, 0, 1, 0, 5, 0, 6, 0, 53, 0, 53, 0, 53, 0, 0, 4, 0, // 18
  
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 2, 0, 8, 0, 1, 0, 1, 0, 5, 0, 0, 0,
  
        4, 0, 53, 0, 53, 0, 4, 0, 20, 0, 0, 0, 1, 0, 1, 0, 5, 0, 0, 0, 7, 0, 0, 0, 7, 0,
  
        0, 0, 2, 0, 8, 0, 4, 4, 0, 14, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 21
  
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
   
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 14, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 1, 1, 5, 0, 9, 0, 0, 0, 1, 1, 1, 0, 5, 0, 9, 0, 0, 1, 4, 0, 1, 4,

        0, 1, 1, 1, 5, 0, 0, 0, 2, 2, 0, 4, 0, 8, 8, 0, 4, 0, 2, 8, 2, 8, 0, 4, 0, 2, // 3
   
        8, 2, 8, 4, 0, 0, 1, 0, 1, 0, 5, 0, 9, 0, 0, 1, 0, 1, 0, 5, 0, 5, 0, 0, 2, 2,

        0, 4, 0, 8, 8, 0, 4, 5, 0, 0, 0, 0, 0, 0, 0, 1, 0, 2, 2, 0, 4, 0, 5, 0, 0, 1,

        0, 8, 8, 0, 4, 0, 5, 0, 0, 1, 0, 2, 2, 0, 4, 0, 8, 8, 0, 4, 0, 0, 1, 1, 0, 5, // 6
   
        0, 9, 0, 0, 1, 1, 1, 0, 5, 0, 9, 0, 0, 5, 0, 0, 0, 53, 0, 0, 0, 5, 0, 0, 0, 53,

        0, 0, 0, 9, 0, 0, 0, 1, 0, 1, 0, 5, 0, 9, 0, 0, 0, 53, 0, 0, 4, 0, 0, 53, 0, 0,

        4, 0, 0, 0, 1, 1, 5, 0, 0, 1, 1, 6, 0, 0, 1, 1, 1, 5, 0, 0, 0, 7, 0, 0, 0, 7, // 9
   
        0, 1, 0, 53, 0, 0, 4, 0, 0, 1, 0, 1, 0, 5, 0, 9, 0, 0, 53, 0, 53, 0, 0, 5, 0, 0,

        0, 1, 0, 0, 53, 0, 0, 9, 0, 0, 1, 0, 1, 0, 5, 5, 0, 4, 0, 5, 5, 0, 4, 0, 5, 0,

        9, 0, 0, 0, 1, 0, 1, 0, 5, 0, 1, 0, 1, 0, 9, 0, 0, 54, 0, 20, 0, 55, 0, 20, 0, 0, // 12
  
        7, 0, 0, 1, 1, 5, 0, 1, 1, 9, 0, 0, 53, 0, 1, 0, 8, 0, 5, 0, 0, 53, 0, 53, 0, 2,

        2, 4, 8, 0, 5, 0, 0, 7, 0, 0, 7, 0, 1, 0, 5, 0, 9, 0, 1, 0, 5, 0, 9, 0, 0, 55,

        20, 0, 54, 20, 0, 1, 5, 1, 9, 0, 2, 8, 0, 4, 0, 4, 5, 0, 0, 53, 20, 54, 0, 5, 0, 9, // 15
  
        0, 53, 55, 20, 0, 1, 1, 1, 0, 5, 0, 9, 0, 0, 7, 0, 0, 7, 0, 0, 1, 0, 1, 0, 5, 0,

        0, 2, 4, 8, 0, 9, 0, 1, 1, 5, 0, 14, 0, 0, 7, 0, 0, 0, 0, 0, 0, 1, 1, 6, 0, 5,

        0, 1, 1, 6, 0, 5, 0, 53, 0, 53, 0, 1, 1, 1, 0, 5, 0, 53, 0, 54, 0, 53, 0, 0, 5, 0, // 18
  
        0, 7, 0, 0, 7, 0, 7, 0, 0, 7, 0, 0, 1, 0, 1, 0, 5, 0, 2, 2, 0, 4, 0, 8, 8, 0,

        4, 0, 0, 53, 20, 0, 0, 53, 20, 0, 0, 1, 1, 1, 1, 0, 5, 0, 0, 0, 7, 0, 0, 0, 7, 0,

        0, 0, 1, 1, 6, 0, 1, 1, 5, 0, 4, 4, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 21
  
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Medium", score_database2, "Medium");
    }
}