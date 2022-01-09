using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// This class is the Maze Manager, it collects all the maze's data from a given session 
/// </summary>

public class MazeManager : MonoBehaviour
{

    #region Instance
    public static MazeManager instance = null;
    public static MazeManager Instance { get { return instance; } }
    #endregion
    #region Cached References
    private MazeGenerator mazeGenerator;
    private TherapySession currentTherapy;
    [SerializeField] private GameObject resultCanvas, tableContent, tableEntryPrefab;
    #endregion
    #region Properties
    public Dictionary<int, MazeDataObject> mazeDataObjects;
    private int amountOfMazeFinished;
    private bool finishedSession;
    #endregion
    #region Unity Messages
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            finishedSession = false;
            resultCanvas.SetActive(false);
            amountOfMazeFinished = 0;
        }
    }
    void Start()
    {
        mazeGenerator = this.gameObject.GetComponent<MazeGenerator>();
        if (!mazeGenerator.enabledDebug)
        {
            currentTherapy = SessionManager.instance.CurrentGameSession as TherapySession;
        }
    }
    void Update()
    {
        if (amountOfMazeFinished == mazeDataObjects.Count && mazeDataObjects.Count != 0 && !finishedSession)
        {
            finishedSession = true;
            if (SessionManager.instance != null && SessionManager.instance.CurrentGameSession is TherapySession)
            {
                CalculateAverageFinishTime();
            }
            StartCoroutine(StopPlayer());
        }
    }
    IEnumerator StopPlayer()
    {
        yield return new WaitForSeconds(3);
        DisplayResultPanel();
    }

    #endregion
    #region Methods
    public void updateFinishedMazes(int mazeIndex)
    {
        amountOfMazeFinished++;
        mazeIndex++;
        if (mazeGenerator.mazesGODict.Count > mazeIndex)
        {

            mazeGenerator.mazesGODict[mazeIndex].SetActive(true);
        }

    }

    private void CalculateAverageFinishTime()
    {
        int sumOfSeconds = 0, sumOfScores = 0;
        foreach (KeyValuePair<int, MazeDataObject> maze in mazeDataObjects)
        {
            Debug.Log("Maze index :" + maze.Key + " maze finished Time: " + maze.Value.FinishTime);
            sumOfSeconds += maze.Value.FinishTime;
            sumOfScores += (int)maze.Value.Score;
        }
        currentTherapy.AvgTime = sumOfSeconds / mazeDataObjects.Count;
        currentTherapy.averageScore = sumOfScores / mazeDataObjects.Count;

    }

    void DisplayResultPanel()
    {
        foreach (KeyValuePair<int, MazeDataObject> maze in mazeDataObjects)
        {
            GameObject tableEntryGO = Instantiate(tableEntryPrefab, tableContent.transform);
            TableEntry tableEntry = tableEntryGO.GetComponent<TableEntry>();
            if (tableEntry == null)
            {
                return;
            }
            tableEntry.ChangeMazeNum(maze.Value.MazeSerializer.ToString());
            tableEntry.ChangeScore(maze.Value.Score.ToString());
            tableEntry.ChangeTime(maze.Value.FinishTime.ToString());
            tableEntry.ChangeTriesNum(maze.Value.NumberOftries.ToString());
        }

        resultCanvas.SetActive(true);
    }
    #endregion
}
