using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class Detailing Where the dialouge tree for this NPC is and to start dialouge when clicked on
/// </summary>
public class NPCDialogue : MonoBehaviour
{
    [SerializeField] private string jsonPath = "NPCText";
    private bool inDialouge = false;

    private void Awake()
    {
        if (!inDialouge && !PauseControl.gameIsPaused)
        {
            inDialouge = true;
            JSONParser.Instance.OpenJson(jsonPath);
        }

    }
    public void EndDialouge()
    {
        inDialouge = false;
    }
}
