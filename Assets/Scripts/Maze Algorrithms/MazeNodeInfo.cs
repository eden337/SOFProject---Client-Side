using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class include the information about a node in the maze graph, such as node number, node position,
/// the number of times which this node has visited, the maze piece style, and more... 
/// </summary>

public class MazeNodeInfo
{
    #region  Properties    
    /// <summary>
    /// Node number is the index number of the node in the graph. 
    /// </summary>
    public int nodeNumber;
    /// <summary>
    /// The location of the node on the maze
    /// </summary>
    public MapLocation nodePosition;
    /// <summary>
    /// the amount of time which this node had been visited
    /// </summary>
    public int visted;
    /// <summary>
    /// The maze piece style 
    /// </summary>
    public Pattern mazePieceType;
    /// <summary>
    /// list of available neighbors
    /// </summary>
    public bool[] isThereNeighbors;
    public List<int> neighborsKeys;
    /// <summary>
    /// The directions of the neighbors;
    /// </summary>
    public int[] neighborsIndexDirections;
    /// <summary>
    /// Is this node a start point?
    /// </summary>
    public bool startPoint;
    /// <summary>
    /// Is this node a end point?
    /// </summary>
    public bool endPoint;
    /// <summary>
    /// Visual represention of the node
    /// </summary>
    public GameObject sphereNode;
    #endregion
    
    #region Contructor
    public MazeNodeInfo(int x, int z, Pattern pattern)
    {
        nodeNumber = -1;
        nodePosition = new MapLocation(x, z);
        mazePieceType = pattern;
        visted = 0;
        isThereNeighbors = new bool[4];//// 0-Top, 1-Bottom, 2- Right, 3- Left
                                       // maybe need to add try catch if is null returned 
                                       //holds possible directions of neighbors
        neighborsIndexDirections = mazePieceType.getNeighbors();
        //holds the neighbors nodeNumbers
        neighborsKeys = new List<int>(4);
        startPoint = false;
        endPoint = false;

    }
    #endregion
    #region Methods
    public bool isConnectedToNeighbors()
    {

        for (int i = 0; i < neighborsIndexDirections.Length; i++)
        {
            if (!isThereNeighbors[neighborsIndexDirections[i]])
                return false;
        }
        return true;
    }

    public void setStartPoint()
    {
        startPoint = true;
    }
    public void setEndPoint()
    {
        endPoint = true;
    }
    public void createNodeVisual(GameObject parent, Vector3 pos, float scale, Vector3 rotation)
    {

        sphereNode = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphereNode.gameObject.name = "Node " + this.nodeNumber + " " + this.mazePieceType.getMazeType();
        sphereNode.transform.localScale = new Vector3(scale, scale, scale);
        sphereNode.transform.position = pos;
        sphereNode.transform.Rotate(rotation);
        sphereNode.transform.SetParent(parent.transform);

    }

    public override bool Equals(object obj)
    {
        return obj is MazeNodeInfo info &&
               nodeNumber == info.nodeNumber;
    }

    public override int GetHashCode()
    {
        return 358973224 + nodeNumber.GetHashCode();
    }
    #endregion
}