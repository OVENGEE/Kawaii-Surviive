using System;
using UnityEngine;

[System.Serializable]// <summary>
// This class is used to save the inventory data for a player.
public class InventorySaveData
{
    public int itemID;// the ID of the item
    public int slotIndex; // the index of the slot where the item is placed within our inventory
    public int quantity = 1; // the quantity of the item in the slot
}
