using UnityEngine;

public class 01 : MonoBehaviour;

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
        Debug.Log("i am deleted");
        if (collision.gameObject.CompareTag("Target"))
            {
             Destroy(collision.gameObject);
            }   
    }
}
