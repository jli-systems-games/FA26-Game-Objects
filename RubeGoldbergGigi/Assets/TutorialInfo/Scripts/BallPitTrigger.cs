using UnityEngine;

public class BallPitTrigger : MonoBehaviour
{
    public Rigidbody[] pitBalls;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Ball2")
        {
            foreach (Rigidbody ball in pitBalls)
            {
                ball.isKinematic = false;
            }
        }
    }
}