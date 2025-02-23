﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score406 : MonoBehaviour
{
    // 67, 68, 72, 73, 51, 42, 43

    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 1, 0, 5, 0, 1, 0, 5, 0, 1, 1, 0, 5, 0, 6, 0, 0, 0, 1, 1, 0, 5, 1, 0, 6, 1,

        0, 5, 1, 0, 9, 0, 0, 0, 0, 0, 42, 0, 0, 2, 2, 0, 8, 0, 5, 0, 0, 43, 0, 0, 8, 0,

        2, 0, 5, 9, 0, 0, 1, 5, 0, 5, 1, 0, 9, 1, 0, 2, 4, 0, 8, 4, 0, 1, 1, 0, 5, 0, // 3
  
        0, 9, 0, 0, 72, 0, 0, 1, 1, 0, 5, 5, 0, 2, 2, 0, 8, 8, 0, 0, 0, 0, 73, 0, 0, 1,

        5, 0, 1, 9, 0, 1, 1, 0, 5, 5, 0, 0, 9, 0, 0, 2, 5, 0, 6, 0, 8, 5, 0, 9, 0, 0,

        42, 0, 42, 0, 0, 43, 0, 43, 0, 0, 5, 0, 0, 1, 0, 5, 0, 1, 0, 5, 0, 9, 0, 0, 0, 93, // 6
  
        0, 93, 0, 2, 0, 2, 0, 2, 0, 8, 0, 8, 0, 8, 0, 93, 0, 93, 0, 2, 2, 0, 8, 8, 0, 5,

        0, 93, 0, 93, 0, 0, 1, 1, 0, 5, 5, 0, 9, 0, 42, 42, 0, 43, 0, 43, 0, 9, 0, 43, 0, 5,

        1, 0, 5, 0, 9, 0, 0, 3, 0, 3, 0, 0, 72, 0, 73, 0, 9, 0, 1, 1, 0, 5, 0, 6, 0, 0, // 9
  
        7, 0, 0, 73, 0, 72, 0, 9, 0, 1, 1, 0, 5, 0, 6, 0, 7, 0, 2, 2, 0, 5, 5, 0, 8, 8,

        0, 9, 0, 42, 42, 0, 43, 43, 0, 1, 1, 0, 9, 0, 5, 0, 6, 0, 0, 3, 0, 3, 0, 0, 67, 0,

        0, 42, 42, 0, 43, 9, 0, 0, 0, 1, 5, 0, 6, 0, 68, 0, 0, 42, 42, 0, 43, 1, 0, 5, 9, 0, // 12
  
        0, 7, 0, 42, 43, 0, 5, 9, 0, 42, 43, 0, 5, 0, 9, 0, 0, 0, 1, 0, 5, 0, 1, 0, 6, 0,

        72, 0, 0, 7, 0, 0, 7, 0, 1, 0, 5, 9, 0, 42, 42, 0, 1, 0, 9, 5, 0, 43, 43, 0, 1, 5,

        0, 1, 6, 0, 42, 43, 0, 43, 42, 0, 6, 0, 0, 0, 72, 0, 0, 72, 0, 0, 0, 73, 0, 0, 73, 0, // 15
   
        0, 1, 1, 0, 5, 0, 0, 7, 0, 0, 7, 0, 0, 3, 0, 0, 3, 0, 0, 0, 1, 0, 2, 0, 1, 0,

        8, 0, 0, 0, 0, 0, 0, 0, 0, 67, 0, 0, 68, 0, 0, 51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 18
   
        0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 2, 8, 5, 0, 1, 1, 6, 0, 2, 8, 5, 5, 0, 6, 0, 0, 0, 1, 72, 0, 1, 1, 73, 0, 1,
  
        1, 5, 5, 0, 4, 0, 0, 0, 0, 0, 1, 0, 0, 2, 2, 4, 0, 0, 5, 0, 0, 1, 0, 0, 8, 8,
  
        4, 0, 0, 5, 0, 0, 1, 1, 0, 5, 0, 1, 1, 0, 9, 0, 0, 2, 2, 4, 2, 0, 8, 8, 4, 8, // 3
  
        0, 5, 0, 0, 1, 1, 2, 0, 4, 0, 1, 1, 8, 0, 4, 0, 5, 5, 0, 0, 0, 0, 42, 2, 4, 0,
  
        0, 5, 0, 0, 1, 0, 0, 42, 8, 4, 0, 0, 5, 0, 0, 6, 1, 0, 5, 0, 6, 1, 0, 9, 0, 0,

        42, 2, 42, 4, 0, 42, 8, 42, 8, 0, 9, 0, 0, 1, 1, 5, 0, 1, 1, 6, 0, 5, 0, 0, 0, 43, // 6
  
        0, 43, 0, 1, 1, 5, 5, 0, 3, 0, 0, 3, 0, 0, 0, 72, 0, 0, 43, 0, 43, 0, 1, 1, 5, 9,

        0, 93, 0, 93, 0, 0, 73, 0, 0, 42, 2, 2, 5, 0, 42, 8, 8, 5, 0, 43, 1, 5, 0, 43, 1, 9,

        0, 1, 1, 5, 9, 0, 0, 7, 0, 3, 0, 0, 67, 0, 0, 1, 1, 2, 2, 0, 1, 1, 5, 6, 0, 0, // 9
  
        93, 0, 0, 68, 0, 0, 1, 1, 8, 8, 0, 1, 1, 5, 9, 0, 93, 0, 1, 1, 5, 0, 72, 0, 68, 0,

        0, 6, 0, 1, 1, 5, 0, 73, 0, 67, 0, 0, 6, 0, 1, 1, 9, 0, 0, 7, 0, 3, 0, 0, 67, 0,

        68, 0, 42, 1, 5, 0, 1, 6, 0, 1, 1, 5, 5, 0, 68, 0, 67, 0, 42, 1, 43, 0, 1, 1, 9, 0, // 12
  
        0, 7, 0, 43, 0, 43, 0, 9, 0, 1, 1, 5, 1, 1, 9, 0, 0, 0, 1, 1, 5, 0, 1, 1, 6, 0,

        5, 9, 0, 3, 0, 0, 3, 0, 1, 1, 5, 43, 0, 0, 93, 0, 1, 1, 9, 43, 0, 0, 93, 93, 0, 5,

        0, 1, 1, 6, 0, 43, 0, 43, 0, 0, 5, 0, 0, 0, 1, 1, 72, 0, 0, 1, 1, 73, 0, 0, 1, 1, // 15
   
        5, 0, 1, 1, 9, 0, 0, 0, 3, 0, 0, 7, 7, 7, 0, 0, 3, 0, 0, 0, 1, 1, 2, 0, 1, 8,

        8, 5, 0, 0, 0, 0, 0, 0, 1, 1, 51, 0, 0, 1, 1, 1, 51, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 18
   
        0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Medium", score_database2, "Medium");
    }
}
