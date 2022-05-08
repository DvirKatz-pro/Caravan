using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class That handles all of the inventory slots
/// </summary>
public class InventoryManager : MonoBehaviour
{
    [System.Serializable]
    public class InvnetorySlots
    {
        public List<GameObject> slots;
    }
    [SerializeField] private List<InvnetorySlots> slots;
    [SerializeField] private string jsonPath;

    // Start is called before the first frame update
    private void Start()
    {
        AddToInventory("apple");
        AddToInventory("armor");
    }

    // Update is called once per frame
    private void Update()
    {
    }
    /// <summary>
    /// Add item to inventory at first empty spot
    /// </summary>
    private void AddToInventory(string tradeableItem)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            for (int j = 0; j < slots[0].slots.Count; j++)
            {
                UISlot slot = slots[i].slots[j].GetComponent<UISlot>();
                
                if (slot.item == null)
                {
                    TradeableItem item = getItem(tradeableItem);
                    slots[i].slots[j].GetComponent<UISlot>().UpdateItem(item);
                    j = slots[i].slots.Count;
                    i = slots.Count;
                }
            }
        }
    }
    /// <summary>
    /// Read the item data from json
    /// </summary>
    private TradeableItem getItem(string name)
    {
        JObject jsonText = JObject.Parse(File.ReadAllText(jsonPath));
        JObject itemAsJson = (JObject)jsonText[name];

        Texture2D texture = Resources.Load<Texture2D>("Sprites/RPG_inventory_icons/" + name);
        Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        return new TradeableItem(name,float.Parse(itemAsJson["basePrice"].ToString()), sprite);
    }

   




}
