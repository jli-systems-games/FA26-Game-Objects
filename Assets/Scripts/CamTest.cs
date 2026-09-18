using UnityEngine;

public class CamTest : MonoBehaviour
{
    [Header("Camera References")]
    public GameObject mainCamera;
    public GameObject newCamera;
 

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
        mainCamera.SetActive(false);
        newCamera.SetActive(true);
    }
}
