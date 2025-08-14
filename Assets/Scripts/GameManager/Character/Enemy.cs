using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.LowLevel;

public enum EnemyMood 
{
    sleep,
    busy,
    mad,
    scared
}



public class Enemy : MonoBehaviour
{
    [SerializeField] private float enemyHealth;
    private float originalHealth;
    CharacterBase baseScript;

    private Camera mainCam;

    private Rigidbody _rb;

    private Transform _physRep;
    private Animator _anim;

    [SerializeField] private EnemyMood initialMood;
    private EnemyMood currentMood;
    public EnemyMood Mood => currentMood;
    [SerializeField] private float wakeDistance;

    [SerializeField] private GameObject corpseObject;

    private bool _goingRight = true;
    private bool _facingBack = false;
    private bool attacking = false, hurting = false;
    public bool isAttacking => attacking; public bool isTakingDamage => hurting;

    public float movementSpeed;
    public float rotationSpeed;
    private Vector3 rotationDirection;
    //private Vector2 AxisOnGround => AiInput;
    // The variable above returns a 2D vector which makes the character move relative to the rotation of the camera
    private Vector2 AxisRelative => baseScript.getControllerAxis(AiInput.x, AiInput.y, Camera.main, onGround, airMovement);
    private Vector2 airMovement;
    public Collision hittingObj;
    public bool onGround = false;
    [SerializeField] private float jumpHeight;
    private bool isMoving => (AiInput.magnitude > 0.5f);

    public Transform targetTransformation => BasicMovement.WorldTransformation;
    private float targetDist => Vector3.Distance(targetTransformation.position, transform.position);
    private Vector2 walkDirection => pressurePlayer(5);

    [SerializeField] private AnimationState currentState = AnimationState.Idle;
    private AnimationState oldAniState;

    public Vector3 currentEulerAngles;

    public float camY;

    private bool attackNow = false;
    [SerializeField] private bool jumpNow = false;
    public Vector2 AiInput = Vector2.zero;

    [SerializeField] public bool isBackingOff;
    [SerializeField] public float PlayerDistance;

    public int noOfCollisions;
    private Vector3 currentForces;

    [SerializeField] private float AxisMag;

    private Vector3 origPosition;
    private bool setOrigPosition = true;
    private float origDist => Vector3.Distance(origPosition, transform.position);

    private Ray shotRay;
    RaycastHit _hit;

    private Transform playerTrans;
    //Be sure not to overwrite the player transformation

    private int damageOutput => 10 + UnityEngine.Random.Range(-5, 5);

    // Start is called before the first frame update
    void Start()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        currentMood = initialMood;
        onGround = true;
        _rb = GetComponent<Rigidbody>();
        _physRep = transform.Find("PhysRep");
        _anim = GetComponentInChildren<Animator>();
        rotationDirection = _physRep.eulerAngles;

        mainCam = Camera.main;

        baseScript = new CharacterBase();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerDistance = targetDist;
        if (enemyHealth <= 0)
        {
            //transform.localScale = new Vector3(2.33433917e-09f, 2.33865118f, 1.86272371f); //Sets the scale of the corpse, otherwise it does really weird things
            
            foreach (Transform child in gameObject.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            Instantiate<GameObject>(corpseObject, transform);
            transform.DetachChildren();
            Destroy(gameObject);
            // Destroy the children after spawining corpse and before destroying self so that the corpse is all that is left of the enemy
        }
        else
        {
            if (checkIfHit(playerTrans))
            {
                playerTrans.gameObject.GetComponent<BasicMovement>().takeDamage(damageOutput);
            }


            if (wakeDistance > targetDist || originalHealth > enemyHealth)
            {
                currentMood = EnemyMood.mad;
            }

            if (wakeDistance < targetDist*2.5 && currentMood != EnemyMood.sleep)
            {
                currentMood = EnemyMood.busy;
            }

            AxisMag = AiInput.magnitude;
            //baseScript.getControllerAxis(AiInput.x, AiInput.y, Camera.main, onGround, airMovement)
            _goingRight = baseScript.flipChar(walkDirection, _goingRight, _facingBack);
            //I have no clue why, but getting the coordinates relative to the camera direction will make the enemy face the right way when walking

            currentEulerAngles = _physRep.eulerAngles;

            AiInput = walkDirection;

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

            _rb.velocity = baseScript.move(_rb.velocity, AiInput, movementSpeed, (jumpNow && onGround), jumpHeight);

            if (jumpNow && onGround)
            {
                jumpNow = false;
            }

            if (attackNow && attacking == false)
            {
                attacking = true;
            }
            currentState = baseScript.setAnimation(_rb.velocity, _anim, attacking, onGround);
            _anim = baseScript.AnimateCharacter(_anim, currentState);
            attacking = baseScript.isStillAnimating(attacking, _anim);
            hurting = baseScript.isStillAnimating(hurting, _anim);

            Debug.DrawRay(transform.position, new Vector3(walkDirection.x, 0, walkDirection.y) * 10, Color.yellow);
            Debug.DrawRay(transform.position, new Vector3(AxisRelative.x, 0, AxisRelative.y) * 10, Color.green);
        }
    }

    private Vector3 pressurePlayer(float tooClose)
    {
        if (hurting)
        {
            return Vector3.zero;
        }
        Vector3 direction = Vector3.zero;
        if (currentMood == EnemyMood.mad) 
        {
            isBackingOff = false;
            direction = new Vector2((targetTransformation.position.x - transform.position.x), (targetTransformation.position.z - transform.position.z)).normalized;
            if (targetDist < tooClose + 0.5f && targetDist > tooClose - 0.5f)
            {
                isBackingOff = false;
                direction = Vector3.zero;
            }
            else if (targetDist < tooClose)
            {
                isBackingOff = true;
                direction = new Vector2((targetTransformation.position.x - transform.position.x) * -1, (targetTransformation.position.z - transform.position.z) * -1).normalized;
            }
            if (noOfCollisions < 2)
            {

            }
            else if (isMoving)
            {
                jumpNow = true;
                direction = Vector3.zero;
            } 
        }
        else if (currentMood == EnemyMood.sleep)
        {
            if (targetDist <= wakeDistance)
            {

            }
        }
        else if (currentMood == EnemyMood.busy)
        {
            direction = new Vector2((origPosition.x - transform.position.x), (origPosition.z - transform.position.z)).normalized;
            //if (Enumerable.Range((int)(origPosition.x - 5), (int)(origPosition.x + 5)).Contains((int)(transform.position.x)) && Enumerable.Range((int)(origPosition.z - 5), (int)(origPosition.z + 5)).Contains((int)(transform.position.z))) ;
            if (origDist<2)
            {
                currentMood = EnemyMood.sleep;
                direction = Vector2.zero;
            }
        }
        //direction = direction - currentForces / 2;
        return direction;
    }

    private void OnCollisionStay(Collision collisionInfo)
    {
        currentForces = Vector3.zero;
        noOfCollisions = 0;
        // Debug-draw all contact points and normals
        foreach (ContactPoint contact in collisionInfo.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal * 10, Color.white);
            //print("Contact! At: " + contact.point + " With: " + collisionInfo.rigidbody.name);
            noOfCollisions++;
            //currentForces = currentForces + contact.normal;
            if (contact.normal.y < 0.7f && contact.normal.y > -0.7f)
            {
                onGround = false;
            }
            else
            {
                onGround = true;
            }
        }
        //onGround = true;
    }

    private void OnCollisionExit(Collision collisionInfo)
    {
        print("No longer in contact with " + collisionInfo.transform.name);
        onGround = false;
        noOfCollisions--;
        currentForces = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (setOrigPosition)
        {
            setOrigPosition = false;
            origPosition = transform.position;
        }

        //if (collision.transform.tag == "PAttackCollision")
        //{
        //    hurting = true;
        //    enemyHealth = enemyHealth - BasicMovement.DamageDealt;
        //}
    }

    public void TakeDamage()
    {
        if (hurting == false) 
        {
            hurting = true;
            enemyHealth = enemyHealth - BasicMovement.DamageDealt; 
        }
    }

    private void ShootGun()
    {
        attacking = true;
        Transform oldTrans = transform;
        transform.LookAt(transform.Find("Maarntey"));
        float innaccuracyMagnitude = UnityEngine.Random.Range(-50,50);
        Vector3 innaccurateshot = new Vector3(UnityEngine.Random.Range(-innaccuracyMagnitude, innaccuracyMagnitude), UnityEngine.Random.Range(-innaccuracyMagnitude, innaccuracyMagnitude), UnityEngine.Random.Range(-innaccuracyMagnitude, innaccuracyMagnitude));
        shotRay = new Ray(transform.position, transform.eulerAngles + innaccurateshot);
        transform.localRotation = oldTrans.localRotation;
    }

    private bool checkIfHit(Transform HitTrans)
    {
        if (Physics.Raycast(shotRay, out _hit, 1000f) || true)
        {
            //Debug.Log("Raycast Criteria Met");
            if (_hit.transform == HitTrans)
            {
                //Debug.Log("Target - " + _hit.transform.name + " has been hit!");
                return true;
            }
            else
            {
                //Debug.Log("Hit - " + _hit.transform);
                return false;
            }
        }
        else
        {
            return false;
        }
    }

}