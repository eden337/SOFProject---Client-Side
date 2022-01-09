using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Main idea is to move the player to every door
/// </summary>
public class FollowWP : MonoBehaviour
{
    public GameObject[] waypoints;
    int currentWP = 0;
    bool isStopped = false;
    public float speed = 10.0f;
    GameObject currentMaze;
    // Start is called before the first frame update
    void Start()
    {
        isStopped = false;
        currentWP = 0;
        waypoints = GameObject.FindGameObjectsWithTag("Maze Doors");
    }

    // Update is called once per frame
    void Update()
    {
        CheckMazeStatus();

        if (currentWP == waypoints.Length)
        {
            Stop();
            isStopped = true;

        }
        if (!isStopped && currentMaze == null)
        {
            Move();

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Maze"))
        {
            currentMaze = other.gameObject;
            currentWP++;
            isStopped = true;
        }
    }

    void CheckMazeStatus()
    {
        if (currentMaze == null && currentWP != waypoints.Length)
        {
            isStopped = false;
        }
    }



    public void Stop()
    {
        this.transform.Translate(0, 0, 0);

    }

    void Move()
    {
        this.transform.Translate(0, 0, speed * Time.deltaTime);
    }
}
