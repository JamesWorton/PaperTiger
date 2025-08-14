using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollision : MonoBehaviour
{
    BoxCollider selfCollider;
    [SerializeField] private string NameOfUser;

    // Start is called before the first frame update
    void Start()
    {
        selfCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name != NameOfUser && NameOfUser == "Maarntey" && other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage();
        }
        else if (other.gameObject.name != NameOfUser && NameOfUser == "Maarntey" && other.GetComponent<DestructableObject>() != null)
        {
            other.gameObject.GetComponent<DestructableObject>().destroyObject();
        }
        else if (other.gameObject.name != NameOfUser)
        {
            //Maarntey Takes Damage
        }
    }
}
