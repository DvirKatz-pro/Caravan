using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinding : MonoBehaviour
{
    //refernce to tile map
    [SerializeField] private Tilemap tileMap;
    //reference to all nodes in the grid
    private GridNode script;
    //list of open nodes that we might yet traverse in pathfinding
    private List<GameObject> openList;
    //nodes that we should not traverse in pathfinding
    private List<GameObject> closedList;
   
    // Start is called before the first frame update
    void Start()
    {
        script = tileMap.GetComponent<GridNode>();
        if (script == null)
        {
            Debug.LogError("Grid Node is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
     * find a path from one node to another
     */
    public List<GameObject> FindPath(GameObject startNode,GameObject endNode)
    {
        openList = new List<GameObject>();
        closedList = new List<GameObject>();

        GameObject[,] nodes = script.getNodes();
       //add starting node
        openList.Add(startNode);
       // reset all nodes to default
        for (int i = 0; i < nodes.GetLength(0); i++)
        {
            for (int j = 0; j < nodes.GetLength(1); j++)
            { 
                Node node = nodes[i, j].GetComponent<Node>();
                node.setParent(null);
                node.setGcost(int.MaxValue);
            }
        }

        //set the start node and calculate its distance from the end
        startNode.GetComponent<Node>().setGcost(0);
        startNode.GetComponent<Node>().setHcost(CalculateDistance(startNode.GetComponent<Node>(), endNode.GetComponent<Node>()));

       
        while(openList.Count > 0)
        {
            //get the lowest cost node from the list
            GameObject lowest = LowestCost(openList);
           
            //if the lowest cost node is also the end node, then we are done, we get the path and return it
            if (lowest == endNode)
            {
                List<GameObject> path = CalculatePath(endNode);
                return path;
            }
            //we checked the lowest node so we add it to a closed list meaning we dont examine that node anymore
            openList.Remove(lowest);
            closedList.Add(lowest);
            
            //check that nodes neghibours
            foreach (Node n in lowest.GetComponent<Node>().getNeighbours())
            {
                
                if (!closedList.Contains(n.gameObject))
                {
                    //the cost to get to this node + our cost so far
                    int tentativeGCost = lowest.GetComponent<Node>().getGcost() + CalculateDistance(lowest.GetComponent<Node>(), n);
                    if (tentativeGCost < n.getGcost())
                    {
                        //if we are getting closer to the end set the lowest node as parent, set its gCost properly and Hcost and add it to the openList
                        n.setParent(lowest.GetComponent<Node>());
                        n.setGcost(tentativeGCost);
                        n.setHcost(CalculateDistance(n, endNode.GetComponent<Node>()));
                        if (!openList.Contains(n.gameObject))
                        {
                            openList.Add(n.gameObject);
                        }
                        

                    }
                    

                }
                

            }
            

       }

        return null;

        
    }
    /**
     * work backwards from the end Node getting the parent recursivly untill we reach the start node
     */
    List<GameObject> CalculatePath(GameObject endNode)
    {
        
        List<GameObject> path = new List<GameObject>();
        path.Add(endNode);
        GameObject currentNode = endNode;
        while (currentNode.GetComponent<Node>().getParent() != null)
        {
            path.Add(currentNode.GetComponent<Node>().getParent().gameObject);
            currentNode = currentNode.GetComponent<Node>().getParent().gameObject;
        }
        path.Reverse();
        return path;
    }
    /**
     * given a list return the lowest Node from that list
     */
    GameObject LowestCost(List<GameObject> list)
    {
        
        GameObject lowest = list[0];
        for (int i = 0; i < list.Count; i++)
        {
           
            Node node = list[i].GetComponent<Node>();
            if (node.fCost() < lowest.GetComponent<Node>().fCost())
            {
                lowest = list[i];
             }
            
        }
        return lowest;
    }
    /**
     * calculate the distance between two nodes, dosent support diagonal check yet
     */
    int CalculateDistance(Node a, Node b)
    {
        int xDistance = Mathf.Abs(a.getGridX() - b.getGridX());
        int yDistnace = Mathf.Abs(a.getGridY() - b.getGridY());
        int remaining = Mathf.Abs(xDistance - yDistnace);
        //add diagonal cost = 14 here move strieght cost = 10
        //return moveDiagonalCost * Mathf.Min(xDistance,yDistnace) + moveStrieghtCost * remaining
        return (xDistance + yDistnace) * 10;
    }
   
}
