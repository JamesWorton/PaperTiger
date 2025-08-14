using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class AddMusicState : MonoBehaviour
{
    // Adds or changes UI Text to display the Current Region - Debugging purposes only
    [SerializeField]private bool keepText;
    private Text mainText;
    private modes lastMode;
    private modes currentMode => GameObject.FindGameObjectWithTag("Music").GetComponent<MusicManager>().musicCurrent;
    private string ModeName => System.Enum.GetName(typeof(modes), (int)currentMode);
    private string baseText;

    // Start is called before the first frame update
    void Start()
    {
        mainText = GetComponent<Text>();
        baseText = mainText.text;

        if (keepText)
        {
            mainText.text = baseText + ModeName;
        }
        else
        {
            mainText.text = ModeName;
        }
    }

    private void Update()
    {
        if (currentMode != lastMode) 
        {
            if (keepText)
            {
                mainText.text = baseText + ModeName;
            }
            else
            {
                mainText.text = ModeName;
            }
            lastMode = currentMode;
        }
    }

}
