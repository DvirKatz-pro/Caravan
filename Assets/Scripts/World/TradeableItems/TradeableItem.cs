using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Class detailing tradeable object data
/// </summary>
public class TradeableItem
{
    public string itemName { get; set; }
    public float basePrice{ get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public TradeItemAttributes.ItemTypes itemType { get; set; }
    public TradeItemAttributes.Rarity itemRarity { get; set; }
    public Sprite sprite { get; set; }

    
    public TradeableItem(TradeableItem item)
    {
        this.itemName = item.itemName;
        this.basePrice = item.basePrice;
        this.itemType = item.itemType;
        this.itemRarity = item.itemRarity;
        this.sprite = item.sprite;
        
    }

    public TradeableItem(string itemName, float price, TradeItemAttributes.ItemTypes itemType, TradeItemAttributes.Rarity itemRarity, Sprite sprite)
    {
        this.itemName = itemName;
        this.basePrice = price;
        this.itemType = itemType;
        this.itemRarity = itemRarity;
        this.sprite = sprite;
        
    }

   
}
