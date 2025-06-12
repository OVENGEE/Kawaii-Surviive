using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using Vector2 = UnityEngine.Vector2;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    Transform originalParent; //Slot the item is coming from
    CanvasGroup canvasGroup;
    public float minDropDistance = 0.2f;
    public float maxDropDistance = 0.5f;

    private Inventorycontroller inventorycontroller;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        inventorycontroller = Inventorycontroller.Instance; // Get the instance of Inventorycontroller
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent; //Save old parent
        transform.SetParent(transform.root); //Make sure its above any other canvases
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f; //semi-transparent during drag
    }

    //function to follow our mouse as we move
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position; //Follow mouse
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true; //enables raycasts
        canvasGroup.alpha = 1f; //No longer transparent

        SLOT dropSlot = eventData.pointerEnter?.GetComponent<SLOT>(); //Slot where item dropped
        if (dropSlot == null)
        {
            GameObject dropItem = eventData.pointerEnter;
            if (dropItem != null)
            {
                dropSlot = dropItem.GetComponentInParent<SLOT>();
            }
        }
        SLOT originalSlot = originalParent.GetComponent<SLOT>();

        if (dropSlot != null)
        {
            //Is a slot under drag point
            if (dropSlot.currentItem != null)
            {
                Item draggedItem = GetComponent<Item>();
                Item targetItem = dropSlot.currentItem.GetComponent<Item>();

                if (draggedItem.ID == targetItem.ID)
                {
                    targetItem.AddToStack(draggedItem.quantity); //If same item, add to stack
                    originalSlot.currentItem = null; //Clear original slot
                    Destroy(gameObject); //Destroy the dragged item
                }
                else
                {
                    //Slot has an item, swap them around
                    dropSlot.currentItem.transform.SetParent(originalSlot.transform);
                    originalSlot.currentItem = dropSlot.currentItem;
                    dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                    transform.SetParent(dropSlot.transform);
                    dropSlot.currentItem = gameObject;
                    GetComponent<RectTransform>().anchoredPosition = Vector2.zero; //center

                }
            }
            else
            {
                originalSlot.currentItem = null;
                transform.SetParent(dropSlot.transform);
                dropSlot.currentItem = gameObject;
                GetComponent<RectTransform>().anchoredPosition = Vector2.zero; //center

            }
        }
        else
        {
            //If where we're dropping is not within the inventory
            if (!IsWithinInventory(eventData.position))
            {
                //Drop our item
                DropItem(originalSlot);
            }

            //else
            else
            {
                //Snap back on og slot
                transform.SetParent(originalParent);
                GetComponent<RectTransform>().anchoredPosition = Vector2.zero; //center
            }
        }
    }
    bool IsWithinInventory(Vector2 mousePosition)
    {
        RectTransform inventoryRect = originalParent.parent.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, mousePosition);
    }

    void DropItem(SLOT originalSlot)
    {
        Item item = GetComponent<Item>();
        int quantity = item.quantity;

        if (quantity > 1)
        {
            item.RemoveFromStack();

            transform.SetParent(originalParent);
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero; //center

            quantity = 1; //Drop only one item
        }
        else
        {
            originalSlot.currentItem = null;
        }


        //Find player
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (playerTransform == null)
        {
            Debug.LogError("Missing 'Player' tag");
            return;
        }
        //Random Drop position
        Vector2 dropOffset = Random.insideUnitCircle.normalized * Random.Range(minDropDistance, maxDropDistance);
        Vector2 dropPosition = (Vector2)playerTransform.position + dropOffset;

        //Instantiate drop item
        GameObject dropItem = Instantiate(gameObject, dropPosition, UnityEngine.Quaternion.identity);
        Item droppedItem = dropItem.GetComponent<Item>();
        droppedItem.quantity = 1; //Set the quantity of the dropped item

        //Destroy the UI one
        if (quantity <= 1 && originalSlot.currentItem == null)
        {
            Destroy(gameObject);
        }

        Inventorycontroller.Instance.RebuildItemCounts();

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            //SplitStack
            SplitStack(); //Split the stack if right-clicked
        }
    }

    private void SplitStack()
    {
        Item item = GetComponent<Item>();
        if (item == null || item.quantity <= 1) return; //Cannot split if no item or quantity is 1

        int splitAmount = item.quantity / 2; //Split the stack in half
        if (splitAmount <= 0) return; //Cannot split if the amount is less than 1

        item.RemoveFromStack(splitAmount); //Remove the split amount from the original item

        GameObject newItem = item.CloneItem(splitAmount); //Clone the item with the split amount

        if (inventorycontroller == null || newItem == null) return;

        foreach (Transform slotTransform in inventorycontroller.inventoryPanel.transform)
        {
            SLOT slot = slotTransform.GetComponent<SLOT>();
            if (slot != null && slot.currentItem == null)
            {
                //Place the new item in the first empty slot
                slot.currentItem = newItem;
                newItem.transform.SetParent(slot.transform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; //Center the item in the slot
                return; //Exit after placing the item
            }
        }

        //No empty slot found, return to stack
        item.AddToStack(splitAmount); //Add the split amount back to the original item
        Destroy(newItem); //Destroy the cloned item since it couldn't be placed
    }
}
