using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TradeSlot : UISlot
{
    [SerializeField] private GameObject tradeContent;
    private TradeManager tradeManager;
    private bool isInSellInventory = true;
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
        if (gameObject.tag.Equals("InContent"))
        {
           
            tradeManager.AddToTradeInventory(this.item);
            if (slotType == TradeManager.SlotType.sellSlot)
            {
                Destroy(this.gameObject);
            }
            
            
        }
        else
        {
            TradeableItem clone = new TradeableItem(this.item);
            UpdateItem(null);

            GameObject slot = Instantiate(this.gameObject, tradeContent.transform);
            slot.GetComponent<UISlot>().UpdateItem(clone);
            slot.tag = "InContent";
            RectTransform rt = slot.GetComponent<RectTransform>(); 
            rt.sizeDelta = tradeManager.TradeWindowItemSize;
            rt = slot.GetComponent<Transform>().GetChild(0).GetComponent<RectTransform>();
            rt.sizeDelta = tradeManager.TradeWindowItemSize;

            if (slotType == TradeManager.SlotType.sellSlot)
            {
                tradeManager.GetSellScreen().AddItem(slot);
            }


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
