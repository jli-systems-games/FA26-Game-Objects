using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauPanel;
    public PlayerController playerController;

    public static bool ReturnedToMenu;
    private bool paused;
    private bool wasMoving;
    private CursorLockMode previousLock;
    private bool previousCursorVisible;

    void Start()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        PauPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        paused = true;
        wasMoving = playerController.enabled;

        previousLock = Cursor.lockState;
        previousCursorVisible = Cursor.visible;

        playerController.enabled = false;
        PauPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
        AudioListener.pause = true;
    }

    public void Resume()
    {
        paused = false;

        Time.timeScale = 1f;
        AudioListener.pause = false;

        PauPanel.SetActive(false);
        playerController.enabled = wasMoving;

        Cursor.lockState = previousLock;
        Cursor.visible = previousCursorVisible;
    }

    public void BackToMenu()
    {
        ReturnedToMenu = true;

        Time.timeScale = 1f;
        AudioListener.pause = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("Menu");
    }
}