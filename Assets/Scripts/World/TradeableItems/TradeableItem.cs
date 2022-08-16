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
    public bool necessary { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public TradeItemAttributes.ItemTypes itemType { get; set; }
    public Sprite sprite { get; set; }

    
    public TradeableItem(TradeableItem item)
    {
        this.itemName = item.itemName;
        this.basePrice = item.basePrice;
        this.necessary = item.necessary;
        this.sprite = item.sprite;
        
    }

    public TradeableItem(string itemName, float price, bool necessary, TradeItemAttributes.ItemTypes itemType , Sprite sprite)
    {
        this.itemName = itemName;
        this.basePrice = price;
        this.necessary = necessary;
        this.itemType = itemType;
        this.sprite = sprite;
        
    }

   
}
