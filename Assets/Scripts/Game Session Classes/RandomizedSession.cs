using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizedSession : GameSession
{
    /// <summary>
    /// Derived from the GameSession class, basic Session handling for manual session
    /// </summary>
    /// <param name="amountOfMazes"></param>
    /// <param name="avgTime"></param>
    /// <param name="mazeSizeRange"></param>
    /// <returns></returns>
    public RandomizedSession(int amountOfMazes, float avgTime, int mazeSizeRange) : base(amountOfMazes, avgTime, mazeSizeRange)
    {
    }

    public override string ToString()
    {
        return "Randomized Session:"+" \nAmount of mazes " + AmountOfMazes + " \nAverage Time " + AvgTime +"\nMaze Size:" + MazeSizeRange;
    }
}
