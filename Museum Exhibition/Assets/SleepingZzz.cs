using UnityEngine;

public class SleepingZzz : MonoBehaviour
{
    public float floatSpeed = 0.5f;
    public float floatHeight = 3f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        if (transform.position.y >= startPosition.y + floatHeight)
        {
            transform.position = startPosition;
        }
    }
}