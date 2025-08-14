using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraScript : MonoBehaviour
{
    private Camera mainCamera;
    public bool lookingAtPlayer = false;
    [SerializeField] float focalLength;
    [SerializeField] float depth;
    private Transform PlayerPos;
    BasicMovement MaarnScript;

    //public PostProcessVolume Volume;
    private MotionBlur m_MotionBlur;

    private int frameCounter;

    private float deltaTime;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GetComponent<Camera>();
        if (GameObject.Find("Maarntey").GetComponent<Transform>() == null)
        {
            this.enabled = false;
        }
        // This is to make sure that the game does not generate a million errors if Maarntey could not be found or is absent from the scene
        PlayerPos = GameObject.Find("Maarntey").GetComponent<Transform>();
        MaarnScript = GameObject.Find("Maarntey").GetComponent<BasicMovement>();
        m_MotionBlur = mainCamera.GetComponent<PostProcessVolume>().profile.GetSetting<MotionBlur>();
    }

    // Update is called once per frame
    public void Update()
    {
        if (PlayerPos != null && MaarnScript != null) 
        {
            if (lookingAtPlayer)
                transform.LookAt(PlayerPos);

            //mainCamera.focalLength = focalLength;
            //mainCamera.depth = depth;

            if (m_MotionBlur.enabled.value == false && frameCounter > 60 - Time.deltaTime * 60)
            {
                m_MotionBlur.enabled.value = true;
                frameCounter = 0;
            }
            else if (frameCounter <= 60 - Time.deltaTime * 60)
            {
                frameCounter++;
            }
            else
            {
                m_MotionBlur.enabled.value = true;
                frameCounter = 0;
            } }
    }

    public void updateCamera(Vector3 newPosition, Vector3 newRotation, bool lookAtPlayer, float FieldOfView)
    {
        if (mainCamera.transform.position != newPosition || mainCamera.transform.eulerAngles != newRotation || lookingAtPlayer != lookAtPlayer)
        {
            MaarnScript.cameraChanged(); //Lets Maarntey know that the camera has changed
        }
        m_MotionBlur.enabled.value = false;

        mainCamera.transform.position = newPosition;
        mainCamera.transform.eulerAngles = newRotation;
        mainCamera.fieldOfView = FieldOfView;
        lookingAtPlayer = lookAtPlayer;

    }

    int currentFPS()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        return (int)fps;
    }
}
