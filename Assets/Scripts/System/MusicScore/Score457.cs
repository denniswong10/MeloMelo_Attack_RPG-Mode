using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score457 : MonoBehaviour
{
    //    76 (MultipleHitStar_Random)     111, 71, 112 (MultipleHitStar)
    //   101, 14, 102 (Ribbon)     42, 43 (SemiCurve)
    //    94 (FixedAirAttack)    100 (JumpAcrossTheOtherSide)
    //    46 (6-Key)    18, 20, 19 (FixedHeartPack)
    //    81, 82 (CombinePickStar)   48 (4-KeyTap)
    //    44 (House)   11 (Gender_F)

    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 2, 0, 0, 8, 0, 4, 0, 2, 0, 0, 8, 8, 0, 0, 2, 2, 0, 8, 8, 0, 4, 2, 0, 4, 8,

        0, 5, 0, 0, 1, 1, 0, 5, 0, 5, 0, 0, 1, 1, 0, 5, 0, 9, 0, 0, 101, 0, 0, 14, 0, 0,

        102, 0, 0, 1, 5, 0, 1, 9, 0, 2, 4, 0, 8, 4, 0, 20, 0, 20, 0, 20, 0, 42, 0, 0, 0, 43, // 3
  
        0, 0, 0, 0, 0, 0, 5, 0, 1, 1, 0, 9, 0, 8, 5, 0, 0, 0, 1, 0, 5, 0, 1, 0, 9, 0,

        2, 2, 0, 8, 8, 0, 1, 5, 0, 1, 9, 0, 46, 0, 48, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 1, 1, 0, 5, 0, 9, 0, 1, 1, 0, 76, 0, 9, 0, 2, 0, 4, 0, 8, 0, 4, 5, 0, 9, // 6
  
        0, 0, 0, 0, 0, 1, 5, 0, 4, 0, 100, 0, 1, 5, 0, 76, 0, 9, 0, 2, 0, 4, 0, 8, 0, 4,

        0, 5, 0, 4, 0, 0, 0, 0, 1, 5, 0, 76, 0, 76, 0, 1, 5, 0, 9, 0, 76, 0, 2, 2, 0, 8,

        8, 0, 4, 5, 0, 100, 0, 1, 9, 0, 46, 46, 0, 0, 2, 0, 2, 0, 5, 0, 8, 0, 8, 0, 5, 5, // 9
   
        0, 0, 93, 0, 93, 0, 5, 0, 0, 0, 0, 0, 0, 0, 93, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0,

        0, 0, 0, 0, 7, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 5, 0, 0, 0, 1, 2, 0, 1, 8, 0,

        0, 5, 4, 0, 5, 9, 0, 0, 101, 0, 0, 14, 0, 0, 102, 0, 0, 14, 0, 20, 0, 0, 0, 0, 0, 0, // 12
  
        1, 5, 0, 14, 0, 0, 0, 1, 9, 0, 14, 0, 0, 0, 0, 1, 1, 0, 5, 0, 9, 0, 20, 18, 0, 20,

        19, 0, 20, 0, 1, 5, 0, 1, 9, 0, 76, 0, 46, 0, 1, 2, 0, 1, 8, 0, 46, 0, 76, 0, 46, 0,

        1, 1, 0, 5, 0, 5, 0, 9, 5, 0, 0, 0, 0, 101, 0, 14, 0, 102, 0, 14, 0, 101, 0, 14, 0, 102, // 15
   
        0, 20, 20, 0, 14, 0, 20, 0, 1, 5, 0, 7, 0, 0, 1, 1, 0, 5, 0, 1, 1, 0, 5, 9, 0, 1,

        100, 0, 1, 100, 0, 5, 9, 0, 0, 93, 0, 0, 2, 0, 2, 0, 4, 0, 1, 0, 1, 0, 0, 5, 0, 0,

        0, 8, 0, 8, 0, 4, 0, 1, 0, 1, 0, 5, 0, 5, 0, 46, 46, 0, 0, 0, 0, 0, 1, 1, 0, 5, // 18
   
        5, 0, 14, 0, 0, 0, 1, 1, 0, 9, 0, 14, 0, 0, 0, 46, 0, 0, 0, 2, 0, 2, 0, 8, 0, 8,

        0, 14, 0, 101, 0, 14, 0, 102, 0, 14, 0, 101, 0, 14, 0, 20, 20, 0, 14, 0, 20, 0, 5, 0, 7, 0,

        1, 5, 0, 1, 9, 0, 7, 0, 2, 2, 0, 8, 8, 0, 1, 5, 0, 1, 9, 0, 48, 48, 0, 0, 1, 5, // 21
   
        0, 76, 0, 2, 4, 0, 8, 4, 0, 0, 7, 0, 0, 1, 5, 0, 9, 0, 76, 0, 20, 76, 0, 20, 76, 0,

        0, 5, 0, 9, 0, 0, 93, 0, 93, 0, 0, 1, 1, 0, 5, 5, 0, 9, 0, 2, 0, 8, 0, 46, 0, 48,

        0, 0, 2, 0, 4, 0, 8, 0, 4, 0, 0, 2, 0, 8, 8, 0, 2, 0, 8, 8, 0, 5, 0, 0, 9, 0, // 24
   
        1, 1, 0, 100, 14, 0, 0, 100, 14, 0, 20, 20, 0, 100, 2, 0, 5, 0, 9, 0, 0, 0, 93, 0, 0, 93,

        0, 0, 8, 0, 8, 0, 0, 0, 93, 0, 0, 0, 93, 0, 0, 1, 0, 1, 0, 0, 0, 5, 0, 20, 18, 0,

        20, 19, 0, 5, 0, 14, 0, 20, 20, 0, 0, 2, 5, 0, 8, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 27
   
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 2, 2, 8, 8, 0, 4, 0, 2, 8, 8, 2, 0, 93, 0, 2, 8, 2, 8, 0, 4, 0, 2, 8, 8, 2,
  
        0, 93, 0, 0, 1, 2, 2, 4, 0, 101, 0, 0, 1, 8, 8, 4, 0, 102, 0, 0, 1, 2, 8, 4, 0, 14,
  
        0, 0, 1, 0, 5, 5, 0, 9, 0, 0, 42, 0, 18, 0, 43, 0, 19, 0, 42, 0, 18, 0, 43, 0, 19, 0, // 3
  
        42, 0, 0, 0, 43, 0, 20, 0, 42, 0, 94, 0, 43, 0, 94, 0, 0, 0, 1, 1, 5, 0, 9, 0, 101, 0,
  
        100, 0, 102, 0, 100, 0, 1, 1, 5, 0, 1, 1, 9, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
 
        0, 0, 1, 1, 5, 9, 0, 76, 0, 1, 1, 5, 9, 0, 76, 0, 2, 2, 100, 8, 8, 100, 0, 5, 9, 5, // 6
  
        0, 0, 0, 0, 0, 1, 2, 1, 4, 0, 76, 0, 1, 2, 1, 6, 0, 76, 0, 2, 2, 100, 8, 8, 100, 0,
  
        5, 9, 5, 4, 0, 0, 0, 0, 5, 2, 2, 9, 0, 76, 0, 5, 8, 8, 9, 0, 76, 0, 2, 2, 100, 8,
  
        8, 100, 0, 1, 1, 5, 0, 1, 1, 9, 0, 46, 0, 0, 2, 2, 93, 0, 5, 0, 8, 8, 93, 0, 5, 5, // 9
   
        0, 0, 0, 0, 1, 1, 9, 0, 0, 0, 3, 0, 0, 0, 2, 0, 0, 0, 8, 0, 8, 0, 0, 0, 2, 0,
   
        0, 0, 2, 0, 0, 0, 8, 8, 8, 0, 0, 0, 2, 0, 0, 0, 2, 0, 0, 0, 1, 2, 100, 1, 8, 100,
   
        0, 5, 2, 100, 5, 8, 100, 0, 14, 0, 94, 0, 101, 0, 94, 0, 102, 0, 94, 0, 0, 0, 0, 0, 0, 0, // 12
  
        1, 1, 5, 14, 0, 0, 0, 1, 1, 9, 14, 0, 0, 0, 0, 1, 5, 1, 9, 0, 14, 0, 20, 101, 0, 20,
  
        102, 0, 20, 0, 1, 1, 5, 0, 1, 1, 9, 0, 76, 0, 1, 2, 5, 0, 1, 8, 9, 0, 76, 0, 2, 2,
 
        100, 8, 8, 100, 1, 1, 5, 0, 9, 0, 0, 0, 0, 14, 18, 20, 19, 0, 14, 19, 20, 18, 0, 101, 18, 14, // 15
   
        20, 102, 19, 14, 20, 101, 18, 18, 20, 20, 0, 7, 0, 0, 1, 1, 5, 0, 1, 1, 9, 0, 1, 2, 100, 1,
  
        8, 100, 8, 8, 5, 0, 9, 0, 0, 7, 0, 0, 46, 46, 46, 46, 46, 0, 1, 5, 9, 0, 0, 3, 0, 0,
   
        0, 1, 5, 1, 9, 48, 0, 1, 2, 5, 0, 1, 8, 9, 0, 5, 9, 0, 0, 7, 3, 0, 0, 1, 1, 5, // 18
   
        5, 0, 14, 0, 0, 0, 1, 1, 5, 9, 0, 14, 0, 0, 0, 5, 0, 0, 0, 7, 0, 3, 0, 7, 0, 3,
   
        0, 101, 20, 18, 20, 0, 102, 20, 19, 20, 0, 14, 0, 101, 0, 14, 94, 102, 0, 14, 94, 0, 5, 0, 0, 0,
   
        1, 1, 5, 0, 1, 1, 9, 0, 2, 2, 100, 8, 8, 100, 1, 5, 100, 1, 9, 0, 46, 48, 0, 0, 1, 5, // 21
   
        1, 9, 0, 2, 2, 4, 8, 8, 0, 0, 93, 0, 0, 1, 1, 5, 9, 0, 2, 2, 46, 100, 8, 8, 46, 100,
   
        0, 5, 5, 9, 0, 0, 46, 0, 46, 0, 0, 1, 1, 5, 0, 1, 1, 9, 0, 2, 100, 8, 100, 46, 100, 48,
   
        0, 0, 81, 0, 46, 0, 82, 0, 48, 0, 11, 0, 0, 0, 0, 44, 0, 0, 0, 0, 0, 0, 1, 1, 5, 0, // 24
   
        1, 1, 9, 0, 14, 101, 14, 102, 20, 20, 0, 81, 94, 82, 0, 46, 100, 0, 94, 0, 0, 0, 2, 0, 0, 2,
   
        0, 0, 8, 0, 8, 0, 0, 0, 2, 0, 0, 0, 2, 0, 0, 8, 8, 8, 0, 1, 1, 5, 0, 14, 20, 20,
   
        0, 1, 1, 9, 0, 14, 20, 20, 20, 0, 0, 1, 5, 0, 1, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 27
   
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Medium", score_database2, "Medium");
    }
}