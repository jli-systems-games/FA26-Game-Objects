using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject startScreen;
    public GameObject pauseMenu;
    public GameObject crosshair;

    [Header("Player")]
    public PlayerController playerController;
    public PlayerInteraction playerInteraction;

    private bool gameStarted = false;
    private bool isPaused = false;

    void Start()
    {
        startScreen.SetActive(true);
        pauseMenu.SetActive(false);
        crosshair.SetActive(false);

        // 开始前禁用玩家控制
        playerController.enabled = false;
        playerInteraction.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (!gameStarted) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void StartGame()
    {
        gameStarted = true;
        startScreen.SetActive(false);
        crosshair.SetActive(true);

        playerController.enabled = true;
        playerInteraction.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        crosshair.SetActive(false);

        playerController.enabled = false;
        playerInteraction.enabled = false;

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        crosshair.SetActive(true);

        playerController.enabled = true;
        playerInteraction.enabled = true;

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}