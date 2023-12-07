using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static StoryObject;

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

    private float buttonHeight;
    private StoryObject currentStory;
    private List<GameObject> buttons;
    private int chosenResponseNum = -1;


    // Start is called before the first frame update
    void Start()
    {
        parser = JSONParser.Instance;
        pauseControl = PauseControl.Instance;
        roadManager = RoadManager.Instance;
        compareEvents = new TravelEventComparer();
        events = new List<TravelEvent>();

        buttonHeight = ResponseButton.GetComponent<RectTransform>().sizeDelta.y;
        buttons = new List<GameObject>();

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
        OnParseEvent(travelEvent.storyHead);
    }

    private IEnumerator ResponseRoutine()
    {
        while (chosenResponseNum < 0)
        {
            yield return null;
        }
        int tempChosenResponse = chosenResponseNum;
        chosenResponseNum = -1;
        foreach (GameObject button in buttons)
        {
            Destroy(button);
        }
        buttons = new List<GameObject>();
        OnParseEvent(currentStory.stories[tempChosenResponse - 1]);
        yield break;
    }

    public void OnChooseResponse(int responseNum)
    {
        chosenResponseNum = responseNum;
    }

    public void OnParseEvent(StoryObject currentStory)
    {
        if (currentStory.text != null)
        {
            string text = currentStory.text.ToString();
            textObject.GetComponent<TMP_Text>().text = text;
        }
        else if (currentStory.specialStoryAction != null)
        {
            SpecialStoryAction specialStoryAction = currentStory.specialStoryAction;
            string text = specialStoryAction.actionText;
            textObject.GetComponent<TMP_Text>().text = text;
            currentStory = specialStoryAction.actionOutcomes[currentStory.specialStoryAction.rolledOutcome];
            
        }
        this.currentStory = currentStory;
        /*
        switch (currentStory.action)
        {
            case StoryObject.Actions.Trade:
                TradeManager.Instance.OpenTradeScreen(currentNPC);
                OnDisable();
                break;
            case StoryObject.Actions.Fight:
                break;
            case StoryObject.Actions.None:
                break;
            default:
                OnDisable();
                break;
        }
        */
        if (currentStory.stories != null && currentStory.stories.Count > 0)
        {
            List<StoryObject> stories = currentStory.stories;
            for (int i = 0; i < stories.Count; i++)
            {
                //Here we dynamiclly add buttons to UI depending on the amount of responses that exist
                GameObject button = Instantiate(ResponseButton, content.transform);
                Vector3 newButtonPos = buttonPosition;
                newButtonPos.y -= buttonHeight * i;
                button.GetComponent<RectTransform>().anchoredPosition = newButtonPos;
                string responseText = stories[i].responseText;
                if (responseText == null && stories[i].specialStoryAction != null) {
                    responseText = stories[i].specialStoryAction.specialAction.ToString();
                }
                button.transform.GetChild(0).GetComponent<Text>().text = responseText.ToString();
                ResponseButton responseButton = button.GetComponent<ResponseButton>();
                responseButton.responseNum = i + 1;
                responseButton.isDialogue = false;
                buttons.Add(button);
            }
            StartCoroutine(ResponseRoutine());
        }
        else
        { OnDisable(); }
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
    private void OnDisable()
    {
        if (!this.gameObject.scene.isLoaded) return;

        textObject.GetComponent<TMP_Text>().text = "";

        travelEventUI.SetActive(false);

        PauseControl.Instance.ResumeGame();
    }
}
