using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class Detailing Where the dialouge tree for this NPC is
/// </summary>
public class NPCDialogue : MonoBehaviour
{
    [SerializeField] private string jsonPath = "NPCText";
    private bool inDialouge = false;

    private void OnMouseDown()
    {
        if (!inDialouge && !PauseControl.gameIsPaused)
        {
            inDialouge = true;
            DialogueManager.Instance.OpenJson(jsonPath);
        }

    }
}
