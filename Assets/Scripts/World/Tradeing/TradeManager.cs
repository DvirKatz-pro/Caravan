using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TradeManager : SingletonManager<TradeManager>
{
    private InventoryManager inventoryManager;

    [SerializeField] private List<InventoryManager.InvnetorySlots> sellSlots;
    [SerializeField] private List<InventoryManager.InvnetorySlots> buySlots;
    [SerializeField] private TextMeshProUGUI fundsText;
    [SerializeField] private TradeScreen sellScreen;
    [SerializeField] private TradeScreen buyScreen;
    [SerializeField] private TextMeshProUGUI executeTradeText;
    public Vector2 TradeWindowItemSize = Vector2.one;

    private float currentBalance = 0;

    private const string BUTTON_TEXT = "Execute Trade \nBalance: ";


    public enum SlotType
    {
        sellSlot,
        buySlot
    }
  
    public void OpenTradeScreen(List<TradeableItem> NPCStock)
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
                    sellSlots[i].slots[j].GetComponent<UISlot>().UpdateItem(slot.item);
                    sellSlots[i].slots[j].GetComponent<TradeSlot>().SetSlotType(SlotType.sellSlot);
                }
            }
        }

        fundsText.text = InventoryManager.FUNDS_TEXT + inventoryManager.GetCurrentFunds();

        foreach (TradeableItem item in NPCStock)
        {
            AddToBuyInventory(item);
        }
        transform.GetChild(0).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }
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

    public void ChangeTradeBalance(float amount)
    {
        currentBalance += amount;
        executeTradeText.text = BUTTON_TEXT + currentBalance;
    }

    public void ExecuteTrade()
    {
        if (inventoryManager.GetCurrentFunds() + currentBalance >= 0)
        {
            List<GameObject> sellObjects = GetSellScreen().GetItems();
            for (int i = sellObjects.Count-1; i >= 0; i--)
            {
                AddToBuyInventory(sellObjects[i].GetComponent<UISlot>().item);
                inventoryManager.RemoveFromInventory(sellObjects[i].GetComponent<UISlot>().item);
                GameObject g = sellObjects[i];
                sellObjects.RemoveAt(i);
                Destroy(g);
            }

            List<GameObject> buyObjects = GetBuyScreen().GetItems();
            for (int i = buyObjects.Count - 1; i >= 0; i--)
            {
                AddToSellInventory(buyObjects[i].GetComponent<UISlot>().item);
                inventoryManager.AddToInventory(buyObjects[i].GetComponent<UISlot>().item);
                GameObject g = buyObjects[i];
                buyObjects.RemoveAt(i);
                Destroy(g);
            }
            inventoryManager.SetCurrentFunds(inventoryManager.GetCurrentFunds() + currentBalance);
            fundsText.text = InventoryManager.FUNDS_TEXT + inventoryManager.GetCurrentFunds();
            currentBalance = 0;
            ChangeTradeBalance(0);
            
            
        }
    }
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
            AddToBuyInventory(buyObjects[i].GetComponent<UISlot>().item);
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
}
