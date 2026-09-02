using UnityEngine;
using UnityEngine.InputSystem;

public class cameramove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 0.1f;

    private float pitch = 0f;
    private float yaw = 0f;

    void Start()
    {
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // ---------- Keyboard movement ----------
        Vector3 movement = Vector3.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed)
                movement += transform.forward;

            if (Keyboard.current.sKey.isPressed)
                movement -= transform.forward;

            if (Keyboard.current.aKey.isPressed)
                movement -= transform.right;

            if (Keyboard.current.dKey.isPressed)
                movement += transform.right;

            if (Keyboard.current.eKey.isPressed)
                movement += Vector3.up;

            if (Keyboard.current.qKey.isPressed)
                movement -= Vector3.up;

            // Escape releases mouse
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        transform.position += movement.normalized * moveSpeed * Time.deltaTime;


        // ---------- Mouse rotation ----------
        if (Mouse.current != null &&
            Cursor.lockState == CursorLockMode.Locked)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            yaw += mouseDelta.x * mouseSensitivity;
            pitch -= mouseDelta.y * mouseSensitivity;

            pitch = Mathf.Clamp(pitch, -90f, 90f);

            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }

        // Click to lock mouse again
        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}