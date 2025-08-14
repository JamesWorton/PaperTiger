using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlap : MonoBehaviour
{
    private Vector3 origPosition;
    [SerializeField] float movementSpeed;
    [SerializeField] float moveAmount;
    private bool moveForward = true;
    private int multiplier = 1; // setting it to 1 will make it go forawrd and setting it to -1 will make it go back
    private Vector3 baseRotation;

    [SerializeField] private Vector3 rotationOffset;

    Camera mainCam;

    [SerializeField]private float rotationSpeed;
    private float fireDirection => getDirection(mainCam.transform.eulerAngles.y, transform.eulerAngles.y, rotationSpeed);

    // Start is called before the first frame update
    void Start()
    {
        origPosition = transform.position;
        mainCam = Camera.main;
        baseRotation = transform.eulerAngles;
        transform.position = new Vector3(transform.position.x, Random.Range(origPosition.y, origPosition.y + moveAmount), transform.position.z); // Randomise sequence
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3 (baseRotation.x, fireDirection, baseRotation.z);
        if (transform.position.y == origPosition.y)
        {
            multiplier = 1;
        }
        else if (transform.position.y == origPosition.y + moveAmount)
        {
            multiplier = -1;
        }
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y + movementSpeed * multiplier * Time.deltaTime*120, origPosition.y, origPosition.y+moveAmount), transform.position.z);
    }

    private float getDirection(float camDirection, float baseDirection, float speed)
    {
        if (speed <= 0)
        {
            return camDirection;
        }
        else
        {
            if ((int)baseDirection > (int)camDirection)
            {
                baseDirection = Mathf.Clamp(baseDirection - speed, -camDirection, camDirection);
            }
            else if ((int)baseDirection < (int)camDirection)
            {
                baseDirection = Mathf.Clamp(baseDirection + speed, -camDirection, camDirection);
            }
            return baseDirection;
        }
    }
}
