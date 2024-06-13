using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ChartModification", menuName ="ChartMod")]
public class ChartModification : ScriptableObject
{
    [Header("Assign for overriding of note")]
    public int newIndex;
    public int SecondaryNote;

    [Header("Modify Note")]
    public int PrimaryNote;
    public float LanePositioning = 0;
    public float scaling = 1;
}
