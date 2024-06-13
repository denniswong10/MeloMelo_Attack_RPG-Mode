using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MultipleCharting", menuName = "MultiCharting")]
public class MultipleCharting : ScriptableObject
{
    [Header("Input")]
    public int FirstLane;
    public int SecondLane;
    public int ThirdLane;
    public int FourthLane;
    public int FifthLane;

    [Header("Output")]
    public int SecondaryIndex;

    [Header("Position Offset")]
    public float LaneSpacing = 1;
    public float zOffset;
}
