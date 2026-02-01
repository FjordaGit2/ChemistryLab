using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class ButtonTransitioner : MonoBehaviour, IPointerClickHandler
{
    public GameObject VRController;
    public GameObject myPointer;
    public GameObject myCanvas;
    public ToggleGroup myToggles;


    private void Awake()
    {
        VRController.GetComponent<CharacterController>().enabled = false;
    }


    public Toggle currentSelection{
        get { return myToggles.ActiveToggles().FirstOrDefault(); }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        

        print("Button Clicked with: " + currentSelection.name);
        myPointer.SetActive(false);
        myCanvas.SetActive(false);
        VRController.GetComponent<CharacterController>().enabled = true;

    }
}
