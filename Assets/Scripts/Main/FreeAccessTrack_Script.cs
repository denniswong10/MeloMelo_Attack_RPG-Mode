using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreeAccessTrack_Script : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private GameObject selectionPanel;
    [SerializeField] private GameObject virtualDisplayPanel;
    [SerializeField] private GameObject PromptMessage;

    [Header("Item Reference: Ticket")]
    [SerializeField] private ItemData defaultTicketUsage;

    void Update()
    {
        if (selectionPanel.activeInHierarchy)
        {
            if (PlayerPrefs.GetInt("BonusTrackPlay", 0) > 0) UpdateTicketRequireStatus("FREE PLAY", PlayerPrefs.GetInt("BonusTrackPlay", 0));
            else
            {
                const int usedTicket = 1;
                int totalAvailableTicket = MeloMelo_ItemUsage_Settings.GetActiveItem(defaultTicketUsage.itemName).amount -
                MeloMelo_ItemUsage_Settings.GetItemUsed(defaultTicketUsage.itemName);
                UpdateTicketRequireStatus(defaultTicketUsage.itemName, totalAvailableTicket, usedTicket);
            }
        }
    }

    #region MAIN
    public void OpenMainSelection()
    {
        selectionPanel.SetActive(true);
        GettingLockedContent();
    }

    public void OpenFreePlay()
    {
        CheckUsedOfFreeTicket();
        CheckForAccessToFreeTrack();
    }

    public void CloseObtainTab()
    {
        selectionPanel.SetActive(false);
    }
    #endregion

    #region COMPONENT 
    private void GettingLockedContent()
    {
        if (GetComponent<SelectionMenu_Script>().get_selection != null && 
            GetComponent<SelectionMenu_Script>().get_selection.get_form.SetRestriction)
        {
            GameObject contentListing = selectionPanel.transform.GetChild(7).GetChild(0).GetChild(0).gameObject;
            ClearContentSlot(contentListing);

            foreach (RestrictedZoneTemplate track in GetComponent<SelectionMenu_Script>().get_selection.get_form.RestrictRequirement)
            {
                GameObject trackDetail = Instantiate(contentListing.transform.GetChild(0).gameObject, contentListing.transform);
                trackDetail.SetActive(true);

                trackDetail.GetComponent<RawImage>().texture = track.coverImage_name != string.Empty ?
                    Resources.Load<Texture>("Database_CoverImage/" + track.coverImage_name.Split(",")[0] + "/" +
                    track.coverImage_name.Split(",")[1]) : contentListing.transform.GetChild(0).GetComponent<RawImage>().texture;

                trackDetail.transform.GetChild(0).GetComponent<Text>().text = "Title: " + track.TrackTitle + "\n"
                    + "Difficulty: " + (track.difficulty == 1 ? "NORMAL" : track.difficulty == 2 ? "HARD" : 
                        track.difficulty == 3 ? "ULTIMATE" : "ANY ONE") + "\n"
                            + "Cleared: " + MeloMelo_GameSettings.GetScoreRankStructure(track.score.ToString()).rank;
            }
        }
    }

    private void ClearContentSlot(GameObject reference)
    {
        for (int instance = 1; instance < reference.transform.childCount; instance++)
            Destroy(reference.transform.GetChild(instance).gameObject);
    }

    private void UpdateTicketRequireStatus(string itemName, int current, int _usedTicket = -1)
    {
        selectionPanel.transform.GetChild(5).GetComponent<Text>().text = itemName;
        selectionPanel.transform.GetChild(6).GetComponent<Text>().text = current + (_usedTicket != -1 ? " / " + _usedTicket : " more left");
        selectionPanel.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = _usedTicket != 1 || current > _usedTicket ?
            "PLAY" : "USE TICKET";
    }
    #endregion

    #region MISC 
    private void CheckUsedOfFreeTicket()
    {
        if (MeloMelo_ItemUsage_Settings.GetItemUsed(defaultTicketUsage.itemName) < 1 &&
            MeloMelo_ItemUsage_Settings.GetActiveItem(defaultTicketUsage.itemName).amount > 0)
                MeloMelo_ItemUsage_Settings.SetItemUsed(defaultTicketUsage.itemName);
    }

    private void CheckForAccessToFreeTrack()
    {
        if (PlayerPrefs.GetInt("BonusTrackPlay", 0) > 0 || MeloMelo_ItemUsage_Settings.GetItemUsed(defaultTicketUsage.itemName) > 0)
        {
            selectionPanel.SetActive(false);
            GetComponent<SelectionMenu_Script>().BattleMode();
        }
        else
            OpenVirtualItemStore();
    }

    public void ResetUnusedTicket()
    {
        // Checking for ticket used in the incorrect format
        if (!GetComponent<MusicSelectionPage>().get_form.SetRestriction)
        {
            if (MeloMelo_ItemUsage_Settings.GetItemUsed(defaultTicketUsage.itemName) > 0)
                PlayerPrefs.DeleteKey(defaultTicketUsage.itemName + "_VirtualItem_Unsaved_Used");
        }
    }
    #endregion

    #region MISC (Item Sorter)
    private void OpenVirtualItemStore()
    {
        if (GameObject.Find("TicketUsedForTrack") == null)
        {
            GameObject instance_panel = Instantiate(virtualDisplayPanel, GetComponent<SelectionMenu_Script>().currentActiveInfo.transform);
            instance_panel.name = "TicketUsedForTrack";

            instance_panel.GetComponent<VirtualStorageBag>().SetAlertPopReference(PromptMessage);
            instance_panel.GetComponent<VirtualStorageBag>().SetDefaultDescription("Select a ticket to access free play for this track");
            instance_panel.GetComponent<VirtualStorageBag>().SetItemForDisplay(GetItemArray("TRACK_TICKET"));
            instance_panel.GetComponent<VirtualStorageBag>().SetLimitedUsageTime(true);
            PlayerPrefs.SetString(VirtualStorageBag.VirtualStorage_UsableKey, "TRUE");
        }
    }

    private VirtualItemDatabase[] GetItemArray(string panelType)
    {
        List<VirtualItemDatabase> listOfItem;
        if (panelType != string.Empty)
        {
            listOfItem = new List<VirtualItemDatabase>();
            foreach (UsageOfItemDetail item in Resources.LoadAll<UsageOfItemDetail>("Database_Item/Filtered_Items/" + panelType))
            {
                VirtualItemDatabase itemFound = MeloMelo_ItemUsage_Settings.GetActiveItem(item.itemName);
                if (itemFound.amount > 0) listOfItem.Add(itemFound);
            }

            return listOfItem.ToArray();
        }

        return null;
    }
    #endregion
}
