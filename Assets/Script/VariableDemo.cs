using System.Collections.Generic;
using UnityEngine;

public class VariableDemo : MonoBehaviour
{
    public float floatVar = 3.5f;
    public int intVar = 10;
    public string greetingMessage = "Hey everyone!";

    public List<float> listDemo;
    public string[] arrayDemo;
    public GameObject objcDemo;

    void Start()
    {
        print(greetingMessage);
    }

    void Update()
    {
    }
}