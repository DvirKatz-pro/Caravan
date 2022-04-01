using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeableItem : MonoBehaviour
{
    [SerializeField] private string name;
    [SerializeField] private float basePrice;

    public void setName(string name)
    {
        this.name = name;
    }
    public void setBasePrice(float basePrice)
    {
        this.basePrice = basePrice;
    }

    public string getName()
    {
        return name;
    }
    public float getBasePrice()
    {
        return basePrice;
    }
}
