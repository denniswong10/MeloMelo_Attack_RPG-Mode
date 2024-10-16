using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score516 : MonoBehaviour
{
    //      53 (Circle)       54 (Cross)       29 (SailFlag)      201, 12, 202 (HookS)       15 (RisingStar)
    //      85, 86 (RatingFogToggle)        23, 24 (Left/Right)       55, 56 (CurveClosedPattern)
    //      91, 94, 92 (FixedAirAttack)
    //      71 [111, 114] MultipleHitStar     80 (MultipleHitBoomStopStar)        116 (WideItem3)

    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 0, 2, 0, 0, 0, 1, 1, 0, 0, 0, 5, 0, 0, 0, 8, 0, 0, 0, 1, 1, 0, 0, 0, 9, 0,

        0, 20, 0, 20, 0, 0, 0, 133, 0, 0, 93, 0, 0, 133, 0, 0, 93, 0, 0, 1, 0, 5, 0, 1, 0, 9,

        0, 1, 1, 0, 53, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 8, 0, 8, 0, 2, 2, 0, 1, 1, 0, 5, // 3
  
        0, 1, 0, 9, 0, 53, 0, 0, 4, 0, 4, 0, 6, 0, 6, 0, 0, 0, 54, 0, 0, 0, 1, 0, 0, 5,

        0, 0, 1, 0, 0, 9, 0, 0, 15, 0, 15, 0, 0, 2, 2, 0, 8, 8, 0, 0, 0, 53, 0, 0, 2, 0,

        8, 0, 4, 0, 2, 0, 8, 0, 6, 0, 1, 0, 5, 5, 0, 29, 0, 0, 0, 0, 0, 0, 23, 0, 0, 1, // 6
  
        1, 0, 5, 5, 0, 4, 0, 1, 1, 0, 5, 5, 0, 24, 0, 0, 15, 0, 0, 15, 0, 0, 1, 1, 0, 9,

        0, 133, 2, 0, 5, 0, 133, 8, 0, 5, 0, 55, 0, 56, 0, 0, 94, 0, 0, 15, 15, 0, 23, 0, 0, 24,

        0, 0, 29, 0, 20, 20, 0, 0, 0, 2, 0, 8, 0, 116, 0, 1, 1, 0, 5, 0, 9, 0, 0, 0, 2, 2, // 9
  
        0, 116, 0, 8, 8, 0, 116, 0, 5, 0, 9, 0, 0, 1, 0, 5, 0, 116, 0, 0, 1, 0, 9, 0, 116, 0,

        0, 2, 4, 0, 8, 4, 0, 5, 0, 5, 0, 80, 0, 94, 0, 0, 977, 0, 0, 15, 0, 2, 0, 15, 0, 8,

        0, 116, 0, 15, 0, 8, 0, 15, 0, 2, 0, 116, 0, 1, 5, 0, 1, 9, 0, 0, 977, 93, 0, 0, 29, 0, // 12
   
        0, 0, 1, 5, 0, 1, 9, 0, 116, 71, 0, 12, 0, 0, 977, 0, 1, 0, 5, 0, 1, 0, 9, 0, 0, 0,

        133, 0, 2, 2, 0, 8, 8, 0, 5, 9, 0, 0, 133, 0, 15, 0, 15, 0, 1, 5, 0, 1, 9, 0, 20, 20,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 0, 201, 0, 0, 0, 15, 0, 0, 0, 15, 0, 0, 0, 0, 202, 0, 0, 0, 15, 0, 0, 0, 15, 0, 0,
  
        0, 0, 977, 12, 0, 0, 0, 15, 0, 0, 15, 0, 0, 15, 0, 0, 12, 0, 0, 0, 15, 0, 0, 15, 0, 0,
  
        15, 0, 0, 133, 29, 0, 0, 0, 0, 0, 0, 0, 93, 0, 0, 55, 0, 0, 56, 0, 0, 1, 1, 5, 0, 1, // 3
  
        1, 9, 0, 55, 0, 56, 0, 0, 4, 1, 5, 0, 6, 1, 5, 0, 0, 0, 93, 0, 0, 0, 2, 2, 0, 8,
  
        0, 8, 0, 53, 0, 0, 54, 0, 0, 6, 5, 0, 0, 0, 133, 0, 0, 133, 0, 0, 0, 12, 0, 0, 8, 0,
  
        8, 0, 53, 0, 6, 0, 54, 0, 6, 0, 1, 1, 5, 1, 1, 9, 0, 0, 0, 0, 0, 0, 85, 0, 0, 86, // 6
  
        0, 0, 1, 2, 2, 4, 0, 1, 2, 2, 6, 0, 1, 8, 8, 4, 0, 1, 8, 8, 6, 0, 1, 5, 1, 9,
  
        0, 93, 0, 2, 2, 0, 93, 0, 8, 8, 0, 55, 0, 91, 56, 0, 92, 0, 202, 0, 12, 0, 201, 0, 24, 0,
  
        114, 94, 111, 80, 0, 29, 0, 0, 0, 1, 2, 5, 0, 116, 0, 1, 8, 5, 0, 116, 20, 0, 0, 0, 1, 6, // 9
  
        5, 0, 116, 2, 8, 0, 116, 1, 1, 0, 80, 0, 0, 1, 1, 2, 0, 116, 5, 5, 0, 1, 1, 8, 0, 116,
   
        9, 5, 0, 116, 2, 2, 0, 116, 8, 8, 0, 5, 5, 5, 0, 0, 133, 0, 0, 116, 114, 0, 94, 111, 0, 94,
   
        114, 0, 116, 1, 2, 0, 116, 1, 8, 0, 15, 0, 15, 0, 1, 5, 5, 9, 0, 0, 133, 133, 0, 0, 29, 0, // 12
   
        0, 5, 116, 6, 5, 0, 5, 116, 9, 5, 0, 71, 91, 71, 92, 0, 55, 0, 56, 0, 94, 20, 94, 0, 0, 0,
    
        201, 0, 0, 15, 0, 0, 15, 0, 15, 977, 0, 0, 202, 0, 0, 15, 0, 0, 15, 0, 15, 977, 0, 2, 8, 2,
   
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Medium", score_database2, "Medium");
    }
}
