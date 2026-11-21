using UnityEngine;

public class camandplayermove : MonoBehaviour
{
    public Transform playerCamera;
    public Rigidbody rb;
    public float speed = 5f;

    private float horizontal;
    private float vertical;

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        Vector3 forward = playerCamera.forward;
        Vector3 right = playerCamera.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection =
            forward * vertical +
            right * horizontal;

        rb.MovePosition(
            rb.position +
            moveDirection.normalized * speed * Time.fixedDeltaTime
        );
    }
}
