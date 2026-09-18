using UnityEngine;

public class CameraSwap : MonoBehaviour
{
    public GameObject mainCameraRef;//when active, find the main camera
    public GameObject switchCamera;
    public GameObject currentCamera;//this keeps track of what the current camera is

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
        Debug.Log("I Have Been Hit");
        currentCamera.SetActive(false);
        switchCamera.SetActive(true);
        currentCamera = switchCamera;


    }
}
