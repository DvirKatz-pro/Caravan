using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITimeSubscriber
{
    public void NotifyHour();
    public void NotifyDay();
    public void NotifySeason();
    public void NotifyYear();
}