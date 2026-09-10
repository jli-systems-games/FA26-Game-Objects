using UnityEngine;

public class SphereSpawn : MonoBehaviour
{
    public Transform sphereSpawn;
    public GameObject spherePrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space)) 
        { 
            Instantiate(spherePrefab,sphereSpawn.position,Quaternion.identity);   
        }
    }
}
