using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EffectConditionData
{
    public string effectCondition;
    public string valueOfTrigger;
}

[System.Serializable]
public class EffectActionData
{
    public string effectActionName;
    public enum EffectActionStats { STR, VIT, MAG };
    public EffectActionStats effectMainStats;

    public int baseValue;
    public int extraPercentage;
}

[System.Serializable]
public class EffectDataSettings
{
    public string effectName;
    public enum ActivePhase { OnStart, DuringPlay, OnEnd };
    public ActivePhase effectActivationPhase;

    public EffectConditionData[] effectOnCondition;
    public EffectActionData[] effectOnAction;
}

[CreateAssetMenu(fileName = "SkillContainer", menuName = "Skill_Detail")]
public class SkillContainer : ScriptableObject
{
    [Header("Gerenal")]
    public string skillName;
    public Texture skillIcon;
    [TextAreaAttribute]
    public string description;

    public enum TargetType { Single, Multiple, AoE, Ally, Self }
    [Header("Skill: Customize and Caterogy")]
    public TargetType target_caterogy;
    public EffectDataSettings[] customEffectData;

    [Header("Skill Option")]
    public bool isUnlockReady;
}
