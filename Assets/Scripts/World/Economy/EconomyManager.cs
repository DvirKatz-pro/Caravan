using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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

    private void AdvanceMarketConditions() 
    {
        List<Kingdom> kingdoms = kingdomManager.GetKingdoms();

        
    }

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
