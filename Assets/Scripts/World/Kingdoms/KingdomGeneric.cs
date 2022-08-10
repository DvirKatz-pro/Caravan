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
        int amountOfItems = Random.Range(5, 15);
        foreach (GameObject trader in traders)
        {
            NPCInventoryBreakdown breakdown = trader.GetComponent<NPCInventoryBreakdown>();
            if (breakdown != null)
            {
                Dictionary<EconomyManager.ItemTypes, float> precentages = breakdown.getInventoryBreakdown();
                if (precentages.ContainsKey(EconomyManager.ItemTypes.food))
                {
                    float percent = precentages[EconomyManager.ItemTypes.food];
                }
                else if (precentages.ContainsKey(EconomyManager.ItemTypes.armor))
                {
                    float percent = precentages[EconomyManager.ItemTypes.armor];
                }
            }
        }

    }
    private void AddToStock(List<TradeableItem> items, float percent, List<TradeableItem> stock)
    {
        
    }
}
