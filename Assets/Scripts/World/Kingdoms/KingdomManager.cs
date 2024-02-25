using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using Random = UnityEngine.Random;

public class KingdomManager : SingletonManager<KingdomManager>
{
    private Dictionary<Kingdoms, Kingdom> kingdoms = new Dictionary<Kingdoms, Kingdom>();
    private List<Kingdom> kingdomsList = new List<Kingdom>();
    private List<string> leaderNamesMan = new List<string>();
    private List<string> leaderNamesWoman = new List<string>();

    [SerializeField] private string jsonName = "NameData.json";
    private const string NAME_PATH = "Assets\\Resources\\GameData\\";
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
        Tuple<List<string>,List<string>> nameLists = JSONParser.Instance.OpenJsonLeaderNames(NAME_PATH + jsonName);
        leaderNamesMan = nameLists.Item1;
        leaderNamesWoman = nameLists.Item2;
        string leaderName;
        bool isWoman = Convert.ToBoolean(Random.Range(0, 1));
        if (isWoman)
        {
            leaderName = leaderNamesWoman[Random.Range(0, leaderNamesWoman.Count)];
        }
        else
        {
            leaderName = leaderNamesMan[Random.Range(0, leaderNamesMan.Count)];
        }
        for (int i = 0; i < Enum.GetValues(typeof(Kingdoms)).Length - 1; i++) 
        {
            
            Kingdoms kingdomName = kingdomsArr[i];
            Kingdom kingdom = new Kingdom(kingdomName, Kingdom.PoliticalState.Peace,territories, leaderName, 500,500,500);
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
