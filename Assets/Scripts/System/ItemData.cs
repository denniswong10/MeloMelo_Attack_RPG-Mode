using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName="ItemData", menuName="Item_Database")]
public class ItemData : ScriptableObject
{
    [Header("General")]
    public string ItemName;
    public Texture Icon;
    public enum ItemType { Item, Consumable, None };
    public ItemType thisItemType;
    [TextArea] public string description;
    public ushort ItemValue;

    [Header("In-Store Purchase")]
    public bool NoQuantityLimit;
    public ushort CreditSet;
}
