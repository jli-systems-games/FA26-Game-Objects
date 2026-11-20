
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float rotateSpeed;//sensitivity
    private AudioSource footstepAudio;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveSpeed = 10;
        footstepAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //player rotation
        transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0) * Time.deltaTime * rotateSpeed);

        //basic movement
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        //transform.Translate(new Vector3(h, 0, v) * moveSpeed * Time.deltaTime); (world x and y)
        

        // makes sure the character moves accordeing to the direction it is moving
        Vector3 moveDirection = new Vector3(h, 0f, v);
        moveDirection = transform.TransformDirection(moveDirection);
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        // Check if any movement key is currently being held
        bool isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) ||
                        Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) ||
                        Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) ||
                        Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

        // Start the loop if moving and not already playing; stop it when idle
        if (isMoving)
        {
            if (!footstepAudio.isPlaying)
            {
                footstepAudio.Play();
            }
        }
        else
        {
            if (footstepAudio.isPlaying)
            {
                footstepAudio.Stop();
            }
        }
    }

}
        

