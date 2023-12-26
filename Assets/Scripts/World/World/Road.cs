using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Class that handles travel events
/// </summary>
public class Road : MonoBehaviour
{
    [SerializeField] private GameObject player;

    public GameObject fadeImage;
    private RoadManager roadManager;
    private RoadManager.RoadNames roadName;

    public Transform startPos;
    public Transform endPos;


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
        roadManager.InitializeTravel(this);
    }

    
    // called when the game is terminated
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public GameObject getPlayer()
    {
        return player;
    }
}
