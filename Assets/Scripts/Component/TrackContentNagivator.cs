using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackContentNagivator : MonoBehaviour
{
    public int trackIndex;
    public bool contentLocked;

    void Start()
    {
        transform.GetChild(0).gameObject.SetActive(contentLocked);
    }

    public void GetInfo()
    {
        CollectionNew_Script.thisCollect.OpenTrackInfo(trackIndex);
    }
}
