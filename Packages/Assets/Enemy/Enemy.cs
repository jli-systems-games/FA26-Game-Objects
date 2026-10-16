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
        hurtSound = FindFirstObjectByType<HurtSound>();
    }

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
                hurtSound.DisplayARandomHurtText();
            }
        }
    }
}
