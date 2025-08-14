using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionSetter : MonoBehaviour
{
    private Vector2 screenSize => new Vector2(Screen.width, Screen.height);

    private GameObject tagResolution;

    private string objName => "ResolutionSettings";
    private Vector2 baseResolution => new Vector2(1920, 1080);

    // Start is called before the first frame update
    void Start()
    {

        tagResolution = new GameObject();
        tagResolution.name = objName;
        tagResolution.transform.position = new Vector3(Display.main.systemWidth, Display.main.systemHeight, 0);

        if (GameObject.Find(objName) == null)
        {
            Screen.SetResolution(Screen.width, Screen.height, true);
            Screen.SetResolution((int)baseResolution.x, (int)baseResolution.y, false);
            if (Screen.width == baseResolution.x || Screen.height == baseResolution.y)
            {
                Screen.fullScreen = true;
            }
            else if (Screen.width < baseResolution.x || Screen.height < baseResolution.y)
            {
                Screen.SetResolution(Screen.width, Screen.height, true);
                //setCanvasSize(gameWindowCurrent, screenSize, 1);
            }
            else
            {
                Screen.fullScreen = false;
            }
            DontDestroyOnLoad(tagResolution);
            Instantiate(tagResolution);
        }
        // Initialises a gameObject which makes any canvas object in a foreign scene idenifty what the resolution of the game is and does not reset it to 1080p windowed,
        // as it does not know wether it has been initialised or now

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
