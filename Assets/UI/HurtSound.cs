using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HurtSound : MonoBehaviour
{
    private TextMeshProUGUI text;

    public string[] hurtSounds;

    // HP
    public int health = 3;
    public TextMeshProUGUI healthText;

    // Timer
    public float timeToWin = 10f;
    public TextMeshProUGUI timerText;

    // Panels
    public GameObject startPanel;
    public GameObject gameOverPanel;
    public GameObject winPanel;

    // Audio
    public AudioSource audioSource; 
    public AudioClip hurtSoundEffect;
    public AudioClip winSoundEffect;
    public AudioClip loseSoundEffect;

    // Timer
    private float timeRemaining;
    private bool gameEnded = false;

    // Damage cooldown
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

        // Pause the game at the start
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
    
    public void StartLevel1()
    {
        StartLevel(10f);
    }

    public void StartLevel2()
    {
        StartLevel(20f);
    }
    
    public void StartLevel3()
    {
        StartLevel(30f);
    }
    void StartLevel(float levelTime)
    {
        timeToWin = levelTime;
        timeRemaining = timeToWin;
        
        UpdateTimerText();
        
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
        else
        {
            PlaySound(hurtSoundEffect);
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

        PlaySound(loseSoundEffect);

        Time.timeScale = 0f;
    }

    void WinGame()
    {
        gameEnded = true;

        PlaySound(winSoundEffect);

        winPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    void PlaySound(AudioClip sound)
    {
        if (audioSource != null && sound != null)
        {
            audioSource.PlayOneShot(sound);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}