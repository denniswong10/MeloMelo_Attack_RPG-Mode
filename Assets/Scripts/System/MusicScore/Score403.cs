﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score403 : MonoBehaviour
{
    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 2, 0, 5, 0, 0, 8, 0, 8, 0, 0, 4, 0, 4, 0, 4, 0, 0, 1, 5, 0, 1, 5, 0, 110, 0,

        111, 0, 0, 2, 0, 5, 0, 0, 8, 0, 5, 0, 50, 0, 50, 0, 50, 0, 0, 0, 1, 1, 0, 5, 0, 1,

        1, 0, 5, 5, 0, 50, 0, 50, 0, 0, 0, 0, 0, 0, 0, 93, 0, 0, 0, 0, 0, 0, 93, 0, 0, 0, // 3
  
        0, 0, 0, 0, 2, 0, 8, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 93, 0, 0, 0, 0, 0, 0, 0,

        2, 0, 8, 0, 2, 0, 1, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 93, 0, 0, 0,

        0, 0, 0, 93, 0, 0, 0, 0, 0, 0, 2, 2, 0, 8, 0, 1, 5, 0, 0, 0, 0, 0, 0, 0, 93, 0, // 6
  
        0, 93, 0, 0, 0, 0, 2, 2, 0, 8, 8, 0, 1, 1, 0, 5, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 54, 0, 0, 55, 0, 0, 54, 0, 0, 55, 0, 0, 5, 5, 0, 0, 2, 50, 0, 0, 8, 50, 0, 0,

        9, 0, 0, 0, 0, 93, 93, 0, 0, 1, 0, 5, 0, 1, 0, 6, 0, 0, 50, 0, 50, 0, 1, 1, 0, 5, // 9
  
        50, 0, 50, 0, 0, 0, 0, 0, 33, 0, 0, 33, 0, 0, 1, 1, 0, 50, 0, 50, 0, 1, 1, 0, 110, 0,

        110, 0, 1, 1, 0, 111, 0, 111, 0, 0, 0, 0, 4, 0, 5, 0, 0, 4, 4, 0, 5, 5, 0, 0, 1, 1,

        0, 110, 110, 0, 1, 1, 0, 111, 111, 0, 0, 0, 0, 0, 2, 0, 8, 0, 2, 0, 8, 0, 110, 0, 50, 0, // 12
  
        111, 0, 50, 0, 1, 1, 0, 54, 0, 55, 0, 4, 0, 5, 0, 0, 0, 0, 51, 0, 0, 1, 1, 0, 5, 5,

        0, 6, 0, 5, 0, 0, 4, 2, 0, 4, 8, 0, 50, 50, 0, 110, 111, 0, 5, 0, 0, 0, 0, 0, 0, 0,

        0, 93, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 5, 0, 1, 1, 0, 9, 0, // 15
   
        110, 0, 111, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 93, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0,

        0, 0, 2, 2, 0, 8, 8, 0, 0, 0, 0, 0, 50, 0, 50, 0, 110, 0, 110, 0, 0, 111, 0, 0, 0, 0, // 18
  
        1, 0, 50, 0, 0, 0, 0, 0, 0, 0, 53, 0, 0, 54, 0, 0, 102, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 2, 2, 110, 0, 0, 8, 8, 111, 0, 0, 2, 2, 110, 0, 111, 0, 0, 1, 1, 5, 5, 0, 0, 2, 8,
  
        110, 0, 0, 2, 8, 111, 0, 0, 2, 8, 110, 0, 111, 0, 1, 5, 1, 5, 0, 0, 110, 0, 5, 0, 111, 0,
  
        5, 0, 1, 4, 0, 2, 2, 8, 8, 0, 0, 0, 0, 0, 0, 93, 0, 0, 0, 0, 0, 0, 93, 0, 0, 0, // 3
  
        0, 0, 0, 0, 93, 93, 93, 0, 93, 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 0, 0, 0, 0, 0, 0, 0,
  
        2, 2, 2, 0, 8, 8, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 93, 0, 0, 0,
  
        0, 0, 0, 93, 0, 0, 0, 0, 0, 0, 93, 93, 93, 93, 0, 0, 93, 0, 0, 0, 0, 0, 0, 0, 50, 0, // 6
  
        2, 8, 0, 0, 0, 0, 93, 93, 93, 0, 2, 2, 8, 8, 0, 0, 4, 1, 5, 0, 0, 0, 0, 0, 0, 0,
  
        0, 0, 54, 0, 0, 1, 1, 5, 5, 0, 110, 0, 111, 0, 55, 0, 0, 2, 2, 5, 0, 8, 8, 5, 0, 4,
  
        5, 0, 0, 0, 0, 0, 12, 0, 0, 1, 1, 5, 0, 1, 1, 6, 0, 110, 111, 0, 50, 0, 111, 110, 0, 1, // 9
  
        1, 5, 5, 0, 0, 0, 0, 0, 33, 0, 0, 4, 0, 4, 0, 1, 1, 5, 5, 0, 50, 0, 50, 0, 1, 1,
  
        5, 0, 1, 1, 6, 0, 1, 4, 0, 0, 0, 0, 110, 111, 5, 0, 4, 4, 0, 111, 110, 5, 0, 4, 4, 0,
   
        2, 8, 110, 0, 2, 8, 111, 0, 5, 0, 0, 0, 0, 0, 1, 1, 5, 0, 1, 1, 6, 0, 1, 1, 5, 0, // 12
  
        1, 1, 6, 0, 54, 0, 55, 0, 1, 1, 5, 5, 0, 9, 0, 0, 0, 0, 102, 0, 0, 1, 1, 5, 0, 1,
  
        1, 6, 0, 5, 0, 0, 4, 2, 2, 110, 0, 4, 8, 8, 111, 0, 1, 1, 5, 0, 0, 0, 0, 0, 0, 0,
  
        0, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 5, 5, 0, 110, 111, 110, 111, 0, // 15
   
        1, 1, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 0, 0, 0, 0, 0, 0,
   
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0,
   
        0, 0, 93, 93, 93, 93, 0, 5, 0, 0, 0, 0, 1, 1, 5, 0, 1, 1, 6, 0, 0, 5, 0, 0, 0, 0, // 18
  
        2, 8, 50, 0, 0, 0, 0, 0, 0, 0, 54, 0, 0, 54, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0,
  
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
  
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_database3 =
    {
        0,

        0, 110, 20, 110, 0, 0, 111, 20, 111, 0, 0, 11, 20, 110, 0, 50, 0, 0, 1, 5, 1, 9, 0, 0, 111, 20,

        111, 0, 0, 110, 20, 110, 0, 0, 12, 20, 111, 0, 50, 0, 0, 5, 5, 9, 0, 0, 110, 0, 20, 0, 111, 0,

        20, 0, 1, 5, 0, 2, 2, 8, 4, 0, 0, 0, 0, 0, 0, 93, 2, 2, 0, 0, 0, 0, 93, 8, 8, 5, // 3
  
        0, 0, 0, 0, 93, 2, 8, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 0, 2, 8, 5, 0, 0, 0,

        1, 1, 4, 0, 1, 1, 0, 6, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 93, 2, 2, 5,

        0, 0, 0, 93, 8, 8, 5, 0, 0, 0, 93, 2, 8, 2, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 50, 0, // 6
  
        5, 9, 0, 0, 0, 0, 93, 2, 2, 0, 93, 8, 8, 4, 0, 0, 1, 1, 9, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 54, 0, 55, 0, 1, 1, 5, 0, 55, 0, 54, 0, 1, 1, 9, 0, 4, 20, 0, 8, 4, 20, 0, 1,

        5, 0, 0, 0, 0, 0, 12, 0, 12, 0, 0, 1, 1, 5, 5, 6, 0, 50, 20, 0, 50, 20, 0, 1, 1, 5, // 9
  
        1, 1, 9, 0, 0, 0, 0, 0, 33, 0, 0, 1, 1, 5, 0, 12, 0, 12, 0, 20, 110, 0, 111, 0, 1, 1,

        5, 1, 1, 9, 0, 1, 1, 5, 0, 0, 0, 0, 301, 302, 0, 0, 5, 0, 0, 302, 301, 0, 0, 5, 0, 0,

        301, 0, 2, 0, 302, 0, 8, 0, 5, 0, 0, 0, 0, 0, 110, 50, 5, 0, 111, 50, 9, 0, 110, 50, 111, 0, // 12
  
        20, 12, 12, 0, 5, 0, 9, 0, 2, 4, 8, 4, 0, 5, 0, 0, 0, 0, 102, 0, 0, 5, 5, 9, 0, 1,

        1, 5, 0, 6, 0, 0, 20, 2, 2, 5, 5, 20, 8, 8, 5, 5, 1, 1, 9, 0, 0, 0, 0, 0, 0, 0,

        0, 25, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 301, 0, 302, 0, 0, 301, 302, 301, 302, 0, // 15
   
        20, 20, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 0, 0, 5, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0,

        0, 0, 20, 2, 2, 8, 0, 5, 0, 0, 0, 0, 110, 0, 5, 0, 111, 0, 5, 0, 0, 9, 0, 0, 0, 0, // 18
  
        12, 20, 12, 0, 0, 0, 0, 0, 0, 0, 55, 0, 0, 54, 0, 0, 0, 9, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Medium", score_database2, "Medium", score_database3, "Medium");
    }
}