using UnityEngine;
using UnityEngine.InputSystem;

public class movedemoscript : MonoBehaviour
{
  
    public float moveSpeed;
    public Vector2 moveInput;
    public InputActionReference moveAction;
    public bool escaped = false;
    public GameObject mazeParent;
    public float spinSpeed = 200f;


   
    void Start()
    {
        moveAction.action.Enable();
    }

    
    void Update()
    {
        moveInput = moveAction.action.ReadValue<Vector2>();

        transform.position += new Vector3(-moveInput.y, 0, moveInput.x) * moveSpeed * Time.deltaTime;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
          GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
        }

        if (escaped == true)
        {
          mazeParent.transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
        }

  
       
        
    }
      void OnTriggerEnter(Collider other)
        {
          escaped = true;
        }

}
