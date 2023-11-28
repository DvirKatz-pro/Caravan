using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelEventsManager : SingletonManager<TravelEventsManager>
{
    [SerializeField] private GameObject player;
    //Needed GameObjects
    [SerializeField] private GameObject travelEventUI;
    [SerializeField] private GameObject textObject;
    [SerializeField] private GameObject ResponseButton;
    [SerializeField] private GameObject content;
    [SerializeField] private Vector3 buttonPosition;
    [SerializeField] private string jsonName = "AmbushEvent.json";

    JSONParser parser;
    PauseControl pauseControl;
    RoadManager roadManager;

    private const string TRAVEL_EVENT_PATH = "Assets\\Resources\\TravelEvents\\";

    private List<TravelEvent> events;
    TravelEventComparer compareEvents;



    // Start is called before the first frame update
    void Start()
    {
        parser = JSONParser.Instance;
        pauseControl = PauseControl.Instance;
        roadManager = RoadManager.Instance;
        compareEvents = new TravelEventComparer();
        events = new List<TravelEvent>();

        float eventPosX = (roadManager.currentRoad.endPos.position.x +  roadManager.currentRoad.startPos.position.x) / 2.0f;
        float eventPosY = roadManager.currentRoad.startPos.position.y;
        StoryObject headStory = parser.OpenJson(TRAVEL_EVENT_PATH + jsonName);
        TravelEvent travelEvent = new TravelEvent(new Vector2(eventPosX, eventPosY),headStory);
        AddEvent(travelEvent);
    }

    // Update is called once per frame
    void Update()
    {
        if (events.Count > 0 && player.transform.position.x >= events[0].triggerPos.x)
        {
            TriggerEvent(events[0]);
            events.RemoveAt(0);
        }
    }
    public void AddEvent(TravelEvent travelEvent) 
    {
        events.Add(travelEvent);
        events.Sort(compareEvents);
    }

    public void TriggerEvent(TravelEvent travelEvent)
    {
        travelEventUI.SetActive(true);
        pauseControl.PauseGame();
    }

    public class TravelEventComparer : IComparer<TravelEvent> 
    {
        public int Compare(TravelEvent a, TravelEvent b)
        {
            if (a.triggerPos.x == b.triggerPos.x) return 0;
            else if (a.triggerPos.x > b.triggerPos.x) return 1;
            return -1;
        }
    }
}
