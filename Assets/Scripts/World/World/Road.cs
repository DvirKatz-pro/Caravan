using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Road : MonoBehaviour
{
    RoadManager roadManager;
    RoadManager.Roads roadName;

    [SerializeField] private GameObject player;
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform endPos;

    [SerializeField] private GameObject fadeImage;
    [SerializeField, Range(1, 100)] float fadeTimeSeconds;


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
        initializeTravel(roadManager.currentRoad);
    }

    
    // called when the game is terminated
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void initializeTravel(RoadManager.Roads roadName) 
    {
        PauseControl.Instance.ResumeGame();
        StartCoroutine(unFadeScreen(fadeTimeSeconds));

    }
    private IEnumerator unFadeScreen(float fadeTimeSeconds)
    {
        CanvasGroup fadeCanvas = fadeImage.GetComponent<CanvasGroup>();
        float alpha = fadeCanvas.alpha;
        float amountPerSec = alpha / fadeTimeSeconds;
        while (alpha > 0)
        {
            fadeCanvas.alpha -= amountPerSec * Time.deltaTime;
            yield return null;
        }
    }

    public GameObject getPlayer()
    {
        return player;
    }
}
