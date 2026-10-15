
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public Image[] hearts;
    public string gameOverSceneNmae = "Gameover";

    private int currentHealth = 5;

    public void TakeDamage()
    {
        if (currentHealth <= 0) return;

        currentHealth--;

        hearts[currentHealth].enabled = false;

        if (currentHealth <= 0)
        {
            SceneManager.LoadScene(gameOverSceneNmae);
        }
    }
}