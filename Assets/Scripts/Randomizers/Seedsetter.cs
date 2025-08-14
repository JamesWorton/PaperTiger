using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Seedsetter : MonoBehaviour
{
    public string userName => System.Environment.UserName;
    public string date => System.DateTime.Today.ToString();
    public string gamingRigName => SystemInfo.deviceName;
    public string pcModel => SystemInfo.deviceModel;
    public string RTXCard => SystemInfo.graphicsDeviceName;
    public string Pentium => SystemInfo.processorType;
    public string PentiumSpeed => SystemInfo.processorFrequency.ToString();
    public string Windblows => SystemInfo.operatingSystem;

    [SerializeField] private bool getUserName;
    [SerializeField] private bool getDay;
    [SerializeField] private bool getSystemInfo;
    [SerializeField] private bool getModelInfo;
    [SerializeField] private bool getGraphicsCardInfo;
    [SerializeField] private bool getProcessorInfo;
    [SerializeField] private bool getSpeedInfo;
    [SerializeField] private bool getOSName;

    private static string seedText = "Nice";
    public static string seedString => seedText;
    private int seedNumber = 0;

    private string originalTag;

    // Start is called before the first frame update
    void Start()
    {
        originalTag = gameObject.tag;
        transform.name = transform.name + Random.Range(0,1*10^10);
        DontDestroyOnLoad(gameObject);

        if (getUserName)
        {
            seedText = seedText + userName;
        }
        if (getDay)
        {
            seedText = seedText + date;
        }
        if (getSystemInfo)
        {
            seedText = seedText + gamingRigName;
        }
        if (getModelInfo)
        {
            seedText = seedText + pcModel;
        }
        if (getGraphicsCardInfo)
        {
            seedText = seedText + RTXCard;
        }
        if (getProcessorInfo)
        {
            seedText = seedText + Pentium;
        }
        if (getSpeedInfo)
        {
            seedText = seedText + PentiumSpeed;
        }
        if (getOSName)
        {
            seedText = seedText + Windblows;
        }


        for (int i = 0; i < seedString.Length; i++)
        {
            seedNumber = seedNumber + char.ToUpper(seedString[i]); // Add the index of the character to the seed number for all the chars in the string
        }

        Random.InitState(seedNumber); //This is divided by 4 to limit the amount of digits for the seed
        int tempSeed = Random.seed;

        // <summary>
        // Everything in this script gererates a seed based on the current date and information about the user,
        // This is done so that the colour of the straw lights remain consistent each time they are loaded throughout the entire day.
        // Some of the information can be altered in the unity editor, the more that is documented the more unique the seed will be 
        // across different users and/or systems. If no information is documented, the seed will be the same each time the game 
        // is started for all users and systems.
        // </summary>
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene "+ scene.name + " Loaded");
        Debug.Log(mode);
        Random.InitState(seedNumber);
        gameObject.tag = "Temp";
        //This is done so that the seed is consistent between scenes as Unity will change the seed upon scene change
        GameObject[] fakes = GameObject.FindGameObjectsWithTag(originalTag);
        gameObject.tag = originalTag;
        for (int i = 0; i < fakes.Length; i++)
        {
            if (fakes[i] != gameObject)
            {
                Destroy(fakes[i]);
            }
        }
        // Destroy all duplicate objects created when loading the main menu more than once
    }

    //Credit: https://answers.unity.com/questions/1174255/since-onlevelwasloaded-is-deprecated-in-540b15-wha.html
}
