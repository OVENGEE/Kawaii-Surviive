using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using Vector2 = UnityEngine.Vector2;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform originalParent; //Slot the item is coming from
    CanvasGroup canvasGroup;
    public float minDropDistance = 0.2f;
    public float maxDropDistance = 0.5f;
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
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
        if(dropSlot == null)
        {
            GameObject dropItem = eventData.pointerEnter;
            if(dropItem != null)
            {
                dropSlot = dropItem.GetComponentInParent<SLOT>();
            }
        }
        SLOT originalSlot = originalParent.GetComponent<SLOT>();

        if(dropSlot != null)
        {   
            //Is a slot under drag point
            if(dropSlot.currentItem != null)
            { //Slot has an item, swap them around
                dropSlot.currentItem.transform.SetParent(originalSlot.transform);
                originalSlot.currentItem = dropSlot.currentItem;
                dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else
            {
                originalSlot.currentItem = null;
            }
            
            //Move item into drop slot
            transform.SetParent(dropSlot.transform);
            dropSlot.currentItem = gameObject;
        }
        else
        {
            //If where we're dropping is not within the inventory
            if(!IsWithinInventory(eventData.position))
            {
                //Drop our item
                DropItem(originalSlot);
            }

            //else
            else
            {
                //Snap back on og slot
                 transform.SetParent(originalParent);
            }
        }
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero; //center
    }
    bool IsWithinInventory(Vector2 mousePosition)
    {
        RectTransform inventoryRect = originalParent.parent.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, mousePosition);
    }

    void DropItem(SLOT originalSlot)
    {
        originalSlot.currentItem = null;

        //Find player
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if(playerTransform == null)
        {
            Debug.LogError("Missing 'Player' tag");
            return;
        }
        //Random Drop position
        Vector2 dropOffset = Random.insideUnitCircle.normalized * Random.Range(minDropDistance, maxDropDistance);
        Vector2 dropPosition = (Vector2)playerTransform.position + dropOffset;

        //Instantiate drop item
        Instantiate(gameObject, dropPosition, UnityEngine.Quaternion.identity);

        //Destroy the UI one
        Destroy(gameObject);

    }
}
