using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int hits = 0;
    public int maxHits = 5;

    public void TakeDamage()
    {
        hits++;

        Debug.Log("Player hit! Hits taken: " + hits);

        if (hits >= maxHits)
        {
            FindFirstObjectByType<GameManager>().GameOver();
        }
    }
}