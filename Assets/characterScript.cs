using UnityEngine;

public class characterScript : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float mouseSensitivity = 2f;
    public Transform camera;

    public float sprintSpeed = 10f;
    public float jump = 6f;

    Rigidbody rb;
    float currentAngle;
    bool jumped;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update() //for the looking around
    {
        float mx = Input.GetAxis("Mouse X") * mouseSensitivity;
        float my = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(0f, mx, 0f);
        //so the camera doesn't flip around
        currentAngle = Mathf.Clamp(currentAngle - my, -80f, 80f);

        camera.localRotation = Quaternion.Euler(currentAngle, 0f, 0f);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumped = true;
        }
        
    }

    void FixedUpdate() //for the movement
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 move = (transform.right * x + transform.forward * y);
        //so w+d doesn't go faster together
        if (move.sqrMagnitude > 1f) move.Normalize();

        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

        //builds the velocity for unity physics
        Vector3 v = move * speed; 
        v.y = rb.linearVelocity.y;

        if (jumped && IsGrounded())
        {
            v.y = jump;
        }

        jumped = false;
        rb.linearVelocity = v;
    }

    bool IsGrounded()
    {
        return Physics.SphereCast(transform.position, 0.4f, Vector3.down, out _, 0.7f);
    }

}
