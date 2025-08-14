using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddOrRemoveObjectOnButtonPress : MonoBehaviour
{
    private Button selfButton;
    private string hash;
    private string targetName;
    [SerializeField] private Vector3 targetCoordinates;
    [SerializeField]private GameObject GameObjectToMake;
    // Start is called before the first frame update
    void Start()
    {
        hash = Convert.ToString(UnityEngine.Random.Range(0, 9 * 10 ^ 10),16);

        selfButton = GetComponent<Button>();
        selfButton.onClick.AddListener(TaskOnClick);
        targetName = GameObjectToMake.name + hash;
        GameObjectToMake.name = targetName;
        GameObjectToMake.transform.position = targetCoordinates;
    }

    void TaskOnClick()
    {
        if (GameObject.Find(targetName+ "(Clone)") == null)
        {
            Instantiate(GameObjectToMake);
            // It does not exist, make a new one
        }
        else
        {
            Destroy(GameObject.Find(targetName + "(Clone)"));
            // It does exist already, therefore destroy it
        }
    }
}
