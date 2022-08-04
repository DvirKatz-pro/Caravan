using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EconomyManager: SingletonManager<EconomyManager>
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
            allItems.Add(property.Name,new TradeableItem(name, float.Parse(itemAsJson["basePrice"].ToString()), sprite));
            string itemTyep = itemAsJson["type"].ToString();
            switch (itemTyep)
            {
                case "food":
                    foodItems.Add(property.Name, new TradeableItem(name, float.Parse(itemAsJson["basePrice"].ToString()), sprite));
                    break;
                case "armor":
                    armorItems.Add(property.Name, new TradeableItem(name, float.Parse(itemAsJson["basePrice"].ToString()), sprite));
                    break;
            }
        }

    }
}
