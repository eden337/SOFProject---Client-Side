using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

/// <summary>
/// Session Manager is the manager of the session which were generated during run time, only one instance of this class is exist.
/// </summary>
public class SessionManager : MonoBehaviour
{
    public static SessionManager instance = null;

    public static SessionManager Instance { get { return instance; } }

    public GameSession CurrentGameSession { get => currentGameSession; set => currentGameSession = value; }

    private GameSession currentGameSession;

    public MenuController menuController;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    #region Store all session data


    public void StoreTherapySessionData(JSONNode sessionNode)
    {

        CurrentGameSession = new TherapySession(sessionNode["sessionID"],
        sessionNode["averageScore"].AsFloat,
        sessionNode["amountOfMazes"].AsInt,
        sessionNode["averageTime"].AsFloat,
        sessionNode["treatmentTypes"],
        sessionNode["difficulty"],
        sessionNode["dominantArm"].AsBool,
        sessionNode["mazeSizeRange"].AsInt,
        sessionNode["mazes"],
        sessionNode["dateCreated"],
        sessionNode["DatePlayed"],
        sessionNode["false"].AsBool,
        sessionNode["registeredPatient"]
        );
        Debug.Log(CurrentGameSession.ToString());
        MenuController.instance.LoadTherapySession();
    }

    public void StoreRandomizedSessionData(int amountOfMazes,  int mazeSizeRange, float avgTime=0.0f)
    {

        CurrentGameSession = new RandomizedSession(amountOfMazes,avgTime,mazeSizeRange);
        Debug.Log(CurrentGameSession.ToString());
        
    
    }
    #endregion


}
