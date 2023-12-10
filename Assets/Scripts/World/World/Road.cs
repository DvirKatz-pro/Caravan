using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Road : MonoBehaviour
{
    private const string SIDE_SCROLL = "SideScroll";
    RoadManager roadManager;
    RoadManager.RoadNames roadName;

    [SerializeField] private GameObject player;
    public Transform startPos;
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

    public void OnTravelEnded()
    {
        StartCoroutine(LoadYourAsyncScene());
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
            OnTravelEnded();
        }
        
    }

    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.
        roadManager.currentRoadName = RoadManager.RoadNames.Road1;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SIDE_SCROLL);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

    }

    public GameObject getPlayer()
    {
        return player;
    }
}
