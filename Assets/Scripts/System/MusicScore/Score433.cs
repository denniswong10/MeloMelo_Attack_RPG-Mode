using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score433 : MonoBehaviour
{
    // 30, 31 (BigB)    76 (MultipleHitStar)    71, 72 (MiracleStarPath)    54, 55, 56, 57 (ScratchMark)   52 (Circle2)    47 (4-KeyTap)
    // 81, 82 (CombineStarHit)

    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 2, 0, 0, 0, 8, 0, 0, 1, 0, 5, 0, 9, 0, 0, 2, 0, 0, 8, 0, 0, 4, 0, 5, 0, 5,

        0, 9, 0, 0, 0, 93, 0, 93, 0, 93, 0, 0, 5, 0, 93, 0, 93, 0, 93, 0, 0, 9, 0, 0, 2, 0,

        2, 0, 8, 0, 8, 0, 5, 0, 0, 0, 9, 0, 0, 93, 0, 2, 0, 93, 0, 8, 0, 5, 0, 5, 0, 76, // 3
   
        0, 0, 0, 93, 0, 2, 0, 93, 0, 8, 0, 5, 0, 5, 0, 76, 0, 0, 20, 0, 20, 0, 76, 0, 20, 0,

        20, 0, 76, 0, 20, 0, 20, 0, 76, 0, 9, 0, 1, 0, 81, 0, 1, 0, 82, 0, 0, 20, 0, 76, 0, 20,

        0, 76, 0, 20, 0, 76, 0, 20, 0, 20, 0, 1, 0, 47, 0, 0, 3, 0, 0, 0, 5, 0, 5, 0, 76, 0, // 6
  
        0, 0, 9, 0, 5, 0, 0, 0, 1, 0, 1, 0, 5, 0, 47, 0, 47, 0, 0, 0, 3, 0, 0, 0, 30, 0,

        0, 31, 0, 0, 30, 0, 0, 31, 0, 0, 0, 5, 0, 31, 0, 0, 30, 0, 0, 31, 0, 0, 30, 0, 0, 0,

        9, 0, 0, 71, 0, 0, 0, 5, 0, 5, 0, 5, 0, 76, 0, 76, 0, 93, 0, 0, 72, 0, 0, 5, 0, 5, // 9
  
        0, 5, 0, 76, 0, 6, 0, 76, 0, 0, 5, 0, 9, 0, 30, 0, 20, 0, 31, 0, 20, 0, 30, 0, 20, 0,

        5, 0, 0, 71, 0, 0, 72, 0, 0, 71, 0, 0, 3, 0, 0, 5, 0, 9, 0, 5, 0, 81, 0, 82, 0, 47,

        0, 47, 0, 9, 0, 0, 93, 0, 0, 93, 0, 0, 2, 0, 2, 0, 8, 0, 8, 0, 5, 0, 30, 0, 0, 31, // 12
  
        0, 0, 30, 0, 0, 20, 0, 0, 0, 1, 0, 6, 0, 1, 0, 82, 0, 81, 0, 47, 0, 47, 0, 93, 0, 0,

        1, 0, 0, 5, 0, 0, 1, 0, 0, 9, 0, 76, 0, 0, 1, 0, 1, 0, 81, 0, 1, 0, 1, 0, 82, 0,

        47, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 2, 0, 8, 0, 5, 0, 0, 2, 0, 8, 0, 9, 0, 0, 2, 8, 0, 5, 0, 1, 1, 4, 0, 76, 20,
  
        76, 20, 5, 0, 0, 30, 0, 0, 0, 1, 1, 5, 5, 0, 31, 0, 0, 0, 1, 1, 5, 5, 0, 0, 2, 0,
   
        0, 8, 0, 0, 1, 1, 5, 0, 1, 1, 9, 0, 0, 71, 0, 0, 0, 76, 0, 5, 76, 0, 1, 1, 5, 0, // 3
   
        9, 0, 0, 72, 0, 0, 0, 5, 76, 0, 76, 0, 76, 9, 0, 3, 0, 56, 57, 20, 0, 56, 20, 57, 0, 20,
  
        57, 56, 57, 0, 20, 56, 20, 57, 20, 0, 6, 0, 1, 2, 5, 0, 1, 8, 5, 0, 0, 54, 20, 55, 0, 20,
  
        55, 54, 55, 0, 20, 55, 20, 54, 0, 6, 0, 1, 1, 9, 0, 0, 1, 1, 76, 0, 1, 76, 0, 1, 1, 5, // 6
  
        0, 76, 20, 76, 20, 76, 0, 5, 9, 0, 1, 4, 5, 0, 1, 6, 5, 0, 0, 93, 0, 93, 0, 0, 30, 0,
  
        0, 71, 0, 0, 0, 52, 0, 0, 0, 20, 20, 0, 0, 31, 0, 0, 72, 0, 0, 0, 52, 0, 0, 0, 20, 0,
  
        5, 0, 0, 1, 1, 76, 76, 0, 2, 8, 5, 0, 47, 47, 76, 76, 0, 8, 5, 0, 8, 47, 0, 81, 47, 81, // 9
  
        0, 82, 47, 82, 0, 2, 8, 5, 0, 0, 2, 8, 9, 0, 30, 0, 0, 71, 0, 20, 0, 52, 0, 0, 0, 1,
  
        1, 5, 0, 31, 0, 0, 72, 0, 20, 0, 52, 0, 20, 20, 0, 8, 8, 0, 1, 5, 0, 1, 4, 1, 9, 0,
  
        1, 1, 5, 9, 0, 0, 93, 0, 0, 93, 0, 0, 2, 4, 0, 8, 4, 0, 1, 1, 0, 5, 0, 30, 0, 0, // 12
  
        31, 0, 0, 5, 5, 9, 0, 0, 0, 1, 6, 0, 5, 0, 71, 0, 0, 72, 0, 0, 5, 5, 9, 0, 0, 0,
  
        1, 1, 0, 30, 20, 0, 71, 0, 20, 0, 5, 9, 5, 0, 0, 1, 1, 0, 31, 20, 0, 72, 0, 0, 76, 0,
   
        9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Medium", score_database2, "Medium");
    }
}