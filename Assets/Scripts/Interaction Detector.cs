// This script is responsible for detecting interactable objects in the game world.
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    private Iinteractable interactableInRange = null; //Interactable object in range
    public GameObject interactionIcon;// UI element to show when an interactable object is in range
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactionIcon.SetActive(false);// Hide the interaction icon at the start
    }

    public void OnInteract(InputAction.CallbackContext context)// Method to handle interaction input
    // This method is called when the player presses the interact button
    {
        if(context.performed)//check if the button was pressed
        {
            interactableInRange?.Interact();// Call the Interact method on the interactable object if it is not null
            
            if(!interactableInRange.CanInteract())// If the interactable object cannot be interacted with
            {
                interactionIcon.SetActive(false);// Hide the interaction icon
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Iinteractable interactable) && interactable.CanInteract())// Check if the collided object has an Iinteractable component and can be interacted with
        {
            interactableInRange = interactable;// Set the interactable object in range to the collided object
            interactionIcon.SetActive(true);// Show the interaction icon
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Iinteractable interactable) && interactable == interactableInRange)// Check if the collided object has an Iinteractable component and is the current interactable object in range
        {
            interactableInRange = null;// Set the interactable object in range to null
            interactionIcon.SetActive(false);// Hide the interaction icon
        }
    }
}
