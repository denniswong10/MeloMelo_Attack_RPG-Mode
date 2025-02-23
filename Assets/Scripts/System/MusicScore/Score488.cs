using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score488 : MonoBehaviour
{
    //     53 (Circle)      54 (Cross)     56, 57 (ScratchMark)    58, 59 (KeyFollowTile)
    //     18 [300, 301] Eight      31, 30 (Sweep)
    //     60, 61, 62 (FixedHeartPack)      81, 82 (CombinePickStar)
    //     104, 105 (MultipleHitHeart)      13 [102, 103] (People)     107, 108 (BlindSpot_Bombed)
    //     100 (JumpAcrossTheOtherSide)     72, 73 (QuickPickItem)      70 [77] (ItemStrike3)

    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 5, 0, 1, 1, 9, 0, 0, 2, 0, 2, 0, 2, 0, 0, 54, 0,

        0, 0, 1, 1, 5, 0, 1, 1, 9, 0, 0, 8, 0, 2, 0, 8, 0, 0, 1, 5, 0, 9, 5, 5, 0, 53,

        0, 0, 61, 0, 0, 2, 4, 2, 0, 8, 4, 8, 0, 5, 0, 0, 0, 0, 82, 0, 0, 0, 81, 0, 0, 0, // 3
  
        82, 0, 0, 0, 80, 0, 80, 0, 0, 0, 81, 0, 0, 0, 82, 0, 0, 0, 81, 0, 0, 0, 80, 80, 0, 77,

        0, 0, 1, 5, 0, 1, 9, 0, 80, 80, 0, 1, 5, 5, 0, 9, 5, 5, 0, 2, 8, 2, 0, 1, 1, 9,

        0, 0, 2, 2, 0, 8, 8, 0, 2, 2, 0, 8, 8, 0, 56, 60, 0, 57, 62, 0, 80, 0, 80, 0, 0, 2, // 6
  
        0, 8, 8, 0, 0, 1, 1, 0, 5, 0, 59, 0, 0, 0, 1, 1, 0, 5, 5, 58, 0, 0, 0, 0, 80, 0,

        0, 1, 1, 0, 5, 5, 0, 80, 0, 0, 0, 1, 2, 2, 0, 1, 8, 8, 0, 53, 0, 0, 54, 0, 0, 0,

        0, 1, 1, 5, 0, 1, 1, 9, 0, 2, 8, 0, 2, 8, 0, 1, 5, 0, 1, 9, 0, 2, 8, 2, 0, 77, // 9
   
        0, 30, 0, 0, 31, 0, 0, 30, 0, 0, 31, 0, 0, 61, 61, 0, 0, 61, 61, 0, 0, 0, 1, 0, 0, 5,

        0, 0, 93, 0, 0, 1, 0, 0, 5, 5, 0, 80, 0, 0, 1, 0, 5, 0, 77, 0, 0, 80, 0, 0, 80, 0,

        0, 0, 2, 2, 93, 0, 0, 8, 8, 93, 0, 0, 1, 1, 0, 5, 9, 0, 80, 0, 53, 0, 0, 54, 0, 0, // 12
  
        0, 2, 2, 4, 0, 0, 8, 8, 4, 0, 0, 70, 0, 70, 0, 70, 0, 1, 5, 5, 0, 9, 5, 5, 0, 80,

        0, 0, 0, 0, 0, 300, 0, 0, 0, 300, 0, 0, 0, 100, 301, 0, 0, 0, 301, 0, 0, 0, 977, 0, 0, 0,

        0, 82, 0, 0, 81, 0, 0, 82, 0, 0, 80, 80, 0, 0, 300, 0, 0, 100, 301, 0, 0, 0, 61, 61, 0, 80, // 15
   
        0, 80, 0, 5, 53, 0, 0, 0, 9, 54, 0, 0, 0, 2, 4, 2, 0, 70, 0, 77, 0, 8, 4, 8, 0, 70,

        0, 77, 0, 1, 0, 5, 0, 1, 0, 5, 0, 77, 0, 77, 0, 80, 0, 0, 0, 0, 2, 4, 2, 0, 5, 5,

        0, 1, 1, 5, 0, 70, 0, 70, 0, 8, 4, 8, 0, 0, 0, 0, 0, 0, 53, 0, 0, 54, 0, 0, 0, 0, // 18
   
        0, 1, 0, 5, 0, 77, 0, 0, 70, 0, 1, 1, 5, 0, 70, 77, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 300, 0, 104, 300, 0, 0, 31, 0, 0, 301, 0, 105, 301, 0, 0, 0, 54, 0,
  
        0, 0, 133, 0, 1, 0, 0, 133, 0, 66, 0, 0, 301, 0, 0, 301, 105, 0, 30, 0, 0, 300, 0, 0, 300, 104,
  
        0, 0, 53, 0, 0, 56, 60, 61, 0, 57, 62, 61, 0, 80, 0, 0, 0, 0, 81, 300, 0, 0, 100, 301, 0, 0, // 3
  
        100, 300, 0, 104, 0, 82, 301, 0, 0, 100, 300, 0, 0, 100, 301, 0, 105, 54, 0, 0, 80, 0, 0, 56, 0, 30,
  
        0, 0, 0, 57, 0, 31, 0, 0, 0, 80, 1, 1, 5, 0, 80, 1, 1, 9, 0, 56, 0, 57, 0, 61, 61, 80,
  
        0, 0, 93, 0, 0, 93, 0, 0, 93, 0, 93, 0, 0, 56, 60, 61, 0, 57, 62, 61, 0, 80, 53, 0, 0, 0, // 6
  
        0, 3, 0, 0, 0, 1, 1, 5, 0, 56, 59, 0, 0, 0, 1, 1, 9, 0, 57, 58, 0, 0, 0, 80, 54, 0,
  
        0, 0, 80, 0, 61, 61, 61, 80, 0, 0, 0, 72, 77, 0, 104, 53, 0, 0, 0, 73, 77, 0, 105, 54, 0, 0,
   
        0, 80, 0, 80, 0, 70, 80, 0, 0, 2, 8, 0, 0, 1, 5, 1, 5, 0, 77, 0, 1, 9, 1, 9, 0, 77, // 9
   
        0, 30, 0, 977, 0, 77, 31, 0, 3, 0, 77, 30, 0, 977, 0, 18, 0, 0, 61, 0, 0, 0, 107, 0, 8, 8,
  
        0, 0, 108, 0, 8, 8, 0, 0, 80, 1, 1, 5, 0, 80, 1, 1, 9, 0, 77, 77, 0, 0, 977, 0, 0, 102,
  
        0, 0, 108, 0, 0, 0, 103, 0, 0, 107, 0, 0, 1, 5, 5, 66, 0, 80, 53, 0, 0, 0, 80, 54, 0, 0, // 12
  
        0, 59, 56, 0, 0, 0, 58, 57, 0, 0, 0, 70, 58, 0, 0, 30, 0, 0, 0, 70, 59, 0, 0, 0, 77, 0,
  
        0, 0, 0, 0, 0, 300, 0, 0, 100, 301, 0, 0, 0, 56, 60, 61, 0, 57, 62, 61, 0, 53, 0, 0, 0, 0,
   
        0, 80, 82, 82, 0, 80, 81, 81, 0, 77, 0, 301, 0, 0, 100, 300, 0, 0, 0, 57, 62, 61, 0, 56, 60, 61, // 15
   
        0, 80, 82, 80, 81, 0, 80, 54, 0, 0, 0, 0, 0, 1, 1, 5, 0, 77, 0, 77, 0, 1, 1, 9, 0, 77,
   
        0, 77, 0, 56, 0, 57, 0, 0, 72, 77, 0, 73, 77, 0, 80, 80, 0, 0, 0, 0, 1, 1, 5, 5, 0, 70,
    
        0, 1, 1, 5, 9, 0, 77, 77, 0, 61, 61, 80, 0, 0, 0, 0, 0, 0, 0, 80, 0, 2, 2, 0, 8, 8, // 18
   
        0, 56, 0, 0, 70, 57, 0, 0, 70, 0, 60, 60, 61, 62, 62, 77, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    
        0, 0, 0, 0, 0, 0, 0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Medium", score_database2, "Medium");
    }
}
