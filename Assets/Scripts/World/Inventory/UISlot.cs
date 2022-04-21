using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISlot : MonoBehaviour, IPointerClickHandler
{
    public TradeableItem item;
    public UISlot selectedItem { get; set; }

    private void Start()
    {
        this.selectedItem = GameObject.Find("SelectedItem").GetComponent<UISlot>();
    }

    public void updateItem(TradeableItem item)
    {
        this.item = item;
        Image itemImage = transform.GetChild(0).GetComponent<Image>();
        if (this.item != null)
        {
            itemImage.sprite = item.getSprite();
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
            if (this.selectedItem.item != null)
            {
                TradeableItem clone = new TradeableItem(selectedItem.item);
                selectedItem.updateItem(this.item);
                updateItem(clone);
            }
            else
            {
                selectedItem.updateItem(this.item);
                updateItem(null);
                this.item = null;
            }
        }
        else if (this.selectedItem.item != null)
        {
            this.item = new TradeableItem(this.selectedItem.item);
        }
    }
}
