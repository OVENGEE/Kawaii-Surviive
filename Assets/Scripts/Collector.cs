using UnityEngine;

public class Collector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
       IItem item = collision.GetComponent<IItem>();
        if (item != null) // Check if the collided object implements IItem
        {
            item.Collect(); // Call the collect method on the item
        } 
    }
}
