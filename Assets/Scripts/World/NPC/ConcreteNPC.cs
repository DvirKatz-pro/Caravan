using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An NPC Object, Used to represent an NPC that currently exists in a scene
/// </summary>
public class ConcreteNPC : MonoBehaviour
{
    public NPC npcData { get; set; }

    [SerializeField] private int id;

    [SerializeField] private string NPCName;

    NPCManager npcManager;

    public void InitializeNPC()
    {
        npcManager = NPCManager.Instance;
        npcData = npcManager.GetNPCById(id);
    }
}
