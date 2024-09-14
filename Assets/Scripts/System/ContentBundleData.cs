using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ContentBundleData", menuName = "Content_Bundle_Package")]
public class ContentBundleData : ScriptableObject
{
    public string title;
    public Texture Area_BG;
    public string sub_title;

    public enum ChartType { Legacy, NewChart };
    public ChartType chartType;
    public int content_id;

    public bool isRestricted;
    public bool newContent;
}
