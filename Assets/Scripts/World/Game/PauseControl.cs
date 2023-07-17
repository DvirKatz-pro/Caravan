using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseControl : SingletonManager<PauseControl>
{
    public static bool gameIsPaused;
    void Update()
    {
    
    }
    public void PauseGame()
    {
        gameIsPaused = true;
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        gameIsPaused = false;
        Time.timeScale = 1;
    }
}
