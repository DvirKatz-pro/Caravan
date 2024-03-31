using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Territory Object, keep a track of Territory data 
/// </summary>
public class Territory
{

    
    public List<NPC> NPCs { get; set; }

    //Keep track of this territory
    private TerritoryManager.Territories territoryName = TerritoryManager.Territories.None;
    public KingdomManager.Kingdoms owningKingdom {get; set;} = KingdomManager.Kingdoms.None;
    
    private List<Road> nearRoads;

    public List<TradeableItem> itemProduction { get; set; }

    public Dictionary<string, TradeableItem> currentPriceDictionary { get; set; }

    public Territory(List<NPC> NPCs, TerritoryManager.Territories territoryName, KingdomManager.Kingdoms owningKingdom, List<Road> nearRoads, List<TradeableItem> itemProduction, Dictionary<string, TradeableItem> currentPriceDictionary)
    {
        this.NPCs = NPCs;
        this.territoryName = territoryName;
        this.owningKingdom = owningKingdom;
        this.nearRoads = nearRoads;
        this.itemProduction = itemProduction;
        this.currentPriceDictionary = currentPriceDictionary;
    }

    public Territory(List<NPC> NPCs, TerritoryManager.Territories territoryName, KingdomManager.Kingdoms owningKingdom, List<Road> nearRoads, List<TradeableItem> itemProduction)
    {
        this.NPCs = NPCs;
        this.territoryName = territoryName;
        this.owningKingdom = owningKingdom;
        this.nearRoads = nearRoads;
        this.itemProduction = itemProduction;
    }
}
