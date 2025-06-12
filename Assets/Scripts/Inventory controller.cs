//Title: Unity Script Reference – Object.Instantiate
//Author: Unity Technologies
//Date: 21 April 2025
//Code Version: 6000.1
//Availability: https://docs.unity3d.com/ScriptReference/Object.Instantiate.html

//Title: Unity Script Reference – Transform.GetSiblingIndex
//Author: Unity Technologies
//Date: 21 April 2025
//Code Version: 6000.1
//Availability: https://docs.unity3d.com/ScriptReference/Transform.GetSiblingIndex.html


using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class Inventorycontroller : MonoBehaviour
{
    private ItemDictionary itemDictionary;//Reference to the item dictionary
    public GameObject inventoryPanel;//Reference to the inventory panel in the UI
    public GameObject slotPrefab;//Reference to the prefab for the inventory slots
    public int slotCount;//Number of slots in the inventory
    public GameObject[] itemPrefabs;//Array of item prefabs to be used in the inventory

    public static Inventorycontroller Instance { get; private set; } // Singleton instance of the Inventorycontroller
    Dictionary<int, int> itemsCountCache = new();
    public event Action OnInventoryChanged; // Event to notify when the inventory changes, notify the quest system


    private void Awake()
    {
        if (Instance != null && Instance != this) // Check if the instance is not already set
        {
            Destroy(gameObject); // Destroy this object if an instance already exists
            return; // Exit the Awake method
        }

        Instance = this; // Set the singleton instance to this object
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemDictionary = FindAnyObjectByType<ItemDictionary>();//Find the item dictionary in the scene

        RebuildItemCounts(); // Rebuild the item counts cache at the start


        for(int i = 0; i < slotCount ; i++)//Initialize slots
           {    //storing the slot game object
                SLOT slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<SLOT>();
                if( i < itemPrefabs.Length) //Do we have an item that can fit into this slot
                {
                    GameObject item = Instantiate(itemPrefabs[i], slot.transform); //Put the item in the slot
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; //Makes item be centered within the slot
                    slot.currentItem = item;
                }
           } 
    }

    public void RebuildItemCounts()
    {
        itemsCountCache.Clear(); // Clear the cache before rebuilding it 

        foreach (Transform slotTransform in inventoryPanel.transform) // Iterate through each slot in the inventory panel
        {
            SLOT slot = slotTransform.GetComponent<SLOT>(); // Get the SLOT component from the slot transform
            if (slot.currentItem != null) // Check if the slot has an item
            {
                Item item = slot.currentItem.GetComponent<Item>(); // Get the Item component from the current item
                if (item != null) // Check if the item is not null
                {
                    itemsCountCache[item.ID] = itemsCountCache.GetValueOrDefault(item.ID, 0) + item.quantity; // Update the item count in the cache
                }
            }
        }

        OnInventoryChanged?.Invoke(); // Invoke the event to notify that the inventory has changed
    }

    public Dictionary<int, int> GetItemCounts() => itemsCountCache; // Return the cache containing item IDs and their counts

    public bool AddItem(GameObject itemPrefab)//Method to add the item to the inventory
    {
        Item itemToAdd = itemPrefab.GetComponent<Item>();//Get the Item component from the item prefab
        if (itemToAdd == null) return false;//Return false to indicate that the item could not be added

        //Check if the item already exists in the inventory
        foreach (Transform slotTranform in inventoryPanel.transform)//Iterate through each slot in the inventory panel
        {
            SLOT slot = slotTranform.GetComponent<SLOT>();//Get the SLOT component from the slot transform
            if (slot != null && slot.currentItem != null)//Check if the slot is empty
            {
                Item slotItem = slot.currentItem.GetComponent<Item>();//Get the Item component from the current item in the slot
                if (slotItem != null && slotItem.ID == itemToAdd.ID)//Check if the item in the slot matches the item to add
                {
                    //Same item found, add to stack
                    slotItem.AddToStack();//Add the quantity of the item to the existing stack
                    RebuildItemCounts(); // Rebuild the item counts cache after adding to the stack
                    return true;//Return true to indicate that the item was successfully added
                }
            }
        }

        //Look for an empty slot
        foreach (Transform slotTranform in inventoryPanel.transform)//Iterate through each slot in the inventory panel
        {
            SLOT slot = slotTranform.GetComponent<SLOT>();//Get the SLOT component from the slot transform
            if (slot != null && slot.currentItem == null)//Check if the slot is empty
            {
                GameObject newItem = Instantiate(itemPrefab, slotTranform);//Instantiate the item prefab in the slot transform
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;//Center the item within the slot
                slot.currentItem = newItem;//Assign the new item to the slot's current item
                RebuildItemCounts();
                return true;//Return true to indicate that the item was added successfully
            }
        }

        Debug.Log("Inventory is Full!");//Log a message if no empty slot was found
        return false;//Return false to indicate that the item could not be added
    }
    public List<InventorySaveData> GetInventoryItems()//Method to get the items in the inventory
    {
        List<InventorySaveData> invData = new List<InventorySaveData>();//Create a list to store the inventory data
        foreach (Transform slotTranform in inventoryPanel.transform)//Iterate through each slot in the inventory panel
        {
            SLOT slot = slotTranform.GetComponent<SLOT>();//Get the SLOT component from the slot transform
            if (slot.currentItem != null)//Check if the slot has an item
            {
                Item item = slot.currentItem.GetComponent<Item>();//Get the Item component from the current item
                invData.Add(new InventorySaveData
                {
                    itemID = item.ID,
                    slotIndex = slotTranform.GetSiblingIndex(),
                    quantity = item.quantity
                });//Add the item ID and slot index to the inventory data list
            }
        }
        return invData;//Return the list of inventory data
    }
    public void SetInventoryItems(List<InventorySaveData> inventorySaveData)//Method to set the items in the inventory
    {
        //clear inventory panel - avoid duplicates 
        foreach (Transform child in inventoryPanel.transform)//Iterate through each child of the inventory panel
        {
            Destroy(child.gameObject);//Destroy the child game object to clear the inventory panel
        }

        //create new slots
        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, inventoryPanel.transform);//Instantiate a new slot prefab in the inventory panel
        }

        //Populate slots with saved items
        foreach (InventorySaveData data in inventorySaveData)//Iterate through each item in the saved inventory data
        {
            if (data.slotIndex < slotCount)//Check if the slot index is within the valid range
            {
                SLOT slot = inventoryPanel.transform.GetChild(data.slotIndex).GetComponent<SLOT>();//Get the SLOT component from the slot transform at the specified index
                GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemID);//Get the item prefab from the item dictionary using the item ID
                if (itemPrefab != null)//Check if the item prefab is not null
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);//Instantiate the item prefab in the slot transform
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;//Center the item within the slot

                    Item itemComponent = item.GetComponent<Item>();//Get the Item component from the instantiated item
                    if (itemComponent != null && data.quantity > 1)//Check if the Item component is not null
                    {
                        itemComponent.quantity = data.quantity;//Set the quantity of the item to the saved quantity
                        itemComponent.UpdateQuantityDisplay();//Update the display of the item's quantity
                    }

                    slot.currentItem = item;//Assign the new item to the slot's current item
                }
                else
                {
                    Debug.LogWarning($"Item with ID {data.itemID} not found in the item dictionary.");//Log a warning if the item prefab is not found in the item dictionary
                }
            }
        }
        RebuildItemCounts(); // Rebuild the item counts cache after setting the inventory items
    }

    public void RemoveItemsFromInventory(int itemID, int amountToRemove)
    {
        foreach (Transform slotTransform in inventoryPanel.transform) // Iterate through each slot in the inventory panel
        {
            if (amountToRemove <= 0) break; // Exit if no more items need to be removed

            SLOT slot = slotTransform.GetComponent<SLOT>(); // Get the SLOT component from the slot transform
            if (slot?.currentItem?.GetComponent<Item>() is Item item && item.ID == itemID) // Check if the slot has an item and if it matches the item ID
            {
                int removed = Math.Min(amountToRemove, item.quantity); // Determine how much to remove, ensuring it doesn't exceed the current quantity
                item.RemoveFromStack(removed); // Remove the specified amount from the item's stack
                amountToRemove -= removed; // Decrease the amount to remove by the removed amount

                if (item.quantity == 0) // If the item's quantity is zero or less
                {
                    Destroy(slot.currentItem); // Destroy the item GameObject
                    slot.currentItem = null; // Set the current item in the slot to null
                }
            }
        }
        RebuildItemCounts(); // Rebuild the item counts cache after removing items
    }  
}
