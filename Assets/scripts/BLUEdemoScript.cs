using UnityEngine;
using UnityEngine.InputSystem; //importing the Input System package to use the new input system

public class BLUEdemoScript : MonoBehaviour
{
    //public GameObject blueCube;


// OLD VERSION: 2 floats to store the horizontal and vertical movement values, then add move speed to control how fast chara moves 
    // public float horizontalMove;
    // public float verticalMove; 
    
    public float moveSpeed; //variable will control movement speed of my object
    
    public Vector2 moveInput; //variable to store the input values for movement
    public InputActionReference moveAction; //variable to store the input action reference for movement

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

       
    }

    // Update is called once per frame
    void Update()
    {

moveInput = moveAction.action.ReadValue<Vector2>(); //reads the input values from the input action reference and stores them in the moveInput variable
transform.position += new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed * Time.deltaTime; //moves the object in the direction of the input values, multiplied by the move speed and delta time to make it frame rate independent

        transform.position += new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed * Time.deltaTime; //moves the object in the direction of the input values, multiplied by the move speed and delta time to make it frame rate independent
//INPUT MOVEMENT CODE
        // horizontalMove = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        // verticalMove = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        // transform.position += new Vector3(horizontalMove, 0, verticalMove); //params ask for x, y, z values to move the object in 3D space

// TRANSFORMATION CODE
     //transform.position = Vector3.Lerp(transform.position, blueCube.transform.position, Time.deltaTime);
     //^following target cube (blue)
     //transform.position += transform.forward * Time.deltaTime;
     //^moving forward in the direction the cube is facing



    }
}
