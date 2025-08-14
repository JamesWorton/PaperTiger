using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArea : MonoBehaviour
{
    CameraScript updateCamScript;
    private Rigidbody mainPlayer;
    [SerializeField] Vector3 cameraPosition;
    [SerializeField] Vector3 cameraRotation;
    [SerializeField] bool targetPlayer;
    [SerializeField] float FOV;

    [SerializeField] bool deathPlane;

    // Start is called before the first frame update
    void Start()
    {
        mainPlayer = GameObject.Find("Maarntey").GetComponent<Rigidbody>();
        updateCamScript = GameObject.FindWithTag("MainCamera").GetComponent<CameraScript>();
    }

    // Update is called once per frame
    void Update()
    {
        updateCamScript.Update();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (deathPlane == false) 
        {
            print("Trigger Activate!");
            if (other.gameObject.name == "Maarntey")
            {
                updateCamScript.updateCamera(cameraPosition, cameraRotation, targetPlayer, FOV);
                print("Thanks Maarntey!");
            } 
        }
        else
        {
            if (other.gameObject.name == "Maarntey")
            {
                other.gameObject.GetComponent<BasicMovement>().takeDamage(10*10^15);
            }
        }
    }

}
