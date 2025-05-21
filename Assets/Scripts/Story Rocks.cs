

using UnityEngine;

public class StoryRocks : MonoBehaviour, Iinteractable //This class implements the Iinteractable interface, which allows the player to interact with the story rock
{
    public bool IsOpened { get; private set; }//This property indicates whether the story rock has been opened or not
    public string StoryRocksID { get; private set; }//This property stores a unique ID for the story rock, which is generated when the script starts
    public GameObject itemPrefab; //Item that story rock drops
    public Sprite openedSprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }


    void Start()
    {
        StoryRocksID ??= GlobalHelperClass.GenerateUniqueID(gameObject);//This line generates a unique ID for the story rock if it hasn't been set already
    }

    public bool CanInteract()//This method checks if the player can interact with the story rock
    {
        return !IsOpened;//This line returns true if the story rock has not been opened yet, allowing the player to interact with it
    }

    public void Interact()//This method is called when the player interacts with the story rock
    {
        if (!CanInteract()) return;//This line checks if the player can interact with the story rock, and if not, it exits the method
        OpenChest();
        //Open story rock
    }

    private void OpenChest()//This method handles the openig of the story rock
    {
        SetOpened(true);//This line sets the story rock to opened, changing its appearance

        audioManager.PlaySFX(audioManager.ChestOpen);
        //Drop item
        if (itemPrefab)//This line checks if the itemPrefab has been set, and if so, it instantiates a new GameObject at the story rock's position
        {
            GameObject droppeditem = Instantiate(itemPrefab, transform.position + Vector3.down * 0.3f, Quaternion.identity);//This line creates a new GameObject at the story rock's position, slightly lower to avoid overlap
        }
    }

    public void SetOpened(bool opened)
    {
        if (IsOpened = opened)
        {
            GetComponent<SpriteRenderer>().sprite = openedSprite;
        }
    }

}
