using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score470 : MonoBehaviour
{
    //     14 (Ribbon)    34 (PonyDiamond)    81, 82 (CombinePickStar)     55, 56 (CurveClosedPattern)
    //     50 (Diamond)     36, 37 (Leaf Bud)     71, 72 (DoubleShootHeart)     74 (DiamondSurrondHeart)
    //     46 (6-Keys)    76 (MultipleHitStar)

    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 4, 0, 8, 8, 0, 4, 0, 5, 0, 0, 14, 0, 0, 50,

        0, 14, 0, 0, 50, 0, 14, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 36, 0, 0, 37,

        0, 0, 1, 1, 0, 5, 0, 5, 0, 0, 93, 0, 0, 55, 0, 0, 56, 0, 0, 1, 1, 0, 5, 9, 0, 0, // 3
 
        14, 0, 50, 0, 5, 5, 0, 14, 0, 50, 0, 5, 5, 0, 9, 0, 1, 0, 8, 0, 4, 0, 5, 9, 0, 0,

        14, 0, 0, 0, 0, 1, 1, 0, 5, 0, 0, 1, 1, 0, 9, 0, 0, 14, 0, 0, 46, 46, 0, 9, 0, 20,

        0, 20, 0, 0, 0, 0, 0, 0, 1, 5, 0, 2, 0, 1, 9, 0, 8, 0, 50, 0, 14, 0, 20, 0, 0, 46, // 6
  
        0, 46, 0, 1, 5, 0, 1, 9, 0, 2, 0, 8, 4, 0, 0, 0, 14, 0, 20, 20, 0, 46, 0, 1, 0, 5,

        0, 55, 0, 0, 37, 0, 0, 1, 4, 0, 1, 6, 0, 93, 0, 93, 0, 0, 14, 0, 0, 2, 4, 0, 8, 0,

        8, 0, 1, 5, 0, 1, 9, 0, 0, 0, 0, 74, 0, 50, 0, 74, 0, 0, 1, 0, 2, 4, 0, 8, 4, 0, // 9
 
        50, 0, 50, 0, 1, 0, 5, 0, 1, 0, 9, 0, 46, 46, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0,

        0, 0, 8, 8, 0, 46, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 1, 5, 0, 0, 9, 14, 0,

        0, 50, 20, 0, 0, 14, 20, 0, 0, 0, 1, 5, 0, 20, 20, 0, 1, 9, 0, 20, 20, 0, 34, 0, 0, 0, // 12
  
        0, 0, 94, 0, 0, 0, 0, 0, 0, 0, 81, 0, 46, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        82, 0, 46, 20, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 5, 0, 9, 0, 93, 0, 1, 1, 0, 5, 4, 0,

        50, 0, 8, 0, 8, 0, 14, 0, 20, 0, 20, 0, 0, 0, 0, 46, 20, 0, 46, 94, 0, 76, 76, 0, 0, 0, // 15
  
        71, 0, 72, 0, 0, 81, 46, 0, 82, 76, 0, 0, 0, 34, 0, 20, 0, 20, 0, 81, 0, 82, 0, 14, 0, 0,

        0, 93, 93, 0, 0, 1, 14, 0, 0, 74, 0, 8, 0, 5, 0, 0, 1, 14, 0, 0, 74, 0, 1, 0, 5, 0,

        0, 2, 4, 0, 5, 0, 0, 0, 34, 0, 20, 0, 94, 0, 55, 0, 56, 0, 0, 46, 0, 0, 0, 34, 0, 20, // 18
  
        0, 94, 0, 36, 0, 37, 0, 0, 46, 0, 0, 3, 0, 3, 0, 0, 1, 1, 0, 5, 0, 1, 1, 0, 9, 0,

        93, 2, 0, 93, 8, 0, 93, 0, 1, 0, 5, 0, 9, 0, 50, 0, 5, 0, 50, 0, 9, 76, 0, 14, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 4, 0, 8, 8, 0, 4, 0, 14, 0, 0, 0, 1, 5, 0, // 21
  
        1, 9, 0, 46, 0, 34, 0, 20, 20, 0, 94, 0, 81, 0, 81, 0, 82, 0, 82, 0, 1, 5, 0, 93, 0, 0,

        0, 14, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 4, 2, 0, 8, 8, 4, 8, 0, 14, 0, 0, 14, 0, 0, 2,

        50, 4, 0, 0, 8, 50, 4, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 34, 0, 0, 0,

        0, 0, 93, 93, 0, 1, 1, 5, 0, 0, 3, 0, 0, 34, 0, 0, 0, 0, 0, 93, 0, 1, 1, 9, 0, 0, // 3
 
        55, 0, 56, 0, 0, 36, 0, 37, 0, 0, 81, 0, 74, 0, 82, 0, 1, 2, 8, 0, 1, 5, 1, 9, 0, 0,

        14, 0, 0, 0, 0, 2, 4, 2, 5, 0, 0, 8, 4, 8, 5, 0, 0, 14, 0, 0, 1, 1, 5, 5, 0, 93,

        0, 4, 0, 0, 0, 0, 0, 0, 1, 1, 14, 0, 0, 1, 5, 4, 0, 1, 5, 9, 0, 1, 5, 0, 0, 71, // 6
  
        0, 72, 0, 2, 4, 2, 4, 2, 0, 1, 1, 5, 9, 0, 0, 0, 1, 5, 14, 0, 0, 46, 82, 46, 81, 0,

        34, 0, 0, 0, 0, 0, 93, 93, 0, 5, 4, 5, 6, 0, 0, 3, 0, 0, 14, 0, 0, 1, 5, 1, 9, 0,

        93, 0, 1, 1, 5, 0, 9, 0, 0, 0, 0, 55, 0, 56, 0, 0, 36, 0, 37, 0, 0, 74, 0, 1, 5, 1, // 9
 
        9, 0, 93, 0, 71, 0, 72, 0, 8, 4, 8, 4, 8, 5, 0, 0, 0, 2, 0, 2, 0, 4, 0, 0, 8, 0,

        8, 0, 4, 0, 0, 46, 0, 0, 2, 0, 4, 0, 0, 8, 0, 4, 0, 0, 0, 1, 14, 0, 0, 5, 14, 0,

        0, 9, 14, 0, 0, 5, 9, 0, 0, 0, 81, 46, 82, 0, 20, 0, 81, 46, 82, 0, 94, 0, 46, 46, 0, 0, // 12
  
        0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 1, 1, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        1, 1, 5, 6, 0, 0, 0, 0, 0, 0, 0, 76, 0, 0, 1, 1, 5, 0, 4, 0, 1, 5, 1, 0, 4, 0,

        50, 0, 1, 1, 5, 0, 50, 0, 50, 14, 0, 0, 0, 0, 0, 81, 46, 82, 46, 81, 46, 82, 74, 0, 0, 0, // 15
  
        34, 0, 20, 20, 20, 0, 46, 81, 81, 74, 0, 0, 0, 34, 0, 20, 20, 20, 0, 46, 82, 82, 74, 0, 14, 0,

        0, 0, 93, 0, 0, 1, 1, 46, 0, 71, 0, 81, 81, 81, 0, 0, 1, 5, 46, 0, 72, 0, 82, 82, 82, 0,

        0, 1, 5, 1, 9, 0, 0, 0, 34, 0, 20, 20, 94, 0, 46, 81, 46, 82, 0, 74, 0, 0, 0, 34, 0, 20, // 18
  
        20, 94, 0, 46, 82, 46, 81, 0, 74, 0, 0, 93, 93, 93, 0, 0, 1, 2, 4, 2, 0, 1, 8, 4, 8, 0,

        71, 0, 72, 0, 71, 0, 72, 0, 5, 5, 9, 0, 76, 0, 50, 0, 76, 0, 50, 0, 76, 76, 0, 14, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 2, 5, 0, 1, 8, 8, 5, 0, 14, 0, 0, 0, 1, 1, 5, // 21
  
        0, 1, 1, 9, 0, 34, 0, 71, 0, 72, 0, 0, 46, 81, 81, 0, 46, 82, 82, 0, 1, 1, 5, 9, 0, 0,

        0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0
    };

    private int[] score_database3 =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 4, 0, 1, 1, 5, 0, 1, 1, 9, 0, 0, 14, 0, 0, 101,
  
        0, 102, 0, 0, 81, 81, 46, 82, 82, 0, 0, 75, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 34, 55, 0, 56,
 
        0, 0, 46, 20, 20, 0, 5, 9, 0, 0, 3, 0, 0, 34, 56, 0, 55, 0, 0, 20, 20, 20, 0, 46, 0, 0, // 3
 
        1, 2, 4, 0, 1, 8, 4, 0, 55, 0, 20, 56, 0, 20, 46, 0, 1, 1, 5, 0, 1, 9, 5, 46, 0, 0,
 
        76, 0, 20, 20, 0, 76, 76, 0, 46, 0, 81, 0, 82, 0, 81, 0, 102, 0, 14, 0, 101, 0, 0, 36, 0, 37,
  
        0, 20, 0, 0, 0, 0, 0, 0, 101, 0, 0, 14, 0, 20, 102, 0, 0, 14, 94, 0, 36, 0, 37, 0, 55, 0, // 6
  
        56, 0, 0, 1, 5, 0, 1, 9, 0, 46, 81, 81, 46, 0, 0, 0, 1, 1, 5, 0, 93, 0, 1, 1, 9, 0,
 
        46, 20, 46, 94, 0, 0, 93, 75, 0, 102, 0, 0, 14, 0, 20, 101, 0, 0, 14, 94, 0, 37, 0, 36, 0, 56,
 
        0, 55, 0, 1, 5, 0, 1, 9, 0, 0, 0, 46, 82, 82, 46, 0, 34, 0, 20, 20, 0, 34, 55, 0, 56, 0, // 9
 
        20, 20, 94, 0, 36, 0, 37, 0, 0, 94, 20, 20, 82, 46, 0, 0, 0, 1, 0, 2, 0, 2, 0, 5, 5, 0,
  
        1, 0, 8, 0, 8, 0, 5, 5, 9, 0, 20, 0, 46, 46, 0, 94, 0, 0, 0, 101, 34, 0, 102, 0, 0, 101,
  
        0, 20, 94, 0, 0, 81, 46, 0, 0, 0, 36, 0, 20, 94, 37, 0, 94, 20, 55, 0, 56, 0, 0, 46, 0, 0, // 12
  
        3, 0, 14, 0, 0, 0, 20, 0, 0, 0, 94, 14, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
  
        94, 14, 0, 20, 94, 0, 0, 0, 0, 0, 0, 76, 76, 0, 1, 1, 5, 0, 0, 76, 76, 0, 1, 1, 9, 0,
  
        50, 0, 14, 0, 0, 50, 14, 0, 1, 5, 0, 1, 9, 0, 0, 93, 2, 93, 8, 0, 5, 5, 9, 0, 0, 0, // 15
  
        101, 0, 14, 0, 102, 0, 76, 20, 76, 94, 0, 0, 0, 102, 0, 14, 0, 101, 0, 76, 76, 94, 76, 76, 0, 0,
  
        0, 93, 93, 0, 0, 101, 0, 93, 14, 0, 0, 102, 93, 93, 14, 0, 0, 101, 0, 0, 82, 82, 46, 81, 81, 0,
  
        0, 20, 20, 20, 94, 0, 0, 0, 102, 0, 14, 3, 101, 0, 76, 20, 76, 94, 0, 3, 74, 0, 0, 101, 0, 14, // 18
  
        3, 102, 0, 76, 76, 94, 76, 76, 0, 0, 0, 93, 93, 0, 93, 93, 0, 1, 1, 5, 0, 1, 1, 9, 5, 0,
   
        2, 2, 93, 8, 8, 93, 2, 8, 5, 5, 9, 0, 74, 2, 2, 50, 8, 8, 74, 0, 101, 0, 14, 0, 102, 0,
   
        0, 0, 0, 0, 0, 0, 0, 0, 0, 93, 0, 5, 0, 0, 93, 93, 0, 5, 0, 14, 0, 0, 0, 93, 0, 5, // 21
  
        0, 93, 93, 0, 5, 0, 0, 93, 2, 93, 8, 0, 5, 5, 9, 0, 0, 101, 0, 14, 94, 102, 0, 14, 0, 3,
 
        7, 7, 75, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
  
        0, 0, 0, 0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1); // 3
        ScoreController controller = new ScoreController(difficulty, score_database, "Medium", score_database2, "Medium", score_database3, "Medium");
    }
}