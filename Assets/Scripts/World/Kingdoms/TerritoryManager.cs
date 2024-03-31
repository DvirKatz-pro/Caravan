using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Territory Manager Keep track of all the territories
/// </summary>
public class TerritoryManager : SingletonManager<TerritoryManager>
{
    EconomyManager economyManager;
    public enum Territories
    {
        Nis,
        None
    }
    public Dictionary<Territories, Territory> territoryDictionary = new Dictionary<Territories, Territory>();
    // Start is called before the first frame update
    void Start()
    {
        economyManager = EconomyManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addTerritory(Territories territoryName, Territory territory) 
    {
        territoryDictionary.Add(territoryName, territory);
    }

    public Territory GetTerritoryByName(Territories territoryName)
    {
        return territoryDictionary[territoryName];
    }

    /// <summary>
    /// Create A Territory, Initialize it and return it
    /// </summary>
    public Territory initializeTerritory(TerritoryManager.Territories territoryName, KingdomManager.Kingdoms kingdomName)
    {
        List<NPC> npcList = NPCManager.Instance.InitializeNPCsForTerritory(territoryName);
        Territory territory = new Territory(npcList, territoryName, kingdomName, null, null);
        if (economyManager == null)
        {
            economyManager = EconomyManager.Instance;
        }
        territory.currentPriceDictionary = economyManager.GenerateItemPriceDictionaryForTerritory(territory);
        addTerritory(territoryName, territory);
        return territory;
    }
}
