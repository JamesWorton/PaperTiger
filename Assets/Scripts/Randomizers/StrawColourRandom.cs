
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum strawType
{
    plastic,
    stainlessSteel
}

//This script affects the colour and type of the straws
public class StrawColourRandom : MonoBehaviour
{
    private strawType currentStraw = strawType.plastic;
    //Material strawMaterial;
    Renderer strawRenderer;
    Material originalMaterial;
    [SerializeField] private Color32 strawColour;
    public Color32 selfColour => strawColour;
    [SerializeField] private float origTransparency;
    private GameObject[] otherStrawLights;
    private Color32[] otherStrawLightsColour;
    private Color32[] otherStrawLightsColourSimple;
    private Color32[] otherStrawLightsColourSimple2;
    public Color32 simplifiedColour => new Color32((byte)((int)(strawColour.r / simplifiedTreshold) * simplifiedTreshold), (byte)((int)(strawColour.g / simplifiedTreshold) * simplifiedTreshold), (byte)((int)(strawColour.b / simplifiedTreshold) * simplifiedTreshold), strawColour.a);
    public Color32 simplifiedColour2 => new Color32((byte)((int)(strawColour.r / simplifiedTreshold2) * simplifiedTreshold2), (byte)((int)(strawColour.g / simplifiedTreshold2) * simplifiedTreshold2), (byte)((int)(strawColour.b / simplifiedTreshold2) * simplifiedTreshold2), strawColour.a);
    private int simplifiedTreshold => 50;
    private int simplifiedTreshold2 => 10;
    private bool needToChange = false;
    [SerializeField] private int SwitchedColours = 0;
    bool postChanged = false;

    [SerializeField] Material stainlessSteel;


    // Start is called before the first frame update
    void Start()
    {
        strawRenderer = GetComponent<Renderer>();
        originalMaterial = strawRenderer.material;
        //origTransparency = strawRenderer.material.color
        //strawMaterial = strawRenderer.GetComponent<Material>();
        strawColour = new Color32(0, 0, 0, (byte)origTransparency);
        otherStrawLights = GameObject.FindGameObjectsWithTag("StrawLight");

        otherStrawLightsColour = new Color32[otherStrawLights.Length];
        otherStrawLightsColourSimple = new Color32[otherStrawLights.Length];
        otherStrawLightsColourSimple2 = new Color32[otherStrawLights.Length];

        changeColour();

        if (GameObject.FindGameObjectsWithTag("Straw") != null)
        {
            currentStraw = GameObject.FindGameObjectWithTag("Straw").GetComponent<StrawSetter>().Straw;
        }
        // Set the straw based on what was decied in the settings

        if (currentStraw == strawType.plastic) 
        {
            uniqueColourPlease(); 
        }
        else if (currentStraw == strawType.stainlessSteel)
        {
            setStainless();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (strawRenderer.material.color != strawColour) 
        {
            strawRenderer.material.SetColor("_Color", strawColour);
        }
        if (postChanged == false) // One more change to be sure that the posts are unique
        {
            postChanged = true;
            uniqueColourPlease();
        }
    }

    private byte generateColourByte()
    {
        return (byte)((int)(UnityEngine.Random.Range(0, 256)/10)*10);
    }

    private bool checkColourRange(Color32 mainCol, Color32 otherCol, int range)
    {
        return (Enumerable.Range(otherCol.r - range, otherCol.r + range).Contains(mainCol.r) && Enumerable.Range(otherCol.g - range, otherCol.g + range).Contains(mainCol.g) && Enumerable.Range(otherCol.b - range, otherCol.b + range).Contains(mainCol.b));
            // && Enumerable.Range(otherCol.g - 50, otherCol.g + 50).Contains(mainCol.g) && Enumerable.Range(otherCol.b - 50, otherCol.b + 50).Contains(mainCol.b) && Enumerable.Range(otherCol.b - 50, otherCol.b + 50)
    }

    private void changeColour()
    {
        strawColour = new Color32(generateColourByte(), generateColourByte(), generateColourByte(), (byte)origTransparency);
        SwitchedColours++;
        needToChange = true;
        otherStrawLights = GameObject.FindGameObjectsWithTag("StrawLight");
        for (int i = 0; i < otherStrawLights.Length; i++)
        {
            otherStrawLightsColourSimple[i] = otherStrawLights[i].transform.Find("Cylinder001").GetComponent<StrawColourRandom>().selfColour;
            otherStrawLightsColourSimple[i] = otherStrawLights[i].transform.Find("Cylinder001").GetComponent<StrawColourRandom>().simplifiedColour;
            otherStrawLightsColourSimple2[i] = otherStrawLights[i].transform.Find("Cylinder001").GetComponent<StrawColourRandom>().simplifiedColour2;
        }
    }

    private void uniqueColourPlease()
    {
        for (int k = 0; k < 2; k++) // Repeat this 2 times more just in case
        {
            for (int j = 0; j < 100; j++)
            {
                needToChange = false;
                for (int i = 0; i < otherStrawLightsColourSimple.Length; i++)
                {
                    //if (simplifiedColour == (Color)otherStrawLightsColourSimple[i] || simplifiedColour2 == (Color)otherStrawLightsColourSimple2[i])
                    //{
                    //    changeColour();
                    //}
                    if (checkColourRange(strawColour, otherStrawLightsColour[i], 75 - i / 2))
                    {
                        changeColour();
                    }
                }
            }
        }
    }

    private void setStainless()
    {
        strawRenderer.material = stainlessSteel;
        strawColour = new Color32(255, 255, 255, (byte)255);
    }

}
