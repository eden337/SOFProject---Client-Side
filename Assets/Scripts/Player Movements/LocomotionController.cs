using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class LocomotionController : MonoBehaviour
{

    public XRController leftTeleportRay;
    public XRController rightTeleportRay;
    public InputHelpers.Button teleportActivationButton;
    public float activationThreshold =0.1f;

    public XRRayInteractor leftInteractorRay;
    public XRRayInteractor rightInteractorRay;
    public bool EnableLeftTeleport { get; set; } = true;
    public bool EnableRightTeleport { get; set; } = true;

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        Vector3 pos = new Vector3();
        Vector3 norm = new Vector3();
        int index = 0;

        bool validTarget = false;

        if(leftTeleportRay){
            bool isLeftInteractiorRayHovering = leftInteractorRay.TryGetHitInfo(ref pos, ref norm, ref index, ref validTarget);
            leftTeleportRay.gameObject.SetActive(EnableLeftTeleport&&CheckIfActivated(leftTeleportRay)&& !isLeftInteractiorRayHovering);
        }

        if (rightTeleportRay)
        {
            bool isRightInteractiorRayHovering = rightInteractorRay.TryGetHitInfo(ref pos, ref norm, ref index, ref validTarget);
            rightTeleportRay.gameObject.SetActive(EnableRightTeleport && CheckIfActivated(rightTeleportRay)&& !isRightInteractiorRayHovering);
        }
    }


    public bool CheckIfActivated(XRController controller){
        InputHelpers.IsPressed(controller.inputDevice,teleportActivationButton,out bool isActivated, activationThreshold);
        return isActivated;
    }
}
