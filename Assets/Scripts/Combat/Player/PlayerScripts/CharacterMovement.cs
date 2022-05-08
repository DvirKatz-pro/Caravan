using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Class Detailing Player movement and rotation
/// </summary>
public class CharacterMovement : MonoBehaviour
{
    public float movementSpeed = 1.0f;
   
    //components we need
    private Rigidbody rb;
    private CharacterAreaController controller;
    private Animator animator;

    //terrain layer 
    private int floorMask;
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("RigidBody is null");
        }
        controller = GetComponent<CharacterAreaController>();
        if (controller == null)
        {
            Debug.LogError("Player combat controller is null");
        }
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Player animator not found");
        }
        floorMask = LayerMask.GetMask("Terrain");
    }

    

   

    public void Rotate()
    {
        //get the mouse point on screen
        Vector3 lookDir = Vector3.zero;
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(mouseRay, out hit, Mathf.Infinity))
        {
            //calculate direction between player and mouse


            lookDir = hit.point - transform.position;

            // Ensure the vector is entirely along the floor plane.
            lookDir.y = 0f;

            // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
            if (Vector3.Distance(transform.position, hit.point) > 1f)
            {
                //Quaternion newRotation = new Quaternion();
                // newRotation.SetLookRotation(lookDir);
                Quaternion newRotation = Quaternion.LookRotation(lookDir);

                // Set the player's rotation to this new rotation.
                //rigidB.MoveRotation(newRotation);
                this.transform.rotation = newRotation;

            }

        }
        
    }
   
    public void Move()
    {
        Vector3 currentPos = transform.position;
        
        //find the mouse pointer and move to that location
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 movement = Vector3.zero;
        if (Physics.Raycast(mouseRay, out hit, Mathf.Infinity, floorMask))
        {

            if (movement.y > 0)
            {
                movement.y = 0;
            }

            //if "W" hit move forword
            if (Input.GetKey(KeyCode.W) && Vector3.Distance(this.transform.position, hit.point) > 0.0f)
            {
                movement = transform.forward * movementSpeed * Time.fixedDeltaTime;
            }
            //if "S" hit move backword
            else if (Input.GetKey(KeyCode.S) && Vector3.Distance(this.transform.position, hit.point) > 0.0f)
            {
                movement = (transform.forward * -1) * movementSpeed * Time.fixedDeltaTime;
               
            }
        }
        
        if (movement != Vector3.zero)
        {
            
            controller.ChangeState(CharacterAreaController.State.moveing);
            animator.SetBool("Idle", false);
            animator.SetBool("Moving", true);
        }
        //if neither "S" or "W" are hit then idle
        else
        {
            controller.ChangeState(CharacterAreaController.State.idle);
            animator.SetBool("Idle", true);
            animator.SetBool("Moving", false);
        }
        rb.velocity = movement;
        Rotate();

    }






}
