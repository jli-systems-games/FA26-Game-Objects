using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;

public class LoopExample : MonoBehaviour
{
    public GameObject[] blueCubes;
    public float timer = 30f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        blueCubes = GameObject.FindGameObjectsWithTag("blueCubes");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            StartCoroutine(TimerGoDown());
        }
    }
    public void forLoopContainter()
    {
        for (int i = 0; i<= timer; i++)
        {
            //code inside here happens
            print("I variable is now" + i.ToString());
        }
    }

    public IEnumerator TimerGoDown()
    {
        print ("You have called this function");
        timer -= 1;
        yield return new WaitForSeconds(3f);
        print("Timer is now:" + timer);
    }
}
