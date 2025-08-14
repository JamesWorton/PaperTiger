using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrawButton : MonoBehaviour
{
    private Button selfButton;
    [SerializeField] private strawType strawTypeSet;
    void Start()
    {
        selfButton = GetComponent<Button>();
        selfButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        if (GameObject.FindGameObjectWithTag("Straw") != null)
        {
            GameObject.FindGameObjectWithTag("Straw").GetComponent<StrawSetter>().setStraw(strawTypeSet);
        }
        else
        {
            GameObject newStrawSetter = new GameObject("StrawObject");
            newStrawSetter.tag = "Straw";
            newStrawSetter.AddComponent<StrawSetter>();
            newStrawSetter.GetComponent<StrawSetter>().setStraw(strawTypeSet);
            Instantiate(newStrawSetter);
        }
        // Adds a new game object (if one does not already exist) that tells the straws what type it should be.
        // If the game object already exists, edit the data to prevent the generation of unnessesary amounts of game objects.
    }
}
