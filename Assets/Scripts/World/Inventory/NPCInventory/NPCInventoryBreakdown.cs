using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInventoryBreakdown : MonoBehaviour
{
    [Range(0.01f, 1)]
    [SerializeField] private float foodPercent;
    [Range(0.01f, 1)]
    [SerializeField] private float armorPercent;

    private Dictionary<EconomyManager.ItemTypes, float> breakdown;

    // Start is called before the first frame update
    void Start()
    {
        breakdown = new Dictionary<EconomyManager.ItemTypes, float>();
        breakdown[EconomyManager.ItemTypes.food] = foodPercent;
        breakdown[EconomyManager.ItemTypes.armor] = armorPercent;
    }

    public Dictionary<EconomyManager.ItemTypes, float> getInventoryBreakdown()
    {
        return breakdown;
    }
}
