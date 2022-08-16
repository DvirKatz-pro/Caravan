using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Class Detailing How a UI slot behaves in the trade screen
/// </summary>
public class TradeSlot : UISlot
{
    [SerializeField] private GameObject tradeContent;
    private TradeManager tradeManager;
    private TradeManager.SlotType slotType;
    // Start is called before the first frame update
    void Start()
    {
        tradeManager = TradeManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPointerClick(PointerEventData eventData) {
        //check if this slot is in the buy or sell screen
        if (gameObject.tag.Equals("SellScreen") || gameObject.tag.Equals("BuyScreen"))
        {
            //if we are clicked in the sell screen return this item to the Player inventory
            if (gameObject.tag.Equals("SellScreen"))
            {
                tradeManager.AddToSellInventory(this.item);
                tradeManager.GetSellScreen().RemoveItem(this.gameObject);
                tradeManager.ChangeTradeBalance(-this.item.basePrice);
            }
            //otherwise return to the NPC inventory
            else
            {
                tradeManager.AddToBuyInventory(this.item);
                tradeManager.GetBuyScreen().RemoveItem(this.gameObject);
                tradeManager.ChangeTradeBalance(this.item.basePrice);
            }
            
            Destroy(this.gameObject);

        }
        //if this slot is not in the sell/buy screen then add it to the appropriate screen
        else
        { 

            GameObject slot = Instantiate(this.gameObject, tradeContent.transform);
            slot.GetComponent<UISlot>().UpdateItem(this.item);
            
            RectTransform rt = slot.GetComponent<RectTransform>(); 
            rt.sizeDelta = tradeManager.TradeWindowItemSize;
            rt = slot.GetComponent<Transform>().GetChild(0).GetComponent<RectTransform>();
            rt.sizeDelta = tradeManager.TradeWindowItemSize;

            if (slotType == TradeManager.SlotType.sellSlot)
            {
                tradeManager.GetSellScreen().AddItem(slot);
                slot.tag = "SellScreen";
                tradeManager.ChangeTradeBalance(this.item.basePrice);
            }
            else
            {
                tradeManager.GetBuyScreen().AddItem(slot);
                slot.tag = "BuyScreen";
                tradeManager.ChangeTradeBalance(-this.item.basePrice);
            }

            UpdateItem(null);
        }

    }
    public void SetSlotType(TradeManager.SlotType slotType)
    {
        this.slotType = slotType;
    }
    public TradeManager.SlotType GetSlotType()
    {
        return slotType;
    }
}
