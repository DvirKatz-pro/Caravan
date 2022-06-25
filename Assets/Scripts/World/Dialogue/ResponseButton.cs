using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseButton : MonoBehaviour
{
    public int responseNum { get; set; }
    

    public void OnClick()
    {
        DialogueManager.Instance.OnChooseResponse(responseNum);
    }
}
