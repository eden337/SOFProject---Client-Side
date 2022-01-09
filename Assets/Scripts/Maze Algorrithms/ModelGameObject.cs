using UnityEngine;

/// <summary>
/// This class holds all the properties of the model game object
/// </summary>
public class ModelGameObject:MonoBehaviour
{
    public GameObject StartPoint;
    public GameObject EndPoint;

    public GameObject Handle;

    public MazeGraph mazeGraph;
    public GraphAnlyzer anlyzer;
    public int mazeSerializer;
    public MazeSide side;
}

