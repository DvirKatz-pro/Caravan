using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapButton : MonoBehaviour
{
    private const string TRAVEL = "Travel";
    private RoadManager roadManager;

    private Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(StartTravel);
        roadManager = RoadManager.Instance;
    }

    public void StartTravel()
    {
        StartCoroutine(LoadYourAsyncScene());

    }

    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.
        roadManager.currentRoad = RoadManager.Roads.Road1;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(TRAVEL);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

    }
}
