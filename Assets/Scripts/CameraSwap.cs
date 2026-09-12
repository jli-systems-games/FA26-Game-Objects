using UnityEngine;

public class CameraSwap : MonoBehaviour
{
    public GameObject mainCameraRef;//When active, find the Main Camera.
    public GameObject currentCamera;//This variable keeps track of what the current camera is.

    public GameObject switchCamera;//Camera game switches to when trigger is interacted with.

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCameraRef = GameObject.Find("Main Camera");
        currentCamera = mainCameraRef;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("I have been hit");

        currentCamera.SetActive(false);
        switchCamera.SetActive(true);
        currentCamera = switchCamera;


       
    }
}
