using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public int health = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            transform.position += new Vector3(0, speed, 0) * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            transform.position += new Vector3(0, -speed, 0) * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            transform.position += new Vector3(-speed, 0, 0) * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            transform.position += new Vector3(speed, 0, 0) * Time.deltaTime;
        }
        
    }

        void OnTriggerEnter2D(Collider2D col)
        {
            Debug.Log("Player collided with Enemy");
            health--;
            Debug.Log("Player health: " + health);

            if (health <= 0)
            {
                gameObject.SetActive(false);
            }
            
        }
}

