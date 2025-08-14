using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetResButton : MonoBehaviour
{
    private ButtonScriptBase baseScript = new ButtonScriptBase();
    private Button selfButton;
    [SerializeField]private Vector2 newResolution;
    [SerializeField] private bool toggleFullScreen;

    // Start is called before the first frame update
    void Start()
    {
        //baseScript = new ButtonScriptBase();

        selfButton = GetComponent<Button>();
        selfButton.onClick.AddListener(TaskOnClick);

        if (toggleFullScreen == false) 
        {
            transform.Find("Text").GetComponent<Text>().text = (int)newResolution.x + "x" + (int)newResolution.y; 
        }
        else
        {
            transform.Find("Text").GetComponent<Text>().text = "Toggle Fullscreen";
        }

    }

    void TaskOnClick()
    {
        if (toggleFullScreen)
        {
            Screen.fullScreen = Screen.fullScreen == false;// Switches between windowed and fullscreen
        }
        else
        {
            Screen.SetResolution((int)newResolution.x, (int)newResolution.y, Screen.fullScreen); //Set the resolution to what was stated in newResolution
        }
    }
}
