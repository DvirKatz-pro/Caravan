using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KingdomGeneric : MonoBehaviour
{
    [SerializeField] private List<GameObject> traders;

    // Start is called before the first frame update
    void Start()
    {
        InitializeStock();
    }

    protected void InitializeStock()
    {
        List<TradeableItem> npcStock = new List<TradeableItem>();
        TradeableItemsManager tradeableItemsManager = TradeableItemsManager.Instance;
        int amountOfItems = Random.Range(25, 40);
        int amountOfNecessaryItems = Random.Range(1, 3);
        foreach (GameObject trader in traders)
        {
            NPCInventoryBreakdown breakdown = trader.GetComponent<NPCInventoryBreakdown>();
            if (breakdown != null)
            {
                Dictionary<TradeItemAttributes.ItemTypes, float> precentages = breakdown.getInventoryBreakdown();
                if (precentages.ContainsKey(TradeItemAttributes.ItemTypes.food))
                {
                    float percent = precentages[TradeItemAttributes.ItemTypes.food];
                    int amountOfFood = (int)(percent * amountOfItems);
                    List<TradeableItem> foodItems = tradeableItemsManager.GetFoodItemsAsList();
                    AddItemsToStock(npcStock, foodItems, amountOfFood, amountOfNecessaryItems);
                    
                }
                if (precentages.ContainsKey(TradeItemAttributes.ItemTypes.armor))
                {
                    float percent = precentages[TradeItemAttributes.ItemTypes.armor];
                    int amountOfArmor = (int)(percent * amountOfItems);
                    List<TradeableItem> armorItems = tradeableItemsManager.GetArmorItemsAsList();
                    AddItemsToStock(npcStock, armorItems, amountOfArmor, amountOfNecessaryItems);
                }
            }

            trader.GetComponent<NPCInventory>().SetNPCStock(npcStock);
            
        }

    }

    private void AddItemsToStock(List<TradeableItem> npcStock, List<TradeableItem> items,int amountTotal ,int amountOfNecessaryItems)
    {
        TradeableItemsManager tradeableItemsManager = TradeableItemsManager.Instance;
        List<TradeableItem> necessaryItems = tradeableItemsManager.GetItemsByRarity(items, TradeItemAttributes.Rarity.necessary);
        foreach (TradeableItem item in necessaryItems)
        {
            for (int i = 0; i < amountOfNecessaryItems; i++)
            {
                npcStock.Add(item);
                amountTotal--;
            }
        }

        for (int i = 0; i < amountTotal; i++)
        {
            int randomItem = Random.Range(0, items.Count - 1);
            npcStock.Add(items[randomItem]);
            amountTotal--;
        }
    }
}
