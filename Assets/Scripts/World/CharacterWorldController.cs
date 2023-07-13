using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class Detailing Player Controller in 2D
/// </summary>
public class CharacterWorldController : MonoBehaviour
{
    [SerializeField] private GameObject inventoryCanvas;
    [SerializeField] private GameObject mapCanvas;
    private PlayerWorldMovement PlayerWorldMovement;
    private bool openInventory = false;
    private bool openMap = false;
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
        HandleKeyActions();


    }
    public void DisablePlayerActions()
    {
        PlayerWorldMovement.enabled = false;
    }
    public void EnablePlayerActions()
    {
        PlayerWorldMovement.enabled = true;
    }
    public void HandleKeyActions()
    {
        if (Input.GetKeyDown(KeyCode.I) && !openInventory)
        {
            openInventory = true;
            PauseControl.Instance.PauseGame();
            InventoryManager.Instance.OpenInventory();
            inventoryCanvas.SetActive(true);
            mapCanvas.SetActive(false);

        }
        else if (Input.GetKeyDown(KeyCode.I) && openInventory)
        {
            openInventory = false;
            PauseControl.Instance.ResumeGame();
            inventoryCanvas.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.M) && !openMap)
        {
            openMap = true;
            PauseControl.Instance.PauseGame();
            inventoryCanvas.SetActive(false);
            mapCanvas.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.M) && openMap)
        {
            openMap = false;
            PauseControl.Instance.ResumeGame();
            mapCanvas.SetActive(false);
        }
    }

}
