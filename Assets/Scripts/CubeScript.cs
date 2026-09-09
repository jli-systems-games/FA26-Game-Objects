using UnityEngine;
using UnityEngine.InputSystem;

public class CubeScript : MonoBehaviour
{
    //public GameObject YellowCube;
    //public GameObject RedCube;
    public float moveSpeed;
    public Vector2 moveInput;
    public InputActionReference moveAction;

    //public float horizontalMove;
    //public float verticalMove;
  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = moveAction.action.ReadValue<Vector2>();
        Vector3 movement = new Vector3(moveInput.x, moveInput.y);
        transform.position += movement * moveSpeed * Time.deltaTime; 

        //transform.position = Vector3.MoveTowards(transform.position,RedCube.transform.position,moveSpeed * Time.deltaTime);
        //transform.position = Vector3.Lerp(transform.position, RedCube.transform.position, moveSpeed * Time.deltaTime);
        //transform.position = RedCube.transform.position;
        //transform.position += transform.forward * moveSpeed * Time.deltaTime;
        //horizontalMove = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        //verticalMove = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        //transform.position += new Vector3(horizontalMove, 0, verticalMove);

    }
}
