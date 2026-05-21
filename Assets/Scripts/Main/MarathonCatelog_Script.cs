using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarathonCatelog_Script : MonoBehaviour
{
    [SerializeField] private GameObject Registration_PinBox;
    [SerializeField] private GameObject contentList;
    [SerializeField] private RawImage eventInfo_template;

    // Start is called before the first frame update
    void Start()
    {
        GetMarathonRegisteredContent();
    }

    #region MAIN
    public void RegisterForEvent()
    {
        Registration_PinBox.SetActive(true);
    }

    public void RemoveFromEvent()
    {
       
    }

    public void SearchEvent()
    {

    }
    #endregion

    #region COMPONENT
    private void GetMarathonRegisteredContent()
    {
        bool isContentNotEmpty = false;

        if (contentList.transform.childCount - 2 > 0)
            ClearRegisteredContent();

        if (MeloMelo_ExtensionContent_Settings.marathonListing.data != null)
        {
            foreach (BuildInChallengeInfo contentInfo in MeloMelo_ExtensionContent_Settings.marathonListing.data)
            {
                isContentNotEmpty = true;
                EventFillOutFormInstance(contentInfo.title, "Add-ons Content", "- No expiry -", false);
            }
        }

        //if (MeloMelo_Economy.event_exchangeContentOfMarathon != null)
        //{
        //    foreach (MarathonLimitedExchange exchangeInfo in MeloMelo_Economy.event_exchangeContentOfMarathon)
        //    {
        //        isContentNotEmpty = true;

        //        EventFillOutFormInstance(
        //            exchangeInfo.exchangeBundleTitle,
        //            "Exchange Time-Limited",
        //            exchangeInfo.expirationDate != string.Empty ? ("- Expire by " + exchangeInfo.expirationDate + " -") : "- No expiry -",
        //            false
        //            );
        //    }
        //}

        EventEmptyTitle(!isContentNotEmpty);
    }

    private void ClearRegisteredContent()
    {
        for (int slot = 2; slot < contentList.transform.childCount; slot++)
        {
            Destroy(contentList.transform.GetChild(slot).gameObject);
        }
    }
    #endregion

    #region COMPONENT (Group Arrangement Setup)
    private RawImage EventFillOutFormInstance(string title, string typeOfEvent, string dateOfEvent, bool isEventPrivate)
    {
        RawImage eventForDisplay = Instantiate(eventInfo_template, contentList.transform);
        eventForDisplay.gameObject.SetActive(true);

        eventForDisplay.transform.GetChild(0).GetComponent<Text>().text = title;
        eventForDisplay.transform.GetChild(1).GetComponent<Text>().text = typeOfEvent;
        eventForDisplay.transform.GetChild(2).GetComponent<Text>().text = dateOfEvent;
        eventForDisplay.transform.GetChild(3).GetComponent<Text>().text = "Content Visible: " + (isEventPrivate ? "PRIVATE" : "PUBLIC");

        return eventForDisplay;
    }

    private void EventEmptyTitle(bool visible)
    {
        contentList.transform.GetChild(0).gameObject.SetActive(visible);
    }
    #endregion
}
