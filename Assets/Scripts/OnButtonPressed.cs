using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OnButtonPressed : MonoBehaviour
{
    public Button myButton;
    public Toggle myToggle;
    public GameObject myCanvas;


    void Start()
    {
        
        myToggle.onValueChanged.AddListener(delegate { ToggleValue(myToggle); });
    }

    

    void ToggleValue(Toggle tglValue)
    {


        myButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        Debug.Log("You have clicked Strongly Disagree for Q1 and Information is Submitted");
        myCanvas.SetActive(false);
    }

}
