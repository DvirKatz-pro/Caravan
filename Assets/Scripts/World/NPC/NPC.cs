using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An NPC data Object used to keep track of an NPC 
/// </summary>
public class NPC
{
    public enum NPCType
    {
        Trader,
        Leader,
        None
    }
    private NPCType type;

    public int id { get; set; }
    public string NPCname { get; set; }

    public NPCInventoryBreakdown NPCInventoryBreakdown { get; set; }

    public StoryObject headDialouge { get; set;}

    public NPC(NPCType type, int id, string NPCname, NPCInventoryBreakdown NPCInventoryBreakdown, StoryObject headDialouge)
    {
        this.type = type;
        this.id = id;
        this.NPCname = NPCname;
        this.NPCInventoryBreakdown = NPCInventoryBreakdown;
        this.headDialouge = headDialouge;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
