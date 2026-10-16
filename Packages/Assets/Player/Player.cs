using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public InputActionReference moveAction;

    public int health = 3;
    public GameObject[] hearts;

    public GameObject gameOverText;
    public GameObject restartButton;

    public AudioClip hurtClip;

    private Vector2 movement;

    void OnEnable()
    {
        if (moveAction != null)
        {
            moveAction.action.Enable();
        }
    }

    void OnDisable()
    {
        if (moveAction != null)
        {
            moveAction.action.Disable();
        }
    }

    void Start()
    {
        UpdateHearts();

        gameOverText.SetActive(false);
        restartButton.SetActive(false);
    }

    void Update()
    {
        if (moveAction == null)
            return;

        movement = moveAction.action.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        Vector3 direction = new Vector3(movement.x, movement.y, 0);

        transform.position += direction * speed * Time.fixedDeltaTime;
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < health);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && health > 0)
        {
            health--;

            Debug.Log("Player hit! Health remaining: " + health);

            if (hurtClip != null)
            {
                AudioSource.PlayClipAtPoint(
                    hurtClip,
                    transform.position
                );
            }

            UpdateHearts();

            if (health <= 0)
            {
                Debug.Log("Game Over!");

                gameOverText.SetActive(true);
                restartButton.SetActive(true);

                gameObject.SetActive(false);
            }
        }
    }
}
