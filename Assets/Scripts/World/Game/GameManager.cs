using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonManager<GameManager>, ITimeSubscriber
{
    [SerializeField] private GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        TimeManager.Instance.RegisterSeason(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public GameObject GetPlayer()
    {
        return player;
    }

    void ITimeSubscriber.NotifyHour()
    {
        
    }

    void ITimeSubscriber.NotifyDay()
    {
        
    }

    void ITimeSubscriber.NotifySeason()
    {
        
    }

    void ITimeSubscriber.NotifyYear()
    {
        
    }
}
