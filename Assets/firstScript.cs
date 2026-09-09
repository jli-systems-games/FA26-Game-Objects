using UnityEngine;

public class SimpleMovement3D : MonoBehaviour
{
    public float moveSpeed = 1f; //values
    public float rotationSpeed = 300f; 
     public float jumpForce = 1f;
    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal"); //move inputes
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(moveX, 0f, moveZ).normalized;

        if (moveDirection != Vector3.zero)
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World); //move but not camera

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up); //this roatates and stuff

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) //jump if on ground, up if on
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; 
        }


    }

     private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
        }
    }

}
