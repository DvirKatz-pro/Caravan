using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class Detailing Player Controller in 2D
/// </summary>
public class CharacterWorldController : MonoBehaviour
{
    [SerializeField] private GameObject inventoryCanvas;
    private PlayerWorldMovement PlayerWorldMovement;
    private bool openInventory = false;
    // Start is called before the first frame update
    private void Start()
    {
        PlayerWorldMovement = GetComponent<PlayerWorldMovement>();
    }

    // Update is called once per frame
    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput != 0) {
            PlayerWorldMovement.Move(horizontalInput);
        }
        else if (Input.GetKeyDown(KeyCode.I) && !openInventory)
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
    public void DisablePlayerActions()
    {
        PlayerWorldMovement.enabled = false;
    }
    public void EnablePlayerActions()
    {
        PlayerWorldMovement.enabled = true;
    }


}
