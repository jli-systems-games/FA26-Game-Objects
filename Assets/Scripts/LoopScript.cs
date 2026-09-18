using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class LoopScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject[] blueCube;
    public float timer = 30f;
    void Start()
    {
        blueCube = GameObject.FindGameObjectsWithTag("Cube");
        
        //for (int i = 0; i <= 30f; i++)
        //{
        //    //
        //}
        //print("You finished the loop");

    }

    // Update is called once per frame
    void Update()
    {
        //StartCoroutine(TimergoDown()); 
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            StartCoroutine(TimergoDown());
        }
    }
    public IEnumerator TimergoDown()
    {
        print("You have called this function");
        timer -= 1;
        yield return new WaitForSeconds(3f);
        print("Timer is now " +  timer);
    }
}
