﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score365 : MonoBehaviour
{
    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 3
   
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        2, 0, 2, 0, 2, 0, 5, 0, 5, 0, 8, 0, 8, 0, 8, 0, 5, 0, 5, 0, 9, 0, 5, 0, 2, 0,

        8, 0, 2, 0, 8, 0, 5, 0, 5, 0, 9, 0, 5, 0, 2, 0, 2, 0, 8, 0, 8, 0, 5, 0, 4, 0, // 6
   
        5, 0, 4, 0, 0, 93, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 5, 0, 5, 0, 1, 0, 6,

        0, 1, 0, 5, 0, 9, 0, 0, 4, 0, 4, 0, 5, 0, 0, 0, 0, 5, 0, 0, 0, 9, 0, 0, 0, 5, // 9
   
        0, 2, 0, 2, 0, 5, 0, 0, 8, 0, 8, 0, 5, 0, 0, 1, 0, 6, 0, 0, 0, 53, 0, 0, 0, 53,

        0, 0, 0, 53, 0, 0, 0, 5, 0, 0, 0, 1, 0, 1, 0, 53, 0, 53, 0, 5, 0, 0, 6, 0, 0, 4,

        0, 0, 4, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 5, 0, 0, 1, 0, 9, 0, 0, 1, // 12
   
        0, 5, 0, 9, 0, 53, 0, 53, 0, 53, 0, 53, 0, 5, 0, 9, 0, 0, 4, 0, 0, 0, 4, 0, 2, 0,

        8, 0, 0, 5, 0, 5, 0, 4, 0, 2, 0, 4, 0, 8, 0, 53, 0, 53, 0, 5, 0, 0, 0, 53, 0, 53,

        0, 53, 0, 53, 0, 5, 0, 5, 0, 53, 0, 53, 0, 53, 0, 53, 0, 5, 0, 9, 0, 53, 0, 53, 0, 53, // 15
   
        0, 0, 0, 53, 0, 101, 0, 53, 0, 103, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 9, 0, 0, 5, 0, 0,

        9, 0, 0, 53, 0, 53, 0, 101, 0, 103, 0, 101, 0, 103, 0, 4, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
  
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0,
   
        0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 1, 0, 1, 0, 6, 0, 1, 0, 1, 0, // 3
   
        5, 0, 1, 0, 6, 0, 5, 0, 1, 0, 6, 0, 5, 0, 2, 8, 2, 5, 0, 0, 0, 0, 0, 0, 0, 0,
   
        53, 0, 53, 0, 53, 0, 5, 5, 0, 19, 0, 53, 0, 53, 0, 53, 0, 5, 5, 0, 19, 0, 5, 5, 0, 53,
   
        0, 53, 0, 53, 0, 1, 6, 5, 0, 19, 0, 5, 0, 0, 1, 6, 5, 0, 19, 0, 5, 0, 0, 53, 4, 0, // 6
   
        53, 0, 4, 0, 51, 0, 0, 5, 0, 0, 0, 0, 0, 1, 0, 0, 4, 0, 0, 5, 0, 0, 0, 0, 0, 0,
   
        0, 1, 0, 2, 8, 0, 5, 0, 0, 0, 93, 0, 0, 0, 0, 1, 0, 0, 2, 4, 8, 0, 5, 0, 4, 2,
   
        2, 0, 4, 8, 8, 0, 1, 0, 1, 0, 5, 5, 19, 0, 0, 0, 0, 53, 0, 53, 0, 53, 0, 101, 0, 103, // 9
   
        0, 5, 0, 19, 0, 0, 4, 2, 8, 0, 4, 8, 2, 0, 1, 1, 5, 19, 0, 0, 0, 53, 0, 101, 0, 53,
   
        0, 103, 0, 53, 0, 1, 5, 19, 0, 0, 0, 31, 0, 0, 0, 30, 0, 53, 0, 0, 1, 5, 19, 0, 2, 4,
  
        0, 8, 4, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 5, 5, 19, 0, 53, 0, 53, 0, 1, 5, // 12
   
        5, 6, 0, 2, 0, 101, 0, 53, 0, 103, 0, 53, 0, 101, 0, 5, 0, 0, 5, 5, 19, 0, 5, 0, 31, 0,
  
        0, 30, 0, 0, 0, 5, 0, 4, 2, 2, 0, 4, 8, 8, 0, 101, 0, 103, 0, 5, 0, 0, 0, 101, 31, 0,
   
        53, 0, 30, 0, 53, 0, 53, 0, 5, 5, 5, 0, 103, 30, 0, 53, 0, 31, 0, 53, 0, 53, 0, 51, 0, 0, // 15
   
        5, 0, 0, 1, 5, 53, 0, 1, 5, 19, 0, 0, 0, 0, 0, 0, 0, 101, 4, 0, 53, 4, 0, 103, 4, 0,
    
        31, 0, 0, 103, 0, 30, 0, 0, 101, 0, 53, 0, 2, 4, 0, 8, 4, 0, 5, 0, 0, 0, 0, 0, 0, 0,
    
        0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Medium", score_database2, "Medium");
    }
}