using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeManager : SingletonManager<TimeManager>
{
    [SerializeField] private float secondsToHours = 60;
    [SerializeField] private int daysInSeason = 30;
    [SerializeField] private TMP_Text timerText;
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

    private List<ITimeSubscriber> hourSubscribers = new List<ITimeSubscriber>();
    private List<ITimeSubscriber> daySubscribers = new List<ITimeSubscriber>();
    private List<ITimeSubscriber> seasonSubscribers = new List<ITimeSubscriber>();
    private List<ITimeSubscriber> yearSubscribers = new List<ITimeSubscriber>();

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
            timerText.text = "H: " + currentHour + " D: " + currentDay + " S: " + currentSeason + " Y: " + currentYear;
            yield return new WaitForSeconds(secondsToHours);
            
            if (currentHour >= 24)
            {
                currentHour = 0; 
                currentDay++;
                OnHourChanged();
                OnDayChanged();
                if (currentDay > daysInSeason)
                {
                    currentDay = 1;
                    currentSeason++;
                    OnSeasonChanged();
                    if (currentSeason == Seasons.Winter)
                    {
                        currentYear++;
                        OnYearChanged();
                        currentSeason = Seasons.Spring;
                    }
                } 
            }
            else
            {
                currentHour++;
                OnHourChanged();
            }
        }
    }

    public void OnHourChanged()
    {
        foreach (ITimeSubscriber subscriber in hourSubscribers)
        {
            subscriber.NotifyTime();
        }
    }
    public void OnDayChanged()
    {
        foreach (ITimeSubscriber subscriber in daySubscribers)
        {
            subscriber.NotifyTime();
        }
    }
    public void OnSeasonChanged()
    {
        foreach (ITimeSubscriber subscriber in seasonSubscribers)
        {
            subscriber.NotifyTime();
        }
    }
    public void OnYearChanged()
    {
        foreach (ITimeSubscriber subscriber in yearSubscribers)
        {
            subscriber.NotifyTime();
        }
    }

    public void RegisterHour(ITimeSubscriber subscriber)
    {
        hourSubscribers.Add(subscriber);
    }

    public void RegisterDay(ITimeSubscriber subscriber)
    {
        daySubscribers.Add(subscriber);
    }

    public void RegisterSeason(ITimeSubscriber subscriber)
    {
        seasonSubscribers.Add(subscriber);
    }

    public void RegisterYear(ITimeSubscriber subscriber)
    {
        yearSubscribers.Add(subscriber);
    }

    public void UnregisterHour(ITimeSubscriber subscriber)
    {
        hourSubscribers.Remove(subscriber);
    }

    public void UnregisterDay(ITimeSubscriber subscriber)
    {
        daySubscribers.Remove(subscriber);
    }
    public void Unregisterseason(ITimeSubscriber subscriber)
    {
        seasonSubscribers.Remove(subscriber);
    }
    public void UnregisterYear(ITimeSubscriber subscriber)
    {
        yearSubscribers.Remove(subscriber);
    }
}
