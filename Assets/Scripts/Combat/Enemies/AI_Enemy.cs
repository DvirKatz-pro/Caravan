using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class AI_Enemy : MonoBehaviour
{
    private PathFinding path;
    private Transform tr;
    public GameObject Player;
    public Tilemap tileMap;
    public float speed;
    private GameObject playerPreviousPosition;
    private GameObject playerCurrentPosition;
    private GameObject currentNode;
    private GameObject previousNode;
    private List<GameObject> nodes;
    private int index = 0;
    private GameObject target;
    private GameObject previousTarget;
    private CharacterAreaController controller;

    public float health = 100;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Node")
        {
            
            currentNode = collision.gameObject;
            currentNode.GetComponent<SpriteRenderer>().enabled = false;
            if (previousNode == null)
            {
                previousNode = currentNode;
            }
            else if (currentNode != previousNode)
            {
                previousNode.GetComponent<SpriteRenderer>().enabled = true;
            }
            previousNode = currentNode;

        }
    }
    



    // Start is called before the first frame update
    void Start()
    {
        path = GetComponent<PathFinding>();
        tr = GetComponent<Transform>();
        controller = Player.GetComponent<CharacterAreaController>();

    }

    // Update is called once per frame
    void Update()
    {
        //if dead Destroy this gameobject
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
        Moving();
        
    }


    void Moving()
    {
        //get player position
       // playerCurrentPosition = controller.getCurrentNode();
       
        if (playerCurrentPosition != null && currentNode != null)
        {
            if (playerPreviousPosition != playerCurrentPosition)
            {
                //if the player moved, then enemy should keep moving torwords its target, only when the target is reached should a new one be obtained, this gives smoother movements to the AI
                if (target != null && Vector3.Distance(tr.position, target.transform.position) > 0.0f)
                {
                    Collider2D collider = target.GetComponent<Collider2D>();
                    collider.isTrigger = false;

                }
                else
                {
                    
                    nodes = path.FindPath(currentNode, playerCurrentPosition);
                    nodes.RemoveAt(nodes.Count - 1);
                    playerPreviousPosition = playerCurrentPosition;
                    index = 0;
                }

            }
         

            if (nodes != null)
            {
               
                target = nodes[index];
                if (previousTarget == null)
                {
                    previousTarget = target;
                }

                if (Vector3.Distance(tr.position, target.transform.position) > 0.0f)
                {
                    //prevent player or other AI collision
                    Collider2D collider = target.GetComponent<Collider2D>();
                    collider.isTrigger = false;
                   
                    transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

                    Vector3 targetDirection = target.transform.position - transform.position;
                    Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, speed * 5 * Time.deltaTime, 0.0f);
                    transform.rotation = Quaternion.LookRotation(newDirection);
                    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 90);
                }
                
                else
                {
                    index++;
                    if (index >= nodes.Count)
                    {
                        nodes = null;
                    }
                    
                    
                    
                }
               



            }
            if (target != null)
            {
                //only when the AI is moving away from its current position should the tile be unlocked for collision
                if (previousTarget != target && Vector3.Distance(tr.position, previousTarget.transform.position) > 0.0f && currentNode == target)
                {
                    
                    Collider2D collider = previousTarget.GetComponent<Collider2D>();
                    collider.isTrigger = true;
                    previousTarget = target;
                }
            }
           
        }
        
    }
    public GameObject getCurrentNode()
    {
        return currentNode;
    }
    public Vector2 getGridPosition()
    {
        return new Vector2(currentNode.GetComponent<Node>().getGridX(), currentNode.GetComponent<Node>().getGridY());
    }
}

