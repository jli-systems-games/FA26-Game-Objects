using UnityEngine;
using System.Collections;
using TMPro;

public class LoopExample : MonoBehaviour
{
    public GameObject[] blueCubes;
    public TMP_Text timerText;
    public float timerNumber;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        blueCubes = GameObject.FindGameObjectsWithTag("blue");
    }

    // Update is called once per frame
    void Update()
    { 
        if (Input.GetKey(KeyCode.Space))   
        {
            timerNumber += Time.deltaTime;
            timerText.text = timerNumber.ToString("N0");
        }
        else if(Input.GetKeyUp(KeyCode.Space)) 
        {
            timerNumber += 0;
            timerText.text = timerNumber.ToString("N0");
        }
    }
    public void StartTimer() 
    { 
    
    }

    public void EndTimer() 
    { 
        
    }
    

}
