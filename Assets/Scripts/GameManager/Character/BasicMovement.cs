using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

// This is Maarntey's Script, this should only be applied to Maarntey or other playable characters like Maarntey

enum weapons
{
    LunaKatti,
    Gun
}


public class BasicMovement : MonoBehaviour
{
    CharacterBase baseScript;

    private Camera mainCam;

    private Rigidbody _rb;

    private Transform _physRep;
    private Animator _anim;

    public static Transform WorldTransformation;

    private bool _goingRight = true;
    private bool _facingBack = false;
    [SerializeField] private bool attacking = false, attackingOld = false;
    public bool isAttacking => attacking;
    //public Collider _attackCollision;
    private bool hurting = false; public bool isTakingDamage => hurting;

    public float movementSpeed;
    public float rotationSpeed;
    private Vector3 rotationDirection;

    private Vector2 lastDirInput;
    private Vector2 lastDirInputRelative;
    private Vector2 AxisOnGround => REStyle(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), baseScript.getControllerAxis(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), Camera.main, onGround, airMovement));
    // The variable above returns a 2D vector which makes the character move relative to the rotation of the camera
    //[SerializeField] Vector3 AxisRelative;
    private Vector2 airMovement;
    public Collision hittingObj;
    private bool onGround => updateGround();

    [SerializeField] private float jumpHeight;

    [SerializeField] private AnimationState currentState = AnimationState.Idle;
    private AnimationState oldAniState;

    public Vector3 currentEulerAngles;

    public float camY;

    public Vector3 contactPoint;

    public Vector3 counterForce;

    private AudioSource bladeSwing_snd;

    string m_ClipName;
    AnimatorClipInfo[] m_CurrentClipInfo;

    public List<bool> collisionOnFloor;

    private int foruthRequest = 0; // This is made for debugging the onGround check and collision
    public int timesCalled = 0; // This is made for debugging the onGround check and collision

    private bool cameraHasChanged;

    private static int outputDamage;
    public static int DamageDealt => outputDamage + Random.Range(-5, 5);

    private BoxCollider attackCollider;

    private weapons currentWeapon;

    [SerializeField]private int health;

    [SerializeField] private GameObject corpseObject;

    private bool jumpPressed => Input.GetKeyDown("space") || Input.GetButtonDown("A") || Input.GetButtonDown("B");
    private bool attackPressed => Input.GetKeyDown("z") || Input.GetButtonDown("X") || Input.GetButtonDown("Y");

    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        cameraHasChanged = false;
        //onGround = true;
        _rb = GetComponent<Rigidbody>();
        _physRep = transform.Find("PhysRep");
        _anim = GetComponentInChildren<Animator>();
        rotationDirection = _physRep.eulerAngles;
        attackCollider = GameObject.Find("AttackCol").GetComponent<BoxCollider>();

        mainCam = Camera.main;

        baseScript = new CharacterBase();

        bladeSwing_snd = GetComponent<AudioSource>();

        m_CurrentClipInfo = this._anim.GetCurrentAnimatorClipInfo(0);
        //m_CurrentClipLength = m_CurrentClipInfo[0].clip.length;
        m_ClipName = m_CurrentClipInfo[0].clip.name;
    }

    private Transform getTrans()
    {
        return transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWeapon == weapons.LunaKatti)
        {
            outputDamage = 10;
        }

        if (health <= 0)
        {
            foreach (Transform child in gameObject.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            Instantiate<GameObject>(corpseObject, transform);
            corpseObject.GetComponent<Rigidbody>().velocity = _rb.velocity;
            transform.DetachChildren();
            Destroy(gameObject);
        }
        else
        {
            attackCollider.enabled = attacking; // It is only enabled while Maarntey is attacking
            WorldTransformation = getTrans();
            _goingRight = baseScript.flipChar(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), _goingRight, _facingBack);

            //AxisRelative = new Vector3(AxisOnGround.x, 0, AxisOnGround.y);

            currentEulerAngles = _physRep.eulerAngles;

            // Process the flip
            var mainCamyRot = mainCam.transform.localEulerAngles;
            mainCamyRot.x = 0;
            mainCamyRot.z = 0;
            if (_goingRight && rotationDirection.y > 0)
                rotationDirection.y = baseScript.processRotation(rotationDirection, -rotationSpeed);
            else if (!_goingRight && rotationDirection.y < 180)
                rotationDirection.y = baseScript.processRotation(rotationDirection, rotationSpeed);
            currentEulerAngles = rotationDirection + mainCamyRot;

            _physRep.eulerAngles = currentEulerAngles;

            //transform.rotation = Quaternion.Euler(0, mainCam.transform.localEulerAngles.y + rot.y, 0);

            camY = mainCam.transform.localEulerAngles.y;

            _rb.velocity = baseScript.move(_rb.velocity, AxisOnGround, movementSpeed, (jumpPressed && onGround), jumpHeight);

            if (attackPressed && attacking == false)
            {
                attacking = true;
            }
            else if (attacking && m_ClipName != "attack" && _anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !_anim.IsInTransition(0))
            {
                currentState = AnimationState.Idle;
                attacking = false;
            }

            if (hurting && m_ClipName != "hurt" && _anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !_anim.IsInTransition(0))
            {
                currentState = AnimationState.Idle;
                hurting = false;
            }

            currentState = baseScript.setAnimation(_rb.velocity, _anim, attacking, onGround);
            _anim = baseScript.AnimateCharacter(_anim, currentState);
            //attacking = baseScript.isStillAttacking(attacking, _anim);
            if (attacking && attackingOld == false)
            {
                bladeSwing_snd.Play();
            }
            attackingOld = attacking;

            Debug.DrawRay(transform.position, new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * 10, Color.yellow);
            Debug.DrawRay(transform.position, new Vector3(AxisOnGround.x, 0, AxisOnGround.y) * 10, Color.green);
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    foreach (ContactPoint contact in collision.contacts)
    //    {
    //        if ((contact.normal.y < 0.7f && contact.normal.y > -0.7f) == false)
    //        {
    //            collisionOnFloor.Add(true);
    //        }
    //        else
    //        {
    //            collisionOnFloor.Add(false);
    //        }
    //    }
    //}

    private void OnCollisionStay(Collision collisionInfo)
    {
        if (foruthRequest >= 8) 
        {
            foruthRequest = 0;
            collisionOnFloor.Clear(); 
        }
        else
        {
            foruthRequest++;
        }
        collisionOnFloor.Clear();
        counterForce = Vector3.zero;
        contactPoint = Vector3.zero;
        // Debug-draw all contact points and normals
        foreach (ContactPoint contact in collisionInfo.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal * 10, Color.white);
            //print("Contact! At: " + contact.point + " With: " + collisionInfo.rigidbody.name);
            contactPoint = contact.normal;
            counterForce = counterForce + contactPoint;
            counterForce.y = 0;
            if ((contact.normal.y < 0.7f && contact.normal.y > -0.7f) == false)
            {
                collisionOnFloor.Add(true);
            }
            else
            {
                collisionOnFloor.Add(false);
            }
        }
        timesCalled++;
    }

    private void OnCollisionExit(Collision collisionInfo)
    {
        collisionOnFloor.Clear();
        counterForce = Vector3.zero;
        contactPoint = Vector3.zero;
        print("No longer in contact with " + collisionInfo.transform.name);
    }

    private bool updateGround()
    {
        for (int i = 0; i < collisionOnFloor.Count; i++)
        {
            if (collisionOnFloor[i] == true)
            {
                return true;
            }
        }
        return false;
    }

    private Vector3 REStyle(Vector2 controllerInput, Vector2 relInput) 
        // This function emulates the characters' movement during camera changes like in Resident Evil and the Devil May Cry games
        // To keep it brief, if the controller axis remains the dame during a camera transistion, the character will keep moving in the same direction
        // If this is not implemented, transistioning between areas will become unnessesarily annoying (as if Bennet Foddy made this game)
    {
        if (cameraHasChanged)
        {
            if (controllerInput.magnitude == 0)
            {
                lastDirInput = controllerInput;
                lastDirInputRelative = relInput;
                cameraHasChanged = false;
                return Vector3.zero;
            }
            if (controllerInput.x != lastDirInput.x)
            {
                lastDirInput.x = controllerInput.x;
                lastDirInputRelative.x = relInput.x;
            }
            if (controllerInput.y != lastDirInput.y)
            {
                lastDirInput.y = controllerInput.y;
                lastDirInputRelative.y = relInput.y;
            }
            if (controllerInput.x != lastDirInput.x || controllerInput.y != lastDirInput.y)
            {
                cameraHasChanged = false;
            }
            return lastDirInputRelative;
        }
        else
        {
            return relInput;
        }
    }

    public void cameraChanged()
    {
        cameraHasChanged = true;
    }

    public void takeDamage(int amount)
    {
        hurting = true;
        health = health - amount;
    }
}
