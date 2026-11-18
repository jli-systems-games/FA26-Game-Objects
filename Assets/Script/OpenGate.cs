using UnityEngine;

public class OpenGate : MonoBehaviour
{
    public GameObject target;
    public float speed = 2;
    float moved = 0;
    bool triggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            triggered = true;
    }

    void Update()
    {
        if (triggered && moved < 20)
        {
            float step = Mathf.Min(speed * Time.deltaTime, 20 - moved);
            target.transform.position += Vector3.down * step;
            moved += step;
        }
    }
}