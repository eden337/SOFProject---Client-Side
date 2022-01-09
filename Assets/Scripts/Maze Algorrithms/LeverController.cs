using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Lever
{
    public GameObject leverGO;
    public Rigidbody leverRb;
    public LeverController leverController;
}

/// <summary>
/// Holding the properties of the handle such is handle index and its state.
/// </summary>
public class LeverController : MonoBehaviour
{

    public int leverIndex = 0;

    public bool isLeverPulled = false;

    public void ChangeLeverState(bool state)
    {
        isLeverPulled = state;
    }

}
