using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelEvent : MonoBehaviour
{
    //Needed Values
    private float buttonHeight;
    private JArray responses;
    private List<GameObject> buttons;
    private int chosenResponseNum = -1;

    public Vector2 triggerPos { get; set; }

    public TravelEvent(Vector2 triggerPos)
    {
        this.triggerPos = triggerPos;
    }



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
}
