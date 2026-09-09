using UnityEngine;
using System.Collections.Generic;
public class VariableDemo : MonoBehaviour
{
    public float floatVar;
    public int intVar = 10;
    public string GreetingMessage = "Hello, World!";

    public List<float> ListDemo;
    public string[] arrayDemo;

    public GameObject objcDemo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        print(GreetingMessage);
        print("This number is: " + intVar);
    }

    // Update is called once per frame
    void Update()
    {
        floatVar += 1 * Time.deltaTime;
        if (floatVar >= 30)
        {
            MyFunction();
        }
    }

    public void MyFunction()
    {
        print("You activated the function");

    }
}
