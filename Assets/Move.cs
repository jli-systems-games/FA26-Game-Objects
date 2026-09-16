using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public float force;
    public Rigidbody rb;

    Vector2 move;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void MoveInput(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }
    void Start()
    {
        
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        Vector3 rbMovement = new Vector3(move.x, 0, move.y);
        rbMovement = rbMovement.normalized * force * Time.deltaTime;
        rb.MovePosition(transform.position + rbMovement);
    }
}
