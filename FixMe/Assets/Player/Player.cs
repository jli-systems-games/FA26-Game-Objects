
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public int health = 10;
    public TMP_Text healthText;
    public GameObject gameOverPanel;

    private AudioSource hurtSound;

    void Start()
    {
        hurtSound = GetComponent<AudioSource>();
        UpdateHealth();
        gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            transform.position += Vector3.up * speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            transform.position += Vector3.down * speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            transform.position += Vector3.left * speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            transform.position += Vector3.right * speed * Time.deltaTime;
    }

    public void TakeDamage()
    {
        health--;
        UpdateHealth();

        if (hurtSound != null)
            hurtSound.Play();

        if (health <= 0)
        {
            gameOverPanel.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    void UpdateHealth()
    {
        if (healthText != null)
            healthText.text = "Health: " + health;
    }
}
