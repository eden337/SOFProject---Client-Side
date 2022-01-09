using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recursive : Maze
{
    public override void Generate()
    {
        Generate(Random.Range(1, width), Random.Range(1, depth));
    }
    void Generate(int x,int z)
    {
        if (CountSquareNeighbors(x, z) >= 2) return;
        map[x, z] = 0;
        directions.Shuffle();
        for (int i = 0; i < directions.Count; i++)
            Generate(x + directions[i].x,z +directions[i].z);
         string str = "";
    }

}
