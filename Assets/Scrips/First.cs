
using UnityEngine;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using JetBrains.Annotations;

public class First : MonoBehaviour
{


    // the most common c# variable s ar my sdisposal
    public float floatVar = 3.5f;
    public int intVar = 10;
    public string stingVar = "Hello World";
    public List<float> listVar;
    public string[] arrayDemo;
    
    //How to make a variable- first declare is Public(anyone can access), private(exclusive acess, only inside the script), static
    //declare what kind of variable it is-  sting, float, gameobjectt, whole or part numbers, 
    //what do you want to name your variable? Case and spelling sensitive
    public string greetingsMessage = "Hello World";

    //unity specific
    public GameObject objcDemo;

    //How does code work? Computers do things by reading lines of code one line at a  time top to bottom. one at a time, very quickly, never at the same time.
    //The speed of a frame is 1/60th of a second. 
    // semicolons ; are the period to end a sentence at the end of a line of code
    // Start is called once before the first execution of Update after the MonoBehaviour is created. Im only going to read what is in this once and never again. 
    void Start()
    {
        print("hey everyone!");
        print(greetingsMessage);
    }

    // Update is called once per frame. Every 1/60th of a second over and over again
    void Update()
    {
        print("hey everyone again...");
        floatVar += 1 * Time.deltaTime;

        if (floatVar == 100);
            {
            print("made it to 100");
        }
       
    }
}
