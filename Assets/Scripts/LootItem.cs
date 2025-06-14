using UnityEngine;
[System.Serializable]
public class LootItem
{
    public GameObject itemPrefab; // The prefab of the item to be dropped
    [Range(0,100)]public float dropChance; // The chance of this item being dropped (0 to 1)
}
