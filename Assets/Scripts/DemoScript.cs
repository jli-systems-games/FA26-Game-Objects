using UnityEngine;
using UnityEngine.InputSystem;

public class DemoScript : MonoBehaviour
{

    //public float horizontalMove;
    //public float verticalMove;

    public float moveSpeed;//This variable will control the move speed of my object. 
    public Vector2 moveInput;
    public InputActionReference moveAction;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        moveInput = moveAction.action.ReadValue<Vector2>();

        transform.position += new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed * Time.deltaTime;
 

        //horizontalMove = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        //verticalMove = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        //transform.position += new Vector3(horizontalMove, 0, verticalMove);
        
        
    }
}
