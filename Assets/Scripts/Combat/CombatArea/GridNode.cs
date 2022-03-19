using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class GridNode : MonoBehaviour
{
    //grid reference
    ////[SerializeField] private Grid grid;
    //list of obstecules for pathfinding
    //[SerializeField] private List<Tilemap> obstecules;
    //reference to Node
    [SerializeField] private GameObject Nodeprefab;
    //values that determine where we start and end node generation in the grid
    //[SerializeField] private int scanStartX = -5, scanStartY = -5, scanEndX = 4, scanEndY = 5;
    //size of cell
    [SerializeField] private float cellSizeX, cellSizeY;

    [SerializeField] private GameObject gridStart;
    [SerializeField] private Vector2 gridSize;
    private GameObject[,] nodes;

    [SerializeField] private GameObject grid;

    private void Awake()
    {
        nodes = new GameObject[12,12];
        createNodes();
    }


    /*
     * return neghibour Nodes given their x and y position
     */
    public List<Node> getNeighbourNodes(int x, int y)
    {

        List<Node> myNeighbours = new List<Node>();

        //needs the width & height to work out if a tile is not on the edge, also needs to check if the nodes is null due to the accounting for odd shapes

        if (x + 1 < nodes.GetLength(0))
        {
            myNeighbours.Add(nodes[x + 1, y].GetComponent<Node>());
        }
        if (y + 1 < nodes.GetLength(1))
        {
            myNeighbours.Add(nodes[x, y + 1].GetComponent<Node>());
        }
        if (x - 1 >= 0)
        {
            myNeighbours.Add(nodes[x - 1, y].GetComponent<Node>());
        }
        if (y - 1 >= 0)
        {
            myNeighbours.Add(nodes[x, y - 1].GetComponent<Node>());
        }
        if (x - 1 >= 0 && y - 1 >= 0)
        {
            myNeighbours.Add(nodes[x - 1, y - 1].GetComponent<Node>());
        }
        if (x - 1 >= 0 && y + 1 < nodes.GetLength(1))
        {
            myNeighbours.Add(nodes[x - 1, y + 1].GetComponent<Node>());
        }
        if (x + 1 < nodes.GetLength(0) && y - 1 >= 0 )
        {
            myNeighbours.Add(nodes[x + 1, y - 1].GetComponent<Node>());
        }
        if (x + 1 < nodes.GetLength(0) && y + 1 < nodes.GetLength(1))
        {
            myNeighbours.Add(nodes[x + 1, y + 1].GetComponent<Node>());
        }


        return myNeighbours;
    }

    /**
     * create an array of Nodes in a grid
     */
    void createNodes()
    {
        //set the bounds
        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                Vector3 nodePosition = gridStart.transform.position;
                nodePosition.x += i * cellSizeX;
                nodePosition.z += j * cellSizeY;

                GameObject nodeObject = Instantiate(Nodeprefab, nodePosition, Nodeprefab.transform.rotation);
                nodeObject.transform.parent = grid.transform;
                Node node = nodeObject.GetComponent<Node>();
                node.setGridX(i);
                node.setGridY(j);
                nodes[i, j] = nodeObject;

                   

                   
            }


        }
          
    
        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {

                GameObject g = nodes[i, j];
                if (g)
                {
                    Node node = g.GetComponent<Node>();
                    node.setNeighbours(getNeighbourNodes(i, j));
                }
            }
        }
    }

    /* 
     * given a position return the node at that position, takes a vector 3 because WorldToCell unity function needs it
     */
    public GameObject GetNode(Vector3 position)
    {
        return nodes[(int)position.x,(int)position.y];
    }
    /*
     * get all the nodes in the grid 
     */
    public GameObject[,] getNodes()
    {
        return nodes;
    }
}

