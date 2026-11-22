using UnityEngine;

public class Billboarding : MonoBehaviour



{
    private Camera mainCamera;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        cameraPosition.y = transform.position.y; 
        transform.LookAt(cameraPosition);
        transform.Rotate(0f, 180f, 0f);
       
        
        
    }
}
