﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score407 : MonoBehaviour
{
    // 20, 72, 73, 42, 43, 74, 51, 102, 81, 82, 75, 15, 101, 103, 18, 30, 31, 86, 87

    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 2, 0, 8, 0, 4, 0, 0, 8, 0, 2, 0, 4, 0, 0, 5, 0, 0, 0, 1, 0, 5, 0, 1, 0, 5,

        0, 1, 0, 6, 0, 0, 0, 2, 0, 2, 0, 5, 0, 6, 0, 0, 0, 0, 0, 0, 0, 51, 0, 51, 0, 0,

        2, 0, 5, 0, 102, 0, 0, 5, 0, 0, 0, 1, 0, 1, 0, 1, 0, 5, 0, 1, 0, 1, 0, 1, 0, 9, // 3
   
        0, 1, 0, 5, 0, 1, 0, 9, 0, 1, 0, 5, 0, 1, 0, 9, 0, 51, 0, 0, 5, 0, 5, 0, 6, 0,

        0, 1, 0, 5, 0, 0, 42, 0, 0, 43, 0, 0, 0, 42, 0, 20, 0, 43, 0, 20, 0, 0, 5, 0, 20, 0,

        0, 5, 0, 102, 0, 102, 0, 4, 0, 4, 0, 5, 0, 9, 0, 0, 81, 0, 0, 82, 0, 0, 0, 0, 0, 1, // 6
   
        0, 0, 5, 0, 0, 6, 0, 0, 5, 0, 0, 1, 0, 1, 0, 5, 0, 0, 101, 0, 0, 103, 0, 0, 5, 0,

        0, 0, 0, 0, 93, 0, 93, 0, 0, 1, 0, 0, 5, 0, 0, 20, 0, 20, 0, 0, 42, 0, 0, 43, 0, 0,

        0, 0, 5, 0, 0, 0, 0, 1, 0, 93, 0, 93, 0, 0, 5, 0, 0, 5, 0, 6, 0, 0, 81, 0, 0, 82, // 9
  
        0, 0, 75, 0, 75, 0, 74, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 4, 0, 0, 0, 0, 4, 0, 0, 0,

        0, 4, 0, 0, 0, 0, 1, 0, 5, 0, 9, 0, 0, 0, 1, 0, 81, 0, 0, 82, 0, 0, 5, 0, 42, 0,

        0, 43, 0, 0, 5, 0, 5, 0, 0, 9, 0, 0, 1, 0, 1, 0, 75, 0, 0, 0, 0, 2, 0, 2, 0, 5, // 12
  
        0, 8, 0, 8, 0, 5, 0, 0, 0, 0, 15, 0, 0, 0, 15, 0, 0, 0, 15, 0, 0, 9, 0, 0, 0, 1,

        0, 0, 5, 0, 101, 0, 0, 4, 0, 0, 103, 0, 0, 4, 0, 0, 4, 0, 0, 1, 0, 5, 0, 1, 0, 9,

        0, 0, 0, 1, 0, 0, 5, 0, 20, 0, 20, 0, 5, 0, 6, 0, 0, 0, 42, 0, 20, 0, 20, 0, 43, 0, // 15
  
        0, 20, 0, 20, 0, 0, 101, 0, 0, 103, 0, 0, 51, 0, 20, 0, 0, 0, 0, 0, 0, 1, 0, 5, 0, 18,

        0, 0, 1, 0, 9, 0, 15, 0, 0, 0, 0, 0, 0, 2, 0, 2, 0, 5, 0, 8, 0, 8, 0, 15, 0, 0,

        0, 0, 1, 0, 5, 0, 5, 0, 102, 0, 0, 4, 0, 4, 0, 0, 1, 0, 5, 0, 6, 0, 1, 0, 5, 0, // 18
 
        9, 0, 0, 0, 5, 0, 0, 4, 0, 4, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 93, 0, 0, 0, 0,

        0, 93, 0, 93, 0, 0, 0, 0, 0, 0, 93, 0, 0, 0, 0, 0, 93, 0, 4, 0, 93, 0, 5, 0, 0, 0,

        0, 0, 93, 0, 0, 0, 0, 0, 93, 0, 0, 0, 0, 0, 0, 5, 9, 0, 9, 5, 0, 20, 0, 20, 0, 0, // 21
  
        81, 0, 0, 0, 30, 0, 0, 0, 72, 0, 0, 0, 30, 0, 0, 0, 72, 0, 73, 0, 31, 0, 0, 0, 73, 0,

        0, 0, 31, 0, 0, 73, 0, 0, 0, 1, 0, 5, 0, 9, 0, 0, 18, 0, 0, 0, 18, 0, 0, 0, 0, 0,

        20, 0, 20, 0, 0, 81, 0, 82, 0, 0, 5, 0, 0, 81, 0, 82, 0, 0, 5, 0, 6, 0, 0, 0, 0, 0, // 24
   
        0, 1, 0, 4, 0, 1, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 2, 2, 8, 8, 0, 4, 0, 8, 2, 8, 2, 0, 4, 5, 5, 0, 0, 0, 2, 2, 4, 0, 8, 8, 4,
   
        1, 1, 5, 6, 0, 0, 0, 1, 1, 5, 0, 1, 1, 6, 0, 0, 0, 0, 0, 0, 0, 8, 8, 8, 5, 0,
   
        2, 2, 4, 0, 8, 8, 8, 5, 0, 0, 0, 72, 0, 73, 0, 72, 0, 73, 0, 72, 0, 73, 0, 72, 0, 73, // 3
   
        0, 72, 20, 73, 0, 72, 20, 73, 0, 72, 20, 73, 0, 73, 20, 72, 0, 73, 0, 1, 1, 5, 0, 1, 1, 6,
   
        0, 1, 1, 5, 0, 0, 42, 0, 0, 20, 43, 0, 0, 20, 0, 1, 1, 6, 0, 0, 0, 2, 4, 42, 0, 20,
   
        0, 20, 5, 0, 102, 0, 8, 4, 43, 0, 20, 0, 20, 5, 0, 0, 1, 1, 5, 6, 0, 0, 0, 0, 0, 1, // 6
   
        1, 5, 5, 0, 4, 4, 5, 0, 2, 2, 8, 8, 0, 5, 0, 5, 0, 1, 1, 4, 0, 1, 5, 1, 6, 0,
  
        0, 0, 0, 0, 81, 0, 82, 0, 0, 1, 1, 5, 5, 0, 20, 0, 20, 0, 1, 1, 5, 6, 0, 2, 8, 0,
  
        1, 1, 5, 0, 0, 0, 0, 1, 1, 81, 0, 82, 0, 1, 1, 5, 0, 1, 1, 6, 0, 0, 5, 0, 0, 20, // 9
  
        0, 0, 72, 0, 73, 0, 75, 20, 0, 0, 0, 0, 86, 0, 0, 0, 0, 87, 0, 0, 0, 0, 86, 0, 0, 0,
  
        0, 87, 0, 0, 0, 0, 1, 1, 5, 0, 9, 0, 0, 0, 1, 1, 43, 0, 0, 20, 0, 5, 1, 1, 42, 0,
  
        0, 20, 0, 5, 2, 4, 8, 0, 5, 5, 0, 72, 20, 73, 0, 0, 74, 0, 0, 0, 0, 1, 2, 2, 5, 0, // 12
  
        1, 8, 8, 5, 0, 0, 6, 0, 0, 0, 15, 0, 20, 0, 0, 15, 0, 20, 20, 0, 1, 5, 0, 0, 0, 1,
  
        1, 5, 5, 0, 6, 0, 2, 2, 4, 8, 8, 0, 0, 3, 0, 0, 3, 0, 0, 6, 1, 5, 0, 4, 5, 9,
  
        0, 0, 0, 1, 1, 5, 5, 0, 15, 0, 20, 20, 0, 1, 5, 0, 0, 0, 1, 1, 42, 0, 20, 43, 0, 20, // 15
  
        0, 0, 5, 5, 6, 0, 0, 101, 0, 0, 103, 0, 0, 75, 51, 0, 0, 0, 0, 0, 0, 1, 1, 5, 0, 2,
  
        2, 2, 0, 8, 8, 8, 0, 5, 6, 0, 0, 0, 0, 18, 0, 0, 0, 1, 1, 0, 0, 5, 0, 0, 15, 0,
  
        0, 0, 1, 1, 0, 0, 5, 4, 102, 0, 0, 3, 0, 3, 0, 0, 1, 1, 0, 75, 0, 0, 1, 1, 0, 75, // 18
 
        0, 0, 1, 1, 5, 9, 0, 0, 3, 0, 3, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0,
 
        0, 1, 5, 0, 0, 0, 0, 0, 0, 1, 6, 0, 0, 0, 0, 0, 1, 1, 5, 0, 1, 1, 9, 0, 0, 0,
 
        0, 2, 4, 0, 0, 0, 0, 8, 4, 0, 0, 0, 0, 0, 0, 81, 81, 0, 82, 82, 0, 20, 0, 1, 1, 5, // 21
  
        6, 0, 0, 0, 30, 0, 0, 0, 31, 0, 0, 0, 30, 73, 0, 0, 31, 72, 0, 0, 30, 73, 20, 0, 31, 72,
  
        20, 0, 1, 1, 5, 9, 0, 0, 0, 1, 1, 5, 0, 18, 0, 0, 9, 0, 0, 0, 0, 1, 1, 86, 0, 0,
   
        1, 1, 87, 0, 0, 1, 5, 1, 9, 0, 3, 0, 0, 101, 0, 0, 103, 0, 0, 0, 5, 0, 0, 0, 0, 0, // 24
   
        0, 1, 1, 4, 0, 1, 5, 1, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Medium", score_database2, "Medium");
    }
}
