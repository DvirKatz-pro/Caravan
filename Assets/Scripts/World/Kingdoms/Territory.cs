using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Territory : MonoBehaviour
{
    

    [SerializeField] private List<GameObject> traders;

    //Keep track of this territory
    private Kingdom.Territories territoryName = Kingdom.Territories.None;
    private KingdomManager.Kingdoms owningKingdom = KingdomManager.Kingdoms.None;
    
    private List<Road> nearRoads;

    public List<TradeableItem> itemProduction { get; set; }

  
}
