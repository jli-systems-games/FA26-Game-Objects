using UnityEngine;

public class SheepMovement : MonoBehaviour
{
    public float speed = 0.5f;
    public float leftBoundary = -5f;
    public float rightBoundary = 5f;

    private float startY;
    private float floatOffset;

    void Start()
    {
        startY = transform.position.y;
        floatOffset = Random.Range(0f, 6.28f);
    }

    void Update()
    {
       transform.position += Vector3.forward * speed * Time.deltaTime;

        Vector3 pos = transform.position;
        pos.y = startY + Mathf.Sin(Time.time * 1.5f + floatOffset) * 0.15f;

        if (pos.z > rightBoundary)
{
    pos.z = leftBoundary;
}
        transform.position = pos;
    }
}
