using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Class that reacts to player click on item slot in the inventory
/// </summary>
public class UISlot : MonoBehaviour, IPointerClickHandler
{
    public TradeableItem item;
    public UISlot selectedItem { get; set; }

    protected InventoryManager inventoryManager;

    private void Start()
    {
        this.selectedItem = GameObject.Find("SelectedItem").GetComponent<UISlot>();
        this.inventoryManager = InventoryManager.Instance;
    }
    /// <summary>
    /// Given an item set this slot to contain that item
    /// </summary>
    public virtual void UpdateItem(TradeableItem item)
    {
        this.item = item;
        Image itemImage = transform.GetChild(0).GetComponent<Image>();
        if (this.item != null)
        {
            itemImage.sprite = item.sprite;
            itemImage.color = Color.white;
        }
        else
        {
            itemImage.color = Color.clear;
        }
    }

    /// <summary>
    /// update the slot based on the conditions that occuerd when it was clicked
    /// </summary>
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (this.item != null)
        {
            //if this slot already contains an item and the player has no selected item then this item becomes the selected item
            if (this.selectedItem.item == null)
            {
                selectedItem.UpdateItem(this.item);
                UpdateItem(null);
            }
            //otherwise if the player has a selected item we switch between this item and the selected item
            else
            {
                TradeableItem clone = new TradeableItem(selectedItem.item);
                selectedItem.UpdateItem(this.item);
                UpdateItem(clone);
            }
        }
        //if the player already selected an item and this slot is empty then we insert the selected item into this slot
        else
        {
            TradeableItem clone = new TradeableItem(selectedItem.item);
            selectedItem.UpdateItem(null);
            UpdateItem(clone);
        }
    }
}
