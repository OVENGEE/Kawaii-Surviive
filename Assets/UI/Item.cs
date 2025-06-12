using System;
using TMPro;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int ID;
    public string itemName;
    public int quantity = 1; // Default quantity is 1

    private TMP_Text quantityText; // Reference to the TextMeshPro component for displaying item information

    private void Awake()
    {
        quantityText = GetComponentInChildren<TMP_Text>(); // Get the TextMeshPro component in the children of this GameObject
        UpdateQuantityDisplay(); // Initialize the quantity text
    }

    public void UpdateQuantityDisplay()
    {
        if (quantityText != null)
        {
            quantityText.text = quantity > 1 ? quantity.ToString() : ""; // Display quantity if greater than 1, otherwise show nothing  
        }
    }

    public void AddToStack(int amount = 1)
    {
        quantity += amount; // Increase the quantity by the specified amount
        UpdateQuantityDisplay(); // Update the display to reflect the new quantity
    }

    public int RemoveFromStack(int amount = 1)
    {
        int removed = Mathf.Min(amount, quantity); // Determine how much to remove, ensuring it doesn't exceed the current quantity
        quantity -= removed; // Decrease the quantity by the removed amount
        UpdateQuantityDisplay(); // Update the display to reflect the new quantity
        return removed; // Return the amount that was removed
    }

    public GameObject CloneItem(int newQuantity)
    {
        GameObject clone = Instantiate(gameObject); // Create a clone of the current item GameObject
        Item cloneItem = clone.GetComponent<Item>(); // Get the Item component from the cloned GameObject
        cloneItem.quantity = newQuantity; // Set the quantity of the cloned item
        cloneItem.UpdateQuantityDisplay(); // Update the display for the cloned item
        return clone; // Return the cloned GameObject
    }

}
