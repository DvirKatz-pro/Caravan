using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using UnityEditorInternal;

public class CharacterAttacks : MonoBehaviour
{
    private CharacterAreaController controller;
    [SerializeField] private GridNode gridNode;
    public float nextFire = 0f;
    private int floorMask;

    bool swing = false;
    GameObject[] range = null;

    public Color nodeClearColor = new Color(255, 255, 255, 55);
    public Color nodeAttackColor = new Color(255, 0, 0,120);

    int xPos = -1;
    int yPos = -1;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterAreaController>();
        floorMask = LayerMask.GetMask("Terrain");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire || swing)
        {
            Swing();
        }
    }

    void Swing()
    {
        StartCoroutine("SwingRoutine");
    }

 
    /*
     * given a direction and a position determine what nodes can be in the attack range
     */
    GameObject[] DetermineRange(CharacterAreaController.Directions direction, int x, int y)
    {
        GameObject[,] nodes = gridNode.getNodes();
        GameObject[] range = new GameObject[3];
        if (direction == CharacterAreaController.Directions.North)
        {
            
            try
            {
                range[0] = nodes[x + 1, y + 1];             
            }
            catch (System.IndexOutOfRangeException e)
            {

            }


            try
            {
                range[1] = nodes[x + 1, y];
            }
            catch (System.IndexOutOfRangeException e)
            {

            }

            try
            {
                range[2] = nodes[x + 1, y - 1];
            }
            catch (System.IndexOutOfRangeException e)
            {

            }



        }
        else if (direction == CharacterAreaController.Directions.NorthEast)
        {

            try
            {
                range[0] = nodes[x + 1, y];              
            }
            catch (System.IndexOutOfRangeException e)
            {

            }


            try
            {
                range[1] = nodes[x + 1, y - 1];
            }
            catch (System.IndexOutOfRangeException e)
            {

            }

            try
            {
                range[2] = nodes[x, y - 1];
            }
            catch (System.IndexOutOfRangeException e)
            {

            }


        }
        else if (direction == CharacterAreaController.Directions.East)
        {

            try
            {
                range[0] = nodes[x + 1, y - 1];
            }
            catch (System.IndexOutOfRangeException e)
            {

            }


            try
            {
                range[1] = nodes[x, y - 1];
            }
            catch (System.IndexOutOfRangeException e)
            {

            }

            try
            {
                range[2] = nodes[x - 1, y - 1];
            }
            catch (System.IndexOutOfRangeException e)
            {

            }


        }
        else if (direction == CharacterAreaController.Directions.SouthEast)
        {

            try
            {
                range[0] = nodes[x, y - 1];
            }
            catch (System.IndexOutOfRangeException e)
            {

            }


            try
            {
                range[1] = nodes[x - 1, y - 1];
            }
            catch (System.IndexOutOfRangeException e)
            {

            }

            try
            {
                range[2] = nodes[x - 1, y ];
            }
            catch (System.IndexOutOfRangeException e)
            {

            }


        }

        else if (direction == CharacterAreaController.Directions.South)
        {

            try
            {
                range[0] = nodes[x - 1, y - 1];
            }
            catch (System.IndexOutOfRangeException e)
            {

            }


            try
            {
                range[1] = nodes[x - 1, y];
            }
            catch (System.IndexOutOfRangeException e)
            {

            }

            try
            {
                range[2] = nodes[x - 1, y + 1];
            }
            catch (System.IndexOutOfRangeException e)
            {

            }


        }
        else if (direction == CharacterAreaController.Directions.SouthWest)
        {

            try
            {
                range[0] = nodes[x - 1, y];
            }
            catch (System.IndexOutOfRangeException e)
            {

            }


            try
            {
                range[1] = nodes[x - 1, y + 1];
            }
            catch (System.IndexOutOfRangeException e)
            {

            }

            try
            {
                range[2] = nodes[x, y + 1];
            }
            catch (System.IndexOutOfRangeException e)
            {

            }


        }
        else if (direction == CharacterAreaController.Directions.West)
        {

            try
            {
                range[0] = nodes[x - 1, y + 1];
            }
            catch (System.IndexOutOfRangeException e)
            {

            }


            try
            {
                range[1] = nodes[x, y + 1];
            }
            catch (System.IndexOutOfRangeException e)
            {

            }

            try
            {
                range[2] = nodes[x + 1, y + 1];
            }
            catch (System.IndexOutOfRangeException e)
            {

            }


        }
        else if (direction == CharacterAreaController.Directions.NorthWest)
        {

            try
            {
                range[0] = nodes[x, y + 1];
            }
            catch (System.IndexOutOfRangeException e)
            {

            }


            try
            {
                range[1] = nodes[x + 1, y + 1];
            }
            catch (System.IndexOutOfRangeException e)
            {

            }

            try
            {
                range[2] = nodes[x + 1, y];
            }
            catch (System.IndexOutOfRangeException e)
            {

            }


        }
        return range;
    }

    IEnumerator SwingRoutine()
    {

        swing = true;
        //if the player clicks the right click then we cancel the attack and recolor the nodes to their default color
        if (Input.GetMouseButtonDown(1))
        {
            swing = false;
            if (range.Length > 0)
            {
                ColorNodes(range, true);
            }
           
            //nextFire += Time.time;
        }
        else
        {
            //determine the vector between the player and mouse
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
           
            if (Physics.Raycast(mouseRay, out hit, Mathf.Infinity, floorMask))
            {
                Vector3 positionOnScreen = transform.position;
                Vector3 mouseOnScreen = hit.point;
                Vector3 mousePlayer = mouseOnScreen - positionOnScreen;
                float angle = Vector3.SignedAngle(Vector3.right, mousePlayer, Vector3.up);

                CharacterAreaController.Directions direction = controller.getDirection(angle);
                //if our current direction is different than the direction we calculated we assign our current direction to our calculated direction
                if (direction != controller.getCurrentDirection())
                {

                    //if we have a possible range but our direction changed we color the nodes in the range with the default color because we are not looking in the same direction as we were before so the range is going to be different
                    if (range != null)
                    {
                        if (range.Length > 0)
                        {
                            ColorNodes(range, true);
                        }
                    }

                }
                Node playerNode = null;
                //Node playerNode = controller.getCurrentNode().GetComponent<Node>();
                int x = playerNode.getGridX();
                int y = playerNode.getGridY();

                //if this is the first time, initilize our xPos and yPos to current Player position
                if (xPos < 0 || yPos < 0)
                {
                    xPos = x;
                    yPos = y;
                }

                if (x != xPos || y != yPos)
                {
                    //if our position changed then we clear the range we were in because the range will now be different
                    xPos = x;
                    yPos = y;
                    ColorNodes(range, true);
                }
                Debug.Log(direction);
                //calculate the range
                range = DetermineRange(direction, x, y);




                if (range != null)
                {
                    // if we have a range, color it red indicating what nodes are in range
                    ColorNodes(range, false);
                }
                //if we hit the left click we perform the attack and set player direction in the direction of the attack
                if (Input.GetMouseButtonDown(0))
                {
                    //controller.setDirection(direction);
                }
            }


                


            
            
          
        }
        return null;

    }
    /**
     * color nodes given an array
     */
    void ColorNodes(GameObject[] range, bool clear)
    {
        if (clear)
        {
            foreach (GameObject g in range)
            {
                g.GetComponent<SpriteRenderer>().color = nodeClearColor;
            }
        }
        else
        {
            foreach (GameObject g in range)
            {
                g.GetComponent<SpriteRenderer>().color = nodeAttackColor;
            }
        }
    }
}




