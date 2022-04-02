using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject textObject;
    [SerializeField] private GameObject ResponseButton;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 buttonPosition;
    private float buttonHeight;
    JArray responses;
    private List<GameObject> buttons;
    private int chosenResponseNum = -1;
  
    
    // Start is called before the first frame update
    void Awake()
    {
        buttonHeight = ResponseButton.GetComponent<RectTransform>().sizeDelta.y;
        buttons = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onChooseResponse(int responseNum)
    {
        chosenResponseNum = responseNum;
       
    }

    private IEnumerator responseRoutine()
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
        parseDialouge((JObject)responses[tempChosenResponse - 1]);
        yield break;
    }
    private void parseDialouge(JObject responseObject)
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
              
                StartCoroutine(responseRoutine());
            }
           
        }
        else
        {
            OnDisable();
        }

    }
    private void OnDisable()
    {
        player.GetComponent<CharacterWorldController>().enabled = false;
        player.GetComponent<RunningTransition>().enabled = false;
        textObject.GetComponent<TMP_Text>().text = "";
        gameObject.SetActive(false);
    }

    public void openJson(string jsonPath)
    {
       

        JObject jsonText = JObject.Parse(File.ReadAllText(jsonPath));
        JObject dialouge = (JObject)jsonText["Dialouge"];
        
        if (dialouge != null)
        {

            parseDialouge(dialouge);
            
        }
       

    }

}
