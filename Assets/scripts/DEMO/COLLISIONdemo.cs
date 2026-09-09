using UnityEngine;
public class COLLISIONdemo : MonoBehaviour
{
    public Transform startPosLocation;
    public GameObject playerObj; 

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
        //print(collision.gameObject.name); 

        //playerObj.transform.position = startPosLocation.position;

        if(collision.gameObject.tag == "Player")
        {
            playerObj.transform.position = startPosLocation.position;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.name); 

        playerObj.transform.position = startPosLocation.position;
    }
}
