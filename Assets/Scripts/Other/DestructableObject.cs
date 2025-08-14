using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour
{
    [SerializeField] private AudioClip destructionSound;
    [SerializeField] private float volume;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<AudioSource>();
        gameObject.GetComponent<AudioSource>().clip = destructionSound;
        gameObject.GetComponent<AudioSource>().volume = volume;
    }


    public void destroyObject()
    {
        Light[] Lights = gameObject.GetComponentsInChildren<Light>();
        for (int i=0; i < Lights.Length; i++)
        {
            Lights[i].enabled = false;
        }
        gameObject.AddComponent<Rigidbody>();
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;

    }
}
