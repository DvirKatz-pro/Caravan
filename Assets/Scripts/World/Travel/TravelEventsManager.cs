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

    PauseControl pauseControl;
    RoadManager roadManager;

    private List<TravelEvent> events;
    TravelEventComparer compareEvents;



    // Start is called before the first frame update
    void Start()
    {
        pauseControl = PauseControl.Instance;
        roadManager = RoadManager.Instance;
        compareEvents = new TravelEventComparer();
        events = new List<TravelEvent>();

        float eventPosX = (roadManager.currentRoad.endPos.position.x +  roadManager.currentRoad.startPos.position.x) / 2.0f;
        float eventPosY = roadManager.currentRoad.startPos.position.y;
        Vector2 eventTrigger = new Vector2(eventPosX, eventPosY);
        AddEvent(eventTrigger);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (events.Count > 0 && player.transform.position.x >= events[0].triggerPos.x)
        {
            TriggerEvent(events[0]);
            events.RemoveAt(0);
        }
    }
    public void AddEvent(Vector2 triggerPos) 
    {
        events.Add(new TravelEvent(triggerPos));
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
