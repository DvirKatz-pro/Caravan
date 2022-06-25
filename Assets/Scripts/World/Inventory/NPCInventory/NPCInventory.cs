using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInventory : MonoBehaviour
{
    private List<TradeableItem> NPCStock;
    // Start is called before the first frame update
    void Start()
    {
        NPCStock = new List<TradeableItem>();
        InventoryManager inventoryManager = InventoryManager.Instance;
        NPCStock.Add(inventoryManager.GetItem("armor"));
        NPCStock.Add(inventoryManager.GetItem("armor"));
        NPCStock.Add(inventoryManager.GetItem("armor"));
        NPCStock.Add(inventoryManager.GetItem("armor"));
        NPCStock.Add(inventoryManager.GetItem("apple"));
        NPCStock.Add(inventoryManager.GetItem("apple"));
        NPCStock.Add(inventoryManager.GetItem("apple"));
        NPCStock.Add(inventoryManager.GetItem("apple"));
        NPCStock.Add(inventoryManager.GetItem("apple"));
    }
    public void SetNPCStock(List<TradeableItem> NPCStock)
    {
        this.NPCStock = NPCStock;
    }
    public List<TradeableItem> GetNPCStock()
    {
        return NPCStock;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
}
