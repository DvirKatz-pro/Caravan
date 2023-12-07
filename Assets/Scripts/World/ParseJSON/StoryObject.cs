using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StoryObject;

public class StoryObject
{
    public string text { get; set; }
    public string responseText { get; set; }
    public StoryObject parentEvent { get; set; }
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
