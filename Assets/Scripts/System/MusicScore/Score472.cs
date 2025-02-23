using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score472 : MonoBehaviour
{
    //   56, 57 (ScratchMark2)     18, 20, 19 (FixedHeartPack)     91, 94, 92 (FixedAirAttack)
    //   22, 23 (BoldArrow)     50, 51 (WideTapItemDash)    81, 82 (CombinePickStar)
    //   [122, 123] [124, 125] (BoldArrow_Modded)    80 (BoomStopStar)

    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 1, 5, 0, 9, 1, 0, 5, 9, 0, 1, 5, 0, 0, 0, 82, 80, 0, 0,

        0, 0, 4, 5, 0, 9, 1, 0, 5, 4, 0, 1, 9, 0, 0, 0, 81, 80, 0, 0, 1, 1, 0, 5, 5, 0,

        1, 1, 0, 5, 6, 0, 0, 22, 0, 22, 0, 0, 23, 0, 23, 0, 0, 81, 82, 0, 80, 0, 0, 1, 1, 0, // 3
   
        5, 0, 1, 1, 0, 9, 0, 2, 0, 2, 0, 8, 0, 8, 0, 20, 0, 94, 0, 0, 0, 0, 1, 0, 5, 5,

        0, 1, 0, 5, 9, 0, 0, 3, 0, 0, 1, 2, 0, 1, 8, 0, 0, 80, 0, 0, 22, 122, 0, 23, 124, 0,

        20, 0, 0, 0, 0, 1, 5, 0, 2, 4, 0, 1, 9, 0, 8, 4, 0, 56, 0, 57, 0, 56, 0, 57, 0, 0, // 6
  
        93, 0, 0, 1, 5, 0, 2, 2, 0, 8, 8, 0, 1, 9, 0, 0, 20, 0, 18, 0, 20, 0, 19, 0, 80, 0,

        0, 1, 1, 0, 5, 1, 0, 9, 1, 0, 5, 1, 0, 4, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 93, 0, 8, 93, 0, 20, 20, // 9
  
        0, 1, 1, 0, 5, 0, 0, 1, 93, 0, 1, 93, 0, 20, 20, 0, 1, 1, 0, 9, 0, 0, 2, 93, 0, 1,

        93, 0, 1, 5, 0, 9, 1, 0, 5, 4, 0, 6, 0, 0, 2, 2, 0, 8, 8, 0, 5, 0, 0, 9, 0, 0,

        0, 3, 0, 0, 22, 0, 123, 0, 0, 23, 0, 125, 0, 0, 22, 0, 23, 0, 0, 5, 9, 0, 80, 80, 0, 0, // 12
   
        0, 0, 0, 0, 2, 4, 0, 5, 0, 22, 0, 123, 0, 18, 0, 23, 0, 125, 0, 19, 0, 0, 80, 0, 0, 0,

        0, 0, 0, 0, 20, 0, 20, 0, 0, 20, 0, 20, 0, 0, 18, 94, 0, 19, 0, 0, 80, 0, 0, 18, 20, 0,

        19, 20, 0, 0, 56, 0, 57, 0, 0, 0, 3, 0, 0, 0, 8, 4, 0, 5, 0, 22, 0, 123, 0, 91, 0, 23, // 15
   
        0, 125, 0, 92, 0, 0, 0, 80, 0, 80, 0, 0, 1, 0, 5, 0, 1, 0, 9, 0, 2, 0, 8, 0, 2, 0,

        1, 0, 5, 0, 1, 0, 9, 0, 2, 0, 93, 0, 93, 0, 93, 0, 5, 0, 1, 1, 0, 9, 0, 0, 56, 18,

        0, 57, 19, 0, 20, 20, 0, 56, 18, 0, 57, 19, 0, 20, 0, 20, 0, 1, 5, 0, 9, 0, 0, 1, 5, 0, // 18
   
        80, 0, 0, 93, 0, 2, 0, 93, 0, 8, 0, 1, 0, 5, 0, 0, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 56, 18, 20, 0, 57, 19, 20, 0, 124, 20, 122, 0, 0, 0, 82, 51, 0, 0,
   
        0, 0, 56, 18, 94, 0, 57, 19, 94, 0, 122, 94, 124, 0, 0, 0, 81, 50, 0, 0, 1, 1, 5, 0, 4, 0,
   
        1, 5, 6, 0, 93, 0, 4, 0, 122, 123, 20, 124, 125, 0, 0, 82, 50, 0, 81, 51, 0, 0, 0, 1, 2, 5, // 3
   
        0, 1, 8, 5, 0, 13, 0, 1, 1, 5, 0, 1, 1, 9, 0, 93, 0, 80, 0, 0, 0, 0, 1, 1, 5, 5,
  
        0, 13, 0, 1, 1, 5, 9, 0, 13, 0, 2, 4, 8, 4, 5, 0, 0, 80, 0, 0, 122, 123, 94, 124, 125, 0,
   
        20, 0, 0, 0, 0, 1, 1, 5, 0, 4, 0, 1, 1, 9, 0, 0, 56, 18, 20, 0, 57, 19, 20, 0, 80, 0, // 6
  
        80, 0, 0, 80, 0, 1, 1, 5, 0, 4, 0, 1, 1, 9, 0, 0, 56, 20, 18, 0, 57, 20, 19, 0, 80, 0,
   
        0, 80, 0, 1, 2, 4, 0, 5, 0, 1, 8, 4, 0, 5, 9, 0, 0, 0, 13, 0, 0, 0, 77, 0, 0, 0,
   
        13, 0, 0, 0, 77, 0, 0, 0, 13, 0, 0, 0, 77, 0, 0, 0, 0, 0, 82, 51, 0, 0, 82, 51, 0, 20, // 9
  
        20, 0, 1, 1, 5, 0, 0, 81, 50, 0, 0, 81, 50, 0, 20, 20, 0, 1, 1, 9, 0, 0, 82, 51, 0, 81,
  
        50, 0, 1, 1, 5, 9, 0, 50, 0, 51, 0, 50, 0, 0, 2, 2, 93, 0, 8, 8, 93, 0, 0, 5, 0, 0,
  
        0, 4, 0, 0, 22, 20, 23, 0, 0, 122, 20, 124, 0, 0, 22, 94, 23, 0, 0, 1, 5, 9, 0, 80, 0, 0, // 12
   
        0, 0, 0, 0, 1, 1, 5, 5, 0, 22, 123, 0, 23, 125, 0, 20, 20, 18, 20, 20, 19, 0, 80, 0, 0, 0,
   
        0, 0, 0, 0, 82, 82, 51, 0, 0, 81, 81, 50, 0, 0, 1, 5, 0, 9, 0, 0, 13, 0, 0, 82, 82, 80,
  
        81, 81, 0, 0, 1, 5, 9, 0, 0, 13, 0, 77, 0, 0, 1, 1, 5, 6, 0, 23, 125, 0, 22, 123, 0, 20, // 15
   
        20, 91, 20, 20, 92, 0, 0, 80, 0, 80, 0, 0, 50, 0, 81, 0, 51, 0, 82, 0, 50, 0, 20, 0, 20, 0,
   
        51, 0, 82, 0, 50, 0, 81, 0, 51, 0, 20, 0, 20, 0, 2, 8, 93, 0, 1, 1, 5, 9, 0, 0, 56, 57,
   
        0, 56, 57, 0, 94, 94, 0, 56, 57, 0, 56, 57, 0, 20, 20, 20, 0, 22, 20, 94, 23, 0, 0, 22, 94, 20, // 18
   
        23, 0, 0, 1, 5, 9, 0, 2, 2, 93, 0, 8, 8, 93, 0, 0, 80, 0, 0, 0, 0, 0, 0, 0, 0, 0,
   
        0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Medium", score_database2, "Medium");
    }
}
