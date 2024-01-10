using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class representing a travel event with its StoryObject and trigger position
/// </summary>
public class TravelEvent
{

    public Vector2 triggerPos { get; set; }
    public StoryObject storyHead { get; set; }

    public TravelEvent(Vector2 triggerPos, StoryObject headStory)
    {
        this.triggerPos = triggerPos;
        this.storyHead = headStory;
    }
}
