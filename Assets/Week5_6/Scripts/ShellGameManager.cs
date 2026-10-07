using UnityEngine;

public class ShellGameManager : MonoBehaviour
{
    public static ShellGameManager instance;

    public GameObject completePanel;
    public int totalShells = 9;

    private int placedShells = 0;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        completePanel.SetActive(false);
    }

    public void ShellPlacedCorrectly()
    {
        placedShells++;

        if (placedShells >= totalShells)
        {
            completePanel.SetActive(true);
        }
    }
}