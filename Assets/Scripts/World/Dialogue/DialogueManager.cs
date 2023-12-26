using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;

/// <summary>
/// Class that opens dialouge between NPC and player and handles player response
/// </summary>
public class DialogueManager : SingletonManager<DialogueManager>
{
    //Needed GameObjects
    [SerializeField] private GameObject dialogeUI;
    [SerializeField] private GameObject textObject;
    [SerializeField] private GameObject ResponseButton;
    [SerializeField] private GameObject content;
    [SerializeField] private Vector3 buttonPosition;

    //Needed Values
    private float buttonHeight;
    private StoryObject currentStory;
    private List<GameObject> buttons;
    private int chosenResponseNum = -1;

    private GameObject currentNPC; 
  
    
    // Start is called before the first frame update
    private new void Awake()
    {
        buttonHeight = ResponseButton.GetComponent<RectTransform>().sizeDelta.y;
        buttons = new List<GameObject>();
    }
    // <summary>
    /// Activate the Dialogue UI and parse the head storyObject
    /// </summary>
    public void OnOpenDialogue(StoryObject storyObject, GameObject currentNPC) 
    {
        this.currentNPC = currentNPC;
        dialogeUI.SetActive(true);
        PauseControl.Instance.PauseGame();
        OnParseDialouge(storyObject);
    }
   
    #region Dialogue and response parsing
    public void OnChooseResponse(int responseNum)
    {
        chosenResponseNum = responseNum;
    }
    /// <summary>
    /// Wait for player response and handle it once chosen
    /// </summary>
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
        OnParseDialouge(currentStory.stories[tempChosenResponse - 1]);
        yield break;
    }
    // <summary>
    /// Parse a single storyObject and display the appropriate text onscreen
    /// </summary>
    public void OnParseDialouge(StoryObject currentStory)
    {
        //get the basic text
        this.currentStory = currentStory;
        if (currentStory.text != null) 
        {
            string text = currentStory.text.ToString();
            textObject.GetComponent<TMP_Text>().text = text;
        }
        //if the story object holds an action, execute the action
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
        //if the story has follow up responses dynamically create buttons to represent them
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
                button.transform.GetChild(0).GetComponent<Text>().text = stories[i].responseText.ToString();
                ResponseButton responseButton = button.GetComponent<ResponseButton>();
                responseButton.responseNum = i + 1;
                responseButton.isDialogue = true;
                buttons.Add(button);
            }
            StartCoroutine(ResponseRoutine());
        }
        else
        { OnDisable(); }

    }

    #endregion
    private void OnDisable()
    {
        if (!this.gameObject.scene.isLoaded) return;

        textObject.GetComponent<TMP_Text>().text = "";

        if (currentNPC != null)
        {
            currentNPC.GetComponent<NPCDialogue>().inDialogue = false;
        }
        dialogeUI.SetActive(false);
        
        PauseControl.Instance.ResumeGame();
    }
}
