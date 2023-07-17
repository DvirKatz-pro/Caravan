using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInputController : MonoBehaviour
{
    private Dictionary<KeyCode, List<HandleKeyDownInterface>> KeySubscribers;

    public void SubscribeToKey(KeyCode key, HandleKeyDownInterface component)
    {
        if (component.GetType().IsSubclassOf(typeof(HandleKeyDownInterface)))
        {
            if (KeySubscribers == null)
            {
                KeySubscribers = new Dictionary<KeyCode, List<HandleKeyDownInterface>>();
            }
            if (KeySubscribers[key] == null)
            {
                KeySubscribers[key] = new List<HandleKeyDownInterface>();
            }
            KeySubscribers[key].Add(component);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CheckKeys()
    {
        
    }
    
}
