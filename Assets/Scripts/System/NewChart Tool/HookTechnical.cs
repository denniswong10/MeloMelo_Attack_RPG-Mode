using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HookTechnical", menuName = "HookNote")]
public class HookTechnical : ScriptableObject
{
    [Header("Input")]
    public int PrimaryIndex;

    [Header("Output")]
    public int SecondaryIndex;

    [Header("Note Settings")]
    public int DelayNoteTick;
    public int NextNoteTick;
    public int HitPoints;
}
