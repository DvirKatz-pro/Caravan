using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that makes chosen item in the Inventory follow mouse after player click
/// </summary>
public class FollowMouse : MonoBehaviour
{
    private GameObject initialSlot;
    public void SetInitialSlot(GameObject initialSlot)
    {
        this.initialSlot = initialSlot;
    }
    public GameObject GetInitialSlot()
    {
        return initialSlot;
    }
    // Update is called once per frame
    private void Update()
    {
        transform.position = Input.mousePosition;
    }
}
