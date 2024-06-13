using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AreaInfo", menuName = "AreaRegister")]
public class AreaInfo : ScriptableObject
{
    public Texture BG;
    public string AreaName;
    public enum AreaType { Main, Event, Exclusive };
    public AreaType thisType = AreaType.Main;
    public bool checkArea;
    public bool MemberOnly;

    public int totalMusic;
    public int[] EnemyBaseHealth;

    [Header("File_Label")]
    public int season_num;
    public string package_title;
    public bool reverseOrder;
}
