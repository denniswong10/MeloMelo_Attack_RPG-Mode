using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score452 : MonoBehaviour
{
    //   71 (MultipleHitStar)    76 (MultipleHitStar_Random)   105, 33, 106 (M)    15 (RisingHeart)    11 (RisingHeart)
    //   18, 20, 19 (FixedHeartPack)    72, 73 (QuickPickUpItem)    51, 52 (HalfArrow)    91, 92 (FixedAttack)
    //   87, 88 (QuickJumpAndAttack)    89 (MultipleQuickTap)    101, 14, 102 (Ribbon)
    //   77, 78 (MultipleHitStar_Left/Right Pick)

    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 1, 5, 0, 1, 9, 0, 14, 0, 33, 0, 14, 0, 5, 0, 71, 18, 0, 71, 19, 0, 5, 9, 0, 0, 0,

        93, 0, 0, 2, 8, 0, 71, 0, 5, 0, 1, 5, 0, 1, 9, 0, 0, 93, 0, 0, 93, 0, 0, 1, 0, 9,

        0, 0, 0, 93, 0, 0, 0, 11, 8, 0, 15, 8, 0, 71, 0, 1, 0, 5, 5, 0, 9, 0, 1, 1, 0, 5, // 3
   
        0, 76, 20, 0, 14, 0, 11, 0, 8, 0, 33, 0, 11, 0, 8, 0, 101, 14, 0, 71, 0, 9, 5, 0, 33, 0,

        0, 0, 1, 1, 0, 15, 0, 15, 0, 20, 0, 0, 0, 0, 0, 0, 8, 0, 0, 0, 0, 0, 0, 2, 4, 0,

        0, 8, 4, 0, 1, 1, 0, 5, 0, 71, 0, 0, 0, 1, 0, 2, 0, 8, 0, 5, 0, 9, 0, 11, 15, 0, // 6
   
        1, 1, 0, 5, 76, 0, 89, 0, 1, 0, 4, 0, 5, 5, 0, 9, 0, 105, 0, 33, 0, 106, 0, 0, 4, 2,

        0, 5, 5, 0, 4, 8, 0, 11, 8, 0, 15, 8, 0, 71, 0, 71, 0, 1, 1, 0, 9, 0, 0, 0, 0, 0,

        0, 1, 5, 0, 9, 0, 76, 0, 0, 2, 8, 0, 11, 2, 0, 11, 8, 0, 15, 0, 33, 0, 5, 0, 5, 0, // 9
   
        11, 8, 0, 5, 0, 9, 0, 0, 3, 3, 0, 0, 1, 0, 0, 2, 0, 0, 8, 0, 8, 0, 0, 5, 0, 0,

        1, 0, 1, 0, 0, 4, 0, 0, 2, 4, 0, 2, 8, 0, 0, 0, 101, 0, 101, 0, 14, 0, 14, 0, 102, 0,

        102, 18, 0, 0, 0, 51, 0, 18, 0, 18, 0, 52, 0, 19, 0, 19, 0, 5, 0, 9, 0, 102, 0, 102, 0, 14, // 12
   
        0, 14, 0, 101, 0, 101, 91, 0, 0, 0, 52, 0, 19, 0, 19, 0, 51, 0, 18, 0, 18, 0, 5, 0, 9, 0,

        0, 105, 0, 105, 0, 20, 0, 106, 0, 106, 0, 18, 19, 0, 20, 0, 15, 0, 11, 2, 0, 11, 8, 0, 0, 3,

        3, 0, 0, 1, 5, 0, 93, 0, 1, 9, 0, 93, 0, 1, 5, 0, 1, 9, 0, 18, 20, 0, 19, 20, 0, 71, // 15
   
        92, 0, 76, 0, 76, 0, 5, 0, 9, 0, 0, 0, 0, 0, 0, 0, 93, 0, 93, 0, 2, 0, 0, 93, 0, 93,

        0, 8, 0, 0, 1, 1, 0, 20, 0, 1, 1, 0, 15, 0, 71, 91, 0, 71, 92, 0, 5, 9, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 77, 20, 0, 78, 20, 0, 105, 0, 0, 106, 0, 0, 9, 0, 77, 20, 18, 78, 20, 19, 0, 5, 0, 0, 0,
  
        93, 0, 0, 11, 8, 15, 0, 71, 20, 0, 1, 1, 5, 5, 9, 0, 0, 2, 0, 0, 8, 0, 0, 5, 5, 9,
   
        0, 0, 0, 93, 0, 0, 0, 87, 0, 71, 0, 88, 0, 71, 0, 1, 2, 2, 5, 0, 76, 0, 1, 8, 8, 5, // 3
   
        76, 0, 9, 0, 105, 0, 11, 8, 8, 0, 106, 0, 11, 8, 8, 0, 89, 20, 89, 20, 0, 51, 18, 0, 20, 52,
   
        19, 0, 20, 0, 33, 72, 20, 73, 0, 91, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 71, 15, 0,
   
        71, 15, 0, 89, 0, 1, 9, 5, 0, 89, 0, 0, 0, 1, 1, 76, 0, 89, 0, 11, 8, 8, 11, 8, 8, 0, // 6
   
        1, 1, 5, 0, 76, 0, 89, 0, 20, 1, 15, 0, 18, 20, 19, 91, 0, 105, 0, 106, 0, 33, 0, 0, 1, 1,
   
        5, 0, 89, 0, 11, 8, 5, 11, 8, 5, 0, 87, 0, 88, 0, 87, 0, 1, 5, 1, 9, 0, 0, 0, 0, 0,
   
        0, 1, 1, 5, 9, 0, 71, 0, 0, 93, 8, 11, 8, 15, 8, 11, 8, 5, 5, 0, 33, 0, 11, 8, 15, 8, // 9
   
        11, 8, 8, 5, 0, 9, 0, 0, 3, 93, 0, 0, 1, 0, 0, 93, 0, 0, 2, 0, 2, 0, 0, 93, 0, 0,
   
        1, 0, 1, 0, 0, 93, 0, 0, 1, 1, 5, 1, 1, 9, 0, 0, 101, 18, 14, 20, 102, 19, 14, 20, 101, 18,
   
        71, 92, 0, 0, 0, 73, 51, 0, 18, 18, 0, 72, 52, 0, 19, 19, 0, 1, 5, 9, 0, 102, 19, 14, 20, 101, // 12
   
        18, 14, 20, 102, 19, 71, 91, 0, 0, 0, 72, 52, 0, 19, 19, 0, 73, 51, 0, 18, 18, 0, 1, 5, 9, 0,
   
        0, 33, 18, 20, 18, 0, 33, 19, 20, 19, 0, 1, 11, 1, 15, 0, 93, 0, 1, 1, 11, 1, 15, 0, 0, 0,
    
        44, 0, 0, 1, 1, 5, 0, 93, 0, 1, 1, 9, 0, 93, 0, 72, 20, 19, 0, 73, 20, 18, 0, 18, 71, 19, // 15
   
        71, 0, 87, 0, 88, 0, 89, 0, 5, 0, 0, 0, 0, 0, 0, 0, 93, 2, 2, 0, 5, 0, 0, 93, 8, 8,
   
        0, 5, 0, 33, 0, 0, 20, 0, 0, 89, 0, 9, 5, 76, 0, 9, 5, 76, 0, 78, 0, 73, 0, 0, 0, 0,
    
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Medium", score_database2, "Medium");
    }
}
