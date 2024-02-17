using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EconomyManager: SingletonManager<EconomyManager> , ITimeSubscriber
{
    private TradeableItemsManager itemManager;
    void Start() 
    {
        TimeManager.Instance.RegisterSeason(this);
        
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    private void AdvanceMarketConditions() 
    {

    }

    void ITimeSubscriber.NotifyTime()
    {
        Debug.Log("Notify season!");
    }
}
