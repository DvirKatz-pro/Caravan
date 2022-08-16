using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class That handles all of the inventory slots of the player
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
    [SerializeField] private float startingFunds = 0;
    [SerializeField] private TextMeshProUGUI fundsText;
    private float currentFunds = 0;

    public const string FUNDS_TEXT = "funds: ";
    private float availableSpace = 0;

    TradeableItemsManager tradeItemManager;

    private void Start()
    {
        tradeItemManager = TradeableItemsManager.Instance;
        availableSpace = slots.Count * slots[0].slots.Count;
        currentFunds = startingFunds;
        AddToInventory("apple");
        AddToInventory("armor");
        AddToInventory("apple");
        AddToInventory("apple");
        AddToInventory("apple");
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
    #region Inventory Actions
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
                    TradeableItem item = tradeItemManager.GetItem(tradeableItem);
                    slots[i].slots[j].GetComponent<UISlot>().UpdateItem(item);
                    availableSpace--;
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
                    availableSpace--;
                    i = slots.Count;
                    j = slots[0].slots.Count;
                }
            }
        }
    }
    /// <summary>
    /// Remove the given item from the inventory if it exists
    /// </summary>
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
                    availableSpace++;
                    i = slots.Count;
                    j = slots[0].slots.Count;
                }
            }
        }
    }
    #endregion
    #region Get and extract Info
   
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
    public float GetAvailableSpace()
    {
        return availableSpace;
    }
    #endregion
}
