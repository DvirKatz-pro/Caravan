using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that automatically moves the traveling player
/// </summary>
public class PlayerTravel : MonoBehaviour
{
    [SerializeField] private float playerSpeed;

    private RoadManager roadManager;
    private Transform endpos;
    private bool endTravel = false;
    // Start is called before the first frame update
    void Start()
    {
        roadManager = RoadManager.Instance;
        endpos = roadManager.currentRoad.endPos;
    }

    // Update is called once per frame
    void Update()
    {
        float xMovement = transform.position.x + playerSpeed * Time.deltaTime;
        transform.position = new Vector3(xMovement, transform.position.y, transform.position.z);
        if (roadManager != null && roadManager.currentRoad != null)
        {
            //when the player reaches the end point we end the travel
            if (transform.position.x >= endpos.position.x && !endTravel) 
            {
                endTravel = true;
                roadManager.EndTravel();
            }
        }
    }
}
