using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that makes chosen item follow mouse after player click
/// </summary>
public class FollowMouse : MonoBehaviour
{
    private GameObject initialSlot;
    public void setInitialSlot(GameObject initialSlot)
    {
        this.initialSlot = initialSlot;
    }
    public GameObject getInitialSlot()
    {
        return initialSlot;
    }
    // Update is called once per frame
    private void Update()
    {
        transform.position = Input.mousePosition;
    }
}
