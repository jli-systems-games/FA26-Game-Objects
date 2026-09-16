using UnityEngine;
using UnityEngine.InputSystem;

public class cameramove : MonoBehaviour
{
    public GameObject mainCameraRef;
    public GameObject switchCamera;
    public GameObject currentCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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