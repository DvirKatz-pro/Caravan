using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Territory;

/// <summary>
/// A Kingdom Object, Keep track of data of a certain kingdom 
/// </summary>
public class Kingdom
{
    public enum PoliticalState
    {
        War,
        Peace
    }

    public KingdomManager.Kingdoms kingdomName { get; set; }
    public PoliticalState politicalState { get; set; }
    public Dictionary<TerritoryManager.Territories, Territory> ownedTerritory { get; set; }

    public string leaderName { get; set; }
    public float populanceMood { get; set; }
    public float armyStrength { get; set; }
    public float economicProsperity { get; set; }
    public float populanceMoodToPlayer { get; set; }

    public Kingdom() 
    {

    }

    public Kingdom(KingdomManager.Kingdoms kingdomName, PoliticalState politicalState, Dictionary<TerritoryManager.Territories,Territory> ownedTerritory, string leaderName, float populanceMood, float armyStrength, float populanceMoodToPlayer)
    {
        this.kingdomName = kingdomName;
        this.politicalState = politicalState;
        this.ownedTerritory = ownedTerritory;
        this.leaderName = leaderName;
        this.populanceMood = populanceMood;
        this.armyStrength = armyStrength;
        this.populanceMoodToPlayer = populanceMoodToPlayer;
    }
  
}
