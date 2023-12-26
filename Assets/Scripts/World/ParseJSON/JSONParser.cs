using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using static StoryObject;
using static StoryObject.SpecialStoryAction;

/// <summary>
/// A class to parse a JSON into storyObject Tree to be used in Dialouge or Travel Events
/// </summary>
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
                headEvent = CreateStoryObjects(dialouge,null);
            }
        }
        return headEvent;
    }
    /// <summary>
    /// given a head story object, create the story object tree and recursevly traverse the json tree
    /// </summary>
    public StoryObject CreateStoryObjects(JObject head, StoryObject parentObject)
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
        StoryObject eventObject = new StoryObject(text, responseText, parentObject, action);
        if (head.ContainsKey("SpecialActionObject"))
        {
            // and create a special story object inside the StoryObject class
            JObject specialActionObject = (JObject)head["SpecialActionObject"];
            StoryObject.SpecialActions specialActionAction = (SpecialActions)Enum.Parse(typeof(SpecialActions), specialActionObject["SpecialAction"].ToString(), ignoreCase: true);
            string specialActionText = specialActionObject["Text"].ToString();
            //get the type of special action
            Dictionary<SpecialStoryAction.ActionOutcomes, StoryObject> actionOutcomes = new Dictionary<ActionOutcomes, StoryObject>();
            JObject options = (JObject)specialActionObject["Options"];
            int succsess = (int)options["SucssessChance"];
            int fail = (int)options["FailChance"];
            int criticalFail = (int)options["CriticalFailChance"];

            //alculate the chance of each outcome of the action
            int chosenRoll = UnityEngine.Random.Range(0, 100);

            SpecialStoryAction.ActionOutcomes chosenOutcome = SpecialStoryAction.ActionOutcomes.Succsess;

            if (chosenRoll <= fail) {
                chosenOutcome = SpecialStoryAction.ActionOutcomes.Failure;
            }
            else if(chosenRoll > fail && chosenRoll <= fail + criticalFail )
            {
                chosenOutcome = SpecialStoryAction.ActionOutcomes.CriticalFailure;
            }


            //recursvly traverse the storyObject tree of each outcome
            actionOutcomes.Add(SpecialStoryAction.ActionOutcomes.Succsess, CreateStoryObjects((JObject)options["Sucssess"], eventObject));
            actionOutcomes.Add(SpecialStoryAction.ActionOutcomes.Failure, CreateStoryObjects((JObject)options["Fail"], eventObject));
            actionOutcomes.Add(SpecialStoryAction.ActionOutcomes.CriticalFailure, CreateStoryObjects((JObject)options["CriticalFail"], eventObject));
            //create a new specialStoryAction inside the StoryObject class
            specialAction = new SpecialStoryAction(specialActionAction,specialActionText, actionOutcomes,chosenOutcome);
        }
        //recursvly traverse the storyObject tree of each response
        JArray responsesArray = (JArray)head["Responses"];
        if (responsesArray != null && responsesArray.Count > 0)
        { 
            events = new List<StoryObject>();
            for (int i = 0; i < responsesArray.Count; i++)
            {
                events.Add(CreateStoryObjects((JObject)responsesArray[i], eventObject));
            }
        }
        eventObject.specialStoryAction = specialAction;
        eventObject.stories = events;
        return eventObject;
    }
}
