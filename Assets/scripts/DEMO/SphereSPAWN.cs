using UnityEngine;

public class SphereSPAWN : MonoBehaviour
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(spherePrefab, sphereSpawn.position, sphereSpawn.rotation);
        }
    }
}
