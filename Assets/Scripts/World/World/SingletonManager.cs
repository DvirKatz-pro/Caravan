using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton class for the easy creation of manager classes 
/// </summary>
public class SingletonManager<T> : MonoBehaviour where T: Component
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    GameObject instnaceObject = new GameObject();
                    _instance = instnaceObject.AddComponent<T>();
                }
            }
            return _instance;
        }
    }
    protected virtual void Awake()
    {
        _instance = this as T;
    }
}
