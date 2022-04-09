using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotTriggerCollider : MonoBehaviour
{
    private InventoryManager inventoryManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        inventoryManager.setCurrentMouseOverSlot(gameObject);
    }

    public void setInventoryManager(InventoryManager inventoryManager)
    {
        this.inventoryManager = inventoryManager;
    }
}
