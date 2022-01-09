using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wilsons : Maze
{


    List<MapLocation> notUsed = new List<MapLocation>();
    public override void Generate()
    {
        
        // create a starting cell
        int x = Random.Range(2, width - 2);
        int z = Random.Range(2, depth - 2);
        map[x, z] = 2;
        int loops = 1000;
        while (GetAvilableCells() > 1 && loops-->0 )
        {
            RandomWalk();
        }
        

    }

    int CountSquareMazeNeighbors(int x,int z)
    {
        int count = 0;
        for(int d=0; d<directions.Count; d++)
        {
            int nx = x + directions[d].x;
            int nz = z + directions[d].z;
            if (map[nx, nz] == 2)
            {
                count++;
            }
        }
        return count;
    }

    
    int GetAvilableCells()
    {
        notUsed.Clear();
        for(int z=1;z<depth-1;z++)
            for(int x = 1; x < width - 1; x++)
            {
                if (CountSquareMazeNeighbors(x, z) == 0)
                {

                    notUsed.Add(new MapLocation(x, z));
                }
            }
        return notUsed.Count;
    }
    void RandomWalk()
    {
        List<MapLocation> inWalk = new List<MapLocation>();
        int cx;
        int cz;
        int rStartIndex=Random.Range(0,notUsed.Count);
        cx = notUsed[rStartIndex].x;
        cz = notUsed[rStartIndex].z;
        inWalk.Add(new MapLocation( cx, cz));

        int countLoops = 0;
        bool validPath = false;
        while(cx>0 && cx<width-1  && cz>0 && cz < depth - 1 && countLoops<5000 && !validPath)
        {
            map[cx, cz] = 0;
            if (CountSquareMazeNeighbors(cx, cz) > 1)
            {
                break;
            }
            int rd = Random.Range(0, directions.Count);
            int nx=cx + directions[rd].x;
            int nz= cz+ directions[rd].z;
            if (CountSquareNeighbors(nx, nz) < 2)
            {
                cx = nx;
                cz = nz;
                inWalk.Add(new MapLocation(cx, cz));
            }
            validPath = CountSquareMazeNeighbors(cx, cz) == 1;
            


            
            countLoops++;
  
        }
        if (validPath)
        {
            map[cx, cz] = 0;
            Debug.Log("PathFound");
            foreach(MapLocation m in inWalk)
            {
                map[m.x, m.z] = 2;

            }
            inWalk.Clear();
        }
        else
        {
            foreach (MapLocation m in inWalk)
            {
                map[m.x, m.z] = 1;

            }
            inWalk.Clear();
        }
    }
}
