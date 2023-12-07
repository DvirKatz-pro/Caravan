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
    private void Awake()
    {
        buttonHeight = ResponseButton.GetComponent<RectTransform>().sizeDelta.y;
        buttons = new List<GameObject>();
    }

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

    public void OnParseDialouge(StoryObject currentStory)
    {
        this.currentStory = currentStory;
        if (currentStory.text != null) 
        {
            string text = currentStory.text.ToString();
            textObject.GetComponent<TMP_Text>().text = text;
        }
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

    /// <summary>
    /// Parse a Json object that contains text, responses and Rest of dialouge tree
    /// </summary>
    /*
    private void ParseDialouge(JObject responseObject)
    {
        if (responseObject["Text"] != null)
        {
            string text = responseObject["Text"].ToString();
            textObject.GetComponent<TMP_Text>().text = text;
            responses = (JArray)responseObject["Responses"];
            if (responses != null && responses.Count > 0)
            {
                for (int i = 0; i < responses.Count; i++)
                {
                    //Here we dynamiclly add buttons to UI depending on the amount of responses that exist
                    GameObject button = Instantiate(ResponseButton, content.transform);
                    Vector3 newButtonPos = buttonPosition;
                    newButtonPos.y -= buttonHeight * i;
                    button.GetComponent<RectTransform>().anchoredPosition = newButtonPos;
                    button.transform.GetChild(0).GetComponent<Text>().text = responses[i]["ResponseText"].ToString();
                    ResponseButton responseButton = button.GetComponent<ResponseButton>();
                    responseButton.responseNum = i + 1;
                    buttons.Add(button);
                }

                StartCoroutine(ResponseRoutine());
            }

        }
        else if (responseObject.SelectToken("Trade").Value<bool>())
        {
            if (currentNPC.GetComponent<NPCInventory>() != null)
            {
                TradeManager.Instance.OpenTradeScreen(currentNPC);
            }
            OnDisable();
        }
        else
        {
            OnDisable();
        }

    }
    */
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
