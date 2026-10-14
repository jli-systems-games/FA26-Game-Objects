using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float xSpeed = -3f;
    public float ySpeed = 0f;

    HurtSound hurtSound;
    PlayerHealth playerHealth;

    // Start is called before the first frame update
    void Start()
    {

        hurtSound = FindFirstObjectByType<HurtSound>();
        playerHealth = FindFirstObjectByType<PlayerHealth>();

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(xSpeed, ySpeed, 0) * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
       
        if (col.CompareTag("Player"))
    {
            if (hurtSound != null)
            {
                hurtSound.PlayHurtSound();
            }

            if (playerHealth != null)
            {
                playerHealth.TakeDamage();
            }
        }
           
    }
}
