using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeScreen : MonoBehaviour
{
    private List<GameObject> items;
    private Vector2 currentPos;
    private Vector2 slotSize;

    private void Start()
    {
        items = new List<GameObject>();
    }
    public void AddItem(GameObject slot)
    {
        if (slotSize == Vector2.zero)
        {
            slotSize = slot.GetComponent<RectTransform>().sizeDelta;
        }

        slot.GetComponent<RectTransform>().anchoredPosition = currentPos;
        items.Add(slot);
    }
    public void RemoveItem(GameObject game)
    {
        int itemIndex = -1;
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == game)
            {
                itemIndex = i;
                break;
            }
        }
        if (itemIndex < 0)
        {
            throw new System.Exception("item: " + game.name + " not found in buy or sell screen");
        }
        items.RemoveAt(itemIndex);
    }
    public List<GameObject> GetItems()
    {
        return items; 
    }
}
