using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score492 : MonoBehaviour
{
    //    52 [121, 123, 122] Circle2      56, 57 (ScratchMark)      303, 304 (RearGuard)
    //    18, 20, 19 (FixedHeartPack)     80 [110, 111] BoomStopStar     76 (MultipleHiStar)
    //    41, 42 (QuickStarBound)      91, 94, 92 (FixedHeartPack)       114, 115 (NoJumpAvailable)
    //    116 (WideItem3)

    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 0, 1, 0, 5, 0, 1, 0, 5, 0, 2, 2, 2, 0, 8, 8, 8, 0, 1, 0, 9, 0, 1, 0, 9, 0,

        8, 2, 8, 0, 2, 8, 2, 0, 56, 0, 57, 0, 56, 0, 57, 0, 94, 0, 0, 0, 0, 1, 0, 4, 0, 1,

        0, 66, 0, 2, 2, 0, 8, 8, 0, 4, 2, 2, 0, 4, 8, 8, 0, 123, 0, 20, 0, 20, 0, 80, 0, 0, // 3
 
        2, 2, 0, 4, 0, 8, 8, 0, 4, 0, 1, 0, 5, 80, 0, 0, 2, 2, 0, 5, 0, 8, 8, 0, 5, 0,

        9, 0, 1, 4, 0, 1, 66, 0, 2, 0, 8, 0, 2, 0, 0, 1, 1, 5, 0, 93, 0, 1, 1, 9, 0, 977,

        80, 0, 0, 0, 13, 0, 2, 8, 0, 5, 0, 5, 0, 13, 0, 8, 2, 0, 5, 0, 9, 0, 123, 0, 20, 20, // 6
  
        0, 0, 93, 0, 2, 0, 93, 0, 0, 1, 5, 0, 1, 9, 0, 2, 0, 2, 0, 2, 0, 8, 0, 8, 0, 8,

        0, 1, 1, 5, 0, 2, 8, 2, 0, 80, 20, 0, 0, 2, 2, 0, 8, 0, 8, 0, 2, 2, 0, 8, 0, 8,

        0, 1, 5, 0, 1, 9, 0, 5, 5, 4, 0, 0, 13, 0, 0, 1, 0, 5, 0, 1, 0, 9, 0, 80, 0, 110, // 9
  
        0, 111, 0, 80, 0, 5, 5, 0, 2, 2, 2, 0, 8, 8, 8, 0, 1, 5, 0, 1, 1, 66, 0, 0, 13, 0,

        0, 13, 0, 0, 2, 2, 4, 0, 8, 8, 4, 0, 5, 0, 5, 0, 56, 56, 0, 57, 57, 0, 20, 0, 20, 0,

        110, 80, 0, 111, 80, 0, 116, 0, 0, 0, 0, 114, 0, 1, 1, 5, 0, 114, 114, 0, 80, 115, 0, 0, 0, 1, // 12
  
        1, 5, 0, 1, 1, 9, 0, 0, 93, 0, 93, 0, 0, 76, 0, 18, 76, 0, 19, 0, 76, 0, 20, 0, 56, 18,

        0, 57, 19, 0, 20, 20, 94, 0, 114, 0, 115, 0, 1, 80, 0, 0, 0, 114, 0, 1, 9, 1, 0, 114, 80, 0,

        8, 8, 4, 0, 0, 114, 114, 0, 115, 115, 0, 80, 80, 0, 1, 5, 0, 80, 0, 0, 0, 2, 2, 5, 0, 13, // 15 
  
        0, 8, 8, 5, 0, 13, 0, 13, 0, 115, 0, 2, 8, 0, 5, 0, 114, 0, 115, 0, 1, 5, 0, 1, 9, 0,

        13, 0, 0, 13, 0, 114, 115, 114, 0, 20, 20, 0, 94, 0, 0, 0, 1, 1, 0, 5, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 // 18
    };

    private int[] score_database2 =
    {
        0,

        0, 0, 303, 0, 0, 80, 111, 80, 0, 20, 20, 0, 122, 0, 19, 0, 19, 0, 304, 0, 0, 80, 110, 80, 0, 20,
  
        20, 0, 121, 0, 18, 0, 18, 0, 56, 0, 20, 57, 0, 20, 0, 76, 80, 0, 0, 0, 0, 1, 1, 5, 0, 1,
 
        1, 99, 0, 80, 110, 80, 111, 0, 0, 5, 6, 5, 0, 5, 99, 5, 0, 3, 116, 0, 80, 111, 80, 110, 0, 0, // 3
 
        1, 5, 5, 0, 20, 0, 1, 5, 5, 0, 20, 0, 111, 80, 0, 0, 2, 4, 2, 5, 0, 8, 4, 8, 5, 0,
  
        20, 0, 2, 2, 0, 8, 8, 0, 93, 0, 93, 0, 93, 0, 0, 1, 4, 5, 0, 20, 0, 1, 99, 5, 0, 110,
  
        80, 0, 0, 0, 1, 2, 93, 8, 0, 5, 0, 5, 0, 1, 2, 93, 8, 0, 99, 0, 5, 0, 123, 0, 0, 94, // 6
  
        0, 0, 3, 0, 116, 0, 3, 0, 0, 1, 1, 5, 5, 0, 114, 0, 114, 0, 0, 115, 0, 115, 0, 0, 114, 0,
  
        114, 0, 110, 80, 0, 1, 1, 99, 5, 0, 20, 0, 0, 114, 0, 114, 0, 0, 115, 0, 115, 0, 0, 114, 0, 114,
  
        0, 0, 115, 0, 115, 0, 1, 5, 1, 99, 0, 93, 0, 0, 93, 0, 0, 111, 80, 0, 0, 114, 0, 115, 0, 114, // 9
  
        0, 115, 0, 1, 5, 5, 0, 0, 115, 0, 114, 0, 115, 0, 114, 0, 115, 0, 1, 1, 5, 99, 0, 0, 93, 0,
  
        0, 93, 0, 0, 1, 1, 5, 0, 1, 1, 99, 0, 1, 1, 5, 0, 56, 0, 57, 0, 94, 0, 80, 110, 80, 111,
  
        80, 94, 0, 123, 0, 0, 20, 0, 0, 0, 0, 56, 0, 7, 80, 57, 0, 7, 80, 0, 114, 115, 0, 0, 0, 57, // 12
  
        0, 7, 80, 56, 0, 7, 80, 0, 114, 115, 114, 0, 0, 41, 0, 0, 76, 0, 42, 0, 0, 76, 0, 115, 56, 18,
  
        18, 0, 115, 57, 19, 19, 94, 0, 114, 80, 114, 80, 115, 80, 0, 0, 0, 56, 18, 20, 80, 57, 19, 20, 80, 0,
  
        5, 6, 5, 0, 0, 57, 19, 94, 80, 56, 18, 94, 80, 0, 5, 99, 5, 80, 0, 0, 0, 1, 4, 5, 0, 93, // 15 
  
        0, 1, 6, 5, 0, 93, 0, 80, 114, 80, 0, 1, 4, 5, 5, 0, 93, 0, 93, 0, 1, 6, 5, 5, 0, 93,
  
        0, 93, 0, 93, 0, 80, 115, 80, 0, 5, 4, 5, 99, 0, 0, 0, 20, 8, 8, 94, 0, 0, 0, 0, 0, 0,
   
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 // 18
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Medium", score_database2, "Medium");
    }
}