using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;


/// <summary>
/// This class handles the conversion of the maze into a graph
/// </summary>
public class MazeGraph
{
    #region  Properties
    private List<MazeNodeInfo> nodes;
    private MazeEdgeInfo[,] edges;
    private int currAmountNode;
    private int maxSize;
    private HashSet<MazeEdgeInfo> addedEdges;
    #endregion

    #region Constructor
    public MazeGraph(int maxSize)
    {
        this.maxSize = maxSize;
        currAmountNode = 0;
        nodes = new List<MazeNodeInfo>(maxSize);
        addedEdges = new HashSet<MazeEdgeInfo>();


    }
    #endregion

    #region Methods
    public void printEdges()
    {
        Debug.Log("Printing all edges");
        string str = "";
        for (int i = 0; i < edges.GetLength(0); i++)
        {
            for (int j = 0; j < edges.GetLength(1); j++)
            {
                str += edges[i, j] + ", ";
            }
            str += "\n";
        }
        Debug.Log(str);
    }

    public int changeDirection(int node1, int node2, int node3)
    {
        if (node1 == node3)
            return 0;
        MapLocation first = getNode(node1).nodePosition;
        MapLocation middle = getNode(node2).nodePosition;
        MapLocation last = getNode(node3).nodePosition;
        if (first == null || middle == null || last == null)
            return -1;
        return (((first.x == middle.x) && (middle.x == last.x)) || ((first.z == middle.z) && (middle.z == last.z))) ? 1 : 0;
    }
    //Getters Functions
    public MazeNodeInfo getNode(int index)
    {
        return nodes[index];
    }

    public MazeEdgeInfo getEdge(int indexNode1, int indexNode2)
    {
        if (indexNode1 > edges.GetLength(0) || indexNode2 > edges.GetLength(1))
        {
            throw new IndexOutOfRangeException("Specific index1: " + indexNode1 + " index 2: " + indexNode2 + " maze Dim (0)" + edges.GetLength(0) + " maze Dim (1)" + edges.GetLength(1) + "Out of bounds");
        }
        else
        {
            MazeEdgeInfo e = edges[indexNode1, indexNode2];
            return e;
        }
    }

    public int getNumberOfNodes()
    {
        return nodes.Count;
    }
    public int getDistance(int indexNode1, int indexNode2)
    {
        MazeEdgeInfo e = getEdge(indexNode1, indexNode2);
        return e == null ? -1 : e.getDistance();
    }
    public int getPassed(int indexNode1, int indexNode2)
    {
        MazeEdgeInfo e = getEdge(indexNode1, indexNode2);
        return e == null ? -1 : e.getPassed();
    }

    public void resetAllEdgesPasses()
    {
        foreach (MazeEdgeInfo e in addedEdges)
        {
            e.resetPass();
        }
    }

    public MazeNodeInfo GetStartNode()
    {
        foreach (MazeNodeInfo node in nodes)
        {
            if (node.startPoint)
            {
                return node;
            }
        }
        return null;
    }
    public MazeNodeInfo GetEndNode()
    {
        foreach (MazeNodeInfo node in nodes)
        {
            if (node.endPoint)
            {
                return node;
            }
        }
        return null;
    }

    //Adding functions
    public bool addNode(MazeNodeInfo node)
    {
        if (node.nodeNumber != -1 || currAmountNode >= maxSize)
        {
            return false;
        }
        node.nodeNumber = currAmountNode;
        currAmountNode++;
        nodes.Add(node);
        return true;

    }
    public bool addEdge(int node1, int node2, int distance)
    {
        int type1 = getNode(node1).mazePieceType.getBaseType();
        int type2 = getNode(node2).mazePieceType.getBaseType();
        if (type1 > 1 || type2 > 1)
        {
            ;
        }

        if (node1 == node2)
            return false;
        if (node1 < currAmountNode && node2 < currAmountNode)
        {
            if (edges[node1, node2] != null || edges[node2, node1] != null)
            {
                Debug.Log(node1 + getNode(node1).mazePieceType.getMazeType() + " , " + node2 + getNode(node2).mazePieceType.getMazeType() + "failed");
                return false;
            }
            edges[node1, node2] = new MazeEdgeInfo(distance, node1, node2);
            edges[node2, node1] = edges[node1, node2];
            getNode(node1).neighborsKeys.Add(getNode(node2).nodeNumber);
            getNode(node2).neighborsKeys.Add(getNode(node1).nodeNumber);
            addedEdges.Add(edges[node1, node2]);
            return true;
        }
        else
        {
            Debug.Log(node1 + getNode(node1).mazePieceType.getMazeType() + " , " + node2 + getNode(node2).mazePieceType.getMazeType() + "failed");
            return false;
        }

    }

    //Set Edegs functions
    public void initiliazeEdges()
    {
        edges = new MazeEdgeInfo[currAmountNode, currAmountNode];
    }
    public MazeNodeInfo findNode(MapLocation currentLocation)
    {
        foreach (MazeNodeInfo node in nodes)
        {
            if (currentLocation.Equals(node.nodePosition))
                return node;
        }
        return null;
    }
    public void connectEdges(byte[,] map)
    {
        HashSet<MazeNodeInfo> visted = new HashSet<MazeNodeInfo>();
        foreach (MazeNodeInfo node in nodes)
        {
            if (!visted.Contains(node) && !node.isConnectedToNeighbors())
            {
                // search all possible neighbors
                for (int i = 0; i < node.neighborsIndexDirections.Length; i++)
                {
                    int direction = node.neighborsIndexDirections[i];
                    if (!FindNeighbor(direction, map, node))
                    {
                        //Debug.Log("coulden't find an edge");
                        //FindNeighbor(direction, map, node);
                    }
                }
                visted.Add(node);
            }
        }
    }
    public void resetListCapacityToCount()
    {
        nodes.TrimExcess();
    }


    private bool FindNeighbor(int direction, byte[,] map, MazeNodeInfo node)
    {
        // if Search is in the z direction dimFlag=true

        //else search is in the x direction
        bool dimFlag = direction / 2 == 0;
        // if Search is increasing... incFlag=true
        //else search is decreaing
        bool incFlag = direction % 2 == 0;
        int x = node.nodePosition.x;
        int z = node.nodePosition.z;
        int distance = 1;
        int axis = dimFlag ? z : x;
        int threshold = dimFlag ? map.GetLength(1) : map.GetLength(0);
        if (incFlag)
        {
            for (int i = axis + 1; i < threshold; i++)
            {
                byte cell = dimFlag ? map[x, i] : map[i, z];
                MapLocation location = dimFlag ? new MapLocation(x, i) : new MapLocation(i, z);
                if (cell == 0)
                {
                    distance++;
                }
                else if (cell == 1)
                {
                    return false;
                }
                else if (cell == 5)
                {
                    MazeNodeInfo neighbor = findNode(location);
                    int neighborIndex = incFlag ? direction + 1 : direction - 1;
                    if (neighbor == null)
                    {
                        ;//need an exception
                    }
                    if (!neighbor.isThereNeighbors[neighborIndex])
                    {

                        if (!addEdge(node.nodeNumber, neighbor.nodeNumber, distance))
                        {
                            //throw execption or something
                        }
                        node.isThereNeighbors[direction] = true;
                        neighbor.isThereNeighbors[neighborIndex] = true;
                        return true;

                    }
                }
            }
        }
        else
        {
            for (int i = axis - 1; i >= 0; i--)
            {
                byte cell = dimFlag ? map[x, i] : map[i, z];
                MapLocation location = dimFlag ? new MapLocation(x, i) : new MapLocation(i, z);
                if (cell == 0)
                {
                    distance++;
                }
                else if (cell == 1)
                {
                    return false;
                }
                else if (cell == 5)
                {
                    MazeNodeInfo neighbor = findNode(location);
                    int neighborIndex = incFlag ? direction + 1 : direction - 1;
                    if (!neighbor.isThereNeighbors[neighborIndex])
                    {
                        if (!addEdge(node.nodeNumber, neighbor.nodeNumber, distance))
                        {
                            //throw execption or something
                        }
                        node.isThereNeighbors[direction] = true;
                        neighbor.isThereNeighbors[neighborIndex] = true;
                        return true;

                    }
                }
            }
        }
        return false;



    }
    #endregion
}

