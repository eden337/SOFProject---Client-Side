using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Irrelevant for this project 
/// </summary>
public class TwoHandGrabInteractable : XRGrabInteractable
{
    public List<XRSimpleInteractable> secondHandGrabPoints = new List<XRSimpleInteractable>();
    public XRBaseInteractor secondInteractor;
    private Quaternion attachInitialRotation;
    public enum TwoHandRotationType { None,First,Second};
    public TwoHandRotationType twoHandRotationType;
    public bool snapToSecondHand = true;
    private Quaternion initialRotationOffset;
    private void Start()
    {
        foreach(var item in secondHandGrabPoints)
        {
            item.onSelectEntered.AddListener(OnSecondHandGrab);
            item.onSelectExited.AddListener(OnSecondHandRelease);
        }
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (secondInteractor && selectingInteractor)
        {
            //Compute Rotation
            if(snapToSecondHand)
                selectingInteractor.attachTransform.rotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position);
            else
            {
                selectingInteractor.attachTransform.rotation = GetTwoHandRotation() * initialRotationOffset;
            }
        }
        base.ProcessInteractable(updatePhase);
    }

    private Quaternion GetTwoHandRotation()
    {
        Quaternion targetRotation;
        switch (twoHandRotationType)
        {
            case TwoHandRotationType.First: 
            {
                    targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position, selectingInteractor.transform.up);
                    break;
            }
            case TwoHandRotationType.Second:
            {
                    targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position, secondInteractor.attachTransform.up);
                    break;
            }
            default:
                {
                    targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position);
                    break;
                }
            
        }
        return targetRotation;
    }

    public void OnSecondHandGrab(XRBaseInteractor interactor)
    {
        secondInteractor = interactor;
        initialRotationOffset = Quaternion.Inverse(GetTwoHandRotation()) * selectingInteractor.attachTransform.rotation;
    }

    public void OnSecondHandRelease(XRBaseInteractor interactor)
    {
        secondInteractor = null;
    }

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        base.OnSelectEntered(interactor);
        attachInitialRotation = interactor.attachTransform.localRotation;
    }

    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        base.OnSelectExited(interactor);
        secondInteractor = null;
        interactor.attachTransform.localRotation = attachInitialRotation;
    }

    public override bool IsSelectableBy(XRBaseInteractor interactor)
    {
        bool isAlreadyGrabbed = selectingInteractor && !interactor.Equals(selectingInteractor);
        return base.IsSelectableBy(interactor)&&!isAlreadyGrabbed;

    }
}
