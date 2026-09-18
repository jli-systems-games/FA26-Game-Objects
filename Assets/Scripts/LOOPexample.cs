using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class LOOPexample : MonoBehaviour
{
    public GameObject[] blueCubes;
    public float timer = 30f; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        blueCubes = GameObject.FindGameObjectsWithTag("Blue"); 

        for (int i = 0; i <= timer; i++)
        {
            //code inside here happens
            timer += Time.deltaTime;
            print("I variable is now" + i.ToString());
        }

        print("You finished the loop"); 
        
    }

    // Update is called once per frame (DO NOT put for loops in void update) 
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            //forLoopContainer(); //calling the forloop function 
            StartCoroutine(TimerGoDown());
        } 

        //StartCoroutine(TimerGoDown());
    }

public void forLoopContainer()
    {
        //code
    }


    public IEnumerator TimerGoDown() 
    {
        timer -= 1;
        yield return new WaitForSeconds(3f);
        print("Timer is now" + timer); 
    }

}
