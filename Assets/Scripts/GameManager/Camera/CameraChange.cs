using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraChange : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 position;
    private Vector3 rotation;
    private bool lookingAtPlayer;

    CameraScript camScript;

    PostProcessVolume m_Volume;
    MotionBlur m_MotionBlur;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GetComponent<Camera>();
        camScript = new CameraScript();
    }

    // Update is called once per frame
    public void Update()
    {

    }

    public void updateCamera(Vector3 newPosition, Vector3 newRotation, bool lookAtPlayer)
    {
        m_MotionBlur.active = false;
        m_Volume.profile.TryGetSettings(out m_MotionBlur);
        lookingAtPlayer = lookAtPlayer;
        mainCam.transform.position = newPosition;
        mainCam.transform.eulerAngles = newRotation;
        m_MotionBlur.active = true;
        m_Volume.profile.TryGetSettings(out m_MotionBlur);
    }
}
