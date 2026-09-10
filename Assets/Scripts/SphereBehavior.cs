using UnityEngine;

public class SphereBehavior : MonoBehaviour
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
        Debug.Log("I am deleted.");
        if (collision.gameObject.tag == "Player") 
        {
            
            //gameObject.SetActive(false);

            Destroy(gameObject, 3f);
        }
    }
}
