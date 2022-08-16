using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInventoryBreakdown : MonoBehaviour
{
    [Range(0.01f, 1)]
    [SerializeField] private float foodPercent;
    [Range(0.01f, 1)]
    [SerializeField] private float armorPercent;

    private Dictionary<TradeItemAttributes.ItemTypes, float> breakdown;

    // Start is called before the first frame update
    void Start()
    {
        breakdown = new Dictionary<TradeItemAttributes.ItemTypes, float>();
        breakdown[TradeItemAttributes.ItemTypes.food] = foodPercent;
        breakdown[TradeItemAttributes.ItemTypes.armor] = armorPercent;
    }

    public Dictionary<TradeItemAttributes.ItemTypes, float> getInventoryBreakdown()
    {
        return breakdown;
    }
}
