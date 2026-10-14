using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float xSpeed;
    public float ySpeed;
    HurtSound hurtSound;
    Rigidbody2D rb;

    void Start()
    {
        hurtSound = FindObjectOfType<HurtSound>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("ChangeDirection", 0f, 2f);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(xSpeed, ySpeed);
    }

    void ChangeDirection()
    {
        Vector2 direction = Random.insideUnitCircle.normalized;
        float speed = Random.Range(2f, 5f);

        xSpeed = direction.x * speed;
        ySpeed = direction.y * speed;
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.collider.gameObject.layer == LayerMask.NameToLayer("Wall")
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

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && hurtSound != null)
        {
            hurtSound.DisplayARandomHurtText();
        }
    }

}


