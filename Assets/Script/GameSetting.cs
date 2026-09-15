using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSetting : MonoBehaviour
{
    public static GameSetting Instance;
    public int totalPins = 6;
    private int pinsHit = 0;
    private bool gameEnded = false;

// Make this script instance, can be accessible for any other script
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        pinsHit = 0;
        gameEnded = false;
    }

// When a pin is hit, the pinsHit counter add 1.
    public void HitPin()
    {
        if (gameEnded)
            return;

        pinsHit++;
    }

// If all the pins are hit, the player is won.
    public bool CheckWin()
    {
        return pinsHit >= totalPins;
    }

    public void WinGame()
    {
        gameEnded = true;
    }
// If the player loses, reset the game.
    public void ResetGame()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}