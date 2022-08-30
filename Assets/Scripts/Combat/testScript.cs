using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float movementSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var cc = GetComponent<CharacterController>();
        var offset = player.transform.position - transform.position;
        //Get the difference.
        if (offset.magnitude > .1f)
        {
            //If we're further away than .1 unit, move towards the target.
            //The minimum allowable tolerance varies with the speed of the object and the framerate. 
            // 2 * tolerance must be >= moveSpeed / framerate or the object will jump right over the stop.
            offset = offset.normalized * movementSpeed;
            //normalize it and account for movement speed.
            cc.Move(offset * Time.deltaTime);
            //actually move the character.
        }
    }
}
