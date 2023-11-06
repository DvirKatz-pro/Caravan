using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using static EventObject;

public class JSONParser : SingletonManager<JSONParser>
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Open given json of NPC that contains the dialouge tree
    /// </summary>
    public void OpenJson(string jsonPath)
    {
        JObject jsonText = JObject.Parse(File.ReadAllText(jsonPath));
        if (jsonText.ContainsKey("Dialouge")) 
        {
            JObject dialouge = (JObject)jsonText["Dialouge"];
            if (dialouge != null)
            {
                EventObject headEvent = CreateEvents(dialouge,null);
            }
        }
    }

    public EventObject CreateEvents(JObject head, EventObject parentObject)
    {
        string text = null;
        string responseText = null;
        EventObject.EventActions action = EventObject.EventActions.None;
        List<EventObject> events = null;
        if (head.ContainsKey("Text"))
        {
            text = head["Text"].ToString();
        }
        if (head.ContainsKey("ResponseText"))
        {
            responseText = head["ResponseText"].ToString();
        }
        if (head.ContainsKey("Action")) 
        {
            action = (EventActions)Enum.Parse(typeof(EventActions), head["Action"].ToString(), ignoreCase: true);
        }
        JArray responsesArray = (JArray)head["Responses"];
        EventObject eventObject = new EventObject(text, responseText,parentObject, action);
        if (responsesArray != null && responsesArray.Count > 0)
        { 
            events = new List<EventObject>();
            for (int i = 0; i < responsesArray.Count; i++)
            {
                events.Add(CreateEvents((JObject)responsesArray[i], eventObject));
            }
        }
        eventObject.events = events;
        return eventObject;
    }
    /*
    public EventObject ParseJSON(JObject head, EventObject eventObject) {

        if (head.ContainsKey("Text"))
        {
            string text = head["Text"].ToString();
            string responseText = null;
            if (head.ContainsKey("ResponseText"))
            {
                responseText = head["ResponseText"].ToString();
            }

            JArray responsesArray = (JArray)head["Responses"];
            if (responsesArray != null && responsesArray.Count > 0)
            {
                for (int i = 0; i < responsesArray.Count; i++)
                {
                    responses.Add(ParseJSON((JObject)responsesArray[i], new List<EventObject>()));
                }
                return new EventObject(text, responseText, responses);
            }

        }
        else if (head.ContainsKey("ResponseText")) 
        {
            string responseText = head["ResponseText"].ToString();
            return new EventObject(null, responseText, null);
        }
        else if (head.SelectToken("Trade").Value<bool>())
        {

        }
        return null;
        

    }
    */
}
