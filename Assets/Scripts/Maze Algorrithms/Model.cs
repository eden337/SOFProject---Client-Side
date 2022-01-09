using UnityEngine;

/// <summary>
/// This Class holds the properties of the maze model
/// </summary>
public class Model
{
    public GameObject StartPoint;
    public GameObject EndPoint;
    public Lever lever;
    public TraveledNode currentNode;

    public Model(GameObject startPoint = null, GameObject endPoint = null, Lever lever = null, TraveledNode currentNode = null)
    {
        StartPoint = startPoint;
        EndPoint = endPoint;
        this.lever = lever;
        this.currentNode = currentNode;
    }
}