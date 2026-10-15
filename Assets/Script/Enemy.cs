using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    public float xSpeed;
    public float ySpeed;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("ChangeDirection", 0f, 2f); // Change direction every 2 seconds
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(xSpeed, ySpeed); // Set the velocity of the Rigidbody2D to move the enemy
    }

    void ChangeDirection() // Change the direction of the enemy
    {
        Vector2 direction = Random.insideUnitCircle.normalized; 
        float speed = Random.Range(2f, 5f);

        xSpeed = direction.x * speed;
        ySpeed = direction.y * speed;
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.collider.gameObject.layer == LayerMask.NameToLayer("Wall") // Check if collide with wall
            && col.contactCount > 0)
        {
            Vector2 direction = new Vector2(xSpeed, ySpeed);
            Vector2 normal = col.GetContact(0).normal;

            if (Vector2.Dot(direction, normal) < 0)
            {
                direction = Vector2.Reflect(direction, normal);

                xSpeed = direction.x;
                ySpeed = direction.y;
            }
        }
    }
}


