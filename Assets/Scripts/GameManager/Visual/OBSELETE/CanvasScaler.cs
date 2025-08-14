using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScaler : MonoBehaviour
{
    private Canvas selfCanvas;
    private Vector2 gameWindowOld;
    private Vector2 gameWindowCurrent => new Vector2(Display.main.systemWidth, Display.main.systemHeight);

    // Start is called before the first frame update
    void Start()
    {
        selfCanvas = GetComponent<Canvas>();
        gameWindowOld = gameWindowCurrent;

    }

    // Update is called once per frame
    void Update()
    {
        if (gameWindowOld != gameWindowCurrent)
        {
            gameWindowOld = gameWindowCurrent;
            //setCanvasSize(gameWindowCurrent, screenSize, 1);
            //tagResolution.transform.position = new Vector3(Display.main.systemWidth, Display.main.systemHeight, 0);
        }
        // Rescales the canvas to accomidate any changes done with the resolution
    }

    private void setCanvasSize(Vector2 gameSize,Vector2 resolution, float power)
    {
        gameSize = new Vector2((int)gameSize.x, (int)gameSize.y);

        //selfCanvas.scaleFactor = Mathf.Pow(gameSize.x / resolution.x, 1f - power) * Mathf.Pow(gameSize.y / resolution.y, power);
    }

}
