using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTravel : MonoBehaviour
{
    [SerializeField] private float playerSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float xMovement = transform.position.x + playerSpeed * Time.deltaTime;
        transform.position = new Vector3(xMovement, transform.position.y, transform.position.z);
    }
}
