using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// The class is the maze generator class, its responsible the generate all the mazes in the play session, according to the required specifications.
/// </summary>
public class MazeGenerator : MonoBehaviour
{

    #region Cached References
    /// <summary>
    /// The maze algorithm which will generate our maze
    /// </summary>
    public Maze maze;


    /// <summary>
    /// The maze model itself as a Game Object
    /// </summary>
    public GameObject mazeModel;


    /// <summary>
    /// The door Gameobject prefab
    /// </summary>
    public GameObject doorPrefab;


    /// <summary>
    /// Referance for the randomized Session instance
    /// </summary>
    private RandomizedSession randomizedSession;
    /// <summary>
    /// Referance for the therapy Session instance
    /// </summary>
    private TherapySession therapySession;

    /// <summary>
    /// Referance of the cached maze
    /// </summary>
    private Maze cachedMaze;
    #endregion

    #region Properties
    /// <summary>
    /// Door starting position in the world.
    /// </summary>
    private float doorPositionX = -0.4f, doorPositionY = 0.95f, doorPositionZ = 4;
    /// <summary>
    /// The distance between each generated door
    /// </summary>
    private int zFactor = 10;

    /// <summary>
    /// This flag is for debugging purposes, when you want to play without any session requirements
    /// </summary>
    public bool enabledDebug = false;



    /// <summary>
    /// The amount of predefined doors, can be adjusted in Unity Editor while enabling Debug mode from any session scene
    /// </summary>
    [SerializeField] private int amountPreDefineDoors = 5;
    #endregion

    #region Dictionaries
    /// <summary>
    /// Dictionary of all maze GameObjects, were the key is the maze index and value is the maze itself
    /// </summary>
    [SerializeReference]
    public Dictionary<int, GameObject> mazesGODict;
    /// <summary>
    /// Dictionary of all doors GameObjects, were the key is the maze index and value is the door itself
    /// </summary>
    public Dictionary<int, GameObject> doorDict;



    /// <summary>
    /// Models dictionary 
    /// </summary>
    public Dictionary<int, GameObject> models;




    /// <summary>
    /// Dictionary of all Maze Data Objects, were the key is the maze index and value is the maze data object itself 
    /// </summary>

    public Dictionary<int, MazeDataObject> mazeDataObjectsDictionary;
    #endregion

    #region Unity Messages

    void Awake()
    {
        cachedMaze = GetComponent<Recursive>();
        mazeDataObjectsDictionary = new Dictionary<int, MazeDataObject>();

        doorDict = new Dictionary<int, GameObject>();
        if (!enabledDebug)
        {
            if (SessionManager.instance.CurrentGameSession is TherapySession)
            {
                therapySession = SessionManager.instance.CurrentGameSession as TherapySession;
            }
            else
            {
                randomizedSession = SessionManager.instance.CurrentGameSession as RandomizedSession;
            }

            cachedMaze.width = therapySession != null ? therapySession.MazeSizeRange : randomizedSession.MazeSizeRange;
            cachedMaze.depth = therapySession != null ? therapySession.MazeSizeRange : randomizedSession.MazeSizeRange;
            int amountOfMazes = therapySession != null ? therapySession.AmountOfMazes : randomizedSession.AmountOfMazes;
            for (int i = 0; i < amountOfMazes; i++)
            {
                doorDict.Add(i, Instantiate(doorPrefab, new Vector3(doorPositionX, doorPositionY, doorPositionZ + zFactor * i), Quaternion.Euler(0, 270, 90)));
            }
        }
        else
        {
            for (int i = 0; i < amountPreDefineDoors; i++)
            {
                doorDict.Add(i, Instantiate(doorPrefab, new Vector3(doorPositionX, doorPositionY, doorPositionZ + zFactor * i), Quaternion.Euler(0, 270, 90)));
            }
        }
        InitMazeModelsInScene();
    }


    #endregion

    #region Methods

    private void FindAllMazeGameObjects()
    {
        mazesGODict = new Dictionary<int, GameObject>();
        foreach (KeyValuePair<int, GameObject> doorGODict in doorDict)
        {
            GameObject mazeGO = doorGODict.Value.GetComponent<DoorScript>().mazeGO;
            if (mazeGO == null)
            {
                Debug.LogError("Cannot add new maze Gameobject");
            }
            else
            {
                mazesGODict.Add(doorGODict.Key, mazeGO);
            }



        }
    }

    private void InitMazeModelsInScene()
    {
        Transform[] children;
        FindAllMazeGameObjects();

        models = new Dictionary<int, GameObject>();


        foreach (KeyValuePair<int, GameObject> goDict in mazesGODict)
        {
            GameObject model = Instantiate(mazeModel, Vector3.zero, Quaternion.identity, this.transform);
            maze.mazeModel = model;
            maze.startPoint = model.GetComponent<ModelGameObject>().StartPoint;
            maze.endPoint = model.GetComponent<ModelGameObject>().EndPoint;
            do
            {
                maze.BuildMap();
                children = model.gameObject.GetComponentsInChildren<Transform>();
            } while (maze.getHandle() == null || children.Length < 4);
            model.GetComponent<ModelGameObject>().Handle = maze.getHandle();
            model.GetComponent<ModelGameObject>().Handle.transform.SetParent(model.transform);
            model.GetComponent<ModelGameObject>().mazeGraph = maze.mazeGraph;
            model.GetComponent<ModelGameObject>().side = MazeSide.Center;
            model.GetComponent<ModelGameObject>().mazeSerializer = goDict.Key;
            MazeNodeInfo startNode = maze.mazeGraph.GetStartNode();
            MazeNodeInfo endNode = maze.mazeGraph.GetEndNode();
            model.GetComponent<ModelGameObject>().anlyzer = new GraphAnlyzer(maze.mazeGraph, startNode, endNode); ;
            model.SetActive(false);
            if (therapySession != null)
            {
                mazeDataObjectsDictionary.Add(goDict.Key, new MazeDataObject(therapySession.SessionID, goDict.Key, therapySession.MazeSizeRange, 1, "graph", "shortestPath", "playerPath"));

            }
            else if (randomizedSession != null)
            {
                mazeDataObjectsDictionary.Add(goDict.Key, new MazeDataObject(0, goDict.Key, randomizedSession.MazeSizeRange, 1, "graph", "shortestPath", "playerPath"));
            }
            models.Add(goDict.Key, model);
        }

        foreach (KeyValuePair<int, GameObject> go in mazesGODict)
        {
            go.Value.GetComponent<MazeController>().setIndex(go.Key);
            // go.Value.SetActive(true);
        }
        mazesGODict[0].SetActive(true);
        MazeManager.instance.mazeDataObjects = mazeDataObjectsDictionary;
    }

     public GameObject getModel(int index)
    {
        return models[index];
    }

    #endregion 

}
