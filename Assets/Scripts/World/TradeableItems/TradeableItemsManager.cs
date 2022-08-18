using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TradeableItemsManager : SingletonManager<TradeableItemsManager>
{
    [SerializeField] private string jsonPath;

    Dictionary<string, TradeableItem> allItems;
    Dictionary<string, TradeableItem> foodItems;
    Dictionary<string, TradeableItem> armorItems;

    // Start is called before the first frame update
    void Awake()
    {
        allItems = new Dictionary<string, TradeableItem>();
        foodItems = new Dictionary<string, TradeableItem>();
        armorItems = new Dictionary<string, TradeableItem>();

        LoadAndSortItems();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public TradeableItem GetItem(string itemName)
    {
        return allItems.ContainsKey(itemName) ? allItems[itemName] : null;
    }
    public List<TradeableItem> GetItemsByAttribue(TradeItemAttributes.ItemQueryAttributes attribute, int amount)
    {
        return GetItemsByAttribue(TradeItemAttributes.ItemTypes.all, attribute, amount);
    }
    public List<TradeableItem> GetItemsByAttribue(TradeItemAttributes.ItemTypes type, TradeItemAttributes.ItemQueryAttributes attribute, int amount)
    {
        List<TradeableItem> itemResult = new List<TradeableItem>();
        Dictionary<string, TradeableItem> dictionaryToQuery = new Dictionary<string, TradeableItem>();
        dictionaryToQuery = FillterDictionaryByType(dictionaryToQuery, type);

        switch (attribute)
        {
            case TradeItemAttributes.ItemQueryAttributes.neccesity:
                foreach (KeyValuePair<string, TradeableItem> entry in dictionaryToQuery)
                {
                    if (entry.Value.necessary)
                    {
                        itemResult.Add(entry.Value);
                    }
                }
                break;
           

        }
        return itemResult;
    }
    private Dictionary<string,TradeableItem> FillterDictionaryByType(Dictionary<string, TradeableItem> dictionaryToQuery,TradeItemAttributes.ItemTypes type)
    {
        switch (type)
        {
            case TradeItemAttributes.ItemTypes.all:
                dictionaryToQuery = allItems;
                break;
            case TradeItemAttributes.ItemTypes.food:
                dictionaryToQuery = foodItems;
                break;
            case TradeItemAttributes.ItemTypes.armor:
                dictionaryToQuery = armorItems;
                break;
            default:
                dictionaryToQuery = allItems;
                break;
        }
        return dictionaryToQuery;
    }

    /// <summary>
    /// Read the item data from json and assign it to a TradeableItem class and add it to the appropriate dictionary
    /// </summary>
    private void LoadAndSortItems()
    {
        JObject jsonText = JObject.Parse(File.ReadAllText(jsonPath));

        foreach (JProperty property in jsonText.Properties())
        {
            JObject itemAsJson = (JObject)property.Value;

            Texture2D texture = Resources.Load<Texture2D>("Sprites/RPG_inventory_icons/" + property.Name);
            Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
            string typeAsString = itemAsJson["itemType"].ToString();
            TradeItemAttributes.ItemTypes type;
            bool isParseSuccessful = Enum.TryParse(typeAsString, out type);
            if (!isParseSuccessful)
            {
                throw new Exception("Item type not found for " + property.Name);
            }
            bool necessary = itemAsJson["necessary"].Value<bool>();

            TradeableItem item = new TradeableItem(property.Name, float.Parse(itemAsJson["basePrice"].ToString()), necessary, type, sprite);

            allItems.Add(property.Name, item);
         
            switch (type)
            {
                case TradeItemAttributes.ItemTypes.food:
                    
                    foodItems.Add(property.Name,item);
                    break;
                case TradeItemAttributes.ItemTypes.armor:
                    armorItems.Add(property.Name,item);
                    break;
            }
        }
        

    }
}
