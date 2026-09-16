using UnityEngine;
using UnityEngine.SceneManagement; 

public class CameraTriggerZone : MonoBehaviour
{
    public CameraManager cameraManager;
    public int targetCameraIndex;

    [Header("Reset Settings")]
    public bool isResetTrigger = false; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>() != null)
        {
            if (isResetTrigger)
            {
                
                
                
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                return; 
            }

            if (cameraManager != null)
            {
                cameraManager.SwitchCamera(targetCameraIndex);
            
            }
        }
    }
}
