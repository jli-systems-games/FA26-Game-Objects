using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class WheelCrosshair : MonoBehaviour
{
    private Image crosshair;

    private void Awake()
    {
        crosshair = GetComponent<Image>();
        crosshair.raycastTarget = false;
    }

    private void LateUpdate()
    {
        crosshair.enabled =
            Cursor.lockState == CursorLockMode.Locked &&
            Time.timeScale > 0f;
    }
}