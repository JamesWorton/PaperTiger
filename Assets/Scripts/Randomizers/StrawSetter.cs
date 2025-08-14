using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StrawSetter : MonoBehaviour
{
    private strawType newType;
    public strawType Straw => newType;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        gameObject.tag = "Straw";
    }


    public void setStraw(strawType newStraw)
    {
        newType = newStraw;
    }
}
