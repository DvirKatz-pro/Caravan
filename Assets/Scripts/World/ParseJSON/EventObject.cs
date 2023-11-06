using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventObject
{
    public string text { get; set; }
    public string responseText { get; set; }
    public EventObject parentEvent { get; set; }
    public List<EventObject> events { get; set; }
    public EventActions action { get; set; }

    public enum EventActions
    {
        Trade,
        Fight,
        None
    }

    public EventObject(string text, string responseText,EventObject parentEvent,EventActions action, List<EventObject> responses) {
        this.text = text;
        this.responseText = responseText;
        this.parentEvent = parentEvent;
        this.action = action;
        this.events = responses;
    }

    public EventObject(string text, string responseText, EventObject parentEvent, EventActions action)
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
