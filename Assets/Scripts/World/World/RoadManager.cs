using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that handles the roads connecting the towns
/// </summary>
public class RoadManager : SingletonManager<RoadManager>
{
    [SerializeField, Range(1, 100)] private float unFadeTimeSeconds;
    [SerializeField, Range(1, 100)] private float fadeTimeSeconds;
    private const string SIDE_SCROLL = "SideScroll";
    private GameObject fadeImage;
    public Road currentRoad { get; set; }
    public RoadNames currentRoadName { get; set; }

    public enum RoadNames
    {
        Road1
    }
    /// <summary>
    /// set the current road we are on
    /// </summary>
    public void InitializeTravel(Road road)
    {
        this.currentRoad = road;
        fadeImage = this.currentRoad.fadeImage;
        PauseControl.Instance.ResumeGame();
        StartCoroutine(FadeScreen(unFadeTimeSeconds, false));

    }
    public void EndTravel()
    {
        StartCoroutine(FadeScreen(fadeTimeSeconds, true));
    }

    public void OnTravelEnded()
    {
        StartCoroutine(LoadYourAsyncScene());
    }
    /// <summary>
    /// handles fading and unfading the screen based on if we are starting/ending travel
    /// </summary>
    private IEnumerator FadeScreen(float timeSeconds, bool fade)
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
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SIDE_SCROLL);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

    }
}
