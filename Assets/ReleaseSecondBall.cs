using UnityEngine;

public class ReleaseSecondBall : MonoBehaviour
{
    public Rigidbody secondBall;
    private bool released = false;

    private void OnTriggerEnter(Collider other)
    {
        if (released || other.gameObject.name != "Domino06" || secondBall == null)
            return;

        released = true;
        secondBall.isKinematic = false;
        secondBall.AddForce(Vector3.right * 5f, ForceMode.Impulse);
        Debug.Log("Second ball released!");
    }
}