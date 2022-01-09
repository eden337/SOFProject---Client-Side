using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// This class is responisble for the collision detection between the Node Trigger Prefab to the Handle collider
/// This class is very important for the event handling, when the handle is reaching a node trigger, then an event will occur.
/// </summary>

public class HandleTriggerController : MonoBehaviour
{

    public Event handleMoved, handleExit;
    [SerializeReference]
    public MazeNodeInfo node;
    public bool eventFlag=true;

    public TextMeshProUGUI NodeNumberText;
    private TraveledNode traveledNode;
    private ModelGameObject mazeModel;
    public bool showTextInNodes;
    private void Start()
    {
        mazeModel = this.gameObject.GetComponentInParent<ModelGameObject>();
        eventFlag = true;

        if(!showTextInNodes){
            NodeNumberText.gameObject.SetActive(false);
        }else{
            NodeNumberText.gameObject.SetActive(true);
        }
    }
    private void OnTriggerEnter(Collider other) {
        if(other.tag=="Handle"){
            if(eventFlag){
                traveledNode = new TraveledNode(node.nodeNumber,mazeModel.side,this.gameObject.transform, mazeModel.mazeSerializer);
                NodeNumberText.text=traveledNode.number.ToString();
                handleMoved.Occurred(traveledNode);
            }
            eventFlag = false;
        }
    }

    private void OnTriggerExit(Collider other) {
         if(other.tag=="Handle"&& !eventFlag){

            handleExit.Occurred(traveledNode);
            eventFlag = true;
        }
    }
}
