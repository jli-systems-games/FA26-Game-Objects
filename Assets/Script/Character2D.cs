
using UnityEngine;
using UnityEngine.InputSystem;

public class Character2D : MonoBehaviour
{
    public bool horizontal, vertical;
    public float speed = 5f, jumpSpeed = 4f;
    public float jumpDuration = 0.3f;
    public Vector2 gravity = new Vector2(0, -3);

    private float jumpTime;

    void Update()
    {
        if (Time.timeScale == 0) return;

        float x = gravity.x, y = gravity.y;

        if (horizontal)
        {
            if (Keyboard.current.aKey.isPressed) x -= speed;
            if (Keyboard.current.dKey.isPressed) x += speed;
        }

        if (vertical)
        {
            if (Keyboard.current.wKey.isPressed) y += speed;
            if (Keyboard.current.sKey.isPressed) y -= speed;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            jumpTime = jumpDuration;

        if (jumpTime > 0)
        {
            y = jumpSpeed;
            jumpTime -= Time.deltaTime;
        }

        transform.position += new Vector3(x, y, 0) * Time.deltaTime;
    }
}
