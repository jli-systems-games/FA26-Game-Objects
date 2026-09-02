using UnityEngine;
using UnityEngine.InputSystem;

public class NewMoveScript : MonoBehaviour
{
    public float moveSpeed;
    private Vector2 moveInput;
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
    }

    public void moveCharacter(InputAction.CallbackContext context) 
    { 
        //moveInput = context.ReadValue<Vector2>();
    }
}
