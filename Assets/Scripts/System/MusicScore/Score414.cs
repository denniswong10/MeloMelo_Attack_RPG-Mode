﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score414 : MonoBehaviour
{
    // 18, 300, 301   201, 202, 12

    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 0, 300, 0, 0, 0, 8, 0, 8, 0, 8, 0, 0, 301, 0, 0, 0, 8, 0, 8, 0, 8, 0, 0, 5, 0,
  
        5, 0, 2, 0, 2, 0, 300, 0, 0, 301, 0, 0, 0, 5, 0, 0, 0, 2, 0, 2, 0, 8, 0, 8, 0, 0,
  
        5, 0, 5, 0, 18, 0, 0, 0, 300, 0, 0, 301, 0, 0, 4, 0, 5, 0, 5, 0, 5, 0, 0, 4, 0, 0, // 3
  
        0, 0, 2, 0, 2, 0, 2, 0, 8, 0, 2, 0, 8, 0, 5, 0, 5, 0, 0, 0, 2, 0, 4, 0, 8, 0,
  
        4, 0, 2, 0, 4, 0, 8, 0, 5, 0, 0, 0, 0, 1, 0, 5, 0, 4, 0, 0, 1, 0, 5, 0, 4, 0,
  
        0, 30, 0, 0, 0, 31, 0, 0, 0, 5, 0, 4, 0, 1, 0, 31, 0, 0, 0, 30, 0, 0, 0, 12, 0, 0, // 6
  
        5, 0, 0, 0, 93, 0, 0, 0, 93, 0, 0, 0, 2, 0, 2, 0, 8, 0, 8, 0, 0, 0, 0, 0, 1, 0,
  
        1, 0, 5, 0, 8, 0, 8, 0, 0, 0, 5, 0, 0, 0, 5, 0, 0, 1, 0, 8, 0, 1, 0, 4, 0, 1,
  
        0, 5, 0, 0, 9, 0, 0, 93, 0, 0, 0, 93, 0, 0, 300, 0, 31, 0, 0, 301, 0, 30, 0, 0, 0, 5, // 9
 
        0, 5, 0, 5, 0, 1, 0, 6, 0, 0, 0, 1, 0, 0, 5, 0, 1, 0, 0, 5, 0, 5, 0, 9, 0, 0,
   
        0, 93, 0, 0, 93, 0, 0, 93, 0, 0, 5, 0, 0, 9, 0, 0, 0, 0, 1, 0, 5, 0, 9, 0, 12, 0,
   
        0, 1, 0, 5, 0, 9, 0, 5, 0, 12, 0, 0, 0, 18, 0, 0, 0, 5, 0, 5, 0, 4, 0, 0, 2, 0, // 12
   
        0, 8, 0, 0, 1, 0, 0, 5, 0, 201, 0, 0, 202, 0, 0, 5, 0, 0, 5, 0, 9, 0, 0, 18, 0, 0,
  
        4, 0, 0, 2, 0, 8, 0, 5, 0, 0, 7, 0, 0, 7, 0, 0, 2, 0, 0, 5, 0, 8, 0, 0, 5, 0,
  
        4, 0, 5, 0, 201, 0, 0, 202, 0, 0, 4, 0, 4, 0, 5, 0, 5, 0, 4, 0, 4, 0, 0, 7, 0, 0, // 15
  
        0, 2, 0, 2, 0, 4, 0, 8, 0, 8, 0, 5, 0, 0, 1, 0, 5, 0, 4, 0, 0, 0, 9, 0, 0, 0,
  
        0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 93, 0, 0, 93, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
  
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 0, 18, 0, 0, 0, 300, 0, 0, 301, 0, 0, 0, 1, 1, 5, 0, 1, 1, 9, 0, 18, 0, 0, 0, 300,

        0, 0, 301, 0, 0, 0, 1, 6, 0, 1, 4, 5, 0, 11, 0, 0, 0, 4, 0, 4, 0, 18, 0, 0, 4, 0,

        4, 0, 8, 8, 300, 0, 0, 8, 8, 301, 0, 0, 300, 0, 4, 0, 301, 0, 4, 0, 1, 5, 5, 6, 0, 0, // 3
  
        0, 0, 1, 1, 5, 0, 0, 0, 93, 0, 0, 0, 93, 0, 0, 0, 93, 0, 0, 0, 1, 1, 9, 0, 0, 0,

        93, 0, 0, 0, 93, 0, 0, 0, 93, 0, 0, 0, 0, 1, 1, 5, 0, 4, 0, 1, 1, 5, 0, 4, 0, 30,

        0, 0, 31, 0, 0, 0, 1, 1, 5, 5, 0, 4, 0, 1, 1, 5, 0, 4, 0, 1, 1, 5, 0, 12, 0, 0, // 6
  
        5, 0, 0, 0, 93, 0, 0, 0, 93, 0, 0, 0, 300, 0, 0, 301, 0, 0, 300, 4, 0, 0, 0, 0, 1, 1,

        8, 0, 4, 0, 1, 5, 1, 5, 0, 0, 7, 0, 0, 0, 7, 0, 0, 1, 1, 8, 8, 0, 4, 0, 1, 1,

        5, 0, 1, 1, 9, 0, 0, 7, 0, 0, 0, 7, 0, 0, 300, 16, 300, 0, 0, 6, 0, 1, 1, 5, 5, 0, // 9
 
        301, 17, 301, 0, 0, 1, 1, 5, 0, 0, 0, 1, 2, 2, 5, 0, 1, 8, 8, 5, 0, 4, 0, 5, 0, 0,

        0, 3, 0, 0, 7, 0, 0, 3, 0, 0, 1, 5, 1, 9, 0, 0, 0, 0, 201, 0, 0, 9, 0, 0, 202, 0,

        0, 9, 0, 2, 2, 4, 8, 8, 5, 0, 300, 0, 0, 301, 0, 0, 2, 2, 4, 8, 8, 9, 0, 0, 93, 0, // 12
   
        0, 93, 0, 0, 1, 1, 5, 5, 0, 300, 0, 0, 301, 0, 0, 1, 5, 1, 5, 0, 301, 0, 0, 300, 0, 0,

        4, 2, 2, 0, 4, 8, 8, 5, 0, 0, 0, 93, 93, 0, 0, 0, 1, 4, 5, 5, 0, 2, 2, 0, 1, 4,

        5, 5, 0, 8, 8, 0, 5, 4, 0, 0, 201, 0, 0, 202, 0, 0, 201, 0, 0, 202, 0, 0, 0, 7, 0, 0, // 15
  
        0, 1, 1, 2, 0, 4, 0, 1, 1, 8, 0, 4, 0, 1, 1, 5, 0, 4, 0, 1, 5, 1, 9, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 1, 5, 1, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Medium", score_database2, "Medium");
    }
}