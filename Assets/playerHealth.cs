using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int hits = 0;
    public int maxHits = 5;

    public void TakeDamage()
    {
        hits++;

        if (hits >= maxHits)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}