using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using static StoryObject;
using static StoryObject.SpecialStoryAction;
using Object = System.Object;
using Random = UnityEngine.Random;

/// <summary>
/// A class to parse a JSON into storyObject Tree to be used in Dialouge or Travel Events
/// </summary>
public class JSONParser : SingletonManager<JSONParser>
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Open given json of NPC that contains the dialouge tree
    /// </summary>
    public StoryObject OpenJsonDialougeTree(string jsonPath)
    {
        JObject jsonText = JObject.Parse(File.ReadAllText(jsonPath));
        if (jsonText.ContainsKey("Dialouge"))
        {
            StoryObject headEvent = null;
            JObject dialouge = (JObject)jsonText["Dialouge"];
            if (dialouge != null)
            {
                headEvent = CreateStoryObjects(dialouge, null);
                return headEvent;
            }
        }
        return null;
    }

    public Tuple<List<string>, List<string>> OpenJsonLeaderNames(string jsonPath)
    {
        JObject jsonText = JObject.Parse(File.ReadAllText(jsonPath));
        if (jsonText.ContainsKey("LeaderNames"))
        {
            JObject leaderNames = (JObject)jsonText["LeaderNames"];
            Tuple<List<string>,List<string>> nameLists = GenerateLeaderName(leaderNames);
            return nameLists;
        }
        return null;
    }

    public List<TerritoryManager.Territories> OpenJSONOwnedTerritories(string jsonPath,KingdomManager.Kingdoms kingdomName) 
    {
        JObject jsonText = JObject.Parse(File.ReadAllText(jsonPath));
        if (jsonText.ContainsKey("Kingdoms"))
        {
            JObject kingdom = (JObject)jsonText["Kingdoms"];
            JObject ownedTerritories = (JObject)kingdom[kingdomName.ToString()];
            List<TerritoryManager.Territories> ownedTerritory = GetKingdomOwnedTerritory(ownedTerritories);
            return ownedTerritory;
        }
        return null;
    }

    public List<NPC> OpenJSONNPCsInTerritory(string jsonPath, TerritoryManager.Territories territoryName) 
    {
        JObject jsonText = JObject.Parse(File.ReadAllText(jsonPath));
        if (jsonText.ContainsKey("Territories"))
        {
            JArray territories = (JArray)jsonText["Territories"];
            if (territories == null || territories.Count == 0)
            {
                return null;
            }
            for (int i = 0; i < territories.Count; i++)
            {
                JObject territory = (JObject)territories[i];
                if (territory[territoryName.ToString()] != null)
                {
                    List<NPC> NPCs = CreateNPCs((JArray)territory[territoryName.ToString()]);
                    return NPCs;
                }
            }
 
        }
        return null;
    }

    public List<TradeableItem> OpenJSONTradeableItems(string jsonPath)
    {
        JObject jsonText = JObject.Parse(File.ReadAllText(jsonPath));
        if (jsonText.ContainsKey("TradeableItems"))
        {
            JObject tradeableItems = (JObject)jsonText["TradeableItems"];
            List<TradeableItem> tradeableItemsList = CreateTradeableItems(tradeableItems);
            return tradeableItemsList;
        }
        return null;
    }
    /// <summary>
    /// given a head story object, create the story object tree and recursevly traverse the json tree
    /// </summary>
    private StoryObject CreateStoryObjects(JObject head, StoryObject parentObject)
    {
        string text = null;
        string responseText = null;
        StoryObject.Actions action = StoryObject.Actions.None;
        SpecialStoryAction specialAction = null;
        List<StoryObject> events = null;
        if (head.ContainsKey("Text"))
        {
            text = head["Text"].ToString();
        }
        if (head.ContainsKey("ResponseText"))
        {
            responseText = head["ResponseText"].ToString();
        }
        if (head.ContainsKey("Action")) 
        {
            action = (Actions)Enum.Parse(typeof(Actions), head["Action"].ToString(), ignoreCase: true);
        }
        StoryObject eventObject = new StoryObject(text, responseText, parentObject, action);
        if (head.ContainsKey("SpecialActionObject"))
        {
            // and create a special story object inside the StoryObject class
            JObject specialActionObject = (JObject)head["SpecialActionObject"];
            StoryObject.SpecialActions specialActionAction = (SpecialActions)Enum.Parse(typeof(SpecialActions), specialActionObject["SpecialAction"].ToString(), ignoreCase: true);
            string specialActionText = specialActionObject["Text"].ToString();
            //get the type of special action
            Dictionary<SpecialStoryAction.ActionOutcomes, StoryObject> actionOutcomes = new Dictionary<ActionOutcomes, StoryObject>();
            JObject options = (JObject)specialActionObject["Options"];
            int succsess = (int)options["SucssessChance"];
            int fail = (int)options["FailChance"];
            int criticalFail = (int)options["CriticalFailChance"];

            //alculate the chance of each outcome of the action
            int chosenRoll = UnityEngine.Random.Range(0, 100);

            SpecialStoryAction.ActionOutcomes chosenOutcome = SpecialStoryAction.ActionOutcomes.Succsess;

            if (chosenRoll <= fail) {
                chosenOutcome = SpecialStoryAction.ActionOutcomes.Failure;
            }
            else if(chosenRoll > fail && chosenRoll <= fail + criticalFail )
            {
                chosenOutcome = SpecialStoryAction.ActionOutcomes.CriticalFailure;
            }


            //recursvly traverse the storyObject tree of each outcome
            actionOutcomes.Add(SpecialStoryAction.ActionOutcomes.Succsess, CreateStoryObjects((JObject)options["Sucssess"], eventObject));
            actionOutcomes.Add(SpecialStoryAction.ActionOutcomes.Failure, CreateStoryObjects((JObject)options["Fail"], eventObject));
            actionOutcomes.Add(SpecialStoryAction.ActionOutcomes.CriticalFailure, CreateStoryObjects((JObject)options["CriticalFail"], eventObject));
            //create a new specialStoryAction inside the StoryObject class
            specialAction = new SpecialStoryAction(specialActionAction,specialActionText, actionOutcomes,chosenOutcome);
        }
        //recursvly traverse the storyObject tree of each response
        JArray responsesArray = (JArray)head["Responses"];
        if (responsesArray != null && responsesArray.Count > 0)
        { 
            events = new List<StoryObject>();
            for (int i = 0; i < responsesArray.Count; i++)
            {
                events.Add(CreateStoryObjects((JObject)responsesArray[i], eventObject));
            }
        }
        eventObject.specialStoryAction = specialAction;
        eventObject.stories = events;
        return eventObject;
    }

    private Tuple<List<string>,List<string>> GenerateLeaderName(JObject names)
    {
        List<string> boyNames = (List<string>)names["BoyNames"].ToObject(typeof(List<string>));
        List<string> girlNames = (List<string>)names["GirlNames"].ToObject(typeof(List<string>));

        return new Tuple<List<string>, List<string>>(boyNames,girlNames);
    }

    private List<TerritoryManager.Territories> GetKingdomOwnedTerritory(JObject ownedTerritory)
    {
        List<string> territoryNames = (List<string>)ownedTerritory["OwnedTerritory"].ToObject(typeof(List<string>));
        List<TerritoryManager.Territories> ownedTerritories = new List<TerritoryManager.Territories>();
        foreach (string name in territoryNames)
        {
            TerritoryManager.Territories territory = (TerritoryManager.Territories)Enum.Parse(typeof(TerritoryManager.Territories), name, ignoreCase: true);
            ownedTerritories.Add(territory);
        }
        return ownedTerritories;
    }

    private List<TradeableItem> CreateTradeableItems(JObject tradeableItems)
    {
        List<TradeableItem> items = new List<TradeableItem>();
        foreach (JProperty property in tradeableItems.Properties())
        {
            JObject itemAsJson = (JObject)property.Value;
            //Get texture for this item from the resources folder
            Texture2D texture = Resources.Load<Texture2D>("Sprites/RPG_inventory_icons/" + property.Name);
            Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
            string typeAsString = itemAsJson["itemType"].ToString();
            //convert string to TradeableItems enums
            TradeItemAttributes.ItemTypes type;
            bool isParseSuccessful = Enum.TryParse(typeAsString, out type);
            if (!isParseSuccessful)
            {
                throw new Exception("Item type not found for " + property.Name);
            }
            string rarityAsString = itemAsJson["rarity"].ToString();
            TradeItemAttributes.Rarity rarity;
            isParseSuccessful = Enum.TryParse(rarityAsString, out rarity);
            if (!isParseSuccessful)
            {
                throw new Exception("Item rarity not found for " + property.Name);
            }

            TradeableItem item = new TradeableItem(property.Name, float.Parse(itemAsJson["basePrice"].ToString()), type, rarity, sprite);
            items.Add(item);
        }
        return items;
    }

    private List<NPC> CreateNPCs(JArray npcsArray)
    {

        if (npcsArray != null && npcsArray.Count > 0)
        {
            List<NPC> npcs = new List<NPC>();
            for (int i = 0; i < npcsArray.Count; i++)
            {
                NPC.NPCType type;
                JObject npcObj = (JObject)npcsArray[i];
                int id = int.Parse(npcObj["id"].ToString());
                string name = npcObj["NPCname"].ToString();
                string typeAsString = npcObj["NPCType"].ToString();
                bool isParseSuccessful = Enum.TryParse(typeAsString, out type);
                if (!isParseSuccessful)
                {
                    throw new Exception("NPC type not found for " + typeAsString);
                }
                NPCInventoryBreakdown nPCInventoryBreakdown = null;
                if (type == NPC.NPCType.Trader)
                {
                    JObject breakdown = (JObject)npcObj["NPCInventoryBreakdown"];
                    int inventorySize = int.Parse(breakdown["inventorySize"].ToString());
                    int amountOfNecessary = int.Parse(breakdown["amountOfNecessary"].ToString());
                    float foodPercent = float.Parse(breakdown["foodPercent"].ToString());
                    float armorPercent = float.Parse(breakdown["armorPercent"].ToString());
                    nPCInventoryBreakdown = new NPCInventoryBreakdown(inventorySize, amountOfNecessary, foodPercent, armorPercent,null);
                }
                NPC npc = new NPC(type,id,name,nPCInventoryBreakdown,null);
                NPCManager.Instance.AddNewNPC(npc);
                npcs.Add(npc);
            }
            return npcs;
        }
        return null;
    }
}
