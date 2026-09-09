using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TransportScript : MonoBehaviour
{
    public float moveSpeed;
    public List<Transform> targetPositions;    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space)) //only if its pressed not how long it's preesed/released

        {
            transform.position = targetPositions[Random.Range(0, targetPositions.Count)].position;
            print("You are hitting spacebar");
        }
        //transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
}
