﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score188 : MonoBehaviour
{
    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 2, 0, 0, 0, 2, 0, 0, 0, 2, 0, 0, 0, 4, 0, 0, 0, 2, 0, 0, 0, 2, 0, 0, 0, 4,

        0, 0, 0, 5, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 4, 0, 0,
    
        0, 9, 0, 0, 0, 0, 0, 5, 0, 0, 5, 0, 0, 0, 2, 0, 2, 0, 2, 0, 0, 0, 5, 0, 0, 5, // 3
    
        0, 0, 0, 2, 0, 2, 0, 2, 0, 0, 0, 1, 0, 5, 0, 0, 1, 0, 5, 0, 0, 2, 0, 2, 0, 4,
    
        0, 0, 2, 0, 2, 0, 4, 0, 0, 5, 0, 0, 0, 1, 0, 0, 0, 4, 0, 0, 0, 2, 0, 0, 0, 4,
    
        0, 0, 2, 0, 2, 0, 0, 0, 1, 0, 0, 0, 6, 0, 0, 0, 1, 0, 0, 0, 6, 0, 0, 2, 0, 0, // 6
    
        2, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 4, 0, 2, 0, 5, 0, 0, 0, 0, 0, 8, 0, 0, 0, 8,
    
        0, 0, 0, 5, 0, 0, 0, 0, 8, 0, 0, 0, 8, 0, 0, 0, 5, 0, 0, 0, 1, 0, 2, 0, 1, 0,
    
        0, 0, 8, 0, 0, 0, 8, 0, 0, 0, 4, 0, 0, 5, 0, 0, 0, 1, 0, 0, 2, 0, 0, 4, 0, 0, // 9 
    
        5, 0, 0, 1, 0, 1, 0, 2, 0, 0, 0, 6, 0, 0, 1, 0, 2, 0, 0, 1, 0, 2, 0, 0, 0, 4,
    
        0, 0, 2, 0, 0, 5, 0, 0, 1, 0, 0, 2, 0, 5, 0, 0, 4, 0, 0, 0, 8, 0, 2, 0, 8, 0,
    
        0, 5, 0, 0, 0, 0, 0, 6, 0, 0, 2, 0, 2, 0, 0, 0, 9, 0, 0, 2, 0, 0, 8, 0, 0, 0, // 12
    
        5, 0, 1, 0, 1, 0, 5, 0, 0, 0, 9, 0, 0, 8, 0, 0, 9, 0, 0, 0, 8, 0, 0, 8, 0, 0,
    
        9, 0, 0, 0, 1, 0, 0, 2, 0, 2, 0, 2, 0, 0, 4, 0, 0, 2, 0, 2, 0, 2, 0, 0, 0, 1,
    
        0, 0, 4, 0, 0, 1, 0, 0, 2, 0, 0, 2, 0, 0, 5, 0, 0, 5, 0, 0, 1, 0, 0, 8, 0, 8, // 15
    
        0, 5, 0, 0, 0, 0, 1, 0, 0, 2, 0, 2, 0, 0, 4, 0, 0, 2, 0, 2, 0, 0, 4, 0, 0, 1,
    
        0, 2, 0, 8, 0, 0, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    
        0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 1, 1, 2, 0, 4, 2, 0, 0, 1, 1, 5, 0, 2, 4, 0, 0, 1, 1, 2, 0, 2, 4, 0, 0, 1,

        1, 5, 0, 8, 0, 8, 0, 9, 0, 0, 0, 1, 0, 2, 2, 8, 0, 6, 0, 2, 8, 2, 0, 1, 0, 8,

        2, 2, 0, 5, 0, 0, 1, 1, 2, 0, 4, 2, 0, 0, 8, 0, 2, 0, 8, 0, 1, 1, 5, 0, 0, 6, // 3
    
        0, 2, 0, 4, 0, 2, 1, 5, 0, 0, 0, 1, 1, 5, 0, 8, 4, 0, 0, 1, 1, 2, 0, 4, 8, 0,

        0, 1, 1, 2, 0, 8, 4, 0, 0, 1, 1, 5, 0, 4, 8, 0, 0, 2, 0, 4, 0, 5, 0, 0, 0, 1,

        1, 2, 2, 0, 4, 0, 8, 0, 1, 1, 5, 0, 9, 0, 0, 0, 1, 1, 1, 0, 5, 0, 1, 1, 1, 0, // 6
    
        9, 0, 0, 1, 1, 2, 0, 0, 4, 0, 0, 1, 1, 2, 0, 5, 0, 0, 0, 0, 0, 8, 2, 5, 0, 1,

        0, 8, 0, 5, 0, 0, 0, 0, 2, 8, 5, 0, 1, 0, 8, 0, 5, 0, 0, 0, 2, 2, 8, 0, 5, 0,

        0, 0, 1, 1, 2, 0, 4, 0, 1, 1, 5, 0, 0, 6, 0, 0, 1, 1, 1, 0, 2, 0, 1, 1, 1, 0, // 9 
    
        5, 0, 0, 1, 1, 2, 0, 4, 0, 1, 1, 6, 0, 0, 1, 2, 4, 0, 2, 2, 0, 1, 2, 5, 0, 6,

        0, 2, 2, 0, 0, 1, 1, 0, 5, 0, 0, 3, 0, 7, 0, 0, 3, 0, 7, 0, 2, 2, 8, 0, 9, 1,

        1, 5, 0, 0, 0, 0, 0, 6, 7, 0, 2, 2, 8, 0, 0, 1, 1, 2, 0, 5, 0, 0, 3, 1, 9, 0, // 12
    
        1, 1, 2, 0, 8, 0, 1, 1, 5, 0, 9, 0, 0, 1, 2, 1, 5, 0, 0, 1, 1, 0, 2, 8, 0, 0,

        9, 0, 2, 0, 5, 0, 0, 1, 1, 2, 0, 4, 0, 1, 1, 5, 0, 1, 1, 2, 5, 0, 0, 6, 0, 1,

        0, 6, 1, 5, 0, 1, 1, 0, 2, 2, 0, 9, 0, 0, 1, 1, 0, 2, 2, 0, 9, 0, 0, 8, 2, 8, // 15
    
        0, 5, 0, 0, 0, 0, 1, 1, 2, 0, 4, 2, 0, 0, 1, 1, 5, 0, 2, 4, 0, 0, 8, 0, 0, 9,

        0, 8, 0, 9, 1, 1, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Small", score_database2, "Medium");
    }
}