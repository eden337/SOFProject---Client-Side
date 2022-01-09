using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prims : Maze
{
    public override void Generate()
    {
        int x = 2;
        int z = 2;
        map[x, z] = 0;
        List<MapLocation> walls = new List<MapLocation>();
        AddNeighborsToList(walls, x, z);
        int countLoops = 0;
        while(walls.Count>0 && countLoops < 5000)
        {
            int rwall = Random.Range(0, walls.Count);
            x = walls[rwall].x;
            z = walls[rwall].z;
            walls.RemoveAt(rwall);
            if (CountSquareNeighbors(x, z) == 1)
            {
                map[x, z] = 0;
                AddNeighborsToList(walls, x, z);

            }
            countLoops++;
        }
    }
    public void AddNeighborsToList(List<MapLocation> walls,int x,int z)
    {
        walls.Add(new MapLocation(x + 1, z));
        walls.Add(new MapLocation(x - 1, z));
        walls.Add(new MapLocation(x, z - 1));
        walls.Add(new MapLocation(x, z + 1));
    }
}
