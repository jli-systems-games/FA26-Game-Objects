using UnityEngine;

public class BallController : MonoBehaviour
{
    public float initialVelocity = 10f;
    private Rigidbody rb;

    void Start()
    {
    rb = GetComponent<Rigidbody>();

    rb.linearVelocity = new Vector3(initialVelocity, 0.0f, 0.0f); 
    }
}
