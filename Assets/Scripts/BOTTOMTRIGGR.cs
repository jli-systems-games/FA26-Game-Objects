using UnityEngine;

public class BOTTOMTRIGGR : MonoBehaviour
{
    public GameObject mainCamera;//This variable keeps track of what the current camera is.

    public GameObject switchCamera;

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

        
        mainCamera.SetActive(false);
        switchCamera.SetActive(true);
        mainCamera = switchCamera;
        
       
    }
}
