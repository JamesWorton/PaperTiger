using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyForce : MonoBehaviour
{
    private Rigidbody _rb;

    [SerializeField]private Vector3 force;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.AddForce(force);
        _rb.velocity = force;
        force = Vector3.zero;
    }

}
