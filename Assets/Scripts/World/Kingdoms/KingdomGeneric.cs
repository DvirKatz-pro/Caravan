using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingdomGeneric : MonoBehaviour
{
    [SerializeField] private GameObject Commontrader;
    [SerializeField] private GameObject Blacksmithtrader;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected void InitializeCommonStock(GameObject comman)
    {

        int amountOfItems = Random.Range(5, 15);

    }
}
