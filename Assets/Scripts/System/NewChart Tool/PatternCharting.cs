using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PatternLane
{
    public int PrimaryNote;
    public int hitDelay;
    public float xOffset;
    public bool random_xOffset;
}

[System.Serializable]
public class PaterrnDefine
{
    public PatternLane[] noteOutput;
    public float zOffset;
    public bool matchBeat_zOffset;
}

[CreateAssetMenu(fileName ="PatternCharting", menuName ="NotePattern")]
public class PatternCharting : ScriptableObject
{
    public int SecondaryIndex;
    public PaterrnDefine[] noteArray;
}

/* Pattern Mapping

    0 0 0
    0 0

    ChartUsage
    {
        noteArray
        {
            PatternLane
            {
                noteInput,
                xOffset
            }

            PatternLane
            {
                noteInput,
                xOffset
            }

            yOffset
        }

        noteArray
        {
            PatternLane
            {
                noteInput,
                xOffset
            }

            PatternLane
            {
                noteInput,
                xOffset
            }

            PatternLane
            {
                noteInput,
                xOffset
            }

            yOffset
        }
    }
*/