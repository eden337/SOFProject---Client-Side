using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

/// <summary>
/// Clicking the buttons at the numpad panel
/// </summary>
public class PushTheButton : MonoBehaviour
{
    public static event Action<string> ButtonPressed = delegate{};
    private int deviderPosition;
    private string buttonName,buttonValue;

    private void Start() {
        buttonName=gameObject.name;
        deviderPosition=buttonName.IndexOf("_");
        buttonValue = buttonName.Substring(0,deviderPosition);

        gameObject.GetComponent<Button>().onClick.AddListener(ButtonClicked);
    }

    private void ButtonClicked(){
        ButtonPressed(buttonValue);
    }


}
