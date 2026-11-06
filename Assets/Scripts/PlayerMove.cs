using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 150f;

    float cameraRotationX = 0f;
    public Transform playerCamera;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Move with WASD
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 movement = transform.right * x + transform.forward * z;
        transform.position += movement.normalized * speed * Time.deltaTime;

        // Rotate player left/right with mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        // Rotate camera up/down with mouse
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        cameraRotationX -= mouseY;
        cameraRotationX = Mathf.Clamp(cameraRotationX, -80f, 80f);

        playerCamera.localRotation = Quaternion.Euler(cameraRotationX, 0f, 0f);
    }
}