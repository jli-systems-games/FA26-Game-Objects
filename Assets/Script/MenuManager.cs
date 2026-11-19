using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject menuButton, menuPanel, controlsPanel, exitButton;
    public GameObject player, p1, p2, p3, p4, p5;

    public static bool menuOpen;
    private float oldTimeScale = 1f; //

    void Start()
    {
        menuOpen = false;
        menuButton.SetActive(false);
        menuPanel.SetActive(false);
        controlsPanel.SetActive(false);
    }

    void Update()
    {
        menuButton.SetActive(MainLogic.scene >= 0 && !menuOpen);
    }

    public void OpenMenu()
    {
        if (menuOpen || MainLogic.scene < 0) return;

        oldTimeScale = Time.timeScale;
        menuOpen = true;
        Time.timeScale = 0f;

        menuButton.SetActive(false);
        menuPanel.SetActive(true);
        controlsPanel.SetActive(false);
        exitButton.SetActive(MainLogic.scene > 0);
    }

    public void Resume()
    {
        menuPanel.SetActive(false);
        controlsPanel.SetActive(false);
        menuOpen = false;
        Time.timeScale = oldTimeScale;
    }

    public void ShowControls()
    {
        menuPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }

    public void Back()
    {
        controlsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void RestartGame()
    {
        MainLogic.key1 = false;
        MainLogic.key2 = false;
        MainLogic.key3 = false;
        MainLogic.key4 = false;
        MainLogic.key5 = false;
        MainLogic.hasWon = false;
        MainLogic.scene = -1;

        menuOpen = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitToMuseum()
    {
        GameObject point = null;

        if (MainLogic.scene == 1) point = p1;
        if (MainLogic.scene == 2) point = p2;
        if (MainLogic.scene == 3) point = p3;
        if (MainLogic.scene == 4) point = p4;
        if (MainLogic.scene == 5) point = p5;
        if (point == null) return;

        CharacterController cc = player.GetComponent<CharacterController>();
        cc.enabled = false;
        player.transform.position = point.transform.position;
        player.transform.rotation = point.transform.rotation;
        cc.enabled = true;

        MainLogic.scene = 0;
        oldTimeScale = 1f;
        Resume();
    }
}