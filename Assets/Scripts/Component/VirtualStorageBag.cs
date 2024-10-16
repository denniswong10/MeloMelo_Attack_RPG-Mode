using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualStorageBag : MonoBehaviour
{
    [SerializeField] private GameObject storagePanel;
    private GameObject AlertPop;

    void Start()
    {
        ReAlignOfPanelPosition();
    }

    #region SETUP
    private void ReAlignOfPanelPosition()
    {
        if (PlayerPrefs.HasKey(name + "_PositionStand"))
            GetComponent<RectTransform>().position = JsonUtility.FromJson<Vector3>(PlayerPrefs.GetString(name + "_PositionStand", string.Empty));
        else
            GetComponent<RectTransform>().position = new Vector3(0, 0, 0);
    }

    public void SetDefaultDescription(string description)
    {
        transform.GetChild(2).GetComponent<Text>().text = description;
    }

    public void SetAlertPopReference(GameObject panel)
    {
        AlertPop = panel;
    }

    public void SetItemForDisplay(VirtualItemDatabase[] itemList)
    {
        for (int itemId = 0; itemId < itemList.Length; itemId++)
        {
            for (int slot = 0; slot < storagePanel.transform.childCount; slot++)
            {
                if (itemList[itemId].amount > 0 && !PlayerPrefs.HasKey(slot + "_" + name + "_Slot_ItemBound"))
                {
                    storagePanel.transform.GetChild(slot).gameObject.SetActive(true);
                    storagePanel.transform.GetChild(slot).GetComponent<RawImage>().texture = FindItemToPlot(itemList[itemId].itemName).Icon;
                    PlayerPrefs.SetString(slot + "_" + name + "_Slot_ItemBound", itemList[itemId].itemName);
                    break;
                }
            }
        }
    }
    #endregion

    #region MAIN
    public void ToggleItemInfo(int index)
    {
        if (AlertPop)
        {
            string itemFound = PlayerPrefs.GetString(index + "_" + name + "_Slot_ItemBound", string.Empty);
            AlertPop.SetActive(true);

            AlertPop.transform.GetChild(0).GetComponent<Text>().text = itemFound + 
                " ( x " + Mathf.Clamp(GetTotalCountItem(itemFound), 0, 9999) + " )";
        }
    }

    public void ToggleOutItemInfo()
    {
        if (AlertPop)
        {
            AlertPop.SetActive(false);
        }
    }

    public void DragPanel()
    {
        transform.position = new Vector3(Mathf.Clamp(Input.mousePosition.x, transform.localScale.x * 0.5f,
            Screen.width - (transform.localScale.x * 0.5f)),
                Mathf.Clamp(Input.mousePosition.y, transform.localScale.y * 0.5f,
                    Screen.height - (transform.localScale.y * 0.5f)), 0);

        string saveCurrentPosition = JsonUtility.ToJson(transform.position);
        PlayerPrefs.SetString(name + "_PositionStand", saveCurrentPosition);
    }

    public void ClosePanel()
    {
        for (int slot = 0; slot < storagePanel.transform.childCount; slot++)
            PlayerPrefs.DeleteKey(slot + "_" + name + "_Slot_ItemBound");

        Destroy(gameObject);
    }
    #endregion

    #region COMPONENT
    private ItemData FindItemToPlot(string itemName)
    {
        foreach (ItemData item in Resources.LoadAll<ItemData>("Database_Item")) if (item.ItemName == itemName) return item;
        return Resources.Load<ItemData>("Database_Item/#0");
    }

    private int GetTotalCountItem(string itemName)
    {
        // Item counted in storage bag and current used items
        int itemAmount = MeloMelo_GameSettings.GetAllItemFromLocal(itemName).amount -
            MeloMelo_ItemUsage_Settings.GetItemUsed(itemName);

        return itemAmount;
    }
    #endregion
}
