using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StoryObject;

/// <summary>
/// A class representing a Story object, it contains all needed objects for a dialouge tree or Travel Event Tree
/// </summary>
public class StoryObject
{
    //the main text that displays at the top
    public string text { get; set; }
    //the text to appear as part of a response button
    public string responseText { get; set; }
    //the event that called this event
    public StoryObject parentEvent { get; set; }
    //list of responses
    public List<StoryObject> stories { get; set; }
    public Actions action { get; set; }

    public SpecialStoryAction specialStoryAction {get; set;}

    public enum Actions
    {
        Trade,
        Fight,
        None
    }

    public enum SpecialActions
    {
        Intimidate,
        Convince
    }

    public StoryObject(string text, string responseText,StoryObject parentEvent,Actions action, SpecialStoryAction specialStoryAction, List<StoryObject> responses) {
        this.text = text;
        this.responseText = responseText;
        this.parentEvent = parentEvent;
        this.action = action;
        this.specialStoryAction = specialStoryAction;
        this.stories = responses;
    }

    public StoryObject(string text, string responseText, StoryObject parentEvent, Actions action)
    {
        this.text = text;
        this.responseText = responseText;
        this.parentEvent = parentEvent;
        this.action = action;
    }

    public class SpecialStoryAction 
    {
        public SpecialActions specialAction { get; set; }
        public string actionText { get; set; }
        public Dictionary<ActionOutcomes, StoryObject> actionOutcomes { get; set; }

        public ActionOutcomes rolledOutcome { get; set; }

        public enum ActionOutcomes
        {
            Succsess,
            Failure,
            CriticalFailure
        }
        public SpecialStoryAction(SpecialActions specialAction, string actionText, Dictionary<ActionOutcomes, StoryObject> actionOutcomes,ActionOutcomes rolledOutcome)
        {
            this.specialAction = specialAction;
            this.actionText = actionText;
            this.actionOutcomes = actionOutcomes;
            this.rolledOutcome = rolledOutcome;
        }
    }
}
