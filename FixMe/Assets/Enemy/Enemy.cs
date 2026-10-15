
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float xSpeed;
    public float ySpeed;

    void Update()
    {
        transform.position += new Vector3(xSpeed, ySpeed, 0) * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Player player = col.GetComponent<Player>();

        if (player != null)
        {
            player.TakeDamage();
        }
    }
}
