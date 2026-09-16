using UnityEngine;

public class PhysicsButton : MonoBehaviour
{
    public Rigidbody secondBallRigidbody; 
    public float launchVelocity = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (secondBallRigidbody != null)
        {
            secondBallRigidbody.linearVelocity = new Vector3(-launchVelocity, 0.0f, 0.0f);

        }
         
    }
}
