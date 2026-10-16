using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;

    void Start()
    {
        
    }
void Update()
{
    if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && transform.position.y < 5.5f)  //not allowed to move out the canvas
    {
        transform.position += new Vector3(0, speed, 0) * Time.deltaTime;
    }

    if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && transform.position.y > -3.5f)
    {
        transform.position += new Vector3(0, -speed, 0) * Time.deltaTime;
    }

    if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && transform.position.x > -6f)
    {
        transform.position += new Vector3(-speed, 0, 0) * Time.deltaTime;
    }

    if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && transform.position.x < 6f)
    {
        transform.position += new Vector3(speed, 0, 0) * Time.deltaTime;
    }
}
}
