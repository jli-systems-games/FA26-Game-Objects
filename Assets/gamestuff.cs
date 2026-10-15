using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject gameOverPanel;
    public GameObject youWinPanel;

    public float timeToWin = 15f;
    private float timer = 0f;

    void Start()
    {
        Time.timeScale = 0f;

        startPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        youWinPanel.SetActive(false);
    }

    void Update()
    {
        if (Time.timeScale == 0f)
            return;

        timer += Time.deltaTime;

        if (timer >= timeToWin)
        {
            YouWin();
        }
    }

    public void StartGame()
    {
        timer = 0f;

        startPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        youWinPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
    }

    public void YouWin()
    {
        if (Time.timeScale == 0f)
            return;

        Time.timeScale = 0f;
        youWinPanel.SetActive(true);
    }

    public void TryAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}