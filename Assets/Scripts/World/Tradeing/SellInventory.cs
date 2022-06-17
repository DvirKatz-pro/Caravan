using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellInventory : MonoBehaviour
{
    private InventoryManager inventoryManager;

    [SerializeField] private List<InventoryManager.InvnetorySlots> slots;
    [SerializeField] private TextMeshProUGUI fundsText; 
    // Start is called before the first frame update
    void Start()
    {
        
        this.inventoryManager = InventoryManager.Instance;

        fundsText.text = InventoryManager.FUNDS_TEXT + inventoryManager.GetCurrentFunds();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddToSellInventory(TradeableItem tradeableItem)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            for (int j = 0; j < slots[0].slots.Count; j++)
            {
                UISlot slot = slots[i].slots[j].GetComponent<UISlot>();

                if (slot.item == null)
                {
                    slots[i].slots[j].GetComponent<UISlot>().UpdateItem(tradeableItem);
                    j = slots[i].slots.Count;
                    i = slots.Count;
                }
            }
        }
    }
}
