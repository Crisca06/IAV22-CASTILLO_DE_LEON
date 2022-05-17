    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UCM.IAV.CristianCastillo;

public class Node : System.IComparable
{
    public float nodeTotalCost;
    public float estimatedCost;
    public bool bObstacle;
    public Node parent;
    public Vector3 position;
    public Node()
    {
        this.estimatedCost = 0.0f;
        this.nodeTotalCost = 1.0f;
        this.bObstacle = false;
        this.parent = null;
    }
    public Node(Vector3 pos)
    {
        this.estimatedCost = 0.0f;
        this.nodeTotalCost = 1.0f;
        this.bObstacle = false;
        this.parent = null;
        this.position = pos;
    }
    public void MarkAsObstacle()
    {
        this.bObstacle = true;
    }

    public int CompareTo(object obj)
    {
        Node node = (Node)obj;
        //Negative value means object comes before this in the sort
        //order.
        if (this.estimatedCost < node.estimatedCost)
            return -1;
        //Positive value means object comes after this in the sort
        //order.
        if (this.estimatedCost > node.estimatedCost) return 1;
        return 0;
    }
}


public class A_Star
{
    public static BinaryHeap<Edge> openList;
    public static HashSet<Node> closedList;
    //public static ArrayList FindPath(Edge start, Edge goal)
    //{
    //    openList = new BinaryHeap<Edge>();
    //    openList.Add(start);
    //    start.nodeTotalCost = 0.0f;
    //    start.estimatedCost = 
    //    closedList = new HashSet<Node>();
    //    Node node = null;

    //}
}
