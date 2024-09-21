using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AwardsSettings
{
    public enum TypeOfAwards { STR, VIT, MAG, SKILL }
    public TypeOfAwards awards_caterogy;
    public string awards_value;
}

[CreateAssetMenu(fileName = "MasteryContainer", menuName = "Mastery_Booklet")]
public class MasteryContainer : ScriptableObject
{
    public string title;
    [TextAreaAttribute]
    public string description;
    public string awardsTitle;

    [Header("Setup")]
    public AwardsSettings[] awards_settings;
}
