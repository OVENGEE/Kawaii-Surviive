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


using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventorycontroller : MonoBehaviour
{
    private ItemDictionary itemDictionary;//Reference to the item dictionary
    public GameObject inventoryPanel;//Reference to the inventory panel in the UI
    public GameObject slotPrefab;//Reference to the prefab for the inventory slots
    public int slotCount;//Number of slots in the inventory
    public GameObject[] itemPrefabs;//Array of item prefabs to be used in the inventory

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemDictionary = FindAnyObjectByType<ItemDictionary>();//Find the item dictionary in the scene
    
       /*for(int i = 0; i < slotCount ; i++)//Initialize slots
       {    //storing the slot game object
            SLOT slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<SLOT>();
            if( i < itemPrefabs.Length) //Do we have an item that can fit into this slot
            {
                GameObject item = Instantiate(itemPrefabs[i], slot.transform); //Put the item in the slot
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; //Makes item be centered within the slot
                slot.currentItem = item;
            }
       }*/ 
    }

    public bool AddItem(GameObject itemPrefab)//Method to add the item to the inventory
    {
        //Look for an empty slot
        foreach(Transform slotTranform in inventoryPanel.transform)//Iterate through each slot in the inventory panel
        {
            SLOT slot = slotTranform.GetComponent<SLOT>();//Get the SLOT component from the slot transform
            if(slot != null && slot.currentItem == null)//Check if the slot is empty
            {
                GameObject newItem = Instantiate(itemPrefab, slotTranform);//Instantiate the item prefab in the slot transform
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;//Center the item within the slot
                slot.currentItem = newItem;//Assign the new item to the slot's current item
                return true;//Return true to indicate that the item was added successfully
            }
        }

        Debug.Log("Inventory is Full!");//Log a message if no empty slot was found
        return false;//Return false to indicate that the item could not be added
    }
    public List<Inventorysavedata> GetInventoryItems()//Method to get the items in the inventory
    {
        List<Inventorysavedata> invData = new List<Inventorysavedata>();//Create a list to store the inventory data
        foreach(Transform slotTranform in inventoryPanel.transform)//Iterate through each slot in the inventory panel
        {
            SLOT slot = slotTranform.GetComponent<SLOT>();//Get the SLOT component from the slot transform
            if(slot.currentItem != null)//Check if the slot has an item
            {
                Item item = slot.currentItem.GetComponent<Item>();//Get the Item component from the current item
                invData.Add(new Inventorysavedata { itemID = item.ID, slotIndex = slotTranform.GetSiblingIndex()});//Add the item ID and slot index to the inventory data list
            }
        }
        return invData;//Return the list of inventory data
    }
   public void SetInventoryItems(List<Inventorysavedata> inventorysavedata)//Method to set the items in the inventory
   {
        //clear inventory panel - avoid duplicates 
        foreach(Transform child in inventoryPanel.transform)//Iterate through each child of the inventory panel
        {
            Destroy(child.gameObject);//Destroy the child game object to clear the inventory panel
        }

        //create new slots
        for(int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, inventoryPanel.transform);//Instantiate a new slot prefab in the inventory panel
        }

        //Populate slots with saved items
        foreach(Inventorysavedata data in inventorysavedata)//Iterate through each item in the saved inventory data
        {
            if(data.slotIndex < slotCount)//Check if the slot index is within the valid range
            {
                SLOT slot = inventoryPanel.transform.GetChild(data.slotIndex).GetComponent<SLOT>();//Get the SLOT component from the slot transform at the specified index
                GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemID);//Get the item prefab from the item dictionary using the item ID
                if (itemPrefab != null)//Check if the item prefab is not null
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);//Instantiate the item prefab in the slot transform
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;//Center the item within the slot
                    slot.currentItem = item;//Assign the new item to the slot's current item
                }
                else
                {
                    Debug.LogWarning($"Item with ID {data.itemID} not found in the item dictionary.");//Log a warning if the item prefab is not found in the item dictionary
                }
            }
        }
   }
}
