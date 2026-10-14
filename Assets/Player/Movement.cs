using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //private Rigidbody2D rb;

    public float moveSpeed;
    public float speedLimit;

    

    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }

    // void FixedUpdate()
    // {
    //     if (horizontal != 0 && vertical != 0)
    //     {
    //         horizontal *= speedLimit;
    //         vertical *= speedLimit;
    //     }

    //     rb.linearVelocity = new Vector2(horizontal * moveSpeed, vertical * moveSpeed);
    // }
}

