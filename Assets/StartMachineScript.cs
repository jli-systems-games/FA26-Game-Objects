using UnityEngine;
using UnityEngine.InputSystem;

public class StartMachineScript : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame &&
            rb.isKinematic)
        {
            rb.isKinematic = false;
            Debug.Log("Machine started!");
        }
    }
}