﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score411 : MonoBehaviour
{
    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 30, 0, 0, 0, 1, 1, 0, 31, 0, 0, 0, 5, 0, 0, 0, 0,

        0, 0, 2, 0, 2, 0, 2, 0, 8, 0, 8, 0, 8, 0, 93, 0, 93, 0, 93, 0, 0, 0, 0, 0, 0, 0, // 3
  
        0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 5, 0, 6, 0, 25, 0, 0, 0, 0, 0, 0, 0, 1, 0, 5, 0,

        1, 1, 0, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 5, 0, 4, 0, 2, 2, 0, 4, 5, 0, 14,

        0, 5, 0, 1, 0, 1, 0, 5, 0, 4, 0, 0, 0, 0, 0, 20, 0, 1, 1, 0, 5, 0, 25, 0, 0, 0, // 6
  
        0, 0, 0, 0, 0, 5, 5, 0, 20, 0, 30, 0, 0, 31, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 2, 5,

        0, 4, 0, 8, 5, 0, 93, 0, 1, 0, 5, 0, 5, 0, 1, 0, 9, 0, 0, 2, 2, 0, 8, 0, 5, 0,

        0, 0, 0, 93, 0, 0, 0, 0, 93, 0, 0, 0, 0, 93, 0, 0, 0, 93, 0, 0, 0, 5, 0, 0, 1, 0, // 9
  
        5, 0, 0, 2, 2, 0, 5, 0, 25, 0, 9, 0, 0, 8, 8, 0, 5, 9, 0, 0, 1, 0, 5, 0, 2, 2,

        0, 8, 8, 0, 5, 0, 1, 4, 0, 0, 14, 0, 14, 0, 0, 1, 1, 0, 4, 5, 0, 9, 0, 0, 20, 20,

        0, 20, 20, 0, 1, 5, 0, 1, 9, 0, 4, 5, 0, 0, 20, 20, 0, 20, 20, 0, 1, 1, 0, 5, 0, 0, // 12
   
        0, 34, 0, 0, 8, 8, 0, 34, 0, 0, 8, 8, 0, 5, 0, 1, 1, 0, 5, 25, 0, 1, 0, 9, 0, 5,

        0, 29, 0, 0, 0, 0, 9, 1, 0, 5, 0, 25, 0, 25, 0, 25, 0, 25, 0, 5, 5, 0, 20, 20, 0, 5,

        0, 1, 1, 0, 5, 9, 0, 0, 0, 1, 0, 5, 0, 1, 0, 6, 0, 2, 8, 0, 5, 0, 1, 0, 9, 0, // 15
  
        0, 2, 2, 0, 4, 0, 8, 8, 0, 4, 0, 93, 93, 0, 5, 0, 0, 0, 0, 0, 0, 2, 8, 0, 5, 0,

        29, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        73, 0, 0, 0, 74, 0, 0, 0, 73, 0, 0, 0, 74, 0, 0, 0, 73, 0, 0, 74, 0, 0, 73, 0, 0, 74, // 18
  
        0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 5, 0, 25, 0, 0, 0, 8, 8, 0, 9, 0, 25,

        0, 14, 0, 0, 0, 101, 0, 20, 0, 102, 0, 20, 0, 1, 1, 0, 5, 0, 1, 1, 0, 9, 5, 0, 9, 5, // 21
   
        0, 1, 1, 0, 5, 0, 0, 0, 93, 0, 0, 0, 8, 8, 0, 0, 93, 0, 2, 2, 0, 1, 1, 0, 5, 0,

        1, 0, 5, 9, 0, 0, 108, 0, 0, 5, 5, 0, 0, 108, 20, 0, 0, 1, 0, 5, 0, 0, 1, 0, 5, 25,

        0, 1, 0, 9, 25, 0, 0, 4, 20, 0, 0, 0, 0, 5, 5, 0, 9, 0, 5, 5, 0, 9, 0, 25, 0, 25, // 24
 
        0, 0, 9, 5, 5, 0, 25, 0, 25, 0, 5, 9, 0, 0, 0, 1, 1, 0, 5, 0, 1, 1, 0, 5, 9, 0,

        2, 4, 0, 5, 0, 8, 4, 0, 5, 0, 6, 0, 20, 20, 0, 20, 20, 0, 5, 0, 5, 0, 20, 20, 0, 20,

        20, 0, 5, 0, 9, 0, 8, 0, 8, 0, 75, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 5, 0, // 27
   
        1, 0, 1, 0, 9, 0, 30, 0, 0, 31, 0, 0, 0, 0, 1, 0, 0, 5, 0, 0, 0, 1, 0, 1, 0, 5,

        0, 1, 0, 1, 0, 9, 0, 2, 2, 0, 8, 8, 0, 5, 0, 0, 0, 2, 0, 0, 0, 2, 0, 0, 0, 8,

        0, 0, 0, 8, 0, 0, 0, 5, 2, 0, 5, 8, 0, 5, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 30
  
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0
    };

    private int[] score_database2 =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
  
        0, 0, 0, 0, 0, 0, 0, 73, 30, 0, 0, 0, 8, 74, 31, 0, 0, 0, 8, 1, 1, 5, 0, 0, 0, 0,
  
        0, 0, 4, 2, 2, 2, 4, 8, 8, 8, 4, 2, 2, 8, 8, 0, 93, 93, 93, 93, 0, 0, 0, 0, 0, 0, // 3
  
        0, 0, 0, 0, 0, 0, 0, 2, 8, 5, 5, 0, 1, 1, 25, 0, 0, 0, 0, 0, 0, 0, 2, 8, 5, 9,
  
        0, 1, 1, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 5, 0, 14, 0, 4, 1, 1, 0, 14, 0, 4,
  
        1, 5, 0, 2, 2, 8, 8, 0, 5, 0, 0, 0, 0, 0, 0, 25, 0, 5, 5, 2, 8, 0, 30, 0, 0, 0, // 6
  
        0, 0, 0, 0, 0, 1, 1, 5, 5, 0, 31, 0, 0, 4, 5, 0, 0, 0, 0, 0, 0, 0, 1, 2, 2, 5,
  
        0, 1, 8, 8, 5, 0, 4, 0, 5, 5, 9, 0, 4, 0, 1, 1, 5, 0, 0, 93, 93, 93, 93, 0, 5, 0,
  
        0, 0, 0, 5, 5, 0, 0, 0, 93, 0, 0, 0, 0, 9, 0, 0, 0, 5, 0, 0, 0, 4, 0, 0, 93, 0, // 9
  
        93, 0, 0, 1, 1, 4, 5, 0, 25, 0, 25, 0, 0, 1, 1, 6, 25, 0, 0, 0, 1, 1, 5, 0, 2, 8,
  
        2, 0, 1, 1, 5, 0, 8, 4, 0, 0, 101, 14, 102, 0, 0, 1, 1, 8, 0, 5, 0, 9, 0, 0, 20, 30,
  
        0, 0, 20, 0, 8, 8, 0, 93, 93, 0, 5, 5, 0, 0, 20, 31, 0, 0, 20, 0, 8, 8, 5, 9, 0, 0, // 12
   
        0, 34, 0, 0, 20, 34, 0, 0, 20, 34, 0, 0, 20, 5, 0, 93, 0, 2, 2, 25, 0, 8, 8, 25, 0, 1,
   
        1, 29, 0, 0, 0, 0, 75, 1, 1, 5, 0, 6, 0, 1, 2, 2, 0, 1, 8, 8, 0, 75, 0, 25, 0, 25,
  
        0, 1, 1, 5, 0, 9, 0, 0, 0, 1, 1, 5, 0, 1, 1, 6, 0, 2, 2, 0, 93, 93, 0, 5, 9, 0, // 15
  
        0, 20, 20, 20, 20, 0, 5, 5, 5, 5, 0, 9, 5, 9, 5, 0, 0, 0, 0, 0, 0, 1, 5, 1, 9, 0,
  
        29, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
  
        2, 2, 2, 4, 8, 8, 8, 5, 0, 93, 93, 93, 93, 0, 73, 0, 0, 74, 0, 0, 0, 0, 30, 0, 0, 31, // 18
  
        0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
   
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 5, 9, 0, 25, 0, 0, 0, 5, 5, 5, 9, 0, 25,
   
        0, 25, 0, 0, 0, 1, 2, 4, 0, 1, 8, 4, 0, 5, 0, 9, 0, 20, 20, 20, 20, 0, 5, 5, 5, 5, // 21
   
        0, 9, 5, 9, 5, 0, 0, 0, 2, 0, 0, 0, 2, 0, 0, 0, 8, 0, 1, 1, 5, 0, 1, 1, 9, 0,
   
        1, 1, 5, 9, 0, 0, 108, 0, 20, 20, 108, 0, 20, 20, 108, 0, 20, 20, 0, 5, 0, 0, 1, 1, 5, 25,
  
        0, 1, 1, 5, 25, 0, 0, 5, 9, 0, 0, 0, 0, 75, 5, 5, 0, 25, 0, 1, 1, 5, 0, 25, 0, 4, // 24
 
        0, 0, 75, 5, 9, 0, 25, 0, 1, 5, 1, 4, 0, 0, 0, 1, 73, 0, 0, 74, 0, 0, 0, 0, 5, 0, 
        
        1, 5, 1, 9, 0, 8, 4, 8, 0, 5, 5, 0, 20, 20, 20, 20, 0, 5, 9, 0, 75, 0, 20, 20, 20, 20,
   
        0, 9, 5, 9, 5, 0, 93, 93, 93, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 5, 0, 25, 0, // 27
   
        30, 0, 0, 0, 1, 1, 9, 0, 25, 0, 31, 0, 0, 0, 4, 8, 8, 5, 0, 0, 0, 1, 1, 5, 0, 20,
   
        0, 1, 1, 9, 0, 20, 0, 2, 2, 4, 8, 8, 0, 5, 0, 0, 0, 93, 0, 0, 0, 5, 0, 0, 0, 93,
  
        0, 0, 0, 5, 0, 0, 0, 2, 2, 2, 0, 8, 8, 8, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 30
  
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
   
        0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Medium", score_database2, "Medium");
    }
}