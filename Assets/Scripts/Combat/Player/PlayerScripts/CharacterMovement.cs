using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    //directions that we can face

    public float movementSpeed = 1.0f;
   
    //components we need
    Rigidbody rb;
    CharacterAreaController controller;
    Animator animator;

    //terrain layer 
    private int floorMask;
    // Start is called before the first frame update
    void Start()
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

    

    /**
     * given a direction set the character to face that direction
     */
    /*
    void rotate(Vector2 direction)
    {
        Vector2 normalDir = direction.normalized;
       

        float angle = Vector2.SignedAngle(Vector2.up, direction);

        CharacterAreaController.Directions heading = controller.getDirection(angle);

        controller.setDirection(heading);
    }
    /*
    public void move()
    {
        Vector2 currentPos = rb.position;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");


        if (verticalInput != 0 && horizontalInput != 0)
        {
            //these are the grid cell bounds, that are applied to the vertical input so that the character walks in a straight line with the grid 
            verticalInput *= 5.45f / 9.3f;
        }

        //get move vector and move in that direction at the speed we set
        Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
        if (inputVector != Vector2.zero)
        {
            controller.changeState(CharacterAreaController.State.moveing);
        }
        else
        {
            controller.changeState(CharacterAreaController.State.idle);
        }
        inputVector = Vector2.ClampMagnitude(inputVector, 1);

        Vector2 movement = inputVector * movementSpeed;
        Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
        rb.MovePosition(newPos);

        if (movement.magnitude > 0.01f)
        {
            //rotate based on movement vector
            rotate(movement);
        }
    }
    */
    public void rotate()
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
   
    public void move()
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
            
            controller.changeState(CharacterAreaController.State.moveing);
            animator.SetBool("Idle", false);
            animator.SetBool("Moving", true);
        }
        //if neither "S" or "W" are hit then idle
        else
        {
            controller.changeState(CharacterAreaController.State.idle);
            animator.SetBool("Idle", true);
            animator.SetBool("Moving", false);
        }
        rb.velocity = movement;
        rotate();

    }






}
