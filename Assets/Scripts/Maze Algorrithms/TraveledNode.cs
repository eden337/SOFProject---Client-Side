using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds the properties of each node which each handle had collided with
/// </summary>
public class TraveledNode{
    public int number;
    public MazeSide side;
    public Transform transform=null;

    public int mazeSerializer;

    public TraveledNode(int number, MazeSide side,Transform transform, int mazeSerializer){
        this.number=number;
        this.side=side;
        this.transform = transform;
        this.mazeSerializer= mazeSerializer;

    }
    public void setTransform(Transform transform)
    {
        this.transform = transform;
    }
}