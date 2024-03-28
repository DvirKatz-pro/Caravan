using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Class Detailing Where the dialouge tree for this NPC is and to start dialouge when clicked on
/// </summary>
public class NPCDialogue : MonoBehaviour
{
    [SerializeField] private float dialogueDistance;
    [SerializeField] private string jsonName = "NPCText.json";

    private const string DIALOGUE_PATH = "Assets\\Resources\\NPCDialouge\\";

    public StoryObject headDialouge { get; set; }

    private GameObject player;

    public bool inDialogue { get; set; } = false;

    private void Awake()
    {
        if (headDialouge == null) {
            headDialouge = (StoryObject)JSONParser.Instance.OpenJsonDialougeTree(DIALOGUE_PATH + jsonName);
        }
        
        player = GameManager.Instance.GetPlayer();
    }

    private void Update()
    {
        float distance = Vector2.Distance(this.transform.position, player.transform.position);
        if (!inDialogue && distance <= dialogueDistance && headDialouge != null && Input.GetAxis("Submit") != 0)
        {
            inDialogue = true;
            DialogueManager.Instance.OnOpenDialogue(headDialouge,this.gameObject);
        }
    }
}
