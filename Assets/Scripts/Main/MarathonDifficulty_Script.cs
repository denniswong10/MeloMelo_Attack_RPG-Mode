using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class MarathonDifficulty_Script : MonoBehaviour
{
    private MarathonAdditionalMode addtional_mode;
    private string[] difficulty_mode = { "Easy Mode", "Normal Mode", "Hard Mode" };
    [SerializeField] private Text Difficulty_Display;
    [SerializeField] private Button[] NavigatorButton;
    [SerializeField] private GameObject DisplayPanel;
    [SerializeField] private GameObject PromptMessage;
    [SerializeField] private GameObject MainPanel;
    private string currentInfo_cache;

    // Start is called before the first frame update
    void Start()
    {
        currentInfo_cache = string.Empty;
    }

    #region MAIN
    public void ModifyChange(bool reserve)
    {
        if (CheckSupportPass())
        {
            int currentState = PlayerPrefs.GetInt("MarathonPlay_DifficultyMode", 1);
            PlayerPrefs.SetInt("MarathonPlay_DifficultyMode", currentState + (reserve ? -1 : 1));
            UpdateDifficultyMode();
        }
    }

    public void CheckParameterForAdditionalMode()
    {
        if (currentInfo_cache != PlayerPrefs.GetString("ChallengeInfo_Title_Cache", string.Empty))
        {
            currentInfo_cache = PlayerPrefs.GetString("ChallengeInfo_Title_Cache", string.Empty);
            addtional_mode = Resources.Load<MarathonAdditionalMode>("Database_Marathon/" + currentInfo_cache);

            if (PlayerPrefs.GetInt("MarathonPlay_DifficultyCount", 0) <= 0 || addtional_mode == null)
                PlayerPrefs.DeleteKey("MarathonPlay_DifficultyMode");
        }

        // Update changes to content
        UpdateDifficultyMode();
    }
    #endregion

    private void UpdateDifficultyMode()
    {
        int currentState = PlayerPrefs.GetInt("MarathonPlay_DifficultyMode", 1);

        Difficulty_Display.text = difficulty_mode[currentState];
        NavigatorButton[0].interactable = addtional_mode != null && currentState > 0;
        NavigatorButton[1].interactable = addtional_mode != null && currentState < difficulty_mode.Length - 1;

        // Update changes to content
        if (addtional_mode != null)
            MarathonSelection_Script.thisMarathon.ReloadChallengeInfo(addtional_mode, currentState);
    }

    private bool CheckSupportPass()
    {
        if (PlayerPrefs.GetInt("MarathonPlay_DifficultyCount", 0) > 0)
            return true;

        else
        {
            const string entryPanel = "Pass_Entry_Panel";

            if (GameObject.Find(entryPanel) == null) 
                StartCoroutine(SupportPassEntry(entryPanel, "A support pass is required for modify difficulty mode"));

            return false;
        }
    }

    private IEnumerator SupportPassEntry(string nameOfPanel, string describePanelDoing)
    {
        GameObject instance_panel = Instantiate(DisplayPanel);
        instance_panel.name = nameOfPanel;
        instance_panel.GetComponent<VirtualStorageBag>().SetDefaultDescription(describePanelDoing);
        PlayerPrefs.SetString(VirtualStorageBag.VirtualStorage_UsableKey, "TRUE");

        Task<List<UsageOfItemDetail>> isItemUsageReady = PreLoadingFilteredItem("Filtered_Items/Marathon_Pass");
        yield return new WaitUntil(() => isItemUsageReady.IsCompleted);

        Task<List<VirtualItemDatabase>> isItemReadyForUse = PerformFilteredItem(isItemUsageReady.Result.ToArray());
        yield return new WaitUntil(() => isItemReadyForUse.IsCompleted);

        instance_panel.GetComponent<VirtualStorageBag>().SetAlertPopReference(PromptMessage);
        instance_panel.GetComponent<VirtualStorageBag>().SetLimitedUsageTime(true);
        instance_panel.transform.SetParent(MainPanel.transform);

        instance_panel.GetComponent<VirtualStorageBag>().SetItemForDisplay(isItemReadyForUse.Result.ToArray());
        instance_panel.transform.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
    }

    #region MISC 
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
                isItemFound = Resources.LoadAsync("Database_Item/" + panelType + "/#" + currentLoadedIndex);
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
}
