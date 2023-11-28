using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using static StoryObject;
using static StoryObject.SpecialStoryAction;

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
    public StoryObject OpenJson(string jsonPath)
    {
        StoryObject headEvent = null;
        JObject jsonText = JObject.Parse(File.ReadAllText(jsonPath));
        if (jsonText.ContainsKey("Dialouge")) 
        {
            JObject dialouge = (JObject)jsonText["Dialouge"];
            if (dialouge != null)
            {
                headEvent = CreateEvents(dialouge,null);
            }
        }
        return headEvent;
    }

    public StoryObject CreateEvents(JObject head, StoryObject parentObject)
    {
        string text = null;
        string responseText = null;
        StoryObject.Actions action = StoryObject.Actions.None;
        SpecialStoryAction specialAction = null;
        List<StoryObject> events = null;
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
            action = (Actions)Enum.Parse(typeof(Actions), head["Action"].ToString(), ignoreCase: true);
        }
        if (head.ContainsKey("SpecialActionObject"))
        {
            JObject specialActionObject = (JObject)head["SpecialActionObject"];
            StoryObject.SpecialActions specialActionAction = (SpecialActions)Enum.Parse(typeof(SpecialActions), specialActionObject["SpecialAction"].ToString(), ignoreCase: true);
            string specialActionText = specialActionObject["Text"].ToString();
            Dictionary<SpecialStoryAction.ActionOutcomes, Tuple<int, string>> actionOutcomes = new Dictionary<ActionOutcomes, Tuple<int, string>>();
            JObject options = (JObject)specialActionObject["Options"];
            string succsessText = options["Sucssess"].ToString();
            string failText = options["Fail"].ToString();
            string criticalFailText = options["CriticalFail"].ToString();

            int succsessChance = int.Parse(succsessText.Substring(succsessText.Length-3,2));
            int failChance = int.Parse(failText.Substring(failText.Length - 3, 2));
            int criticalFailChance = int.Parse(criticalFailText.Substring(criticalFailText.Length - 3,2));

            actionOutcomes.Add(SpecialStoryAction.ActionOutcomes.Succsess, new Tuple<int, string>(succsessChance, succsessText));
            actionOutcomes.Add(SpecialStoryAction.ActionOutcomes.Failure, new Tuple<int, string>(failChance, failText));
            actionOutcomes.Add(SpecialStoryAction.ActionOutcomes.CriticalFailure, new Tuple<int, string>(criticalFailChance, criticalFailText));

            specialAction = new SpecialStoryAction(specialActionAction,specialActionText, actionOutcomes);
        }
        JArray responsesArray = (JArray)head["Responses"];
        StoryObject eventObject = new StoryObject(text, responseText,parentObject, action, specialAction);
        if (responsesArray != null && responsesArray.Count > 0)
        { 
            events = new List<StoryObject>();
            for (int i = 0; i < responsesArray.Count; i++)
            {
                events.Add(CreateEvents((JObject)responsesArray[i], eventObject));
            }
        }
        eventObject.stories = events;
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
