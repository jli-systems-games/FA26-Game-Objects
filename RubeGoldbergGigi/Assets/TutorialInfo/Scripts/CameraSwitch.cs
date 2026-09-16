using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public GameObject currentCamera;
    public GameObject nextCamera;

    private bool switched = false;

    void OnTriggerEnter(Collider other)
    {
        if (!switched)
        {
            currentCamera.SetActive(false);
            nextCamera.SetActive(true);
            switched = true;
        }
    }
}