using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseUiScript : MonoBehaviour
{
    public GameObject pauseScreen;
    public GameObject firstImage;
    public GameObject controlsImage;
    public GameObject fullscreenImage;
    public GameObject controlsPanel;
    public GameObject[] hideOnControls;
    public characterScript player;
    public menuUiScript startMenu;

    int selected = 1;
    bool paused = false;

    void Start()
    {
        pauseScreen.SetActive(false);
        controlsPanel.SetActive(false);
    }

    void Update()
    {
        // don't do anything
        if (startMenu.enabled) return;

        // wait for p
        if (!paused)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                paused = true;
                selected = 1;
                Time.timeScale = 0f;
                player.enabled = false;
                pauseScreen.SetActive(true);
                ShowSelected();
            }
            return;
        }

        // closes control
        if (controlsPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                controlsPanel.SetActive(false);
                SetMenuVisible(true);
            }
            return;
        }

        // p closes it
        if (Input.GetKeyDown(KeyCode.P))
        {
            paused = false;
            Time.timeScale = 1f;
            player.enabled = true;
            pauseScreen.SetActive(false);
            return;
        }

        // move selection
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            selected++;
            if (selected > 3) selected = 1;
            ShowSelected();
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            selected--;
            if (selected < 1) selected = 3;
            ShowSelected();
        }

        // choose
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (selected == 1)
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            if (selected == 2)
            {
                controlsPanel.SetActive(true);
                SetMenuVisible(false);
            }
            if (selected == 3) Screen.fullScreen = !Screen.fullScreen;
        }
    }

    void ShowSelected()
    {
        firstImage.SetActive(selected == 1);
        controlsImage.SetActive(selected == 2);
        fullscreenImage.SetActive(selected == 3);
    }

    void SetMenuVisible(bool visible)
    {
        foreach (GameObject g in hideOnControls)
        {
            g.SetActive(visible);
        }
    }
}