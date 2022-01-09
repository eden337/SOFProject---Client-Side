
/// <summary>
/// This data object is for saving the progress of each solved maze
/// </summary>

public class MazeDataObject
{
    #region Properties
    int sessionID;
    int mazeSerializer;

    int mazeSize;

    int mazeType;

    float score;

    int numberOftries;

    int finishTime;

    string graph, shortestPath, playerPath;

    bool dominantHand;

    bool orientation;
    #endregion
    #region Constuctor
    public MazeDataObject(int sessionID, int mazeSerializer, int mazeSize, int mazeType,
    string graph = "", string shortestPath = "", string playerPath = "", bool dominantHand = false, bool orientation = false)
    {
        this.SessionID = sessionID;
        this.MazeSerializer = mazeSerializer;
        this.MazeSize = mazeSize;
        this.MazeType = mazeType;
        this.Score = 0;
        this.NumberOftries = 0;
        this.FinishTime = 0;
        this.Graph = graph;
        this.ShortestPath = shortestPath;
        this.PlayerPath = playerPath;
        this.DominantHand = dominantHand;
        this.Orientation = orientation;
    }
    #endregion
    #region Setters and Getters
    public int SessionID { get => sessionID; set => sessionID = value; }
    public int MazeSerializer { get => mazeSerializer; set => mazeSerializer = value; }
    public int MazeSize { get => mazeSize; set => mazeSize = value; }
    public int MazeType { get => mazeType; set => mazeType = value; }
    public float Score { get => score; set => score = value; }
    public int NumberOftries { get => numberOftries; set => numberOftries = value; }
    public int FinishTime { get => finishTime; set => finishTime = value; }
    public string Graph { get => graph; set => graph = value; }
    public string ShortestPath { get => shortestPath; set => shortestPath = value; }
    public string PlayerPath { get => playerPath; set => playerPath = value; }
    public bool DominantHand { get => dominantHand; set => dominantHand = value; }
    public bool Orientation { get => orientation; set => orientation = value; }
    #endregion

}
