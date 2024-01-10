using System;
using System.Collections;
using System.Collections.Generic;
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




        Kingdoms[] kingdomsArr = (Kingdoms[])Enum.GetValues(typeof(Kingdoms));
        for (int i = 0; i < Enum.GetValues(typeof(Kingdoms)).Length - 1; i++) 
        {
            Kingdoms kingdomName = kingdomsArr[i];
            Kingdom kingdom = new Kingdom(kingdomName, Kingdom.PoliticalState.Peace,null,"Peter",500,500,500);
            kingdoms.Add(kingdomName, kingdom);
        }

    }

    public Kingdom GetKingdomByName(Kingdoms kingdom) 
    {
        return kingdoms[kingdom];
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
