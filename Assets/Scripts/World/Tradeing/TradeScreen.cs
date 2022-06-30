using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class Detailing Where clicked items in the trade screen are contained and displayed
/// </summary>
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
        items.Remove(game);
    }
    public List<GameObject> GetItems()
    {
        return items; 
    }
}
