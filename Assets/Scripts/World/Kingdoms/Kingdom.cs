using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Territory;

public class Kingdom : MonoBehaviour
{
    public enum PoliticalState
    {
        War,
        Peace
    }

    public enum Territories
    {
        Nis,
        None
    }

    public KingdomManager.Kingdoms kingdomName { get; set; }
    public PoliticalState politicalState { get; set; }
    public HashSet<Territories> ownedTerritory { get; set; }

    public string leaderName { get; set; }
    public float populanceMood { get; set; }
    public float armyStrength { get; set; }
    public float economicProsperity { get; set; }
    public float populanceMoodToPlayer { get; set; }

    public Kingdom() 
    {

    }

    public Kingdom(KingdomManager.Kingdoms kingdomName, PoliticalState politicalState, HashSet<Territories> ownedTerritory, string leaderName, float populanceMood, float armyStrength, float populanceMoodToPlayer)
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
