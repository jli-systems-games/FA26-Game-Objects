using UnityEngine;
using UnityEngine.InputSystem;

public class CUPmove : MonoBehaviour



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
        moveInput = moveAction.action.ReadValue<Vector2>(); //reads the input values from the input action reference and stores them in the moveInput variable
        transform.position += new Vector3(moveInput.x, 0, 0) * moveSpeed * Time.deltaTime; 
    }
}
