using UnityEngine;

public class CamSystem : MonoBehaviour
{
    public GameObject currentCamera;//This will keep track of the current camera. 
    public GameObject switchCamera; // When the object is interacted with, it will switch to a specific camera.

    //public Camera mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentCamera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {

            GameObject mainCamera = GameObject.Find("Main Camera");
            mainCamera.SetActive(true);
            currentCamera = mainCamera;
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            switchCamera.SetActive(true);
            currentCamera.SetActive(false);
            currentCamera = switchCamera;
        }
    }
}
