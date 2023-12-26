using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Class handleing trade between a player and an NPC
/// </summary>
public class TradeManager : SingletonManager<TradeManager>
{
   
    //Needed items and classes
    [SerializeField] private List<InventoryManager.InvnetorySlots> sellSlots;
    [SerializeField] private List<InventoryManager.InvnetorySlots> buySlots;
    [SerializeField] private TextMeshProUGUI fundsText;
    [SerializeField] private TradeScreen sellScreen;
    [SerializeField] private TradeScreen buyScreen;
    [SerializeField] private TextMeshProUGUI executeTradeText;

    //Needed Classes 
    private InventoryManager inventoryManager;
    private GameObject currentNPC;

    //the current balance of the trade
    private float currentBalance = 0;
    //the constant text that is displayed in the 'execute trade' button
    private const string BUTTON_TEXT = "Execute Trade \nBalance: ";

    public Vector2 TradeWindowItemSize = Vector2.one;

    /// <summary>
    /// enum that designates the two types of trade slots
    /// </summary>
    public enum SlotType
    {
        sellSlot,
        buySlot
    }
    #region Trade actions
    /// <summary>
    /// Set the trade inventorys of both the NPC and the player from their inventory classes
    /// </summary>
    public void OpenTradeScreen(GameObject npc)
    {
        this.currentNPC = npc;

        this.inventoryManager = InventoryManager.Instance;

        currentBalance = 0;
        executeTradeText.text = BUTTON_TEXT + currentBalance;

        List<InventoryManager.InvnetorySlots> slots = inventoryManager.GetInventorySlots();
        for (int i = 0; i < slots.Count; i++)
        {
            for (int j = 0; j < slots[0].slots.Count; j++)
            {
                UISlot slot = slots[i].slots[j].GetComponent<UISlot>();

                if (slot.item != null)
                {
                    sellSlots[i].slots[j].GetComponent<UISlot>().UpdateItem(slot.item);
                    sellSlots[i].slots[j].GetComponent<TradeSlot>().SetSlotType(SlotType.sellSlot);
                }
            }
        }
        //set the avialable player funds for the trade
        fundsText.text = InventoryManager.FUNDS_TEXT + inventoryManager.GetCurrentFunds();

        foreach (TradeableItem item in this.currentNPC.GetComponent<NPCInventory>().GetNPCStock())
        {
            AddToBuyInventory(item);
        }
        transform.GetChild(0).gameObject.SetActive(true);
    }

   
    /// <summary>
    /// When an item is added/removed from the trade screen we update the current trade balance
    /// </summary>
    public void ChangeTradeBalance(float amount)
    {
        currentBalance += amount;
        executeTradeText.text = BUTTON_TEXT + currentBalance;
    }

    /// <summary>
    /// Add a the sell objects to the NPC inventory and the buy objects to the Players inventory
    /// </summary>
    public void ExecuteTrade()
    {
        //check if the player has the funds to execute the trade
        if (inventoryManager.GetCurrentFunds() + currentBalance >= 0)
        {
            //check that the Player has enough space in their inventory for the bought items
            if (inventoryManager.GetAvailableSpace() >= GetBuyScreen().GetItems().Count)
            {
                //add the bought items to the NPC inventory and remove the items from the sell screen
                List<GameObject> sellObjects = GetSellScreen().GetItems();
                for (int i = sellObjects.Count - 1; i >= 0; i--)
                {
                    AddToBuyInventory(sellObjects[i].GetComponent<UISlot>().item);
                    inventoryManager.RemoveFromInventory(sellObjects[i].GetComponent<UISlot>().item);
                    currentNPC.GetComponent<NPCInventory>().AddToInventory(sellObjects[i].GetComponent<UISlot>().item);
                    GameObject g = sellObjects[i];
                    sellObjects.RemoveAt(i);
                    Destroy(g);
                }
                //add the sold items to the Player inventory and remove the items from the buy screen
                List<GameObject> buyObjects = GetBuyScreen().GetItems();
                for (int i = buyObjects.Count - 1; i >= 0; i--)
                {
                    AddToSellInventory(buyObjects[i].GetComponent<UISlot>().item);
                    inventoryManager.AddToInventory(buyObjects[i].GetComponent<UISlot>().item);
                    currentNPC.GetComponent<NPCInventory>().RemoveFromInventory(buyObjects[i].GetComponent<UISlot>().item);
                    GameObject g = buyObjects[i];
                    buyObjects.RemoveAt(i);
                    Destroy(g);
                }

                //set the updated player funds and reset the current trade balance
                inventoryManager.SetCurrentFunds(inventoryManager.GetCurrentFunds() + currentBalance);
                fundsText.text = InventoryManager.FUNDS_TEXT + inventoryManager.GetCurrentFunds();
                currentBalance = 0;
                ChangeTradeBalance(0);

            }
        }
    }
    /// <summary>
    /// Remove items from buy/sell screens, restore inventories to pre trade state and close the trade screen
    /// </summary>
    public void ExitTrade()
    {
        List<GameObject> sellObjects = GetSellScreen().GetItems();
        for (int i = sellObjects.Count - 1; i >= 0; i--)
        {
            GameObject g = sellObjects[i];
            sellObjects.RemoveAt(i);
            Destroy(g);
        }
        List<GameObject> buyObjects = GetBuyScreen().GetItems();
        for (int i = buyObjects.Count - 1; i >= 0; i--)
        {
            GameObject g = buyObjects[i];
            buyObjects.RemoveAt(i);
            Destroy(g);
        }
        for (int i = buySlots.Count-1; i >= 0; i--)
        {
            for (int j = buySlots[i].slots.Count-1; j >= 0; j--)
            {
                buySlots[i].slots[j].GetComponent<UISlot>().UpdateItem(null);
            }
            
        }
        TradeManager.Instance.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
    #endregion
    #region Buy/Sell Screen actions
    /// <summary>
    /// Add a tradeableItem to the player trade inventory at the first avialable slot
    /// </summary>
    public void AddToSellInventory(TradeableItem tradeableItem)
    {
        for (int i = 0; i < sellSlots.Count; i++)
        {
            for (int j = 0; j < sellSlots[0].slots.Count; j++)
            {
                UISlot slot = sellSlots[i].slots[j].GetComponent<UISlot>();

                if (slot.item == null)
                {
                    sellSlots[i].slots[j].GetComponent<UISlot>().UpdateItem(tradeableItem);
                    sellSlots[i].slots[j].GetComponent<TradeSlot>().SetSlotType(SlotType.sellSlot);
                    j = sellSlots[i].slots.Count;
                    i = sellSlots.Count;
                }
            }
        }
    }
    /// <summary>
    /// Add a tradeableItem to the NPC trade inventory at the first avialable slot
    /// </summary>
    public void AddToBuyInventory(TradeableItem tradeableItem)
    {
        for (int i = 0; i < buySlots.Count; i++)
        {
            for (int j = 0; j < buySlots[0].slots.Count; j++)
            {
                UISlot slot = buySlots[i].slots[j].GetComponent<UISlot>();

                if (slot.item == null)
                {
                    buySlots[i].slots[j].GetComponent<UISlot>().UpdateItem(tradeableItem);
                    buySlots[i].slots[j].GetComponent<TradeSlot>().SetSlotType(SlotType.buySlot);
                    j = buySlots[i].slots.Count;
                    i = buySlots.Count;
                }
            }
        }
    }
    public TradeScreen GetSellScreen()
    {
        return sellScreen;
    }
    public TradeScreen GetBuyScreen()
    {
        return buyScreen;
    }
    #endregion
}
