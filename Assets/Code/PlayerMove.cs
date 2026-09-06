using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody rb;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private Animator animator;

void Start()
{
    rb = GetComponent<Rigidbody>();
    playerInput = GetComponent<PlayerInput>();
    moveAction = playerInput.actions["Walking"]; // Using DemoAction
    animator = GetComponentInChildren<Animator>();
}

void Update()
{
    Vector2 input = moveAction.ReadValue<Vector2>();

    Vector3 moveInput = new Vector3(
        input.x,
        0f,
        input.y
    ); // Convert 2D input to 3D movement

    rb.linearVelocity = new Vector3(
        moveInput.x * moveSpeed,
        rb.linearVelocity.y,
        moveInput.z * moveSpeed
    );

    animator.SetFloat("Horizontal", moveInput.x);
    animator.SetFloat("Vertical", moveInput.z);
    // Trigger corresponding walking animation based on WASD input
    }
}
