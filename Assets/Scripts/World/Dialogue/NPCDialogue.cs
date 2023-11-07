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
    [SerializeField] private string jsonPath = "NPCText";

    public StoryObject headDialouge { get; set; }

    private GameManager gameManager;
    private GameObject player;

    public bool inDialogue { get; set; } = false;

    private void Awake()
    {
        headDialouge = JSONParser.Instance.OpenJson(jsonPath);
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
