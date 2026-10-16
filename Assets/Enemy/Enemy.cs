using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float xSpeed;
    public float ySpeed;

    HurtSound hurtSound;
    HPUI hpUI;
    Vector3 startPosition;

 
    void Start()
    {

        hurtSound = FindObjectOfType<HurtSound>();
        hpUI = FindObjectOfType<HPUI>();
        startPosition = transform.position;

     // Unify all the enemies' speed
        Vector3 spawn = transform.localPosition;
        xSpeed = spawn.x >= 9 ? -3 : spawn.x <= -9 ? 3 : 0;
        ySpeed = xSpeed != 0 ? 0 : spawn.y >= 10 ? -3 : 3;
        
    }

     //refresh the enemies those gone
    void Update()
    {
        transform.position += new Vector3(xSpeed, ySpeed, 0) * Time.deltaTime;
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);

        if ((xSpeed > 0 && pos.x > 1.05f) ||
            (xSpeed < 0 && pos.x < -0.05f) ||
            (ySpeed > 0 && pos.y > 1.05f) ||
            (ySpeed < 0 && pos.y < -0.05f))
            transform.position = startPosition;
    }


    void OnTriggerEnter2D(Collider2D col)   // wow a spelling mistake
    {
        if (col.GetComponent<Player>() == null) return;
        if (Time.timeScale == 0 || hpUI.hp <= 0) return;

        hurtSound.DisplayARandomHurtText();
        hpUI.TakeDamage();
    }


}
