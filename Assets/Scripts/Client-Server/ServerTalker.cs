using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System;
using System.Text;
using TMPro;

/// <summary>
/// This class is responsible for the Client server communication, it process new session data from the server, and send back new data to the server
/// </summary>
public class ServerTalker : MonoBehaviour
{
    public static ServerTalker instance = null;
    public static ServerTalker Instance { get { return instance; } }

    public string gameSessionURL = "http://localhost:3000/gameSession/";

    private string savedPatientID;
    #region Unity Messages
    /// <summary>
    /// Awake method for this class, is for the use to singleton pattern implemention
    /// </summary>
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            savedPatientID = "";
            DontDestroyOnLoad(this.gameObject);
        }
    }

    /// <summary>
    /// Posting the maze data back to the server
    /// </summary>
    /// <param name="MazeID"></param>
    /// <returns></returns>

    IEnumerator PostMazesData(Dictionary<int, MazeDataObject> mazeDataObjects)
    {
        TherapySession currentTherapy = SessionManager.instance.CurrentGameSession as TherapySession;
        string url = gameSessionURL + "mazes/" + currentTherapy.sessionID;
        List<int> mazeNumbers = new List<int>();
        bool isFailedToProcess = false;
        foreach (KeyValuePair<int, MazeDataObject> mazeData in mazeDataObjects)
        {
            mazeNumbers.Add(mazeData.Value.MazeSerializer);
            WWWForm form = new WWWForm();
            form.AddField("sessionID", mazeData.Value.SessionID.ToString());
            form.AddField("patientID", currentTherapy.registeredPatient);
            form.AddField("mazeSerializer", mazeData.Value.MazeSerializer);
            form.AddField("mazeSize", mazeData.Value.MazeSize);
            form.AddField("mazeType", mazeData.Value.MazeType);
            form.AddField("score", mazeData.Value.Score.ToString());
            form.AddField("numberOfTries", mazeData.Value.NumberOftries);
            form.AddField("finishTime", mazeData.Value.FinishTime);
            form.AddField("graph", mazeData.Value.Graph);
            form.AddField("shortestPath", mazeData.Value.ShortestPath);
            form.AddField("playerPath", mazeData.Value.PlayerPath);
            form.AddField("dominantHand", mazeData.Value.DominantHand.ToString().ToLower());
            form.AddField("orientation", mazeData.Value.Orientation.ToString().ToLower());


            using (UnityWebRequest www = UnityWebRequest.Post(url, form))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                    isFailedToProcess = true;
                }
                else
                {
                    Debug.Log("Uploaded maze #" + mazeData.Value.MazeSerializer + " from Session " + mazeData.Value.SessionID);
                }
            }
        }
        if (!isFailedToProcess)
        {
            StartCoroutine(PostSessionData(mazeNumbers));
        }
        else
        {

            MenuController.instance.submitFailPanel.SetActive(true);
        }

    }

    /// <summary>
    /// Posting the session data back to the server
    /// </summary>
    /// <param name="MazeID"></param>
    /// <returns></returns>

    IEnumerator PostSessionData(List<int> MazeID)
    {
        TherapySession currentTherapy = SessionManager.instance.CurrentGameSession as TherapySession;
        string url = gameSessionURL + currentTherapy.RegisteredPatient;
        Debug.Log(url);
        WWWForm form = new WWWForm();
        form.AddField("sessionID", currentTherapy.sessionID);
        form.AddField("amountOfMazes", currentTherapy.amountOfMazes.ToString());
        form.AddField("averageScore", currentTherapy.averageScore.ToString());
        form.AddField("averageTime", currentTherapy.avgTime.ToString());

        string treatmentTypesString = "[" + (currentTherapy.treatmentTypes[0].ToString().ToLower()) + ", " + currentTherapy.treatmentTypes[1].ToString().ToLower() + ", " + currentTherapy.treatmentTypes[2].ToString().ToLower() + "]";
        form.AddField("treatmentTypes", treatmentTypesString);
        form.AddField("difficulty", currentTherapy.difficulty);
        form.AddField("dominantArm", currentTherapy.dominantArm.ToString().ToLower());
        form.AddField("mazeSizeRange", currentTherapy.mazeSizeRange.ToString());
        form.AddField("mazes", "[" + String.Join(", ", MazeID) + "]");
        form.AddField("dateCreated", currentTherapy.dateCreated);
        form.AddField("DatePlayed", DateTime.UtcNow.Date.ToString("dd/MM/yyyy"));
        form.AddField("isFinished", "true");
        form.AddField("registeredPatient", currentTherapy.registeredPatient);




        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    Debug.Log("Form upload complete!");
                    MenuController.instance.submitFailPanel.SetActive(false);
                    StartCoroutine(MenuController.instance.LoadAsyncronously(0));
                }
            }
        }
    }
    /// <summary>
    /// Getting the session Data from the server
    /// </summary>
    /// <param name="sessionURL"></param>
    /// <param name="patientID"></param>
    /// <returns></returns>
    IEnumerator GetSessionData(string sessionURL, string patientID)
    {

        string address = sessionURL + patientID;
        Debug.Log("Getting Data...." + address);
        savedPatientID = patientID;
        using (UnityWebRequest www = UnityWebRequest.Get(address))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    Debug.Log(www.downloadHandler.text);
                    ProcessServerResponse(www.downloadHandler.text);

                }
            }
        }
    }
    #endregion

    #region Methods

    /// <summary>
    /// Process The session results and transfer it to the server
    /// </summary>
    /// <param name="mazeDataObjects"></param>
    public void ProcessSessionToDatabase(Dictionary<int, MazeDataObject> mazeDataObjects)
    {
        StartCoroutine(PostMazesData(mazeDataObjects));
    }

    /// <summary>
    /// Process the session data from the server
    /// </summary>
    /// <param name="rawResponse"></param>
    void ProcessServerResponse(string rawResponse)
    {
        JSONNode node = JSON.Parse(rawResponse);

        Debug.Log(node);
        if (node.Count == 0)
        {
            //notify user there is no available session for him...
            Debug.Log("No available sessions, please try again later or go back to the main menu");
            MenuController.instance.noSessionPanel.SetActive(true);
            return;
        }

        SessionManager.instance.StoreTherapySessionData(node[0]);
    }


    public void FindSession(string patientID)
    {
        StartCoroutine(GetSessionData(gameSessionURL, patientID));
    }

    #endregion



}
