using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sphere1 : MonoBehaviour
{
    public float moveSpeed = 10f;
    private bool started = false; //yes or no switch
    private Rigidbody rb;
    private bool NoTotsugeki = false;
    private bool Totsugeki = false;
    private bool stopped = false;
    public GameObject startText;
    // public Vector3 moveDirection = Vector3.right;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //find rigid body and save in this variable
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 movement = Vector3.zero;
        // //press space to start!
        // if (Keyboard.current.spaceKey.isPressed)
        // {
        //     Totsugeki(); oh no i cant use this anymore
        // }
        //if (!started && Input.GetKeyDown(KeyCode.Space))
        //that didnt work
        if (!started && Keyboard.current !=null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Totsugeki = true;
            NoTotsugeki = true;
            if (startText !=null)
            {
                startText.SetActive(false);
            }
        }
    }

    // void Totsugeki()
    // {
    //     transform.position += Vector3.right * moveSpeed * Time.deltaTime;
    // }
    void FixedUpdate()
    //updazte according to physical update
    {
        if (NoTotsugeki)
        {
            //rb.MovePosition(rb.position + Vector3.right * moveSpeed * Time.fixedDeltaTime);
            NoTotsugeki = false;
            rb.AddForce(Vector3.right * moveSpeed, ForceMode.VelocityChange);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!stopped && collision.gameObject.CompareTag("Dominooo"))
        {
            stopped = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
