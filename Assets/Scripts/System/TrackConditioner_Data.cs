using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TrackConditioner_Data", menuName = "TrackImport_Condition")]
public class TrackConditioner_Data : ScriptableObject
{
    /// <summary>
    ///   
    ///     Condition Mode
    ///     
    ///     - Item => [<itemName>,<quality> or <itemName>]
    ///     - Track => [<title>,<difficulty>,<score> or <title>,<score>]
    ///     - Character => [<name>,<level>,<contentStatus> or <name>,<contentStatus>]
    ///     - Event => [<eventName>]
    ///     
    /// </summary>
    
    public enum Track_Condition_Type { Item, Track, Character, Event };

    [Header("Reference: Track Title")]
    public string title;

    [Header("Reference: Track Title")]
    public bool contentLocked;
    public Track_Condition_Type trackCondition;
    public string[] conditionArray;
}
