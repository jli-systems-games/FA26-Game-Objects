using UnityEngine;
using UnityEngine.InputSystem;

public class MoveScript : MonoBehaviour
{
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
        transform.position += new Vector3(moveInput.x, moveInput.y, 0) * moveSpeed * Time.deltaTime;
    }
}
