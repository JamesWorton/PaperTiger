using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddSeedInformation : MonoBehaviour
{
    // Adds or changes UI Text to display the Random Seed - Debugging purposes only
    [SerializeField]private bool keepText;
    private Text mainText;
    // Start is called before the first frame update
    void Start()
    {
        mainText = GetComponent<Text>();
        if (keepText)
        {
            mainText.text = mainText.text + Random.seed.ToString(); //This is obselete  
        }
        else
        {
            mainText.text = Random.seed.ToString();
        }
    }

}
