using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that contains the list of what the NPC is going to sell
/// </summary>
public class NPCInventory : MonoBehaviour
{
    private List<TradeableItem> NPCStock;
    // Start is called before the first frame update
    void Start()
    {
        NPCStock = new List<TradeableItem>();
        TradeableItemsManager itemManager = TradeableItemsManager.Instance;
        NPCStock.Add(itemManager.GetItem("armor"));
        NPCStock.Add(itemManager.GetItem("armor"));
        NPCStock.Add(itemManager.GetItem("armor"));
        NPCStock.Add(itemManager.GetItem("armor"));
        NPCStock.Add(itemManager.GetItem("apple"));
        NPCStock.Add(itemManager.GetItem("apple"));
        NPCStock.Add(itemManager.GetItem("apple"));
        NPCStock.Add(itemManager.GetItem("apple"));
        NPCStock.Add(itemManager.GetItem("apple"));
    }
    public void SetNPCStock(List<TradeableItem> NPCStock)
    {
        this.NPCStock = NPCStock;
    }
    public List<TradeableItem> GetNPCStock()
    {
        return NPCStock;
    }
    public void AddToInventory(TradeableItem tradeableItem)
    {
        NPCStock.Add(tradeableItem);
    }
    public void RemoveFromInventory(TradeableItem tradeableItem)
    {
        NPCStock.Remove(tradeableItem);
    }


   
}
