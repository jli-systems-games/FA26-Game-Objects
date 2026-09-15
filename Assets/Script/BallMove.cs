using UnityEngine;
using UnityEngine.InputSystem;

public class BallMove : MonoBehaviour
{
    public Rigidbody rb;
    public float ForceSpeed = 10f;
    public float moveSpeed = 5f;
    public Vector2 moveInput;
    public InputActionReference MoveAction;

    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

// Move input system
    void Update()
    {
        if (MoveAction != null)
        {
            moveInput = MoveAction.action.ReadValue<Vector2>();

            Vector3 movement = new Vector3(
                moveInput.x,
                0f,
                moveInput.y
            );

            transform.position += movement * moveSpeed * Time.deltaTime;
        }

// When the space key is pressed, apply a forward force to the ball
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Vector3 forwardForce = transform.forward * ForceSpeed;

            rb.AddForce(
                forwardForce,
                ForceMode.Impulse
            );
        }
    }
}