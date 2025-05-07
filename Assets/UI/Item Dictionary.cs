using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class ItemDictionary : MonoBehaviour
{
    public List<Item> itemPrefabs;
    private Dictionary<int, GameObject> itemDictonary;

    private void Awake()
    {
        itemDictonary = new Dictionary<int, GameObject>();

        //AutoIncrementIDs
        for(int i = 0; i < itemPrefabs.Count; i++)
        {
            if(itemPrefabs[i] != null)
            {
                itemPrefabs[i].ID = i + 1;
            }
        }
        foreach(Item item in itemPrefabs)
        {
            itemDictonary[item.ID] = item.gameObject;
        }
    }
    public GameObject GetItemPrefab(int itemID)
    {
        itemDictonary.TryGetValue(itemID, out GameObject prefab);
        if(prefab == null)
        {
            Debug.LogWarning($"Item with ID {itemID} not found in dictionary");
        }
        return prefab;
    }

}
