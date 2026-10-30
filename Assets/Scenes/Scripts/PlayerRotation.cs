using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    void Update()
    {
        // Mouse position in screen space -> world space
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Direction from this object to the mouse
        Vector2 direction = new Vector2(
            mousePos.x - transform.position.x,
            mousePos.y - transform.position.y
        );

        // Point the object's up axis along that direction
        transform.up = direction;
    }
}

