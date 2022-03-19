using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] private string jsonPath = "NPCText";
    [SerializeField] private DialogueManager manager;
    bool inDialouge = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnMouseDown()
    {
        if (!inDialouge)
        {
            manager.gameObject.SetActive(true);
           
            inDialouge = true;
            manager.openJson(jsonPath);
        }

    }
}
