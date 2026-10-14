using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;

    private Camera mainCamera;
    private Collider2D playerCollider;

    void Start()
    {
        mainCamera = Camera.main;
        playerCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        float horizontal = 0f;
        float vertical = 0f;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            vertical = 1f;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            vertical = -1f;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            horizontal = -1f;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            horizontal = 1f;
        }

        Vector3 movement = new Vector3(horizontal, vertical, 0);

        // 防止斜着走时速度变快
        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }

        transform.position += movement * speed * Time.deltaTime;

        KeepPlayerOnScreen();
    }

    void KeepPlayerOnScreen()
    {
        if (mainCamera == null)
        {
            return;
        }

        float distanceFromCamera =
            Mathf.Abs(transform.position.z - mainCamera.transform.position.z);

        Vector3 bottomLeft =
            mainCamera.ViewportToWorldPoint(
                new Vector3(0f, 0f, distanceFromCamera)
            );

        Vector3 topRight =
            mainCamera.ViewportToWorldPoint(
                new Vector3(1f, 1f, distanceFromCamera)
            );

        float halfWidth = 0f;
        float halfHeight = 0f;

        if (playerCollider != null)
        {
            halfWidth = playerCollider.bounds.extents.x;
            halfHeight = playerCollider.bounds.extents.y;
        }

        Vector3 position = transform.position;

        position.x = Mathf.Clamp(
            position.x,
            bottomLeft.x + halfWidth,
            topRight.x - halfWidth
        );

        position.y = Mathf.Clamp(
            position.y,
            bottomLeft.y + halfHeight,
            topRight.y - halfHeight
        );

        transform.position = position;
    }
}
