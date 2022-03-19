using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseButton : MonoBehaviour
{


    public int responseNum { get; set; }
    public DialogueManager dialogueManager {get;set;}

    public void onClick()
    {
        dialogueManager.onChooseResponse(responseNum);
    }
}
