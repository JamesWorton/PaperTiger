using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This is a base script for the buttons, it shows the inner section of the button.
// Much like the border it can have it's own colour wihch can be different from the border

public class ButtonScriptBase : MonoBehaviour
{
    //[SerializeField] private Sprite innerTexture;
    //[SerializeField] private Color32 innerColour;
    [SerializeField] private GameObject selfGameObjectInner;
    //private Button selfButton;
    //public Button button => selfButton;

    void Start()
    {

    }

    public GameObject createObj(GameObject self, Sprite innerSprite, Color32 innerColour)
    {
        selfGameObjectInner = self;
        selfGameObjectInner.GetComponent<Image>().sprite = innerSprite;
        selfGameObjectInner.GetComponent<Image>().color = innerColour;
        //if (selfGameObjectInner.GetComponent("ButtonSceneScript").name.Contains("(Clone)"))
        //{
        //    selfGameObjectInner.GetComponent("ButtonSceneScript");
        //}
        return selfGameObjectInner;
    }

    void TaskOnClick()
    {
        
    }
}
