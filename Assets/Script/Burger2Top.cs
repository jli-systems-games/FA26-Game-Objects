using UnityEngine;

public class Burger2Top : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Timer.timeLeft<=0.1) GetComponent<Rigidbody>().useGravity = true;
    }
}
