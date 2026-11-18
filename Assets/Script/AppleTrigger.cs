
using UnityEngine;

public class AppleTrigger : MonoBehaviour
{
    public float speed = 3f;
    private bool falling = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            falling = true;
    }

    void Update()
    {
        if (falling)
            transform.Translate(0, -speed * Time.deltaTime, 0);
    }
}
