using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class AddRegionalInformation : MonoBehaviour
{
    // Adds or changes UI Text to display the Current Region - Debugging purposes only
    [SerializeField]private bool keepText;
    private Text mainText;
    // Start is called before the first frame update
    void Start()
    {
        mainText = GetComponent<Text>();
        if (keepText)
        {
            mainText.text = mainText.text + RegionInfo.CurrentRegion.DisplayName;
        }
        else
        {
            mainText.text = RegionInfo.CurrentRegion.DisplayName;
        }
    }

}
