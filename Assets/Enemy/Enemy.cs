using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float xSpeed;
    public float ySpeed;
    public float distance;
    public Vector3 startingPosition;

    HurtSound hurtSound;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        hurtSound = FindFirstObjectByType<HurtSound>();
        
    }

    // Update is called once per frame 
    void Update()
    {
        float newX = startingPosition.x + Mathf.Sin(Time.time * xSpeed) * distance;
        float newY = startingPosition.y + Mathf.Sin(Time.time * ySpeed) * distance;
        transform.position = new Vector3(newX, newY, 0);
        
    }


}
