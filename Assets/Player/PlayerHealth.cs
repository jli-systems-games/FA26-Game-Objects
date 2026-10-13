using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("UI Reference")]
    public Slider healthSlider;   // drag the Slider here
    public Image fillImage;       // optional: Fill child image for color

    [Header("Optional")]
    public Gradient healthGradient;

    public bool IsDead { get; private set; }

    void Start()
    {
        currentHealth = maxHealth;
        IsDead = false;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
        UpdateBar();
    }

    public void TakeDamage(float amount)
    {
        if (IsDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateBar();

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (IsDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateBar();
    }

    void UpdateBar()
    {
        if (healthSlider != null)
            healthSlider.value = currentHealth;

        if (fillImage != null && healthGradient != null)
            fillImage.color = healthGradient.Evaluate(currentHealth / maxHealth);
    }

    void Die()
    {
        IsDead = true;
        Debug.Log("Player died!");
        // death animation, disable movement, game over screen...
    }
    

    public float GetHealthPercent() => currentHealth / maxHealth;
}
