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
        EconomyManager economyManager = EconomyManager.Instance;
        NPCStock.Add(economyManager.GetItem("armor"));
        NPCStock.Add(economyManager.GetItem("armor"));
        NPCStock.Add(economyManager.GetItem("armor"));
        NPCStock.Add(economyManager.GetItem("armor"));
        NPCStock.Add(economyManager.GetItem("apple"));
        NPCStock.Add(economyManager.GetItem("apple"));
        NPCStock.Add(economyManager.GetItem("apple"));
        NPCStock.Add(economyManager.GetItem("apple"));
        NPCStock.Add(economyManager.GetItem("apple"));
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
