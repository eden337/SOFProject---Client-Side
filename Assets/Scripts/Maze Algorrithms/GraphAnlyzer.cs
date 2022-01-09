using System.Collections;
using System.Collections.Generic;

using System.Linq;
using UnityEngine;

[System.Serializable]

public class GraphAnlyzer
{
    private List<int> bestPath;
    private int shortestDistance;
    private List<int> usersPath;
    private MazeGraph mazeGraph;

    public List<int> UsersPath { get => usersPath; set => usersPath = value; }

    public GraphAnlyzer(MazeGraph mazeGraph, MazeNodeInfo startNode, MazeNodeInfo endNode)
    {
        this.mazeGraph = mazeGraph;
        bestPath = dijkstraNodeToNode(startNode.nodeNumber, endNode.nodeNumber);
        UsersPath = new List<int>();
    }

    private List<int> dijkstraNodeToNode(int startNode, int endNode)
    {
        //setup
        int numNodes = mazeGraph.getNumberOfNodes();
        int[] pathToNode = Enumerable.Repeat(int.MaxValue, numNodes).ToArray();
        int[] parentNode = Enumerable.Repeat(-1, numNodes).ToArray();
        bool[] visted = new bool[numNodes];

        pathToNode[startNode] = 0;
        //run Algorithm to find shortest path
        for (int visitCounter = 0; visitCounter < numNodes - 1; visitCounter++)
        {
            int minNodeIndex = findMinimumNode(visted, pathToNode);
            if (minNodeIndex == -1)
            {
                Debug.LogError("failed to find Minimun");
                dijkstraNodeToNode(startNode, endNode);
                return null;
            }
            
            MazeNodeInfo currNode = mazeGraph.getNode(minNodeIndex);

            foreach (int neighbor in currNode.neighborsKeys)
            {
                if (!visted[neighbor])
                {
                    int newPathDist = pathToNode[currNode.nodeNumber] + mazeGraph.getDistance(currNode.nodeNumber, neighbor);
                    if (newPathDist < pathToNode[neighbor])
                    {
                        pathToNode[neighbor] = newPathDist;
                        parentNode[neighbor] = currNode.nodeNumber;

                    }
                }
            }
            visted[currNode.nodeNumber] = true;


        }
        shortestDistance = pathToNode[endNode];
        List<int> shortestPath = new List<int>();
        createShortestPath(endNode, parentNode, shortestPath);
        /*if (shortestPath.Count == 1)
        {
            dijkstraNodeToNode(startNode, endNode);

            
        }*/
        return shortestPath;




    }

    private void createShortestPath(int node, int[] parent, List<int> path)
    {
        if (node == -1)
            return;
        createShortestPath(parent[node], parent, path);
        path.Add(node);

    }
    private int findMinimumNode(bool[] visited, int[] pathes)
    {
        int minIndex = -1;
        int currMin = int.MaxValue;
        for (int i = 0; i < pathes.Length; i++)
        {
            if (!visited[i] && pathes[i] < currMin)
            {
                minIndex = i;
                currMin = pathes[i];
            }
        }
        return minIndex;
    }
    private int countDirectionChanges(List<int> path)
    {
        if (path.Count < 3)
            return 0;
        int cnt = 0;
        for (int i = 0; i < path.Count - 2; i++)
        {
            int changeDirection = mazeGraph.changeDirection(path[i], path[i + 1], path[i + 2]);
            if (changeDirection == -1)
            {
                //execption or something
                return -1;
            }
            else
                cnt += changeDirection;

        }
        return cnt;
    }
    private int countExtraPasses(List<int> path)
    {

        int cnt = 0;
        for (int i = 0; i < path.Count - 1; i++)
        {
            int passes = mazeGraph.getEdge(path[i], path[i + 1]).getPassed();
            if (passes == -1)
                return -1;
            cnt += (passes - 1);


        }
        return cnt;
    }
    private int countDistance(List<int> path)
    {
        int cnt = 0;
        for (int i = 0; i < path.Count - 1; i++)
        {
            int distance = mazeGraph.getEdge(path[i], path[i + 1]).getDistance();
            if (distance == -1)
                return -1;
            cnt += distance;

        }
        return cnt;
    }

    
    public void printSelectedPath(List<int> path){
         string pathStr= "";
        for (int i = 0; i < path.Count; i++)
        {
            pathStr += path[i] + "-->";
        }
        pathStr+=" Distance is "+ countDistance(path);
        Debug.Log(pathStr);
    }
    
    public void printPath()
    {
        string user = "";
        string best = "";
        for (int i = 0; i < UsersPath.Count; i++)
        {
            user += UsersPath[i] + "-->";
        }
        user+=" Distance is "+ countDistance(this.usersPath);
        for(int i = 0; i<bestPath.Count; i++)
        {
            best += bestPath[i] + "-->";
        }
        best+=" Distance is "+ countDistance(this.bestPath);
        Debug.Log(user+"\n"+best);

    }

    public void Analysis(MazeDataObject mazeData)
    {
        mazeData.Score = pathScoreGrader();
        Debug.Log("Printing the paths");
        printSelectedPath(usersPath);
        printSelectedPath(bestPath);
        Debug.Log("Finished printing paths");
        addtPaths(mazeData);


    }
    public void addtPaths(MazeDataObject mazeData)
    {
        string user = "";
        string best = "";
        for (int i = 0; i < UsersPath.Count; i++)
        {
            user += UsersPath[i] + "-->";
        }
        for (int i = 0; i < bestPath.Count; i++)
        {
            best += bestPath[i] + "-->";
        }
        mazeData.PlayerPath = user;
        mazeData.ShortestPath = best;

    }

    public void reset()
    {
        mazeGraph.resetAllEdgesPasses();
        usersPath.Clear();

    }

    private int pathScoreGrader()
    {
        int userDistance = countDistance(this.usersPath);
        // distanceGrading =(userdistance - bestdistance)* 100/bestDistance
        // passes = 100/bestdistance * countextrapass(userPath)
        float factor = ((float)shortestDistance/Mathf.Max(shortestDistance,userDistance));
        float distanceGrading =factor*100;
        Debug.LogFormat("shortest {0}, user {1}",shortestDistance,userDistance);
        // float passes = 100 - (countExtraPasses(usersPath)/2.0f)*factor;
        float passes = 1 - (countExtraPasses(usersPath)/2.0f);
        Debug.LogFormat("Distance Grading {0}, Passes {1}",distanceGrading,countExtraPasses(usersPath));


        // float avgGrade = (distanceGrading + (100 - passes)) / 2;
        float avgGrade = distanceGrading;
        avgGrade = Mathf.Max(avgGrade,0);
        return Mathf.CeilToInt(avgGrade);
         
    }

}