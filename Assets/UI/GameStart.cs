using UnityEngine;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    public Button startButton;

    void Start()
    {
        Time.timeScale = 0;  
        startButton.onClick.AddListener(BeginGame);
    }

    void BeginGame()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}