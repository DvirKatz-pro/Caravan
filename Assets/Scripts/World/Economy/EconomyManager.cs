using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


/// <summary>
/// A Territory Object, keep a track of Territory data
/// </summary>
public class EconomyManager: SingletonManager<EconomyManager> , ITimeSubscriber
{
    private TradeableItemsManager itemManager;
    private KingdomManager kingdomManager;
    void Start() 
    {
        TimeManager.Instance.RegisterSeason(this);
        kingdomManager = KingdomManager.Instance;
        itemManager = TradeableItemsManager.Instance;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Calculate the buying and selling price of all items for all territories
    /// </summary>
    private void AdvanceMarketConditions() 
    {
        List<Kingdom> kingdoms = kingdomManager.GetKingdoms();
    }

    /// <summary>
    /// Generate the buying and selling prices for a certain Territory
    /// </summary>
    public Dictionary<string,TradeableItem> GenerateItemPriceDictionaryForTerritory(Territory territory)
    {
        Dictionary<string,TradeableItem> itemPriceAsDictionary = new Dictionary<string,TradeableItem>();

        if (itemManager == null)
        {
            itemManager = TradeableItemsManager.Instance;
        }
        List<TradeableItem> allItems = itemManager.GetAllItemsAsList();

        foreach (TradeableItem item in allItems)
        {
            itemPriceAsDictionary.Add(item.itemName, item);
        }

        return itemPriceAsDictionary;
    }

    void ITimeSubscriber.NotifyHour()
    {
        
    }

    void ITimeSubscriber.NotifyDay()
    {
        
    }

    void ITimeSubscriber.NotifySeason()
    {
        
    }

    void ITimeSubscriber.NotifyYear()
    {
        
    }
}
