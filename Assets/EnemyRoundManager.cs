
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyRoundManager : MonoBehaviour
{
    public string winSceneName = "WinScene";

    private int remainingEnemies;
    private Health playerHealth;

    void Start()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        remainingEnemies = enemies.Length;

        playerHealth = GetComponent<Health>();
    }

    public void EnemyFinished()
    {
        if (remainingEnemies <= 0) return;

        remainingEnemies--;

        if (remainingEnemies == 0)
        {
            
            if (playerHealth != null && playerHealth.IsAlive)
            {
                SceneManager.LoadScene(winSceneName);
            }
        }
    }
}