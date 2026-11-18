using UnityEngine;
using UnityEngine.InputSystem;

//Variables list:

// control for CharacterController
// move 
// look 


public class MainCharacter : MonoBehaviour
{

     private float speed = 5f;
     public CharacterController control;
     public InputAction move, look;
     private Vector2 direction,mouse;
     private float pitch;


    void Start()
    {
        control = GetComponent<CharacterController>();  
        move = GetComponent<PlayerInput>().actions["Move"];
        look = GetComponent<PlayerInput>().actions["Look"];
    }

    void Update()
    {
        if (MainLogic.scene == 0)
        {
            direction = move.ReadValue<Vector2>();
            control.Move(this.transform.TransformDirection(new Vector3(direction.x,0,direction.y)) * speed * Time.deltaTime);
            mouse = look.ReadValue<Vector2>();
            transform.Rotate(0, mouse.x * 0.5f, 0);

            pitch = Mathf.Clamp(pitch - mouse.y * 0.5f, -80f, 80f);
            Camera.main.transform.localRotation = Quaternion.Euler(pitch, 0, 0);

    }
    }
}
