using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float cameraOffsetX;
    bool followPlayer = false;

    [SerializeField] private float cameraInitSpeed;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (followPlayer)
        {
            transform.position = new Vector3(player.position.x + cameraOffsetX, transform.position.y, transform.position.z);
        }
        else 
        {
            float xMovement = transform.position.x + cameraInitSpeed * Time.deltaTime;
            transform.position = new Vector3(xMovement, transform.position.y, transform.position.z);

            float intendedXPos = player.position.x + cameraOffsetX;
            if (!(transform.position.x <= intendedXPos))
            {
                followPlayer = true;
            }
        }
    }
}
