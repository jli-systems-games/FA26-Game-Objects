using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitch : MonoBehaviour
{
    public GameObject mainCameraRef;
    public GameObject switchCamera;
    public GameObject currentCamera;
    void Start()
    {
        mainCameraRef = GameObject.Find("Main Camera");
        currentCamera = mainCameraRef;
    }
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("I have been hit");

        currentCamera.SetActive(false);
        switchCamera.SetActive(true);
        currentCamera = switchCamera;
    }
}

