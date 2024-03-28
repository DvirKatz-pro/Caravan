using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ConcreteTerritory : MonoBehaviour
{
    [SerializeField] private List<GameObject> traders;
    [SerializeField] private TerritoryManager.Territories territoryName;

    private Territory territoryData;

    // Start is called before the first frame update
    void Start()
    {
        territoryData = TerritoryManager.Instance.GetTerritoryByName(territoryName);
        InitializeStock();
    }
    /// <summary>
    /// Create an NPC inventory containing different frquency and types of items
    /// </summary>
    protected void InitializeStock()
    {
        TradeableItemsManager tradeableItemsManager = TradeableItemsManager.Instance;
        
        
        foreach (GameObject trader in traders)
        {
            ConcreteNPC concreteNPC = trader.GetComponent<ConcreteNPC>();
            concreteNPC.InitializeNPC();
            NPC NPC = concreteNPC.npcData;
            NPCInventoryBreakdown breakdown = NPC.NPCInventoryBreakdown;
            List<TradeableItem> npcStock = new List<TradeableItem>();

            if (breakdown != null)
            {
                //calculate how many items of a certain type should be added to this inventory
                if (breakdown.foodPercent > 0)
                {
                    float percent = breakdown.foodPercent;
                    int amountOfItems = breakdown.inventorySize;
                    int amountOfFood = (int)(percent/100f * amountOfItems);
                    int amountOfNecessaryItems = breakdown.amountOfNecessary;
                    List<TradeableItem> foodItems = tradeableItemsManager.GetFoodItemsAsList();
                    AddItemsToStock(npcStock, foodItems, amountOfFood, amountOfNecessaryItems);
                }
                if (breakdown.armorPercent > 0)
                {
                    float percent = breakdown.armorPercent;
                    int amountOfItems = breakdown.inventorySize;
                    int amountOfArmor = (int)(percent/100 * amountOfItems);
                    int amountOfNecessaryItems = breakdown.amountOfNecessary;
                    List<TradeableItem> armorItems = tradeableItemsManager.GetArmorItemsAsList();
                    AddItemsToStock(npcStock, armorItems, amountOfArmor, amountOfNecessaryItems);
                }
            }

            trader.GetComponent<NPCInventory>().SetNPCStock(npcStock);

        }

    }

    /// <summary>
    /// Add random items items to an NPC Inventory stock, making sure that an x amount of "must have" items are always added
    /// </summary>
    private void AddItemsToStock(List<TradeableItem> npcStock, List<TradeableItem> items, int amountTotal, int amountOfNecessaryItems)
    {
        TradeableItemsManager tradeableItemsManager = TradeableItemsManager.Instance;
        List<TradeableItem> necessaryItems = tradeableItemsManager.GetItemsByRarity(items, TradeItemAttributes.Rarity.necessary);
        foreach (TradeableItem item in necessaryItems)
        {
            item.currentPrice = territoryData.currentPriceDictionary[item.itemName].currentPrice;
            for (int i = 0; i < amountOfNecessaryItems; i++)
            {
                npcStock.Add(item);
                amountTotal--;
            }
        }

        for (int i = 0; i < amountTotal; i++)
        {
            int randomItem = Random.Range(0, items.Count - 1);
            TradeableItem item = items[randomItem];
            item.currentPrice = territoryData.currentPriceDictionary[item.itemName].currentPrice;
            npcStock.Add(item);
            amountTotal--;
        }
    }
}
