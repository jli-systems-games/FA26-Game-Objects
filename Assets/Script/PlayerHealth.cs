using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public GameObject[] hearts; 
    public GetHurt getHurt; // Reference to the GetHurt script
    private int health;

    void Start()
    {
        Time.timeScale = 1f;
        health = hearts.Length;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy") && health > 0) // Check if there's health remaining
        {
            health--;
            hearts[health].SetActive(false);

            if (getHurt != null)
            {
                getHurt.RandomDisplay(); //display a random hurt text & sound
            }

            if (health == 0)
            {
                StartCoroutine(GameOver());
            }
        }
    }

    IEnumerator GameOver()
    {
        SceneManager.LoadScene("Over");
        yield return null; // Wait for the scene to load before continuing
    }
}

