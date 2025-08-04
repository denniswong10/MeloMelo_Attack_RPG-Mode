using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName="ItemData", menuName="Item_Database")]
public class ItemData : ScriptableObject
{
    [Header("General")]
    public string itemName;
    public Texture Icon;
    public bool stackable;

    public enum ItemType { Item, Consumable, Artifact, None };
    public ItemType thisItemType;
    [TextArea] public string description;
    public ushort itemValue;

    [Header("In-Store Purchase")]
    public bool NoQuantityLimit;
    public ushort CreditSet;
}
