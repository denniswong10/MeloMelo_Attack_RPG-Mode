using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score496 : MonoBehaviour
{
    //     30, 31 (Big_B)      61, 62 (QuickWideItemStar)     81, 82 (CombinePickStar)      45 (4-Keys)
    //     18, 20, 19 (FixedHeartPack)      28 (Flag)       71, 70 [78, 77] ItemStrikeWithItem3      80 (BoomStopStar)
    //     94 (FixedAirAttack)      13 [102, 103] (People)      108 (DoubleJumpAndAttack)     14 (Ribbon)      40 (SplitBomb)
    //     76 (MultipleHitStar)     29 [106, 107] (SailFlag)

    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 0, 1, 2, 2, 0, 3, 0, 1, 8, 8, 0, 40, 0, 29, 0, 0, 0, 20, 0, 45, 45, 80, 0, 1, 1,

        14, 0, 0, 0, 0, 1, 0, 1, 0, 5, 0, 82, 0, 0, 81, 0, 0, 80, 0, 0, 1, 0, 1, 0, 9, 0,

        45, 81, 0, 0, 45, 82, 0, 0, 78, 0, 77, 0, 28, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 3
  
        2, 0, 2, 0, 2, 0, 0, 8, 0, 8, 0, 8, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 14, 0, 0,

        18, 14, 0, 0, 19, 0, 45, 0, 45, 0, 0, 93, 0, 93, 0, 93, 0, 93, 0, 1, 1, 0, 5, 5, 9, 0,

        0, 0, 78, 0, 78, 0, 0, 77, 0, 77, 0, 0, 45, 80, 45, 0, 1, 1, 0, 93, 0, 1, 1, 0, 93, 0, // 6
  
        5, 5, 0, 9, 0, 0, 2, 2, 2, 0, 78, 0, 8, 8, 8, 0, 77, 0, 2, 8, 2, 0, 78, 77, 0, 8,

        2, 8, 0, 3, 0, 40, 0, 14, 0, 0, 0, 5, 5, 0, 94, 5, 5, 0, 94, 5, 94, 0, 78, 78, 0, 1,

        1, 0, 5, 0, 1, 1, 0, 80, 14, 0, 0, 0, 71, 0, 70, 0, 0, 40, 0, 28, 0, 0, 18, 19, 0, 28, // 9
  
        0, 0, 19, 18, 0, 14, 0, 0, 0, 0, 0, 1, 0, 5, 5, 0, 93, 2, 2, 0, 93, 8, 8, 0, 45, 0,

        0, 1, 5, 0, 1, 9, 0, 1, 1, 5, 0, 1, 1, 9, 0, 40, 0, 14, 0, 0, 93, 8, 8, 0, 93, 2,

        2, 0, 5, 5, 0, 93, 0, 93, 0, 78, 77, 78, 0, 1, 1, 5, 0, 45, 80, 0, 0, 0, 14, 0, 3, 0, // 12
  
        14, 0, 40, 0, 29, 0, 0, 0, 20, 29, 0, 0, 0, 4, 2, 2, 0, 4, 8, 8, 0, 1, 1, 9, 0, 0,

        0, 2, 2, 93, 0, 8, 8, 93, 0, 45, 0, 80, 0, 45, 0, 1, 1, 0, 14, 0, 0, 1, 1, 0, 14, 0,

        0, 94, 0, 0, 0, 2, 0, 0, 0, 8, 8, 8, 0, 0, 5, 0, 0, 0, 14, 0, 18, 18, 14, 0, 19, 19, // 15
  
        0, 45, 80, 0, 0, 0, 0, 0, 0, 28, 0, 0, 0, 1, 1, 5, 0, 29, 0, 0, 0, 1, 1, 9, 0, 0,

        14, 0, 0, 20, 18, 18, 0, 14, 0, 0, 20, 19, 19, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 93, 0, 93, 0, 93, 0, 14, 0, 0, 93, 0, 93, 0, 93, 0, 0, 71, 0, 71, 0, 71, 0, 14, 0, // 18
  
        0, 70, 0, 70, 0, 70, 0, 14, 0, 0, 94, 0, 0, 1, 1, 5, 0, 45, 80, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 0, 30, 0, 0, 0, 18, 0, 19, 0, 0, 31, 0, 0, 0, 19, 0, 18, 0, 0, 1, 1, 5, 0, 78, 77,
 
        0, 80, 0, 0, 0, 2, 8, 2, 5, 0, 20, 0, 82, 45, 0, 5, 4, 5, 0, 0, 8, 2, 8, 2, 5, 0,
  
        20, 0, 81, 45, 0, 5, 5, 9, 0, 78, 77, 0, 28, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 3
  
        2, 0, 0, 8, 8, 0, 0, 93, 0, 0, 93, 0, 0, 5, 80, 0, 0, 0, 0, 0, 0, 0, 0, 61, 0, 19,
  
        19, 62, 0, 18, 18, 0, 1, 1, 5, 0, 45, 82, 0, 18, 18, 0, 45, 81, 0, 19, 19, 0, 20, 94, 20, 0,
  
        0, 0, 102, 0, 0, 0, 108, 14, 0, 0, 103, 0, 0, 0, 19, 20, 0, 1, 4, 5, 0, 81, 45, 0, 18, 82, // 6
  
        45, 0, 93, 80, 0, 0, 3, 2, 2, 8, 0, 77, 78, 77, 0, 70, 8, 8, 2, 0, 78, 77, 78, 0, 40, 2,
  
        2, 8, 0, 1, 5, 9, 0, 80, 80, 0, 0, 76, 20, 0, 76, 76, 20, 0, 76, 76, 76, 0, 20, 94, 0, 1,
  
        5, 5, 4, 0, 1, 5, 5, 9, 0, 80, 0, 0, 3, 0, 40, 0, 3, 70, 0, 106, 0, 18, 0, 107, 0, 19, // 9
  
        0, 29, 0, 94, 28, 0, 0, 0, 0, 0, 0, 14, 0, 0, 82, 45, 93, 81, 45, 0, 1, 5, 1, 9, 0, 0,
  
        0, 78, 1, 1, 77, 1, 1, 0, 71, 2, 2, 70, 8, 8, 0, 3, 0, 14, 0, 0, 81, 45, 93, 82, 45, 0,
   
        1, 9, 1, 5, 0, 3, 0, 3, 0, 78, 1, 5, 77, 1, 5, 0, 20, 20, 94, 0, 0, 0, 1, 5, 5, 9, // 12
  
        0, 103, 0, 0, 0, 108, 14, 0, 0, 102, 0, 0, 0, 18, 20, 0, 2, 4, 8, 0, 1, 9, 1, 5, 0, 0,
  
        0, 2, 4, 2, 0, 8, 4, 8, 0, 45, 82, 0, 45, 81, 0, 8, 2, 8, 0, 14, 0, 20, 94, 20, 0, 80,
  
        0, 80, 0, 0, 0, 93, 0, 0, 0, 93, 0, 93, 0, 0, 3, 0, 0, 0, 94, 20, 18, 0, 94, 20, 19, 0, // 15
  
        78, 78, 80, 0, 0, 0, 0, 0, 0, 28, 0, 0, 94, 0, 94, 0, 0, 28, 0, 0, 40, 20, 0, 20, 0, 0,
  
        106, 0, 18, 20, 107, 0, 19, 20, 29, 0, 20, 20, 40, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
  
        0, 0, 30, 0, 0, 94, 0, 0, 31, 0, 0, 94, 0, 0, 93, 0, 0, 0, 62, 0, 18, 18, 61, 0, 19, 19, // 18
  
        0, 45, 81, 0, 45, 82, 0, 80, 0, 0, 4, 2, 8, 4, 2, 8, 0, 40, 14, 0, 0, 0, 0, 0, 0, 0,
   
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Medium", score_database2, "Medium");
    }
}