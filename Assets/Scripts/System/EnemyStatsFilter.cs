using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitGroup
{
    public int HP;
    public int DMG;
    public int str;
    public int mag;
    public int vit;
}

[CreateAssetMenu(fileName = "EnemyStatsFilter", menuName = "EnemyStats")]
public class EnemyStatsFilter : ScriptableObject
{
    public int level;
    public UnitGroup[] myEnemySlot = new UnitGroup[3];
}
