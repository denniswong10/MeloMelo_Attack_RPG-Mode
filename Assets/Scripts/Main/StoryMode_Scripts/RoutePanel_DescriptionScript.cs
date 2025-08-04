using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class RoutePanel_DescriptionScript : MonoBehaviour
{
    [SerializeField] private Text Title;
    [SerializeField] private Text Description;
    [SerializeField] private GameObject Interaction_Panel;

    [SerializeField] private GameObject Submission_Panel;
    [SerializeField] private GameObject Submission_Prompt_Panel;

    [SerializeField] private GameObject LoadingScreen_Content;

    private enum Interaction_Identify_Value { Detail, Play, Story, Task, ClaimRewards, AllButtonSelection, CLOSED, COMPLETED }

    private GameObject Cahce_Submission_Panel;
    private FragmentInfo currentStage;
    private int currentRouteId = 0;
    private bool isRouteUnlocked = false;

    void Update()
    {
        if (Cahce_Submission_Panel != null)
        {
            if (PlayerPrefs.GetInt("Progress_Fragement", 0) >= PlayerPrefs.GetInt("TempCount_MapFragment_Required", 0))
            {
                Cahce_Submission_Panel.GetComponent<VirtualStorageBag>().SetDefaultDescription("Gathering Completed!");

                Interaction_Panel.transform.GetChild((int)Interaction_Identify_Value.Detail).GetComponentInChildren<Text>().text = "FINISH";
                Interaction_Panel.transform.GetChild((int)Interaction_Identify_Value.Play).gameObject.SetActive(false);
            }

            else
            {
                Cahce_Submission_Panel.GetComponent<VirtualStorageBag>().SetDefaultDescription("Fragment Required: " +
                    PlayerPrefs.GetInt("Progress_Fragement", 0) + " / " + PlayerPrefs.GetInt("TempCount_MapFragment_Required", 0));

                Interaction_Panel.transform.GetChild((int)Interaction_Identify_Value.Detail).GetComponentInChildren<Text>().text = "SUBMIT ITEM";
                Interaction_Panel.transform.GetChild((int)Interaction_Identify_Value.Play).gameObject.SetActive(true);
            }

            if (!Cahce_Submission_Panel.activeInHierarchy) Cahce_Submission_Panel = null;
        }
    }

    #region MAIN
    public void UpdateCurrentStage(FragmentInfo info)
    {
        currentStage = info;
    }

    public void UpdateTitleContext(string title, int instance_id, SlotQuestLog.SlotType mode)
    {
        switch (mode)
        {
            case SlotQuestLog.SlotType.Story:
                Title.text = "Chapter " + instance_id + " - " + title + " ( Story Mode )";
                break;

            case SlotQuestLog.SlotType.Quest:
                Title.text = "Quest " + instance_id + " - " + title + " ( Mission Play )";
                break;

            case SlotQuestLog.SlotType.Step:
                Title.text = "Stage " + instance_id + " - " + title + " ( Progression Play )";
                break;

            default:
                Title.text = "Story Completed!";
                break;
        }
    }

    public void UpdateDescriptionContext(SlotQuestLog log)
    {
        currentRouteId = log.id;
        isRouteUnlocked = log.isOpen;
        ResetInteractionPanel();

        switch (log.mySlotTypte)
        {
            case SlotQuestLog.SlotType.Story:
                Description.text = "Goals: Complete Story Play Through" + "\n" + "Status: " + RouteCompletionStatus(log.id);
                GetInteractionButton((int)Interaction_Identify_Value.Story);
                break;

            case SlotQuestLog.SlotType.Step:
                Description.text = "Goals: " + GetFragmentRequire(log) + "\n" + "Status: " + RouteCompletionStatus(log.id);
                GetInteractionButton((int)Interaction_Identify_Value.Detail);
                GetInteractionButton((int)Interaction_Identify_Value.Play);
                break;

            case SlotQuestLog.SlotType.Quest:
                Description.text = "Goals: " + GetTaskPlay(log) + "\n" + "Status: " + RouteCompletionStatus(log.id);
                GetInteractionButton((int)Interaction_Identify_Value.Task);
                break;

            default:
                Description.text = "Goals: Claim rewards for playing in story mode";
                GetInteractionButton((int)Interaction_Identify_Value.ClaimRewards);
                break;
        }
    }
    #endregion

    #region COMPONENT 
    private string RouteCompletionStatus(int index)
    {
        int statusId = PlayerPrefs.GetInt("RoutePlayableStatus_" + PlayerPrefs.GetInt("StoryTypePlayBack", 0) + index, 0);
        return MeloMelo_Adventure.routeStatus[statusId];
    }

    private string GetFragmentRequire(SlotQuestLog log)
    {
        int dataPoint = 0;

        foreach (SlotQuestLog questLog in currentStage.myQuestLog)
        {
            if (questLog.mySlotTypte == SlotQuestLog.SlotType.Step)
            {
                if (questLog.id == log.id) break;
                else dataPoint++;
            }
        }

        if (dataPoint < currentStage.numberOfSteps.Length)
        {
            PlayerPrefs.SetInt("TempCount_MapFragment_Required", currentStage.numberOfSteps[dataPoint]);
            return "Gather " + currentStage.numberOfSteps[dataPoint] + " map fragment";
        }
        else
            PlayerPrefs.SetInt("TempCount_MapFragment_Required", 0);

        return "???";
    }

    private string GetTaskPlay(SlotQuestLog log)
    {
        int dataPoint = 0;

        foreach (SlotQuestLog questLog in currentStage.myQuestLog)
        {
            if (questLog.mySlotTypte == SlotQuestLog.SlotType.Quest)
            {
                if (questLog.id == log.id) break;
                else dataPoint++;
            }
        }

        if (dataPoint < currentStage.Quest_Stage.Length)
        {
            StoryMode_Scripts.thisStory.RegisterMissionTrack(currentStage.Quest_Stage[dataPoint]);
            return "Cleared " + currentStage.Quest_Stage[dataPoint].Title;
        }

        return "???";
    }

    private void MarkAsCompletedRoute()
    {
        int storyId = PlayerPrefs.GetInt("StoryTypePlayBack", 0);
        int routeId = currentRouteId;
        MeloMelo_Adventure.MarkRouteCleared(storyId, routeId);
    }
    #endregion

    #region MISC
    private void GetInteractionButton(int index)
    {
        if (Interaction_Panel != null)
        {
            if (StoryMode_Scripts.thisStory.IsTitleDeedPresented())
            {
                Interaction_Panel.transform.GetChild((int)Interaction_Identify_Value.COMPLETED - 1).gameObject.SetActive(true);
                return;
            }

            // Toggle the visibiltity on route play between each route available
            switch (isRouteUnlocked)
            {
                case true:
                    if (MeloMelo_Adventure.GetRouteCleared(PlayerPrefs.GetInt("StoryTypePlayBack", 0), currentRouteId))
                        Interaction_Panel.transform.GetChild((int)Interaction_Identify_Value.COMPLETED - 1).gameObject.SetActive(true);

                    else if (index < (int)Interaction_Identify_Value.AllButtonSelection)
                    {
                        Interaction_Panel.transform.GetChild(index).gameObject.SetActive(true);
                        Interaction_Panel.transform.GetChild(index).GetComponent<Button>().interactable = true;
                    }
                    break;

                case false:
                    if (!MeloMelo_Adventure.GetPreviousRouteCleared(PlayerPrefs.GetInt("StoryTypePlayBack", 0), currentRouteId))
                        Interaction_Panel.transform.GetChild((int)Interaction_Identify_Value.CLOSED - 1).gameObject.SetActive(true);

                    else if (MeloMelo_Adventure.GetRouteCleared(PlayerPrefs.GetInt("StoryTypePlayBack", 0), currentRouteId))
                        Interaction_Panel.transform.GetChild((int)Interaction_Identify_Value.COMPLETED - 1).gameObject.SetActive(true);

                    else if (index < (int)Interaction_Identify_Value.AllButtonSelection)
                    {
                        Interaction_Panel.transform.GetChild(index).gameObject.SetActive(true);
                        Interaction_Panel.transform.GetChild(index).GetComponent<Button>().interactable = true;
                    }
                    break;
            }
        }
    }

    private void ResetInteractionPanel()
    {
        if (Interaction_Panel != null)
        {
            for (int id = 0; id < Interaction_Panel.transform.childCount; id++)
            {
                GameObject selectionBtn = Interaction_Panel.transform.GetChild(id).gameObject;
                if (selectionBtn.activeInHierarchy) selectionBtn.SetActive(false);
            }
        }
    }
    #endregion

    #region MISC (Play Session)
    public void BeginStory()
    {
        StoryMode_Scripts.thisStory.BeginStoryMode(Title.text);
        if (PlayerPrefs.HasKey("Display_Story")) MarkAsCompletedRoute();
    }

    public void StartGatheringPlay()
    {
        PlayerPrefs.SetInt("GatheringMode", 1);
        PlayerPrefs.DeleteKey("Mission_Played");

        Interaction_Panel.transform.GetChild((int)Interaction_Identify_Value.Detail).GetComponent<Button>().interactable = true;
        StartCoroutine(StoryTransitionLeave("SeasonSelection"));
    }

    public void StartMissionPlay()
    {
        PlayerPrefs.SetInt("Mission_Played", 1);
        PlayerPrefs.SetInt("DifficultyLevel_valve", 1);
        PlayerPrefs.SetString("Mission_Title", "RoutePlayableStatus_" + PlayerPrefs.GetInt("StoryTypePlayBack", 0) + currentRouteId);
        StartCoroutine(StoryTransitionLeave("Music Selection Stage"));
    }

    private IEnumerator StoryTransitionLeave(string scene_load)
    {
        AsyncOperation isMissionLoaded = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene_load);
        while (!isMissionLoaded.isDone) yield return null;
    }
    #endregion

    #region MISC (Submission Item)
    public void CheckForSubmissionItem()
    {
        if (PlayerPrefs.GetInt("Progress_Fragement", 0) >= PlayerPrefs.GetInt("TempCount_MapFragment_Required", 0))
        {
            int usedFragment = PlayerPrefs.GetInt("Progress_Fragement", 0);
            int requiredFragment = PlayerPrefs.GetInt("TempCount_MapFragment_Required", 0);

            PlayerPrefs.SetInt("Progress_Fragement", usedFragment - requiredFragment);
            Interaction_Panel.transform.GetChild((int)Interaction_Identify_Value.Detail).GetComponent<Button>().interactable = false;
            MarkAsCompletedRoute();
        }

        else if (PlayerPrefs.GetInt("RoutePlayableStatus_" + PlayerPrefs.GetInt("StoryTypePlayBack", 0) + currentRouteId, 0) != 1)
        {
            PlayerPrefs.SetInt("RoutePlayableStatus_" + PlayerPrefs.GetInt("StoryTypePlayBack", 0) + currentRouteId, 2);
            StartCoroutine(OpenSubmissionPanel());
        }
    }

    private IEnumerator OpenSubmissionPanel()
    {
        if (GameObject.Find("SubmissionPanel") == null)
        {
            LoadingScreen_Content.SetActive(true);

            Task<List<UsageOfItemDetail>> allFilteredItem = PreLoadingFilteredItem("Material_Section");
            yield return new WaitUntil(() => allFilteredItem.IsCompleted);

            Task<List<VirtualItemDatabase>> isFilteredPerformed = PerformFilteredItem(allFilteredItem.Result.ToArray());
            yield return new WaitUntil(() => isFilteredPerformed.IsCompleted);

            GameObject instance_panel = Instantiate(Submission_Panel, StoryMode_Scripts.thisStory.selection_main.transform);
            instance_panel.name = "SubmissionPanel";
            LoadingScreen_Content.SetActive(false);

            Cahce_Submission_Panel = instance_panel;
            instance_panel.GetComponent<VirtualStorageBag>().SetAlertPopReference(Submission_Prompt_Panel);
            instance_panel.GetComponent<VirtualStorageBag>().SetDefaultDescription("Select a item to complete your submission");
            instance_panel.GetComponent<VirtualStorageBag>().SetItemForDisplay(isFilteredPerformed.Result.ToArray());
            instance_panel.GetComponent<VirtualStorageBag>().SetLimitedUsageTime(false);
            PlayerPrefs.SetString(VirtualStorageBag.VirtualStorage_UsableKey, "TRUE");
        }
    }

    private async Task<List<UsageOfItemDetail>> PreLoadingFilteredItem(string panelType)
    {
        if (panelType != string.Empty)
        {
            List<UsageOfItemDetail> loadedItem = new List<UsageOfItemDetail>();
            ResourceRequest isItemFound;
            UsageOfItemDetail itemReadReady;
            int currentLoadedIndex = 1;

            do
            {
                isItemFound = Resources.LoadAsync("Database_Item/Filtered_Items/" + panelType + "/#" + currentLoadedIndex);
                while (!isItemFound.isDone) await Task.Yield();
                itemReadReady = isItemFound.asset as UsageOfItemDetail;

                if (itemReadReady != null) loadedItem.Add(itemReadReady);
                currentLoadedIndex++;
            }
            while (itemReadReady != null);

            return loadedItem;
        }

        return null;
    }

    private async Task<List<VirtualItemDatabase>> PerformFilteredItem(UsageOfItemDetail[] readyItemList)
    {
        return await Task.Run(() => 
        {
            if (readyItemList != null && readyItemList.Length > 0)
            {
                List<VirtualItemDatabase> listOfItem = new List<VirtualItemDatabase>();

                foreach (UsageOfItemDetail item in readyItemList)
                {
                    VirtualItemDatabase itemFound = MeloMelo_ItemUsage_Settings.GetActiveItem(item.itemName);
                    if (itemFound.amount > 0) listOfItem.Add(itemFound);
                }

                return listOfItem;
            }

            return null;
        });
    }
    #endregion

    #region MISC (Rewarding Session)
    public void ClaimRewardInteraction(Button interaction)
    {
        interaction.interactable = false;
        ItemData[] itemList = Resources.LoadAll<ItemData>("Database_Item");

        MeloMelo_Local.LocalSave_DataManagement openStorage = new MeloMelo_Local.LocalSave_DataManagement(
                            LoginPage_Script.thisPage.GetUserPortOutput(), "StreamingAssets/LocalData/MeloMelo_LocalSave_InGameProgress");
        openStorage.SelectFileForActionWithUserTag(MeloMelo_GameSettings.GetLocalFileVirtualItemData);

        foreach (RewardTerminal reward in StoryMode_Scripts.thisStory.GetStoryArea().storyEndRewards)
        {
            foreach (ItemData item in itemList)
            {
                if (item.itemName == reward.rewardName && !IsItemAlreadyBeenClaimed(item.itemName))
                {
                    openStorage.SaveVirtualItemFromPlayer(item.itemName, 1, item.stackable);
                    StoryMode_Scripts.thisStory.ItemMessageAlert("Item Added: " + item.itemName);
                    break;
                }
            }
        }

        MarkAsCompletedRoute();
    }

    private bool IsItemAlreadyBeenClaimed(string itemName)
    {
        VirtualItemDatabase itemExisting = MeloMelo_ItemUsage_Settings.GetActiveItem(itemName);
        return itemExisting.itemName == itemName && itemExisting.amount > 0;
    }
    #endregion
}
