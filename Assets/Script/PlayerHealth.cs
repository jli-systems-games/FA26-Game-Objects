using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public GameObject[] hearts;
    public GetHurt getHurt;
    public float hitCooldown = 1f;

    private int health;
    private float nextHitTime;

    void Start()
    {
        Time.timeScale = 1f;
        health = hearts.Length;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy") && health > 0 && Time.time >= nextHitTime)
        {
            health--;
            hearts[health].SetActive(false);

            nextHitTime = Time.time + hitCooldown;

            if (getHurt != null)
            {
                getHurt.DisplayARandomHurtText();
            }

            if (health == 0)
            {
                StartCoroutine(GameOver());
            }
        }
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Over");
    }
}
