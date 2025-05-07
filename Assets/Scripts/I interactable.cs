public interface Iinteractable // This interface is used to define the contract for interactable objects in the game.
{
    void Interact();// This method is called when the player interacts with the object.
    bool CanInteract();// This method checks if the player can interact with the object.
}
