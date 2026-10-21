using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //making a ray from the canter of the viewport(camera) goes from 0-1 so 0.5
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        //hitting 
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            Debug.Log("hit something");
        }
        else
        {
            Debug.Log("Hit nothing");
        }
        //right now it works no matter how far away a thing is
    }
}
