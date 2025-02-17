using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualStorageBag : MonoBehaviour
{
    public static readonly string VirtualStorage_UsableKey = "Character_VirtualItem_UsageOfItem";

    [SerializeField] private GameObject storagePanel;
    [SerializeField] private GameObject openChoiceSet;
    private bool limitedUseTime;
    private GameObject AlertPop;

    void Start()
    {
        ReAlignOfPanelPosition();
        PlayerPrefs.DeleteKey(name + "_ItemUsingBound");
    }

    #region SETUP
    private void ReAlignOfPanelPosition()
    {
        if (PlayerPrefs.HasKey(name + "_PositionStand"))
            GetComponent<RectTransform>().position = JsonUtility.FromJson<Vector3>(PlayerPrefs.GetString(name + "_PositionStand", string.Empty));
        else
            GetComponent<RectTransform>().position = Input.mousePosition;
    }

    public void SetDefaultDescription(string description)
    {
        transform.GetChild(2).GetComponent<Text>().text = description;
    }

    public void SetAlertPopReference(GameObject panel)
    {
        AlertPop = panel;
    }

    public void SetLimitedUsageTime(bool active)
    {
        limitedUseTime = active;
    }

    public void SetItemForDisplay(VirtualItemDatabase[] itemList)
    {
        if (itemList != null && itemList.Length > 0)
        {
            for (int itemId = 0; itemId < itemList.Length; itemId++)
            {
                for (int slot = 0; slot < storagePanel.transform.childCount - 1; slot++)
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
        else
            storagePanel.transform.GetChild(storagePanel.transform.childCount - 1).gameObject.SetActive(true);
    }
    #endregion

    #region MAIN
    // Toggle: IN
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

    // Toggle: OUT
    public void ToggleOutItemInfo()
    {
        if (AlertPop) AlertPop.SetActive(false);
    }

    // Clickable: Content
    public void UseStorageItem(int index)
    {
        if (PlayerPrefs.GetInt(name + "_ItemUsingBound", -1) == index)
            ActivationOfItemUsage(PlayerPrefs.GetString(index + "_" + name + "_Slot_ItemBound", string.Empty));       
        else
            PlayerPrefs.SetInt(name + "_ItemUsingBound", index);
    }

    // Dragable: Object
    public void DragPanel()
    {
        transform.position = new Vector3(Mathf.Clamp(Input.mousePosition.x, transform.localScale.x * 0.5f,
            Screen.width - (transform.localScale.x * 0.5f)),
                Mathf.Clamp(Input.mousePosition.y, transform.localScale.y * 0.5f,
                    Screen.height - (transform.localScale.y * 0.5f)), 0);

        string saveCurrentPosition = JsonUtility.ToJson(transform.position);
        PlayerPrefs.SetString(name + "_PositionStand", saveCurrentPosition);
    }

    // Object: Clear from scene
    public void ClosePanel()
    {
        for (int slot = 0; slot < storagePanel.transform.childCount; slot++)
            PlayerPrefs.DeleteKey(slot + "_" + name + "_Slot_ItemBound");

        AlertPop.SetActive(false);
        Destroy(gameObject);
    }
    #endregion

    #region COMPONENT (Item Control)
    private ItemData FindItemToPlot(string itemName)
    {
        foreach (ItemData item in Resources.LoadAll<ItemData>("Database_Item")) if (item.itemName == itemName) return item;
        return Resources.Load<ItemData>("Database_Item/#0");
    }

    private int GetTotalCountItem(string itemName)
    {
        // Item counted in storage bag and current used items
        int itemAmount = MeloMelo_GameSettings.GetAllItemFromLocal(itemName).amount -
            MeloMelo_ItemUsage_Settings.GetItemUsed(itemName);

        return itemAmount;
    }

    private void ItemNotAbleForUse(string customText)
    {
        if (AlertPop)
        {
            AlertPop.SetActive(true);
            AlertPop.transform.GetChild(0).GetComponent<Text>().text = customText;
            PlayerPrefs.DeleteKey(name + "_ItemUsingBound");
            Invoke("ToggleOutItemInfo", 2);
        }
    }
    #endregion

    #region COMPONENT (Activation Command)
    private void ActivationOfItemUsage(string itemToUsed)
    {
        VirtualItemDatabase itemFound = MeloMelo_GameSettings.GetAllItemFromLocal(itemToUsed);
        if (itemFound.itemName == itemToUsed && GetTotalCountItem(itemToUsed) > 0)
        {
            PlayerPrefs.DeleteKey(name + "_ItemUsingBound");
            string usedItemInDirectory = PlayerPrefs.GetString("ItemDirectoryToUsed", string.Empty);

            // Use of item and activation
            foreach (UsageOfItemDetail item in Resources.LoadAll<UsageOfItemDetail>
                ("Database_Item/" + usedItemInDirectory))
            {
                if (item.itemName == itemFound.itemName)
                {
                    switch (PlayerPrefs.GetString(VirtualStorage_UsableKey, "None"))
                    {
                        case "TRUE":
                        case "FALSE":
                            if (PlayerPrefs.GetString(VirtualStorage_UsableKey, "None") == "TRUE") 
                                PlayerItemUsage(item);
                            else
                            {
                                ItemNotAbleForUse("Item refuse to used this way");
                                return;
                            }
                            break;

                        default:
                            if (MeloMelo_CharacterInfo_Settings.GetCharacterStatus(
                                PlayerPrefs.GetString(VirtualStorage_UsableKey, "None").Split(",")[0])
                                )
                            {
                                if (PlayerPrefs.GetString(VirtualStorage_UsableKey, "None").Split(",")[2] == "0")
                                {
                                    ItemNotAbleForUse("Character had reached the max level");
                                    return;
                                }
                                else
                                    CharacterItemUsage(item);
                            }
                            else
                            {
                                ItemNotAbleForUse("Item cannot be used for this character");
                                return;
                            }
                            break;
                    }

                    if (limitedUseTime) ClosePanel();
                }
            }
        }
    }

    private void PlayerItemUsage(UsageOfItemDetail itemToUsed)
    {
        switch (itemToUsed.useTypeDetail)
        {
            case UsageOfItemDetail.UseType.Instant:
                int currentAmount = PlayerPrefs.GetInt(itemToUsed.dataArray.Split(",")[0], 0);

                PlayerPrefs.SetInt(itemToUsed.dataArray.Split(",")[0],
                currentAmount + int.Parse(itemToUsed.dataArray.Split(",")[1]));
                MeloMelo_ItemUsage_Settings.SetItemUsed(itemToUsed.itemName);
                break;

            default:
                break;
        }
    }

    private void CharacterItemUsage(UsageOfItemDetail itemUsage)
    {
        string[] splitUsage = PlayerPrefs.GetString(VirtualStorage_UsableKey, string.Empty).Split(",");

        switch (itemUsage.useTypeDetail)
        {
            case UsageOfItemDetail.UseType.Instant:
                MeloMelo_ItemUsage_Settings.SetItemUsed(itemUsage.itemName);

                if (splitUsage[1] == "1")
                {
                    // Stackable: Comsumable for one character
                    string onlyCharacter = splitUsage[0];
                    int currentAmount = PlayerPrefs.GetInt(onlyCharacter + itemUsage.dataArray.Split(",")[0], 0);

                    PlayerPrefs.SetInt(onlyCharacter + itemUsage.dataArray.Split(",")[0],
                        currentAmount + int.Parse(itemUsage.dataArray.Split(",")[1]));
                }
                else
                {
                    // Single Usage: Consumable for all characters 
                    for (int slot_id = 0; slot_id < 3; slot_id++)
                    {
                        string currentCharacter = PlayerPrefs.GetString("Slot" + (slot_id + 1) + "_charName", "None");
                        PlayerPrefs.SetInt(currentCharacter + itemUsage.dataArray.Split(",")[0],
                            int.Parse(itemUsage.dataArray.Split(",")[1]));
                    }
                }
                break;

            case UsageOfItemDetail.UseType.OpenChoice:
                if (GameObject.Find("OpenChoice_Setup") == null)
                {
                    GameObject instance = Instantiate(openChoiceSet, GameObject.Find("Selection_Character").transform);
                    instance.name = "OpenChoice_Setup";

                    int activationTicket = int.Parse(itemUsage.dataArray.Split("^")[1]);
                    instance.GetComponent<OpenChoiceSetupScript>().Setup(activationTicket, itemUsage, splitUsage[0]);
                    instance.GetComponent<OpenChoiceSetupScript>().SetupPromptMessage(AlertPop);
                }
                break;

            default:
                break;
        }
    }
    #endregion
}
