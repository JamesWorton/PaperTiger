using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameAfterStopMoving : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] private string gameOverScene;
    [SerializeField]private float timeBeforeTransition;
    [SerializeField] private float smallerTimeBeforeTransition;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Destroy(GameObject.FindGameObjectWithTag("Music"));
    }

    // Update is called once per frame
    void Update()
    {
        if (timeBeforeTransition <=0 || smallerTimeBeforeTransition <=0)
        {
            SceneManager.LoadScene(gameOverScene);
        }
        timeBeforeTransition = timeBeforeTransition - Time.deltaTime;
        if (_rb.velocity.magnitude <=0)
        {
            smallerTimeBeforeTransition = smallerTimeBeforeTransition - Time.deltaTime;
        }
    }
}
