using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

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
    private JArray responses;
    private List<GameObject> buttons;
    private int chosenResponseNum = -1;
  
    
    // Start is called before the first frame update
    private void Awake()
    {
        buttonHeight = ResponseButton.GetComponent<RectTransform>().sizeDelta.y;
        buttons = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
        ParseDialouge((JObject)responses[tempChosenResponse - 1]);
        yield break;
    }
    /// <summary>
    /// Parse a Json object that contains text, responses and Rest of dialouge tree
    /// </summary>
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
                    responseButton.dialogueManager = this;
                    buttons.Add(button);
                }
              
                StartCoroutine(ResponseRoutine());
            }
           
        }
        else
        {
            OnDisable();
        }

    }
    private void OnDisable()
    {
        GameObject player = GameManager.Instance.GetPlayer();
        player.GetComponent<CharacterWorldController>().EnablePlayerActions();
        textObject.GetComponent<TMP_Text>().text = "";
        dialogeUI.SetActive(false);
    }

    public void OpenJson(string jsonPath)
    {   
        JObject jsonText = JObject.Parse(File.ReadAllText(jsonPath));
        JObject dialouge = (JObject)jsonText["Dialouge"];
        dialogeUI.SetActive(true);
        if (dialouge != null)
        {
            GameObject player = GameManager.Instance.GetPlayer();
            player.GetComponent<CharacterWorldController>().DisablePlayerActions();
            ParseDialouge(dialouge);
            
        }
    }

    

}
