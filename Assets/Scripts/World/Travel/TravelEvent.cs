using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelEvent : MonoBehaviour
{
    
    public Vector2 triggerPos { get; set; }
    public StoryObject storyHead { get; set; }

    public TravelEvent(Vector2 triggerPos,StoryObject headStory)
    {
        this.triggerPos = triggerPos;
        this.storyHead = headStory;
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
