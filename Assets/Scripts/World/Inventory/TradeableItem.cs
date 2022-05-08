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
    [SerializeField] private string Name;
    [SerializeField] private float basePrice;
    [SerializeField] private Sprite sprite;

    public TradeableItem(TradeableItem item)
    {
        this.Name = item.Name;
        this.basePrice = item.basePrice;
        this.sprite = item.sprite;
    }

    public TradeableItem(string Name, float price,Sprite sprite)
    {
        this.Name = Name;
        this.basePrice = price;
        this.sprite = sprite;
    }

    public void SetName(string Name)
    {
        this.Name = Name;      
    }
    public string GetName()
    {
        return this.Name;
    }
    public void SetBasePrice(float basePrice)
    {
       this.basePrice=basePrice;
    }
    public float GetBasePrice()
    {
        return this.basePrice;
    }
    public void SetSprite(Sprite sprite)
    {
        this.sprite=sprite;
    }
    public Sprite GetSprite()
    {
        return this.sprite;
    }
   
    

}
