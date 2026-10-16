using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float xSpeed;
    public float ySpeed;

    private HurtSound hurtSound;
    public Player playerScript;
    

    // Start is called before the first frame update
    void Start()
    {

        hurtSound = GameObject.Find("GameManager").GetComponent<HurtSound>();
        playerScript = GameObject.Find("Player").GetComponent<Player>();
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(xSpeed, ySpeed, 0) * Time.deltaTime;
    }

    public void OnTriggerEnter(Collider other)
    {
        print("I have hit something");
        hurtSound.DisplayARandomHurtText();
        playerScript.playerHealth -= 1;

        
    }


}
