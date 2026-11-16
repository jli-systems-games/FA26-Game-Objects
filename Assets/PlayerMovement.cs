using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Transform playerCamera;
    public float moveSpeed = 4f;
    public float mouseSensitivity = 0.15f;

    private CharacterController controller;
    private float cameraTilt;
    private float verticalSpeed;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        LockCursor(true);
    }

    void Update()
    {
        Keyboard keyboard = Keyboard.current;
        Mouse mouse = Mouse.current;

        if (keyboard != null && keyboard.escapeKey.wasPressedThisFrame)
        {
            LockCursor(false);
        }
        else if (mouse != null && mouse.leftButton.wasPressedThisFrame)
        {
            LockCursor(true);
        }

        Vector2 movement = Vector2.zero;

        if (Cursor.lockState == CursorLockMode.Locked)
        {
  
            if (keyboard != null)
            {
                if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
                    movement.y += 1;

                if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
                    movement.y -= 1;

                if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
                    movement.x += 1;

                if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
                    movement.x -= 1;
            }

            if (mouse != null)
            {
                Vector2 look = mouse.delta.ReadValue() * mouseSensitivity;

                transform.Rotate(Vector3.up * look.x);

                cameraTilt -= look.y;
                cameraTilt = Mathf.Clamp(cameraTilt, -80f, 80f);

                playerCamera.localRotation =
                    Quaternion.Euler(cameraTilt, 0f, 0f);
            }
        }

        movement = Vector2.ClampMagnitude(movement, 1f);

        Vector3 velocity =
            (transform.right * movement.x +
             transform.forward * movement.y) * moveSpeed;

   
        if (controller.isGrounded && verticalSpeed < 0f)
        {
            verticalSpeed = -2f;
        }

        verticalSpeed += Physics.gravity.y * Time.deltaTime;
        velocity.y = verticalSpeed;

        controller.Move(velocity * Time.deltaTime);
    }

    void LockCursor(bool locked)
    {
        Cursor.lockState = locked
            ? CursorLockMode.Locked
            : CursorLockMode.None;

        Cursor.visible = !locked;
    }
}