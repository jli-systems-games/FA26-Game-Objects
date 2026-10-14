using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    public float xSpeed = -3f;
    public float ySpeed = 0f;

    [SerializeField] float minimumSpeed = 1.8f;
    [SerializeField] float maximumSpeed = 4.2f;

    HurtSound hurtSound;
    Camera mainCamera;
    Vector3 baseScale;
    SpriteRenderer spriteRenderer;
    Color originalColor;
    float pulsePhase;

    void Awake()
    {
        baseScale = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer != null ? spriteRenderer.color : Color.white;
        pulsePhase = Random.Range(0f, Mathf.PI * 2f);
    }

    void Start()
    {
        mainCamera = Camera.main;
        hurtSound = FindFirstObjectByType<HurtSound>();
    }

    void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsPlaying)
            return;

        transform.position += new Vector3(xSpeed, ySpeed, 0f) * Time.deltaTime;

        float pulse = 1f + Mathf.Sin(Time.time * 4.5f + pulsePhase) * 0.08f;
        transform.localScale = baseScale * pulse;
        if (spriteRenderer != null)
        {
            float glow = (Mathf.Sin(Time.time * 3f + pulsePhase) + 1f) * 0.5f;
            spriteRenderer.color = Color.Lerp(originalColor, new Color(1f, 0.25f, 0.72f, 1f), glow * 0.32f);
        }

        if (mainCamera != null && mainCamera.WorldToViewportPoint(transform.position).x < -0.08f)
            ResetEnemy(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player == null)
            return;

        if (player.TakeDamage(1))
        {
            if (hurtSound == null)
                hurtSound = FindFirstObjectByType<HurtSound>();

            if (hurtSound != null)
                hurtSound.DisplayARandomHurtText();

            ResetEnemy(false);
        }
    }

    public void ResetEnemy(bool spreadAcrossScreen)
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera == null)
            return;

        float viewportX = spreadAcrossScreen ? Random.Range(1.05f, 4.2f) : Random.Range(1.05f, 1.8f);
        float viewportY = Random.Range(0.12f, 0.88f);
        Vector3 newPosition = mainCamera.ViewportToWorldPoint(new Vector3(viewportX, viewportY, 0f));
        newPosition.z = transform.position.z;
        transform.position = newPosition;

        xSpeed = -Random.Range(minimumSpeed, maximumSpeed);
        ySpeed = Random.Range(-0.25f, 0.25f);
    }
}
