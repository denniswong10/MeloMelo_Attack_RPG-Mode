﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score383 : MonoBehaviour
{
    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 2, 0, 0, 0, 0, 0, 8, 0, 0, 0, 0, 5, 0, 0, 0, 0, 8, 0, 0, 0, 0, 0, 2, 0, 0,

        0, 5, 0, 0, 0, 0, 1, 0, 2, 0, 5, 0, 0, 4, 0, 4, 0, 0, 1, 0, 8, 0, 5, 0, 0, 4,

        0, 4, 0, 5, 0, 5, 0, 0, 0, 0, 0, 2, 2, 0, 4, 0, 8, 8, 0, 5, 0, 0, 14, 0, 0, 0, // 3
   
        0, 1, 2, 0, 4, 0, 1, 8, 0, 5, 4, 0, 5, 6, 0, 0, 93, 0, 93, 0, 0, 1, 5, 0, 1, 6,

        0, 2, 1, 0, 8, 5, 0, 0, 0, 0, 7, 0, 0, 0, 0, 7, 0, 0, 0, 14, 0, 14, 0, 0, 11, 0,

        0, 5, 0, 6, 0, 0, 14, 0, 14, 0, 0, 12, 0, 0, 5, 0, 6, 0, 0, 7, 0, 0, 0, 3, 0, 0, // 6
 
        0, 7, 0, 0, 1, 0, 4, 0, 5, 0, 1, 0, 6, 0, 5, 0, 7, 0, 5, 0, 7, 0, 3, 0, 8, 8,

        0, 2, 2, 0, 14, 0, 14, 0, 0, 14, 0, 14, 0, 0, 5, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 7, 0, 0, 0, 310, 0, 0, 0, 0, 0, 0, 0, 311, 0, 0, 0, 0, 0, 0, 0, 310, 0, 0, 0, // 9
   
        0, 0, 0, 0, 311, 0, 0, 0, 0, 0, 0, 0, 4, 0, 5, 0, 5, 0, 0, 1, 1, 0, 5, 0, 1, 1,

        0, 6, 0, 0, 5, 4, 0, 5, 6, 0, 5, 5, 0, 0, 0, 1, 0, 3, 0, 2, 2, 0, 14, 0, 0, 3,

        0, 5, 0, 5, 0, 3, 0, 8, 8, 0, 14, 0, 0, 1, 5, 0, 0, 101, 0, 101, 0, 102, 0, 102, 0, 0, // 12
   
        5, 0, 0, 307, 0, 0, 0, 14, 0, 14, 0, 0, 308, 0, 0, 0, 14, 0, 14, 0, 0, 5, 0, 1, 1, 0,

        2, 2, 0, 4, 0, 8, 5, 0, 0, 0, 0, 0, 101, 0, 0, 102, 0, 0, 5, 0, 1, 1, 0, 14, 0, 14,

        0, 14, 0, 0, 0, 0, 310, 0, 0, 0, 0, 0, 0, 0, 311, 0, 0, 0, 0, 0, 0, 0, 310, 0, 0, 0, // 15
 
        0, 0, 0, 0, 311, 0, 0, 0, 0, 0, 0, 0, 307, 0, 0, 0, 5, 308, 0, 0, 0, 5, 5, 0, 11, 20,

        0, 12, 20, 0, 14, 101, 0, 102, 14, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 18
  
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 1, 2, 2, 5, 0, 0, 4, 2, 0, 4, 8, 5, 0, 0, 0, 0, 1, 8, 8, 5, 0, 0, 4, 2, 0,

        4, 5, 0, 0, 0, 0, 1, 2, 2, 5, 0, 0, 4, 2, 0, 4, 8, 0, 0, 1, 8, 8, 5, 0, 0, 4,

        2, 0, 4, 8, 0, 5, 0, 0, 0, 0, 0, 300, 0, 0, 0, 0, 0, 1, 1, 5, 0, 0, 6, 0, 0, 0, // 3
   
        0, 301, 0, 0, 0, 0, 0, 4, 2, 0, 4, 8, 0, 1, 5, 0, 0, 93, 0, 0, 1, 1, 5, 0, 1, 1,

        6, 0, 1, 1, 6, 5, 0, 0, 0, 0, 7, 0, 0, 0, 0, 7, 0, 0, 0, 303, 0, 0, 0, 0, 0, 11,

        0, 20, 0, 12, 0, 0, 304, 0, 0, 0, 0, 0, 12, 0, 20, 0, 11, 0, 0, 307, 0, 0, 0, 308, 0, 0, // 6
 
        0, 5, 0, 0, 1, 1, 4, 0, 5, 0, 5, 0, 1, 1, 4, 0, 5, 0, 5, 0, 2, 2, 4, 0, 8, 8,

        4, 0, 0, 101, 14, 102, 0, 0, 5, 0, 102, 14, 101, 0, 0, 5, 0, 0, 0, 0, 3, 0, 0, 0, 0, 7,

        0, 0, 5, 0, 0, 0, 310, 0, 0, 0, 0, 0, 4, 0, 4, 0, 0, 311, 0, 0, 0, 0, 0, 4, 0, 4, // 9
   
        0, 0, 1, 1, 6, 0, 5, 0, 0, 0, 11, 53, 12, 0, 101, 14, 102, 0, 0, 12, 53, 11, 0, 102, 14, 102,

        0, 0, 1, 1, 5, 0, 1, 1, 6, 0, 5, 6, 0, 0, 0, 14, 0, 14, 0, 14, 20, 14, 0, 14, 0, 14,

        11, 14, 12, 14, 0, 101, 14, 0, 102, 14, 0, 0, 1, 6, 5, 0, 0, 307, 0, 0, 0, 5, 5, 0, 1, 6, // 12
   
        0, 5, 0, 1, 1, 14, 0, 0, 1, 1, 101, 0, 102, 0, 0, 1, 1, 5, 0, 0, 308, 0, 0, 0, 5, 5,

        6, 0, 5, 2, 8, 2, 5, 0, 0, 0, 0, 0, 1, 1, 101, 14, 0, 0, 5, 0, 1, 1, 102, 14, 0, 0,

        5, 5, 6, 0, 0, 0, 310, 0, 0, 11, 0, 0, 11, 0, 0, 0, 0, 311, 0, 0, 12, 0, 0, 12, 0, 0, // 15
 
        0, 0, 5, 1, 5, 0, 0, 0, 308, 0, 0, 0, 307, 0, 0, 0, 5, 0, 5, 0, 101, 14, 102, 14, 101, 11,

        20, 12, 0, 102, 14, 101, 14, 102, 12, 20, 11, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 18
  
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Medium", score_database2, "Medium");
    }
}
