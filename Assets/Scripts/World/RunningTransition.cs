using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningTransition : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        //if moving set Running parameter to true otherwise false
        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput != 0)
        {
            anim.SetBool("Running", true);
        }
        else
        {
            anim.SetBool("Running", false);
        }
    }
}
