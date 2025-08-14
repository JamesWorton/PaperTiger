using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploder : MonoBehaviour
{
    [SerializeField] private GameObject baseObject;
    [SerializeField] private int amount;
    private Rigidbody _rb;
    public List<GameObject> objects;
    public int frames;

    public float deltaTime;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = new Vector3(0, 50*amount, 0);

        for (int i = 0; i < amount; i++)
        {
            if (currentFPS() > 30)
            {
                GameObject newObj = Instantiate(baseObject);
                newObj.transform.parent = transform;
                newObj.name = gameObject.name+" - Exploded Object No: "+ i;
                newObj.transform.position = new Vector3(transform.position.x + Random.Range(-0.005f, 0.005f), transform.position.y + 5 + Random.Range(-0.005f, 0.005f), transform.position.z + Random.Range(-0.005f, 0.005f));
                objects.Add(newObj);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        frames = currentFPS();
        for (int j= 0; j < objects.Count; j++)
        {
            Vector3 location = objects[j].transform.position;
            if (location.y < -100 && currentFPS() > 55)
            {
                Destroy(objects[j]);
                objects.RemoveAt(j);
            } // Removes object to saves resources
        }
    }

    int currentFPS()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        return (int)fps;
    }
}
