using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GatheringTag_Script : MonoBehaviour
{
    [SerializeField] private RawImage selectionTab;
    [SerializeField] private Transform placementTab;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(PlayerPrefs.HasKey("GatheringMode"));
    }

    public void OpenVirtualTab()
    {
        if (!GameObject.Find("GatheringInstance_Panel"))
        {
            RawImage virtualTab = Instantiate(selectionTab, placementTab);
            virtualTab.name = "GatheringInstance_Panel";
        }
    }
}
