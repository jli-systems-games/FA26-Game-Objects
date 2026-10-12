using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f; // Set a default speed in case you forget in Inspector
    public int health;

    private Animator anim;
    private Rigidbody rb;      // Added for physics movement
    private Vector2 moveInput;   // Added to store user input

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        health = 5;

        // Automatically set the Rigidbody settings needed for top-down games
        if (rb != null)
        {
           // rb.gravityScale = 0f;       // Stops your character from falling down the screen
            rb.freezeRotation = true;   // Stops your character from spinning when hitting walls
        }
    }

    // Update is called once per frame (Use ONLY for inputs and animations)
    void Update()
    {
        // 1. Reset input vector
        moveInput = Vector2.zero;

        // 2. Reset animation flags
        if (anim != null)
        {
            anim.SetBool("up", false);
            anim.SetBool("down", false);
            anim.SetBool("left", false);
            anim.SetBool("right", false);
        }

        // 3. Gather Input and Set Animation States
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            moveInput.y = 1;
            if (anim != null) anim.SetBool("up", true);
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            moveInput.y = -1;
            if (anim != null) anim.SetBool("down", true);
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveInput.x = -1;
            if (anim != null) anim.SetBool("left", true);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveInput.x = 1;
            if (anim != null) anim.SetBool("right", true);
        }
    }

    // FixedUpdate runs on a reliable timer and is where ALL physics movement must go
    void FixedUpdate()
    {
        // 4. Apply physics-based movement using Rigidbody2D velocity
        // .normalized stops diagonal movement from being faster than horizontal/vertical movement
        rb.linearVelocity = moveInput.normalized * speed;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            print("I've been Hit");
            health--;
        }
    }
}
