using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempBuild : MonoBehaviour
{
    
    public Maze maze;
    public GameObject MazeModel;
    // Start is called before the first frame update
    void Awake()
    {
        maze.BuildMap();
        //MazeModel.transform.Rotate(-90.0f, 0.0f, 0.0f);
        MazeModel.SetActive(false);
    }

    public GameObject getModel()
    {
        return MazeModel;
    }

    // Update is called once per frame
}
