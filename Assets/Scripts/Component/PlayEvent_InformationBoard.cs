using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayEvent_InformationBoard : MonoBehaviour
{
    [SerializeField] private GameObject listing;
    [SerializeField] private RawImage slot_template;
    [SerializeField] private Text HeadTitle;
    [SerializeField] private Text SubTitle;

    [SerializeField] private Button EventIcon;
    [SerializeField] private GameObject PlayEventNotice;
    [SerializeField] private GameObject RewardPanelIcon;

    // Start is called before the first frame update
    void Start()
    {
        PlayEventListing();
        GetNextPileReward();
        EventIcon.interactable = true;

        if (!PlayerPrefs.HasKey("MarathonPermit")) PlayEventAlertBox();
    }

    #region SETUP
    private void PlayEventListing()
    {
        if (MeloMelo_ExtensionContent_Settings.GetEventRewardArray() != null)
        {
            int playId = 1;

            switch (ServerGateway_Script.thisServer.get_loginType)
            {
                case (int)MeloMelo_PlayerSettings.LoginType.GuestLogin:
                    HeadTitle.text = "Play Event: " + MeloMelo_ExtensionContent_Settings.GetPlayEventSetup().title;
                    SubTitle.text = "( " + MeloMelo_ExtensionContent_Settings.GetPlayEventSetup().Start + " to " +
                        MeloMelo_ExtensionContent_Settings.GetPlayEventSetup().End + " )";
                    break;

                default:
                    HeadTitle.text = "Play Event: " + "Private Server - Play Limited Event";
                    SubTitle.text = string.Empty;
                    break;

            }

            foreach (PlayEventRewardData reward in MeloMelo_ExtensionContent_Settings.GetEventRewardArray())
            {
                ItemData relocate_item = GetAllItemInGame(reward.itemName);

                if (relocate_item != null)
                {
                    RawImage createList_instance = Instantiate(slot_template, listing.transform);
                    createList_instance.transform.GetChild(0).GetComponent<RawImage>().texture = relocate_item.Icon;
                    createList_instance.transform.GetChild(1).GetComponent<Text>().text = relocate_item.itemName;
                    createList_instance.transform.GetChild(2).GetComponent<Text>().text = relocate_item.description;
                    createList_instance.transform.GetChild(3).GetComponent<Text>().text = reward.maxObtain.ToString();
                    createList_instance.transform.GetChild(4).GetComponent<Text>().text =
                        PlayerPrefs.GetInt(LoginPage_Script.thisPage.GetUserPortOutput() + "_playedCount_EventReward", 0) + " / " +
                        reward.playRequirement * PlayerPrefs.GetInt(playId + "_RepeatableRewarding", 1);
                        
                    createList_instance.gameObject.SetActive(true);
                }

                playId++;
            }
        }
    }

    private void GetNextPileReward()
    {
        RewardPanelIcon.transform.GetChild(0).GetComponent<Text>().text = "Next Reward: " + FindNextItemReward();

        RewardPanelIcon.transform.GetChild(1).GetComponent<Text>().text = "Total Accumulated Count: " +
            PlayerPrefs.GetInt(LoginPage_Script.thisPage.GetUserPortOutput() + "_playedCount_EventReward", 0);
    }
    #endregion

    #region MISC
    private ItemData GetAllItemInGame(string itemName)
    {
        foreach (ItemData item in MeloMelo_ItemStore_Management.preloaded_itemListing)
        {
            if (itemName == item.itemName) return item;
        }

        return null;
    }

    private string FindNextItemReward()
    {
        if (GetNextItemRewardByListing() != null)
        {
            if (GetNextItemRewardByListing().Count > 1)
            {
                string nextReward = string.Empty;

                for (int itemId = 0; itemId < GetNextItemRewardByListing().Count; itemId++)
                {
                    nextReward += GetNextItemRewardByListing()[itemId];
                    if (itemId + 1 < GetNextItemRewardByListing().Count) nextReward += ", ";
                }

                return nextReward;
            }
            else if (GetNextItemRewardByListing().Count == 1)
                return GetNextItemRewardByListing()[0];
            else
                return "- None -";
        }

        return "- No reward -";
    }

    private List<string> GetNextItemRewardByListing()
    {
        int eventPlayId = 1;
        List<string> allItemForNextObtain = new List<string>();

        var playEventRewards = MeloMelo_ExtensionContent_Settings
            .GetEventRewardArray()
            .OrderBy(r => r.playRequirement)
            .ToList();

        foreach (PlayEventRewardData reward in playEventRewards)
        {
            int playedCount = PlayerPrefs.GetInt(LoginPage_Script.thisPage.GetUserPortOutput() + "_playedCount_EventReward", 0);
            int numberOfObtainCount = PlayerPrefs.GetInt(eventPlayId + "_RepeatableRewarding", 1);

            int requiredPlays = reward.playRequirement * numberOfObtainCount;
            if (playedCount < requiredPlays) 
            {
                return playEventRewards
                .Where(r =>
                {
                    int id = playEventRewards.IndexOf(r) + 1;
                    int count = PlayerPrefs.GetInt(id + "_RepeatableRewarding", 1);
                    return r.playRequirement * count == requiredPlays;
                })
                .Select(r => r.itemName)
                .ToList();
            }

            eventPlayId++;
        }

        return allItemForNextObtain;
    }
    #endregion

    #region MISC (Play Event)
    private void PlayEventAlertBox()
    {
        if (PlayerPrefs.GetInt(MeloMelo_PlayerSettings.GetPlayEventSettings_DisplayKey) == 1 &&
            MeloMelo_ExtensionContent_Settings.GetEventRewardArray() != null)
        {
            PlayEventNotice.SetActive(true);
            bool isUpdateRequire = false;

            foreach (PlayEventRewardData data in MeloMelo_ExtensionContent_Settings.GetEventRewardArray())
            {
                if (MeloMelo_ExtensionContent_Settings.GetVersionNumber(StartMenu_Script.thisMenu.version) <
                    MeloMelo_ExtensionContent_Settings.GetVersionNumber(data.upToDate))
                {
                    isUpdateRequire = true;
                    break;
                }
            }

            PlayEventNotice.transform.GetChild(0).GetChild(0).GetComponent<Text>().text =
                GetPlayEventMessage(MeloMelo_ExtensionContent_Settings.GetEventRewardArray().Length < 1, isUpdateRequire ?
                "Game isn't up-to-date for this event" :
                "Keep playing track to obtain reward");

            Invoke("ClosePanelPlayEventNotice", 5);
        }
    }

    private string GetPlayEventMessage(bool eventFinsihed, string extraMessage)
    {
        if (eventFinsihed)
            return "Play Event has ended\n" + "Hope to see you on the next event";
        else
            return "Play Event is happening\n" + extraMessage;
    }

    private void ClosePanelPlayEventNotice()
    {
        PlayEventNotice.SetActive(false);
    }
    #endregion
}
