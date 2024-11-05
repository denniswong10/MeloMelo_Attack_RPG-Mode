using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[System.Serializable]
public class RestrictedZoneTemplate
{
    public string TrackTitle;
    public string coverImage_name;
    public int difficulty;
    public int score;
}

[CreateAssetMenu(fileName = "MuiscScore", menuName = "Music_Database")]
public class MusicScore : ScriptableObject
{
    [Header("Setup")]
    public float BPM;
    public float offset;
    public AudioClip Music;
    public float PreviewTime;

    [Header("Information")]
    public string ArtistName;
    public string Title;
    public string DesignerName;
    public Texture Background_Cover;
    public GameObject ScoreObject;

    [Header("Movie Version (Optional)")]
    public VideoClip videoImport;
    public float videoOffset;

    [Header("Deploy Enemy Unit")]
    public EnemyStatsFilter[] Insert_Enemy = new EnemyStatsFilter[3];

    [Header("Credit")]
    public string creditPoint;
    public float BPM_Skin;
    public bool UseBPMDisplay;
    public string ReleasedDate;

    [Header("Skill Leveling Label")]
    public int ScaleLevel;
    public int seasonNo;

    [Header("Additional Chart Content")]
    public bool NewChartSystem;
    public MultipleCharting[] addons1;
    public HookTechnical[] addons2;
    public PatternCharting[] addons3;
    public ChartModification[] addons3b;

    [Header("Additional Difficulty Content")]
    public bool UltimateAddons;

    [Header("Restricted Content: Requirement")]
    public bool SetRestriction;
    public RestrictedZoneTemplate[] RestrictRequirement;
}
