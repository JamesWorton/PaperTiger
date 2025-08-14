using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is the base script for all the characters, each character must have a variable of type "CharacterBase" declared and initated once.
// The script includes all actions which all characters like Maarntey and all the NPCS perform, i.e. turning, moving, jumping and attacking.
// The purpose is to keep code organised, efficent and easier to maintain.

// IF THIS SCRIPT OR ITS FUNCTIONS ARE INNACESSABLE, REMOVED, BROKEN AND/OR DISABLED ALL OF THE CHARACTERS WILL LIKELY NOT WORK

public enum AnimationState
{
    Idle,
    Walking,
    Jumping,
    Falling,
    Shooting,
    Hurt
}

public class CharacterBase : MonoBehaviour
{
    private AnimationState oldAniState;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public bool flipChar(Vector2 axis, bool _goingRight, bool _facingBack)
    {
        return flipChar(axis, Vector2.zero, _goingRight, _facingBack);
    }

    public bool flipChar(Vector2 axis, Vector2 baseAxis, bool _goingRight, bool _facingBack)
    {
        // Should we be flipping?
        if (axis.x < baseAxis.x)
            _goingRight = false;
        else if (axis.x > baseAxis.x)
            _goingRight = true;

        //Are we facing back?
        if (axis.y < baseAxis.y)
            _facingBack = true;
        else if (axis.y >= baseAxis.y)
            _facingBack = false;

        return _goingRight;

    }

    public Vector3 processFlip(Vector3 currentEulerAngles, Camera mainCam, bool _goingRight, Vector3 rotationDirection, float rotationSpeed)
    {
        // Process the flip
        var mainCamyRot = mainCam.transform.localEulerAngles;
        mainCamyRot.x = 0;
        mainCamyRot.z = 0;
        if (_goingRight && rotationDirection.y > 0)
            rotationDirection.y = Mathf.Clamp(rotationDirection.y - rotationSpeed * Time.deltaTime * 120, 0, 180);
        else if (!_goingRight && rotationDirection.y < 180)
            rotationDirection.y = Mathf.Clamp(rotationDirection.y + rotationSpeed * Time.deltaTime * 120, 0, 180);
        currentEulerAngles = rotationDirection + mainCamyRot;

        return currentEulerAngles;
    }

    public float processRotation(Vector3 rotationDirection, float rotationSpeed) 
    {
        return Mathf.Clamp(rotationDirection.y + rotationSpeed * Time.deltaTime * 120, 0, 180);
    }


    //public Transform processFlip(Transform currentEulerAngles, Camera cam)
    //{

    //    // Process the flip
    //    var mainCamyRot = cam.transform.localEulerAngles;
    //    mainCamyRot.x = 0;
    //    mainCamyRot.z = 0;
    //    if (_goingRight && rotationDirection.y > 0)
    //        rotationDirection.y = Mathf.Clamp(rotationDirection.y - rotationSpeed, 0, 180);
    //    else if (!_goingRight && rotationDirection.y < 180)
    //        rotationDirection.y = Mathf.Clamp(rotationDirection.y + rotationSpeed, 0, 180);
    //    currentEulerAngles = rotationDirection + mainCamyRot;

    //    return currentEulerAngles;
    //}

    public Vector3 move(Vector3 _rbVel, Vector2 axis, float movementSpeed, bool jumpCondition, float jumpHeight)
    {
        // Update based on buttons pressed
        _rbVel = new Vector3(axis.x * movementSpeed,
            _rbVel.y,
            axis.y * movementSpeed);

        if (jumpCondition) //If condition has been met in the pharsed "jumpCondition" make the character jump
        {
            print("space key was pressed");
            _rbVel = new Vector3(_rbVel.x, 1 * jumpHeight, _rbVel.z);
        }

        return _rbVel;
    }

    public AnimationState setAnimation(Vector3 _rbVel, Animator _anim, bool attacking, bool onGround)
    {
        AnimationState currentState = AnimationState.Idle;
        if (attacking == false) 
        {
            Vector3 horiVelocity = _rbVel;
            horiVelocity.y = 0;

            var m_CurrentClipInfo = _anim.GetCurrentAnimatorClipInfo(0);
            var m_ClipName = m_CurrentClipInfo[0].clip.name;


            if (attacking)
            {
                currentState = AnimationState.Shooting;
            }
            // Are we standing still?
            else if (horiVelocity.magnitude > -1 && horiVelocity.magnitude < 1 && onGround)
            {
                currentState = AnimationState.Idle;
            }
            else if ((horiVelocity.magnitude > -1 && horiVelocity.magnitude < 1 == false) && onGround)
            {
                currentState = AnimationState.Walking;
            }
            else if (onGround == false && _rbVel.y > 0)
            {
                currentState = AnimationState.Jumping;
            }
            else if (onGround == false)
            {
                currentState = AnimationState.Falling;
            }

            if (m_ClipName != "attacking")
            {
                attacking = false;
            }

            oldAniState = currentState; 
        }
        else
        {
            currentState = AnimationState.Shooting;
        }

        return currentState;
    }

    public bool isStillAnimating(bool attacking, Animator _anim) //Stop the attacking animation when finished
    {

        var m_CurrentClipInfo = _anim.GetCurrentAnimatorClipInfo(0);
        var m_ClipName = m_CurrentClipInfo[0].clip.name;

        if (m_ClipName != "attacking")
        {
            attacking = false;
        }
        return attacking;
    }

    public Animator AnimateCharacter(Animator _anim, AnimationState currentState)
    {
        _anim.SetBool("Standing", currentState == AnimationState.Idle);
        _anim.SetBool("Jumping", currentState == AnimationState.Jumping);
        _anim.SetBool("Walking", currentState == AnimationState.Walking);
        _anim.SetBool("Falling", currentState == AnimationState.Falling);
        _anim.SetBool("Attacking", currentState == AnimationState.Shooting);
        _anim.SetBool("Hurt", currentState == AnimationState.Hurt);

        return _anim;
    }
    public Vector2 getControllerAxis(float xAxis, float yAxis, Camera mainCam, bool onGround, Vector2 airMovement) 
    // Makes the player move relative to the camera's direction, returns 2d coordinates of the axis relative to the camera's direction plus considering their condition or action
    {
        Vector2 axis = getControllerAxis(xAxis, yAxis, mainCam);

        if (onGround == false)
        {
            axis.x = airMovement.x + axis.x / 4;
            axis.y = airMovement.y + axis.y / 4;
        }
        else if (onGround)
        {
            airMovement = axis;
        }
        //if (isDodging)
        //{
        //    axis = dodgingAxis;
        //    dodgeCountdown--;
        //}
        //else
        //{
        //    if (axis.x > 0 || axis.y > 0)
        //    {
        //        dodgingAxis = axis;
        //    }
        //}
        return axis;
    }

    public Vector2 getControllerAxis(float xAxis, float yAxis, Camera mainCam)
    // Makes the player move relative to the camera's direction, returns 2d coordinates of the axis relative to the camera's direction
    {
        Vector2 axis = new Vector2(xAxis, yAxis);

        Vector3 forCam = mainCam.transform.forward;
        Vector3 riteCam = mainCam.transform.right;

        forCam.y = forCam.z;
        riteCam.y = riteCam.z;
        forCam.z = 0;
        riteCam.z = 0;

        axis = (forCam * yAxis + riteCam * xAxis);

        return axis;
    }
}
