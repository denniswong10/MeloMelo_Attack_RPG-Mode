using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;

public class FragmentPanel_Script : MonoBehaviour
{
    [SerializeField] private GameObject virtualStorageReference;
    [SerializeField] private Button openButtonInteraction;

    void Start()
    {
        transform.GetChild(2).GetComponent<Text>().text = "Fragment Usage: " + PlayerPrefs.GetInt("Progress_Fragement", 0);
        transform.GetChild(3).GetComponent<Text>().text = "Fragment in Storage: " + MeloMelo_ItemUsage_Settings.GetActiveItem("Map Fragment").amount;
    }

    public void OpenRemoteItemSubmit()
    {
        openButtonInteraction.interactable = false;
        StartCoroutine(OpenSubmissionPanel());
    }

    public void ClosePanel()
    {
        Destroy(gameObject);
    }

    private IEnumerator OpenSubmissionPanel()
    {
        if (!virtualStorageReference.activeInHierarchy)
        {
            Task<List<UsageOfItemDetail>> allFilteredItem = PreLoadingFilteredItem("Material_Section");
            yield return new WaitUntil(() => allFilteredItem.IsCompleted);

            Task<List<VirtualItemDatabase>> isFilteredPerformed = PerformFilteredItem(allFilteredItem.Result.ToArray());
            yield return new WaitUntil(() => isFilteredPerformed.IsCompleted);

            virtualStorageReference.SetActive(true);
            virtualStorageReference.GetComponent<VirtualStorageBag>().SetAlertPopReference(null);
            virtualStorageReference.GetComponent<VirtualStorageBag>().SetDefaultDescription("Select a item to complete your submission");
            virtualStorageReference.GetComponent<VirtualStorageBag>().SetItemForDisplay(isFilteredPerformed.Result.ToArray());
            virtualStorageReference.GetComponent<VirtualStorageBag>().SetLimitedUsageTime(false);
            PlayerPrefs.SetString(VirtualStorageBag.VirtualStorage_UsableKey, "TRUE");

            openButtonInteraction.interactable = true;
        }
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
}
