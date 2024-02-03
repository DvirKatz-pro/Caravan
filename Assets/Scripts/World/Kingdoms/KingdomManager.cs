using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class KingdomManager : SingletonManager<KingdomManager>
{
    private Dictionary<Kingdoms, Kingdom> kingdoms;
    private List<Kingdom> kingdomsList = new List<Kingdom>();
  
    public enum Kingdoms
    {
        KievRus,
        None
    }
    
    private new void Awake()
    {
        base.Awake();



        HashSet<Kingdom.Territories> territories = new HashSet<Kingdom.Territories>
        {
            Kingdom.Territories.Nis
        };
        Kingdoms[] kingdomsArr = (Kingdoms[])Enum.GetValues(typeof(Kingdoms));
        for (int i = 0; i < Enum.GetValues(typeof(Kingdoms)).Length - 1; i++) 
        {
            Kingdoms kingdomName = kingdomsArr[i];
            Kingdom kingdom = new Kingdom(kingdomName, Kingdom.PoliticalState.Peace,territories,"Peter",500,500,500);
            kingdoms.Add(kingdomName, kingdom);
            kingdomsList.Add(kingdom);
        }

    }

    public Kingdom GetKingdomByName(Kingdoms kingdom) 
    {
        return kingdoms[kingdom];
    }
    public Kingdom GetKingdomByTerritory(Kingdom.Territories territory)
    {
        foreach (KeyValuePair<Kingdoms, Kingdom> kingdom in kingdoms)
        {
            if (kingdom.Value.ownedTerritory.Contains(territory)) 
            {
                return kingdom.Value;
            }
        }
        return null;
    }

    public void StartWar(List<Kingdoms> attackingKingdoms, List<Kingdom> defendingKingdoms) 
    {
        //add code here for when kingdoms start wars, create a Wars Object to hold data
    }

    public void ProgressWar() 
    {
        //add code here to progress all of the wars
    }

    public void EndWar(List<Kingdom> winningKingdoms, List<Kingdom> losingKingdom)
    {
        //add code here to end a war
    }

    public List<Kingdom> GetKingdoms() { return kingdomsList; }
}
