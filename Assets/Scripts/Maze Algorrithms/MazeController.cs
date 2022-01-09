using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;


/// <summary>
/// This class is responsible to handle all to initialize maze properties during gameplay, and control the maze behaviour
/// </summary>
public class MazeController : MonoBehaviour
{
    #region Cached References
    public MazeGenerator generator;
    private GameObject mazeModel, mirroredMazeModel;
    private GameObject particleClone;
    public MazeDataObject mazeDataObject;
    private Model model, mirroredModel;
    private GraphAnlyzer graphAnlyzer;
    [SerializeField] private Animator doorAnim;
    [SerializeField] private GameObject particles;
    [SerializeField] private TextMeshProUGUI TimerText;
    [SerializeField] private XRGrabInteractable leftHandInteractable, rightHandInteractable;
    #endregion
    #region Maze Properties
    private float mazeTimer = 100, startingTime = 60, coordinationTimer = 5, mazeRunTimer = 0;
    private bool hasSpawned;
    public bool isFinished, finishedProcess;
    public int mazeIndex;
    private bool startCurrentMazeTimer, changeScore;
    private int tries;
    private int score;
    #endregion
    #region Unity Messages

    void OnEnable()
    {
        generator = GameObject.Find("Maze Manager").GetComponent<MazeGenerator>();
        if (SessionManager.instance != null)
        {

            mazeDataObject = generator.mazeDataObjectsDictionary[mazeIndex];
            if (SessionManager.Instance.CurrentGameSession is TherapySession)
            {
                TherapySession session = SessionManager.instance.CurrentGameSession as TherapySession;
                SetDifficulty(session.difficulty);
            }else{
                SetDifficulty("Medium");
            }

        }

        InitMaze();
    }

    // Update is called once per frame
    void Update()
    {

        if (startCurrentMazeTimer)
        {
            CalculateTimePassed();
        }
        if (model.lever.leverGO == null || mirroredModel.lever.leverGO == null) return;
        if (model.lever.leverController.isLeverPulled && mirroredModel.lever.leverController.isLeverPulled && mazeTimer > 0)
        {
            UnfreezeHandlePosition(model.lever.leverRb);
            UnfreezeHandlePosition(mirroredModel.lever.leverRb);
            UpdateTimer();
            startCurrentMazeTimer = true;
            changeScore = true;
        }
        else
        {

            if (rightHandInteractable.isSelected || leftHandInteractable.isSelected)
            {
                resetGame();
            }

            if (!isFinished)
            {
                mazeTimer = startingTime;
                enableHands();
            }
        }
        if (mazeCompleted() && !isFinished && finishedProcess)
        {
            isFinished = true;
            StopAllCoroutines();
            if (SessionManager.instance != null)
            {
                mazeDataObject.FinishTime = Mathf.CeilToInt(mazeRunTimer);
                mazeDataObject.NumberOftries = tries;
                graphAnlyzer.Analysis(mazeDataObject);

            }
            MazeManager.instance.updateFinishedMazes(mazeDataObject.MazeSerializer);
            doorAnim.SetTrigger("mazeCompleted");
            if (!hasSpawned)
            {
                particleClone = Instantiate(particles, transform.position, Quaternion.identity);
                hasSpawned = true;
                Destroy(model.lever.leverGO, 2);
                Destroy(mirroredModel.lever.leverGO, 2);
                TimerText.enabled = false;
            }
            Destroy(this.gameObject, 2f);

        }
    }


    #endregion
    #region Methods


    public void setIndex(int i)
    {
        this.mazeIndex = i;
    }

    void SetDifficulty(string diffSetting)
    {
        if (diffSetting == "Easy")
        {
            mazeTimer = 60;
            coordinationTimer = 5;
        }
        else if (diffSetting == "Medium")
        {
            mazeTimer = 30;
            coordinationTimer = 2.5f;
        }
        else
        {
            mazeTimer = 20;
            coordinationTimer = 1.5f;
        }
        startingTime = mazeTimer;
        TimerText.text = "Time left: " + startingTime;
    }


    void InitMaze(TreatmentType treatType = TreatmentType.Bilateral, bool isRightDominant = false)
    {


        model = new Model();
        isFinished = false;
        hasSpawned = false;

        mazeRunTimer = 0f;
        tries = 1;
        score = 100;
        changeScore = false;
        switch (treatType)
        {
            case (TreatmentType.Bilateral):
                {

                    mirroredModel = new Model();
                    mazeModel = generator.getModel(mazeIndex);
                    mirroredMazeModel = Instantiate(mazeModel, mazeModel.transform.position, Quaternion.identity);
                    mirroredMazeModel.GetComponent<ModelGameObject>().mazeGraph = mazeModel.GetComponent<ModelGameObject>().mazeGraph;
                    graphAnlyzer = mazeModel.GetComponent<ModelGameObject>().anlyzer;
                    graphAnlyzer.reset();
                    if (setupModel(mazeModel, model, MazeSide.Right) == false)
                    {
                        Debug.LogError("Could not init maze on the right side");
                        return;
                    }
                    if (setupModel(mirroredMazeModel, mirroredModel, MazeSide.Left) == false)
                    {
                        Debug.LogError("Could not init maze on the left side");
                        return;
                    }
                    mirroredMazeModel.SetActive(true);
                    mazeModel.SetActive(true);
                    rightHandInteractable = model.lever.leverRb.GetComponent<XRGrabInteractable>();
                    leftHandInteractable = model.lever.leverRb.GetComponent<XRGrabInteractable>();
                    break;
                }
            case (TreatmentType.Smoothness):
                {
                    return;
                }
            case (TreatmentType.Midline):
                {
                    return;
                }
            default:
                break;

        }

    }




    private bool setupModel(GameObject modelGO, Model model, MazeSide side)
    {
        model.lever = new Lever();
        modelGO.transform.SetParent(this.transform, false);
        if (side == MazeSide.Left)
            modelGO.transform.localScale = new Vector3(modelGO.transform.localScale.x, modelGO.transform.localScale.y, -modelGO.transform.localScale.z);

        modelGO.GetComponent<ModelGameObject>().side = side;
        model.currentNode = getStartNode(modelGO, modelGO.GetComponent<ModelGameObject>().StartPoint.transform, side);

        if (model.currentNode == null)
        {
            return false;
        }
        model.StartPoint = modelGO.GetComponent<ModelGameObject>().StartPoint;
        model.EndPoint = modelGO.GetComponent<ModelGameObject>().EndPoint;
        model.lever.leverGO = modelGO.GetComponent<ModelGameObject>().Handle;
        model.lever.leverController = model.lever.leverGO.GetComponent<LeverController>();
        model.lever.leverController.leverIndex = side == MazeSide.Left ? 1 : 0;
        model.lever.leverRb = model.lever.leverGO.GetComponent<Rigidbody>();
        FreezeHandlePosition(model.lever.leverRb);
        return true;


    }

    private TraveledNode getStartNode(GameObject model, Transform startPosition, MazeSide side)
    {
        MazeNodeInfo tempNode = model.GetComponent<ModelGameObject>().mazeGraph.GetStartNode();
        if (tempNode == null)
            return null;
        return new TraveledNode(tempNode.nodeNumber, side, startPosition, mazeDataObject.MazeSerializer);

    }



    private bool mazeCompleted()
    {
        return (isAroundNode(model.EndPoint.transform, model.lever.leverGO.transform, 0.025f) && isAroundNode(mirroredModel.EndPoint.transform, mirroredModel.lever.leverGO.transform, 0.025f));
    }
    private void resetGame()
    {
        mazeTimer = startingTime;
        tries++;
        relocateHandles(model.StartPoint.transform, mirroredModel.StartPoint.transform);
        graphAnlyzer.reset();
    }

    public bool isAroundNode(Transform nodeLocation, Transform handleLocation, float maxMagnatude)
    {
        return (handleLocation.position - nodeLocation.position).magnitude <= maxMagnatude;
    }
    public void enableHands()
    {
        model.lever.leverGO.transform.position = model.StartPoint.transform.position;
        mirroredModel.lever.leverGO.transform.position = mirroredModel.StartPoint.transform.position;
        rightHandInteractable.enabled = true;
        leftHandInteractable.enabled = true;
    }

    public void relocateHandles(Transform leftHandle, Transform rightHandle)
    {
        FreezeHandlePosition(model.lever.leverRb);
        FreezeHandlePosition(mirroredModel.lever.leverRb);
        rightHandInteractable.enabled = false;
        leftHandInteractable.enabled = false;
    }

    private void FreezeHandlePosition(Rigidbody handleRb1, Rigidbody handleRb2)
    {
        FreezeHandlePosition(handleRb1);
        FreezeHandlePosition(handleRb2);
    }

    private void FreezeHandlePosition(Rigidbody handleRb)
    {
        handleRb.constraints = RigidbodyConstraints.FreezeAll;
    }


    private void UnfreezeHandlePosition(Rigidbody handleRb1, Rigidbody handleRb2)
    {
        UnfreezeHandlePosition(handleRb1);
        UnfreezeHandlePosition(handleRb2);
    }

    private void UnfreezeHandlePosition(Rigidbody handleRb)
    {
        handleRb.constraints &= ~RigidbodyConstraints.FreezePositionX;
        handleRb.constraints &= ~RigidbodyConstraints.FreezePositionY;
    }

    void UpdateTimer()
    {
        float seconds;
        if (mazeTimer > 0)
        {
            mazeTimer -= Time.deltaTime;
            seconds = Mathf.FloorToInt(mazeTimer);
            TimerText.text = "Time left: " + seconds;
        }

    }

    void CalculateTimePassed()
    {
        mazeRunTimer += Time.deltaTime;
    }

    void AddNodeUserPath(int nodeNumber)
    {

        if (graphAnlyzer.UsersPath.Count == 0)
        {
            graphAnlyzer.UsersPath.Add(nodeNumber);
        }
        else if (graphAnlyzer.UsersPath[graphAnlyzer.UsersPath.Count - 1] != nodeNumber)
        {
            try
            {
                MazeEdgeInfo edge = mazeModel.GetComponent<ModelGameObject>().mazeGraph.getEdge(graphAnlyzer.UsersPath[graphAnlyzer.UsersPath.Count - 1], nodeNumber);
                if (edge != null)
                {

                    edge.addPassed();
                    graphAnlyzer.UsersPath.Add(nodeNumber);
                }
                finishedProcess = true;
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.LogError("Problem with maze " + mazeDataObject.MazeSerializer + "\n" + e.Message);
            }

            // }



        }
    }




    public void NodeEnterResponse(TraveledNode traveled)
    {
        if (traveled.mazeSerializer != mazeIndex) return;
        /// if the nodes of each sides are equal we stop the timer
        finishedProcess = false;
        switch (traveled.side)
        {
            case MazeSide.Left:
                mirroredModel.currentNode.number = traveled.number;

                break;
            case MazeSide.Right:
                model.currentNode.number = traveled.number;

                break;
            default:
                break;
        }


        if (model.currentNode.number == mirroredModel.currentNode.number)
        {
            StopAllCoroutines();
            AddNodeUserPath(traveled.number);
        }
    }

    public void NodeExitResponse(TraveledNode traveledNode)
    {
        ///start timer
        if (traveledNode.mazeSerializer != mazeIndex) return;
        StartCoroutine(delayTimer());
    }

    IEnumerator delayTimer()
    {

        yield return new WaitForSecondsRealtime(coordinationTimer);
        resetGame();

    }
    #endregion
}
