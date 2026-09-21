using UnityEngine;
using System.Collections;

public class LoopExample: MonoBehaviour
{
    public GameObject[] greenCubes;
    public float timer = 30f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        greenCubes = GameObject.FindGameObjectsWithTag("green");

        for (int i= 0; i >= timer; i++)
        {
            //code inside here happens
            print ("I variable is now" + i.ToString ());

        }
        print("You finished the loop");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            forLoopContainer();
        }
    }
    public void forLoopContainer() 
    {
        for (int i = 0 ; i <= timer; i++)
        {
            print("I variable is now" + i.ToString());
        }
    }

    public IEnumerator TimerRoutine()
    {
        timer -= 1;
        yield return new WaitForSeconds(3f);
        print("Timer is now: " + timer);
    }
}
