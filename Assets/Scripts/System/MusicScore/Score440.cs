using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeloMelo_LevelBuilder;

public class Score440 : MonoBehaviour
{
    // 35, 36 (QuickTapping)    18, 300, 301 (Eight)    42, 43 (BowPattern1)    53, 103, 104 (Circle)
    // 60, 20, 19 (FixedHeartPack)    91, 92 (FixedAttack)    94 (FixedAirAttack)    15, 16 (BowTwist)    76 (MultipleHitStar)    88 (RisingStar_Random)
    // 85, 87, 86 (RatingFogToogle)    51 (Heart)    12, 204, 203, 205 (HookS)    34 (PonyDiamond)    14 (House)    52 (Circle2)    102 (HeartRED)

    public int difficulty;

    private int[] score_database =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 8, 8, 0, 0, 1,

        1, 0, 5, 0, 9, 0, 0, 300, 0, 0, 0, 301, 0, 0, 0, 18, 0, 20, 20, 0, 102, 0, 0, 102, 0, 0,

        0, 94, 0, 0, 15, 0, 0, 0, 0, 16, 0, 0, 0, 0, 0, 5, 4, 0, 1, 0, 1, 0, 5, 0, 1, 0, // 3
  
        1, 0, 9, 0, 34, 0, 20, 0, 20, 0, 20, 0, 0, 103, 0, 0, 104, 0, 0, 53, 0, 0, 104, 20, 0, 103,

        20, 0, 53, 0, 94, 0, 0, 300, 0, 0, 301, 0, 0, 300, 0, 0, 301, 0, 0, 18, 0, 0, 0, 94, 0, 2,

        2, 0, 8, 8, 0, 1, 5, 0, 1, 9, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 6
   
        0, 0, 1, 1, 0, 0, 5, 0, 5, 0, 34, 0, 20, 0, 20, 0, 0, 1, 1, 0, 0, 5, 0, 5, 0, 9,

        0, 20, 20, 0, 34, 0, 0, 20, 0, 20, 0, 12, 0, 0, 0, 12, 0, 0, 0, 34, 0, 0, 20, 0, 20, 0,

        12, 20, 0, 0, 12, 20, 0, 0, 0, 5, 0, 0, 0, 76, 0, 76, 0, 1, 5, 0, 0, 76, 0, 76, 0, 1, // 9
  
        9, 0, 103, 0, 53, 0, 104, 0, 20, 0, 0, 34, 0, 20, 0, 20, 0, 20, 0, 93, 0, 0, 93, 0, 34, 0,

        20, 0, 20, 0, 20, 0, 52, 0, 0, 7, 0, 0, 0, 0, 2, 0, 0, 1, 0, 1, 0, 5, 0, 76, 9, 0,

        0, 8, 0, 0, 1, 0, 1, 0, 5, 0, 76, 0, 76, 0, 0, 7, 0, 0, 2, 2, 0, 8, 8, 0, 1, 5, // 12
   
        0, 1, 9, 0, 0, 0, 0, 0, 0, 0, 42, 0, 0, 0, 43, 0, 0, 0, 102, 0, 0, 0, 12, 0, 0, 0,

        102, 0, 0, 0, 12, 0, 20, 20, 0, 1, 5, 0, 1, 9, 0, 0, 2, 2, 0, 0, 1, 0, 5, 0, 9, 0,

        0, 1, 0, 9, 0, 5, 0, 0, 76, 0, 76, 0, 76, 0, 300, 0, 0, 301, 0, 0, 300, 0, 0, 301, 0, 0, // 15
  
        0, 0, 0, 2, 0, 8, 0, 0, 0, 0, 1, 0, 5, 0, 4, 0, 1, 0, 5, 0, 6, 0, 1, 1, 0, 5,

        0, 1, 1, 0, 9, 0, 53, 0, 0, 53, 0, 0, 102, 0, 0, 0, 34, 0, 60, 0, 20, 0, 19, 0, 34, 0,

        20, 0, 60, 0, 20, 0, 4, 4, 0, 5, 0, 0, 93, 0, 0, 5, 0, 9, 0, 5, 0, 9, 0, 1, 1, 0, // 18
  
        3, 0, 0, 4, 0, 0, 9, 0, 5, 0, 9, 0, 5, 0, 1, 1, 0, 3, 0, 0, 0, 0, 93, 0, 0, 0,

        93, 0, 0, 0, 76, 0, 0, 94, 0, 0, 0, 76, 0, 0, 94, 0, 94, 0, 0, 1, 1, 0, 6, 0, 2, 2,

        0, 5, 0, 1, 1, 0, 6, 0, 8, 8, 0, 93, 0, 76, 0, 76, 0, 20, 0, 20, 94, 0, 0, 7, 0, 0, // 21
   
        5, 9, 0, 300, 60, 0, 301, 19, 0, 18, 0, 0, 0, 8, 4, 0, 102, 0, 0, 0, 0, 7, 0, 0, 0, 0,

        87, 0, 0, 0, 12, 0, 0, 20, 20, 0, 1, 5, 0, 1, 9, 0, 12, 0, 0, 60, 19, 0, 0, 76, 0, 76,

        0, 9, 0, 5, 0, 0, 0, 2, 0, 2, 0, 8, 0, 8, 0, 5, 4, 0, 5, 6, 0, 0, 42, 0, 0, 43, // 24
   
        0, 0, 42, 0, 0, 43, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2,

        2, 0, 8, 0, 0, 0, 93, 0, 0, 0, 93, 0, 0, 0, 3, 0, 0, 0, 7, 0, 0, 0, 3, 0, 0, 0,

        8, 8, 0, 2, 0, 4, 0, 0, 0, 0, 93, 0, 0, 0, 93, 0, 0, 0, 3, 0, 0, 0, 7, 0, 0, 0, // 27
   
        3, 0, 0, 0, 1, 5, 0, 1, 9, 0, 0, 0, 0, 5, 9, 0, 5, 9, 0, 42, 0, 0, 43, 0, 0, 42,

        0, 0, 43, 0, 0, 0, 20, 0, 20, 0, 1, 1, 0, 5, 0, 51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 87, 0, 0, 0, 9, 0, 0, 87, 0, // 30
   
        0, 0, 9, 0, 0, 87, 0, 0, 0, 9, 0, 0, 53, 0, 0, 53, 0, 0, 0, 0, 0, 0, 2, 0, 4, 0,

        8, 0, 4, 0, 5, 5, 0, 94, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_database2 =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 5, 0, 0, 9, 0, 0, 1,
  
        1, 0, 2, 8, 4, 0, 0, 103, 0, 1, 1, 5, 0, 104, 0, 1, 1, 9, 0, 53, 0, 1, 4, 0, 1, 9, 
 
        0, 5, 0, 0, 34, 0, 60, 0, 20, 0, 19, 0, 34, 0, 19, 0, 20, 0, 60, 0, 5, 9, 5, 0, 42, 0, // 3
  
        0, 43, 0, 0, 0, 1, 1, 5, 0, 300, 0, 0, 0, 301, 0, 0, 0, 18, 0, 0, 0, 0, 20, 20, 0, 94,
  
        0, 1, 8, 5, 9, 0, 0, 76, 0, 76, 0, 1, 1, 5, 0, 1, 1, 9, 0, 76, 0, 76, 0, 1, 5, 0,
   
        4, 5, 0, 1, 9, 5, 0, 2, 2, 8, 8, 0, 5, 0, 0, 3, 0, 0, 0, 17, 0, 0, 0, 3, 0, 0, // 6
   
        0, 1, 1, 1, 5, 0, 15, 0, 20, 0, 0, 16, 0, 20, 0, 0, 5, 4, 0, 1, 9, 0, 5, 4, 0, 87,
   
        0, 0, 0, 87, 0, 0, 0, 1, 1, 5, 9, 0, 34, 0, 60, 20, 0, 0, 94, 0, 1, 1, 9, 5, 0, 34,
  
        0, 19, 20, 0, 0, 94, 0, 1, 1, 5, 0, 0, 0, 5, 5, 9, 0, 76, 0, 76, 0, 1, 1, 1, 0, 5, // 9
  
        5, 9, 0, 204, 0, 0, 205, 0, 17, 0, 0, 43, 0, 0, 43, 0, 0, 42, 0, 20, 43, 0, 20, 0, 34, 0,
  
        60, 20, 0, 19, 20, 0, 18, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 5, 0, 0, 0, 9, 0, 1, 1, 0,
   
        0, 5, 0, 0, 2, 0, 8, 0, 5, 0, 93, 0, 93, 0, 0, 1, 1, 0, 5, 0, 5, 0, 1, 1, 0, 4, // 12
   
        0, 5, 0, 5, 0, 0, 0, 0, 0, 0, 1, 2, 0, 1, 8, 0, 0, 0, 1, 1, 5, 0, 12, 0, 60, 19,
   
        0, 1, 1, 5, 0, 12, 0, 19, 60, 0, 42, 0, 43, 0, 0, 0, 1, 5, 0, 0, 9, 0, 5, 0, 1, 1,
   
        0, 5, 0, 2, 8, 5, 0, 0, 76, 4, 0, 76, 4, 0, 53, 0, 53, 0, 103, 0, 104, 0, 103, 0, 104, 0, // 15
  
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 5, 0, 0, 1, 1, 0, 0, 9, 0, 0, 5, 5, 5,
  
        9, 0, 12, 0, 20, 0, 12, 0, 20, 0, 53, 0, 53, 0, 91, 94, 92, 0, 94, 92, 94, 0, 1, 0, 1, 0,
  
        0, 204, 0, 205, 0, 102, 0, 0, 0, 5, 0, 0, 17, 0, 0, 1, 76, 0, 1, 76, 0, 5, 9, 0, 1, 1, // 18
  
        4, 0, 0, 5, 0, 0, 1, 76, 0, 1, 76, 0, 9, 5, 0, 1, 1, 4, 0, 9, 0, 0, 17, 0, 0, 0,
  
        17, 0, 0, 1, 1, 5, 0, 94, 0, 0, 0, 3, 0, 0, 93, 0, 0, 3, 0, 0, 15, 0, 20, 0, 20, 0,
  
        0, 20, 0, 16, 0, 20, 0, 20, 0, 20, 5, 0, 0, 42, 0, 0, 43, 0, 0, 42, 60, 0, 43, 19, 0, 0, // 21
   
        5, 9, 0, 76, 0, 1, 1, 5, 5, 0, 9, 0, 1, 1, 1, 0, 51, 0, 0, 0, 0, 93, 0, 0, 0, 0,
   
        87, 0, 0, 0, 5, 0, 9, 0, 5, 0, 0, 1, 1, 0, 9, 0, 103, 0, 104, 0, 103, 0, 0, 1, 1, 5,
   
        5, 0, 76, 76, 0, 0, 0, 2, 2, 5, 0, 8, 8, 5, 0, 1, 5, 1, 9, 0, 1, 5, 1, 9, 0, 102, // 24
   
        0, 0, 0, 5, 4, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
   
        0, 0, 5, 0, 0, 0, 2, 0, 0, 0, 2, 0, 0, 0, 8, 0, 0, 0, 8, 0, 0, 0, 4, 0, 0, 0,
   
        1, 1, 0, 0, 0, 5, 0, 0, 0, 0, 2, 0, 0, 0, 2, 0, 0, 0, 8, 0, 0, 0, 8, 0, 0, 0, // 27
   
        4, 0, 0, 0, 1, 5, 0, 1, 9, 0, 0, 0, 0, 76, 0, 76, 0, 76, 0, 20, 0, 76, 0, 76, 0, 76,
   
        0, 20, 0, 1, 76, 0, 20, 0, 1, 76, 0, 20, 0, 5, 5, 51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
   
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 5, 0, 5, 0, 87, 0, // 30
   
        0, 0, 0, 1, 1, 9, 0, 5, 0, 5, 0, 6, 102, 0, 0, 0, 5, 9, 0, 0, 0, 0, 1, 1, 0, 9,
   
        5, 0, 20, 20, 0, 60, 0, 19, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
    
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    private int[] score_database3 =
    {
        0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 2, 0, 8, 0, 8, 0, 1,

        1, 1, 0, 5, 9, 0, 0, 103, 19, 0, 92, 94, 91, 0, 104, 60, 0, 91, 94, 92, 0, 42, 20, 0, 43, 20,

        0, 9, 0, 0, 300, 20, 20, 0, 301, 20, 20, 0, 1, 1, 1, 0, 5, 0, 1, 1, 1, 0, 9, 0, 42, 19, // 3
  
        0, 43, 60, 0, 20, 91, 94, 92, 0, 15, 0, 0, 0, 0, 16, 0, 0, 0, 0, 103, 20, 0, 104, 20, 0, 88,

        0, 91, 94, 92, 94, 0, 0, 76, 0, 76, 0, 91, 20, 92, 0, 92, 20, 91, 0, 91, 94, 92, 0, 7, 0, 93,

        2, 5, 0, 93, 8, 5, 0, 1, 5, 1, 9, 0, 3, 0, 0, 85, 0, 0, 0, 86, 0, 0, 0, 87, 0, 0, // 6
   
        0, 0, 76, 0, 76, 0, 1, 1, 6, 0, 204, 0, 205, 0, 20, 0, 1, 1, 6, 0, 205, 0, 204, 0, 0, 12,

        0, 0, 20, 20, 0, 0, 34, 0, 60, 0, 20, 0, 19, 0, 0, 18, 0, 0, 7, 0, 34, 0, 19, 0, 20, 0,

        60, 0, 0, 18, 0, 0, 7, 0, 87, 0, 0, 0, 0, 76, 6, 5, 0, 1, 1, 5, 0, 76, 6, 5, 0, 1, // 9
  
        1, 9, 0, 300, 0, 0, 301, 0, 19, 0, 0, 34, 0, 60, 0, 19, 0, 60, 0, 52, 0, 0, 7, 0, 34, 0,

        19, 0, 60, 0, 19, 0, 14, 0, 0, 7, 0, 0, 0, 0, 3, 0, 0, 2, 0, 2, 0, 5, 0, 76, 9, 0,

        0, 3, 0, 0, 8, 0, 8, 0, 5, 0, 76, 0, 76, 0, 0, 3, 0, 0, 2, 4, 2, 0, 8, 4, 8, 0, // 12
   
        1, 5, 1, 5, 0, 0, 0, 0, 0, 0, 1, 5, 0, 1, 9, 0, 0, 0, 204, 0, 12, 0, 205, 0, 12, 0,

        204, 0, 0, 42, 0, 20, 43, 0, 20, 42, 0, 20, 20, 5, 0, 0, 7, 3, 0, 0, 1, 0, 93, 0, 5, 0,

        0, 1, 0, 93, 0, 5, 0, 0, 76, 9, 0, 76, 9, 0, 103, 0, 104, 0, 204, 0, 205, 0, 204, 60, 0, 0, // 15
  
        0, 0, 0, 7, 0, 7, 0, 0, 0, 0, 1, 1, 0, 0, 5, 0, 1, 1, 0, 0, 9, 0, 1, 5, 0, 1,

        9, 0, 34, 0, 20, 0, 20, 0, 20, 0, 103, 0, 53, 0, 104, 0, 94, 91, 91, 0, 94, 92, 92, 0, 42, 0,

        0, 43, 0, 0, 20, 102, 0, 0, 0, 5, 0, 0, 93, 0, 0, 76, 0, 76, 0, 76, 0, 76, 0, 1, 5, 1, // 18
  
        9, 0, 0, 88, 0, 0, 76, 0, 76, 0, 76, 0, 76, 0, 1, 9, 1, 5, 0, 0, 0, 0, 3, 0, 0, 0,

        3, 0, 0, 0, 52, 0, 0, 94, 0, 0, 0, 93, 0, 0, 93, 0, 0, 93, 0, 0, 1, 1, 15, 0, 0, 0,

        0, 5, 0, 1, 1, 16, 0, 0, 0, 0, 5, 0, 0, 76, 9, 0, 42, 60, 0, 43, 19, 0, 0, 94, 0, 0, // 21
   
        76, 9, 0, 43, 19, 0, 42, 60, 0, 0, 94, 0, 1, 5, 1, 9, 51, 0, 0, 0, 0, 7, 0, 0, 0, 0,

        87, 0, 0, 0, 204, 0, 203, 0, 205, 0, 0, 1, 5, 1, 9, 0, 205, 0, 203, 0, 204, 0, 0, 1, 9, 1,

        5, 0, 76, 9, 0, 0, 0, 2, 2, 2, 93, 8, 8, 8, 93, 0, 1, 1, 5, 0, 93, 0, 1, 1, 9, 0, // 24
   
        3, 0, 5, 9, 5, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5,

        5, 5, 9, 0, 0, 0, 3, 0, 0, 0, 7, 0, 0, 0, 3, 0, 0, 0, 7, 0, 0, 0, 3, 0, 0, 0,

        1, 0, 5, 5, 5, 4, 0, 0, 0, 0, 3, 0, 0, 0, 7, 0, 0, 0, 3, 0, 0, 0, 7, 0, 0, 0, // 27
   
        3, 0, 0, 0, 1, 0, 5, 5, 9, 0, 0, 0, 0, 76, 9, 0, 42, 20, 0, 43, 20, 0, 0, 76, 0, 76,

        9, 0, 103, 53, 104, 0, 43, 19, 20, 42, 60, 20, 0, 1, 1, 51, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 87, 0, 0, 0, 9, 5, 5, 87, 0, // 30
   
        0, 0, 9, 5, 5, 87, 0, 0, 0, 9, 5, 5, 53, 0, 0, 0, 8, 4, 0, 0, 0, 0, 1, 5, 9, 0,

        102, 20, 0, 51, 0, 0, 0, 94, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,

        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
    };

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("DifficultyLevel_valve", 1);
        ScoreController controller = new ScoreController(difficulty, score_database, "Medium", score_database2, "Medium", score_database3, "Medium");
    }
}
