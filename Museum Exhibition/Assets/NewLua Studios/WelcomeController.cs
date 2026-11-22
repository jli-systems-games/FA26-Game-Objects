using UnityEngine;

public class WelcomeController : MonoBehaviour
{
    public GameObject welcomePanel;

    public void OpenWelcome()
    {
        welcomePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseWelcome()
    {
        welcomePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}