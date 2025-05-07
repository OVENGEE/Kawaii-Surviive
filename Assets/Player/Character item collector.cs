using UnityEngine;

public class Characteritemcollector : MonoBehaviour
{
    private Inventorycontroller inventorycontroller;//reference to the inventory controller
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventorycontroller = FindAnyObjectByType<Inventorycontroller>();//find the inventory controller in the scene
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Item"))//if we walked into the item
        {
            Item item = collision.GetComponent<Item>();//get the item component from the item
            if(item != null)//if the item is not null
            {
                bool itemAdded  = inventorycontroller.AddItem(collision.gameObject);//add the item to the inventory

                if(itemAdded)//if the item was added to the inventory
                {
                    Destroy(collision.gameObject);//remove the item from the world
                }
            }
        }
    }
}
