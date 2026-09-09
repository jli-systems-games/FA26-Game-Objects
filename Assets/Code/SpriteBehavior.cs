using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class SpriteBehavior : MonoBehaviour
{
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
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        if (collision.gameObject.tag == "Player")
        {
            //gameObject.SetActive(false);
            Destroy(gameObject, 3f);
        }
    }
    


}
