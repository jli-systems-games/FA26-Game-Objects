using UnityEngine;

public class BounceCube : MonoBehaviour
{
    public float FlyDooDoo = 8f;
    public float FlyAmount = 0.6f;

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Sphere2")) return;
        Rigidbody sphere = collision.rigidbody;
        if (sphere == null) return;
        Vector3 direction = (transform.forward + Vector3.up * FlyAmount).normalized;
        sphere.linearVelocity = direction * FlyDooDoo;
    }
    //the direction is kinda unexpected but i like it so maybe il not change it
}
