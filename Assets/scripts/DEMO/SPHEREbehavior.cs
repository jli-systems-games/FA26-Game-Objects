using UnityEngine;

public class SPHEREbehavior : MonoBehaviour
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
    
        // if (collision.gameObject.tag == "Player")
        // {
        //     Debug.Log
        //     gameObject.SetActive(false);
        // }
        // ^^ HIDES OBJECT FROM SCENE WHEN COLLIDED WITH PLAYER ^^

        // if (collision.gameObject.tag == "Player")
        // {
        //     Destroy(gameObject);
        // }
        //^DESTROYS OBJECT FROM SCENE WHEN COLLIDED WITH PLAYER^
    }
        
    
    
}
