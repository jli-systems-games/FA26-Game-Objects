using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    public float moveSpeed = 3f;

    void Update()
    {
        float horizontal =
            Input.GetAxisRaw("Horizontal");

        float vertical =
            Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(
            horizontal,
            0f,
            vertical
        ).normalized;

        transform.position +=
            direction * moveSpeed * Time.deltaTime;
    }
}