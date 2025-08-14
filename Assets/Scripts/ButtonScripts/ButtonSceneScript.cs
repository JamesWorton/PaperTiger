using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonSceneScript : MonoBehaviour
{
    private ButtonScriptBase baseScript = new ButtonScriptBase();
    [SerializeField]private string newSceneName;

    [SerializeField] private Sprite innerSprite;
    [SerializeField] private Color32 innerColour;
    private GameObject mainGameObject => gameObject;
    [SerializeField] GameObject temp;
    private Button selfButton;
    public bool IsChild => transform.name.Contains("(Clone)");

    // Start is called before the first frame update
    void Start()
    {
        selfButton = GetComponent<Button>();
        selfButton.onClick.AddListener(TaskOnClick);
        if (IsChild == false) 
        {
            //origGameObject = gameObject;
            Color temp_colour = mainGameObject.GetComponent<Image>().color;
            Sprite temp_image = mainGameObject.GetComponent<Image>().sprite;
            temp = baseScript.createObj(mainGameObject, innerSprite, innerColour);
            GameObject newObj = Instantiate(temp);
            newObj.transform.SetParent(gameObject.transform);
            newObj.transform.position = gameObject.transform.position;
            newObj.transform.localScale = gameObject.transform.localScale;
            gameObject.GetComponent<Image>().sprite = temp_image;
            gameObject.GetComponent<Image>().color = temp_colour; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TaskOnClick()
    {
        if (newSceneName != "" && newSceneName != null) 
        {
            SceneManager.LoadScene(newSceneName); 
        }
    }

}
