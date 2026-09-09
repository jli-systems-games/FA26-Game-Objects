using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TransportScript : MonoBehaviour
{
    public float moveSpeed;
    public List<Transform> targetpositions;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                transform.position = targetpositions[Random.Range(0, targetpositions.Count)].position;

            }
    }
}
