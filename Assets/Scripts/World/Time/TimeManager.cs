using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : SingletonManager<TimeManager>
{
    [SerializeField] private float secondsToHours = 60;
    [SerializeField] private int daysInSeason = 30;
    public enum Seasons
    {
        Spring,
        Summer,
        Fall,
        Winter
    }

    public int currentYear { get; private set; } = 0;
    public Seasons currentSeason { get; private set; } = Seasons.Spring;
    public int currentDay { get; private set; } = 1;
    public int currentHour { get; private set; } = 8;
            
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PassageOfTime());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator PassageOfTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(secondsToHours);
            
            if (currentHour > 24)
            {
                currentHour = 0; 
                currentDay++;
                if (currentDay > daysInSeason)
                {
                    currentDay = 1;
                    currentSeason++;
                    if (currentSeason == Seasons.Winter)
                    {
                        currentYear++;
                        currentSeason = Seasons.Spring;
                    }
                } 
            }
            else
            {
                currentHour++;
            }
        }
    }
}
