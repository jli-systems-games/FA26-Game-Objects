using UnityEngine;

public class SPHERE_SPAWN : MonoBehaviour
{

    public Transform sphereSpawn;
    public GameObject spherePrefab;
    public Camera mainCamera; 

    public float speed = 2.0f;
    public float distance = 3.0f;
    public Vector3 startPosition;
    public float timer = 0f; 
    public float checkInterval = 2.0f; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float newX = startPosition.x + Mathf.Sin(Time.time * speed) * distance;
        
        if(mainCamera.gameObject.activeInHierarchy)
        {
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);

            timer += Time.deltaTime;
            if (timer >= checkInterval)
            {
                Instantiate(spherePrefab, sphereSpawn.position, sphereSpawn.rotation);
                timer -= checkInterval;
            }
        }
    }
}
