using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellInventory : MonoBehaviour
{
    private InventoryManager inventoryManager;

    [SerializeField] private List<InventoryManager.InvnetorySlots> slots;
    // Start is called before the first frame update
    void Start()
    {
        this.inventoryManager = InventoryManager.Instance;
        List<InventoryManager.InvnetorySlots> inventorySlots = inventoryManager.GetInventorySlots();
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            for (int j = 0; j < inventorySlots[0].slots.Count; j++)
            {
                UISlot slot = inventorySlots[i].slots[j].GetComponent<UISlot>();
                if (slot.item != null)
                {
                    TradeableItem item = slot.item;
                    slots[i].slots[j].GetComponent<UISlot>().UpdateItem(item);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
