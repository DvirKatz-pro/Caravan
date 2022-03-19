using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * class representation of a Node thats used with pathfinding
 */
public class Node : MonoBehaviour
{
    private int hCost;
    private int gCost;
    private int gridX, gridY;
    private bool walkable = true;
    private List<Node> neighbours;
    private Node parent;

    //fcost for A*
    public int fCost()
    {
        return hCost + gCost;
    }
    //get grid positions x and y
    public int getGridX()
    {
        return gridX;
    }
    public int getGridY()
    {
        return gridY;
    }
    //get g cost for A*
    public int getGcost()
    {
        return gCost;
    }
    //is this tile walkable
    public bool getWalkable()
    {
        return walkable;
    }
    //get tile neighbours
    public List<Node> getNeighbours()
    {
        return neighbours;
    }
    //get the parent in the A* path
    public Node getParent()
    {
        return parent;
    }
    //set parent for A* path
    public void setParent(Node m_parent)
    {
        parent = m_parent;
    }
    //set neighbour Nodes
    public void setNeighbours(List<Node> m_neighbours)
    {
        neighbours = m_neighbours;
    }
    //set the x any y in the grid
    public void setGridX(int m_gridX)
    {
        gridX = m_gridX;
    }
    public void setGridY(int m_gridY)
    {
        gridY = m_gridY;
    }
    //set the walkability of this tile
    public void setWalkable(bool m_walkable)
    {
        walkable = m_walkable;
    }
    //set G and H costs for A*
    public void setGcost(int value)
    {
        gCost = value;
    }
    public void setHcost(int value)
    {
        hCost = value;
    }
   
}
