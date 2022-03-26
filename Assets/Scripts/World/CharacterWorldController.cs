using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWorldController : MonoBehaviour
{
    PlayerWorldMovement PlayerWorldMovement;
    [SerializeField] private GameObject inventoryCanvas;
    private bool openInventory = false;
    // Start is called before the first frame update
    void Start()
    {
        PlayerWorldMovement = GetComponent<PlayerWorldMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !openInventory)
        {
            openInventory = true;
            inventoryCanvas.SetActive(true);

        }
        else if (Input.GetKeyDown(KeyCode.I) && openInventory)
        {
            openInventory = false;
            inventoryCanvas.SetActive(false);
        }
    }
    public void disablePlayerActions()
    {
        PlayerWorldMovement.enabled = false;
    }
    public void enablePlayerActions()
    {
        PlayerWorldMovement.enabled = true;
    }


}
