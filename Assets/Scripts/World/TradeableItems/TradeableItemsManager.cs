using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
/// <summary>
/// Class to hold and sort TradeableItems
/// </summary>
public class TradeableItemsManager : SingletonManager<TradeableItemsManager>
{
    [SerializeField] private string jsonName = "TradeableItems.json";
    private const string RESOURCE_PATH = "Assets\\Resources\\GameData\\";

    Dictionary<string, TradeableItem> allItems;
    Dictionary<string, TradeableItem> foodItems;
    Dictionary<string, TradeableItem> armorItems;

    // Start is called before the first frame update
    new void Awake()
    {
        allItems = new Dictionary<string, TradeableItem>();
        foodItems = new Dictionary<string, TradeableItem>();
        armorItems = new Dictionary<string, TradeableItem>();

        LoadAndSortItems();
    }

    // Update is called once per frame
    void Update()
    {

    }
    #region Get Items 
    public Dictionary<string, TradeableItem> GetAllItems()
    {
        return allItems;
    }
    public Dictionary<string, TradeableItem> GetFoodItems()
    {
        return foodItems;
    }
    public Dictionary<string, TradeableItem> GetArmorItems()
    {
        return armorItems;
    }
    public List<TradeableItem> GetAllItemsAsList()
    {
        return allItems.Values.ToList();
    }
    public List<TradeableItem> GetFoodItemsAsList()
    {
        return foodItems.Values.ToList();
    }
    public List<TradeableItem> GetArmorItemsAsList()
    {
        return armorItems.Values.ToList();
    }

    public TradeableItem GetItem(string itemName)
    {
        return allItems.ContainsKey(itemName) ? allItems[itemName] : null;
    }
    #endregion
    #region Get fillterd Items  
    /// <summary>
    /// Given a list return all the items in that list where their rarity is equal to the given rarity 
    /// </summary>
    public List<TradeableItem> GetItemsByRarity(List<TradeableItem> listToQuery,TradeItemAttributes.Rarity rarity)
    {
        List<TradeableItem> itemResult = new List<TradeableItem>();


        foreach (TradeableItem item in listToQuery)
        {
            if (item.itemRarity == rarity)
            {
                itemResult.Add(item);
            }
        }

        return itemResult;
    }
    /// <summary>
    /// Given a list return all the items in that list sorted by rarity from low to high
    /// </summary>
    public List<TradeableItem> GetItemsSortedByRarity(List<TradeableItem> listToQuery)
    {
        return listToQuery.OrderBy(x => (int)(x.itemRarity)).ToList();
    }
    /// <summary>
    /// Given a list return all the items in that list sorted by price from low to high
    /// </summary>
    public List<TradeableItem> GetItemsSortedByPrice(List<TradeableItem> listToQuery)
    {
        return listToQuery.OrderBy(x => (int)(x.basePrice)).ToList();
    }
    #endregion
    #region Load items from json 
    /// <summary>
    /// Read the item data from json and assign it to a TradeableItem class and add it to the appropriate dictionary
    /// </summary>
    private void LoadAndSortItems()
    {
        List<TradeableItem> tradeAbleItems = JSONParser.Instance.OpenJSONTradeableItems(RESOURCE_PATH + jsonName);
        foreach (TradeableItem item in tradeAbleItems)
        {
            
            allItems.Add(item.itemName, item);
            //add the item to the appropriate item dictionary based on its type
            switch (item.itemType)
            {
                case TradeItemAttributes.ItemTypes.food:

                    foodItems.Add(item.itemName, item);
                    break;
                case TradeItemAttributes.ItemTypes.armor:
                    armorItems.Add(item.itemName, item);
                    break;
            }
        }
        

    }
    #endregion
}
