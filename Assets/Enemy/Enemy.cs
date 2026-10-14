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
    Vector3 originalScale;
    Vector3 baseScale;
    SpriteRenderer spriteRenderer;
    Color variantColor;
    Color glowColor;
    float pulsePhase;
    float activationDelay;
    float pulseSpeed = 4.5f;
    float pulseAmount = 0.08f;
    float angularSpeed;
    float speedMultiplier = 1f;

    void Awake()
    {
        originalScale = transform.localScale;
        baseScale = originalScale;
        spriteRenderer = GetComponent<SpriteRenderer>();
        variantColor = spriteRenderer != null ? spriteRenderer.color : Color.white;
        glowColor = new Color(1f, 0.25f, 0.72f, 1f);
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

        if (activationDelay > 0f)
        {
            activationDelay -= Time.deltaTime;
            return;
        }

        transform.position += new Vector3(xSpeed, ySpeed, 0f) * Time.deltaTime;

        transform.Rotate(0f, 0f, angularSpeed * Time.deltaTime);
        float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed + pulsePhase) * pulseAmount;
        transform.localScale = baseScale * pulse;
        if (spriteRenderer != null)
        {
            float glow = (Mathf.Sin(Time.time * 3f + pulsePhase) + 1f) * 0.5f;
            spriteRenderer.color = Color.Lerp(variantColor, glowColor, glow * 0.38f);
        }

        if (mainCamera != null)
        {
            Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);
            if (viewportPosition.x < -0.22f || viewportPosition.x > 1.22f ||
                viewportPosition.y < -0.22f || viewportPosition.y > 1.22f)
                ResetEnemy(false);
        }
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

        int edge = Random.Range(0, 4);
        Vector2 spawnViewport;
        Vector2 targetViewport;
        float edgeOffset = Random.Range(0.07f, 0.15f);

        if (edge == 0)
        {
            spawnViewport = new Vector2(-edgeOffset, Random.Range(0.08f, 0.92f));
            targetViewport = new Vector2(1.08f, Random.Range(0.08f, 0.92f));
        }
        else if (edge == 1)
        {
            spawnViewport = new Vector2(1f + edgeOffset, Random.Range(0.08f, 0.92f));
            targetViewport = new Vector2(-0.08f, Random.Range(0.08f, 0.92f));
        }
        else if (edge == 2)
        {
            spawnViewport = new Vector2(Random.Range(0.08f, 0.92f), -edgeOffset);
            targetViewport = new Vector2(Random.Range(0.08f, 0.92f), 1.08f);
        }
        else
        {
            spawnViewport = new Vector2(Random.Range(0.08f, 0.92f), 1f + edgeOffset);
            targetViewport = new Vector2(Random.Range(0.08f, 0.92f), -0.08f);
        }

        Vector3 newPosition = mainCamera.ViewportToWorldPoint(new Vector3(spawnViewport.x, spawnViewport.y, 0f));
        newPosition.z = transform.position.z;
        transform.position = newPosition;

        Vector3 targetPosition = mainCamera.ViewportToWorldPoint(new Vector3(targetViewport.x, targetViewport.y, 0f));
        Vector2 direction = ((Vector2)(targetPosition - newPosition)).normalized;
        ConfigureVariant();
        float speed = Random.Range(minimumSpeed, maximumSpeed) * speedMultiplier;
        xSpeed = direction.x * speed;
        ySpeed = direction.y * speed;
        activationDelay = spreadAcrossScreen ? Random.Range(0f, 3.2f) : Random.Range(0.15f, 0.85f);
    }

    void ConfigureVariant()
    {
        int variant = Random.Range(0, 3);

        if (variant == 0)
        {
            // Fast, narrow signal shard.
            baseScale = Vector3.Scale(originalScale, new Vector3(0.82f, 0.48f, 1f));
            variantColor = Color.white;
            glowColor = new Color(0.25f, 1f, 0.95f, 1f);
            speedMultiplier = 1.28f;
            angularSpeed = 0f;
            pulseSpeed = 8f;
            pulseAmount = 0.05f;
        }
        else if (variant == 1)
        {
            // Large, slow pressure wave.
            baseScale = originalScale * 1.42f;
            variantColor = new Color(1f, 0.72f, 0.92f, 1f);
            glowColor = new Color(1f, 0.18f, 0.62f, 1f);
            speedMultiplier = 0.68f;
            angularSpeed = 18f;
            pulseSpeed = 2.8f;
            pulseAmount = 0.14f;
        }
        else
        {
            // Tall rotating fragment.
            baseScale = Vector3.Scale(originalScale, new Vector3(0.62f, 1.18f, 1f));
            variantColor = new Color(0.86f, 0.76f, 1f, 1f);
            glowColor = new Color(0.62f, 0.28f, 1f, 1f);
            speedMultiplier = 0.96f;
            angularSpeed = Random.Range(-150f, 150f);
            pulseSpeed = 5.2f;
            pulseAmount = 0.09f;
        }

        transform.localScale = baseScale;
        if (spriteRenderer != null)
            spriteRenderer.color = variantColor;
    }
}
