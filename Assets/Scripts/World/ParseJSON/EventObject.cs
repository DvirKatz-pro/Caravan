using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventObject
{
    string text;
    string responseText;
    List<EventObject> responses;
    public EventObject(string text, string responseText, List<EventObject> responses) {
        this.text = text;
        this.responseText = responseText;
        this.responses = responses;
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
