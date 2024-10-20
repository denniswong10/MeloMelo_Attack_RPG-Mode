using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score507 : MonoBehaviour
{
    //    18 [300, 301] Eight        30, 31 (Sweep)       101, 14, 102 (Ribbon)        51, 52 [150, 151] (PoorWand)
    //    81 (BoomStopStar_Random)       87, 88 (CombinePickStar)        80 (BoomStopStar)      50 (Diamond)      56 (DiamondSurrondStar)
    //    70 (ItemStrikeWithItem3)      94 (FixedAirAttack)          20 (FixedHeartPack)

    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 2, 0, 5, 0, 5, 0,

        14, 0, 0, 1, 0, 8, 0, 5, 0, 5, 0, 14, 0, 0, 0, 30, 0, 0, 31, 0, 0, 101, 0, 20, 0, 94, // 3
 
        0, 102, 0, 20, 0, 94, 0, 14, 0, 20, 0, 0, 31, 0, 0, 30, 0, 0, 50, 0, 0, 70, 0, 0, 80, 80,

        0, 80, 80, 0, 14, 0, 20, 94, 0, 0, 300, 0, 0, 301, 0, 0, 14, 0, 0, 20, 20, 0, 94, 94, 0, 1,

        0, 5, 0, 9, 5, 0, 80, 80, 0, 87, 88, 0, 7, 0, 0, 193, 0, 0, 193, 0, 0, 2, 2, 0, 4, 0, // 6
 
        5, 0, 8, 8, 0, 4, 5, 0, 0, 193, 0, 0, 193, 0, 0, 2, 8, 0, 4, 0, 8, 2, 0, 4, 0, 5,

        0, 50, 0, 56, 0, 0, 0, 1, 1, 0, 5, 0, 5, 0, 1, 1, 0, 5, 0, 5, 0, 1, 0, 1, 0, 56,

        0, 18, 0, 0, 0, 20, 20, 0, 94, 0, 94, 0, 81, 2, 0, 81, 8, 0, 5, 0, 9, 0, 5, 0, 0, 51, // 9
 
        0, 0, 52, 0, 0, 0, 0, 7, 0, 7, 0, 0, 101, 0, 14, 0, 102, 0, 18, 0, 0, 102, 0, 14, 0, 101,

        0, 7, 0, 80, 0, 0, 0, 1, 1, 0, 5, 0, 1, 1, 0, 9, 0, 81, 0, 81, 0, 81, 0, 14, 0, 94,

        0, 0, 7, 0, 0, 102, 0, 14, 0, 101, 0, 18, 0, 0, 101, 0, 14, 0, 102, 0, 7, 0, 80, 80, 0, 0, // 12
  
        0, 1, 4, 0, 5, 0, 1, 6, 0, 5, 0, 193, 0, 70, 0, 193, 0, 2, 0, 8, 0, 0, 30, 0, 0, 31,

        0, 0, 30, 0, 0, 300, 0, 20, 301, 0, 20, 14, 0, 0, 94, 94, 0, 50, 0, 56, 0, 56, 0, 0, 3, 0,

        3, 0, 1, 2, 0, 5, 5, 0, 1, 8, 0, 9, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 5, 0, 0, 2, // 15
  
        8, 0, 0, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
 
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 300, 0, 30, 301, 0, 31, 300, 0,
 
        0, 0, 87, 88, 0, 80, 0, 301, 0, 31, 300, 0, 30, 301, 0, 0, 88, 87, 0, 80, 0, 14, 0, 4, 1, 5, // 3
 
        0, 14, 0, 6, 1, 5, 0, 80, 80, 14, 0, 0, 1, 2, 2, 0, 81, 0, 1, 8, 8, 0, 81, 0, 4, 1,
 
        1, 0, 6, 5, 5, 0, 50, 30, 0, 0, 31, 0, 0, 56, 0, 50, 0, 56, 0, 4, 2, 8, 0, 6, 1, 1,
 
        0, 5, 0, 0, 101, 14, 102, 0, 101, 14, 102, 0, 3, 0, 2, 2, 93, 2, 2, 0, 81, 2, 81, 2, 0, 4, // 6
 
        4, 0, 56, 0, 80, 88, 80, 0, 0, 8, 8, 70, 8, 8, 0, 81, 8, 81, 8, 0, 4, 0, 56, 4, 56, 0,
 
        80, 87, 80, 88, 0, 0, 0, 1, 2, 2, 3, 0, 1, 8, 8, 3, 0, 1, 2, 93, 3, 0, 1, 8, 3, 93,
 
        0, 150, 0, 0, 0, 151, 0, 0, 0, 56, 0, 81, 56, 0, 81, 0, 1, 1, 5, 0, 1, 1, 99, 0, 0, 101, // 9
 
        14, 102, 0, 101, 14, 102, 0, 94, 0, 94, 0, 0, 301, 0, 94, 30, 0, 300, 0, 94, 31, 0, 0, 50, 77, 0,
   
        1, 1, 99, 5, 0, 93, 0, 1, 2, 93, 5, 0, 1, 2, 93, 5, 0, 8, 8, 93, 8, 8, 0, 1, 5, 99,
 
        0, 0, 3, 0, 0, 300, 0, 94, 31, 0, 301, 0, 94, 30, 0, 0, 50, 77, 77, 0, 99, 0, 5, 0, 93, 93, // 12
  
        0, 1, 2, 77, 6, 0, 1, 2, 77, 99, 0, 8, 8, 70, 8, 8, 0, 1, 5, 99, 0, 0, 101, 18, 14, 0,
 
        102, 0, 0, 102, 18, 14, 0, 101, 0, 0, 81, 1, 81, 1, 0, 56, 0, 56, 0, 1, 1, 5, 5, 0, 0, 101,

        14, 102, 0, 20, 0, 102, 14, 101, 0, 20, 20, 0, 0, 0, 93, 0, 0, 0, 2, 2, 0, 0, 8, 0, 0, 1, // 15
  
        1, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Medium", score_database2, "Medium");
    }
}
