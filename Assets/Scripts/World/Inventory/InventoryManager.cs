using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [System.Serializable]
    public class InvnetorySlots
    {
        public List<GameObject> slots;
    }
    [SerializeField] private List<InvnetorySlots> slots;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void addToInventory(GameObject tradeableItem)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            for (int j = 0; j < slots[i].slots.Count; j++)
            {
                GameObject currentSlot = slots[i].slots[j];
                if (currentSlot.name.Contains("Empty"))
                {
                    slots[i].slots[j] = tradeableItem;
                }
            }
        }
    }
}
