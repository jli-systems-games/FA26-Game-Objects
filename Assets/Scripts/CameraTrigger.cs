using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public GameObject triggeringObject;
    public Camera currentCamera;
    public Camera nextCamera;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == triggeringObject)
        {
            currentCamera.gameObject.SetActive(false);
            nextCamera.gameObject.SetActive(true);

            Debug.Log(gameObject.name + " changed camera!");
        }
    }
}
