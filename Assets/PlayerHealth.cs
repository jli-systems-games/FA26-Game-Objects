using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 3;

    public GameObject gameOverUI;
    public MonoBehaviour playerMovement;

    private float damageCooldown = 1f;
    private float lastDamageTime = -1f;

    void Start()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }
    }

    public void TakeDamage(int damage)
    {
        if (health <= 0)
        {
            return;
        }

        if (Time.time - lastDamageTime < damageCooldown)
        {
            return;
        }

        lastDamageTime = Time.time;
        health -= damage;

        Debug.Log("Health: " + health);

        if (health <= 0)
        {
            GameOver();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }

    void GameOver()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        Debug.Log("Game Over!");
    }
}
