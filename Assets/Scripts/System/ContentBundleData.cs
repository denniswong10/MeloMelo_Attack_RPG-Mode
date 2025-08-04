using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ContentBundleData", menuName = "Content_Bundle_Package")]
public class ContentBundleData : ScriptableObject
{
    public string title;
    public Texture Area_BG;
    public string sub_title;

    public enum ChartType { Legacy, Modern, NewChart };
    public ChartType chartType;
    public int content_id;

    public bool isRestricted;
    public bool newContent;

    public string ChartRepresentType()
    {
        switch (chartType)
        {
            case ChartType.Legacy:
                return "LEGACY";

            case ChartType.Modern:
                return "MODERN";

            case ChartType.NewChart:
                return "NEW CHART";

            default:
                return "???";
        }    
    }
}
