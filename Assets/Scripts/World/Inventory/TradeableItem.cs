using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    public void setName(string Name)
    {
        this.Name = Name;      
    }
    public string getName()
    {
        return this.Name;
    }
    public void setBasePrice(float basePrice)
    {
       this.basePrice=basePrice;
    }
    public float getBasePrice()
    {
        return this.basePrice;
    }
    public void setSprite(Sprite sprite)
    {
        this.sprite=sprite;
    }
    public Sprite getSprite()
    {
        return this.sprite;
    }
   
    

}
