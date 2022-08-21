using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingdomGeneric : MonoBehaviour
{
    [SerializeField] private List<GameObject> traders;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected void InitializeStock()
    {
        int amountOfItems = Random.Range(10, 25);
        foreach (GameObject trader in traders)
        {
            NPCInventoryBreakdown breakdown = trader.GetComponent<NPCInventoryBreakdown>();
            if (breakdown != null)
            {
                Dictionary<TradeItemAttributes.ItemTypes, float> precentages = breakdown.getInventoryBreakdown();
                if (precentages.ContainsKey(TradeItemAttributes.ItemTypes.food))
                {
                    float percent = precentages[TradeItemAttributes.ItemTypes.food];
                    int amountOfFood = (int)(percent * amountOfItems);
                }
                if (precentages.ContainsKey(TradeItemAttributes.ItemTypes.armor))
                {
                    float percent = precentages[TradeItemAttributes.ItemTypes.armor];
                }
            }
        }

    }
    private void AddToStock(List<TradeableItem> items, float percent, List<TradeableItem> stock)
    {
        
    }
}
