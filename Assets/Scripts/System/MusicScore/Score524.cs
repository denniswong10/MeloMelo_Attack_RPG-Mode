using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score524 : MonoBehaviour
{
    //      91, 94, 92 (FixedAirAttack)       18, 20, 19 (FixedHeartPack)
    //      14, 15 (CurveLeft/CurveRight)       30, 31 (Sweep)       118, 119 (FixedAllHeartPack)
    //      81, 82 (MultipleHitBoomStopStar)       76 (MultipleHitStar)
    //      45, 46 (4-6Keys)       53 [101, 102] Circle      114, 115 (PatternFollowStar)
    //      50, 51 (QuickWideItemDash)

    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 2, 2, 0, 93, 0, 0, 5, 0, 9, 0, 0, 8, 8, 0, 93, 0, 0, 5, 0, 9, 0, 1, 1, 0, 5,

        5, 0, 9, 0, 0, 0, 0, 2, 2, 0, 45, 0, 8, 8, 0, 46, 0, 1, 0, 5, 5, 0, 5, 5, 0, 3,

        0, 3, 0, 93, 0, 93, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 8, 0, 5, 0, 0, 0, // 3
  
        8, 0, 2, 0, 5, 0, 5, 0, 30, 0, 0, 30, 0, 0, 91, 0, 91, 0, 0, 0, 0, 0, 0, 0, 0, 2,

        2, 0, 8, 8, 0, 1, 0, 5, 0, 1, 0, 9, 0, 76, 0, 1, 5, 0, 1, 9, 0, 3, 3, 0, 14, 0,

        0, 15, 0, 0, 45, 5, 0, 45, 9, 0, 46, 46, 0, 1, 5, 0, 1, 9, 0, 5, 5, 0, 50, 0, 51, 0, // 6
  
        20, 20, 0, 0, 3, 93, 0, 15, 0, 0, 14, 0, 0, 45, 5, 0, 45, 9, 0, 46, 46, 0, 1, 0, 5, 0,

        9, 0, 76, 0, 76, 0, 94, 0, 94, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0,

        8, 0, 0, 5, 0, 2, 2, 0, 4, 0, 8, 8, 0, 4, 0, 101, 53, 0, 102, 53, 0, 1, 0, 5, 0, 94, // 9
  
        94, 0, 0, 0, 0, 0, 0, 0, 2, 0, 8, 0, 2, 0, 8, 0, 2, 0, 1, 5, 0, 1, 9, 0, 45, 0,

        0, 2, 2, 0, 8, 8, 0, 1, 5, 0, 1, 9, 0, 0, 0, 50, 18, 0, 51, 19, 0, 2, 2, 0, 5, 0,

        8, 8, 0, 5, 9, 0, 31, 0, 0, 31, 0, 0, 92, 92, 0, 20, 0, 20, 0, 0, 1, 1, 0, 5, 0, 1, // 12
  
        1, 0, 5, 0, 46, 0, 77, 0, 77, 0, 14, 0, 15, 0, 14, 0, 15, 0, 119, 0, 118, 0, 94, 0, 94, 0,

        0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 2, 2, 8, 45, 0, 2, 8, 2, 8, 0, 2, 2, 2, 0, 8, 8, 46, 0, 4, 2, 0, 4, 8, 0, 1,
  
        1, 5, 5, 0, 0, 7, 0, 0, 101, 20, 0, 45, 46, 92, 0, 0, 102, 20, 0, 46, 45, 91, 0, 53, 0, 7,
  
        0, 7, 0, 1, 5, 0, 1, 9, 0, 0, 0, 0, 93, 0, 0, 93, 0, 0, 2, 8, 8, 4, 0, 1, 5, 0, // 3
  
        1, 9, 0, 8, 2, 2, 4, 0, 31, 18, 0, 30, 92, 0, 45, 46, 45, 0, 0, 7, 0, 0, 3, 0, 0, 2,
  
        93, 0, 8, 93, 0, 76, 0, 1, 1, 5, 0, 0, 18, 20, 0, 19, 20, 0, 81, 94, 0, 114, 20, 0, 14, 0,
  
        0, 15, 0, 0, 94, 0, 76, 20, 18, 20, 19, 45, 0, 76, 0, 1, 1, 5, 1, 1, 9, 0, 76, 20, 81, 0, // 6
  
        18, 82, 0, 0, 115, 20, 0, 15, 0, 0, 14, 0, 0, 1, 5, 1, 9, 45, 0, 76, 0, 1, 1, 5, 1, 1,
  
        9, 0, 76, 76, 20, 81, 0, 19, 82, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 93, 0,
  
        93, 0, 45, 46, 45, 0, 1, 2, 4, 0, 1, 8, 4, 0, 3, 101, 118, 0, 7, 102, 119, 0, 52, 0, 53, 94, // 9
  
        94, 0, 0, 0, 0, 0, 0, 0, 2, 2, 8, 50, 0, 2, 8, 2, 8, 0, 2, 8, 2, 0, 8, 2, 8, 0,
  
        0, 1, 1, 5, 0, 1, 1, 9, 0, 45, 46, 51, 0, 0, 0, 1, 76, 0, 76, 9, 0, 1, 1, 5, 5, 0,
  
        20, 19, 20, 18, 81, 0, 31, 19, 0, 30, 91, 0, 46, 45, 46, 0, 20, 94, 0, 0, 1, 1, 5, 0, 93, 0, // 12
  
        93, 0, 1, 1, 9, 0, 3, 0, 3, 0, 30, 0, 14, 119, 0, 30, 0, 15, 118, 0, 45, 5, 0, 46, 9, 0,
   
        0, 0, 0, 0, 0, 0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Medium", score_database2, "Medium");
    }
}
