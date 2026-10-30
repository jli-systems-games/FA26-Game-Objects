using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed;
    public float HealthVariable = 100;
    public float moveSpeed; 
    float horizontal;
    float vertical;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()

    {
        horizontal += Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
        vertical += Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime;
        transform.position = new Vector2 (horizontal,vertical); 

        //  if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
        //      transform.position += new Vector3(0, speed, 0) * Time.deltaTime;
        //  }

        // if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
        //     transform.position += new Vector3(0, -speed, 0) * Time.deltaTime;
        // }

        // if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
        //     transform.position += new Vector3(speed, 0, 0) * Time.deltaTime;
        // }

        // if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
        //     transform.position += new Vector3(speed, 0, 0) * Time.deltaTime;
        // }
        
    }
      public void OnCollisionEnter2D(Collision2D collision)
  {
      HealthVariable -= 10;

      if(HealthVariable <= 0)
      {
        SceneManager.LoadScene(0);
      }
  }
}


