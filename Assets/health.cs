
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public Image[] hearts;
    public string gameOverSceneName = "Gameover";

    private int currentHealth = 5;

    public bool IsAlive
    {
        get { return currentHealth > 0; }
    }

    public void TakeDamage()
    {
        if (currentHealth <= 0) return;

        currentHealth--;

        hearts[currentHealth].enabled = false;

        if (currentHealth <= 0)
        {
            SceneManager.LoadScene(gameOverSceneName);
        }
    }
}