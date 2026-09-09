using UnityEngine;
using UnityEngine.InputSystem;
public class INclassScript : MonoBehaviour
{

    //public float horizontalMove;
    //public float verticalMove;
    public float moveSpeed;
    public Vector2 moveInput;
    public InputActionReference MoveAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = MoveAction.action.ReadValue<Vector2>();
        transform.position += new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed * Time.deltaTime;

        //horizontalMove = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime * Time.deltaTime;
        //verticalMove = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime * Time.deltaTime; 
        //transform.position += new Vector3(horizontalMove, 0, verticalMove);
    }
}
