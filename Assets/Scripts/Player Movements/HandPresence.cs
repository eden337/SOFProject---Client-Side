using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Hand visual representation
/// </summary>
public class HandPresence : MonoBehaviour
{
    public bool showController = false;
    public InputDeviceCharacteristics controllerCharacteristics;
    // Start is called before the first frame update
    public List<GameObject> controllerPrefabs;
    public GameObject handModelPrefab;
    private InputDevice targetDevice;
    private GameObject spawnedController;
    private GameObject spawnedHandModel;
    private Animator handAnimator;

    public bool isGripped = false;

    void Start()
    {
        TryInitialize();
    }



    void TryInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>();


        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        foreach (var item in devices)
        {

        }

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);
            if (prefab)
            {
                spawnedController = Instantiate(prefab, transform);
            }
            else
            {
                Debug.LogError("Did not find corresponding controller model");
                spawnedController = Instantiate(controllerPrefabs[0], transform);
            }
        }
        if (!spawnedHandModel)
        {
            spawnedHandModel = Instantiate(handModelPrefab, transform);
            handAnimator = spawnedHandModel.GetComponent<Animator>();
        }
    }


    void UpdateHandAnimation()
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
            isGripped = true;
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
            isGripped = false; 
        }
    }

    // Update is called once per frame
    void Update()
    {

        // if(targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue)&&primaryButtonValue){
        //     //Debug.Log("Pressing primary button");
        // }


        // if(targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue)&&triggerValue>0.1f){
        //     //Debug.Log("Pressing trigger "+ triggerValue);
        // }


        // if(targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue)&&primary2DAxisValue!=Vector2.zero){
        //     //Debug.Log("Primary Touchpad "+ primary2DAxisValue);
        // }

        if (!targetDevice.isValid)
        {
            TryInitialize();
        }
        else
        {
            if (showController)
            {
                spawnedHandModel.SetActive(false);
                spawnedController.SetActive(true);
            }
            else
            {
                spawnedController.SetActive(false);
                spawnedHandModel.SetActive(true);
                UpdateHandAnimation();
            }
        }



    }
}
