using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SkillEffectType
{
    public string effectName;
    public enum MainTypeStats { STR, VIT, MAG };
    public MainTypeStats stats;
    public int baseDamageStats;
    public int extraStatsPercentage;
    public string valueOfTrigger;
}

[CreateAssetMenu(fileName = "SkillContainer", menuName = "Skill_Detail")]
public class SkillContainer : ScriptableObject
{
    [Header("Gerenal")]
    public string skillName;
    public Texture skillIcon;
    [TextAreaAttribute]
    public string description;

    [Header("Skill Action Style")]
    public SkillEffectType[] onStartOfEffect;
    public SkillEffectType[] duringPlayOfEffect;
    public SkillEffectType[] onEndOfEffect;

    public enum TargetType { Single, Multiple, AoE, Ally, Self }
    public TargetType target_caterogy;

    [Header("Skill Option")]
    public bool isUnlockReady;
}
