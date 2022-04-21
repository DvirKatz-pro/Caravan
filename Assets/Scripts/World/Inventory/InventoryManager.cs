using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [System.Serializable]
    public class InvnetorySlots
    {
        public List<GameObject> slots;
    }
    [SerializeField] private List<InvnetorySlots> slots;
    [SerializeField] private string jsonPath;
    [SerializeField] private string spritePath;


    // Start is called before the first frame update
    void Start()
    {
        addToInventory("apple");
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void addToInventory(string tradeableItem)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            for (int j = 0; j < slots[0].slots.Count; j++)
            {
                UISlot slot = slots[i].slots[j].GetComponent<UISlot>();
                
                if (slot.item == null)
                {
                    TradeableItem item = getItem(tradeableItem);
                    slots[i].slots[j].GetComponent<UISlot>().updateItem(item);
                    j = slots[i].slots.Count;
                    i = slots.Count;
                }
            }
        }
    }

    private TradeableItem getItem(string name)
    {
        JObject jsonText = JObject.Parse(File.ReadAllText(jsonPath));
        JObject itemAsJson = (JObject)jsonText[name];

        Texture2D texture = Resources.Load<Texture2D>("Sprites/RPG_inventory_icons/apple");
        Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        return new TradeableItem(name,float.Parse(itemAsJson["basePrice"].ToString()), sprite);
    }


}
