using UnityEngine;
using UnityEngine.InputSystem;

public class GalleryExplorerController : MonoBehaviour
{
    public float moveSpeed = 3.2f;
    public float lookSensitivity = .12f;
    public float gravity = -18f;

    private CharacterController character;
    private float yaw;
    private float pitch;
    private float verticalSpeed;

    private void Start()
    {
        character = GetComponent<CharacterController>();
        if (character == null) character = gameObject.AddComponent<CharacterController>();
        character.height = 1.7f;
        character.radius = .28f;
        character.center = new Vector3(0, -.84f, 0);
        yaw = transform.eulerAngles.y;
    }

    private void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null || character == null) return;

        var forward = transform.forward;
        forward.y = 0;
        forward.Normalize();
        var right = transform.right;
        right.y = 0;
        right.Normalize();
        Vector3 move = Vector3.zero;
        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) move += forward;
        if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) move -= forward;
        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) move += right;
        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) move -= right;
        if (move.sqrMagnitude > 1) move.Normalize();

        if (character.isGrounded && verticalSpeed < 0) verticalSpeed = -1f;
        verticalSpeed += gravity * Time.deltaTime;
        character.Move((move * moveSpeed + Vector3.up * verticalSpeed) * Time.deltaTime);

        var mouse = Mouse.current;
        if (mouse != null && mouse.rightButton.isPressed)
        {
            Vector2 delta = mouse.delta.ReadValue() * lookSensitivity;
            yaw += delta.x;
            pitch = Mathf.Clamp(pitch - delta.y, -78f, 78f);
            transform.rotation = Quaternion.Euler(pitch, yaw, 0);
        }
    }
}
