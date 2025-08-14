using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HideParentOnPress : MonoBehaviour
{
    //[SerializeField] private string newSceneName;
    private Button hideButton;
    [SerializeField] private GameObject showInFavour;
    // Start is called before the first frame update
    void Start()
    {
        hideButton = GetComponent<Button>();
        hideButton.onClick.AddListener(TaskOnClick);
        if (showInFavour != null) 
        {
            showInFavour.SetActive(false); 
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void TaskOnClick()
    {
        //SceneManager.LoadScene(newSceneName);
        //transform.parent.gameObject.SetActive(false);
        transform.parent.parent.gameObject.SetActive(false);
        showInFavour.SetActive(true);
    }
}
