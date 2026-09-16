using UnityEngine;

public class BallStart : MonoBehaviour
{
    public Rigidbody rb;
public float pushForce = 6f;
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
       if (UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame)
    {
        Debug.Log("SPACE WORKS");
        rb.AddForce(-transform.right * pushForce, ForceMode.Impulse);
    }
        
    }
}
