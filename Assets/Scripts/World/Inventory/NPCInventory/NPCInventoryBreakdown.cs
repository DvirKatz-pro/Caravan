using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInventoryBreakdown
{
    public int inventorySize { get; set; }

    public int amountOfNecessary { get; set; }

    public float foodPercent { get; set; }

    public float armorPercent { get; set; }

    public List<TradeableItem> questItems { get; set; }

    // Start is called before the first frame update

    public NPCInventoryBreakdown(int inventorySize, int amountOfNecessary, float foodPercent, float armorPercent, List<TradeableItem> questItems) {
        this.inventorySize = inventorySize;
        this.amountOfNecessary = amountOfNecessary;
        this.foodPercent = foodPercent;
        this.armorPercent = armorPercent;
        this.questItems = questItems;
    }
   
}
