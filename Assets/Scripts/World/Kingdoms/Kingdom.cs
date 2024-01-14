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
    private PoliticalState politicalState { get; set; }
    private HashSet<Territories> ownedTerritory { get; set; }

    private string leaderName { get; set; }
    private float populanceMood { get; set; }
    private float armyStrength { get; set; }
    private float economicProsperity { get; set; }
    private float populanceMoodToPlayer { get; set; }

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
