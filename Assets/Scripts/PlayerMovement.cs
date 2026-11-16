using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
     public Rigidbody rb;
    public float forcepower; //jump strength
    public float moveSpeed;
    public float moveX;
    public float moveY;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       rb = GetComponent<Rigidbody>(); 
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        // moveInput = PlayerMovement.action.ReadValue<Vector2>();
        // transform.position += new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed * Time.deltaTime;
        
        
        Vector3 jumpForce = transform.up;
        jumpForce = jumpForce * forcepower;


        if (Input.GetKey(KeyCode.Space)) 
        {
            rb.AddForce(jumpForce, ForceMode.Impulse);
        }

        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");

        transform.position += new Vector3(moveX, 0, moveY) * moveSpeed * Time.deltaTime;
    }
}
