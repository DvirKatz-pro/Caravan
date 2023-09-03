using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventTreeManager : SingletonManager<EventTreeManager>
{
    public enum EventActions { 
        Trade,
        Fight
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OpenEventUI(EventObject eventObject) {
        PauseControl.Instance.PauseGame();
    }

    private void OnDisable()
    {
        /*if (!this.gameObject.scene.isLoaded) return;

        if (currentNPC != null)
        {
            currentNPC.GetComponent<NPCDialogue>().EndDialouge();
        }
        textObject.GetComponent<TMP_Text>().text = "";

        dialogeUI.SetActive(false);

        PauseControl.Instance.ResumeGame();*/
    }
}
