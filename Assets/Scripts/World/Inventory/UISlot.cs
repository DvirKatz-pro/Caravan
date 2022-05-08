using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Class that reacts to player click on item slot
/// </summary>
public class UISlot : MonoBehaviour, IPointerClickHandler
{
    public TradeableItem item;
    public UISlot selectedItem { get; set; }

    private InventoryManager inventoryManager;

    private void Start()
    {
        this.selectedItem = GameObject.Find("SelectedItem").GetComponent<UISlot>();
        this.inventoryManager = GameObject.Find("Inventory").GetComponent<InventoryManager>();
    }
  
    public void UpdateItem(TradeableItem item)
    {
        this.item = item;
        Image itemImage = transform.GetChild(0).GetComponent<Image>();
        if (this.item != null)
        {
            itemImage.sprite = item.GetSprite();
            itemImage.color = Color.white;
        }
        else
        {
            itemImage.color = Color.clear;
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.item != null)
        {
            if (this.selectedItem.item == null)
            {
                selectedItem.UpdateItem(this.item);
                UpdateItem(null);
            }
            else
            {
                TradeableItem clone = new TradeableItem(selectedItem.item);
                selectedItem.UpdateItem(this.item);
                UpdateItem(clone);
            }
        }
        else
        {
            TradeableItem clone = new TradeableItem(selectedItem.item);
            selectedItem.UpdateItem(null);
            UpdateItem(clone);
        }
    }
}
