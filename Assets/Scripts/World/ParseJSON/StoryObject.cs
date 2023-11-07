using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryObject
{
    public string text { get; set; }
    public string responseText { get; set; }
    public StoryObject parentEvent { get; set; }
    public List<StoryObject> stories { get; set; }
    public EventActions action { get; set; }

    public enum EventActions
    {
        Trade,
        Fight,
        None
    }

    public StoryObject(string text, string responseText,StoryObject parentEvent,EventActions action, List<StoryObject> responses) {
        this.text = text;
        this.responseText = responseText;
        this.parentEvent = parentEvent;
        this.action = action;
        this.stories = responses;
    }

    public StoryObject(string text, string responseText, StoryObject parentEvent, EventActions action)
    {
        this.text = text;
        this.responseText = responseText;
        this.parentEvent = parentEvent;
        this.action = action;
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
