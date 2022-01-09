using UnityEngine;
using TMPro;


/// <summary>
/// This Class is the display of the ID number which the user is inputing
/// </summary>
public class IDDisplay : MonoBehaviour
{
    private string sequence;
    [SerializeField] TMP_InputField idTextField;
    void Start()
    {
        sequence = "";
        idTextField = GetComponent<TMP_InputField>();
        PushTheButton.ButtonPressed += AddDigitToSequence;
    }

    private void AddDigitToSequence(string digitEntered)
    {
        if (sequence.Length > 0)
        {
            // placeHolderText.text = "";
        }
        if (sequence.Length < 9)
        {
            switch (digitEntered)
            {
                case "Zero":
                    sequence += "0";
                    Debug.Log(sequence);
                    idTextField.text = sequence;
                    break;
                case "One":
                    sequence += "1";
                    idTextField.text = sequence;
                    break;
                case "Two":
                    sequence += "2";
                    idTextField.text = sequence;
                    break;
                case "Three":
                    sequence += "3";
                    idTextField.text = sequence;
                    break;
                case "Four":
                    sequence += "4";
                    idTextField.text = sequence;
                    break;
                case "Five":
                    sequence += "5";
                    idTextField.text = sequence;
                    break;
                case "Six":
                    sequence += "6";
                    idTextField.text = sequence;
                    break;
                case "Seven":
                    sequence += "7";
                    idTextField.text = sequence;
                    break;
                case "Eight":
                    sequence += "8";
                    idTextField.text = sequence;
                    break;
                case "Nine":
                    sequence += "9";
                    idTextField.text = sequence;
                    break;

                default:
                    break;
            }

        }

        switch (digitEntered)
        {
            case "Clear":
                sequence = "";
                idTextField.text = sequence;
                break;
            case "Del":
                if (sequence.Length > 0)
                {
                    sequence = sequence.Substring(0, sequence.Length - 1);
                }
                idTextField.text = sequence;
                break;
            case "Submit":
                Debug.Log(sequence);
                ServerTalker.instance.FindSession(sequence);
                break;

        }
    }

    private void OnDestroy()
    {
        PushTheButton.ButtonPressed -= AddDigitToSequence;
    }
}
