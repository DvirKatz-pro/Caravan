using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TradeManager : SingletonManager<TradeManager>
{
    private InventoryManager inventoryManager;

    [SerializeField] private List<InventoryManager.InvnetorySlots> sellslots;
    [SerializeField] private TextMeshProUGUI fundsText;
    [SerializeField] private SellScreen sellScreen;
    public Vector2 TradeWindowItemSize = Vector2.one;


    public enum SlotType
    {
        sellSlot,
        buySlot
    }
    // Start is called before the first frame update
    void Start()
    {

        this.inventoryManager = InventoryManager.Instance;
        List<InventoryManager.InvnetorySlots> slots = inventoryManager.GetInventorySlots();
        for (int i = 0; i < slots.Count; i++)
        {
            for (int j = 0; j < slots[0].slots.Count; j++)
            {
                UISlot slot = slots[i].slots[j].GetComponent<UISlot>();

                if (slot.item != null)
                {
                    sellslots[i].slots[j].GetComponent<UISlot>().UpdateItem(slot.item);
                    sellslots[i].slots[j].GetComponent<TradeSlot>().SetSlotType(SlotType.sellSlot);
                }
            }
        }

        fundsText.text = InventoryManager.FUNDS_TEXT + inventoryManager.GetCurrentFunds();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void AddToTradeInventory(TradeableItem tradeableItem)
    {
        for (int i = 0; i < sellslots.Count; i++)
        {
            for (int j = 0; j < sellslots[0].slots.Count; j++)
            {
                UISlot slot = sellslots[i].slots[j].GetComponent<UISlot>();

                if (slot.item == null)
                {
                    sellslots[i].slots[j].GetComponent<UISlot>().UpdateItem(tradeableItem);
                    j = sellslots[i].slots.Count;
                    i = sellslots.Count;
                }
            }
        }
    }
    public SellScreen GetSellScreen()
    {
        return sellScreen;
    }
}
