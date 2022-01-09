using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
/// <summary>
/// relevant for future development of this project
/// </summary>
public class Climber : MonoBehaviour
{
    private CharacterController character;
    public static XRController climbingHand;
    private ContinuosMovement continuosMovement;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        continuosMovement = GetComponent<ContinuosMovement>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (climbingHand)
        {
            continuosMovement.enabled = false;
            Climb();
        }
        else
        {
            continuosMovement.enabled = true;
        }
    }
    
    //Climbing Computations
    void Climb()
    {
        InputDevices.GetDeviceAtXRNode(climbingHand.controllerNode).TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocity);
        character.Move(transform.rotation*-velocity * Time.fixedDeltaTime);

    }
}
