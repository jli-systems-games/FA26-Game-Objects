using UnityEngine;
using UnityEngine.InputSystem;

public class TestScript : MonoBehaviour
{
    public Rigidbody rb;
    public float forcepower;
    public float moveSpeed;
    public Vector2 moveInput;
    public InputActionReference moveAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 jumpForce = transform.up;
        jumpForce = jumpForce * forcepower;

        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            print("I am activating");
        }

        moveInput = moveAction.action.ReadValue<Vector2>();
        transform.position += new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed * Time.deltaTime;
    }
}
