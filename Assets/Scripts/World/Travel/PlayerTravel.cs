using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTravel : MonoBehaviour
{
    [SerializeField] private float playerSpeed;
    private RoadManager roadManager;
    bool endTravel = false;
    // Start is called before the first frame update
    void Start()
    {
        roadManager = RoadManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        float xMovement = transform.position.x + playerSpeed * Time.deltaTime;
        transform.position = new Vector3(xMovement, transform.position.y, transform.position.z);
        if (roadManager != null && roadManager.currentRoad != null)
        {
            Transform endpos = roadManager.currentRoad.endPos;
            if (transform.position.x >= endpos.position.x && !endTravel) 
            {
                endTravel = true;
                roadManager.currentRoad.EndTravel();
            }
        }
    }
}
