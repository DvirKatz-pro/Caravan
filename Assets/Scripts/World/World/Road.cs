using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Road : MonoBehaviour
{
    RoadManager roadManager;
    RoadManager.RoadNames roadName;

    [SerializeField] private GameObject player;
    [SerializeField] private Transform startPos;
    public Transform endPos;

    [SerializeField] private GameObject fadeImage;
    [SerializeField, Range(1, 100)] private float unFadeTimeSeconds;
    [SerializeField, Range(1, 100)] private float fadeTimeSeconds;


    // Start is called before the first frame update
    // called first
    void OnEnable()
    {
        roadManager = RoadManager.Instance;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeTravel(roadManager.currentRoadName);
        roadManager.currentRoad = this;
    }

    
    // called when the game is terminated
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void InitializeTravel(RoadManager.RoadNames roadName) 
    {
        PauseControl.Instance.ResumeGame();
        StartCoroutine(fadeScreen(unFadeTimeSeconds,false));

    }
    public void EndTravel()
    {
        StartCoroutine(fadeScreen(fadeTimeSeconds, true));
    }
    private IEnumerator fadeScreen(float timeSeconds, bool fade)
    {
        CanvasGroup fadeCanvas = fadeImage.GetComponent<CanvasGroup>();
        float amountPerSec = 0;
        if (!fade)
        {
            amountPerSec = 1 / timeSeconds;
            while (fadeCanvas.alpha > 0)
            {
                fadeCanvas.alpha -= amountPerSec * Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            amountPerSec = 1 / timeSeconds;
            while (fadeCanvas.alpha < 1)
            {
                fadeCanvas.alpha += amountPerSec * Time.deltaTime;
                yield return null;
            }
        }
        
    }

    public GameObject getPlayer()
    {
        return player;
    }
}
