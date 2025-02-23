﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score176 : MonoBehaviour
{
    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    
        1, 0, 0, 0, 1, 0, 0, 0, 5, 0, 0, 0, 2, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 5, 0, // 3
    
        0, 0, 2, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 5, 0, 0, 0, 1, 0, 0, 2, 2, 0, 5, 0,
    
        0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0,
    
        0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, // 6
    
        0, 0, 4, 2, 0, 0, 0, 0, 0, 0, 2, 4, 0, 0, 0, 0, 0, 0, 0, 4, 2, 0, 0, 0, 0, 0,
    
        1, 0, 0, 0, 2, 2, 0, 0, 1, 0, 0, 0, 2, 2, 0, 0, 5, 0, 0, 0, 1, 0, 0, 0, 2, 0,
    
        0, 0, 4, 0, 0, 0, 5, 0, 0, 0, 2, 2, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 4, 0, 0, 0, // 9
    
        5, 0, 0, 0, 2, 2, 0, 0, 4, 0, 0, 0, 2, 2, 0, 0, 6, 0, 0, 0, 1, 0, 0, 0, 4, 0,
    
        0, 0, 5, 0, 0, 0, 2, 2, 0, 0, 1, 0, 0, 0, 2, 2, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0,
    
        4, 0, 0, 0, 2, 2, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 4, 0, // 12
    
        0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0,
    
        2, 2, 0, 0, 5, 0, 0, 0, 1, 0, 0, 2, 0, 0, 2, 0, 0, 4, 0, 0, 0, 6, 0, 0, 2, 0,
    
        0, 2, 0, 0, 5, 0, 0, 2, 0, 0, 2, 0, 0, 4, 0, 0, 0, 2, 2, 0, 0, 1, 0, 0, 0, 2, // 15
    
        2, 0, 0, 4, 0, 0, 0, 2, 2, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0,
    
        0, 0, 2, 2, 0, 0, 4, 0, 0, 0, 2, 2, 0, 0, 6, 0, 0, 0, 2, 0, 0, 4, 0, 0, 0, 2,
    
        0, 0, 4, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 4, 0, 0, 0, 5, 0, 0, 0, 2, 2, 0, 0, // 18
    
        4, 0, 0, 0, 2, 2, 0, 0, 6, 0, 0, 0, 1, 0, 0, 6, 0, 0, 0, 1, 0, 0, 6, 0, 0, 0,
    
        1, 0, 0, 0, 2, 0, 0, 0, 4, 0, 0, 0, 5, 0, 0, 0, 1, 0, 2, 0, 5, 0, 0, 1, 0, 2,
    
        0, 5, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 2, 0, 0, 0, 0, 2, 2, 0, // 21
    
        0, 0, 0, 5, 0, 0, 0, 2, 0, 0, 0, 4, 0, 0, 0, 5, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0,
    
        0, 4, 0, 0, 0, 5, 0, 0, 0, 2, 2, 0, 0, 4, 0, 0, 0, 2, 2, 0, 0, 6, 0, 0, 0, 1,
    
        0, 0, 0, 2, 0, 0, 0, 4, 0, 0, 0, 5, 0, 0, 0, 2, 2, 0, 0, 1, 0, 0, 0, 2, 2, 0, // 24
    
        0, 6, 0, 0, 0, 5, 0, 0, 0, 4, 0, 0, 0, 5, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 2, 0,
    
        0, 2, 0, 0, 4, 0, 0, 0, 1, 0, 0, 2, 2, 0, 0, 0, 6, 0, 0, 2, 2, 0, 0, 0, 4, 0,
    
        0, 0, 2, 0, 0, 0, 4, 0, 0, 0, 5, 0, 0, 0, 4, 0, 0, 2, 2, 0, 0, 0, 6, 0, 0, 2, // 27
    
        2, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 4, 0, 0, 0, 5, 0, 0, 0, 4, 0, 0, 0, 2, 0,
    
        0, 0, 1, 0, 0, 0, 2, 2, 0, 0, 5, 0, 0, 0, 2, 0, 0, 4, 0, 0, 2, 0, 0, 5, 0, 0,
    
        0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, // 30
    
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_datbase2 =
    {
        0,

        0, 0, 0, 0, 2, 0, 0, 0, 4, 0, 0, 0, 2, 0, 0, 0, 5, 0, 0, 0, 0, 2, 0, 0, 0, 4,

        0, 0, 0, 2, 0, 0, 0, 1, 2, 2, 2, 0, 6, 0, 2, 2, 2, 5, 0, 1, 4, 5, 0, 0, 0, 0,

        2, 2, 0, 0, 1, 0, 0, 0, 2, 2, 0, 0, 6, 0, 0, 0, 2, 2, 5, 0, 4, 0, 0, 0, 2, 2, // 3
    
        0, 0, 7, 0, 0, 0, 2, 2, 0, 0, 3, 0, 0, 0, 2, 2, 5, 0, 6, 0, 0, 1, 4, 2, 5, 0,

        0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0, 0, 0, 4, 0, 0, 0, 0, 2, 0, 0, 0, 0, 2, 0, 0,

        0, 0, 1, 0, 2, 2, 5, 0, 0, 0, 1, 0, 2, 2, 6, 0, 0, 0, 1, 0, 2, 2, 4, 0, 0, 0, // 6
    
        1, 0, 2, 2, 0, 5, 0, 0, 0, 1, 0, 2, 2, 0, 4, 0, 0, 0, 1, 0, 2, 2, 0, 6, 0, 0,

        1, 1, 5, 0, 2, 2, 0, 0, 1, 1, 6, 0, 2, 2, 0, 0, 1, 1, 4, 0, 2, 2, 0, 0, 1, 1,

        2, 5, 0, 0, 0, 0, 2, 0, 0, 0, 4, 0, 0, 0, 2, 0, 0, 0, 1, 0, 0, 0, 6, 0, 0, 0, // 9
    
        1, 0, 4, 0, 2, 2, 0, 0, 1, 0, 6, 0, 2, 2, 0, 0, 5, 0, 4, 0, 2, 2, 1, 0, 5, 0,

        0, 0, 2, 2, 6, 0, 2, 2, 0, 1, 0, 1, 0, 2, 2, 5, 0, 1, 0, 1, 0, 2, 2, 0, 0, 4,

        0, 2, 0, 2, 2, 1, 0, 0, 5, 0, 0, 0, 7, 0, 0, 0, 2, 0, 0, 0, 7, 0, 0, 0, 4, 0, // 12
    
        0, 0, 7, 0, 0, 0, 2, 0, 0, 0, 7, 0, 0, 0, 5, 0, 0, 0, 2, 2, 0, 0, 4, 0, 0, 0,

        2, 2, 0, 0, 5, 0, 0, 0, 2, 2, 5, 0, 0, 0, 2, 2, 0, 4, 0, 2, 0, 6, 0, 2, 2, 0,

        0, 1, 2, 2, 5, 0, 0, 1, 0, 0, 2, 2, 0, 7, 0, 0, 0, 6, 0, 0, 2, 2, 0, 7, 0, 0, // 15
    
        1, 0, 2, 2, 0, 4, 0, 2, 2, 1, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 2,

        2, 2, 2, 1, 2, 5, 2, 1, 2, 2, 2, 1, 2, 5, 2, 1, 2, 2, 2, 1, 2, 5, 0, 0, 0, 1,

        0, 0, 6, 0, 0, 0, 1, 0, 0, 0, 5, 2, 2, 2, 2, 2, 1, 2, 3, 2, 1, 2, 2, 2, 1, 2, // 18
    
        3, 2, 1, 2, 2, 2, 1, 2, 5, 0, 0, 0, 4, 0, 0, 1, 0, 0, 2, 2, 6, 0, 0, 5, 0, 0,

        1, 2, 2, 1, 2, 7, 2, 2, 1, 2, 3, 2, 2, 1, 2, 5, 0, 0, 0, 1, 0, 0, 2, 2, 0, 4,

        0, 0, 1, 0, 0, 2, 2, 0, 6, 0, 0, 1, 0, 0, 2, 0, 0, 2, 2, 5, 0, 0, 0, 1, 1, 2, // 21
    
        0, 4, 0, 1, 1, 2, 0, 4, 0, 0, 2, 2, 0, 4, 0, 2, 2, 0, 4, 0, 2, 2, 0, 5, 0, 0,

        0, 1, 0, 2, 0, 7, 0, 2, 0, 3, 0, 5, 0, 2, 0, 7, 5, 0, 0, 0, 1, 6, 2, 5, 0, 0,

        0, 1, 0, 2, 0, 3, 0, 4, 0, 2, 0, 2, 0, 3, 0, 2, 2, 0, 1, 0, 2, 2, 0, 4, 0, 2, // 24
    
        2, 0, 5, 0, 4, 0, 5, 0, 0, 0, 2, 2, 0, 6, 0, 0, 0, 2, 2, 0, 4, 0, 0, 1, 0, 0,

        2, 2, 4, 0, 2, 2, 5, 0, 0, 1, 0, 2, 2, 0, 2, 0, 6, 0, 0, 2, 2, 0, 2, 0, 4, 0,

        0, 0, 7, 2, 0, 0, 7, 2, 0, 0, 5, 0, 0, 0, 1, 0, 4, 0, 2, 0, 2, 2, 0, 0, 0, 1, // 27
    
        0, 2, 2, 0, 4, 0, 0, 1, 0, 2, 2, 0, 4, 0, 0, 0, 1, 0, 2, 2, 0, 6, 0, 0, 0, 1,

        0, 2, 2, 0, 5, 0, 0, 2, 0, 0, 4, 0, 0, 2, 2, 0, 0, 5, 0, 0, 2, 2, 0, 5, 0, 0,

        0, 0, 7, 2, 3, 0, 0, 7, 0, 0, 3, 2, 7, 0, 3, 2, 0, 1, 0, 2, 2, 0, 5, 0, 0, 0, // 30
    
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Medium", score_datbase2, "Medium");
    }
}
