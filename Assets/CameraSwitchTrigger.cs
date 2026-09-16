using UnityEngine;

public class CameraSwitchTrigger : MonoBehaviour
{
    public Rigidbody targetBody;
    public GameObject previousCamera;
    public GameObject nextCamera;

    private bool switched = false;

    private void OnTriggerEnter(Collider other)
    {
        if (switched || targetBody == null ||
            other.attachedRigidbody != targetBody ||
            previousCamera == null || nextCamera == null)
            return;

        switched = true;
        previousCamera.SetActive(false);
        nextCamera.SetActive(true);
        Debug.Log("Camera switched!");
    }
}