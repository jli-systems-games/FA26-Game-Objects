using UnityEngine;

public class ResetZoneScript : MonoBehaviour
{
    public Rigidbody playerBody;
    public Transform startPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody == playerBody)
        {
            Debug.Log("Player entered ResetZone");

            playerBody.linearVelocity = Vector3.zero;
            playerBody.angularVelocity = Vector3.zero;
            playerBody.position = startPosition.position;
            playerBody.rotation = startPosition.rotation;
            playerBody.isKinematic = true;
        }
    }
}