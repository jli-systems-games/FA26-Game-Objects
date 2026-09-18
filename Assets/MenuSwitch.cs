using UnityEngine;

public class MenuSwitch : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject mainMenuPanel;
    public GameObject randomizerPanel;

    public void OpenRandomizer()
    {
        if (mainMenuPanel != null && randomizerPanel != null)
        {
            mainMenuPanel.SetActive(false);
            randomizerPanel.SetActive(true);
        }
    }

    public void OpenMainMenu()
    {
        if (mainMenuPanel != null && randomizerPanel != null)
        {
            mainMenuPanel.SetActive(true);
            randomizerPanel.SetActive(false);
        }
    }
}
