using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// Table entry object for the results table
/// </summary>
public class TableEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mazeNumberText,triesText,timeText,scoreText;
    // Start is called before the first frame update
    public void ChangeMazeNum(string input){
        mazeNumberText.text = input;
    } 

    public void ChangeTriesNum(string input){
        triesText.text=input;
    } 

    public void ChangeTime(string input){
        timeText.text=input;
    } 

    public void ChangeScore(string input){
        scoreText.text=input;
    } 


}
