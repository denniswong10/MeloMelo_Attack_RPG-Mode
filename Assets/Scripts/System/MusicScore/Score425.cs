using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score425 : MonoBehaviour
{ 
    // 35, 36 (SideOfRootSmash)    37, 38 (PathOfLight)    91, 94, 92 (FixedAirAttack)    95, 96, 97  (FixedAttack)    18, 20, 19 (FixedHeartPack)
    // 101, 102  14 (Ribbon)    30, 31 (Sweep)

    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 14, 0, 96, 0, 101, 0, 95, 0, 14, 0, 96, 0, 1, 0, 5, 0, 14, 0, 96, 0, 102, 0, 97, 0, 14,

        0, 96, 0, 1, 0, 5, 0, 30, 0, 0, 20, 31, 0, 0, 20, 30, 0, 0, 20, 31, 0, 0, 20, 30, 0, 0,

        20, 0, 1, 0, 5, 0, 1, 0, 9, 0, 1, 0, 4, 0, 35, 0, 0, 0, 35, 0, 0, 0, 35, 0, 0, 5, // 3 
   
        0, 9, 0, 36, 0, 0, 0, 36, 0, 0, 0, 36, 0, 0, 0, 5, 0, 9, 0, 38, 0, 0, 0, 0, 37, 0,

        0, 0, 0, 92, 0, 96, 0, 20, 20, 0, 1, 0, 5, 0, 1, 0, 9, 0, 1, 0, 2, 8, 0, 5, 0, 3,

        0, 1, 1, 0, 5, 0, 1, 1, 0, 9, 0, 20, 20, 0, 14, 0, 96, 0, 14, 0, 96, 0, 101, 0, 95, 0, // 6
   
        101, 0, 95, 0, 1, 1, 0, 5, 0, 1, 1, 0, 9, 5, 0, 2, 0, 2, 0, 5, 0, 9, 0, 8, 0, 8,

        0, 101, 0, 14, 0, 102, 0, 4, 2, 0, 4, 8, 0, 30, 0, 0, 31, 0, 0, 30, 0, 0, 31, 0, 0, 30,

        20, 0, 31, 20, 0, 30, 0, 0, 31, 0, 0, 30, 0, 0, 31, 0, 0, 30, 20, 0, 31, 20, 0, 101, 0, 0, // 9
   
        102, 0, 0, 101, 0, 0, 102, 0, 0, 101, 20, 0, 102, 20, 0, 101, 0, 0, 102, 0, 0, 101, 0, 0, 102, 0,

        0, 94, 0, 14, 0, 14, 0, 14, 0, 14, 0, 14, 0, 14, 0, 14, 0, 14, 0, 14, 0, 14, 0, 102, 0, 101,

        0, 14, 0, 14, 0, 14, 0, 14, 0, 14, 0, 14, 0, 14, 0, 14, 0, 14, 0, 14, 0, 101, 0, 102, 0, 14, // 12
  
        0, 14, 0, 14, 0, 14, 0, 14, 0, 20, 0, 20, 0, 101, 0, 101, 0, 101, 0, 14, 0, 14, 0, 14, 0, 102,

        0, 102, 0, 102, 0, 20, 0, 20, 0, 14, 0, 101, 0, 14, 0, 102, 0, 14, 0, 101, 0, 14, 0, 102, 0, 14,

        0, 20, 20, 0, 20, 0, 20, 20, 0, 5, 0, 101, 0, 102, 0, 92, 0, 1, 0, 5, 0, 35, 0, 0, 36, 0, // 15
  
        0, 1, 0, 5, 0, 1, 0, 9, 0, 93, 0, 0, 93, 0, 101, 0, 0, 102, 0, 0, 37, 0, 0, 0, 0, 36,

        0, 0, 0, 0, 92, 0, 96, 0, 0, 1, 1, 0, 5, 0, 1, 1, 0, 9, 0, 0, 20, 0, 8, 0, 20, 0,

        8, 8, 0, 5, 0, 9, 0, 93, 0, 93, 0, 35, 0, 0, 0, 36, 0, 0, 0, 35, 0, 0, 0, 5, 9, 0, // 18
   
        5, 0, 36, 0, 0, 0, 35, 0, 0, 0, 36, 0, 0, 0, 5, 9, 0, 5, 0, 38, 0, 0, 0, 0, 37, 0,

        0, 0, 0, 92, 0, 96, 0, 35, 0, 0, 36, 0, 0, 2, 2, 0, 8, 8, 0, 5, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 21
    
        0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 35, 0, 0, 0, 35, 0, 0, 0, 35, 0, 0, 0, 1, 1, 5, 0, 36, 0, 0, 0, 36, 0, 0, 0, 36,
   
        0, 0, 0, 1, 1, 5, 0, 37, 0, 0, 0, 0, 92, 38, 0, 0, 0, 0, 91, 95, 96, 35, 0, 0, 36, 0,
   
        0, 4, 1, 2, 0, 4, 1, 8, 0, 5, 1, 1, 4, 0, 35, 18, 0, 0, 35, 18, 0, 0, 35, 0, 0, 1, // 3 
   
        1, 9, 0, 36, 19, 0, 0, 36, 19, 0, 0, 36, 19, 0, 0, 1, 1, 9, 0, 38, 0, 20, 0, 0, 91, 37,
   
        0, 20, 0, 0, 92, 97, 96, 35, 0, 20, 36, 0, 20, 0, 4, 1, 2, 0, 4, 1, 8, 0, 9, 5, 0, 3,
   
        0, 1, 1, 5, 0, 20, 0, 1, 1, 5, 0, 20, 0, 101, 0, 18, 18, 14, 0, 20, 20, 0, 102, 0, 19, 19, // 6
   
        0, 1, 1, 5, 0, 1, 1, 9, 0, 3, 0, 1, 5, 9, 0, 93, 0, 93, 0, 1, 5, 9, 0, 93, 0, 93,
   
        0, 101, 0, 20, 102, 0, 20, 0, 1, 1, 5, 5, 0, 14, 0, 20, 14, 0, 20, 14, 0, 18, 14, 19, 0, 101,
  
        0, 20, 14, 20, 0, 102, 0, 0, 14, 0, 20, 14, 0, 20, 14, 0, 19, 14, 18, 0, 102, 0, 20, 14, 20, 0, // 9
   
        101, 0, 0, 14, 0, 20, 14, 0, 20, 14, 0, 20, 14, 0, 0, 1, 1, 5, 0, 1, 5, 1, 9, 5, 0, 101,
  
        0, 101, 0, 14, 0, 102, 0, 102, 0, 14, 0, 101, 0, 14, 0, 101, 0, 101, 0, 101, 0, 14, 0, 102, 0, 14,
  
        0, 102, 0, 102, 0, 102, 0, 14, 0, 101, 0, 14, 0, 102, 0, 14, 0, 101, 0, 101, 0, 14, 0, 101, 0, 14, // 12
  
        0, 102, 0, 14, 0, 101, 0, 94, 0, 102, 0, 102, 0, 14, 0, 101, 0, 101, 0, 14, 0, 102, 0, 14, 0, 102,
  
        0, 102, 0, 102, 0, 14, 0, 101, 0, 14, 0, 101, 0, 101, 0, 101, 0, 14, 0, 102, 0, 14, 0, 101, 0, 14,
  
        102, 0, 102, 0, 14, 0, 101, 0, 14, 0, 102, 0, 14, 0, 101, 0, 94, 1, 1, 5, 0, 35, 0, 0, 36, 0, // 15
  
        0, 4, 1, 2, 0, 4, 1, 8, 0, 5, 1, 1, 4, 0, 30, 0, 0, 31, 0, 0, 1, 1, 1, 1, 0, 94,
  
        37, 0, 0, 0, 0, 92, 38, 0, 0, 0, 0, 91, 95, 96, 35, 0, 0, 36, 0, 0, 4, 1, 8, 0, 4, 1,
  
        2, 0, 5, 1, 1, 4, 0, 20, 0, 20, 0, 35, 18, 0, 0, 36, 19, 0, 0, 35, 20, 0, 0, 0, 1, 1, // 18
   
        5, 0, 36, 19, 0, 0, 35, 18, 0, 0, 36, 20, 0, 0, 0, 1, 1, 5, 0, 38, 0, 20, 0, 0, 91, 37,
   
        0, 20, 20, 0, 92, 97, 96, 35, 0, 0, 36, 0, 0, 4, 2, 0, 4, 8, 0, 5, 0, 0, 0, 0, 0, 0,
   
        0, 0, 0, 0, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 21
    
        0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Medium", score_database2, "Medium");
    }
}