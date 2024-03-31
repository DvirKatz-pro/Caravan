using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NPC Manager Keep track of all the NPCs
/// </summary>
public class NPCManager : SingletonManager<NPCManager>
{
    Dictionary<int, NPC> NPCsById = new Dictionary<int, NPC>();
    Dictionary<string,NPC> NPCsByName = new Dictionary<string,NPC>();

    [SerializeField] private string jsonName = "NPCsInTerritory.json";

    private const string NPC_PATH = "Assets\\Resources\\NPCData\\";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<NPC> InitializeNPCsForTerritory(TerritoryManager.Territories territoryName) 
    {
        return JSONParser.Instance.OpenJSONNPCsInTerritory(NPC_PATH + jsonName,territoryName);
    }

    public void AddNewNPC(NPC npc)
    {
        NPCsById.Add(npc.id, npc);
        NPCsByName.Add(npc.NPCname, npc);
    }

    public NPC GetNPCById(int id) 
    {
        return NPCsById[id];
    }

    public NPC GetNPCByName(string name)
    {
        return NPCsByName[name];
    }
}
