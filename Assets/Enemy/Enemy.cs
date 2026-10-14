using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float xSpeed;
    public float ySpeed;

    HurtSound hurtSound;

    // Start is called before the first frame update
    void Start()
    {

        hurtSound = Object.FindFirstObjectByType<HurtSound>();
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(xSpeed, ySpeed, 0) * Time.deltaTime;
    }

    void OnTriggerEntur(Collider other)
    {
        hurtSound.DisplayARandomHurtText();
    }


}
