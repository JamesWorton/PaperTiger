using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialButton : MonoBehaviour
{
    private ButtonScriptBase baseScript = new ButtonScriptBase();
    private Button selfButton;
    [SerializeField] private GameObject targetobject;
    [SerializeField] private List<Material> Materials;
    [SerializeField] private Sprite innerSprite;
    [SerializeField] private Color32 innerColour;
    private int index=0;
    private GameObject mainGameObject => gameObject;
    public bool IsChild => transform.name.Contains("(Clone)");
    [SerializeField] GameObject temp;

    private Text caption;

    // Start is called before the first frame update
    void Start()
    {
        caption = transform.Find("Text").GetComponent<Text>();

        //baseScript = new ButtonScriptBase();

        selfButton = GetComponent<Button>();
        selfButton.onClick.AddListener(TaskOnClick);
        changeMaterial();
        //if (IsChild == false)
        //{
        //    //origGameObject = gameObject;
        //    Color temp_colour = mainGameObject.GetComponent<Image>().color;
        //    Sprite temp_image = mainGameObject.GetComponent<Image>().sprite;
        //    temp = baseScript.createObj(mainGameObject, innerSprite, innerColour);
        //    GameObject newObj = Instantiate(temp);
        //    newObj.transform.SetParent(gameObject.transform);
        //    newObj.transform.position = gameObject.transform.position;
        //    gameObject.GetComponent<Image>().sprite = temp_image;
        //    gameObject.GetComponent<Image>().color = temp_colour;
        //}
    }

    void TaskOnClick()
    {
        changeMaterial();
    }

    private void changeMaterial()
    {
        targetobject.GetComponent<Renderer>().material = Materials[index];
        targetobject.GetComponent<Renderer>().material.mainTextureScale = new Vector2(1, 1);
        targetobject.GetComponent<Renderer>().material.mainTextureScale = new Vector2(1, 1);
        if (index >= Materials.Count - 1)
        {
            index = 0;
        }
        else
        {
            index++;
        }
        caption.text = "Change To \n";
        if (index >= Materials.Count)
        {
            caption.text = caption.text + Materials[0].name;
        }
        else
        {
            caption.text = caption.text + Materials[index].name;
        }
    }
}
