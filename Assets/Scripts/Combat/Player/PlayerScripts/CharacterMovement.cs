using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Class Detailing Player movement and rotation
/// </summary>
public class CharacterMovement : MonoBehaviour
{
    public float movementSpeed = 1.0f;
    private bool canMove { get; set; }
   
    //components we need
    private CharacterController charController;
    private CharacterAreaController controller;
    private Animator animator;
    private NavMeshAgent agent;

    //terrain layer 
    private int floorMask;
    // Start is called before the first frame update
    private void Start()
    {
        charController = GetComponent<CharacterController>();
        controller = GetComponent<CharacterAreaController>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        floorMask = LayerMask.GetMask("Terrain");
        canMove = true;
    }




    #region Rotate and movement
    /// <summary>
    /// rotate player forword to mouse
    /// </summary>
    public void RotateToMouse()
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
        
        if (!canMove)
        {
            return;
        }

        /*
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
        */
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        //Rotate our movement vector by 45 degrees because our camera is at a 45 degree angle (isomentric)
        movement = Quaternion.AngleAxis(45, Vector3.up) * movement;
        movement.y = 0;
        Debug.Log(Time.deltaTime);
        agent.velocity = movement * movementSpeed * Time.deltaTime;
        if (movement != Vector3.zero)
        {
            gameObject.transform.forward = movement;
            controller.ChangeState(CharacterAreaController.State.moveing);
            animator.SetBool("Idle", false);
            animator.SetBool("Moving", true);
        }
        //if no movement then idle
        else
        {
            controller.ChangeState(CharacterAreaController.State.idle);
            animator.SetBool("Idle", true);
            animator.SetBool("Moving", false);
        }
        //Rotate();

    }
    /// <summary>
    /// given a movement direction the player will move a certain distance in a certain amount of time
    /// </summary>
    public IEnumerator MoveOverTime(Vector3 movementDirection, float distance,float movementDuration)
    {
        yield return null;
        Vector3 startValue = transform.position;
        Vector3 endValue = transform.position + movementDirection.normalized * distance;
        float timeElapsed = 0;
        Vector3 valueToLerp;
        canMove = false;
        while (timeElapsed < movementDuration)
        {
            valueToLerp = Vector3.Lerp(startValue, endValue, timeElapsed / movementDuration);
            agent.velocity = valueToLerp - transform.position;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        valueToLerp = endValue;
        agent.velocity = valueToLerp - transform.position;
        yield return null;
        canMove = true;
    }
    #endregion





}
