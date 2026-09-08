using UnityEngine;
using UnityEngine.InputSystem;
public class CUBE : MonoBehaviour
{
    public float moveSpeed; 
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
        transform.position += new Vector3(moveInput.x, moveInput.y, 0) * moveSpeed * Time.deltaTime; 

    }
}
