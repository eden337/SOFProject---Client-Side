using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLocation
{
    public int x;
    public int z;

    public MapLocation(int _x, int _z)
    {
        x = _x;
        z = _z;
    }
    public override bool Equals(object obj)
    {
        return obj is MapLocation location &&
               x == location.x &&
               z == location.z;
    }
}
public class Maze : MonoBehaviour
{
    protected List<MapLocation> directions = new List<MapLocation>() {
        new MapLocation(1, 0), new MapLocation(0, 1), new MapLocation(-1, 0), new MapLocation(0, -1) };


    public int width = 30;//x length
    public int depth = 30;//z length
    public byte[,] map;
    public byte[,] mapGraph;
    public float scale = 0.16f;
    //public int scale=1;
    public GameObject startPoint;
    public GameObject endPoint;
    public GameObject mazeModel;
    public GameObject Corridor;
    public GameObject Corner;
    public GameObject TJunction;
    public GameObject Crossroad;
    public GameObject DeadEnd;
    public GameObject handlePrefab;
    //public GraphAnlyzer anlyzer;

    //public GameObject mazeGraphVisual;

    public MazeSide side;

    public MazeGraph mazeGraph;
    public GameObject Handle;

    private bool isMapGenerated()
    {
        int counter = 0;
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                if (map[x, z] == 0)
                {
                    counter++;
                }
            }
        }

        return counter >= 2;
    }

    // Start is called before the first frame update
    public void BuildMap()
    {
        InitializeMap();

        do{
            Generate();
        }while(!isMapGenerated());
        copyMapToMapGraph();
        DrawMap();

        //mazeGraphVisual.SetActive(false);
    }


    void InitializeMap()
    {
        mazeGraph = new MazeGraph(depth * width);
        map = new byte[width, depth];
        mapGraph = new byte[width, depth];

        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                map[x, z] = 1; // 1:wall, 0: corridor

            }
        }

    }


    public void setSide(MazeSide side)
    {
        this.side = side;
    }
    void SetupGraph()
    {
        MazeNodeInfo startNode, endNode;
        mazeGraph.initiliazeEdges();
        mazeGraph.resetListCapacityToCount();

        mazeGraph.connectEdges(mapGraph);
        printMap();
        startNode = mazeGraph.GetStartNode();
        endNode = mazeGraph.GetEndNode();

        //anlyzer = new GraphAnlyzer(mazeGraph, startNode, endNode);


    }

    private void printMap()
    {
        string mapStr = "-";
        for (int i = 0; i < width; i++)
            mapStr += i;
        for (int x = 0; x < width; x++)
        {
            mapStr += x;
            for (int z = 0; z < depth; z++)
            {
               
                mapStr += mapGraph[x, z];

            }
            mapStr += "\n";

        }
        

    }
    private void copyMapToMapGraph()
    {

        for (int x = 0; x < width; x++)
        {
       
            for (int z = 0; z < depth; z++)
            {
                mapGraph[x, z] = map[x, z];
               

            }
            

        }

    }
    void DrawMap()
    {
        bool handleFlag = false;
        int endPointKey = -1;
        GameObject endPointPinter = null;
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {

                if (map[x, z] == 1)
                {
                    /*
                    Vector3 pos = new Vector3(x*scale, 0, z*scale);
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    wall.transform.localScale = new Vector3(scale, scale, scale);
                    wall.transform.position = pos;
                    wall.transform.SetParent(mazeModel.transform);
                    */
                }
                else if (Search2D(x, z, Pattern.V_CORRIDOR.getPattern()))
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    Instantiate(Corridor, pos, Quaternion.identity, mazeModel.transform);


                }
                else if (Search2D(x, z, Pattern.H_CORRIDOR.getPattern()))
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject go = Instantiate(Corridor, pos, Quaternion.identity, mazeModel.transform);
                    go.transform.Rotate(0, 90, 0);


                }
                else if (Search2D(x, z, Pattern.V_T_END_PIECE.getPattern()))
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject go = Instantiate(DeadEnd, pos, Quaternion.identity, mazeModel.transform);
                    MazeNodeInfo node = new MazeNodeInfo(x, z, Pattern.V_T_END_PIECE);

                    mazeGraph.addNode(node);
                    go.GetComponentInChildren<HandleTriggerController>().node = node;
                    mapGraph[x, z] = 5;//5 to indicate node
                    if (!handleFlag)
                    {
                        handleFlag = true;
                        node.setStartPoint();
                        startPoint.transform.position = pos;
                        go.GetComponentInChildren<Renderer>().materials[0].color = Color.green;
                        Handle = Instantiate(handlePrefab, pos, Quaternion.identity, mazeModel.transform);
                    }
                    else
                    {
                        endPoint.transform.position = pos;
                        endPointPinter = go;
                        endPointKey = node.nodeNumber;
                        //go.GetComponentInChildren<Renderer>().materials[0].color =Color.red;
                    }
                    //node.createNodeVisual(mazeGraphVisual, pos, scale, Vector3.zero);

                }
                else if (Search2D(x, z, Pattern.V_B_END_PIECE.getPattern()))
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject go = Instantiate(DeadEnd, pos, Quaternion.identity, mazeModel.transform);
                    go.transform.Rotate(0, 180, 0);
                    MazeNodeInfo node = new MazeNodeInfo(x, z, Pattern.V_B_END_PIECE);

                    mazeGraph.addNode(node);
                    go.GetComponentInChildren<HandleTriggerController>().node = node;
                    mapGraph[x, z] = 5;//5 to indicate node
                    if (!handleFlag)
                    {
                        handleFlag = true;
                        node.setStartPoint();
                        startPoint.transform.position = pos;
                        go.GetComponentInChildren<Renderer>().materials[0].color = Color.green;
                        Handle = Instantiate(handlePrefab, pos, Quaternion.identity, mazeModel.transform);
                    }
                    else
                    {
                        endPoint.transform.position = pos;
                        endPointPinter = go;
                        endPointKey = node.nodeNumber;
                        //go.GetComponentInChildren<Renderer>().materials[0].color =Color.red;
                    }
                    //node.createNodeVisual(mazeGraphVisual, pos, scale, new Vector3(0, 180, 0));
                }
                else if (Search2D(x, z, Pattern.H_L_END_PIECE.getPattern()))
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject go = Instantiate(DeadEnd, pos, Quaternion.identity, mazeModel.transform);
                    go.transform.Rotate(0, -90, 0);
                    MazeNodeInfo node = new MazeNodeInfo(x, z, Pattern.H_L_END_PIECE);

                    mazeGraph.addNode(node);
                    go.GetComponentInChildren<HandleTriggerController>().node = node;
                    mapGraph[x, z] = 5;//5 to indicate node
                    if (!handleFlag)
                    {
                        handleFlag = true;
                        node.setStartPoint();
                        startPoint.transform.position = pos;
                        go.GetComponentInChildren<Renderer>().materials[0].color = Color.green;
                        Handle = Instantiate(handlePrefab, pos, Quaternion.identity, mazeModel.transform);
                    }
                    else
                    {
                        endPoint.transform.position = pos;
                        endPointPinter = go;
                        endPointKey = node.nodeNumber;
                        //go.GetComponentInChildren<Renderer>().materials[0].color =Color.red;
                    }
                    //node.createNodeVisual(mazeGraphVisual, pos, scale, new Vector3(0, -90, 0));
                }
                else if (Search2D(x, z, Pattern.H_R_END_PIECE.getPattern()))
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject go = Instantiate(DeadEnd, pos, Quaternion.identity, mazeModel.transform);
                    go.transform.Rotate(0, 90, 0);
                    MazeNodeInfo node = new MazeNodeInfo(x, z, Pattern.H_R_END_PIECE);

                    mazeGraph.addNode(node);
                    go.GetComponentInChildren<HandleTriggerController>().node = node;
                    mapGraph[x, z] = 5;//5 to indicate node
                    if (!handleFlag)
                    {
                        handleFlag = true;
                        node.setStartPoint();
                        startPoint.transform.position = pos;
                        go.GetComponentInChildren<Renderer>().materials[0].color = Color.green;
                        Handle = Instantiate(handlePrefab, pos, Quaternion.identity, mazeModel.transform);
                    }
                    else
                    {
                        endPoint.transform.position = pos;
                        endPointPinter = go;
                        endPointKey = node.nodeNumber;
                        //go.GetComponentInChildren<Renderer>().materials[0].color =Color.red;
                    }
                    // node.createNodeVisual(mazeGraphVisual, pos, scale, new Vector3(0, 90, 0));
                }
                else if (Search2D(x, z, Pattern.T_R_CORNER.getPattern()))
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject go = Instantiate(Corner, pos, Quaternion.identity, mazeModel.transform);
                    MazeNodeInfo node = new MazeNodeInfo(x, z, Pattern.T_R_CORNER);

                    mazeGraph.addNode(node);
                    go.GetComponentInChildren<HandleTriggerController>().node = node;
                    mapGraph[x, z] = 5;//5 to indicate node
                    // node.createNodeVisual(mazeGraphVisual, pos, scale, Vector3.zero);
                }
                else if (Search2D(x, z, Pattern.T_L_CORNER.getPattern()))
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject go = Instantiate(Corner, pos, Quaternion.identity, mazeModel.transform);
                    go.transform.Rotate(0, -90, 0);
                    MazeNodeInfo node = new MazeNodeInfo(x, z, Pattern.T_L_CORNER);
                    mazeGraph.addNode(node);
                    go.GetComponentInChildren<HandleTriggerController>().node = node;
                    mapGraph[x, z] = 5;//5 to indicate node
                    //node.createNodeVisual(mazeGraphVisual, pos, scale, new Vector3(0, -90, 0));

                }
                else if (Search2D(x, z, Pattern.B_R_CORNER.getPattern()))
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject go = Instantiate(Corner, pos, Quaternion.identity, mazeModel.transform);
                    go.transform.Rotate(0, 90, 0);
                    MazeNodeInfo node = new MazeNodeInfo(x, z, Pattern.B_R_CORNER);

                    mazeGraph.addNode(node);
                    go.GetComponentInChildren<HandleTriggerController>().node = node;
                    mapGraph[x, z] = 5;//5 to indicate node
                                       // node.createNodeVisual(mazeGraphVisual, pos, scale, new Vector3(0, 90, 0));

                }
                else if (Search2D(x, z, Pattern.B_L_CORNER.getPattern()))
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject go = Instantiate(Corner, pos, Quaternion.identity, mazeModel.transform);
                    go.transform.Rotate(0, 180, 0);
                    MazeNodeInfo node = new MazeNodeInfo(x, z, Pattern.B_L_CORNER);

                    mazeGraph.addNode(node);
                    go.GetComponentInChildren<HandleTriggerController>().node = node;
                    mapGraph[x, z] = 5;//5 to indicate node
                    // node.createNodeVisual(mazeGraphVisual, pos, scale, new Vector3(0, 180, 0));
                }
                else if (Search2D(x, z, Pattern.T_TJUNCTION.getPattern()))
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject go = Instantiate(TJunction, pos, Quaternion.identity, mazeModel.transform);
                    MazeNodeInfo node = new MazeNodeInfo(x, z, Pattern.T_TJUNCTION);

                    mazeGraph.addNode(node);
                    go.GetComponentInChildren<HandleTriggerController>().node = node;
                    mapGraph[x, z] = 5;//5 to indicate node
                    //node.createNodeVisual(mazeGraphVisual, pos, scale, Vector3.zero);
                }
                else if (Search2D(x, z, Pattern.B_TJUNCTION.getPattern()))
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject go = Instantiate(TJunction, pos, Quaternion.identity, mazeModel.transform);
                    go.transform.Rotate(0, 180, 0);
                    MazeNodeInfo node = new MazeNodeInfo(x, z, Pattern.B_TJUNCTION);

                    mazeGraph.addNode(node);
                    go.GetComponentInChildren<HandleTriggerController>().node = node;
                    mapGraph[x, z] = 5;//5 to indicate node
                    //node.createNodeVisual(mazeGraphVisual, pos, scale, new Vector3(0, 180, 0));
                }
                else if (Search2D(x, z, Pattern.R_TJUNCTION.getPattern()))
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject go = Instantiate(TJunction, pos, Quaternion.identity, mazeModel.transform);
                    go.transform.Rotate(0, 90, 0);
                    MazeNodeInfo node = new MazeNodeInfo(x, z, Pattern.R_TJUNCTION);

                    mazeGraph.addNode(node);
                    go.GetComponentInChildren<HandleTriggerController>().node = node;
                    mapGraph[x, z] = 5;//5 to indicate node
                    //node.createNodeVisual(mazeGraphVisual, pos, scale, new Vector3(0, 90, 0));
                }
                else if (Search2D(x, z, Pattern.L_TJUNCTION.getPattern()))
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject go = Instantiate(TJunction, pos, Quaternion.identity, mazeModel.transform);
                    go.transform.Rotate(0, -90, 0);
                    MazeNodeInfo node = new MazeNodeInfo(x, z, Pattern.R_TJUNCTION);

                    mazeGraph.addNode(node);
                    go.GetComponentInChildren<HandleTriggerController>().node = node;
                    mapGraph[x, z] = 5;//5 to indicate node
                    //node.createNodeVisual(mazeGraphVisual, pos, scale, new Vector3(0, -90, 0));

                }
                else if (Search2D(x, z, Pattern.CROSSROAD.getPattern()))
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject go = Instantiate(Crossroad, pos, Quaternion.identity, mazeModel.transform);
                    MazeNodeInfo node = new MazeNodeInfo(x, z, Pattern.CROSSROAD);

                    mazeGraph.addNode(node);
                    go.GetComponentInChildren<HandleTriggerController>().node = node;
                    mapGraph[x, z] = 5;//5 to indicate node
                    //node.createNodeVisual(mazeGraphVisual, pos, scale, Vector3.zero);


                }

                if (Handle != null)
                {
                    mazeModel.GetComponent<ModelGameObject>().Handle = Handle;
                }




            }
        }
        if (endPointPinter)
        {
            endPointPinter.GetComponentInChildren<Renderer>().materials[0].color = Color.red;
            mazeGraph.getNode(endPointKey).setEndPoint();
        }
        SetupGraph();

        //mazeModel.transform.localScale = new Vector3(0.16f, 0, 0.16f);

    }


    private void logVector(int count, Vector3 pos)
    {
        if (count < 10 && Random.Range(0, 5) % 2 == 0)
        {
            count++;
            //Debug.Log("pos "+pos);
        }
    }
    public GameObject getStartPoint()
    {

        return startPoint;
    }
    public GameObject getEndPoint()
    {
        return endPoint;
    }
    public GameObject getHandle()
    {
        return Handle;
    }
    bool Search2D(int c, int r, int[] pattern)
    {
        int matchCount = 0;
        int pos = 0;
        /*
         *      |0  1   2|      |(x-1,z+1)  (x,z+1)  (x+1,z+1)|     
         *      |3  4   5| =>   |(x-1,z)    (x,z)    (x+1,z)  |   
         *      |6  7   8|      |(x-1,z-1)  (x,z-1)  (x+1,z-1)|   
         */
        for (int z = 1; z > -2; z--)
        {
            for (int x = -1; x < 2; x++)
            {
                if (pattern[pos] == map[c + x, r + z] || pattern[pos] == 5)// 5 is a wildcard
                    matchCount++;
                pos++;
            }
        }
        return (matchCount == 9);

    }
    public virtual void Generate()
    {
        for (int z = 0; z < depth; z++)
            for (int x = 0; x < width; x++)
            {
                if (Random.Range(0, 100) < 50)
                    map[x, z] = 0; // 1:wall, 0: corridor

            }

    }
    public int CountSquareNeighbors(int x, int z)
    {
        int count = 0;
        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
        if (map[x - 1, z] == 0) count++;
        if (map[x + 1, z] == 0) count++;
        if (map[x, z - 1] == 0) count++;
        if (map[x, z + 1] == 0) count++;
        return count;
    }

    public int CountDiagonalNeigbors(int x, int z)
    {
        int count = 0;
        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
        if (map[x - 1, z - 1] == 0) count++;
        if (map[x + 1, z + 1] == 0) count++;
        if (map[x + 1, z - 1] == 0) count++;
        if (map[x - 1, z + 1] == 0) count++;
        return count;

    }

    public int CountAllNeighbors(int x, int z)
    {
        return CountSquareNeighbors(x, z) + CountDiagonalNeigbors(x, z);
    }
}
