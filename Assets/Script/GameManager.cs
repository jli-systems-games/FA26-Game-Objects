using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int count = 0; 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Make sure the GameManager persists across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate GameManager instances
        }
    }

    public void AddPoint()
    {
        count++; // Increment the score by 1
    }

    public void ResetScore()
    {
        count = 0; 
    }
}
