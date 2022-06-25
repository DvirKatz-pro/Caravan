using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class That handles all of the inventory slots
/// </summary>
public class InventoryManager : SingletonManager<InventoryManager>
{
    [System.Serializable]
    public class InvnetorySlots
    {
        public List<GameObject> slots;
    }

    [SerializeField] private List<InvnetorySlots> slots;
    [SerializeField] private string jsonPath;
    [SerializeField] private GameObject selectedObject;
    [SerializeField] private GameObject slotsContainer;
    [SerializeField] private GameObject inventoryCanvas;
    [SerializeField] private float startingFunds = 0;
    [SerializeField] private TextMeshProUGUI fundsText;
    private float currentFunds = 0;

    public const string FUNDS_TEXT = "funds: ";

    private void Awake()
    {
        currentFunds = startingFunds;
        AddToInventory("apple");
        AddToInventory("armor");
        AddToInventory("apple");
        AddToInventory("apple");
        AddToInventory("apple");
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        //Return item to inventory if click outside inventory bounds
        if (Input.GetMouseButtonDown(0) && selectedObject.GetComponent<UISlot>().item != null)
        {
            Vector3[] corners = new Vector3[4];
            slotsContainer.GetComponent<RectTransform>().GetWorldCorners(corners);
            Vector3 botLeft = corners[0];
            Vector3 topRight = corners[2];
            Vector3 mousePosition = Input.mousePosition;
            if (mousePosition.x < botLeft.x || mousePosition.x > topRight.x || mousePosition.y > topRight.y || mousePosition.y < botLeft.y)
            {
                UISlot selectedItem = selectedObject.GetComponent<UISlot>();
                TradeableItem clone = new TradeableItem(selectedItem.item);
                selectedItem.UpdateItem(null);
                AddToInventory(clone);
            }
        }
    }
    public void OpenInventory()
    {
        fundsText.text = InventoryManager.FUNDS_TEXT + currentFunds;
    }
    /// <summary>
    /// Add item to inventory at first empty spot given the Tradeable Item name
    /// </summary>
    public void AddToInventory(string tradeableItem)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            for (int j = 0; j < slots[0].slots.Count; j++)
            {
                UISlot slot = slots[i].slots[j].GetComponent<UISlot>();
                
                if (slot.item == null)
                {
                    TradeableItem item = GetItem(tradeableItem);
                    slots[i].slots[j].GetComponent<UISlot>().UpdateItem(item);
                    j = slots[i].slots.Count;
                    i = slots.Count;
                }
            }
        }
    }

    /// <summary>
    /// Add item to inventory at first empty spot given the Tradeable Item class
    /// </summary>
    public void AddToInventory(TradeableItem tradeableItem)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            for (int j = 0; j < slots[0].slots.Count; j++)
            {
                UISlot slot = slots[i].slots[j].GetComponent<UISlot>();

                if (slot.item == null)
                {
                    slots[i].slots[j].GetComponent<UISlot>().UpdateItem(tradeableItem);
                    j = slots[i].slots.Count;
                    i = slots.Count;
                }
            }
        }
    }
    public void RemoveFromInventory(TradeableItem tradeableItem)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            for (int j = 0; j < slots[0].slots.Count; j++)
            {
                UISlot slot = slots[i].slots[j].GetComponent<UISlot>();

                if (slot.item == tradeableItem)
                {
                    slots[i].slots[j].GetComponent<UISlot>().UpdateItem(null);
                    j = slots[i].slots.Count;
                    i = slots.Count;
                }
            }
        }
    }

    /// <summary>
    /// Read the item data from json
    /// </summary>
    public TradeableItem GetItem(string name)
    {
        JObject jsonText = JObject.Parse(File.ReadAllText(jsonPath));
        JObject itemAsJson = (JObject)jsonText[name];

        Texture2D texture = Resources.Load<Texture2D>("Sprites/RPG_inventory_icons/" + name);
        Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        return new TradeableItem(name,float.Parse(itemAsJson["basePrice"].ToString()), sprite);
    }
    public GameObject GetInventoryCanvas()
    {
        return inventoryCanvas;
    }
    public List<InvnetorySlots> GetInventorySlots()
    {
        return slots;
    }
    public float GetCurrentFunds()
    {
        return currentFunds;
    }
    public void SetCurrentFunds(float currentFunds)
    {
        this.currentFunds = currentFunds;

    }

}
