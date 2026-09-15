using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
public class CollisionDemo : MonoBehaviour

{
    public Transform startPoslocation;
    public GameObject PlayerObj;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerObj.transform.position = startPoslocation.position;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerObj.transform.position = startPoslocation.position;
        }
    }

}
