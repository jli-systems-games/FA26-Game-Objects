using UnityEngine;
using TMPro;

public class HurtSound : MonoBehaviour
{
    private TextMeshProUGUI text;

    public string[] hurtSounds;

    public int health = 3;
    public TextMeshProUGUI healthText;

    public float timeToWin = 10f;
    public TextMeshProUGUI timerText;

    public GameObject startPanel;
    public GameObject gameOverPanel;
    public GameObject winPanel;

    private float timeRemaining;
    private bool gameEnded = false;

    public float damageCooldown = 1f;
    private float nextDamageTime = 0f;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();

        timeRemaining = timeToWin;

        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);
        startPanel.SetActive(true);

        UpdateHealthText();
        UpdateTimerText();

        Time.timeScale = 0f;
    }

    void Update()
    {
        if (gameEnded == true)
        {
            return;
        }

        timeRemaining = timeRemaining - Time.deltaTime;

        if (timeRemaining < 0)
        {
            timeRemaining = 0;
        }

        UpdateTimerText();

        if (timeRemaining <= 0)
        {
            WinGame();
        }
    }

    public void StartGame()
    {
        startPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void PlayerHurt()
    {
        if (gameEnded == true)
        {
            return;
        }

        if (Time.time < nextDamageTime)
        {
            return;
        }

        nextDamageTime = Time.time + damageCooldown;

        DisplayARandomHurtText();

        health = health - 1;

        UpdateHealthText();

        if (health <= 0)
        {
            GameOver();
        }
    }

    public void DisplayARandomHurtText()
    {
        if (hurtSounds.Length > 0)
        {
            string randomHurtSound =
                hurtSounds[Random.Range(0, hurtSounds.Length)];

            text.text = randomHurtSound;
        }
    }

    void UpdateHealthText()
    {
        healthText.text = "Health: " + health;
    }

    void UpdateTimerText()
    {
        timerText.text =
            "Time: " + Mathf.CeilToInt(timeRemaining);
    }

    void GameOver()
    {
        gameEnded = true;

        gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    void WinGame()
    {
        gameEnded = true;

        winPanel.SetActive(true);

        Time.timeScale = 0f;
    }
}