
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TMP_Text timerText;
    public TMP_Text scoreText;
    public TMP_Text messageText;

    public int totalCoins = 10;
    public float timeLeft = 60f;

    private int coinsCollected = 0;
    private bool gameEnded = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        messageText.text = "";
        UpdateScore();
    }

    void Update()
    {
        if (gameEnded)
        {
            return;
        }

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0)
{
        timeLeft = 0;
        timerText.text = "Time's Up!";
        EndGame(false);
}

        timerText.text = "Time: " + Mathf.CeilToInt(timeLeft);
    }

    public void AddCoin()
    {
        if (gameEnded)
        {
            return;
        }

        coinsCollected++;
        UpdateScore();

        if (coinsCollected >= totalCoins)
        {
            EndGame(true);
        }
    }

    void UpdateScore()
    {
        scoreText.text = "Coins: " + coinsCollected + " / " + totalCoins;
    }

    void EndGame(bool won)
    {
        gameEnded = true;

        if (won)
        {
            messageText.text = "YOU WIN!";
        }
        else
        {
            messageText.text = "YOU LOSE!";
        }

        Invoke("RestartGame", 2f);
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
