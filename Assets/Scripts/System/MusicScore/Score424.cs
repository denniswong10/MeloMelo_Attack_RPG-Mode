﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score424 : MonoBehaviour
{
    // 113, 114, 46 (6-Key)    101-108, 44 (2-Key)    18, 20, 19 (Fixed Heart)
    // 47 (2-Key Tap)  48 (6-Key Tap)   124, 125, 34 (BoxOfDiamond)   121, 123, 122 (Circle2_S)   111, 112, 76 (MultipleHit)    30, 31 (Sweep)
    // 91, 94, 92 (Fixed Air Attack)    14 (Ribbon)   40 (SplitBomb)   75 (QuickDodgeItem3)    74 (BombWithItem3)

    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 2, 0, 0, 0, 0, 0, 8, 0, 0, 0, 0, 0, 0, 0, 0, 101, 0, 0, 0, 0, 0, 0, 0, 0, 102,

        0, 1, 0, 0, 5, 0, 0, 0, 0, 0, 0, 8, 0, 8, 0, 0, 101, 0, 0, 103, 0, 0, 0, 0, 0, 0,

        0, 1, 0, 1, 0, 5, 0, 0, 48, 0, 0, 0, 0, 48, 0, 0, 0, 0, 30, 0, 0, 0, 31, 0, 0, 0, // 3
   
        30, 0, 0, 0, 31, 0, 0, 0, 94, 0, 1, 1, 0, 30, 0, 20, 0, 31, 0, 20, 0, 30, 0, 20, 0, 31,

        0, 0, 0, 94, 0, 1, 1, 0, 2, 0, 2, 0, 44, 48, 0, 0, 0, 0, 2, 2, 0, 0, 8, 8, 0, 0,

        1, 0, 4, 0, 0, 2, 8, 0, 0, 2, 8, 0, 0, 1, 0, 4, 0, 0, 5, 0, 0, 9, 0, 0, 0, 1, // 6
  
        5, 0, 0, 6, 5, 0, 0, 48, 0, 48, 0, 0, 5, 6, 0, 0, 5, 9, 0, 0, 48, 48, 0, 92, 0, 0,

        0, 30, 0, 0, 0, 31, 0, 0, 0, 1, 0, 1, 0, 5, 0, 34, 0, 0, 0, 34, 0, 0, 0, 34, 0, 0,

        0, 20, 20, 0, 5, 0, 101, 102, 0, 108, 107, 0, 5, 0, 48, 0, 0, 76, 0, 76, 0, 6, 0, 76, 0, 76, // 9
  
        0, 5, 5, 0, 9, 5, 0, 0, 111, 111, 0, 76, 76, 0, 112, 112, 0, 1, 5, 0, 1, 4, 0, 5, 0, 48,

        0, 1, 1, 0, 5, 48, 0, 1, 1, 0, 9, 48, 0, 76, 76, 0, 5, 9, 0, 111, 0, 76, 0, 112, 0, 94,

        0, 30, 20, 0, 31, 20, 0, 34, 20, 0, 124, 20, 0, 0, 125, 20, 0, 0, 0, 101, 102, 0, 108, 107, 0, 5, // 12
   
        0, 9, 0, 14, 0, 0, 14, 20, 0, 14, 0, 0, 14, 20, 0, 5, 5, 0, 48, 0, 48, 0, 0, 1, 0, 1,

        0, 5, 0, 52, 0, 0, 52, 0, 0, 20, 0, 0, 1, 0, 5, 0, 1, 0, 6, 0, 0, 30, 0, 94, 0, 31,

        0, 94, 0, 30, 0, 0, 0, 0, 76, 0, 5, 0, 76, 0, 9, 0, 76, 5, 76, 94, 0, 76, 76, 0, 0, 0, // 15
   
        0, 93, 0, 0, 0, 93, 0, 0, 0, 93, 0, 0, 0, 93, 0, 0, 0, 0, 101, 0, 102, 0, 108, 0, 107, 0,

        20, 0, 20, 0, 5, 0, 5, 0, 20, 20, 0, 124, 0, 0, 0, 34, 0, 0, 0, 125, 0, 0, 0, 4, 1, 0,

        6, 1, 0, 5, 0, 52, 0, 20, 0, 94, 5, 0, 94, 5, 0, 7, 0, 52, 0, 20, 20, 0, 94, 5, 0, 94, // 18
   
        5, 0, 92, 0, 1, 5, 0, 1, 9, 0, 48, 0, 1, 0, 48, 0, 0, 93, 0, 0, 93, 0, 0, 103, 102, 0,

        107, 108, 0, 5, 48, 0, 0, 1, 5, 0, 0, 1, 9, 0, 0, 76, 0, 76, 0, 93, 93, 0, 1, 48, 0, 0,

        1, 48, 0, 0, 76, 0, 76, 0, 0, 48, 0, 48, 0, 0, 101, 102, 0, 103, 104, 0, 105, 106, 0, 107, 108, 0, // 21
   
        40, 0, 1, 5, 0, 76, 76, 0, 94, 0, 1, 9, 0, 76, 76, 0, 9, 0, 5, 0, 30, 20, 0, 31, 20, 0,

        52, 0, 20, 0, 0, 75, 0, 75, 0, 1, 1, 0, 5, 0, 1, 1, 0, 9, 0, 111, 0, 76, 0, 112, 0, 30,

        0, 0, 31, 0, 0, 0, 94, 76, 0, 94, 76, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 24
  
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 46, 0, 0, 0, 0, 0, 113, 0, 0, 0, 0, 0, 0, 0, 0, 114, 0, 0, 0, 0, 0, 0, 0, 0, 101,

        0, 102, 0, 0, 46, 0, 0, 0, 0, 0, 0, 108, 0, 107, 0, 0, 44, 0, 0, 48, 0, 0, 0, 0, 0, 0,

        0, 101, 0, 102, 0, 103, 0, 0, 124, 0, 0, 0, 0, 34, 0, 0, 0, 0, 125, 0, 0, 0, 0, 34, 0, 0, // 3
   
        0, 0, 1, 1, 5, 5, 0, 121, 0, 20, 0, 123, 0, 20, 20, 122, 0, 20, 0, 48, 0, 0, 1, 1, 5, 0,

        34, 0, 20, 0, 4, 1, 5, 0, 6, 1, 5, 0, 8, 4, 0, 0, 0, 0, 1, 5, 0, 0, 1, 9, 0, 0,

        1, 1, 5, 0, 0, 1, 4, 0, 0, 1, 6, 0, 0, 1, 1, 9, 0, 0, 1, 1, 5, 5, 0, 0, 0, 1, // 6
  
        48, 0, 0, 6, 48, 0, 0, 5, 5, 9, 0, 0, 5, 48, 0, 0, 9, 48, 0, 0, 1, 5, 1, 9, 0, 0,

        0, 124, 0, 0, 0, 124, 0, 0, 0, 124, 0, 0, 0, 20, 20, 125, 0, 0, 0, 125, 0, 0, 0, 125, 0, 0,

        0, 4, 1, 1, 5, 0, 48, 0, 102, 107, 46, 0, 1, 1, 5, 0, 0, 121, 0, 18, 0, 123, 0, 20, 0, 122, // 9
  
        0, 19, 0, 4, 1, 5, 0, 0, 111, 94, 0, 112, 94, 0, 111, 94, 0, 1, 1, 5, 0, 1, 1, 4, 0, 5,

        0, 101, 102, 103, 0, 108, 107, 106, 0, 48, 0, 0, 76, 91, 0, 111, 94, 0, 76, 94, 0, 1, 5, 1, 9, 5,

        0, 124, 18, 0, 31, 20, 0, 125, 19, 0, 30, 20, 0, 0, 1, 1, 5, 0, 0, 76, 94, 76, 94, 0, 1, 5, // 12
   
        1, 5, 0, 30, 0, 0, 52, 0, 0, 31, 0, 0, 0, 48, 47, 48, 47, 0, 0, 111, 94, 112, 94, 0, 1, 1,

        5, 0, 30, 18, 0, 52, 20, 0, 31, 19, 0, 0, 1, 5, 4, 0, 1, 5, 6, 0, 0, 76, 94, 5, 0, 76,

        9, 5, 0, 1, 5, 1, 9, 0, 76, 9, 5, 0, 76, 9, 5, 0, 47, 48, 47, 48, 0, 1, 5, 0, 0, 0, // 15
   
        0, 14, 0, 0, 5, 0, 5, 0, 111, 0, 76, 0, 112, 0, 1, 1, 5, 5, 0, 101, 102, 0, 108, 107, 121, 0,

        20, 20, 0, 5, 34, 0, 20, 20, 0, 5, 0, 30, 0, 0, 31, 0, 34, 30, 0, 34, 31, 0, 0, 4, 1, 1,

        1, 5, 9, 0, 0, 52, 0, 20, 0, 122, 0, 20, 0, 123, 0, 20, 0, 52, 0, 20, 0, 121, 0, 20, 0, 123, // 18
   
        0, 20, 20, 0, 1, 1, 5, 0, 1, 1, 9, 0, 1, 1, 5, 0, 31, 0, 0, 30, 0, 0, 101, 102, 103, 0,

        106, 107, 108, 0, 48, 0, 0, 1, 76, 0, 0, 6, 76, 0, 0, 48, 48, 48, 0, 0, 40, 0, 48, 111, 0, 0,

        48, 112, 0, 0, 76, 91, 76, 92, 0, 1, 1, 9, 0, 0, 3, 0, 40, 0, 31, 0, 0, 30, 0, 20, 20, 0, // 21
   
        75, 0, 1, 5, 1, 9, 0, 123, 0, 0, 52, 0, 20, 20, 0, 1, 5, 1, 9, 0, 121, 20, 0, 122, 20, 0,

        52, 0, 20, 20, 0, 1, 1, 5, 0, 30, 18, 20, 0, 31, 19, 20, 0, 124, 0, 20, 0, 124, 0, 20, 20, 34,

        0, 20, 7, 34, 0, 20, 0, 7, 20, 0, 74, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 24
  
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0
    };

    private int[] score_database3 =
    {
        0,

        0, 48, 101, 102, 103, 104, 0, 48, 108, 107, 106, 105, 0, 46, 46, 0, 48, 0, 0, 0, 0, 0, 0, 0, 0, 101,
  
        48, 0, 102, 48, 0, 94, 0, 0, 0, 0, 0, 113, 0, 44, 0, 114, 0, 48, 113, 94, 0, 0, 0, 0, 0, 0,
   
        1, 1, 5, 0, 1, 1, 9, 0, 30, 0, 0, 31, 0, 34, 30, 0, 0, 31, 0, 0, 30, 0, 0, 31, 0, 0, // 3
   
        0, 0, 5, 4, 5, 9, 0, 111, 94, 112, 0, 112, 94, 111, 0, 1, 1, 5, 0, 1, 1, 9, 0, 76, 94, 0,
  
        76, 94, 0, 101, 104, 48, 108, 105, 48, 0, 1, 1, 5, 9, 0, 0, 0, 0, 2, 4, 1, 0, 8, 4, 1, 0,
  
        1, 5, 9, 0, 0, 8, 4, 1, 0, 8, 4, 1, 0, 1, 9, 5, 0, 0, 48, 1, 5, 5, 0, 0, 0, 48, // 6
  
        1, 5, 9, 0, 30, 0, 20, 31, 0, 20, 0, 0, 48, 1, 1, 48, 1, 1, 48, 0, 2, 4, 8, 9, 0, 0,
   
        0, 121, 30, 0, 122, 31, 0, 121, 30, 0, 122, 31, 0, 0, 121, 34, 0, 122, 0, 0, 121, 34, 0, 122, 0, 0,
   
        0, 4, 5, 4, 9, 0, 101, 102, 103, 108, 107, 106, 0, 5, 6, 0, 0, 14, 121, 14, 0, 122, 14, 0, 121, 14, // 9
  
        0, 122, 14, 0, 0, 94, 0, 0, 48, 111, 48, 112, 48, 0, 5, 9, 0, 1, 2, 5, 0, 1, 8, 5, 0, 20,
  
        0, 101, 105, 103, 0, 108, 105, 106, 0, 94, 0, 0, 76, 0, 76, 0, 111, 0, 112, 0, 76, 0, 5, 5, 5, 9,
   
        0, 30, 0, 20, 31, 0, 20, 20, 30, 0, 20, 31, 0, 0, 4, 8, 5, 0, 0, 112, 94, 111, 94, 0, 5, 4, // 12
   
        5, 9, 0, 52, 0, 0, 0, 20, 20, 20, 20, 0, 101, 48, 103, 48, 105, 0, 76, 0, 76, 0, 76, 0, 5, 4,
   
        5, 0, 52, 0, 0, 0, 19, 20, 18, 20, 0, 0, 101, 102, 103, 108, 107, 106, 48, 0, 0, 111, 94, 0, 112, 94,
   
        0, 76, 0, 76, 0, 76, 0, 76, 0, 1, 1, 5, 5, 0, 30, 18, 20, 31, 19, 20, 0, 48, 48, 0, 0, 0, // 15
   
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 5, 0, 5, 0, 5, 0, 0, 1, 1, 5, 5, 9, 0,
   
        30, 0, 0, 75, 0, 31, 0, 0, 75, 0, 30, 0, 18, 31, 0, 20, 30, 0, 19, 31, 0, 20, 20, 20, 20, 0,
   
        1, 1, 5, 0, 0, 124, 123, 20, 0, 123, 0, 20, 125, 123, 0, 20, 123, 0, 20, 20, 20, 0, 30, 0, 20, 31, // 18
   
        0, 20, 20, 0, 1, 76, 20, 0, 1, 76, 94, 0, 111, 94, 112, 0, 101, 102, 103, 102, 103, 104, 0, 108, 107, 106,
   
        107, 106, 105, 0, 5, 9, 0, 1, 1, 5, 0, 76, 76, 0, 1, 1, 9, 5, 0, 0, 40, 0, 40, 0, 40, 0,
   
        40, 0, 48, 48, 48, 48, 0, 94, 0, 20, 20, 20, 0, 0, 3, 0, 7, 0, 3, 0, 7, 0, 3, 0, 5, 0, // 21
   
        9, 0, 2, 5, 8, 9, 0, 121, 0, 20, 122, 0, 20, 123, 0, 20, 0, 1, 1, 5, 123, 0, 18, 121, 0, 20,
  
        122, 0, 20, 20, 0, 76, 0, 76, 0, 0, 75, 101, 102, 103, 75, 104, 105, 106, 74, 0, 108, 107, 106, 74, 105, 104,
  
        103, 74, 0, 76, 94, 0, 111, 20, 112, 20, 111, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 24
  
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
   
        0, 0, 0, 0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Medium", score_database2, "Medium", score_database3, "Medium");
    }
}