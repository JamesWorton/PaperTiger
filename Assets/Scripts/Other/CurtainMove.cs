using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurtainMove : MonoBehaviour
{
    [SerializeField] private Vector3 moveVector;
    private Vector3 originalMoveVector;
    [SerializeField] private Vector3 rotateEulerAngles;
    private Vector3 originalRotationAmount;
    [SerializeField] private float moveTimes;
    private float originalTimes;
    [SerializeField] private Vector3 postTransformScale;
    [SerializeField] private bool Repeat;
    [SerializeField] private float rotateX;
    private Vector3 startRotation;
    private Vector3 originalRotation;
    private Vector3 originalPosition;
    // Start is called before the first frame update
    void Start()
    {
        originalRotation = gameObject.transform.eulerAngles;
        originalPosition = gameObject.transform.position;
        originalTimes = moveTimes;
        originalRotationAmount = rotateEulerAngles;
        originalMoveVector = moveVector;

        transform.localScale = postTransformScale;
        startRotation = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        rotateX = transform.eulerAngles.x;
        if (moveTimes > 0)
        {
            transform.position = transform.position + moveVector;
            transform.eulerAngles = transform.eulerAngles + rotateEulerAngles;
            
            if (transform.eulerAngles.x >= 90 && rotateEulerAngles.x > 0 && transform.eulerAngles.x <= 91 )
            {
                rotateEulerAngles = new Vector3(rotateEulerAngles.x*-1, rotateEulerAngles.y, rotateEulerAngles.z);
            }
            moveTimes--;
        }
        else
        {
            if (Repeat)
            {
                GameObject originalState = gameObject;
                originalState.transform.eulerAngles = originalRotation;
                originalState.transform.position = originalPosition;
                originalState.GetComponent<CurtainMove>().setArrtibutes(originalTimes, originalMoveVector, originalRotationAmount);
                Instantiate(originalState);
            }
            Destroy(gameObject);
        }
    }
    public void setArrtibutes(float times, Vector3 move, Vector3 rotate)
    {
        moveTimes = times;
        rotateEulerAngles = rotate;
        moveVector = move;
    }
}
