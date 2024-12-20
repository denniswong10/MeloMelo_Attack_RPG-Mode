﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score166 : MonoBehaviour
{
    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    
        0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 1, 0, 0,
    
        0, 2, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 5, 0, 0, 2, 0, 0, 4, 0, 0, 2, 0, 0, 4, 0, // 3
    
        0, 5, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2,
    
        0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0,
    
        0, 0, 0, 0, 0, 1, 0, 5, 0, 0, 0, 2, 0, 2, 0, 4, 0, 0, 0, 2, 0, 2, 0, 4, 0, 0, // 6
    
        0, 2, 0, 2, 0, 4, 0, 0, 0, 2, 0, 2, 0, 4, 0, 0, 5, 0, 0, 0, 1, 0, 0, 2, 0, 0,
    
        4, 0, 0, 1, 0, 0, 2, 0, 0, 4, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0,
    
        0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 2, 0, 0, // 9
    
        5, 0, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 1, 0, 0, 0, 2,
    
        0, 5, 0, 0, 1, 0, 1, 0, 5, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0,
    
        0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, // 12
    
        0, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 2, 0, 2, 0, 4, 0, 0, 0, 0, 5, 0, 0, 0,
    
        0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 2, 0, 2, 0, 5, 0, 0, 0, 0, 0,
    
        1, 0, 0, 2, 0, 0, 5, 0, 0, 0, 1, 0, 0, 2, 0, 0, 4, 0, 0, 0, 1, 0, 0, 2, 0, 0, // 15
    
        6, 0, 0, 0, 1, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 5,
    
        0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0,
    
        2, 0, 0, 1, 0, 0, 4, 0, 0, 1, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 4, 0, // 18
    
        0, 1, 0, 0, 2, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 2, 0, 0, 0, 1, 0, 0, 0, 2, 2, 0, 1, 0, 1, 0,

        0, 2, 0, 0, 0, 4, 0, 2, 0, 2, 0, 1, 5, 0, 0, 2, 2, 0, 0, 0, 2, 2, 0, 0, 0, 2, // 3
    
        2, 5, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 1, 0, 0, 0, 2,

        0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0,

        0, 1, 0, 0, 0, 2, 1, 5, 0, 0, 0, 2, 2, 0, 1, 0, 0, 4, 0, 0, 2, 2, 0, 1, 0, 0, // 6
    
        4, 0, 0, 1, 0, 2, 2, 0, 0, 4, 0, 1, 0, 2, 0, 1, 5, 0, 0, 0, 1, 2, 1, 5, 0, 0,

        6, 0, 0, 1, 0, 0, 1, 2, 0, 5, 0, 0, 0, 1, 0, 0, 0, 7, 0, 0, 0, 2, 2, 0, 0, 7,

        0, 0, 0, 5, 0, 0, 0, 7, 0, 0, 0, 2, 2, 0, 0, 7, 0, 0, 0, 5, 0, 0, 0, 7, 0, 1, // 9
    
        5, 0, 0, 0, 0, 1, 0, 1, 0, 2, 0, 0, 0, 1, 0, 1, 0, 4, 0, 0, 0, 1, 0, 1, 0, 5,

        0, 5, 1, 2, 5, 0, 1, 2, 5, 0, 0, 0, 0, 1, 0, 0, 0, 2, 2, 0, 0, 4, 0, 0, 0, 1,

        0, 0, 0, 2, 2, 0, 0, 4, 0, 0, 1, 2, 1, 5, 0, 0, 2, 0, 0, 1, 0, 0, 0, 2, 0, 0, // 12
    
        1, 5, 0, 0, 0, 0, 0, 0, 0, 1, 1, 5, 0, 4, 0, 1, 2, 5, 0, 0, 0, 0, 2, 2, 0, 0,

        0, 7, 0, 0, 2, 2, 0, 0, 0, 7, 0, 0, 5, 0, 1, 1, 2, 0, 2, 1, 5, 0, 0, 0, 0, 0,

        1, 1, 0, 2, 2, 0, 5, 0, 0, 0, 1, 2, 1, 5, 0, 0, 4, 0, 0, 2, 1, 0, 2, 1, 0, 2, // 15
    
        4, 0, 0, 1, 1, 0, 2, 5, 0, 2, 2, 4, 0, 0, 0, 0, 0, 5, 0, 0, 0, 7, 0, 0, 0, 5,

        0, 0, 0, 7, 0, 0, 0, 5, 0, 0, 0, 7, 0, 0, 0, 5, 0, 0, 0, 7, 0, 0, 0, 1, 5, 0,

        2, 2, 0, 1, 0, 0, 4, 0, 1, 1, 0, 2, 5, 0, 0, 0, 0, 0, 0, 0, 0, 2, 1, 2, 4, 0, // 18
    
        1, 1, 0, 2, 2, 0, 1, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Medium", score_database2, "Medium");
    }
}
