//using UnityEngine;

//public class EnemyMove : MonoBehaviour
//{ // FIX: Added missing opening bracket for the class

//    // FIX: Removed the duplicate speed variable at the top
//    public float speed = 5.0f;

//    private HurtSound hurtSound;
//    private float startPos;
//    private int direction = 1; // 1 means forward/up, -1 means backward/down

//    public float maxDistance = 3.0f; // Total distance to move before turning around
//    public bool moveOnX = false;     // SET TO FALSE BY DEFAULT TO MOVE ON Y AXIS

//    void Start()
//    {
//        hurtSound = Object.FindFirstObjectByType<HurtSound>();

//        // Save the correct starting position based on axis
//        if (moveOnX) startPos = transform.position.x;
//        else startPos = transform.position.y;
//    }

//    void Update()
//    {
//        if (moveOnX)
//        {
//            // 1. Move the object on X
//            transform.position += new Vector3(direction * speed * Time.deltaTime, 0, 0);

//            // 2. Check if it went too far right, or too far left, and flip direction
//            if (transform.position.x > startPos + maxDistance) direction = -1;
//            else if (transform.position.x < startPos) direction = 1;
//        }
//        else // Move on Y
//        {
//            // 1. Move the object on Y
//            transform.position += new Vector3(0, direction * speed * Time.deltaTime, 0);

//            // 2. Check if it went too far up, or too far down, and flip direction
//            if (transform.position.y > startPos + maxDistance) direction = -1;
//            else if (transform.position.y < startPos) direction = 1;
//        }
//    }


//    void OnTriggerEnter(Collider other)
//    {
//        // Added a safety check to make sure hurtSound was successfully found in Start()
//        if (hurtSound != null)
//        {
//            hurtSound.DisplayARandomHurtText();
//        }
//    }
//}
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float speed = 5.0f;
    private HurtSound hurtSound;

    [Header("Movement Targets")]
    public Transform pointA; // Drag your first empty GameObject here
    public Transform pointB; // Drag your second empty GameObject here

    private Transform currentTarget;

    void Start()
    {
        hurtSound = Object.FindFirstObjectByType<HurtSound>();

        // Start by walking towards Point A
        if (pointA != null)
        {
            currentTarget = pointA;
        }
    }

    void Update()
    {
        if (currentTarget == null) return;

        // 1. Move towards the current active target position
        transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);

        // 2. Check if we have arrived exactly at the target
        if (transform.position == currentTarget.position)
        {
            // Swap targets
            if (currentTarget == pointA)
            {
                currentTarget = pointB;
            }
            else
            {
                currentTarget = pointA;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (hurtSound != null)
        {
            hurtSound.DisplayARandomHurtText();
        }
    }
}
