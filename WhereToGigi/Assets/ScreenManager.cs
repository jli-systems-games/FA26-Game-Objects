

using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public GameObject startScreen;
    public GameObject doorsScreen;
    public GameObject moonScreen;
    public GameObject oceanScreen;

    void Start()
    {
        ShowStart();
    }

    public void ShowStart()
    {
        ShowOnly(startScreen);
    }

    public void ShowDoors()
    {
        ShowOnly(doorsScreen);
    }

    public void ShowMoon()
    {
        ShowOnly(moonScreen);
    }

    public void ShowOcean()
    {
        ShowOnly(oceanScreen);
    }

    public void BackToDoors()
    {
        ShowOnly(doorsScreen);
    }

    void ShowOnly(GameObject screen)
    {
        startScreen.SetActive(false);
        doorsScreen.SetActive(false);
        moonScreen.SetActive(false);
        oceanScreen.SetActive(false);

        screen.SetActive(true);
    }
}
