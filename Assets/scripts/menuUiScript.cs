using UnityEngine;

public class menuUiScript : MonoBehaviour
{
    public GameObject startImage;
    public GameObject controlsImage;
    public GameObject fullscreenImage;
    public GameObject controlsPanel;
    public GameObject menuScreen;
    public GameObject[] hideOnControls;
    public characterScript player;

    int selected = 1;

    void Start()
    {
        controlsPanel.SetActive(false);
        Time.timeScale = 0f;
        player.enabled = false;
        Cursor.visible = false;
        ShowSelected();
    }

    void Update()
    {
        if (controlsPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                controlsPanel.SetActive(false);
                SetMenuVisible(true); 
            }
            return;
        }

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

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (selected == 1)
            {
                Time.timeScale = 1f;
                player.enabled = true;
                Cursor.lockState = CursorLockMode.Locked;  
                Cursor.visible = false;                     
                menuScreen.SetActive(false);
                enabled = false;
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
        startImage.SetActive(selected == 1);
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

