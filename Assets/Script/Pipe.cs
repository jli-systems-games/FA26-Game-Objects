
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public float hitSpeed = 7f, hitRotate = 360f;
    private float speed = 0f, rotateSpeed = 0f;

    void Update()
    {
        transform.Translate(speed * Time.deltaTime, 0, 0, Space.World);
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            speed = hitSpeed;
            rotateSpeed = hitRotate;
        }
    }
}
