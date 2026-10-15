using UnityEngine;

public class ArrowPointer : MonoBehaviour
{
    public Transform player;
    public Transform goal;

    void Update()
    {
        if (player != null && goal != null) // Both player and goal must be assigned
        {
            Vector2 direction = goal.position - player.position; // Calculate the direction from the player to the goal

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Calculate the angle in degrees

            transform.localRotation = Quaternion.Euler(0, 0, angle - 90f); // Rotate the arrow to point towards the goal
        }
    }
}

