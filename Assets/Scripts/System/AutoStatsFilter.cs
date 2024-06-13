using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class List
{
    public int BaseDamage;
    public int BaseHealth;
    public int str;
    public int mag;
    public int vit;
    public int NextLevelExp;
}

[CreateAssetMenu(fileName = "AutoStatsFilter", menuName = "AutoFilter")]
public class AutoStatsFilter : ScriptableObject
{
    public List[] Level;
}
