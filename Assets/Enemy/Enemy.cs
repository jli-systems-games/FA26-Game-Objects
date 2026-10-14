using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float xSpeed;
    public float ySpeed;

    HurtSound hurtSound;

    void Start()
    {
        hurtSound = FindObjectOfType<HurtSound>();
    }

    void Update()
    {
        // Move the enemy
        transform.position += new Vector3(xSpeed, ySpeed, 0) * Time.deltaTime;

        // Reset the enemy after it leaves the screen
        if (transform.position.x > 10f ||
            transform.position.x < -10f ||
            transform.position.y > 6f ||
            transform.position.y < -6f)
        {
            ResetPosition();
        }
    }

    void ResetPosition()
    {
        float randomX = Random.Range(-8f, 8f);
        float randomY = Random.Range(-4f, 4f);

        // Mostly horizontal movement
        if (Mathf.Abs(xSpeed) >= Mathf.Abs(ySpeed))
        {
            if (xSpeed > 0)
            {
                transform.position = new Vector3(-10f, randomY, 0);
            }
            else
            {
                transform.position = new Vector3(10f, randomY, 0);
            }
        }
        // Mostly vertical movement
        else
        {
            if (ySpeed > 0)
            {
                transform.position = new Vector3(randomX, -6f, 0);
            }
            else
            {
                transform.position = new Vector3(randomX, 6f, 0);
            }
        }
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            hurtSound.PlayerHurt();
        }

    }

}