using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrackRestriction_Character
{
    public string trackName;
    public int difficulty;
    public int score;
}

[CreateAssetMenu(fileName = "CharacterRestriction", menuName = "CharacterRestrict_Data")]
public class CharacterRestriction : ScriptableObject
{
    public string description;
    public TrackRestriction_Character[] trackCleared;
}
