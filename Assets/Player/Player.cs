using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed = 6.5f;
    [SerializeField] float boostMultiplier = 1.7f;
    [SerializeField] float boostDuration = 3f;

    [Header("Health")]
    [SerializeField] int maxHealth = 3;
    [SerializeField] float invincibilityTime = 0.9f;

    public int CurrentHealth { get; private set; }
    public int MaxHealth => maxHealth;
    public float BoostTimeRemaining { get; private set; }

    Vector3 startingPosition;
    SpriteRenderer spriteRenderer;
    Color normalColor;
    bool isInvincible;
    TrailRenderer lightTrail;

    void Awake()
    {
        startingPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        normalColor = spriteRenderer != null ? spriteRenderer.color : Color.white;
        CurrentHealth = maxHealth;
        CreateLightTrail();
    }

    void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsPlaying)
            return;

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input = Vector2.ClampMagnitude(input, 1f);
        float currentSpeed = BoostTimeRemaining > 0f ? speed * boostMultiplier : speed;
        transform.position += (Vector3)(input * currentSpeed * Time.deltaTime);

        if (BoostTimeRemaining > 0f)
            BoostTimeRemaining = Mathf.Max(0f, BoostTimeRemaining - Time.deltaTime);

        if (lightTrail != null)
            lightTrail.startWidth = BoostTimeRemaining > 0f ? 0.52f : 0.28f;
        KeepInsideCamera();
    }

    public float ActivateSpeedBoost()
    {
        BoostTimeRemaining = boostDuration;
        return boostDuration;
    }

    void KeepInsideCamera()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
            return;

        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);
        viewportPosition.x = Mathf.Clamp(viewportPosition.x, 0.035f, 0.965f);
        viewportPosition.y = Mathf.Clamp(viewportPosition.y, 0.06f, 0.94f);
        Vector3 clampedPosition = mainCamera.ViewportToWorldPoint(viewportPosition);
        clampedPosition.z = transform.position.z;
        transform.position = clampedPosition;
    }

    public bool TakeDamage(int amount)
    {
        if (amount <= 0 || isInvincible || CurrentHealth <= 0 ||
            GameManager.Instance == null || !GameManager.Instance.IsPlaying)
            return false;

        CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
        GameManager.Instance.OnPlayerDamaged(CurrentHealth, maxHealth);

        if (CurrentHealth <= 0)
            GameManager.Instance.EndGame(false);
        else
            StartCoroutine(HurtFlash());

        return true;
    }

    IEnumerator HurtFlash()
    {
        isInvincible = true;
        float elapsed = 0f;

        while (elapsed < invincibilityTime)
        {
            if (spriteRenderer != null)
                spriteRenderer.color = spriteRenderer.color == normalColor
                    ? new Color(1f, 0.25f, 0.25f, 0.45f)
                    : normalColor;

            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }

        if (spriteRenderer != null)
            spriteRenderer.color = normalColor;

        isInvincible = false;
    }

    public void ResetPlayer()
    {
        StopAllCoroutines();
        transform.position = startingPosition;
        CurrentHealth = maxHealth;
        isInvincible = false;
        BoostTimeRemaining = 0f;

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
            spriteRenderer.color = normalColor;
        }

        if (lightTrail != null)
            lightTrail.Clear();
    }

    void CreateLightTrail()
    {
        Shader shader = Shader.Find("Sprites/Default");
        if (shader == null)
            return;

        lightTrail = gameObject.AddComponent<TrailRenderer>();
        lightTrail.material = new Material(shader);
        lightTrail.time = 0.42f;
        lightTrail.startWidth = 0.28f;
        lightTrail.endWidth = 0f;
        lightTrail.minVertexDistance = 0.03f;
        lightTrail.sortingOrder = -1;

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new[]
            {
                new GradientColorKey(new Color(0.2f, 1f, 0.95f), 0f),
                new GradientColorKey(new Color(1f, 0.15f, 0.72f), 1f)
            },
            new[]
            {
                new GradientAlphaKey(0.9f, 0f),
                new GradientAlphaKey(0f, 1f)
            });
        lightTrail.colorGradient = gradient;
    }
}
