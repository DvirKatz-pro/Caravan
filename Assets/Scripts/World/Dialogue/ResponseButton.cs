using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that is attached to a response button and given a response num and it tells the manager that 'this' button was clicked
/// sending back the response num identifier
/// </summary>
public class ResponseButton : MonoBehaviour
{
    public int responseNum { get; set; }
    

    public void OnClick()
    {
        DialogueManager.Instance.OnChooseResponse(responseNum);
    }
}
