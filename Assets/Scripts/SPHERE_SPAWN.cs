using UnityEngine;

public class SPHERE_SPAWN : MonoBehaviour
{

    public float speed = 2.0f;
    public float distance = 3.0f;
    public Vector3 startPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float newX = startPosition.x + Mathf.Sin(Time.time * speed) * distance;
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
