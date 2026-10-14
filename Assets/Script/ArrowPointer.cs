using UnityEngine;

public class ArrowPointer : MonoBehaviour
{
    public Transform player;
    public Transform goal;

    public float height = 2f;

    void LateUpdate()
    {
        if (player != null && goal != null)
        {
            transform.position = player.position + new Vector3(0, height, 0);

            Vector2 direction = goal.position - player.position;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
    }
}
