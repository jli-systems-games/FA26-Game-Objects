using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MuseumMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject descriptionPanel;

    private MonoBehaviour[] playerScripts;
    private bool[] previousScriptStates;

    private bool menuOpen;
    private bool resuming;
    private float previousTimeScale;

    private void Start()
    {
        ShowMenu();
    }

    private void Update()
    {
        if (menuOpen || resuming)
            return;

        // Finish reading before opening the menu.
        if (descriptionPanel != null &&
            descriptionPanel.activeInHierarchy)
            return;

        if (Keyboard.current != null &&
            Keyboard.current.mKey.wasPressedThisFrame)
        {
            ShowMenu();
        }
    }

    public void ShowMenu()
    {
        if (menuOpen || menuPanel == null || player == null)
            return;

        menuOpen = true;

        // Remember which player scripts were enabled.
        playerScripts = player.GetComponents<MonoBehaviour>();
        previousScriptStates = new bool[playerScripts.Length];

        for (int i = 0; i < playerScripts.Length; i++)
        {
            if (playerScripts[i] == null || playerScripts[i] == this)
                continue;

            previousScriptStates[i] = playerScripts[i].enabled;
            playerScripts[i].enabled = false;
        }

        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        menuPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StartGame()
    {
        if (!menuOpen)
            return;

        menuOpen = false;
        resuming = true;

        menuPanel.SetActive(false);
        Time.timeScale = previousTimeScale;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        StartCoroutine(RestorePlayerScripts());
    }

    private IEnumerator RestorePlayerScripts()
    {
        // Prevent the menu-button click from also clicking an exhibit.
        yield return null;

        for (int i = 0; i < playerScripts.Length; i++)
        {
            if (playerScripts[i] == null || playerScripts[i] == this)
                continue;

            playerScripts[i].enabled = previousScriptStates[i];
        }

        resuming = false;
    }
}