using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score441 : MonoBehaviour
{
    // 120, 14, 121 (Ribbon)    101, 54, 102 (Cross)    105, 33, 106 (M)    103, 53, 104 (Circle)     303, 304 (RearGuard)
    // 41, 42 (HourStand)    201 (RushingStar)    18, 20, 19 (FixedHeartPack)    76 (MultipleHitStar)    94 (FixedAirAttack)
    // 32 (EnemyStopper)    80 (BoomStopStar)    48 (4-Key-Tap)
    // 95 (FixedAirAttack_v2b)

    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 0, 1, 1, 0, 5, 0, 0, 2, 0, 0, 2, 0, 0, 2, 0, 0, 1, 1, 0, 9, 0, 0, 8, 0, 0,

        8, 0, 0, 8, 0, 0, 76, 0, 0, 1, 5, 0, 1, 9, 0, 103, 0, 104, 0, 20, 0, 1, 1, 0, 5, 0,

        1, 1, 0, 9, 0, 76, 0, 20, 0, 76, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 8, 0, // 3
    
        0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 8, 0, 0, 0, 1, 1, 0, 5, 5, 0, 41, 0, 42, 0, 20,

        0, 20, 0, 1, 5, 0, 1, 9, 0, 80, 0, 0, 0, 76, 0, 0, 1, 1, 0, 5, 0, 20, 0, 20, 0, 1,

        1, 0, 5, 9, 0, 0, 303, 0, 304, 0, 20, 5, 0, 1, 1, 0, 4, 0, 1, 1, 0, 6, 0, 101, 0, 20, // 6
   
        102, 0, 20, 0, 5, 0, 1, 9, 0, 0, 101, 0, 14, 0, 102, 0, 14, 0, 101, 0, 0, 0, 0, 0, 53, 0,

        103, 0, 104, 0, 94, 0, 48, 0, 48, 0, 48, 0, 48, 0, 54, 0, 101, 0, 102, 0, 94, 0, 48, 0, 48, 0,

        48, 0, 20, 20, 0, 0, 7, 0, 0, 1, 1, 0, 101, 0, 1, 1, 0, 102, 20, 0, 2, 0, 2, 0, 5, 0, // 9
  
        8, 8, 0, 9, 0, 0, 0, 32, 32, 0, 80, 0, 48, 0, 48, 0, 48, 0, 48, 20, 0, 1, 5, 0, 9, 0,

        120, 14, 0, 121, 14, 0, 20, 20, 0, 48, 48, 0, 7, 0, 0, 0, 0, 0, 1, 1, 0, 76, 76, 0, 48, 0,

        41, 0, 42, 0, 101, 0, 102, 0, 120, 0, 121, 0, 76, 20, 0, 32, 0, 80, 0, 0, 0, 0, 0, 0, 0, 0, // 12
   
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 76, 0, 201, 0,

        1, 0, 76, 0, 201, 0, 1, 1, 0, 48, 0, 9, 0, 32, 0, 80, 0, 9, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 15
  
        0, 0, 4, 0, 0, 0, 0, 0, 0, 48, 0, 48, 0, 0, 1, 1, 0, 5, 0, 33, 0, 0, 76, 0, 76, 20,

        0, 0, 7, 0, 0, 1, 5, 0, 48, 0, 94, 0, 1, 1, 0, 76, 0, 76, 0, 48, 0, 48, 0, 0, 1, 5,

        0, 1, 9, 0, 303, 0, 304, 0, 120, 121, 0, 94, 0, 0, 7, 0, 0, 1, 1, 0, 5, 0, 2, 2, 0, 4, // 18
   
        0, 5, 0, 41, 0, 42, 0, 0, 48, 0, 48, 0, 7, 0, 0, 1, 1, 0, 5, 5, 0, 32, 32, 0, 80, 80,

        0, 76, 0, 41, 0, 42, 0, 103, 0, 104, 0, 120, 0, 121, 0, 32, 0, 1, 0, 5, 0, 32, 0, 1, 0, 9,

        0, 48, 48, 0, 105, 0, 33, 0, 106, 0, 33, 0, 105, 0, 33, 0, 106, 0, 5, 5, 0, 48, 0, 48, 0, 0, // 21
  
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,

        1, 0, 5, 0, 1, 1, 0, 76, 0, 76, 0, 1, 1, 0, 201, 0, 201, 0, 1, 1, 0, 48, 48, 0, 48, 48,

        0, 1, 5, 0, 32, 80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 24
   
        0, 0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 0, 1, 1, 5, 5, 0, 0, 3, 0, 0, 17, 0, 0, 3, 0, 0, 1, 5, 1, 5, 0, 0, 3, 0, 0,
   
        17, 0, 0, 3, 0, 0, 201, 0, 0, 1, 5, 1, 9, 0, 53, 0, 1, 4, 1, 6, 0, 103, 53, 104, 0, 121,
    
        14, 120, 0, 9, 0, 41, 0, 42, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 5, 0, // 3
    
        0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 9, 0, 0, 0, 105, 20, 0, 1, 1, 4, 0, 106, 20, 0, 1,
   
        1, 6, 0, 33, 20, 0, 1, 1, 5, 9, 0, 0, 0, 201, 0, 0, 1, 1, 5, 5, 0, 303, 0, 19, 1, 1,
   
        5, 0, 9, 0, 93, 0, 304, 0, 18, 1, 1, 5, 0, 1, 4, 1, 6, 0, 9, 0, 3, 0, 53, 20, 1, 5, // 6
   
        0, 53, 20, 1, 9, 0, 5, 5, 0, 0, 103, 53, 104, 102, 54, 101, 121, 14, 120, 0, 0, 0, 0, 0, 1, 1,
   
        5, 4, 0, 41, 94, 0, 42, 91, 0, 41, 94, 0, 76, 0, 1, 1, 5, 6, 0, 303, 94, 0, 304, 94, 0, 303,
   
        94, 0, 304, 94, 0, 0, 3, 0, 0, 1, 1, 5, 0, 102, 0, 101, 0, 102, 20, 0, 93, 0, 93, 0, 41, 0, // 9
  
        0, 42, 0, 20, 0, 0, 0, 1, 4, 1, 9, 0, 41, 18, 94, 42, 19, 94, 0, 76, 0, 1, 2, 1, 5, 0,
   
        105, 20, 0, 106, 94, 0, 303, 20, 0, 304, 94, 0, 3, 0, 0, 0, 0, 0, 1, 1, 5, 9, 0, 76, 76, 0,
   
        14, 120, 54, 101, 53, 104, 41, 0, 42, 0, 104, 53, 103, 20, 0, 5, 5, 9, 0, 0, 32, 0, 0, 80, 0, 0, // 12
   
        80, 0, 93, 0, 32, 32, 0, 80, 0, 80, 0, 0, 32, 32, 80, 80, 9, 0, 0, 1, 1, 76, 0, 201, 0, 1,
   
        1, 76, 76, 0, 201, 0, 32, 80, 0, 1, 1, 9, 0, 32, 32, 80, 0, 9, 0, 0, 0, 0, 0, 0, 2, 0,
   
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 15
  
        0, 0, 4, 0, 0, 0, 0, 0, 0, 1, 1, 5, 5, 0, 32, 80, 0, 120, 14, 121, 0, 101, 54, 102, 0, 94,
  
        0, 0, 3, 0, 0, 1, 48, 1, 48, 0, 41, 94, 0, 42, 19, 94, 0, 76, 0, 6, 48, 6, 48, 0, 303, 94,
   
        0, 304, 94, 0, 303, 18, 94, 304, 19, 94, 0, 80, 0, 0, 3, 0, 0, 1, 32, 1, 80, 0, 41, 18, 20, 42, // 18
   
        19, 20, 0, 80, 32, 80, 0, 6, 32, 1, 80, 0, 3, 0, 0, 41, 19, 0, 42, 18, 0, 303, 19, 0, 304, 18,
   
        0, 76, 0, 1, 1, 5, 9, 0, 103, 54, 104, 0, 101, 54, 102, 0, 32, 32, 80, 0, 93, 0, 76, 0, 76, 0,
   
        201, 0, 103, 53, 104, 102, 54, 101, 120, 14, 121, 0, 41, 18, 20, 42, 19, 20, 0, 20, 0, 5, 5, 9, 0, 0, // 21
  
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
  
        1, 76, 0, 76, 0, 76, 0, 80, 76, 80, 201, 0, 1, 5, 76, 0, 76, 0, 76, 0, 80, 76, 80, 76, 0, 201,
  
        0, 201, 0, 32, 32, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 24
   
        0, 0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_database3 =
    {
        0,

        0, 0, 120, 14, 121, 14, 120, 14, 121, 20, 14, 120, 18, 14, 121, 19, 14, 120, 14, 121, 0, 0, 1, 1, 5, 0,

        32, 32, 80, 0, 0, 48, 48, 0, 0, 76, 9, 76, 9, 0, 201, 0, 201, 0, 76, 9, 76, 9, 0, 101, 54, 102,

        18, 54, 101, 19, 54, 102, 0, 0, 1, 9, 0, 2, 2, 2, 2, 8, 8, 8, 8, 0, 93, 93, 93, 93, 0, 2, // 3
    
        2, 2, 2, 8, 8, 8, 8, 0, 93, 93, 93, 93, 0, 1, 1, 5, 0, 20, 0, 1, 1, 9, 0, 20, 0, 1,

        32, 5, 0, 32, 1, 9, 0, 48, 48, 48, 0, 0, 76, 76, 0, 0, 1, 6, 1, 9, 0, 201, 0, 32, 5, 32,

        9, 0, 76, 0, 3, 0, 103, 0, 104, 0, 103, 0, 0, 48, 1, 1, 0, 32, 80, 0, 7, 0, 303, 0, 0, 76, // 6
   
        76, 0, 3, 3, 304, 0, 0, 201, 201, 0, 3, 3, 0, 1, 5, 1, 9, 48, 48, 0, 0, 3, 0, 3, 0, 1,

        1, 5, 0, 101, 53, 102, 121, 14, 120, 101, 54, 102, 18, 0, 48, 0, 41, 0, 18, 42, 0, 19, 0, 32, 32, 80,

        80, 0, 76, 0, 76, 0, 3, 0, 0, 303, 0, 304, 0, 303, 0, 304, 0, 20, 20, 20, 20, 0, 18, 19, 18, 19, // 9
  
        0, 41, 0, 42, 0, 0, 0, 32, 5, 32, 9, 0, 201, 0, 201, 0, 80, 0, 48, 48, 0, 4, 2, 4, 8, 0,

        120, 14, 121, 20, 101, 54, 102, 20, 0, 1, 1, 5, 9, 0, 3, 93, 3, 0, 32, 1, 80, 9, 0, 76, 0, 76,

        0, 201, 201, 0, 76, 76, 76, 0, 201, 201, 201, 0, 76, 76, 76, 0, 48, 48, 0, 0, 3, 0, 0, 7, 0, 0, // 12
   
        3, 0, 7, 0, 93, 93, 0, 5, 0, 9, 0, 0, 1, 32, 5, 80, 94, 0, 0, 76, 0, 76, 0, 120, 14, 121,

        54, 54, 101, 102, 53, 53, 104, 103, 120, 14, 121, 14, 120, 14, 121, 14, 0, 3, 0, 0, 0, 0, 0, 2, 0, 2,

        0, 2, 0, 2, 0, 8, 0, 8, 0, 8, 0, 8, 0, 93, 0, 93, 0, 93, 0, 93, 0, 2, 0, 8, 0, 2, // 15
  
        0, 76, 94, 0, 0, 0, 0, 0, 0, 1, 1, 1, 5, 0, 76, 0, 76, 0, 54, 121, 120, 54, 120, 121, 20, 20,

        20, 20, 94, 0, 0, 48, 48, 32, 80, 0, 41, 0, 42, 0, 41, 94, 0, 42, 94, 0, 121, 120, 54, 0, 20, 94,

        20, 94, 20, 0, 93, 2, 93, 2, 0, 8, 8, 9, 0, 0, 7, 0, 0, 48, 32, 48, 80, 0, 41, 0, 42, 0, // 18
   
        41, 94, 0, 42, 94, 0, 121, 54, 120, 53, 121, 0, 94, 0, 0, 76, 76, 0, 201, 201, 0, 48, 48, 0, 76, 9,

        76, 9, 0, 32, 32, 80, 80, 0, 103, 104, 54, 0, 120, 121, 14, 0, 54, 54, 103, 103, 104, 104, 0, 76, 53, 0,

        0, 76, 54, 0, 0, 76, 201, 76, 0, 105, 106, 33, 106, 105, 33, 0, 20, 20, 20, 20, 0, 80, 80, 48, 0, 0, // 21
  
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,

        1, 5, 0, 1, 1, 9, 0, 76, 76, 0, 201, 0, 1, 32, 5, 0, 1, 32, 9, 0, 76, 76, 0, 201, 201, 0,

        1, 48, 0, 32, 80, 48, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 24
   
        0, 0, 0, 0, 0, 0, 0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Medium", score_database2, "Medium", score_database3, "Medium");
    }
}