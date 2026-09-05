using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public class firstScript : MonoBehaviour
{
   //public GameObject redCube;
   public float moveSpeed;
  // public float verticalMove;
   //public float horizontalMove;
   
   public InputActionReference moveAction;

    public Vector2 moveInput;


   void Start()
   {

 
    moveInput = moveAction.action.ReadValue<Vector2>();

    transform.position += new Vector3(moveInput.x, 0, moveInput.y) *moveSpeed * Time.deltaTime;
   }

    void Update()
    {


        
       // horizontalMove = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime; old system
        //verticalMove = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        //transform.position += new Vector3(horizontalMove, 0, verticalMove);

        //transform.position = Vector3.Lerp(transform.position, redCube.transform.position, moveSpeed * Time.deltaTime); move to cube
    }

}