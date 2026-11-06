using UnityEngine;
using UnityEngine.InputSystem;

//Variables list:

// control for CharacterController
// move 
// look 


public class characterlook : MonoBehaviour
{
    public GameObject Camera;

    private float speed = 5f;
    public CharacterController control;
    public InputAction move, look;
    private Vector2 direction, mouse;


    void Start()
    {
        control = GetComponent<CharacterController>();
        move = GetComponent<PlayerInput>().actions["Move"];
        look = GetComponent<PlayerInput>().actions["Look"];
    }

    void Update()
    {
       
       
            direction = move.ReadValue<Vector2>();
            control.Move(this.transform.TransformDirection(new Vector3(direction.x, 0, direction.y)) * speed * Time.deltaTime);
            mouse = look.ReadValue<Vector2>();
            transform.Rotate(0, mouse.x * 0.1f, 0);
            Camera.transform.Rotate(-mouse.y * 0.1f, 0, 0);
      

    }
}