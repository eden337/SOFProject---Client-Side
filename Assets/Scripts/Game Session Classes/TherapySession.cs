using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

using System;

[Serializable]
/// <summary>
/// Handling the Therapy session Parameters
/// </summary>
public class TherapySession : GameSession
{
    public int sessionID;

    public float averageScore = 0;

    public bool[] treatmentTypes;

    public string difficulty = "Easy";
    public bool dominantArm;

    public string[] mazeIDList;

    public string dateCreated;
    public string datePlayed;
    public bool isFinished;
    public string registeredPatient;

    public TherapySession(int sessionID, float averageScore, int amountOfMazes, float avgTime, JSONNode treatmentTypes,
    string difficulty, bool dominantArm, int mazeSizeRange, JSONNode mazeIDList, string dateCreated, string datePlayed, bool isFinished,
    string registeredPatient) : base(amountOfMazes, avgTime, mazeSizeRange)
    {

        this.sessionID=sessionID;

        this.averageScore = averageScore;
        this.treatmentTypes = new bool[treatmentTypes.Count];
        for (int i = 0; i < treatmentTypes.Count; i++)
        {
            this.treatmentTypes[i] = treatmentTypes[i].AsBool;
        }
        this.difficulty = difficulty;
        this.dominantArm = dominantArm;
        this.mazeIDList = new string[mazeIDList.Count];
        for (int i = 0; i < mazeIDList.Count; i++)
        {
            this.mazeIDList[i] = mazeIDList[i];
        }
        this.dateCreated = dateCreated;
        this.datePlayed = datePlayed;
        this.isFinished = isFinished;
        this.registeredPatient = registeredPatient;

    }


    public int SessionID{
        get=>sessionID;
        set{
            sessionID=value;
        }
    }

    public float AverageScore
    {
        get => averageScore;
        set
        {
            averageScore = value;
        }
    }


    public bool[] TreatmentTypes
    {
        get => treatmentTypes;
        set
        {
            treatmentTypes = value;
        }
    }

    public string Difficulty
    {
        get => difficulty;
        set
        {
            difficulty = value;
        }
    }

    public bool DominantArm
    {
        get => dominantArm;
        set
        {
            dominantArm = value;
        }
    }
    public string[] MazeIDList
    {
        get => mazeIDList;
        set
        {
            mazeIDList = value;
        }
    }
    public string DateCreated
    {
        get => dateCreated;
        set
        {
            dateCreated = value;
        }
    }

    public string DatePlayed
    {
        get => datePlayed;
        set
        {
            datePlayed = value;
        }
    }
    public bool IsFinished
    {
        get => isFinished;
        set
        {
            isFinished = value;
        }
    }
    public string RegisteredPatient
    {
        get => registeredPatient;
        set
        {
            registeredPatient = value;
        }
    }

    public override string ToString()
    {
        return "Therapy Session:\n" + "Session ID: "+sessionID+" \nAverage score " + AverageScore + " \nAmount of mazes " + AmountOfMazes + " \nAverage Time " + AvgTime +
        " \nTreatment Types " + TreatmentTypes + " \nDifficulty" + Difficulty + " \nDominantArm " + DominantArm + " \nMaze Size:" + MazeSizeRange +
        " \nMaze ID List " + MazeIDList + " \nDate Created " + DateCreated + " \nDate Played " + DatePlayed + " \nIs Finished " + IsFinished +
        " \n Registered Patient: " + RegisteredPatient;
    }

}
