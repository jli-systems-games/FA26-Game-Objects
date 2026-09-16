using UnityEngine;

public class WINTRIGGER : MonoBehaviour
{

    public GameObject currentCamera;//This variable keeps track of what the current camera is.

    public GameObject switchCamera;//Camera game switches to when trigger is interacted with.
    public int count = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

     public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Entered");
        count++; 
        if (count == 5)
        {
            currentCamera.SetActive(false);
            switchCamera.SetActive(true);
            currentCamera = switchCamera;
        }
       
    }
}
