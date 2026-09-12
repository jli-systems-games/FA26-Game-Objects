using UnityEngine;
using UnityEngine.InputSystem;

public class MoveScript : MonoBehaviour
{
    public Rigidbody rb;
    public float forcepower;

    //==================
    [Space]
    [Header("Using Unity movmement")]
    public float moveSpeed;
    public InputActionReference moveAction;
    public Vector2 moveInput;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       rb = GetComponent<Rigidbody>(); 
      
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 jumpForce = transform.forward;
        jumpForce = jumpForce * forcepower;


        if (Input.GetKey(KeyCode.Space)) 
        {
            Debug.Log("You pressed the space key");
          
            rb.AddForce(jumpForce, ForceMode.Impulse);
        }

        //moveInput = moveAction.action.ReadValue<Vector2>();
        //transform.position += new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed * Time.deltaTime;
    }
}
