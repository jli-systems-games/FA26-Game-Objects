using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float xSpeed;
    public float ySpeed;
    private HurtSound hurtSound;
    public bool canMove = false;

    private Health playerHealth; 
    // Start is called before the first frame update
    void Start()
    {

        hurtSound = GameObject.Find("Game manager").GetComponent<HurtSound>();
        playerHealth = GameObject.Find("Game manager").GetComponent<Health>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove) return;
        transform.position += new Vector3(xSpeed, ySpeed, 0) * Time.deltaTime;
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("this hits me" + other.name);

        if (other.CompareTag("Player"))
        {
            hurtSound.DisplayARandomHurtText();
            playerHealth.TakeDamage();
        }
    }
}