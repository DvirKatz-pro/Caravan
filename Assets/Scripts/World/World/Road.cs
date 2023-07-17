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

    [SerializeField] private Transform cameraPos;
    [SerializeField] private Vector2 cameraOffset;
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
        Debug.Log(roadName);
    }

    public GameObject getPlayer()
    {
        return player;
    }
}
