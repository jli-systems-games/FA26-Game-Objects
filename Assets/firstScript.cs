using UnityEngine;

public class SimpleMovement3D : MonoBehaviour
{
    public float moveSpeed = 5f; 

    public float rotationSpeed = 720f; 

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(moveX, 0f, moveZ).normalized;

        if (moveDirection != Vector3.zero)
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
